#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2016 FUJIWARA, Yusuke
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

namespace MsgPack
{
	/// <summary>
	///		An interface which is implemented by the objects which know how to pack themselves using specified <see cref="Packer"/>.
	/// </summary>
	/// <remarks>
	///		<include file='remarks.xml' path='doc/para[@name="MsgPack.Serialization.customPackingUnpacking"]'/>
	/// </remarks>
	public interface IPackable
	{
		/// <summary>
		///		Packs this object contents to the specified <see cref="Packer"/>.
		/// </summary>
		/// <param name="packer">The <see cref="Packer"/> that this object will write to.</param>
		/// <param name="options">Packing options. This value can be null.</param>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="packer"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="System.Runtime.Serialization.SerializationException">
		///		Failed to serialize this object.
		/// </exception>
		/// <include file='remarks.xml' path='doc/remarks[@name="MsgPack.Serialization.customPacking"]'/>
		void PackToMessage( Packer packer, PackingOptions options );
	}
}
