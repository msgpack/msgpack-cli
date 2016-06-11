#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2012 FUJIWARA, Yusuke
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

#if UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_WII || UNITY_IPHONE || UNITY_ANDROID || UNITY_PS3 || UNITY_XBOX360 || UNITY_FLASH || UNITY_BKACKBERRY || UNITY_WINRT
#define UNITY
#endif

using System;
using System.Diagnostics.CodeAnalysis;

namespace MsgPack.Serialization
{
	/// <summary>
	///		Defines non-generic message pack serializer interface.
	/// </summary>
	[Obsolete( "Use MessagePackSerializer abstract class instead." )]
	public interface IMessagePackSerializer
	{
		/// <summary>
		///		Serialize specified object with specified <see cref="Packer"/>.
		/// </summary>
		/// <param name="packer"><see cref="Packer"/> which packs values in <paramref name="objectTree"/>.</param>
		/// <param name="objectTree">Object to be serialized.</param>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="packer"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="ArgumentException">
		///		<paramref name="objectTree"/> is not compatible for this serializer.
		/// </exception>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Failed to serialize object.
		/// </exception>
		/// <exception cref="NotSupportedException">
		///		The type of <paramref name="objectTree"/> is not serializable even if it can be deserialized.
		/// </exception>
		/// <seealso cref="MessagePackSerializer.Capabilities"/>
		[SuppressMessage( "Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "object", Justification = "'objectTree' does not mean System.Object." )]
		void PackTo( Packer packer, object objectTree );

		/// <summary>
		///		Deserialize object with specified <see cref="Unpacker"/>.
		/// </summary>
		/// <param name="unpacker"><see cref="Unpacker"/> which unpacks values of resulting object tree.</param>
		/// <returns>The deserialized object.</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="unpacker"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Failed to deserialize object.
		/// </exception>
		/// <exception cref="MessageTypeException">
		///		Failed to deserialize object due to invalid stream.
		/// </exception>
		/// <exception cref="InvalidMessagePackStreamException">
		///		Failed to deserialize object due to invalid stream.
		/// </exception>
		/// <exception cref="NotSupportedException">
		///		The type of deserializing is not serializable even if it can be serialized.
		/// </exception>
		/// <seealso cref="MessagePackSerializer.Capabilities"/>
		object UnpackFrom( Unpacker unpacker );

		/// <summary>
		///		Deserialize collection items with specified <see cref="Unpacker"/> and stores them to <paramref name="collection"/>.
		/// </summary>
		/// <param name="unpacker"><see cref="Unpacker"/> which unpacks values of resulting object tree.</param>
		/// <param name="collection">Collection that the items to be stored.</param>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="unpacker"/> is <c>null</c>.
		///		Or <paramref name="collection"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="ArgumentException">
		///		<paramref name="collection"/> is not compatible for this serializer.
		/// </exception>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Failed to deserialize object.
		/// </exception>
		/// <exception cref="MessageTypeException">
		///		Failed to deserialize object due to invalid stream.
		/// </exception>
		/// <exception cref="InvalidMessagePackStreamException">
		///		Failed to deserialize object due to invalid stream.
		/// </exception>
		/// <exception cref="NotSupportedException">
		///		The type of deserializing is not mutable collection.
		/// </exception>
		/// <seealso cref="MessagePackSerializer.Capabilities"/>
		void UnpackTo( Unpacker unpacker, object collection );
	}
}
