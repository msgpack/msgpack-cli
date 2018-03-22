#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2015-2018 FUJIWARA, Yusuke
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

#if UNITY_5 || UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_WII || UNITY_IPHONE || UNITY_ANDROID || UNITY_PS3 || UNITY_XBOX360 || UNITY_FLASH || UNITY_BKACKBERRY || UNITY_WINRT
#define UNITY
#endif

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

using MsgPack.Serialization.Reflection;

namespace MsgPack.Serialization
{
#if UNITY && DEBUG
	public
#else
	internal
#endif
	static partial class AotHelper
	{
		public static void HandleAotError( Type mayBeGenericArgument, Exception mayBeAotError )
		{
			if ( mayBeGenericArgument == null )
			{
				return;
			}

			Exception targetException;
			TargetInvocationException targetInvocationException;
			if ( ( targetInvocationException = mayBeAotError as TargetInvocationException ) != null )
			{
				targetException = targetInvocationException.InnerException;
			}
			else
			{
				targetException = mayBeAotError;
			}

			if ( targetException is ExecutionEngineException )
			{
				string api = null;
				if ( mayBeGenericArgument.GetIsGenericType() )
				{
					var definition = mayBeGenericArgument.GetGenericTypeDefinition();
					if ( definition == typeof( ArraySegment<> )
#if MSGPACK_UNITY_FULL
						|| definition == typeof( Stack<> )
						|| definition == typeof( Queue<> )
#endif// MSGPACK_UNITY_FULL
					)
					{
						api = String.Format( CultureInfo.InvariantCulture, "MessagePackSerializer.PrepareCollectionType<{0}>", mayBeGenericArgument.GetGenericArguments()[ 0 ].GetFullName() );
					}
					else if ( definition == typeof( KeyValuePair<,> ) )
					{
						var genericArguments = mayBeGenericArgument.GetGenericArguments();
						api = String.Format( CultureInfo.InvariantCulture, "MessagePackSerializer.PrepareDictionaryType<{0}, {1}>", genericArguments[ 0 ].GetFullName(), genericArguments[ 1 ].GetFullName() );
					}
				}

				if ( api == null )
				{
					api = String.Format( CultureInfo.InvariantCulture, "MessagePackSerializer.PrepareType<{0}>", mayBeGenericArgument.GetFullName() );
				}

				throw new InvalidOperationException(
					String.Format(
						CultureInfo.CurrentCulture,
						"An AOT error is occurred. {0} is should be called in advance.",
						api
					),
					mayBeAotError
				);
			}

		}

		private static readonly Dictionary<RuntimeTypeHandle, object> EqualityComparerTable =
			InitializeEqualityComparerTable();

		public static object CreateSystemCollectionsGenericDictionary( ConstructorInfo constructor, Type keyType, int initialCapacity )
		{
			return constructor.InvokePreservingExceptionType( initialCapacity, GetEqualityComparer( keyType ) );
		}

		public static IEqualityComparer<T> GetEqualityComparer<T>()
		{
			return ( IEqualityComparer<T> ) GetEqualityComparer( typeof( T ) );
		}

		public static object GetEqualityComparer( Type type )
		{
			lock ( EqualityComparerTable )
			{
				object result;
				if ( !EqualityComparerTable.TryGetValue( type.TypeHandle, out result ) )
				{
					result =
						ReflectionExtensions.CreateInstancePreservingExceptionType(
							typeof( BoxingGenericEqualityComparer<> ).MakeGenericType( type )
						);
					EqualityComparerTable[ type.TypeHandle ] = result;
				}

				return result;
			}
		}

		public static void PrepareEqualityComparer<T>()
		{
			lock ( EqualityComparerTable )
			{
				object result;
				if ( !EqualityComparerTable.TryGetValue( typeof( T ).TypeHandle, out result ) )
				{
					result = new BoxingGenericEqualityComparer<T>();
					EqualityComparerTable[ typeof( T ).TypeHandle ] = result;
				}
			}
		}

		private sealed class BoxingGenericEqualityComparer<T> : IEqualityComparer<T>
		{
			public BoxingGenericEqualityComparer() {}

			public bool Equals( T x, T y )
			{
				if ( ReferenceEquals( x, y ) )
				{
					return true;
				}

				if ( x == null )
				{
					return false;
				}

				return x.Equals( y );
			}

			public int GetHashCode( T obj )
			{
				return obj == null ? 0 : obj.GetHashCode();
			}
		}

	}
}
