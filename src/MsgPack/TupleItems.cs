#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2017 FUJIWARA, Yusuke
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

#if !UNITY
using System;
using System.Collections.Generic;
#if FEATURE_MPCONTRACT
using Contract = MsgPack.MPContract;
#else
using System.Diagnostics.Contracts;
#endif // FEATURE_MPCONTRACT
using System.Linq;

namespace MsgPack
{
	/// <summary>
	///		Defines helper method for items of tuple type.
	/// </summary>
	internal static class TupleItems
	{
		/// <summary>
		///		Creates type list for nested tuples.
		/// </summary>
		/// <param name="rootTupleType">The type of base tuple.</param>
		/// <returns>
		///		The type list for nested tuples.
		///		The order is from outer to inner.
		/// </returns>
		public static List<Type> CreateTupleTypeList( Type rootTupleType )
		{
			if ( !rootTupleType.GetIsGenericType() )
			{
				// arity 0 value tuple
				return new List<Type>( 1 ) { rootTupleType };
			}

			var assembly = rootTupleType.GetAssembly();
			var baseName = rootTupleType.FullName.Remove( rootTupleType.FullName.IndexOf( '`' ) + 1 );
			var result = new List<Type>();
			var tupleType = rootTupleType;
			while ( true )
			{
				result.Add( tupleType );
				if ( !tupleType.GetIsGenericType() )
				{
					// arity 0
					break;
				}

				var itemTypes = tupleType.GetGenericArguments();
				if ( itemTypes.Length < 8 )
				{
					// leaf tuple
					break;
				}

				tupleType = itemTypes.Last();
			}

			return result;
		}

		public static IList<Type> GetTupleItemTypes( Type tupleType )
		{
#if DEBUG
			Contract.Assert( IsTuple( tupleType ), "IsTuple( "+ tupleType.AssemblyQualifiedName + " )" );
#endif // DEBUG
			var arguments = tupleType.GetGenericArguments();
			var itemTypes = new List<Type>( tupleType.GetGenericArguments().Length );
			GetTupleItemTypes( arguments, itemTypes );
			return itemTypes;
		}

		private static void GetTupleItemTypes( IList<Type> itemTypes, IList<Type> result )
		{
			var count = itemTypes.Count == 8 ? 7 : itemTypes.Count;
			for ( var i = 0; i < count; i++ )
			{
				result.Add( itemTypes[ i ] );
			}

			if ( itemTypes.Count == 8 )
			{
				var trest = itemTypes[ 7 ];
#if DEBUG
				Contract.Assert( IsTuple( trest ), "IsTuple( " + trest.AssemblyQualifiedName + " )" );
#endif // DEBUG
				// Put nested tuple's item types recursively.
				GetTupleItemTypes( trest.GetGenericArguments(), result );
			}
		}

		public static bool IsTuple( Type type )
		{
			return
				type.GetIsPublic()
				&& ( ( type.FullName.StartsWith( "System.ValueTuple`", StringComparison.Ordinal ) && type.GetIsValueType() )
					|| ( type.FullName.StartsWith( "System.Tuple`", StringComparison.Ordinal ) && !type.GetIsValueType() )
					|| ( type.FullName == "System.ValueTuple" && type.GetIsValueType() )
				);
		}
	}
}
#endif // !UNITY
