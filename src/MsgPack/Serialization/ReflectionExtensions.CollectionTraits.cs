// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;
#if FEATURE_MPCONTRACT
using Contract = MsgPack.MPContract;
#else
using System.Diagnostics.Contracts;
#endif // FEATURE_MPCONTRACT
using System.Linq;
using System.Reflection;

using MsgPack.Serialization.Reflection;

namespace MsgPack.Serialization
{
	internal partial class ReflectionExtensions
	{
		private static readonly Type[] WithCapacityConstructorParameterTypes = new[] { typeof(int) };
		private static readonly (ConstructorInfo? WithCapacity, ConstructorInfo? Default) ArrayListConstructors =
			(typeof(ArrayList).GetConstructor(WithCapacityConstructorParameterTypes),
			typeof(ArrayList).GetConstructor(ReflectionAbstractions.EmptyTypes));
		private static readonly (ConstructorInfo? WithCapacity, ConstructorInfo? Default) HashtableConstructors =
			(typeof(Hashtable).GetConstructor(WithCapacityConstructorParameterTypes),
			typeof(Hashtable).GetConstructor(ReflectionAbstractions.EmptyTypes));

		public static CollectionTraits GetCollectionTraits(this Type source, CollectionTraitOptions options, bool allowNonCollectionEnumerableTypes)
		{
#if DEBUG
			Contract.Assert(!source.GetContainsGenericParameters(), "!source.GetContainsGenericParameters()");
#endif // DEBUG
			/*
			 * SPEC
			 * 1. If the object has single public method TEnumerator GetEnumerator() (where TEnumerator implements IEnumerator<TItem>),
			 *    then the object is considered as the collection of TItem.
			 *   1-1. If the object is considered as the collection of TItem, TItem is KeyValuePair<TKey,TValue>, 
			 *        and the object implements IDictionary<TKey,TValue>,
			 *        then the object is considered as dictionary of TKey and TValue.
			 *   1-2. Else, if the object has single public method IEnumerator GetEnumerator(),
			 *        then the object is considered as the collection of Object.
			 *     1-2-1. If it also implements IDictionary,
			 *            then it is considered dictionary of Object and Object.
			 *     1-2-2. Otherwise, that means it implements multiple collection interface.
			 *       1-2-2-1. First, if the object implements IDictionary<MessagePackObject,MessagePackObject>, then it is considered as MPO dictionary.
			 *       1-2-2-2. Second, if the object implements IEnumerable<MPO>, then it is considered as MPO collection.
			 *       1-2-2-3. Third, if the object implement SINGLE IDictionary<TKey,TValue> and multiple IEnumerable<T>, then it is considered as dictionary of TKey and TValue.
			 *       1-2-2-4. Fourth, the object is considered as UNSERIALIZABLE member. This behavior similer to DataContract serialization behavor
			 *                (see http://msdn.microsoft.com/en-us/library/aa347850.aspx ).
			 */

			if (!source.IsAssignableTo(typeof(IEnumerable)))
			{
				return CollectionTraits.NotCollection;
			}

			if (source.IsArray)
			{
				return
					new CollectionTraits(
						CollectionDetailedKind.Array,
						constructorWithCapacity: null, // Never used for array.
						defaultConstructor: null, // Never used for array.
						source.GetElementType(),
						getEnumeratorMethod: null, // Never used for array.
						addMethod: null, // Never used for array.
						countPropertyGetter: null  // Never used for array.
					);
			}

			// If the type is an interface then a concrete collection has to be
			// made for it (if the interface is a collection type), therefore,
			// ignore the check for an add method
			if (!source.GetIsInterface() && allowNonCollectionEnumerableTypes)
			{
				options = options | CollectionTraitOptions.AllowNonCollectionEnumerableTypes;
			}

			var getEnumerator = source.GetMethod("GetEnumerator", ReflectionAbstractions.EmptyTypes);
			if (getEnumerator != null && getEnumerator.ReturnType.IsAssignableTo(typeof(IEnumerator)))
			{
				// If public 'GetEnumerator' is found, it is primary collection traits.
				CollectionTraits result;
				if (TryCreateCollectionTraitsForHasGetEnumeratorType(source, options, getEnumerator, out result))
				{
					return result;
				}
			}

			var genericTypes = new GenericCollectionTypes();
			Type? ienumerable = null;
			Type? icollection = null;
			Type? ilist = null;
			Type? idictionary = null;

			var sourceInterfaces = source.FindInterfaces(FilterCollectionType, null);
			if (source.GetIsInterface() && FilterCollectionType(source, null))
			{
				var originalSourceInterfaces = sourceInterfaces.ToArray();
				var concatenatedSourceInterface = new Type[originalSourceInterfaces.Length + 1];
				concatenatedSourceInterface[0] = source;
				for (int i = 0; i < originalSourceInterfaces.Length; i++)
				{
					concatenatedSourceInterface[i + 1] = originalSourceInterfaces[i];
				}

				sourceInterfaces = concatenatedSourceInterface;
			}

			foreach (var type in sourceInterfaces)
			{
				if (!DetermineCollectionInterfaces(
						type,
						ref genericTypes,
						ref idictionary,
						ref ilist,
						ref icollection,
						ref ienumerable
					)
				)
				{
					return CollectionTraits.Unserializable;
				}
			}

			if (genericTypes.IDictionaryT != null)
			{
				var genericArguments = genericTypes.IDictionaryT.GetGenericArguments();
				var elementType = typeof(KeyValuePair<,>).MakeGenericType(genericArguments);
				var constructor = GetGenericDictionaryConstructor(source, genericArguments[0], genericArguments[1], options);

				return
					new CollectionTraits(
						CollectionDetailedKind.GenericDictionary,
						constructor.WithCapacity,
						constructor.Default,
						elementType,
						GetGetEnumeratorMethodFromElementType(source, elementType, options),
						GetAddMethod(source, genericArguments[0], genericArguments[1], options),
						GetCountGetterMethod(source, elementType, options)
					);
			}

#if !NET35 && !UNITY && !NET40 && !(SILVERLIGHT && !WINDOWS_PHONE)
			if (genericTypes.IReadOnlyDictionaryT != null)
			{
				var genericArguments = genericTypes.IReadOnlyDictionaryT.GetGenericArguments();
				var elementType = typeof(KeyValuePair<,>).MakeGenericType(genericArguments);
				var constructor = GetGenericDictionaryConstructor(source, genericArguments[0], genericArguments[1], options);

				return
					new CollectionTraits(
						CollectionDetailedKind.GenericReadOnlyDictionary,
						constructor.WithCapacity,
						constructor.Default,
						elementType,
						GetGetEnumeratorMethodFromElementType(source, elementType, options),
						addMethod: null, // add
						GetCountGetterMethod(source, elementType, options)
					);
			}
#endif // !NET35 && !UNITY && !NET40 && !( SILVERLIGHT && !WINDOWS_PHONE )

			if (genericTypes.IEnumerableT != null)
			{
				CollectionTraits traits;
				if (TryCreateCollectionTraitsForIEnumerableT(source, genericTypes, options, null, out traits))
				{
					return traits;
				}
			}

			if (idictionary != null)
			{
				var constructor = GetNonGenericDictionaryConstructor(source, options);
				return
					new CollectionTraits(
						CollectionDetailedKind.NonGenericDictionary,
						constructor.WithCapacity,
						constructor.Default,
						typeof(object),
						GetGetEnumeratorMethodFromEnumerableType(source, idictionary, options),
						GetAddMethod(source, typeof(object), typeof(object), options),
						GetCountGetterMethod(source, typeof(object), options)
					);
			}

			if (ienumerable != null)
			{
				var addMethod = GetAddMethod(source, typeof(object), options | CollectionTraitOptions.WithAddMethod);
				if (addMethod != null || ((options & CollectionTraitOptions.AllowNonCollectionEnumerableTypes) == 0))
				{
					var constructor = GetNonGenericCollectionConstructor(source, options);
					// This should be appendable or unappendable collection
					return
						new CollectionTraits(
							(ilist != null)
								? CollectionDetailedKind.NonGenericList
								: (icollection != null)
									? CollectionDetailedKind.NonGenericCollection
									: CollectionDetailedKind.NonGenericEnumerable,
							constructor.WithCapacity,
							constructor.Default,
							typeof(object),
							GetGetEnumeratorMethodFromEnumerableType(source, ienumerable, options),
							addMethod,
							GetCountGetterMethod(source, typeof(object), options)
						);
				}
			}

			return CollectionTraits.NotCollection;
		}

