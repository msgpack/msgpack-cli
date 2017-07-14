#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2017 FUJIWARA, Yusuke and contributors
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
	partial class Packer
	{
		#region -- Stream --

		/// <summary>
		///		Create standard Safe <see cref="Packer"/> instance wrapping specified <see cref="Stream"/> with <see cref="DefaultCompatibilityOptions"/>.
		/// </summary>
		/// <param name="stream"><see cref="Stream"/> object. This stream will be closed when <see cref="Packer.Dispose(Boolean)"/> is called.</param>
		/// <returns>Safe <see cref="Packer"/>. This will not be null.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="stream"/> is null.</exception>
		/// <remarks>
		///		 You can specify any derived <see cref="Stream"/> class like FileStream, <see cref="MemoryStream"/>,
		///		 NetworkStream, UnmanagedMemoryStream, or so.
		/// </remarks>
		public static Packer Create( Stream stream )
		{
			return Create( stream, true );
		}

		/// <summary>
		///		Create standard Safe <see cref="Packer"/> instance wrapping specified <see cref="Stream"/> with specified <see cref="PackerCompatibilityOptions"/>.
		/// </summary>
		/// <param name="stream"><see cref="Stream"/> object. This stream will be closed when <see cref="Packer.Dispose(Boolean)"/> is called.</param>
		/// <param name="compatibilityOptions">A <see cref="PackerCompatibilityOptions"/> which specifies compatibility options.</param>
		/// <returns>Safe <see cref="Packer"/>. This will not be null.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="stream"/> is null.</exception>
		/// <remarks>
		///		 You can specify any derived <see cref="Stream"/> class like FileStream, <see cref="MemoryStream"/>,
		///		 NetworkStream, UnmanagedMemoryStream, or so.
		/// </remarks>
		public static Packer Create( Stream stream, PackerCompatibilityOptions compatibilityOptions )
		{
			return Create( stream, compatibilityOptions, true );
		}

		/// <summary>
		///		Create standard Safe <see cref="Packer"/> instance wrapping specified <see cref="Stream"/> with <see cref="DefaultCompatibilityOptions"/>.
		/// </summary>
		/// <param name="stream"><see cref="Stream"/> object.</param>
		/// <param name="ownsStream">
		///		<c>true</c> to close <paramref name="stream"/> when this instance is disposed;
		///		<c>false</c>, otherwise.
		/// </param>
		/// <returns>Safe <see cref="Packer"/>. This will not be null.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="stream"/> is null.</exception>
		/// <remarks>
		///		 You can specify any derived <see cref="Stream"/> class like FileStream, <see cref="MemoryStream"/>,
		///		 NetworkStream, UnmanagedMemoryStream, or so.
		/// </remarks>
		public static Packer Create( Stream stream, bool ownsStream )
		{
			return Create( stream, DefaultCompatibilityOptions, ownsStream );
		}

		/// <summary>
		///		Create standard Safe <see cref="Packer"/> instance wrapping specified <see cref="Stream"/> with specified <see cref="PackerCompatibilityOptions"/>.
		/// </summary>
		/// <param name="stream"><see cref="Stream"/> object.</param>
		/// <param name="compatibilityOptions">A <see cref="PackerCompatibilityOptions"/> which specifies compatibility options.</param>
		/// <param name="ownsStream">
		///		<c>true</c> to close <paramref name="stream"/> when this instance is disposed;
		///		<c>false</c>, otherwise.
		/// </param>
		/// <returns>Safe <see cref="Packer"/>. This will not be null.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="stream"/> is null.</exception>
		/// <remarks>
		///		 You can specify any derived <see cref="Stream"/> class like FileStream, <see cref="MemoryStream"/>,
		///		 NetworkStream, UnmanagedMemoryStream, or so.
		/// </remarks>
		public static Packer Create( Stream stream, PackerCompatibilityOptions compatibilityOptions, bool ownsStream )
		{
			return new MessagePackStreamPacker( stream, ownsStream ? PackerUnpackerStreamOptions.SingletonOwnsStream : PackerUnpackerStreamOptions.None, compatibilityOptions );
		}

		/// <summary>
		///		Create standard Safe <see cref="Packer"/> instance wrapping specified <see cref="Stream"/> with specified <see cref="PackerCompatibilityOptions"/>.
		/// </summary>
		/// <param name="stream"><see cref="Stream"/> object.</param>
		/// <param name="compatibilityOptions">A <see cref="PackerCompatibilityOptions"/> which specifies compatibility options.</param>
		/// <param name="streamOptions"><see cref="PackerUnpackerStreamOptions"/> which specifies stream handling options.</param>
		/// <returns>Safe <see cref="Packer"/>. This will not be null.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="stream"/> is null.</exception>
		/// <remarks>
		///		 You can specify any derived <see cref="Stream"/> class like FileStream, <see cref="MemoryStream"/>,
		///		 NetworkStream, UnmanagedMemoryStream, or so.
		/// </remarks>
		public static Packer Create( Stream stream, PackerCompatibilityOptions compatibilityOptions, PackerUnpackerStreamOptions streamOptions )
		{
			return new MessagePackStreamPacker( stream, streamOptions, compatibilityOptions );
		}

		#endregion -- Stream --

		#region -- byte[] --

		/// <summary>
		///		Creates a new <see cref="ByteArrayPacker"/> from specified byte array with <see cref="DefaultCompatibilityOptions"/> allowing expansion.
		/// </summary>
		/// <param name="buffer">The source byte array.</param>
		/// <returns><see cref="ByteArrayPacker"/> instance. This value will not be <c>null</c>.</returns>
		public static ByteArrayPacker Create( byte[] buffer )
		{
			return Create( buffer, true, DefaultCompatibilityOptions );
		}

		/// <summary>
		///		Creates a new <see cref="ByteArrayPacker"/> from specified byte array list with <see cref="DefaultCompatibilityOptions"/> allowing expansion.
		/// </summary>
		/// <param name="buffer">The source byte array.</param>
		/// <param name="startOffset">The effective start offset of the <paramref name="buffer"/>.</param>
		/// <returns><see cref="ByteArrayPacker"/> instance. This value will not be <c>null</c>.</returns>
		/// <exception cref="ArgumentOutOfRangeException">
		///		<paramref name="startOffset"/> is negative.
		///	</exception>
		/// <exception cref="ArgumentException">The array length of <paramref name="buffer"/> is too small.</exception>
		public static ByteArrayPacker Create( byte[] buffer, int startOffset )
		{
			return Create( buffer, startOffset, true, DefaultCompatibilityOptions );
		}

		/// <summary>
		///		Creates a new <see cref="ByteArrayPacker"/> from specified byte array list with compatibility options.
		/// </summary>
		/// <param name="buffer">The source byte array.</param>
		/// <param name="allowsBufferExpansion">
		///		If <c>true</c>, new buffer is allocated in product of the original size and golden ratio.
		///		Otherwise, the buffer will never be replaced.
		/// </param>
		/// <param name="compatibilityOptions">A <see cref="PackerCompatibilityOptions"/> which specifies compatibility options.</param>
		/// <returns><see cref="ByteArrayPacker"/> instance. This value will not be <c>null</c>.</returns>
		public static ByteArrayPacker Create( byte[] buffer, bool allowsBufferExpansion, PackerCompatibilityOptions compatibilityOptions )
		{
			return new MessagePackByteArrayPacker( buffer, 0, allowsBufferExpansion ? SingleArrayBufferAllocator.Default : FixedArrayBufferAllocator.Instance, compatibilityOptions );
		}

		/// <summary>
		///		Creates a new <see cref="ByteArrayPacker"/> from specified byte array with compatibility options.
		/// </summary>
		/// <param name="buffer">The source byte array.</param>
		/// <param name="startOffset">The effective start offset of the <paramref name="buffer"/>.</param>
		/// <param name="allowsBufferExpansion">
		///		If <c>true</c>, new buffer is allocated in product of the original size and golden ratio.
		///		Otherwise, the buffer will never be replaced.
		/// </param>
		/// <param name="compatibilityOptions">A <see cref="PackerCompatibilityOptions"/> which specifies compatibility options.</param>
		/// <returns><see cref="ByteArrayPacker"/> instance. This value will not be <c>null</c>.</returns>
		/// <exception cref="ArgumentOutOfRangeException">
		///		<paramref name="startOffset"/> is negative.
		///	</exception>
		/// <exception cref="ArgumentException">The array length of <paramref name="buffer"/> is too small.</exception>
		public static ByteArrayPacker Create( byte[] buffer, int startOffset, bool allowsBufferExpansion, PackerCompatibilityOptions compatibilityOptions )
		{
			return new MessagePackByteArrayPacker( buffer, startOffset, allowsBufferExpansion ? SingleArrayBufferAllocator.Default : FixedArrayBufferAllocator.Instance, compatibilityOptions );
		}

		/// <summary>
		///		Creates a new <see cref="ByteArrayPacker"/> from specified byte array with compatibility options and custom allocator.
		/// </summary>
		/// <param name="buffer">The source byte array.</param>
		/// <param name="allocator">
		///		A delegate to allocate new byte array which has requested size at least.
		///		The first argument is old buffer which contains written data and second argument is requested (required) size.
		///		The delegate must return new byte array which has enough size for requested write and contains old buffer's content.
		///		If the delegate returns <c>null</c>, the packer will consider it as allocation failure.
		/// </param>
		/// <param name="compatibilityOptions">A <see cref="PackerCompatibilityOptions"/> which specifies compatibility options.</param>
		/// <returns><see cref="ByteArrayPacker"/> instance. This value will not be <c>null</c>.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="allocator"/> is <c>null</c>.</exception>
		public static ByteArrayPacker Create( byte[] buffer, Func<byte[], int, byte[]> allocator, PackerCompatibilityOptions compatibilityOptions )
		{
			return Create( buffer, 0, allocator, compatibilityOptions );
		}

		/// <summary>
		///		Creates a new <see cref="ByteArrayPacker"/> from specified byte array with compatibility options and custom allocator.
		/// </summary>
		/// <param name="buffer">The source byte array.</param>
		/// <param name="startOffset">The effective start offset of the <paramref name="buffer"/>.</param>
		/// <param name="allocator">
		///		A delegate to allocate new byte array which has requested size at least.
		///		The first argument is old buffer which contains written data and second argument is requested (required) size.
		///		The delegate must return new byte array which has enough size for requested write and contains old buffer's content.
		///		If the delegate returns <c>null</c>, the packer will consider it as allocation failure.
		/// </param>
		/// <param name="compatibilityOptions">A <see cref="PackerCompatibilityOptions"/> which specifies compatibility options.</param>
		/// <returns><see cref="ByteArrayPacker"/> instance. This value will not be <c>null</c>.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="allocator"/> is <c>null</c>.</exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///		<paramref name="startOffset"/> is negative.
		///	</exception>
		/// <exception cref="ArgumentException">The array length of <paramref name="buffer"/> is too small.</exception>
		public static ByteArrayPacker Create( byte[] buffer, int startOffset, Func<byte[], int, byte[]> allocator, PackerCompatibilityOptions compatibilityOptions )
		{
			return new MessagePackByteArrayPacker( buffer, startOffset, new SingleArrayBufferAllocator( allocator ), compatibilityOptions );
		}

		#endregion -- byte[] --
	}
}
