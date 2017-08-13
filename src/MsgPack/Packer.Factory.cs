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

#if UNITY_5 || UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_WII || UNITY_IPHONE || UNITY_ANDROID || UNITY_PS3 || UNITY_XBOX360 || UNITY_FLASH || UNITY_BKACKBERRY || UNITY_WINRT
#define UNITY
#endif

using System;
using System.Collections.Generic;
#if CORE_CLR || UNITY || NETSTANDARD1_1
using Contract = MsgPack.MPContract;
#else
using System.Diagnostics.Contracts;
#endif // CORE_CLR || UNITY || NETSTANDARD1_1
using System.Globalization;
using System.IO;
#if FEATURE_TAP
using System.Threading;
using System.Threading.Tasks;
#endif // FEATURE_TAP

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
			return new DefaultStreamPacker( stream, ownsStream ? PackerUnpackerStreamOptions.SingletonOwnsStream : PackerUnpackerStreamOptions.None, compatibilityOptions );
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
			return new DefaultStreamPacker( stream, streamOptions, compatibilityOptions );
		}

		#endregion -- Stream --

		#region -- byte[] --

		/// <summary>
		///		Creates a new <see cref="ByteArrayPacker"/> from specified byte array list with default allocation strategy which uses product of the original size and golden ratio and <see cref="DefaultCompatibilityOptions"/>.
		/// </summary>
		/// <param name="array">The source byte array.</param>
		/// <returns><see cref="ByteArrayPacker"/> instance. This value will not be <c>null</c>.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="array"/> is <c>null</c>.</exception>
		public static ByteArrayPacker Create( byte[] array )
		{
			return Create( new ArraySegment<byte>( array ) );
		}

		/// <summary>
		///		Creates a new <see cref="ByteArrayPacker"/> from specified byte array list with default allocation strategy which uses product of the original size and golden ratio and compatibility options.
		/// </summary>
		/// <param name="array">The source byte array.</param>
		/// <param name="compatibilityOptions">A <see cref="PackerCompatibilityOptions"/> which specifies compatibility options.</param>
		/// <returns><see cref="ByteArrayPacker"/> instance. This value will not be <c>null</c>.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="array"/> is <c>null</c>.</exception>
		public static ByteArrayPacker Create( byte[] array, PackerCompatibilityOptions compatibilityOptions )
		{
			return Create( new ArraySegment<byte>( array ), compatibilityOptions );
		}

		/// <summary>
		///		Creates a new <see cref="ByteArrayPacker"/> from specified byte array list with with <see cref="DefaultCompatibilityOptions"/>.
		/// </summary>
		/// <param name="array">The source byte array.</param>
		/// <param name="allowsBufferExpansion">
		///		If <c>true</c>, new buffer is allocated in product of the original size and golden ratio.
		///		Otherwise, the buffer will never be replaced.
		/// </param>
		/// <returns><see cref="ByteArrayPacker"/> instance. This value will not be <c>null</c>.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="array"/> is <c>null</c>.</exception>
		public static ByteArrayPacker Create( byte[] array, bool allowsBufferExpansion )
		{
			return Create( new ArraySegment<byte>( array ), allowsBufferExpansion, DefaultCompatibilityOptions );
		}

		/// <summary>
		///		Creates a new <see cref="ByteArrayPacker"/> from specified byte array list with compatibility options.
		/// </summary>
		/// <param name="array">The source byte array.</param>
		/// <param name="allowsBufferExpansion">
		///		If <c>true</c>, new buffer is allocated in product of the original size and golden ratio.
		///		Otherwise, the buffer will never be replaced.
		/// </param>
		/// <param name="compatibilityOptions">A <see cref="PackerCompatibilityOptions"/> which specifies compatibility options.</param>
		/// <returns><see cref="ByteArrayPacker"/> instance. This value will not be <c>null</c>.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="array"/> is <c>null</c>.</exception>
		public static ByteArrayPacker Create( byte[] array, bool allowsBufferExpansion, PackerCompatibilityOptions compatibilityOptions )
		{
			return Create( new ArraySegment<byte>( array ), allowsBufferExpansion, compatibilityOptions );
		}

		#endregion -- byte[] --

		#region -- byte[], int --

		/// <summary>
		///		Creates a new <see cref="ByteArrayPacker"/> from specified byte array list with default allocation strategy which uses product of the original size and golden ratio and <see cref="DefaultCompatibilityOptions"/>.
		/// </summary>
		/// <param name="array">The source byte array.</param>
		/// <param name="offset">The effective start offset of the <paramref name="array"/>.</param>
		/// <returns><see cref="ByteArrayPacker"/> instance. This value will not be <c>null</c>.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="array"/> is <c>null</c>.</exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///		<paramref name="offset"/> is negative.
		///	</exception>
		/// <exception cref="ArgumentException">The array length of <paramref name="array"/> is too small.</exception>
		public static ByteArrayPacker Create( byte[] array, int offset )
		{
			return Create( new ArraySegment<byte>( array, offset, array == null ? 0 : ( array.Length - offset ) ) );
		}

		/// <summary>
		///		Creates a new <see cref="ByteArrayPacker"/> from specified byte array list with default allocation strategy which uses product of the original size and golden ratio and compatibility options.
		/// </summary>
		/// <param name="array">The source byte array.</param>
		/// <param name="offset">The effective start offset of the <paramref name="array"/>.</param>
		/// <param name="compatibilityOptions">A <see cref="PackerCompatibilityOptions"/> which specifies compatibility options.</param>
		/// <returns><see cref="ByteArrayPacker"/> instance. This value will not be <c>null</c>.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="array"/> is <c>null</c>.</exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///		<paramref name="offset"/> is negative.
		///	</exception>
		/// <exception cref="ArgumentException">The array length of <paramref name="array"/> is too small.</exception>
		public static ByteArrayPacker Create( byte[] array, int offset, PackerCompatibilityOptions compatibilityOptions )
		{
			return Create( new ArraySegment<byte>( array, offset, array == null ? 0 : ( array.Length - offset ) ), compatibilityOptions );
		}

		/// <summary>
		///		Creates a new <see cref="ByteArrayPacker"/> from specified byte array list with with <see cref="DefaultCompatibilityOptions"/>.
		/// </summary>
		/// <param name="array">The source byte array.</param>
		/// <param name="offset">The effective start offset of the <paramref name="array"/>.</param>
		/// <param name="allowsBufferExpansion">
		///		If <c>true</c>, new buffer is allocated in product of the original size and golden ratio.
		///		Otherwise, the buffer will never be replaced.
		/// </param>
		/// <returns><see cref="ByteArrayPacker"/> instance. This value will not be <c>null</c>.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="array"/> is <c>null</c>.</exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///		<paramref name="offset"/> is negative.
		///	</exception>
		/// <exception cref="ArgumentException">The array length of <paramref name="array"/> is too small.</exception>
		public static ByteArrayPacker Create( byte[] array, int offset, bool allowsBufferExpansion )
		{
			return Create( new ArraySegment<byte>( array, offset, array == null ? 0 : ( array.Length - offset ) ), allowsBufferExpansion, DefaultCompatibilityOptions );
		}

		/// <summary>
		///		Creates a new <see cref="ByteArrayPacker"/> from specified byte array list with compatibility options.
		/// </summary>
		/// <param name="array">The source byte array.</param>
		/// <param name="offset">The effective start offset of the <paramref name="array"/>.</param>
		/// <param name="allowsBufferExpansion">
		///		If <c>true</c>, new buffer is allocated in product of the original size and golden ratio.
		///		Otherwise, the buffer will never be replaced.
		/// </param>
		/// <param name="compatibilityOptions">A <see cref="PackerCompatibilityOptions"/> which specifies compatibility options.</param>
		/// <returns><see cref="ByteArrayPacker"/> instance. This value will not be <c>null</c>.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="array"/> is <c>null</c>.</exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///		<paramref name="offset"/> is negative.
		///	</exception>
		/// <exception cref="ArgumentException">The array length of <paramref name="array"/> is too small.</exception>
		public static ByteArrayPacker Create( byte[] array, int offset, bool allowsBufferExpansion, PackerCompatibilityOptions compatibilityOptions )
		{
			return Create( new ArraySegment<byte>( array, offset, array == null ? 0 : ( array.Length - offset ) ), allowsBufferExpansion, compatibilityOptions );
		}

		#endregion -- byte[], int --

		#region -- byte[], int, int --

		/// <summary>
		///		Creates a new <see cref="ByteArrayPacker"/> from specified byte array list with default allocation strategy which uses product of the original size and golden ratio and <see cref="DefaultCompatibilityOptions"/>.
		/// </summary>
		/// <param name="array">The source byte array.</param>
		/// <param name="offset">The effective start offset of the <paramref name="array"/>.</param>
		/// <param name="count">The effective count of the <paramref name="array"/>.</param>
		/// <returns><see cref="ByteArrayPacker"/> instance. This value will not be <c>null</c>.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="array"/> is <c>null</c>.</exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///		<paramref name="offset"/> is negative.
		///		Or <paramref name="count"/> is negative.
		///	</exception>
		/// <exception cref="ArgumentException">The array length of <paramref name="array"/> is too small.</exception>
		public static ByteArrayPacker Create( byte[] array, int offset, int count )
		{
			return Create( new ArraySegment<byte>( array, offset, count ) );
		}

		/// <summary>
		///		Creates a new <see cref="ByteArrayPacker"/> from specified byte array list with default allocation strategy which uses product of the original size and golden ratio and compatibility options.
		/// </summary>
		/// <param name="array">The source byte array.</param>
		/// <param name="offset">The effective start offset of the <paramref name="array"/>.</param>
		/// <param name="count">The effective count of the <paramref name="array"/>.</param>
		/// <param name="compatibilityOptions">A <see cref="PackerCompatibilityOptions"/> which specifies compatibility options.</param>
		/// <returns><see cref="ByteArrayPacker"/> instance. This value will not be <c>null</c>.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="array"/> is <c>null</c>.</exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///		<paramref name="offset"/> is negative.
		///		Or <paramref name="count"/> is negative.
		///	</exception>
		/// <exception cref="ArgumentException">The array length of <paramref name="array"/> is too small.</exception>
		public static ByteArrayPacker Create( byte[] array, int offset, int count, PackerCompatibilityOptions compatibilityOptions )
		{
			return Create( new ArraySegment<byte>( array, offset, count ), compatibilityOptions );
		}

		/// <summary>
		///		Creates a new <see cref="ByteArrayPacker"/> from specified byte array list with with <see cref="DefaultCompatibilityOptions"/>.
		/// </summary>
		/// <param name="array">The source byte array.</param>
		/// <param name="offset">The effective start offset of the <paramref name="array"/>.</param>
		/// <param name="count">The effective count of the <paramref name="array"/>.</param>
		/// <param name="allowsBufferExpansion">
		///		If <c>true</c>, new buffer is allocated in product of the original size and golden ratio.
		///		Otherwise, the buffer will never be replaced.
		/// </param>
		/// <returns><see cref="ByteArrayPacker"/> instance. This value will not be <c>null</c>.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="array"/> is <c>null</c>.</exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///		<paramref name="offset"/> is negative.
		///		Or <paramref name="count"/> is negative.
		///	</exception>
		/// <exception cref="ArgumentException">The array length of <paramref name="array"/> is too small.</exception>
		public static ByteArrayPacker Create( byte[] array, int offset, int count, bool allowsBufferExpansion )
		{
			return Create( new ArraySegment<byte>( array, offset, count ), allowsBufferExpansion, DefaultCompatibilityOptions );
		}

		/// <summary>
		///		Creates a new <see cref="ByteArrayPacker"/> from specified byte array list with compatibility options.
		/// </summary>
		/// <param name="array">The source byte array.</param>
		/// <param name="offset">The effective start offset of the <paramref name="array"/>.</param>
		/// <param name="count">The effective count of the <paramref name="array"/>.</param>
		/// <param name="allowsBufferExpansion">
		///		If <c>true</c>, new buffer is allocated in product of the original size and golden ratio.
		///		Otherwise, the buffer will never be replaced.
		/// </param>
		/// <param name="compatibilityOptions">A <see cref="PackerCompatibilityOptions"/> which specifies compatibility options.</param>
		/// <returns><see cref="ByteArrayPacker"/> instance. This value will not be <c>null</c>.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="array"/> is <c>null</c>.</exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///		<paramref name="offset"/> is negative.
		///		Or <paramref name="count"/> is negative.
		///	</exception>
		/// <exception cref="ArgumentException">The array length of <paramref name="array"/> is too small.</exception>
		public static ByteArrayPacker Create( byte[] array, int offset, int count, bool allowsBufferExpansion, PackerCompatibilityOptions compatibilityOptions )
		{
			return Create( new ArraySegment<byte>( array, offset, count ), allowsBufferExpansion, compatibilityOptions );
		}

		#endregion -- byte[], int, int --

		#region -- ArraySegment<byte> --

		/// <summary>
		///		Creates a new <see cref="ByteArrayPacker"/> from specified byte array list with default allocation strategy which uses product of the original size and golden ratio and <see cref="DefaultCompatibilityOptions"/>.
		/// </summary>
		/// <param name="buffer">The destination buffer byte array.</param>
		/// <returns><see cref="ByteArrayPacker"/> instance. This value will not be <c>null</c>.</returns>
		/// <exception cref="ArgumentException"><paramref name="buffer"/> does not contain an array.</exception>
		public static ByteArrayPacker Create( ArraySegment<byte> buffer )
		{
			return Create( buffer, allowsBufferExpansion: true, compatibilityOptions: DefaultCompatibilityOptions );
		}

		/// <summary>
		///		Creates a new <see cref="ByteArrayPacker"/> from specified byte array list with default allocation strategy which uses product of the original size and golden ratio and compatibility options.
		/// </summary>
		/// <param name="buffer">The destination buffer byte array.</param>
		/// <param name="compatibilityOptions">A <see cref="PackerCompatibilityOptions"/> which specifies compatibility options.</param>
		/// <returns><see cref="ByteArrayPacker"/> instance. This value will not be <c>null</c>.</returns>
		/// <exception cref="ArgumentException"><paramref name="buffer"/> does not contain an array.</exception>
		public static ByteArrayPacker Create( ArraySegment<byte> buffer, PackerCompatibilityOptions compatibilityOptions )
		{
			return Create( buffer, allowsBufferExpansion: true, compatibilityOptions: compatibilityOptions );
		}

		/// <summary>
		///		Creates a new <see cref="ByteArrayPacker"/> from specified byte array list with with <see cref="DefaultCompatibilityOptions"/>.
		/// </summary>
		/// <param name="buffer">The destination buffer byte array.</param>
		/// <param name="allowsBufferExpansion">
		///		If <c>true</c>, new buffer is allocated in product of the original size and golden ratio.
		///		Otherwise, the buffers will never be expanded.
		/// </param>
		/// <returns><see cref="ByteArrayPacker"/> instance. This value will not be <c>null</c>.</returns>
		/// <exception cref="ArgumentException"><paramref name="buffer"/> does not contain an array.</exception>
		public static ByteArrayPacker Create( ArraySegment<byte> buffer, bool allowsBufferExpansion )
		{
			return Create( buffer, allowsBufferExpansion, DefaultCompatibilityOptions );
		}

		/// <summary>
		///		Creates a new <see cref="ByteArrayPacker"/> from specified byte array list with compatibility options.
		/// </summary>
		/// <param name="buffer">The destination buffer byte array.</param>
		/// <param name="allowsBufferExpansion">
		///		If <c>true</c>, new buffer is allocated in product of the original size and golden ratio.
		///		Otherwise, the buffers will never be expanded.
		/// </param>
		/// <param name="compatibilityOptions">A <see cref="PackerCompatibilityOptions"/> which specifies compatibility options.</param>
		/// <returns><see cref="ByteArrayPacker"/> instance. This value will not be <c>null</c>.</returns>
		/// <exception cref="ArgumentException"><paramref name="buffer"/> does not contain an array.</exception>
		public static ByteArrayPacker Create( ArraySegment<byte> buffer, bool allowsBufferExpansion, PackerCompatibilityOptions compatibilityOptions )
		{
			return new DefaultByteArrayPacker( buffer, allowsBufferExpansion ? SingleArrayBufferAllocator.Default : FixedArrayBufferAllocator.Instance, compatibilityOptions );
		}

		/// <summary>
		///		Creates a new <see cref="ByteArrayPacker"/> from specified byte array list with specified allocation strategy and <see cref="DefaultCompatibilityOptions"/>.
		/// </summary>
		/// <param name="buffer">The destination buffer byte array.</param>
		/// <param name="allocator">
		///		The delegate which allocates new buffer <see cref="ArraySegment{T}"/> of byte to be replaced with old buffer.
		///		If <c>null</c> is specified, the <paramref name="buffer"/> will never be replaced.
		/// </param>
		/// <returns><see cref="ByteArrayPacker"/> instance. This value will not be <c>null</c>.</returns>
		/// <exception cref="ArgumentException"><paramref name="buffer"/> does not contain an array.</exception>
		public static ByteArrayPacker Create( ArraySegment<byte> buffer, Func<ArraySegment<byte>, int, ArraySegment<byte>> allocator )
		{
			return Create( buffer, allocator, DefaultCompatibilityOptions );
		}

		/// <summary>
		///		Creates a new <see cref="ByteArrayPacker"/> from specified byte array with specified allocation strategy and compatibility options.
		/// </summary>
		/// <param name="buffer">The destination buffer byte array.</param>
		/// <param name="allocator">
		///		The delegate which allocates new buffer <see cref="ArraySegment{T}"/> of byte to be replaced with old buffer.
		///		If <c>null</c> is specified, the <paramref name="buffer"/> will never be replaced.
		/// </param>
		/// <param name="compatibilityOptions">A <see cref="PackerCompatibilityOptions"/> which specifies compatibility options.</param>
		/// <returns><see cref="ByteArrayPacker"/> instance. This value will not be <c>null</c>.</returns>
		/// <exception cref="ArgumentException"><paramref name="buffer"/> does not contain an array.</exception>
		public static ByteArrayPacker Create( ArraySegment<byte> buffer, Func<ArraySegment<byte>, int, ArraySegment<byte>> allocator, PackerCompatibilityOptions compatibilityOptions )
		{
			return new DefaultByteArrayPacker( buffer, allocator == null ? FixedArrayBufferAllocator.Instance : new SingleArrayBufferAllocator( allocator ), compatibilityOptions );
		}

		#endregion -- ArraySegment<byte> --

		#region -- IList<ArraySegment<byte>> --

		/// <summary>
		///		Creates a new <see cref="ByteArrayPacker"/> from specified byte array list with default allocation strategy which uses fixed-size manner (curent size is 64K) and <see cref="DefaultCompatibilityOptions"/>.
		/// </summary>
		/// <param name="buffers">The list of destination buffer byte array.</param>
		/// <param name="startIndex">The start index of <paramref name="buffers"/>.</param>
		/// <param name="startOffset">
		///		The start offset of <see cref="ArraySegment{T}"/> of <paramref name="buffers"/> at <paramref name="startIndex"/>.
		///		Note that this is not relative, but absolute offset to get subarray from the <see cref="ArraySegment{T}"/>.
		///	</param>
		/// <returns><see cref="ByteArrayPacker"/> instance. This value will not be <c>null</c>.</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="buffers"/> is <c>null</c>.
		///	</exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///		<paramref name="startIndex"/> is negative.
		///		Or <paramref name="startOffset"/> is negative.
		///	</exception>
		/// <exception cref="ArgumentException">
		///		The size of <paramref name="buffers"/> is too small to specified <paramref name="startIndex"/>.
		///		Or <paramref name="buffers"/> contains any <see cref="ArraySegment{T}"/> which does not contain an array.
		///		Or <paramref name="startIndex"/> is too large for the <see cref="ArraySegment{T}"/>.
		///		Or <paramref name="startOffset"/> is less than the <see cref="ArraySegment{T}.Offset"/>.
		///	</exception>
		public static ByteArrayPacker Create( IList<ArraySegment<byte>> buffers, int startIndex, int startOffset )
		{
			return Create( buffers, startIndex, startOffset, allowsBufferExpansion: true, compatibilityOptions: DefaultCompatibilityOptions );
		}

		/// <summary>
		///		Creates a new <see cref="ByteArrayPacker"/> from specified byte array list with default allocation strategy which uses fixed-size manner (curent size is 64K) and compatibility options.
		/// </summary>
		/// <param name="buffers">The list of destination buffer byte array.</param>
		/// <param name="startIndex">The start index of <paramref name="buffers"/>.</param>
		/// <param name="startOffset">
		///		The start offset of <see cref="ArraySegment{T}"/> of <paramref name="buffers"/> at <paramref name="startIndex"/>.
		///		Note that this is not relative, but absolute offset to get subarray from the <see cref="ArraySegment{T}"/>.
		///	</param>
		/// <param name="compatibilityOptions">A <see cref="PackerCompatibilityOptions"/> which specifies compatibility options.</param>
		/// <returns><see cref="ByteArrayPacker"/> instance. This value will not be <c>null</c>.</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="buffers"/> is <c>null</c>.
		///	</exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///		<paramref name="startIndex"/> is negative.
		///		Or <paramref name="startOffset"/> is negative.
		///	</exception>
		/// <exception cref="ArgumentException">
		///		The size of <paramref name="buffers"/> is too small to specified <paramref name="startIndex"/>.
		///		Or <paramref name="buffers"/> contains any <see cref="ArraySegment{T}"/> which does not contain an array.
		///		Or <paramref name="startIndex"/> is too large for the <see cref="ArraySegment{T}"/>.
		///		Or <paramref name="startOffset"/> is less than the <see cref="ArraySegment{T}.Offset"/>.
		///	</exception>
		public static ByteArrayPacker Create( IList<ArraySegment<byte>> buffers, int startIndex, int startOffset, PackerCompatibilityOptions compatibilityOptions )
		{
			return Create( buffers, startIndex, startOffset, allowsBufferExpansion: true, compatibilityOptions: compatibilityOptions );
		}

		/// <summary>
		///		Creates a new <see cref="ByteArrayPacker"/> from specified byte array list with <see cref="DefaultCompatibilityOptions"/>.
		/// </summary>
		/// <param name="buffers">The list of destination buffer byte array.</param>
		/// <param name="startIndex">The start index of <paramref name="buffers"/>.</param>
		/// <param name="startOffset">
		///		The start offset of <see cref="ArraySegment{T}"/> of <paramref name="buffers"/> at <paramref name="startIndex"/>.
		///		Note that this is not relative, but absolute offset to get subarray from the <see cref="ArraySegment{T}"/>.
		///	</param>
		/// <param name="allowsBufferExpansion">
		///		If <c>true</c>, new buffer is allocated in fixed-size manner (curent size is 64K).
		///		Otherwise, the buffers will never be expanded.
		/// </param>
		/// <returns><see cref="ByteArrayPacker"/> instance. This value will not be <c>null</c>.</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="buffers"/> is <c>null</c>.
		///	</exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///		<paramref name="startIndex"/> is negative.
		///		Or <paramref name="startOffset"/> is negative.
		///	</exception>
		/// <exception cref="ArgumentException">
		///		The size of <paramref name="buffers"/> is too small to specified <paramref name="startIndex"/>.
		///		Or <paramref name="buffers"/> contains any <see cref="ArraySegment{T}"/> which does not contain an array.
		///		Or <paramref name="startIndex"/> is too large for the <see cref="ArraySegment{T}"/>.
		///		Or <paramref name="startOffset"/> is less than the <see cref="ArraySegment{T}.Offset"/>.
		///	</exception>
		public static ByteArrayPacker Create( IList<ArraySegment<byte>> buffers, int startIndex, int startOffset, bool allowsBufferExpansion )
		{
			return Create( buffers, startIndex, startOffset, allowsBufferExpansion, DefaultCompatibilityOptions );
		}

		/// <summary>
		///		Creates a new <see cref="ByteArrayPacker"/> from specified byte array list with compatibility options.
		/// </summary>
		/// <param name="buffers">The list of destination buffer byte array.</param>
		/// <param name="startIndex">The start index of <paramref name="buffers"/>.</param>
		/// <param name="startOffset">
		///		The start offset of <see cref="ArraySegment{T}"/> of <paramref name="buffers"/> at <paramref name="startIndex"/>.
		///		Note that this is not relative, but absolute offset to get subarray from the <see cref="ArraySegment{T}"/>.
		///	</param>
		/// <param name="allowsBufferExpansion">
		///		If <c>true</c>, new buffer is allocated in fixed-size manner (curent size is 64K).
		///		Otherwise, the buffers will never be expanded.
		/// </param>
		/// <param name="compatibilityOptions">A <see cref="PackerCompatibilityOptions"/> which specifies compatibility options.</param>
		/// <returns><see cref="ByteArrayPacker"/> instance. This value will not be <c>null</c>.</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="buffers"/> is <c>null</c>.
		///	</exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///		<paramref name="startIndex"/> is negative.
		///		Or <paramref name="startOffset"/> is negative.
		///	</exception>
		/// <exception cref="ArgumentException">
		///		The size of <paramref name="buffers"/> is too small to specified <paramref name="startIndex"/>.
		///		Or <paramref name="buffers"/> contains any <see cref="ArraySegment{T}"/> which does not contain an array.
		///		Or <paramref name="startIndex"/> is too large for the <see cref="ArraySegment{T}"/>.
		///		Or <paramref name="startOffset"/> is less than the <see cref="ArraySegment{T}.Offset"/>.
		///	</exception>
		public static ByteArrayPacker Create( IList<ArraySegment<byte>> buffers, int startIndex, int startOffset, bool allowsBufferExpansion, PackerCompatibilityOptions compatibilityOptions )
		{
			return new DefaultByteArrayPacker( buffers, startIndex, startOffset, allowsBufferExpansion ? MultipleArrayBufferAllocator.Default : FixedArrayBufferAllocator.Instance, compatibilityOptions );
		}

		/// <summary>
		///		Creates a new <see cref="ByteArrayPacker"/> from specified byte array list with simple fixed-size allocation strategy and <see cref="DefaultCompatibilityOptions"/>.
		/// </summary>
		/// <param name="buffers">The list of destination buffer byte array.</param>
		/// <param name="startIndex">The start index of <paramref name="buffers"/>.</param>
		/// <param name="startOffset">
		///		The start offset of <see cref="ArraySegment{T}"/> of <paramref name="buffers"/> at <paramref name="startIndex"/>.
		///		Note that this is not relative, but absolute offset to get subarray from the <see cref="ArraySegment{T}"/>.
		///	</param>
		/// <param name="allocationUnitSize">
		///		The size of allocation unit in bytes for each new allocation requests.
		///		Mulitple allocation will occur when the required size is greater than this size.
		/// </param>
		/// <returns><see cref="ByteArrayPacker"/> instance. This value will not be <c>null</c>.</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="buffers"/> is <c>null</c>.
		///	</exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///		<paramref name="startIndex"/> is negative.
		///		Or <paramref name="startOffset"/> is negative.
		///		Or <paramref name="allocationUnitSize"/> is zero or negative.
		///	</exception>
		/// <exception cref="ArgumentException">
		///		The size of <paramref name="buffers"/> is too small to specified <paramref name="startIndex"/>.
		///		Or <paramref name="buffers"/> contains any <see cref="ArraySegment{T}"/> which does not contain an array.
		///		Or <paramref name="startIndex"/> is too large for the <see cref="ArraySegment{T}"/>.
		///		Or <paramref name="startOffset"/> is less than the <see cref="ArraySegment{T}.Offset"/>.
		///	</exception>
		public static ByteArrayPacker Create( IList<ArraySegment<byte>> buffers, int startIndex, int startOffset, int allocationUnitSize )
		{
			return Create( buffers, startIndex, startOffset, allocationUnitSize, DefaultCompatibilityOptions );
		}

		/// <summary>
		///		Creates a new <see cref="ByteArrayPacker"/> from specified byte array list with simple fixed-size allocation strategy and compatibility options.
		/// </summary>
		/// <param name="buffers">The list of destination buffer byte array.</param>
		/// <param name="startIndex">The start index of <paramref name="buffers"/>.</param>
		/// <param name="startOffset">
		///		The start offset of <see cref="ArraySegment{T}"/> of <paramref name="buffers"/> at <paramref name="startIndex"/>.
		///		Note that this is not relative, but absolute offset to get subarray from the <see cref="ArraySegment{T}"/>.
		///	</param>
		/// <param name="allocationUnitSize">
		///		The size of allocation unit in bytes for each new allocation requests.
		///		Mulitple allocation will occur when the required size is greater than this size.
		/// </param>
		/// <param name="compatibilityOptions">A <see cref="PackerCompatibilityOptions"/> which specifies compatibility options.</param>
		/// <returns><see cref="ByteArrayPacker"/> instance. This value will not be <c>null</c>.</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="buffers"/> is <c>null</c>.
		///	</exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///		<paramref name="startIndex"/> is negative.
		///		Or <paramref name="startOffset"/> is negative.
		///		Or <paramref name="allocationUnitSize"/> is zero or negative.
		///	</exception>
		/// <exception cref="ArgumentException">
		///		The size of <paramref name="buffers"/> is too small to specified <paramref name="startIndex"/>.
		///		Or <paramref name="buffers"/> contains any <see cref="ArraySegment{T}"/> which does not contain an array.
		///		Or <paramref name="startIndex"/> is too large for the <see cref="ArraySegment{T}"/>.
		///		Or <paramref name="startOffset"/> is less than the <see cref="ArraySegment{T}.Offset"/>.
		///	</exception>
		public static ByteArrayPacker Create( IList<ArraySegment<byte>> buffers, int startIndex, int startOffset, int allocationUnitSize, PackerCompatibilityOptions compatibilityOptions )
		{
			return new DefaultByteArrayPacker( buffers, startIndex, startOffset, new MultipleArrayBufferAllocator( allocationUnitSize ), compatibilityOptions );
		}

		/// <summary>
		///		Creates a new <see cref="ByteArrayPacker"/> from specified byte array list with specified allocation strategy and <see cref="DefaultCompatibilityOptions"/>.
		/// </summary>
		/// <param name="buffers">The list of destination buffer byte array.</param>
		/// <param name="startIndex">The start index of <paramref name="buffers"/>.</param>
		/// <param name="startOffset">
		///		The start offset of <see cref="ArraySegment{T}"/> of <paramref name="buffers"/> at <paramref name="startIndex"/>.
		///		Note that this is not relative, but absolute offset to get subarray from the <see cref="ArraySegment{T}"/>.
		///	</param>
		/// <param name="allocator">
		///		The delegate which allocates new buffer <see cref="ArraySegment{T}"/> of byte to be appended to the <paramref name="buffers" />.
		///		If <c>null</c> is specified, the <paramref name="buffers"/> will never be expanded.
		/// </param>
		/// <returns><see cref="ByteArrayPacker"/> instance. This value will not be <c>null</c>.</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="buffers"/> is <c>null</c>.
		///	</exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///		<paramref name="startIndex"/> is negative.
		///		Or <paramref name="startOffset"/> is negative.
		///	</exception>
		/// <exception cref="ArgumentException">
		///		The size of <paramref name="buffers"/> is too small to specified <paramref name="startIndex"/>.
		///		Or <paramref name="buffers"/> contains any <see cref="ArraySegment{T}"/> which does not contain an array.
		///		Or <paramref name="startIndex"/> is too large for the <see cref="ArraySegment{T}"/>.
		///		Or <paramref name="startOffset"/> is less than the <see cref="ArraySegment{T}.Offset"/>.
		///	</exception>
		public static ByteArrayPacker Create( IList<ArraySegment<byte>> buffers, int startIndex, int startOffset, Func<int, ArraySegment<byte>> allocator )
		{
			return Create( buffers, startIndex, startOffset, allocator, DefaultCompatibilityOptions );
		}

		/// <summary>
		///		Creates a new <see cref="ByteArrayPacker"/> from specified byte array list with specified allocation strategy and compatibility options.
		/// </summary>
		/// <param name="buffers">The list of destination buffer byte array.</param>
		/// <param name="startIndex">The start index of <paramref name="buffers"/>.</param>
		/// <param name="startOffset">
		///		The start offset of <see cref="ArraySegment{T}"/> of <paramref name="buffers"/> at <paramref name="startIndex"/>.
		///		Note that this is not relative, but absolute offset to get subarray from the <see cref="ArraySegment{T}"/>.
		///	</param>
		/// <param name="allocator">
		///		The delegate which allocates new buffer <see cref="ArraySegment{T}"/> of byte to be appended to the <paramref name="buffers" />.
		///		If <c>null</c> is specified, the <paramref name="buffers"/> will never be expanded.
		/// </param>
		/// <param name="compatibilityOptions">A <see cref="PackerCompatibilityOptions"/> which specifies compatibility options.</param>
		/// <returns><see cref="ByteArrayPacker"/> instance. This value will not be <c>null</c>.</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="buffers"/> is <c>null</c>.
		///	</exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///		<paramref name="startIndex"/> is negative.
		///		Or <paramref name="startOffset"/> is negative.
		///	</exception>
		/// <exception cref="ArgumentException">
		///		The size of <paramref name="buffers"/> is too small to specified <paramref name="startIndex"/>.
		///		Or <paramref name="buffers"/> contains any <see cref="ArraySegment{T}"/> which does not contain an array.
		///		Or <paramref name="startIndex"/> is too large for the <see cref="ArraySegment{T}"/>.
		///		Or <paramref name="startOffset"/> is less than the <see cref="ArraySegment{T}.Offset"/>.
		///	</exception>
		public static ByteArrayPacker Create( IList<ArraySegment<byte>> buffers, int startIndex, int startOffset, Func<int, ArraySegment<byte>> allocator, PackerCompatibilityOptions compatibilityOptions )
		{
			return new DefaultByteArrayPacker( buffers, startIndex, startOffset, allocator == null ? FixedArrayBufferAllocator.Instance : new MultipleArrayBufferAllocator( allocator ), compatibilityOptions );
		}

		#endregion -- IList<ArraySegment<byte>> --
	}
}
