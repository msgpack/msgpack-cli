#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2014 FUJIWARA, Yusuke
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

namespace MsgPack
{
	// This file generated from Packer.Nullable.tt T4Template.
	// Do not modify this file. Edit Packer.Nullable.tt instead.

	partial class Packer
	{
		/// <summary>
		///		Pack nullable <see cref="SByte"/> value.
		/// </summary>
		/// <param name="value">Value to serialize.</param>
		/// <returns>This packer instance.</returns>
#if !UNITY
		[CLSCompliant( false )]
#endif // !UNITY
		public Packer Pack( SByte? value )
		{
			return value.HasValue ? this.Pack( value.Value ) : this.PackNull();
		}
		/// <summary>
		///		Pack nullable <see cref="Byte"/> value.
		/// </summary>
		/// <param name="value">Value to serialize.</param>
		/// <returns>This packer instance.</returns>
		public Packer Pack( Byte? value )
		{
			return value.HasValue ? this.Pack( value.Value ) : this.PackNull();
		}
		/// <summary>
		///		Pack nullable <see cref="Int16"/> value.
		/// </summary>
		/// <param name="value">Value to serialize.</param>
		/// <returns>This packer instance.</returns>
		public Packer Pack( Int16? value )
		{
			return value.HasValue ? this.Pack( value.Value ) : this.PackNull();
		}
		/// <summary>
		///		Pack nullable <see cref="UInt16"/> value.
		/// </summary>
		/// <param name="value">Value to serialize.</param>
		/// <returns>This packer instance.</returns>
#if !UNITY
		[CLSCompliant( false )]
#endif // !UNITY
		public Packer Pack( UInt16? value )
		{
			return value.HasValue ? this.Pack( value.Value ) : this.PackNull();
		}
		/// <summary>
		///		Pack nullable <see cref="Int32"/> value.
		/// </summary>
		/// <param name="value">Value to serialize.</param>
		/// <returns>This packer instance.</returns>
		public Packer Pack( Int32? value )
		{
			return value.HasValue ? this.Pack( value.Value ) : this.PackNull();
		}
		/// <summary>
		///		Pack nullable <see cref="UInt32"/> value.
		/// </summary>
		/// <param name="value">Value to serialize.</param>
		/// <returns>This packer instance.</returns>
#if !UNITY
		[CLSCompliant( false )]
#endif // !UNITY
		public Packer Pack( UInt32? value )
		{
			return value.HasValue ? this.Pack( value.Value ) : this.PackNull();
		}
		/// <summary>
		///		Pack nullable <see cref="Int64"/> value.
		/// </summary>
		/// <param name="value">Value to serialize.</param>
		/// <returns>This packer instance.</returns>
		public Packer Pack( Int64? value )
		{
			return value.HasValue ? this.Pack( value.Value ) : this.PackNull();
		}
		/// <summary>
		///		Pack nullable <see cref="UInt64"/> value.
		/// </summary>
		/// <param name="value">Value to serialize.</param>
		/// <returns>This packer instance.</returns>
#if !UNITY
		[CLSCompliant( false )]
#endif // !UNITY
		public Packer Pack( UInt64? value )
		{
			return value.HasValue ? this.Pack( value.Value ) : this.PackNull();
		}
		/// <summary>
		///		Pack nullable <see cref="Single"/> value.
		/// </summary>
		/// <param name="value">Value to serialize.</param>
		/// <returns>This packer instance.</returns>
		public Packer Pack( Single? value )
		{
			return value.HasValue ? this.Pack( value.Value ) : this.PackNull();
		}
		/// <summary>
		///		Pack nullable <see cref="Double"/> value.
		/// </summary>
		/// <param name="value">Value to serialize.</param>
		/// <returns>This packer instance.</returns>
		public Packer Pack( Double? value )
		{
			return value.HasValue ? this.Pack( value.Value ) : this.PackNull();
		}
		/// <summary>
		///		Pack nullable <see cref="Boolean"/> value.
		/// </summary>
		/// <param name="value">Value to serialize.</param>
		/// <returns>This packer instance.</returns>
		public Packer Pack( Boolean? value )
		{
			return value.HasValue ? this.Pack( value.Value ) : this.PackNull();
		}
	}
}