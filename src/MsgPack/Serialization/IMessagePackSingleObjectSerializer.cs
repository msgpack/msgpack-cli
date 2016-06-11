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

namespace MsgPack.Serialization
{
	/// <summary>
	///		Defines non-generic message pack serializer interface for byte array which contains a single object.
	/// </summary>
	[Obsolete( "Use MessagePackSerializer abstract class instead." )]
	public interface IMessagePackSingleObjectSerializer : IMessagePackSerializer
	{

		/// <summary>
		///		Serialize specified object to the array of <see cref="Byte"/>.
		/// </summary>
		/// <param name="objectTree">Object to be serialized.</param>
		/// <returns>An array of <see cref="Byte"/> which stores serialized value.</returns>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Failed to serialize object.
		/// </exception>
		/// <exception cref="NotSupportedException">
		///		The type of <paramref name="objectTree"/> is not serializable even if it can be deserialized.
		/// </exception>
		/// <seealso cref="MessagePackSerializer.Capabilities"/>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "object", Justification = "'objectTree' does not mean System.Object." )]
		byte[] PackSingleObject( object objectTree );

		/// <summary>
		///		Deserialize a single object from the array of <see cref="Byte"/> which contains a serialized object.
		/// </summary>
		/// <param name="buffer">An array of <see cref="Byte"/> serialized value to be stored.</param>
		/// <returns>The deserialized object.</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="buffer"/> is <c>null</c>.
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
		/// <remarks>
		///		<para>
		///			This method assumes that <paramref name="buffer"/> contains single serialized object dedicatedly,
		///			so this method does not return any information related to actual consumed bytes.
		///		</para>
		///		<para>
		///			This method is a counter part of <see cref="PackSingleObject"/>.
		///		</para>
		/// </remarks>	
		object UnpackSingleObject( byte[] buffer );
	}
}