		private static bool TryCreateCollectionTraitsForIEnumerableT(
			Type source,
			GenericCollectionTypes genericTypes,
			CollectionTraitOptions options,
			MethodInfo? getMethod,
			out CollectionTraits result
		)
		{
			var elementType = genericTypes.IEnumerableT.GetGenericArguments()[0];
			var addMethod = GetAddMethod(source, elementType, options);
			if (addMethod == null && ((options & CollectionTraitOptions.AllowNonCollectionEnumerableTypes) != 0))
			{
				// This should be non collection object instead of "unappendable" collection.
				result = default(CollectionTraits);
				return false;
			}

			CollectionDetailedKind kind = CollectionDetailedKind.GenericEnumerable;
			if (genericTypes.IListT != null)
			{
				kind = CollectionDetailedKind.GenericList;
			}
#if !NET35 && !UNITY
			else if (genericTypes.ISetT != null)
			{
				kind = CollectionDetailedKind.GenericSet;
			}
#endif // !NET35 && !UNITY
			else if (genericTypes.ICollectionT != null)
			{
				kind = CollectionDetailedKind.GenericCollection;
			}
#if !NET35 && !UNITY && !NET40 && !(SILVERLIGHT && !WINDOWS_PHONE)
			else if (genericTypes.IReadOnlyListT != null)
			{
				kind = CollectionDetailedKind.GenericReadOnlyList;
			}
			else if (genericTypes.IReadOnlyCollectionT != null)
			{
				kind = CollectionDetailedKind.GenericReadOnlyCollection;
			}
#endif // !NET35 && !UNITY && !NET40 && !( SILVERLIGHT && !WINDOWS_PHONE )

			var constructor =
				kind == CollectionDetailedKind.GenericSet ?
					GetGenericSetConstructor(source, elementType, options) :
					GetGenericCollectionConstructor(source, elementType, options);
			result =
				new CollectionTraits(
					kind,
					constructor.WithCapacity,
					constructor.Default,
					elementType,
					getMethod ?? GetGetEnumeratorMethodFromElementType(source, elementType, options),
					addMethod,
					GetCountGetterMethod(source, elementType, options)
				);
			return true;
		}

