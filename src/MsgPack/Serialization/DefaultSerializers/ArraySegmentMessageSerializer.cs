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

using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
#if UNITY
using System.Reflection;
#endif // UNITY

namespace MsgPack.Serialization.DefaultSerializers
{
	internal static class ArraySegmentMessageSerializer
	{
		[SuppressMessage( "Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "itemSerializer", Justification = "For Delegate signature compatibility" )]
#if !UNITY
		public static void PackByteArraySegmentTo( Packer packer, ArraySegment<byte> objectTree, MessagePackSerializer<byte> itemSerializer )
		{
#else
		public static void PackByteArraySegmentTo( Packer packer, object obj, IMessagePackSingleObjectSerializer itemSerializer )
		{
			var objectTree = ( ArraySegment<byte> )obj;
#endif // !UNITY
			if ( objectTree.Array == null )
			{
				packer.PackBinaryHeader( 0 );
				return;
			}

			packer.PackBinaryHeader( objectTree.Count );
			packer.PackRawBody( objectTree.Array.Skip( objectTree.Offset ).Take( objectTree.Count ) );
		}

		[SuppressMessage( "Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "itemSerializer", Justification = "For Delegate signature compatibility" )]
#if !UNITY
		public static void PackCharArraySegmentTo( Packer packer, ArraySegment<char> objectTree, MessagePackSerializer<char> itemSerializer )
		{
#else
		public static void PackCharArraySegmentTo( Packer packer, object obj, IMessagePackSingleObjectSerializer itemSerializer )
		{
			var objectTree = ( ArraySegment<char> )obj;
#endif // !UNITY
			// TODO: More efficient
			packer.PackStringHeader( objectTree.Count );
			packer.PackRawBody( MessagePackConvert.EncodeString( new string( objectTree.Array.Skip( objectTree.Offset ).Take( objectTree.Count ).ToArray() ) ) );
		}

#if !UNITY
		public static void PackGenericArraySegmentTo<T>( Packer packer, ArraySegment<T> objectTree, MessagePackSerializer<T> itemSerializer )
		{
			packer.PackArrayHeader( objectTree.Count );
			for ( int i = 0; i < objectTree.Count; i++ )
			{
				itemSerializer.PackTo( packer, objectTree.Array[ i + objectTree.Offset ] );
			}
		}
#else
		public static void PackGenericArraySegmentTo( Packer packer, object objectTree, IMessagePackSingleObjectSerializer itemSerializer )
		{
			var count = ( int )objectTree.GetType().GetProperty( "Count" ).GetGetMethod().InvokePreservingExceptionType( objectTree );
			var offset = ( int )objectTree.GetType().GetProperty( "Offset" ).GetGetMethod().InvokePreservingExceptionType( objectTree );
			var array = objectTree.GetType().GetProperty( "Array" ).GetGetMethod().InvokePreservingExceptionType( objectTree ) as Array;
			packer.PackArrayHeader( count );
			for ( int i = 0; i < count; i++ )
			{
				itemSerializer.PackTo( packer, array.GetValue( i + offset ) );
			}
		}
#endif // !UNITY


		[SuppressMessage( "Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "itemSerializer", Justification = "For Delegate signature compatibility" )]
#if !UNITY
		public static ArraySegment<byte> UnpackByteArraySegmentFrom( Unpacker unpacker, MessagePackSerializer<byte> itemSerializer )
#else
		public static object UnpackByteArraySegmentFrom( Unpacker unpacker, Type elementType, IMessagePackSingleObjectSerializer itemSerializer )
#endif // !UNITY
		{
			return new ArraySegment<byte>( unpacker.LastReadData.AsBinary() );
		}

		[SuppressMessage( "Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "itemSerializer", Justification = "For Delegate signature compatibility" )]
#if !UNITY
		public static ArraySegment<char> UnpackCharArraySegmentFrom( Unpacker unpacker, MessagePackSerializer<char> itemSerializer )
#else
		public static object UnpackCharArraySegmentFrom( Unpacker unpacker, Type elementType, IMessagePackSingleObjectSerializer itemSerializer )
#endif // !UNITY
		{
			// TODO: More efficient
			return new ArraySegment<char>( unpacker.LastReadData.AsCharArray() );
		}

#if !UNITY
		public static ArraySegment<T> UnpackGenericArraySegmentFrom<T>( Unpacker unpacker, MessagePackSerializer<T> itemSerializer )
		{
			T[] array = new T[ unpacker.ItemsCount ];
			for ( int i = 0; i < array.Length; i++ )
			{
				if ( !unpacker.Read() )
				{
					throw SerializationExceptions.NewMissingItem( i );
				}

				array[ i ] = itemSerializer.UnpackFrom( unpacker );
			}

			return new ArraySegment<T>( array );
		}
#else
		public static object UnpackGenericArraySegmentFrom( Unpacker unpacker, Type elementType, IMessagePackSingleObjectSerializer itemSerializer )
		{
			Array array = Array.CreateInstance( elementType, unpacker.ItemsCount );
			for ( int i = 0; i < array.Length; i++ )
			{
				if ( !unpacker.Read() )
				{
					throw SerializationExceptions.NewMissingItem( i );
				}

				array.SetValue( itemSerializer.UnpackFrom( unpacker ), i );
			}

			return
				ReflectionExtensions.CreateInstancePreservingExceptionType(
					typeof( ArraySegment<> ).MakeGenericType( elementType ),
					array
				);
		}
#endif // !UNITY
	}
}
