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

namespace MsgPack
{
	/// <summary>
	///		Specify option flags for <see cref="Packer.Pack(Object, PackingOptions)"/>.
	/// </summary>
	[Flags]
	public enum ObjectPackingOptions
	{
		/// <summary>
		///		None. <see cref="Packer"/> acts like Message-Pack Java.
		/// </summary>
		None = 0,

		/// <summary>
		///		<see cref="Packer"/> packs specified value as 'strict' type.
		///		For example, when it is not strict and you pass ulong value 1, <see cref="Packer"/> packs its value as positive fixed number 1;
		///		when it is strict and you pass same value, however, <see cref="Packer"/> packs its value as ulong number 1.
		/// </summary>
		Strict = 0x1,

		/// <summary>
		///		<see cref="Packer"/> recursively packs collection items which are not <see cref="MessagePackObject"/>.
		/// </summary>
		[Obsolete( "Use MessagePackFormatter instead.", true )]
		Recursive = 0x2
	}

	internal static class ObjectPackingOptionsExtension
	{
		// Since BCL HasFlag<TEnum> causes boxing, this method is still useful.
		public static bool Has( this ObjectPackingOptions source, ObjectPackingOptions value )
		{
			return ( source & value ) == value;
		}
	}
}