		private static bool TryCreateCollectionTraitsForHasGetEnumeratorType(
			Type source,
			CollectionTraitOptions options,
			MethodInfo getEnumerator,
			out CollectionTraits result
		)
		{
			if (source.Implements(typeof(IDictionary<,>))
#if !NET35 && !UNITY && !NET40 && !(SILVERLIGHT && !WINDOWS_PHONE)
				|| source.Implements(typeof(IReadOnlyDictionary<,>))
#endif // !NET35 && !UNITY && !NET40 && !( SILVERLIGHT && !WINDOWS_PHONE )
			)
			{
				var ienumetaorT =
					getEnumerator.ReturnType.GetInterfaces()
						.FirstOrDefault(@interface =>
							@interface.GetIsGenericType() && @interface.GetGenericTypeDefinition() == typeof(IEnumerator<>)
						);
				if (ienumetaorT != null)
				{
					var elementType = ienumetaorT.GetGenericArguments()[0];
					var elementTypeGenericArguments = elementType.GetGenericArguments();
					var constructor = GetGenericDictionaryConstructor(source, elementTypeGenericArguments[0], elementTypeGenericArguments[1], options);

					result =
						new CollectionTraits(
#if !NET35 && !UNITY && !NET40 && !(SILVERLIGHT && !WINDOWS_PHONE)
							source.Implements(typeof(IDictionary<,>))
								? CollectionDetailedKind.GenericDictionary
								: CollectionDetailedKind.GenericReadOnlyDictionary,
#else
							CollectionDetailedKind.GenericDictionary,
#endif // !NET35 && !UNITY && !NET40 && !( SILVERLIGHT && !WINDOWS_PHONE )
							constructor.WithCapacity,
							constructor.Default,
							elementType,
							getEnumerator,
							GetAddMethod(source, elementTypeGenericArguments[0], elementTypeGenericArguments[1], options),
							GetCountGetterMethod(source, elementType, options)
						);

					return true;
				}
			}

			if (source.IsAssignableTo(typeof(IDictionary)))
			{
				var constructor = GetNonGenericDictionaryConstructor(source, options);
				result =
					new CollectionTraits(
						CollectionDetailedKind.NonGenericDictionary,
						constructor.WithCapacity,
						constructor.Default,
						typeof(DictionaryEntry),
						getEnumerator,
						GetAddMethod(source, typeof(object), typeof(object), options),
						GetCountGetterMethod(source, typeof(object), options)
					);

				return true;
			}

			// Block to limit variable scope
			{
				var ienumetaorT =
					IsIEnumeratorT(getEnumerator.ReturnType)
						? getEnumerator.ReturnType
						: getEnumerator.ReturnType.GetInterfaces().FirstOrDefault(IsIEnumeratorT);

				if (ienumetaorT != null)
				{
					// Get the open generic types once
					Type[] genericInterfaces =
						source.GetInterfaces()
							.Where(i => i.GetIsGenericType())
							.Select(i => i.GetGenericTypeDefinition())
							.ToArray();

					var genericTypes = new GenericCollectionTypes();
					genericTypes.IEnumerableT = ienumetaorT;
					genericTypes.ICollectionT = genericInterfaces.FirstOrDefault(i => i == typeof(ICollection<>));
					genericTypes.IListT = genericInterfaces.FirstOrDefault(i => i == typeof(IList<>));

#if !NET35 && !UNITY
					genericTypes.ISetT = genericInterfaces.FirstOrDefault(i => i == typeof(ISet<>));
#endif // !NET35 && !UNITY
#if !NET35 && !UNITY && !NET40 && !(SILVERLIGHT && !WINDOWS_PHONE)
					genericTypes.IReadOnlyCollectionT = genericInterfaces.FirstOrDefault(i => i == typeof(IReadOnlyCollection<>));
					genericTypes.IReadOnlyListT = genericInterfaces.FirstOrDefault(i => i == typeof(IReadOnlyList<>));
#endif // !NET35 && !UNITY && !NET40 && !( SILVERLIGHT && !WINDOWS_PHONE )

					return TryCreateCollectionTraitsForIEnumerableT(source, genericTypes, options, getEnumerator, out result);
				}
			}

			result = default(CollectionTraits);
			return false;
		}

