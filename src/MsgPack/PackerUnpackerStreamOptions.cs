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

using System;
using System.IO;

namespace MsgPack
{
	/// <summary>
	///		Represents options for stream on <see cref="Packer"/>/<see cref="Unpacker"/> creation.
	/// </summary>
	public sealed class PackerUnpackerStreamOptions
	{
		internal static readonly PackerUnpackerStreamOptions SingletonOwnsStream =
			new PackerUnpackerStreamOptions { OwnsStream = true };

		internal static readonly PackerUnpackerStreamOptions SingletonForAsync =
			new PackerUnpackerStreamOptions { OwnsStream = true, WithBuffering = true };

		internal static readonly PackerUnpackerStreamOptions None = new PackerUnpackerStreamOptions();

		/// <summary>
		///		Gets or sets a value indicating whether stream should be wrapped with buffering stream.
		/// </summary>
		/// <value>
		///   <c>true</c> if stream should be wrapped with buffering stream; otherwise, <c>false</c>.
		/// </value>
		/// <remarks>
		///		<para>
		///			This option is important to improve asynchronous operation performance because deserialization tend to be chatty,
		///			so many tiny asynchrnous operation are issued and then numerous context switching may occurred.
		///			Wrapping with buffering stream mitigate context switching because it should avoid asynchronous operation
		///			as long as it has buffered value.
		///		</para>
		///		<para>
		///			Current built-in <see cref="Unpacker"/> implementation uses <see cref="BufferedStream"/> for buffering,
		///			and avoid buffering for following in-memory or stream with buffering feature:
		///			<list type="bullet">
		///				<item><see cref="System.IO.BufferedStream" /> itself.</item>
		///				<item><see cref="System.IO.MemoryStream" />.</item>
		///				<item><see cref="System.IO.UnmanagedMemoryStream" />.</item>
		///				<item><see cref="System.IO.FileStream" /> which has own internal buffer.</item>
		///			</list>
		///		</para>
		///		<para>
		///			Logically, it is preferred that you should wrap with <see cref="System.IO.BufferedStream"/> yourself for underlying stream
		///			for wrapper stream such as <seealso cref="System.IO.Compression.DeflateStream"/>, <see cref="System.Security.Cryptography.CryptoStream"/>, etc.
		///		</para>
		/// </remarks>
		/// <seealso cref="BufferSize"/>
		public bool WithBuffering { get; set; }

		private int _bufferSize = 64 * 1024;

		/// <summary>
		///		Gets or sets the size of the buffer of wrapping stream in bytes used when <see cref="WithBuffering"/> is <c>true</c>.
		/// </summary>
		/// <value>
		///		The size of the buffer of wrapping stream in bytes used when <see cref="WithBuffering"/> is <c>true</c>.
		///		The default is 64K.
		///		If you attempt to set 0 or negative value, then the value will be set to 1.
		/// </value>
		public int BufferSize
		{
			get { return this._bufferSize; }
			set { this._bufferSize = ( value < 0 ) ? 1 : value; }
		}

		/// <summary>
		///		Gets or sets a value indicating whether <see cref="Packer"/>/<see cref="Unpacker" /> will dispose underlying stream when their <c>Dispose(Boolean)</c> method are called with <c>true</c> value.
		/// </summary>
		/// <value>
		///		<c>true</c> if <see cref="Packer"/>/<see cref="Unpacker" /> will dispose underlying stream when their <c>Dispose(Boolean)</c> method are called with <c>true</c> value; 
		///		otherwise, <c>false</c>.
		/// </value>
		public bool OwnsStream { get; set; }

		/// <summary>
		///		Initializes a new instance of the <see cref="PackerUnpackerStreamOptions"/> class.
		/// </summary>
		public PackerUnpackerStreamOptions() { }

		internal Stream WrapStream( Stream stream )
		{
			if ( !this.WithBuffering )
			{
				return stream;
			}

#if !SILVERLIGHT
			if ( ( stream is BufferedStream ) || ( stream is MemoryStream )
#if !NETFX_CORE && !CORE_CLR
				|| ( stream is UnmanagedMemoryStream ) || ( stream is FileStream ) 
#endif // !NETFX_CORE && !CORE_CLR
			)
			{
				// They have in-memory based synchronous read/write optimization.
				return stream;
			}
			
			return new BufferedStream( stream, this._bufferSize );
#else
			return stream;
#endif // !SILVERLIGHT
		}
	}
}