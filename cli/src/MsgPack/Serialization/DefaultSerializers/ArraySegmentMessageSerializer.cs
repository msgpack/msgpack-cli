#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010 FUJIWARA, Yusuke
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
using System.Linq;
using System.Reflection;

namespace MsgPack.Serialization.DefaultSerializers
{
	internal static class ArraySegmentMessageSerializer
	{
		public static readonly MethodInfo PackByteArraySegmentToMethod = 
			FromExpression.ToMethod( ( Packer packer, ArraySegment<byte> objectTree, SerializationContext context ) => PackByteArraySegmentTo( packer, objectTree, context ) );
		public static readonly MethodInfo PackCharArraySegmentToMethod =
			FromExpression.ToMethod( ( Packer packer, ArraySegment<char> objectTree, SerializationContext context ) => PackCharArraySegmentTo( packer, objectTree, context ) );
		public static readonly MethodInfo PackGenericArraySegmentTo1Method =
			typeof( ArraySegmentMessageSerializer ).GetMethod( "PackGenericArraySegmentTo" );
		public static readonly MethodInfo UnpackByteArraySegmentFromMethod =
			FromExpression.ToMethod( ( Unpacker unpacker, SerializationContext context ) => UnpackByteArraySegmentFrom( unpacker, context ) );
		public static readonly MethodInfo UnpackCharArraySegmentFromMethod =
			FromExpression.ToMethod( ( Unpacker unpacker, SerializationContext context ) => UnpackCharArraySegmentFrom( unpacker, context ) );
		public static readonly MethodInfo UnpackGenericArraySegmentFrom1Method =
			typeof( ArraySegmentMessageSerializer ).GetMethod( "UnpackGenericArraySegmentFrom" );

		public static void PackByteArraySegmentTo( Packer packer, ArraySegment<byte> objectTree, SerializationContext context )
		{
			packer.PackRawHeader( objectTree.Count );
			packer.PackRawBody( objectTree.Array.Skip( objectTree.Offset ).Take( objectTree.Count ) );
		}

		public static void PackCharArraySegmentTo( Packer packer, ArraySegment<char> objectTree, SerializationContext context )
		{
			// TODO: More efficient
			packer.PackRawHeader( objectTree.Count );
			packer.PackRawBody( MessagePackConvert.EncodeString( new string( objectTree.Array.Skip( objectTree.Offset ).Take( objectTree.Count ).ToArray() ) ) );
		}

		public static void PackGenericArraySegmentTo<T>( Packer packer, ArraySegment<T> objectTree, SerializationContext context )
		{
			packer.PackArrayHeader( objectTree.Count );
			for ( int i = 0; i < objectTree.Count; i++ )
			{
				context.MarshalTo( packer, objectTree.Array[ i + objectTree.Offset ] );
			}
		}

		public static ArraySegment<byte> UnpackByteArraySegmentFrom( Unpacker unpacker, SerializationContext context )
		{
			return new ArraySegment<byte>( unpacker.Data.Value.AsBinary() );
		}

		public static ArraySegment<char> UnpackCharArraySegmentFrom( Unpacker unpacker, SerializationContext context )
		{
			// TODO: More efficient
			return new ArraySegment<char>( unpacker.Data.Value.AsCharArray() );
		}

		public static ArraySegment<T> UnpackGenericArraySegmentFrom<T>( Unpacker unpacker, SerializationContext context )
		{
			T[] array = new T[ unpacker.ItemsCount ];
			context.UnmarshalArrayTo( unpacker, array );
			return new ArraySegment<T>( array );
		}
	}
}