		private static bool DetermineCollectionInterfaces(
			Type type,
			ref GenericCollectionTypes genericTypes,
			ref Type? idictionary,
			ref Type? ilist,
			ref Type? icollection,
			ref Type? ienumerable
		)
		{
			if (type.GetIsGenericType())
			{
				var genericTypeDefinition = type.GetGenericTypeDefinition();
				if (genericTypeDefinition == typeof(IDictionary<,>))
				{
					if (genericTypes.IDictionaryT != null)
					{
						// multiple candidates
						return false;
					}

					genericTypes.IDictionaryT = type;
				}
#if !NET35 && !UNITY && !NET40 && !(SILVERLIGHT && !WINDOWS_PHONE)
				else if (genericTypeDefinition == typeof(IReadOnlyDictionary<,>))
				{
					if (genericTypes.IReadOnlyDictionaryT != null)
					{
						// multiple candidates
						return false;
					}

					genericTypes.IReadOnlyDictionaryT = type;
				}
#endif // !NET35 && !UNITY && !NET40 && !( SILVERLIGHT && !WINDOWS_PHONE )
				else if (genericTypeDefinition == typeof(IList<>))
				{
					if (genericTypes.IListT != null)
					{
						// multiple candidates
						return false;
					}

					genericTypes.IListT = type;
				}
#if !NET35 && !UNITY && !NET40 && !(SILVERLIGHT && !WINDOWS_PHONE)
				else if (genericTypeDefinition == typeof(IReadOnlyList<>))
				{
					if (genericTypes.IReadOnlyListT != null)
					{
						// multiple candidates
						return false;
					}

					genericTypes.IReadOnlyListT = type;
				}
#endif // !NET35 && !UNITY && !NET40 && !( SILVERLIGHT && !WINDOWS_PHONE )
#if !NET35 && !UNITY
				else if (genericTypeDefinition == typeof(ISet<>))
				{
					if (genericTypes.ISetT != null)
					{
						// multiple candidates
						return false;
					}

					genericTypes.ISetT = type;
				}
#endif // !NET35 && !UNITY
				else if (genericTypeDefinition == typeof(ICollection<>))
				{
					if (genericTypes.ICollectionT != null)
					{
						// multiple candidates
						return false;
					}

					genericTypes.ICollectionT = type;
				}
#if !NET35 && !UNITY && !NET40 && !(SILVERLIGHT && !WINDOWS_PHONE)
				else if (genericTypeDefinition == typeof(IReadOnlyCollection<>))
				{
					if (genericTypes.IReadOnlyCollectionT != null)
					{
						// multiple candidates
						return false;
					}

					genericTypes.IReadOnlyCollectionT = type;
				}
#endif // !NET35 && !UNITY && !NET40 && !( SILVERLIGHT && !WINDOWS_PHONE )
				else if (genericTypeDefinition == typeof(IEnumerable<>))
				{
					if (genericTypes.IEnumerableT != null)
					{
						// multiple candidates
						return false;
					}

					genericTypes.IEnumerableT = type;
				}
			}
			else
			{
				if (type == typeof(IDictionary))
				{
					idictionary = type;
				}
				else if (type == typeof(IList))
				{
					ilist = type;
				}
				else if (type == typeof(ICollection))
				{
					icollection = type;
				}
				else if (type == typeof(IEnumerable))
				{
					ienumerable = type;
				}
			}

			return true;
		}

