#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2013 FUJIWARA, Yusuke
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
#endregion -- License Terms --

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using MsgPack.Serialization.Reflection;

namespace MsgPack.Serialization
{
	internal static class ReflectionExtensions
	{
		// TODO: Metadata
		private static readonly PropertyInfo _icollectionCount = FromExpression.ToProperty( ( ICollection value ) => value.Count );

		public static Type GetMemberValueType( this MemberInfo source )
		{
			Contract.Requires( source != null );

			PropertyInfo asProperty = source as PropertyInfo;
			FieldInfo asField = source as FieldInfo;
			return asProperty != null ? asProperty.PropertyType : asField.FieldType;
		}

		public static bool CanSetValue( this MemberInfo source )
		{
			PropertyInfo asProperty = source as PropertyInfo;
			FieldInfo asField = source as FieldInfo;
			// GetSetMethod() on WinRT is not compabitle with CLR. CLR returns null but WinRT returns non-null for non-public setter.
			return asProperty != null ? ( asProperty.CanWrite && asProperty.GetSetMethod() != null && asProperty.GetSetMethod().IsPublic ) : !asField.IsInitOnly;
		}

		public static CollectionTraits GetCollectionTraits( this Type source )
		{
			Contract.Assert( !source.GetContainsGenericParameters() );
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
						CollectionKind.Array,
						null, // Add() : Never used for array.
						null, // GetEnumerator() : Never used for array.
						null, // Count : Never used for array.
						source.GetElementType()
					);
			}

			var getEnumerator = source.GetMethod( "GetEnumerator", ReflectionAbstractions.EmptyTypes );
			if ( getEnumerator != null && getEnumerator.ReturnType.IsAssignableTo( typeof( IEnumerator ) ) )
			{
				// If public 'GetEnumerator' is found, it is primary collection traits.
				if ( source.Implements( typeof( IDictionary<,> ) ) )
				{
					var ienumetaorT = getEnumerator.ReturnType.GetInterfaces().FirstOrDefault( @interface => @interface.GetIsGenericType() && @interface.GetGenericTypeDefinition() == typeof( IEnumerator<> ) );
					if ( ienumetaorT != null )
					{
						var elementType = ienumetaorT.GetGenericArguments()[ 0 ];
						return
							new CollectionTraits(
								CollectionKind.Map,
								GetAddMethod( source, elementType.GetGenericArguments()[ 0 ], elementType.GetGenericArguments()[ 1 ] ),
								getEnumerator,
								GetDictionaryCountProperty( source, elementType.GetGenericArguments()[ 0 ], elementType.GetGenericArguments()[ 1 ] ),
								elementType
							);
					}
				}

				if ( source.IsAssignableTo( typeof( IDictionary ) ) )
				{
					return
						new CollectionTraits(
							CollectionKind.Map,
							GetAddMethod( source, typeof( object ), typeof( object ) ),
							getEnumerator,
							_icollectionCount,
							typeof( DictionaryEntry )
						);
				}

				// Block to limit variable scope
				{
					var ienumetaorT = getEnumerator.ReturnType.GetInterfaces().FirstOrDefault( @interface => @interface.GetIsGenericType() && @interface.GetGenericTypeDefinition() == typeof( IEnumerator<> ) );
					if ( ienumetaorT != null )
					{
						var elementType = ienumetaorT.GetGenericArguments()[ 0 ];
						return
							new CollectionTraits(
								CollectionKind.Array,
								GetAddMethod( source, elementType ),
								getEnumerator,
								null,
								elementType
							);
					}
				}
			}

			Type ienumerableT = null;
			Type idictionaryT = null;
			Type ienumerable = null;
			Type idictionary = null;
			foreach ( var type in source.FindInterfaces( FilterCollectionType, null ) )
			{
				if ( type == typeof( IDictionary<MessagePackObject, MessagePackObject> ) )
				{
					return
						new CollectionTraits(
							CollectionKind.Map,
							GetAddMethod( source, typeof( MessagePackObject ), typeof( MessagePackObject ) ),
							FindInterfaceMethod( source, typeof( IEnumerable<KeyValuePair<MessagePackObject, MessagePackObject>> ), "GetEnumerator", ReflectionAbstractions.EmptyTypes ),
							GetDictionaryCountProperty( source, typeof( MessagePackObject ), typeof( MessagePackObject ) ),
							typeof( KeyValuePair<MessagePackObject, MessagePackObject> )
						);
				}

				if ( type == typeof( IEnumerable<MessagePackObject> ) )
				{
					var addMethod = GetAddMethod( source, typeof( MessagePackObject ) );
					if ( addMethod != null )
					{
						return
							new CollectionTraits(
								CollectionKind.Array,
								addMethod,
								FindInterfaceMethod( source, typeof( IEnumerable<MessagePackObject> ), "GetEnumerator", ReflectionAbstractions.EmptyTypes ),
								GetCollectionTCountProperty( source, typeof( MessagePackObject ) ),
								typeof( MessagePackObject )
							);
					}
				}

				if ( type.GetIsGenericType() )
				{
					var genericTypeDefinition = type.GetGenericTypeDefinition();
					if ( genericTypeDefinition == typeof( IDictionary<,> ) )
					{
						if ( idictionaryT != null )
						{
							return CollectionTraits.Unserializable;
						}
						idictionaryT = type;
					}
					else if ( genericTypeDefinition == typeof( IEnumerable<> ) )
					{
						if ( ienumerableT != null )
						{
							return CollectionTraits.Unserializable;
						}
						ienumerableT = type;
					}
				}
				else
				{
					if ( type == typeof( IDictionary ) )
					{
						idictionary = type;
					}
					else if ( type == typeof( IEnumerable ) )
					{
						ienumerable = type;
					}
				}
			}

