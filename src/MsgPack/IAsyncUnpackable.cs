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

namespace MsgPack
{
	/// <summary>
	///		An interface which is implemented by the objects which know how to unpack themselves using specified <see cref="Unpacker"/> asynchronously.
	/// </summary>
	/// <remarks>
	///		<include file='remarks.xml' path='doc/para[@name="MsgPack.Serialization.customPackingUnpacking"]'/>
	/// </remarks>
	public interface IAsyncUnpackable
	{
		/// <summary>
		///		Unpacks this object contents from the specified <see cref="Unpacker"/> asynchronously.
		/// </summary>
		/// <param name="unpacker">The <see cref="Unpacker"/> that this object will read from.</param>
		/// <param name="cancellationToken">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="unpacker"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Failed to deserialize this object.
		/// </exception>
		/// <include file='remarks.xml' path='doc/remarks[@name="MsgPack.Serialization.customUnpacking"]'/>
		Task UnpackFromMessageAsync( Unpacker unpacker, CancellationToken cancellationToken );
	}
}

#endif // FEATURE_TAP