		private static MethodInfo? GetGetEnumeratorMethodFromElementType(Type targetType, Type elementType, CollectionTraitOptions options)
		{
			if ((options | CollectionTraitOptions.WithGetEnumeratorMethod) == 0)
			{
				return null;
			}

			return FindInterfaceMethod(targetType, typeof(IEnumerable<>).MakeGenericType(elementType), "GetEnumerator", ReflectionAbstractions.EmptyTypes);
		}

		private static MethodInfo? GetGetEnumeratorMethodFromEnumerableType(Type targetType, Type enumerableType, CollectionTraitOptions options)
		{
			if ((options | CollectionTraitOptions.WithGetEnumeratorMethod) == 0)
			{
				return null;
			}

			return FindInterfaceMethod(targetType, enumerableType, "GetEnumerator", ReflectionAbstractions.EmptyTypes);
		}

		private static MethodInfo? FindInterfaceMethod(Type targetType, Type interfaceType, string name, Type[] parameterTypes)
		{
			if (targetType.GetIsInterface())
			{
				return targetType.FindInterfaces((type, _) => type == interfaceType, null).Single().GetMethod(name, parameterTypes);
			}

			var map = targetType.GetInterfaceMap(interfaceType);

#if !SILVERLIGHT || WINDOWS_PHONE
			int index = Array.FindIndex(map.InterfaceMethods, method => method.Name == name && method.GetParameters().Select(p => p.ParameterType).SequenceEqual(parameterTypes));
#else
			int index = map.InterfaceMethods.FindIndex( method => method.Name == name && method.GetParameters().Select( p => p.ParameterType ).SequenceEqual( parameterTypes ) );
#endif

			if (index < 0)
			{
#if DEBUG
#if !NET35 && !UNITY
				Contract.Assert(false, interfaceType + "::" + name + "(" + String.Join<Type>(", ", parameterTypes) + ") is not found in " + targetType);
#else
				Contract.Assert( false, interfaceType + "::" + name + "(" + String.Join( ", ", parameterTypes.Select( t => t.ToString() ).ToArray() ) + ") is not found in " + targetType );
#endif // !NET35
#endif // DEBUG
				// ReSharper disable once HeuristicUnreachableCode
				return null;
			}

			return map.TargetMethods[index];
		}

