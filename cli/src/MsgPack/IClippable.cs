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
using System.Collections.Generic;
namespace MsgPack.Collections
{
	/// <summary>
	///		Define interface which can clip segment of items.
	/// </summary>
	public interface IClippable<T>
	{
		/// <summary>
		///		Clip out specified range from this collection.
		/// </summary>
		/// <param name="offset">Start offset of items to be clipped.</param>
		/// <param name="length">Length of clipped.</param>
		/// <returns>Clipped items. These items no longer belong to this collection.</returns>
		/// <remarks>
		///		The contract of this method does not specify that whether returning collection is read only,
		///		or it is fixed size (such as aray).
		///		It depend on implementing type.
		/// </remarks>
		IList<T> Clip( long offset, long length );
	}
}
