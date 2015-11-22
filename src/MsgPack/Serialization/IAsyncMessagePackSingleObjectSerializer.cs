#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2015 FUJIWARA, Yusuke
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

#if FEATURE_TAP

using System;
using System.Threading;
using System.Threading.Tasks;

namespace MsgPack.Serialization
{
	/// <summary>
	///		Defines common intarfaces for single object serializers which has new capability API and asynchronous serialization.
	/// </summary>
	public interface IAsyncMessagePackSingleObjectSerializer : IAsyncMessagePackSerializer, IMessagePackSingleObjectSerializer
	{
		/// <summary>
		///		Serialize specified object to the array of <see cref="Byte"/>.
		/// </summary>
		/// <param name="objectTree">Object to be serialized.</param>
		/// <returns>An array of <see cref="Byte"/> which stores serialized value.</returns>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>
		///		A <see cref="Task"/> that represents the asynchronous operation. 
		///		The value of the <c>TResult</c> parameter contains an array of <see cref="Byte"/> which stores serialized value.
		/// </returns>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Failed to serialize <paramref name="objectTree"/>.
		/// </exception>
		/// <exception cref="NotSupportedException">
		///		The type of <paramref name="objectTree"/> is not serializable even if it can be deserialized.
		/// </exception>
		/// <seealso cref="ISupportMessagePackSerializerCapability.Capabilities"/>
		Task<byte[]> PackSingleObjectAsync( object objectTree, CancellationToken cancellationToken );

		/// <summary>
		///		Deserialize a single object from the array of <see cref="Byte"/> which contains a serialized object.
		/// </summary>
		/// <param name="buffer">An array of <see cref="Byte"/> serialized value to be stored.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>
		///		A <see cref="Task"/> that represents the asynchronous operation. 
		///		The value of the <c>TResult</c> parameter contains the deserialized object.
		/// </returns>
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
		/// <seealso cref="ISupportMessagePackSerializerCapability.Capabilities"/>
		/// <remarks>
		///		<para>
		///			This method assumes that <paramref name="buffer"/> contains single serialized object dedicatedly,
		///			so this method does not return any information related to actual consumed bytes.
		///		</para>
		///		<para>
		///			This method is a counter part of <see cref="PackSingleObjectAsync"/>.
		///		</para>
		/// </remarks>	
		Task<object> UnpackSingleObjectAsync( byte[] buffer, CancellationToken cancellationToken );
	}
}

#endif // FEATURE_TAP