		private static MethodInfo? GetAddMethod(Type targetType, Type argumentType, CollectionTraitOptions options)
		{
			if ((options | CollectionTraitOptions.WithAddMethod) == 0)
			{
				return null;
			}

			var argumentTypes = new[] { argumentType };
			var typedAdd = targetType.GetMethod("Add", argumentTypes);
			if (typedAdd != null)
			{
				return typedAdd;
			}

			var icollectionT = typeof(ICollection<>).MakeGenericType(argumentType);
			if (targetType.IsAssignableTo(icollectionT))
			{
				return icollectionT.GetMethod("Add", argumentTypes);
			}

			// It ensures .NET Framework and .NET Core compatibility and provides "natural" feel.
			var objectAdd = targetType.GetMethod("Add", ObjectAddParameterTypes);
			if (objectAdd != null)
			{
				return objectAdd;
			}

			if (targetType.IsAssignableTo(typeof(IList)))
			{
				return typeof(IList).GetMethod("Add", ObjectAddParameterTypes);
			}

			return null;
		}

		// ReSharper disable UnusedParameter.Local
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "targetType", Justification = "For Unity compatibility")]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "elementType", Justification = "For Unity compatibility")]
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "options", Justification = "For Unity compatibility")]
		private static MethodInfo? GetCountGetterMethod(Type targetType, Type elementType, CollectionTraitOptions options)
		// ReSharper restore UnusedParameter.Local
		{
#if !UNITY
			// get_Count is not used other than Unity.
			return null;
#else
			if ( ( options | CollectionTraitOptions.WithCountPropertyGetter ) == 0 )
			{
				return null;
			}

			var result = targetType.GetProperty( "Count" );
			if ( result != null && result.GetHasPublicGetter() )
			{
				return result.GetGetMethod();
			}

			var icollectionT = typeof( ICollection<> ).MakeGenericType( elementType );
			if ( targetType.IsAssignableTo( icollectionT ) )
			{
				return icollectionT.GetProperty( "Count" ).GetGetMethod();
			}

#if !NET35 && !UNITY && !NET40 && !(SILVERLIGHT && !WINDOWS_PHONE)
			var ireadOnlyCollectionT = typeof( IReadOnlyCollection<> ).MakeGenericType( elementType );
			if ( targetType.IsAssignableTo( ireadOnlyCollectionT ) )
			{
				return ireadOnlyCollectionT.GetProperty( "Count" ).GetGetMethod();
			}
#endif // !NET35 && !UNITY && !NET40 && !( SILVERLIGHT && !WINDOWS_PHONE )

			if ( targetType.IsAssignableTo( typeof( ICollection ) ) )
			{
				return typeof( ICollection ).GetProperty( "Count" ).GetGetMethod();
			}

			return null;
#endif // !UNITY
		}

		private static MethodInfo? GetAddMethod(Type targetType, Type keyType, Type valueType, CollectionTraitOptions options)
		{
			if ((options | CollectionTraitOptions.WithAddMethod) == 0)
			{
				return null;
			}

			var argumentTypes = new[] { keyType, valueType };
			var result = targetType.GetMethod("Add", argumentTypes);
			if (result != null)
			{
				return result;
			}

			return typeof(IDictionary<,>).MakeGenericType(argumentTypes).GetMethod("Add", argumentTypes);
		}

		private static bool FilterCollectionType(Type type, object? filterCriteria)
		{
#if !NETSTANDARD1_1 && !NETSTANDARD1_3
#if DEBUG
			Contract.Assert(type.GetIsInterface(), "type.IsInterface");
#endif // DEBUG
			return type.GetAssembly().Equals(typeof(Array).GetAssembly()) && (type.Namespace == "System.Collections" || type.Namespace == "System.Collections.Generic");
#else
			var typeInfo = type.GetTypeInfo();
			Contract.Assert( typeInfo.IsInterface );
			return typeInfo.Assembly.Equals( typeof( Array ).GetTypeInfo().Assembly ) && ( type.Namespace == "System.Collections" || type.Namespace == "System.Collections.Generic" );
#endif // !NETSTANDARD1_1 && !NETSTANDARD1_3
		}

		private static bool IsIEnumeratorT(Type @interface)
		{
			return @interface.GetIsGenericType() && @interface.GetGenericTypeDefinition() == typeof(IEnumerator<>);
		}
