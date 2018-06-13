#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2016 FUJIWARA, Yusuke and contributors
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
//
// Contributors:
//    Samuel Cragg
//
#endregion -- License Terms --

#if UNITY_5 || UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_WII || UNITY_IPHONE || UNITY_ANDROID || UNITY_PS3 || UNITY_XBOX360 || UNITY_FLASH || UNITY_BKACKBERRY || UNITY_WINRT
#define UNITY
#endif

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
	partial class ReflectionExtensions
	{
		internal static CollectionTraits GetCollectionTraits( this Type source, CollectionTraitOptions options, bool allowNonCollectionEnumerableTypes )
		{
#if DEBUG
			Contract.Assert( !source.GetContainsGenericParameters(), "!source.GetContainsGenericParameters()" );
#endif // DEBUG
			/*
			 * SPEC
			 * If the object has single public method TEnumerator GetEnumerator() ( where TEnumerator implements IEnumerator<TItem>),
			 * then the object is considered as the collection of TItem.
			 * When the object is considered as the collection of TItem, TItem is KeyValuePair<TKey,TValue>, 
			 * and the object implements IDictionary<TKey,TValue>, then the object is considered as dictionary of TKey and TValue.
			 * Else, if the object has single public method IEnumerator GetEnumerator(), then the object is considered as the collection of Object.
			 * When it also implements IDictionary, however, it is considered dictionary of Object and Object.
			 * Otherwise, that means it implements multiple collection interface, is following.
			 * First, if the object implements IDictionary<MessagePackObject,MessagePackObject>, then it is considered as MPO dictionary.
			 * Second, if the object implements IEnumerable<MPO>, then it is considered as MPO dictionary.
			 * Third, if the object implement SINGLE IDictionary<TKey,TValue> and multiple IEnumerable<T>, then it is considered as dictionary of TKey and TValue.
			 * Fourth, the object is considered as UNSERIALIZABLE member. This behavior similer to DataContract serialization behavor
			 * (see http://msdn.microsoft.com/en-us/library/aa347850.aspx ).
			 */

			if ( !source.IsAssignableTo( typeof( IEnumerable ) ) )
			{
				return CollectionTraits.NotCollection;
			}

			if ( source.IsArray )
			{
				return
					new CollectionTraits(
						CollectionDetailedKind.Array,
						source.GetElementType(),
						null, // Never used for array.
						null, // Never used for array.
						null  // Never used for array.
					);
			}

			// If the type is an interface then a concrete collection has to be
			// made for it (if the interface is a collection type), therefore,
			// ignore the check for an add method
			if ( !source.GetIsInterface() && allowNonCollectionEnumerableTypes )
			{
				options = options | CollectionTraitOptions.AllowNonCollectionEnumerableTypes;
			}

			MethodInfo getEnumerator = source.GetMethod( "GetEnumerator", ReflectionAbstractions.EmptyTypes );
			if ( getEnumerator != null && getEnumerator.ReturnType.IsAssignableTo( typeof( IEnumerator ) ) )
			{
				// If public 'GetEnumerator' is found, it is primary collection traits.
				CollectionTraits result;
				if ( TryCreateCollectionTraitsForHasGetEnumeratorType( source, options, getEnumerator, out result ) )
				{
					return result;
				}
			}

			GenericCollectionTypes genericTypes = new GenericCollectionTypes();
			Type ienumerable = null;
			Type icollection = null;
			Type ilist = null;
			Type idictionary = null;

			var sourceInterfaces = source.FindInterfaces( FilterCollectionType, null );
			if ( source.GetIsInterface() && FilterCollectionType( source, null ) )
			{
				var originalSourceInterfaces = sourceInterfaces.ToArray();
				var concatenatedSourceInterface = new Type[ originalSourceInterfaces.Length + 1 ];
				concatenatedSourceInterface[ 0 ] = source;
				for ( int i = 0; i < originalSourceInterfaces.Length; i++ )
				{
					concatenatedSourceInterface[ i + 1 ] = originalSourceInterfaces[ i ];
				}

				sourceInterfaces = concatenatedSourceInterface;
			}

			foreach ( var type in sourceInterfaces )
			{
				CollectionTraits result;
				if ( TryCreateGenericCollectionTraits( source, type, options, out result ) )
				{
					return result;
				}

				if ( !DetermineCollectionInterfaces(
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

			if ( genericTypes.IDictionaryT != null )
			{
				var genericArguments = genericTypes.IDictionaryT.GetGenericArguments();
				var elementType = typeof( KeyValuePair<,> ).MakeGenericType( genericArguments );

				return 
					new CollectionTraits(
						CollectionDetailedKind.GenericDictionary,
						elementType,
						GetGetEnumeratorMethodFromElementType( source, elementType, options ),
						GetAddMethod( source, genericArguments[ 0 ], genericArguments[ 1 ], options ),
						GetCountGetterMethod( source, elementType, options )
					);
			}

#if !NET35 && !UNITY && !NET40 && !( SILVERLIGHT && !WINDOWS_PHONE )
			if ( genericTypes.IReadOnlyDictionaryT != null )
			{
				var elementType = typeof( KeyValuePair<,> ).MakeGenericType( genericTypes.IReadOnlyDictionaryT.GetGenericArguments() );
				return
					new CollectionTraits(
						CollectionDetailedKind.GenericReadOnlyDictionary,
						elementType,
						GetGetEnumeratorMethodFromElementType( source, elementType, options ),
						null, // add
						GetCountGetterMethod( source, elementType, options )
					);
			}
#endif // !NET35 && !UNITY && !NET40 && !( SILVERLIGHT && !WINDOWS_PHONE )

			if ( genericTypes.IEnumerableT != null )
			{
				CollectionTraits traits;
				if ( TryCreateCollectionTraitsForIEnumerableT( source, genericTypes, options, null, out traits ) )
				{
					return traits;
				}
			}

			if ( idictionary != null )
			{
				return
					new CollectionTraits(
						CollectionDetailedKind.NonGenericDictionary,
						typeof( object ),
						GetGetEnumeratorMethodFromEnumerableType( source, idictionary, options ),
						GetAddMethod( source, typeof( object ), typeof( object ), options ),
						GetCountGetterMethod( source, typeof( object ), options )
					);
			}

			if ( ienumerable != null )
			{
				var addMethod = GetAddMethod( source, typeof( object ), options | CollectionTraitOptions.WithAddMethod );
				if ( addMethod != null || ( ( options & CollectionTraitOptions.AllowNonCollectionEnumerableTypes ) == 0 ) )
				{
					// This should be appendable or unappendable collection
					return
						new CollectionTraits(
							( ilist != null )
								? CollectionDetailedKind.NonGenericList
								: ( icollection != null )
									? CollectionDetailedKind.NonGenericCollection
									: CollectionDetailedKind.NonGenericEnumerable,
							typeof( object ),
							GetGetEnumeratorMethodFromEnumerableType( source, ienumerable, options ),
							addMethod,
							GetCountGetterMethod( source, typeof( object ), options )
						);
				}
			}

			return CollectionTraits.NotCollection;
		}

		private static bool TryCreateCollectionTraitsForIEnumerableT(
			Type source,
			GenericCollectionTypes genericTypes,
			CollectionTraitOptions options,
			MethodInfo getMethod,
			out CollectionTraits result
		)
		{
			var elementType = genericTypes.IEnumerableT.GetGenericArguments()[ 0 ];
			var addMethod = GetAddMethod( source, elementType, options );
			if ( addMethod == null && ( ( options & CollectionTraitOptions.AllowNonCollectionEnumerableTypes ) != 0 ) )
			{
				// This should be non collection object isntead of "unappendable" collection.
				result = default( CollectionTraits );
				return false;
			}

			CollectionDetailedKind kind = CollectionDetailedKind.GenericEnumerable;
			if ( genericTypes.IListT != null )
			{
				kind = CollectionDetailedKind.GenericList;
			}
#if !NET35 && !UNITY
			else if ( genericTypes.ISetT != null )
			{
				kind = CollectionDetailedKind.GenericSet;
			}
#endif // !NET35 && !UNITY
			else if ( genericTypes.ICollectionT != null )
			{
				kind = CollectionDetailedKind.GenericCollection;
			}
#if !NET35 && !UNITY && !NET40 && !( SILVERLIGHT && !WINDOWS_PHONE )
			else if ( genericTypes.IReadOnlyListT != null )
			{
				kind = CollectionDetailedKind.GenericReadOnlyList;
			}
			else if ( genericTypes.IReadOnlyCollectionT != null )
			{
				kind = CollectionDetailedKind.GenericReadOnlyCollection;
			}
#endif // !NET35 && !UNITY && !NET40 && !( SILVERLIGHT && !WINDOWS_PHONE )
				
			result =
				new CollectionTraits(
					kind,
					elementType,
					getMethod ?? GetGetEnumeratorMethodFromElementType( source, elementType, options ),
					addMethod,
					GetCountGetterMethod( source, elementType, options )
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
			if ( source.Implements( typeof( IDictionary<,> ) )
#if !NET35 && !UNITY && !NET40 && !( SILVERLIGHT && !WINDOWS_PHONE )
				|| source.Implements( typeof( IReadOnlyDictionary<,> ) )
#endif // !NET35 && !UNITY && !NET40 && !( SILVERLIGHT && !WINDOWS_PHONE )
			)
			{
				var ienumetaorT =
					getEnumerator.ReturnType.GetInterfaces()
						.FirstOrDefault( @interface => 
												@interface.GetIsGenericType() && @interface.GetGenericTypeDefinition() == typeof( IEnumerator<> ) 
						);
				if ( ienumetaorT != null )
				{
					var elementType = ienumetaorT.GetGenericArguments()[ 0 ];
					var elementTypeGenericArguments = elementType.GetGenericArguments();

					result = 
						new CollectionTraits(
#if !NET35 && !UNITY && !NET40 && !( SILVERLIGHT && !WINDOWS_PHONE )
							source.Implements( typeof( IDictionary<,> ) )
								? CollectionDetailedKind.GenericDictionary
								: CollectionDetailedKind.GenericReadOnlyDictionary,
#else
							CollectionDetailedKind.GenericDictionary,
#endif // !NET35 && !UNITY && !NET40 && !( SILVERLIGHT && !WINDOWS_PHONE )
							elementType,
							getEnumerator,
							GetAddMethod( source, elementTypeGenericArguments[ 0 ], elementTypeGenericArguments [ 1 ], options ),
							GetCountGetterMethod( source, elementType, options )
						);

					return true;
				}
			}

			if ( source.IsAssignableTo( typeof( IDictionary ) ) )
			{
				result = 
					new CollectionTraits(
						CollectionDetailedKind.NonGenericDictionary,
						typeof( DictionaryEntry ),
						getEnumerator,
						GetAddMethod( source, typeof( object ), typeof( object ), options ),
						GetCountGetterMethod( source, typeof( object ), options )
					);

				return true;
			}

			// Block to limit variable scope
			{
				var ienumetaorT =
					IsIEnumeratorT( getEnumerator.ReturnType )
						? getEnumerator.ReturnType
						: getEnumerator.ReturnType.GetInterfaces().FirstOrDefault( IsIEnumeratorT );

				if ( ienumetaorT != null )
				{
					// Get the open generic types once
					Type[] genericInterfaces =
						source.GetInterfaces()
							.Where( i => i.GetIsGenericType() )
							.Select( i => i.GetGenericTypeDefinition() )
							.ToArray();

					var genericTypes = new GenericCollectionTypes();
					genericTypes.IEnumerableT = ienumetaorT;
					genericTypes.ICollectionT = genericInterfaces.FirstOrDefault( i => i == typeof(ICollection<>) );
					genericTypes.IListT = genericInterfaces.FirstOrDefault( i => i == typeof(IList<>) );

#if !NET35 && !UNITY
					genericTypes.ISetT = genericInterfaces.FirstOrDefault( i => i == typeof(ISet<>) );
#endif // !NET35 && !UNITY
#if !NET35 && !UNITY && !NET40 && !( SILVERLIGHT && !WINDOWS_PHONE )
					genericTypes.IReadOnlyCollectionT = genericInterfaces.FirstOrDefault( i => i == typeof(IReadOnlyCollection<>) );
					genericTypes.IReadOnlyListT = genericInterfaces.FirstOrDefault( i => i == typeof(IReadOnlyList<>) );
#endif // !NET35 && !UNITY && !NET40 && !( SILVERLIGHT && !WINDOWS_PHONE )

					return TryCreateCollectionTraitsForIEnumerableT( source, genericTypes, options, getEnumerator, out result );
				}
			}

			result = default( CollectionTraits );
			return false;
		}


		private static bool TryCreateGenericCollectionTraits( Type source, Type type, CollectionTraitOptions options, out CollectionTraits result )
		{
			if ( type == typeof( IDictionary<MessagePackObject, MessagePackObject> )
#if !NET35 && !UNITY && !NET40 && !( SILVERLIGHT && !WINDOWS_PHONE )
				|| type == typeof( IReadOnlyDictionary<MessagePackObject, MessagePackObject> )
#endif // !NET35 && !UNITY && !NET40 && !( SILVERLIGHT && !WINDOWS_PHONE )
			)
			{
				result = 
					new CollectionTraits(
#if !NET35 && !UNITY && !NET40 && !( SILVERLIGHT && !WINDOWS_PHONE )
						( source == typeof( IDictionary<MessagePackObject, MessagePackObject> ) || source.Implements( typeof( IDictionary<MessagePackObject, MessagePackObject> ) ) )
							? CollectionDetailedKind.GenericDictionary
							: CollectionDetailedKind.GenericReadOnlyDictionary,
#else
						CollectionDetailedKind.GenericDictionary,
#endif // !NET35 && !UNITY && !NET40 && !( SILVERLIGHT && !WINDOWS_PHONE )
						typeof( KeyValuePair<MessagePackObject, MessagePackObject> ),
						GetGetEnumeratorMethodFromEnumerableType( source, typeof( IEnumerable<KeyValuePair<MessagePackObject, MessagePackObject>> ), options ),
						GetAddMethod( source, typeof( MessagePackObject ), typeof( MessagePackObject ), options ),
						GetCountGetterMethod( source, typeof( KeyValuePair<MessagePackObject, MessagePackObject> ), options )
					);

				return true;
			}

			if ( type == typeof( IEnumerable<MessagePackObject> ) )
			{
				var addMethod = GetAddMethod( source, typeof( MessagePackObject ), options | CollectionTraitOptions.WithAddMethod );
				if ( addMethod != null )
				{
					{
						result = 
							new CollectionTraits(
								( source == typeof( IList<MessagePackObject> ) || source.Implements( typeof( IList<MessagePackObject> ) ) )
									? CollectionDetailedKind.GenericList
#if !NET35 && !UNITY && !NET40 && !( SILVERLIGHT && !WINDOWS_PHONE )
									: ( source == typeof( IReadOnlyList<MessagePackObject> ) || source.Implements( typeof( IReadOnlyList<MessagePackObject> ) ) )
										? CollectionDetailedKind.GenericReadOnlyList
#endif // !NET35 && !UNITY && !NET40 && !( SILVERLIGHT && !WINDOWS_PHONE )
#if !NET35 && !UNITY
										: ( source == typeof( ISet<MessagePackObject> ) || source.Implements( typeof( ISet<MessagePackObject> ) ) )
											? CollectionDetailedKind.GenericSet
#endif // !NET35 && !UNITY
											: ( source == typeof( ICollection<MessagePackObject> ) ||
												source.Implements( typeof( ICollection<MessagePackObject> ) ) )
												? CollectionDetailedKind.GenericCollection
#if !NET35 && !UNITY && !NET40 && !( SILVERLIGHT && !WINDOWS_PHONE )
												: ( source == typeof( IReadOnlyCollection<MessagePackObject> ) || source.Implements( typeof( IReadOnlyCollection<MessagePackObject> ) ) )
													? CollectionDetailedKind.GenericReadOnlyCollection
#endif // !NET35 && !UNITY && !NET40 && !( SILVERLIGHT && !WINDOWS_PHONE )
													: CollectionDetailedKind.GenericEnumerable,
								typeof( MessagePackObject ),
								GetGetEnumeratorMethodFromEnumerableType( source, typeof( IEnumerable<MessagePackObject> ), options ),
								addMethod,
								GetCountGetterMethod( source, typeof( MessagePackObject ), options )
							);

						return true;
					}
				}
			}

			result = default( CollectionTraits );
			return false;
		}

		private static bool DetermineCollectionInterfaces(
			Type type,
			ref GenericCollectionTypes genericTypes,
			ref Type idictionary,
			ref Type ilist,
			ref Type icollection,
			ref Type ienumerable
		)
		{
			if ( type.GetIsGenericType() )
			{
				var genericTypeDefinition = type.GetGenericTypeDefinition();
				if ( genericTypeDefinition == typeof( IDictionary<,> ) )
				{
					if ( genericTypes.IDictionaryT != null )
					{
						return false;
					}

					genericTypes.IDictionaryT = type;
				}
#if !NET35 && !UNITY && !NET40 && !( SILVERLIGHT && !WINDOWS_PHONE )
				else if ( genericTypeDefinition == typeof( IReadOnlyDictionary<,> ) )
				{
					if ( genericTypes.IReadOnlyDictionaryT != null )
					{
						return false;
					}

					genericTypes.IReadOnlyDictionaryT = type;
				}
#endif // !NET35 && !UNITY && !NET40 && !( SILVERLIGHT && !WINDOWS_PHONE )
				else if ( genericTypeDefinition == typeof( IList<> ) )
				{
					if ( genericTypes.IListT != null )
					{
						return false;
					}

					genericTypes.IListT = type;
				}
#if !NET35 && !UNITY && !NET40 && !( SILVERLIGHT && !WINDOWS_PHONE )
				else if ( genericTypeDefinition == typeof( IReadOnlyList<> ) )
				{
					if ( genericTypes.IReadOnlyListT != null )
					{
						return false;
					}

					genericTypes.IReadOnlyListT = type;
				}
#endif // !NET35 && !UNITY && !NET40 && !( SILVERLIGHT && !WINDOWS_PHONE )
#if !NET35 && !UNITY
				else if ( genericTypeDefinition == typeof( ISet<> ) )
				{
					if ( genericTypes.ISetT != null )
					{
						return false;
					}

					genericTypes.ISetT = type;
				}
#endif // !NET35 && !UNITY
				else if ( genericTypeDefinition == typeof( ICollection<> ) )
				{
					if ( genericTypes.ICollectionT != null )
					{
						return false;
					}

					genericTypes.ICollectionT = type;
				}
#if !NET35 && !UNITY && !NET40 && !( SILVERLIGHT && !WINDOWS_PHONE )
				else if ( genericTypeDefinition == typeof( IReadOnlyCollection<> ) )
				{
					if ( genericTypes.IReadOnlyCollectionT != null )
					{
						return false;
					}

					genericTypes.IReadOnlyCollectionT = type;
				}
#endif // !NET35 && !UNITY && !NET40 && !( SILVERLIGHT && !WINDOWS_PHONE )
				else if ( genericTypeDefinition == typeof( IEnumerable<> ) )
				{
					if ( genericTypes.IEnumerableT != null )
					{
						return false;
					}

					genericTypes.IEnumerableT = type;
				}
			}
			else
			{
				if ( type == typeof( IDictionary ) )
				{
					idictionary = type;
				}
				else if ( type == typeof( IList ) )
				{
					ilist = type;
				}
				else if ( type == typeof( ICollection ) )
				{
					icollection = type;
				}
				else if ( type == typeof( IEnumerable ) )
				{
					ienumerable = type;
				}
			}

			return true;
		}

		private static MethodInfo GetGetEnumeratorMethodFromElementType( Type targetType, Type elementType, CollectionTraitOptions options )
		{
			if ( ( options | CollectionTraitOptions.WithGetEnumeratorMethod ) == 0 )
			{
				return null;
			}

			return FindInterfaceMethod( targetType, typeof( IEnumerable<> ).MakeGenericType( elementType ), "GetEnumerator", ReflectionAbstractions.EmptyTypes );
		}

		private static MethodInfo GetGetEnumeratorMethodFromEnumerableType( Type targetType, Type enumerableType, CollectionTraitOptions options )
		{
			if ( ( options | CollectionTraitOptions.WithGetEnumeratorMethod ) == 0 )
			{
				return null;
			}

			return FindInterfaceMethod( targetType, enumerableType, "GetEnumerator", ReflectionAbstractions.EmptyTypes );
		}

		private static MethodInfo FindInterfaceMethod( Type targetType, Type interfaceType, string name, Type[] parameterTypes )
		{
			if ( targetType.GetIsInterface() )
			{
				return targetType.FindInterfaces( ( type, _ ) => type == interfaceType, null ).Single().GetMethod( name, parameterTypes );
			}

			var map = targetType.GetInterfaceMap( interfaceType );

#if !SILVERLIGHT || WINDOWS_PHONE
			int index = Array.FindIndex( map.InterfaceMethods, method => method.Name == name && method.GetParameters().Select( p => p.ParameterType ).SequenceEqual( parameterTypes ) );
#else
			int index = map.InterfaceMethods.FindIndex( method => method.Name == name && method.GetParameters().Select( p => p.ParameterType ).SequenceEqual( parameterTypes ) );
#endif

			if ( index < 0 )
			{
#if DEBUG
#if !NET35 && !UNITY
				Contract.Assert( false, interfaceType + "::" + name + "(" + String.Join<Type>( ", ", parameterTypes ) + ") is not found in " + targetType );
#else
				Contract.Assert( false, interfaceType + "::" + name + "(" + String.Join( ", ", parameterTypes.Select( t => t.ToString() ).ToArray() ) + ") is not found in " + targetType );
#endif // !NET35
#endif // DEBUG
				// ReSharper disable once HeuristicUnreachableCode
				return null;
			}

			return map.TargetMethods[ index ];
		}

		private static MethodInfo GetAddMethod( Type targetType, Type argumentType, CollectionTraitOptions options )
		{
			if ( ( options | CollectionTraitOptions.WithAddMethod ) == 0 )
			{
				return null;
			}

			var argumentTypes = new[] { argumentType };
			var typedAdd = targetType.GetMethod( "Add", argumentTypes );
			if ( typedAdd != null )
			{
				return typedAdd;
			}

			var icollectionT = typeof( ICollection<> ).MakeGenericType( argumentType );
			if ( targetType.IsAssignableTo( icollectionT ) )
			{
				return icollectionT.GetMethod( "Add", argumentTypes );
			}

			// It ensures .NET Framework and .NET Core compatibility and provides "natural" feel.
			var objectAdd = targetType.GetMethod( "Add", ObjectAddParameterTypes );
			if ( objectAdd != null )
			{
				return objectAdd;
			}

			if ( targetType.IsAssignableTo( typeof( IList ) ) )
			{
				return typeof( IList ).GetMethod( "Add", ObjectAddParameterTypes );
			}

			return null;
		}

		// ReSharper disable UnusedParameter.Local
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "targetType", Justification = "For Unity compatibility" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "elementType", Justification = "For Unity compatibility" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "options", Justification = "For Unity compatibility" )]
		private static MethodInfo GetCountGetterMethod( Type targetType, Type elementType, CollectionTraitOptions options )
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

#if !NET35 && !UNITY && !NET40 && !( SILVERLIGHT && !WINDOWS_PHONE )
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

		private static MethodInfo GetAddMethod( Type targetType, Type keyType, Type valueType, CollectionTraitOptions options )
		{
			if ( ( options | CollectionTraitOptions.WithAddMethod ) == 0 )
			{
				return null;
			}

			var argumentTypes = new[] { keyType, valueType };
			var result = targetType.GetMethod( "Add", argumentTypes );
			if ( result != null )
			{
				return result;
			}

			return typeof( IDictionary<,> ).MakeGenericType( argumentTypes ).GetMethod( "Add", argumentTypes );
		}

		private static bool FilterCollectionType( Type type, object filterCriteria )
		{
#if !NETSTANDARD1_1 && !NETSTANDARD1_3
#if DEBUG
			Contract.Assert( type.GetIsInterface(), "type.IsInterface" );
#endif // DEBUG
			return type.GetAssembly().Equals( typeof( Array ).GetAssembly() ) && ( type.Namespace == "System.Collections" || type.Namespace == "System.Collections.Generic" );
#else
			var typeInfo = type.GetTypeInfo();
			Contract.Assert( typeInfo.IsInterface );
			return typeInfo.Assembly.Equals( typeof( Array ).GetTypeInfo().Assembly ) && ( type.Namespace == "System.Collections" || type.Namespace == "System.Collections.Generic" );
#endif // !NETSTANDARD1_1 && !NETSTANDARD1_3
		}

		private static bool IsIEnumeratorT( Type @interface )
		{
			return @interface.GetIsGenericType() && @interface.GetGenericTypeDefinition() == typeof( IEnumerator<> );
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

		public static bool GetHasPublicGetter( this MemberInfo source )
		{
			PropertyInfo asProperty;
			FieldInfo asField;
			if ( ( asProperty = source as PropertyInfo ) != null )
			{
#if !NETSTANDARD1_1 && !NETSTANDARD1_3
				return asProperty.GetGetMethod() != null;
#else
				return ( asProperty.GetMethod != null && asProperty.GetMethod.IsPublic );
#endif // !NETSTANDARD1_1 && !NETSTANDARD1_3
			}
			else if ( ( asField = source as FieldInfo ) != null )
			{
				return asField.IsPublic;
			}
			else
			{
				throw new NotSupportedException( source.GetType() + " is not supported." );
			}
		}

		public static bool GetHasPublicSetter( this MemberInfo source )
		{
			PropertyInfo asProperty;
			FieldInfo asField;
			if ( ( asProperty = source as PropertyInfo ) != null )
			{
#if !NETSTANDARD1_1 && !NETSTANDARD1_3
				return asProperty.GetSetMethod() != null;
#else
				return ( asProperty.SetMethod != null && asProperty.SetMethod.IsPublic );
#endif // !NETSTANDARD1_1 && !NETSTANDARD1_3
			}
			else if ( ( asField = source as FieldInfo ) != null )
			{
				return asField.IsPublic && !asField.IsInitOnly && !asField.IsLiteral;
			}
			else
			{
				throw new NotSupportedException( source.GetType() + " is not supported." );
			}
		}

		public static bool GetIsPublic( this MemberInfo source )
		{
			PropertyInfo asProperty;
			FieldInfo asField;
			MethodBase asMethod;
#if !NETSTANDARD1_1 && !NETSTANDARD1_3
			Type asType;
#endif // !NETSTANDARD1_1 && !NETSTANDARD1_3
			if ( ( asProperty = source as PropertyInfo ) != null )
			{
#if !NETSTANDARD1_1 && !NETSTANDARD1_3
				return asProperty.GetAccessors( true ).Where( a => a.ReturnType != typeof( void ) ).All( a => a.IsPublic );
#else
				return
					( asProperty.GetMethod == null || asProperty.GetMethod.IsPublic );
#endif // !NETSTANDARD1_1 && !NETSTANDARD1_3
			}
			else if ( ( asField = source as FieldInfo ) != null )
			{
				return asField.IsPublic;
			}
			else if ( ( asMethod = source as MethodBase ) != null )
			{
				return asMethod.IsPublic;
			}
#if !NETSTANDARD1_1 && !NETSTANDARD1_3
			else if ( ( asType = source as Type ) != null )
			{
				return asType.IsPublic;
			}
#endif // !NETSTANDARD1_1 && !NETSTANDARD1_3
			else
			{
				throw new NotSupportedException( source.GetType() + " is not supported." );
			}
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
#if !NET40 && !( SILVERLIGHT && !WINDOWS_PHONE )
			internal Type IReadOnlyCollectionT;
			internal Type IReadOnlyListT;
			internal Type IReadOnlyDictionaryT;
#endif // !NET40 && !( SILVERLIGHT && !WINDOWS_PHONE )
#endif // !NET35 && !UNITY
			// ReSharper restore InconsistentNaming
		}
	}
}
