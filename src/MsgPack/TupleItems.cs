#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2015 FUJIWARA, Yusuke
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
#if !UNITY
#if XAMIOS || XAMDROID
using Contract = MsgPack.MPContract;
#else
using System.Diagnostics.Contracts;
#endif // XAMIOS || XAMDROID
#endif // !UNITY
using System.Linq;
#if NETFX_CORE
using System.Reflection;
#endif

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
		/// <param name="itemTypes">The type list of tuple items, in order.</param>
		/// <returns>
		///		The type list for nested tuples.
		///		The order is from outer to inner.
		/// </returns>
		public static List<Type> CreateTupleTypeList( IList<Type> itemTypes )
		{
			var itemTypesStack = new Stack<List<Type>>( itemTypes.Count / 7 + 1 );
			for ( int i = 0; i < itemTypes.Count / 7; i++ )
			{
				itemTypesStack.Push( itemTypes.Skip( i * 7 ).Take( 7 ).ToList() );
			}

			if ( itemTypes.Count % 7 != 0 )
			{
				itemTypesStack.Push( itemTypes.Skip( ( itemTypes.Count / 7 ) * 7 ).Take( itemTypes.Count % 7 ).ToList() );
			}

			var result = new List<Type>( itemTypesStack.Count );
			while ( 0 < itemTypesStack.Count )
			{
				var itemTypesStackEntry = itemTypesStack.Pop();
				if ( 0 < result.Count )
				{
					itemTypesStackEntry.Add( result.Last() );
				}

				var tupleType = Type.GetType( "System.Tuple`" + itemTypesStackEntry.Count, true ).MakeGenericType( itemTypesStackEntry.ToArray() );
				result.Add( tupleType );
			}

			result.Reverse();
			return result;
		}

		public static IList<Type> GetTupleItemTypes( Type tupleType )
		{
#if DEBUG
			Contract.Assert( tupleType.Name.StartsWith( "Tuple`" ) && tupleType.GetAssembly().Equals( typeof( Tuple ).GetAssembly() ), "tupleType.Name.StartsWith( \"Tuple`\" ) && tupleType.GetAssembly().Equals( typeof( Tuple ).GetAssembly() )" );
#endif // DEBUG
			var arguments = tupleType.GetGenericArguments();
			List<Type> itemTypes = new List<Type>( tupleType.GetGenericArguments().Length );
			GetTupleItemTypes( arguments, itemTypes );
			return itemTypes;
		}

		private static void GetTupleItemTypes( IList<Type> itemTypes, IList<Type> result )
		{
			int count = itemTypes.Count == 8 ? 7 : itemTypes.Count;
			for ( int i = 0; i < count; i++ )
			{
				result.Add( itemTypes[ i ] );
			}

			if ( itemTypes.Count == 8 )
			{
				var trest = itemTypes[ 7 ];
#if !NETFX_CORE
#if DEBUG
				Contract.Assert( trest.Name.StartsWith( "Tuple`" ) && trest.Assembly == typeof( Tuple ).Assembly, "trest.Name.StartsWith( \"Tuple`\" ) && trest.Assembly == typeof( Tuple ).Assembly" );
#endif // DEBUG
				GetTupleItemTypes( trest.GetGenericArguments(), result );
#else
				Contract.Assert( trest.Name.StartsWith( "Tuple`" ) && trest.GetTypeInfo().Assembly.Equals( typeof( Tuple ).GetTypeInfo().Assembly ) );
				GetTupleItemTypes( trest.GenericTypeArguments, result );
#endif
			}
		}

		public static bool IsTuple( Type type )
		{
			var assembly = type.GetAssembly();
			return
				( assembly.Equals( typeof( object ).GetAssembly() ) ||
				assembly.Equals( typeof( Enumerable ).GetAssembly() ) )
				&& type.GetIsPublic() &&
				type.Name.StartsWith( "Tuple`", StringComparison.Ordinal );
		}
	}
}
#endif // !UNITY