			if ( idictionaryT != null )
			{
				var elementType = typeof( KeyValuePair<,> ).MakeGenericType( idictionaryT.GetGenericArguments() );
				return
					new CollectionTraits(
						CollectionKind.Map,
						GetAddMethod( source, idictionaryT.GetGenericArguments()[ 0 ], idictionaryT.GetGenericArguments()[ 1 ] ),
						FindInterfaceMethod( source, typeof( IEnumerable<> ).MakeGenericType( elementType ), "GetEnumerator", ReflectionAbstractions.EmptyTypes ),
						GetDictionaryCountProperty( source, idictionaryT.GetGenericArguments()[ 0 ], idictionaryT.GetGenericArguments()[ 1 ] ),
						elementType
					);
			}

			if ( ienumerableT != null )
			{
				var elementType = ienumerableT.GetGenericArguments()[ 0 ];
				return
					new CollectionTraits(
						CollectionKind.Array,
						GetAddMethod( source, elementType ),
						FindInterfaceMethod( source, ienumerableT, "GetEnumerator", ReflectionAbstractions.EmptyTypes ),
						GetCollectionTCountProperty( source, elementType ),
						elementType
					);
			}

			if ( idictionary != null )
			{
				return
					new CollectionTraits(
						CollectionKind.Map,
						GetAddMethod( source, typeof( object ), typeof( object ) ),
						FindInterfaceMethod( source, idictionary, "GetEnumerator", ReflectionAbstractions.EmptyTypes ),
						_icollectionCount,
						typeof( object )
					);
			}

			if ( ienumerable != null )
			{
				var addMethod = GetAddMethod( source, typeof( object ) );
				if ( addMethod != null )
				{
					return
						new CollectionTraits(
							CollectionKind.Array,
							addMethod,
							FindInterfaceMethod( source, ienumerable, "GetEnumerator", ReflectionAbstractions.EmptyTypes ),
							_icollectionCount,
							typeof( object )
						);
				}
			}

			return CollectionTraits.NotCollection;
		}

		private static MethodInfo FindInterfaceMethod( Type targetType, Type interfaceType, string name, Type[] parameterTypes )
		{
			if ( targetType.GetIsInterface() )
			{
				return targetType.FindInterfaces( ( type, _ ) => type == interfaceType, null ).Single().GetMethod( name, parameterTypes );
			}

			var map = targetType.GetInterfaceMap( interfaceType );

#if !SILVERLIGHT
			int index = Array.FindIndex( map.InterfaceMethods, method => method.Name == name && method.GetParameters().Select( p => p.ParameterType ).SequenceEqual( parameterTypes ) );
#else
			int index = map.InterfaceMethods.FindIndex( method => method.Name == name && method.GetParameters().Select( p => p.ParameterType ).SequenceEqual( parameterTypes ) );
#endif

			if ( index < 0 )
			{
#if DEBUG
#if !NETFX_35
				Contract.Assert( false, interfaceType + "::" + name + "(" + String.Join<Type>( ", ", parameterTypes ) + ") is not found in " + targetType );
#else
				Contract.Assert( false, interfaceType + "::" + name + "(" + String.Join( ", ", parameterTypes.Select( t => t.ToString() ).ToArray() ) + ") is not found in " + targetType );
#endif
#endif
				return null;
			}
			else
			{
				return map.TargetMethods[ index ];
			}
		}

		private static PropertyInfo GetCollectionTCountProperty( Type targetType, Type elementType )
		{
			if ( !targetType.GetIsValueType() && targetType.Implements( typeof( ICollection<> ) ) )
			{
				return typeof( ICollection<> ).MakeGenericType( elementType ).GetProperty( "Count" );
			}

			var property = targetType.GetProperty( "Count" );
			if ( property != null && property.PropertyType == typeof( int ) && property.GetIndexParameters().Length == 0 )
			{
				return property;
			}

			return null;
		}

		private static PropertyInfo GetDictionaryCountProperty( Type targetType, Type keyType, Type valueType )
		{
			var property = targetType.GetProperty( "Count" );
			if ( property != null && property.PropertyType == typeof( int ) && property.GetIndexParameters().Length == 0 )
			{
				return property;
			}

			return
				typeof( ICollection<> ).MakeGenericType(
					typeof( KeyValuePair<,> ).MakeGenericType( keyType, valueType )
				).GetProperty( "Count" );
		}

		private static MethodInfo GetAddMethod( Type targetType, Type argumentType )
		{
			var argumentTypes = new[] { argumentType };
			var result = targetType.GetMethod( "Add", argumentTypes );
			if ( result != null )
			{
				return result;
			}

			var icollectionT = typeof( ICollection<> ).MakeGenericType( argumentType );
			if ( targetType.IsAssignableTo( icollectionT ) )
			{
				return icollectionT.GetMethod( "Add", argumentTypes );
			}

			if ( targetType.IsAssignableTo( typeof( IList ) ) )
			{
				return typeof( IList ).GetMethod( "Add", new[] { typeof( object ) } );
			}

			return null;
		}

		private static MethodInfo GetAddMethod( Type targetType, Type keyType, Type valueType )
		{
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
#if !NETFX_CORE
			Contract.Assert( type.IsInterface );
			return type.Assembly == typeof( Array ).Assembly && ( type.Namespace == "System.Collections" || type.Namespace == "System.Collections.Generic" );
#else
			var typeInfo = type.GetTypeInfo();
			Contract.Assert( typeInfo.IsInterface );
			return typeInfo.Assembly.Equals( typeof( Array ).GetTypeInfo().Assembly ) && ( type.Namespace == "System.Collections" || type.Namespace == "System.Collections.Generic" );
#endif
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
	}
}