#if WINDOWS_PHONE
		public static IEnumerable<Type> FindInterfaces( this Type source, Func<Type, object, bool> filter, object criterion )
		{
			foreach ( var @interface in source.GetInterfaces() )
			{
				if ( filter( @interface, criterion ) )
				{
					yield return @interface;
				}
			}
		}
#endif

		public static bool GetHasPublicGetter(this MemberInfo source)
		{
			if (source is PropertyInfo asProperty)
			{
#if !NETSTANDARD1_1 && !NETSTANDARD1_3
				return asProperty.GetGetMethod() != null;
#else
				return ( asProperty.GetMethod != null && asProperty.GetMethod.IsPublic );
#endif // !NETSTANDARD1_1 && !NETSTANDARD1_3
			}
			else if (source is FieldInfo asField)
			{
				return asField.IsPublic;
			}
			else
			{
				throw new NotSupportedException(source.GetType() + " is not supported.");
			}
		}

		public static bool GetHasPublicSetter(this MemberInfo source)
		{
			if (source is PropertyInfo asProperty)
			{
#if !NETSTANDARD1_1 && !NETSTANDARD1_3
				return asProperty.GetSetMethod() != null;
#else
				return ( asProperty.SetMethod != null && asProperty.SetMethod.IsPublic );
#endif // !NETSTANDARD1_1 && !NETSTANDARD1_3
			}
			else if (source is FieldInfo asField)
			{
				return asField.IsPublic && !asField.IsInitOnly && !asField.IsLiteral;
			}
			else
			{
				throw new NotSupportedException(source.GetType() + " is not supported.");
			}
		}

		public static bool GetIsPublic(this MemberInfo source)
		{
			if (source is PropertyInfo asProperty)
			{
				return asProperty.GetAccessors(true).Where(a => a.ReturnType != typeof(void)).All(a => a.IsPublic);
			}
			else if (source is FieldInfo asField)
			{
				return asField.IsPublic;
			}
			else if (source is MethodBase asMethod)
			{
				return asMethod.IsPublic;
			}
			else if (source is Type asType)
			{
				return asType.IsPublic;
			}
			else
			{
				throw new NotSupportedException(source.GetType() + " is not supported.");
			}
		}

		private static (ConstructorInfo? WithCapacity, ConstructorInfo? Default) GetGenericCollectionConstructor(Type source, Type elementType, CollectionTraitOptions options)
		{
			if ((options & CollectionTraitOptions.WithConstructor) == 0)
			{
				// skip
				return (null, null);
			}

			if (source.GetIsInterface())
			{
				var listOfT = typeof(List<>).MakeGenericType(elementType);
				return
					(listOfT.GetConstructor(WithCapacityConstructorParameterTypes),
					listOfT.GetConstructor(ReflectionAbstractions.EmptyTypes));
			}

			return GetConcreteCollectionConstructor(source);
		}

		private static (ConstructorInfo? WithCapacity, ConstructorInfo? Default) GetGenericSetConstructor(Type source, Type elementType, CollectionTraitOptions options)
		{
			if ((options & CollectionTraitOptions.WithConstructor) == 0)
			{
				// skip
				return (null, null);
			}

			if (source.GetIsInterface())
			{
				var hashSetOfT = typeof(HashSet<>).MakeGenericType(elementType);
				return
					(hashSetOfT.GetConstructor(WithCapacityConstructorParameterTypes),
					hashSetOfT.GetConstructor(ReflectionAbstractions.EmptyTypes));
			}

			return GetConcreteCollectionConstructor(source);
		}

		private static (ConstructorInfo? WithCapacity, ConstructorInfo? Default) GetGenericDictionaryConstructor(Type source, Type keyType, Type valueType, CollectionTraitOptions options)
		{
			if ((options & CollectionTraitOptions.WithConstructor) == 0)
			{
				// skip
				return (null, null);
			}

			if (source.GetIsInterface())
			{
				var dictionaryOfTkeyAndTvalue = typeof(Dictionary<,>).MakeGenericType(keyType, valueType);
				return
					(dictionaryOfTkeyAndTvalue.GetConstructor(WithCapacityConstructorParameterTypes),
					dictionaryOfTkeyAndTvalue.GetConstructor(ReflectionAbstractions.EmptyTypes));
			}

			return GetConcreteCollectionConstructor(source);
		}

		private static (ConstructorInfo? WithCapacity, ConstructorInfo? Default) GetNonGenericCollectionConstructor(Type source, CollectionTraitOptions options)
		{
			if ((options & CollectionTraitOptions.WithConstructor) == 0)
			{
				// skip
				return (null, null);
			}

			if (source.GetIsInterface())
			{
				return ArrayListConstructors;
			}

			return GetConcreteCollectionConstructor(source);
		}

		private static (ConstructorInfo? WithCapacity, ConstructorInfo? Default) GetNonGenericDictionaryConstructor(Type source, CollectionTraitOptions options)
		{
			if ((options & CollectionTraitOptions.WithConstructor) == 0)
			{
				// skip
				return (null, null);
			}

			if (source.GetIsInterface())
			{
				return HashtableConstructors;
			}

			return GetConcreteCollectionConstructor(source);
		}

		private static (ConstructorInfo? WithCapacity, ConstructorInfo? Default) GetConcreteCollectionConstructor(Type source)
		{
			var @default = source.GetConstructor(ReflectionAbstractions.EmptyTypes);
			var withCapacity = source.GetConstructor(BindingFlags.Public | BindingFlags.Instance, binder: null, WithCapacityConstructorParameterTypes, null);
			if (withCapacity != null)
			{
				var shouldBeCapacity = withCapacity.GetParameters()[0].Name;
				if (!String.Equals(shouldBeCapacity, "capacity", StringComparison.OrdinalIgnoreCase)
					&& !String.Equals(shouldBeCapacity, "initialCapacity", StringComparison.OrdinalIgnoreCase))
				{
					shouldBeCapacity = null;
				}
			}

			return (withCapacity, @default);
		}

		private struct GenericCollectionTypes
		{
			// ReSharper disable InconsistentNaming
			internal Type IEnumerableT;
			internal Type ICollectionT;
			internal Type IListT;
			internal Type IDictionaryT;

#if !NET35 && !UNITY
			internal Type ISetT;
#if !NET40 && !(SILVERLIGHT && !WINDOWS_PHONE)
			internal Type IReadOnlyCollectionT;
			internal Type IReadOnlyListT;
			internal Type IReadOnlyDictionaryT;
#endif // !NET40 && !( SILVERLIGHT && !WINDOWS_PHONE )
#endif // !NET35 && !UNITY
			// ReSharper restore InconsistentNaming
		}
	}
}
