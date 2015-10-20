#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2015 FUJIWARA, Yusuke
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
#if !UNITY
#if XAMIOS || XAMDROID
using Contract = MsgPack.MPContract;
#else
using System.Diagnostics.Contracts;
#endif // XAMIOS || XAMDROID
#endif // !UNITY
using System.IO;

namespace MsgPack
{
	// This file generated from Unpacking.Intgers.tt T4Template.
	// Do not modify this file. Edit Unpacking.Intgers.tt instead.

	static partial class Unpacking
	{
		///	<summary>
		///		Unpacks <see cref="System.Byte" /> value from the head of specified byte array.
		///	</summary>
		///	<param name="source">The byte array which contains Message Pack binary stream.</param>
		///	<returns>
		///		The <see cref="UnpackingResult{T}"/> of <see cref="System.Byte" /> which contains unpacked <see cref="System.Byte" /> value and processed bytes count.
		///	</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="source"/> is <c>null</c>.
		///	</exception>
		/// <exception cref="ArgumentException">
		///		<paramref name="source"/> is empty.
		///	</exception>
		/// <exception cref="UnpackException">
		///		<paramref name="source"/> is not valid MessagePack stream.
		///	</exception>
		/// <exception cref="MessageTypeException">
		///		The unpacked result in the <paramref name="source"/> is not compatible to <see cref="System.Byte" />.
		///	</exception>
		/// <remarks>
		///		<para>
		/// 		Invocation of this method is equivalant to call <see cref="UnpackByte(byte[], int)"/> with <c>offset</c> is <c>0</c>.
		///		</para>
		///		<para>
		///			When the type of packed value is not known, use <see cref="UnpackObject(byte[])"/> instead.
		///		</para>
		///	</remarks>
		public static UnpackingResult<Byte> UnpackByte( byte[] source )
		{
			return UnpackByte( source, 0 );
		}

		///	<summary>
		///		Unpacks <see cref="System.Byte" /> value from the specified byte array.
		///	</summary>
		///	<param name="source">The byte array which contains Message Pack binary stream.</param>
		///	<param name="offset">The offset to be unpacking start with.</param>
		///	<returns>
		///		The <see cref="UnpackingResult{T}"/> of <see cref="System.Byte" /> which contains unpacked <see cref="System.Byte" /> value and processed bytes count.
		///	</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="source"/> is <c>null</c>.
		///	</exception>
		/// <exception cref="ArgumentException">
		///		<paramref name="source"/> is empty.
		///		Or, the length of <paramref name="source"/> is not grator than <paramref name="offset"/>.
		///	</exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///		<paramref name="offset"/> is negative value.
		///	</exception>
		/// <exception cref="UnpackException">
		///		<paramref name="source"/> is not valid MessagePack stream.
		///	</exception>
		/// <exception cref="MessageTypeException">
		///		The unpacked result in the <paramref name="source"/> is not compatible to <see cref="System.Byte" />.
		///	</exception>
		/// <remarks>
		///		When the type of packed value is not known, use <see cref="UnpackObject(byte[], int)"/> instead.
		///	</remarks>
		public static UnpackingResult<Byte> UnpackByte( byte[] source, int offset )
		{
			ValidateByteArray( source, offset );
#if !UNITY
			Contract.EndContractBlock();
#endif // !UNITY

			using( var stream = new MemoryStream( source ) )
			{
				stream.Position = offset;
				var value = UnpackByteCore( stream );
				return new UnpackingResult<Byte>( value, unchecked( ( int )( stream.Position - offset ) ) );
			}
		}

		///	<summary>
		///		Unpacks <see cref="System.Byte" /> value from the specified <see cref="Stream"/>.
		///	</summary>
		///	<param name="source">The <see cref="Stream"/> which contains Message Pack binary stream.</param>
		///	<returns>
		///		The unpacked <see cref="System.Byte" /> value.
		///	</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="source"/> is <c>null</c>.
		///	</exception>
		/// <exception cref="ArgumentException">
		///		The <see cref="P:Stream.CanRead"/> of <paramref name="source"/> is <c>false</c>.
		///	</exception>
		/// <exception cref="UnpackException">
		///		<paramref name="source"/> is not valid MessagePack stream.
		///		Note that the state of <paramref name="source"/> will be unpredictable espicially it is not seekable.
		///	</exception>
		/// <exception cref="MessageTypeException">
		///		The unpacked result in the <paramref name="source"/> is not compatible to <see cref="System.Byte" />.
		///		Note that the state of <paramref name="source"/> will be unpredictable espicially it is not seekable.
		///	</exception>
		/// <remarks>
		///		<para>
		/// 		The processed bytes count can be calculated via <see cref="P:Stream.Position"/> of <paramref name="source"/> when the <see cref="P:Stream.CanSeek" /> is <c>true</c>.
		///		</para>
		///		<para>
		///			When the type of packed value is not known, use <see cref="UnpackObject(Stream)"/> instead.
		///		</para>
		///	</remarks>
		public static Byte UnpackByte( Stream source )
		{
			ValidateStream( source );
#if !UNITY
			Contract.EndContractBlock();
#endif // !UNITY

			return UnpackByteCore( source );
		}
		
		private static Byte UnpackByteCore( Stream source )
		{
			using ( var unpacker = Unpacker.Create( source, false ) )
			{
				UnpackOne( unpacker );
				VerifyIsScalar( unpacker );
				try
				{
					return ( Byte )unpacker.LastReadData;
				}
				catch( InvalidOperationException ex )
				{
					throw NewTypeMismatchException( typeof( Byte ), ex );
				}
			}
		}
		///	<summary>
		///		Unpacks <see cref="System.SByte" /> value from the head of specified byte array.
		///	</summary>
		///	<param name="source">The byte array which contains Message Pack binary stream.</param>
		///	<returns>
		///		The <see cref="UnpackingResult{T}"/> of <see cref="System.SByte" /> which contains unpacked <see cref="System.SByte" /> value and processed bytes count.
		///	</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="source"/> is <c>null</c>.
		///	</exception>
		/// <exception cref="ArgumentException">
		///		<paramref name="source"/> is empty.
		///	</exception>
		/// <exception cref="UnpackException">
		///		<paramref name="source"/> is not valid MessagePack stream.
		///	</exception>
		/// <exception cref="MessageTypeException">
		///		The unpacked result in the <paramref name="source"/> is not compatible to <see cref="System.SByte" />.
		///	</exception>
		/// <remarks>
		///		<para>
		/// 		Invocation of this method is equivalant to call <see cref="UnpackSByte(byte[], int)"/> with <c>offset</c> is <c>0</c>.
		///		</para>
		///		<para>
		///			When the type of packed value is not known, use <see cref="UnpackObject(byte[])"/> instead.
		///		</para>
		///	</remarks>
#if !UNITY
		[CLSCompliant( false )]
#endif // !UNITY
		public static UnpackingResult<SByte> UnpackSByte( byte[] source )
		{
			return UnpackSByte( source, 0 );
		}

		///	<summary>
		///		Unpacks <see cref="System.SByte" /> value from the specified byte array.
		///	</summary>
		///	<param name="source">The byte array which contains Message Pack binary stream.</param>
		///	<param name="offset">The offset to be unpacking start with.</param>
		///	<returns>
		///		The <see cref="UnpackingResult{T}"/> of <see cref="System.SByte" /> which contains unpacked <see cref="System.SByte" /> value and processed bytes count.
		///	</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="source"/> is <c>null</c>.
		///	</exception>
		/// <exception cref="ArgumentException">
		///		<paramref name="source"/> is empty.
		///		Or, the length of <paramref name="source"/> is not grator than <paramref name="offset"/>.
		///	</exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///		<paramref name="offset"/> is negative value.
		///	</exception>
		/// <exception cref="UnpackException">
		///		<paramref name="source"/> is not valid MessagePack stream.
		///	</exception>
		/// <exception cref="MessageTypeException">
		///		The unpacked result in the <paramref name="source"/> is not compatible to <see cref="System.SByte" />.
		///	</exception>
		/// <remarks>
		///		When the type of packed value is not known, use <see cref="UnpackObject(byte[], int)"/> instead.
		///	</remarks>
#if !UNITY
		[CLSCompliant( false )]
#endif // !UNITY
		public static UnpackingResult<SByte> UnpackSByte( byte[] source, int offset )
		{
			ValidateByteArray( source, offset );
#if !UNITY
			Contract.EndContractBlock();
#endif // !UNITY

			using( var stream = new MemoryStream( source ) )
			{
				stream.Position = offset;
				var value = UnpackSByteCore( stream );
				return new UnpackingResult<SByte>( value, unchecked( ( int )( stream.Position - offset ) ) );
			}
		}

		///	<summary>
		///		Unpacks <see cref="System.SByte" /> value from the specified <see cref="Stream"/>.
		///	</summary>
		///	<param name="source">The <see cref="Stream"/> which contains Message Pack binary stream.</param>
		///	<returns>
		///		The unpacked <see cref="System.SByte" /> value.
		///	</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="source"/> is <c>null</c>.
		///	</exception>
		/// <exception cref="ArgumentException">
		///		The <see cref="P:Stream.CanRead"/> of <paramref name="source"/> is <c>false</c>.
		///	</exception>
		/// <exception cref="UnpackException">
		///		<paramref name="source"/> is not valid MessagePack stream.
		///		Note that the state of <paramref name="source"/> will be unpredictable espicially it is not seekable.
		///	</exception>
		/// <exception cref="MessageTypeException">
		///		The unpacked result in the <paramref name="source"/> is not compatible to <see cref="System.SByte" />.
		///		Note that the state of <paramref name="source"/> will be unpredictable espicially it is not seekable.
		///	</exception>
		/// <remarks>
		///		<para>
		/// 		The processed bytes count can be calculated via <see cref="P:Stream.Position"/> of <paramref name="source"/> when the <see cref="P:Stream.CanSeek" /> is <c>true</c>.
		///		</para>
		///		<para>
		///			When the type of packed value is not known, use <see cref="UnpackObject(Stream)"/> instead.
		///		</para>
		///	</remarks>
#if !UNITY
		[CLSCompliant( false )]
#endif // !UNITY
		public static SByte UnpackSByte( Stream source )
		{
			ValidateStream( source );
#if !UNITY
			Contract.EndContractBlock();
#endif // !UNITY

			return UnpackSByteCore( source );
		}
		
		private static SByte UnpackSByteCore( Stream source )
		{
			using ( var unpacker = Unpacker.Create( source, false ) )
			{
				UnpackOne( unpacker );
				VerifyIsScalar( unpacker );
				try
				{
					return ( SByte )unpacker.LastReadData;
				}
				catch( InvalidOperationException ex )
				{
					throw NewTypeMismatchException( typeof( SByte ), ex );
				}
			}
		}
		///	<summary>
		///		Unpacks <see cref="System.Int16" /> value from the head of specified byte array.
		///	</summary>
		///	<param name="source">The byte array which contains Message Pack binary stream.</param>
		///	<returns>
		///		The <see cref="UnpackingResult{T}"/> of <see cref="System.Int16" /> which contains unpacked <see cref="System.Int16" /> value and processed bytes count.
		///	</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="source"/> is <c>null</c>.
		///	</exception>
		/// <exception cref="ArgumentException">
		///		<paramref name="source"/> is empty.
		///	</exception>
		/// <exception cref="UnpackException">
		///		<paramref name="source"/> is not valid MessagePack stream.
		///	</exception>
		/// <exception cref="MessageTypeException">
		///		The unpacked result in the <paramref name="source"/> is not compatible to <see cref="System.Int16" />.
		///	</exception>
		/// <remarks>
		///		<para>
		/// 		Invocation of this method is equivalant to call <see cref="UnpackInt16(byte[], int)"/> with <c>offset</c> is <c>0</c>.
		///		</para>
		///		<para>
		///			When the type of packed value is not known, use <see cref="UnpackObject(byte[])"/> instead.
		///		</para>
		///	</remarks>
		public static UnpackingResult<Int16> UnpackInt16( byte[] source )
		{
			return UnpackInt16( source, 0 );
		}

		///	<summary>
		///		Unpacks <see cref="System.Int16" /> value from the specified byte array.
		///	</summary>
		///	<param name="source">The byte array which contains Message Pack binary stream.</param>
		///	<param name="offset">The offset to be unpacking start with.</param>
		///	<returns>
		///		The <see cref="UnpackingResult{T}"/> of <see cref="System.Int16" /> which contains unpacked <see cref="System.Int16" /> value and processed bytes count.
		///	</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="source"/> is <c>null</c>.
		///	</exception>
		/// <exception cref="ArgumentException">
		///		<paramref name="source"/> is empty.
		///		Or, the length of <paramref name="source"/> is not grator than <paramref name="offset"/>.
		///	</exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///		<paramref name="offset"/> is negative value.
		///	</exception>
		/// <exception cref="UnpackException">
		///		<paramref name="source"/> is not valid MessagePack stream.
		///	</exception>
		/// <exception cref="MessageTypeException">
		///		The unpacked result in the <paramref name="source"/> is not compatible to <see cref="System.Int16" />.
		///	</exception>
		/// <remarks>
		///		When the type of packed value is not known, use <see cref="UnpackObject(byte[], int)"/> instead.
		///	</remarks>
		public static UnpackingResult<Int16> UnpackInt16( byte[] source, int offset )
		{
			ValidateByteArray( source, offset );
#if !UNITY
			Contract.EndContractBlock();
#endif // !UNITY

			using( var stream = new MemoryStream( source ) )
			{
				stream.Position = offset;
				var value = UnpackInt16Core( stream );
				return new UnpackingResult<Int16>( value, unchecked( ( int )( stream.Position - offset ) ) );
			}
		}

		///	<summary>
		///		Unpacks <see cref="System.Int16" /> value from the specified <see cref="Stream"/>.
		///	</summary>
		///	<param name="source">The <see cref="Stream"/> which contains Message Pack binary stream.</param>
		///	<returns>
		///		The unpacked <see cref="System.Int16" /> value.
		///	</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="source"/> is <c>null</c>.
		///	</exception>
		/// <exception cref="ArgumentException">
		///		The <see cref="P:Stream.CanRead"/> of <paramref name="source"/> is <c>false</c>.
		///	</exception>
		/// <exception cref="UnpackException">
		///		<paramref name="source"/> is not valid MessagePack stream.
		///		Note that the state of <paramref name="source"/> will be unpredictable espicially it is not seekable.
		///	</exception>
		/// <exception cref="MessageTypeException">
		///		The unpacked result in the <paramref name="source"/> is not compatible to <see cref="System.Int16" />.
		///		Note that the state of <paramref name="source"/> will be unpredictable espicially it is not seekable.
		///	</exception>
		/// <remarks>
		///		<para>
		/// 		The processed bytes count can be calculated via <see cref="P:Stream.Position"/> of <paramref name="source"/> when the <see cref="P:Stream.CanSeek" /> is <c>true</c>.
		///		</para>
		///		<para>
		///			When the type of packed value is not known, use <see cref="UnpackObject(Stream)"/> instead.
		///		</para>
		///	</remarks>
		public static Int16 UnpackInt16( Stream source )
		{
			ValidateStream( source );
#if !UNITY
			Contract.EndContractBlock();
#endif // !UNITY

			return UnpackInt16Core( source );
		}
		
		private static Int16 UnpackInt16Core( Stream source )
		{
			using ( var unpacker = Unpacker.Create( source, false ) )
			{
				UnpackOne( unpacker );
				VerifyIsScalar( unpacker );
				try
				{
					return ( Int16 )unpacker.LastReadData;
				}
				catch( InvalidOperationException ex )
				{
					throw NewTypeMismatchException( typeof( Int16 ), ex );
				}
			}
		}
		///	<summary>
		///		Unpacks <see cref="System.UInt16" /> value from the head of specified byte array.
		///	</summary>
		///	<param name="source">The byte array which contains Message Pack binary stream.</param>
		///	<returns>
		///		The <see cref="UnpackingResult{T}"/> of <see cref="System.UInt16" /> which contains unpacked <see cref="System.UInt16" /> value and processed bytes count.
		///	</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="source"/> is <c>null</c>.
		///	</exception>
		/// <exception cref="ArgumentException">
		///		<paramref name="source"/> is empty.
		///	</exception>
		/// <exception cref="UnpackException">
		///		<paramref name="source"/> is not valid MessagePack stream.
		///	</exception>
		/// <exception cref="MessageTypeException">
		///		The unpacked result in the <paramref name="source"/> is not compatible to <see cref="System.UInt16" />.
		///	</exception>
		/// <remarks>
		///		<para>
		/// 		Invocation of this method is equivalant to call <see cref="UnpackUInt16(byte[], int)"/> with <c>offset</c> is <c>0</c>.
		///		</para>
		///		<para>
		///			When the type of packed value is not known, use <see cref="UnpackObject(byte[])"/> instead.
		///		</para>
		///	</remarks>
#if !UNITY
		[CLSCompliant( false )]
#endif // !UNITY
		public static UnpackingResult<UInt16> UnpackUInt16( byte[] source )
		{
			return UnpackUInt16( source, 0 );
		}

		///	<summary>
		///		Unpacks <see cref="System.UInt16" /> value from the specified byte array.
		///	</summary>
		///	<param name="source">The byte array which contains Message Pack binary stream.</param>
		///	<param name="offset">The offset to be unpacking start with.</param>
		///	<returns>
		///		The <see cref="UnpackingResult{T}"/> of <see cref="System.UInt16" /> which contains unpacked <see cref="System.UInt16" /> value and processed bytes count.
		///	</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="source"/> is <c>null</c>.
		///	</exception>
		/// <exception cref="ArgumentException">
		///		<paramref name="source"/> is empty.
		///		Or, the length of <paramref name="source"/> is not grator than <paramref name="offset"/>.
		///	</exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///		<paramref name="offset"/> is negative value.
		///	</exception>
		/// <exception cref="UnpackException">
		///		<paramref name="source"/> is not valid MessagePack stream.
		///	</exception>
		/// <exception cref="MessageTypeException">
		///		The unpacked result in the <paramref name="source"/> is not compatible to <see cref="System.UInt16" />.
		///	</exception>
		/// <remarks>
		///		When the type of packed value is not known, use <see cref="UnpackObject(byte[], int)"/> instead.
		///	</remarks>
#if !UNITY
		[CLSCompliant( false )]
#endif // !UNITY
		public static UnpackingResult<UInt16> UnpackUInt16( byte[] source, int offset )
		{
			ValidateByteArray( source, offset );
#if !UNITY
			Contract.EndContractBlock();
#endif // !UNITY

			using( var stream = new MemoryStream( source ) )
			{
				stream.Position = offset;
				var value = UnpackUInt16Core( stream );
				return new UnpackingResult<UInt16>( value, unchecked( ( int )( stream.Position - offset ) ) );
			}
		}

		///	<summary>
		///		Unpacks <see cref="System.UInt16" /> value from the specified <see cref="Stream"/>.
		///	</summary>
		///	<param name="source">The <see cref="Stream"/> which contains Message Pack binary stream.</param>
		///	<returns>
		///		The unpacked <see cref="System.UInt16" /> value.
		///	</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="source"/> is <c>null</c>.
		///	</exception>
		/// <exception cref="ArgumentException">
		///		The <see cref="P:Stream.CanRead"/> of <paramref name="source"/> is <c>false</c>.
		///	</exception>
		/// <exception cref="UnpackException">
		///		<paramref name="source"/> is not valid MessagePack stream.
		///		Note that the state of <paramref name="source"/> will be unpredictable espicially it is not seekable.
		///	</exception>
		/// <exception cref="MessageTypeException">
		///		The unpacked result in the <paramref name="source"/> is not compatible to <see cref="System.UInt16" />.
		///		Note that the state of <paramref name="source"/> will be unpredictable espicially it is not seekable.
		///	</exception>
		/// <remarks>
		///		<para>
		/// 		The processed bytes count can be calculated via <see cref="P:Stream.Position"/> of <paramref name="source"/> when the <see cref="P:Stream.CanSeek" /> is <c>true</c>.
		///		</para>
		///		<para>
		///			When the type of packed value is not known, use <see cref="UnpackObject(Stream)"/> instead.
		///		</para>
		///	</remarks>
#if !UNITY
		[CLSCompliant( false )]
#endif // !UNITY
		public static UInt16 UnpackUInt16( Stream source )
		{
			ValidateStream( source );
#if !UNITY
			Contract.EndContractBlock();
#endif // !UNITY

			return UnpackUInt16Core( source );
		}
		
		private static UInt16 UnpackUInt16Core( Stream source )
		{
			using ( var unpacker = Unpacker.Create( source, false ) )
			{
				UnpackOne( unpacker );
				VerifyIsScalar( unpacker );
				try
				{
					return ( UInt16 )unpacker.LastReadData;
				}
				catch( InvalidOperationException ex )
				{
					throw NewTypeMismatchException( typeof( UInt16 ), ex );
				}
			}
		}
		///	<summary>
		///		Unpacks <see cref="System.Int32" /> value from the head of specified byte array.
		///	</summary>
		///	<param name="source">The byte array which contains Message Pack binary stream.</param>
		///	<returns>
		///		The <see cref="UnpackingResult{T}"/> of <see cref="System.Int32" /> which contains unpacked <see cref="System.Int32" /> value and processed bytes count.
		///	</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="source"/> is <c>null</c>.
		///	</exception>
		/// <exception cref="ArgumentException">
		///		<paramref name="source"/> is empty.
		///	</exception>
		/// <exception cref="UnpackException">
		///		<paramref name="source"/> is not valid MessagePack stream.
		///	</exception>
		/// <exception cref="MessageTypeException">
		///		The unpacked result in the <paramref name="source"/> is not compatible to <see cref="System.Int32" />.
		///	</exception>
		/// <remarks>
		///		<para>
		/// 		Invocation of this method is equivalant to call <see cref="UnpackInt32(byte[], int)"/> with <c>offset</c> is <c>0</c>.
		///		</para>
		///		<para>
		///			When the type of packed value is not known, use <see cref="UnpackObject(byte[])"/> instead.
		///		</para>
		///	</remarks>
		public static UnpackingResult<Int32> UnpackInt32( byte[] source )
		{
			return UnpackInt32( source, 0 );
		}

		///	<summary>
		///		Unpacks <see cref="System.Int32" /> value from the specified byte array.
		///	</summary>
		///	<param name="source">The byte array which contains Message Pack binary stream.</param>
		///	<param name="offset">The offset to be unpacking start with.</param>
		///	<returns>
		///		The <see cref="UnpackingResult{T}"/> of <see cref="System.Int32" /> which contains unpacked <see cref="System.Int32" /> value and processed bytes count.
		///	</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="source"/> is <c>null</c>.
		///	</exception>
		/// <exception cref="ArgumentException">
		///		<paramref name="source"/> is empty.
		///		Or, the length of <paramref name="source"/> is not grator than <paramref name="offset"/>.
		///	</exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///		<paramref name="offset"/> is negative value.
		///	</exception>
		/// <exception cref="UnpackException">
		///		<paramref name="source"/> is not valid MessagePack stream.
		///	</exception>
		/// <exception cref="MessageTypeException">
		///		The unpacked result in the <paramref name="source"/> is not compatible to <see cref="System.Int32" />.
		///	</exception>
		/// <remarks>
		///		When the type of packed value is not known, use <see cref="UnpackObject(byte[], int)"/> instead.
		///	</remarks>
		public static UnpackingResult<Int32> UnpackInt32( byte[] source, int offset )
		{
			ValidateByteArray( source, offset );
#if !UNITY
			Contract.EndContractBlock();
#endif // !UNITY

			using( var stream = new MemoryStream( source ) )
			{
				stream.Position = offset;
				var value = UnpackInt32Core( stream );
				return new UnpackingResult<Int32>( value, unchecked( ( int )( stream.Position - offset ) ) );
			}
		}

		///	<summary>
		///		Unpacks <see cref="System.Int32" /> value from the specified <see cref="Stream"/>.
		///	</summary>
		///	<param name="source">The <see cref="Stream"/> which contains Message Pack binary stream.</param>
		///	<returns>
		///		The unpacked <see cref="System.Int32" /> value.
		///	</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="source"/> is <c>null</c>.
		///	</exception>
		/// <exception cref="ArgumentException">
		///		The <see cref="P:Stream.CanRead"/> of <paramref name="source"/> is <c>false</c>.
		///	</exception>
		/// <exception cref="UnpackException">
		///		<paramref name="source"/> is not valid MessagePack stream.
		///		Note that the state of <paramref name="source"/> will be unpredictable espicially it is not seekable.
		///	</exception>
		/// <exception cref="MessageTypeException">
		///		The unpacked result in the <paramref name="source"/> is not compatible to <see cref="System.Int32" />.
		///		Note that the state of <paramref name="source"/> will be unpredictable espicially it is not seekable.
		///	</exception>
		/// <remarks>
		///		<para>
		/// 		The processed bytes count can be calculated via <see cref="P:Stream.Position"/> of <paramref name="source"/> when the <see cref="P:Stream.CanSeek" /> is <c>true</c>.
		///		</para>
		///		<para>
		///			When the type of packed value is not known, use <see cref="UnpackObject(Stream)"/> instead.
		///		</para>
		///	</remarks>
		public static Int32 UnpackInt32( Stream source )
		{
			ValidateStream( source );
#if !UNITY
			Contract.EndContractBlock();
#endif // !UNITY

			return UnpackInt32Core( source );
		}
		
		private static Int32 UnpackInt32Core( Stream source )
		{
			using ( var unpacker = Unpacker.Create( source, false ) )
			{
				UnpackOne( unpacker );
				VerifyIsScalar( unpacker );
				try
				{
					return ( Int32 )unpacker.LastReadData;
				}
				catch( InvalidOperationException ex )
				{
					throw NewTypeMismatchException( typeof( Int32 ), ex );
				}
			}
		}
		///	<summary>
		///		Unpacks <see cref="System.UInt32" /> value from the head of specified byte array.
		///	</summary>
		///	<param name="source">The byte array which contains Message Pack binary stream.</param>
		///	<returns>
		///		The <see cref="UnpackingResult{T}"/> of <see cref="System.UInt32" /> which contains unpacked <see cref="System.UInt32" /> value and processed bytes count.
		///	</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="source"/> is <c>null</c>.
		///	</exception>
		/// <exception cref="ArgumentException">
		///		<paramref name="source"/> is empty.
		///	</exception>
		/// <exception cref="UnpackException">
		///		<paramref name="source"/> is not valid MessagePack stream.
		///	</exception>
		/// <exception cref="MessageTypeException">
		///		The unpacked result in the <paramref name="source"/> is not compatible to <see cref="System.UInt32" />.
		///	</exception>
		/// <remarks>
		///		<para>
		/// 		Invocation of this method is equivalant to call <see cref="UnpackUInt32(byte[], int)"/> with <c>offset</c> is <c>0</c>.
		///		</para>
		///		<para>
		///			When the type of packed value is not known, use <see cref="UnpackObject(byte[])"/> instead.
		///		</para>
		///	</remarks>
#if !UNITY
		[CLSCompliant( false )]
#endif // !UNITY
		public static UnpackingResult<UInt32> UnpackUInt32( byte[] source )
		{
			return UnpackUInt32( source, 0 );
		}

		///	<summary>
		///		Unpacks <see cref="System.UInt32" /> value from the specified byte array.
		///	</summary>
		///	<param name="source">The byte array which contains Message Pack binary stream.</param>
		///	<param name="offset">The offset to be unpacking start with.</param>
		///	<returns>
		///		The <see cref="UnpackingResult{T}"/> of <see cref="System.UInt32" /> which contains unpacked <see cref="System.UInt32" /> value and processed bytes count.
		///	</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="source"/> is <c>null</c>.
		///	</exception>
		/// <exception cref="ArgumentException">
		///		<paramref name="source"/> is empty.
		///		Or, the length of <paramref name="source"/> is not grator than <paramref name="offset"/>.
		///	</exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///		<paramref name="offset"/> is negative value.
		///	</exception>
		/// <exception cref="UnpackException">
		///		<paramref name="source"/> is not valid MessagePack stream.
		///	</exception>
		/// <exception cref="MessageTypeException">
		///		The unpacked result in the <paramref name="source"/> is not compatible to <see cref="System.UInt32" />.
		///	</exception>
		/// <remarks>
		///		When the type of packed value is not known, use <see cref="UnpackObject(byte[], int)"/> instead.
		///	</remarks>
#if !UNITY
		[CLSCompliant( false )]
#endif // !UNITY
		public static UnpackingResult<UInt32> UnpackUInt32( byte[] source, int offset )
		{
			ValidateByteArray( source, offset );
#if !UNITY
			Contract.EndContractBlock();
#endif // !UNITY

			using( var stream = new MemoryStream( source ) )
			{
				stream.Position = offset;
				var value = UnpackUInt32Core( stream );
				return new UnpackingResult<UInt32>( value, unchecked( ( int )( stream.Position - offset ) ) );
			}
		}

		///	<summary>
		///		Unpacks <see cref="System.UInt32" /> value from the specified <see cref="Stream"/>.
		///	</summary>
		///	<param name="source">The <see cref="Stream"/> which contains Message Pack binary stream.</param>
		///	<returns>
		///		The unpacked <see cref="System.UInt32" /> value.
		///	</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="source"/> is <c>null</c>.
		///	</exception>
		/// <exception cref="ArgumentException">
		///		The <see cref="P:Stream.CanRead"/> of <paramref name="source"/> is <c>false</c>.
		///	</exception>
		/// <exception cref="UnpackException">
		///		<paramref name="source"/> is not valid MessagePack stream.
		///		Note that the state of <paramref name="source"/> will be unpredictable espicially it is not seekable.
		///	</exception>
		/// <exception cref="MessageTypeException">
		///		The unpacked result in the <paramref name="source"/> is not compatible to <see cref="System.UInt32" />.
		///		Note that the state of <paramref name="source"/> will be unpredictable espicially it is not seekable.
		///	</exception>
		/// <remarks>
		///		<para>
		/// 		The processed bytes count can be calculated via <see cref="P:Stream.Position"/> of <paramref name="source"/> when the <see cref="P:Stream.CanSeek" /> is <c>true</c>.
		///		</para>
		///		<para>
		///			When the type of packed value is not known, use <see cref="UnpackObject(Stream)"/> instead.
		///		</para>
		///	</remarks>
#if !UNITY
		[CLSCompliant( false )]
#endif // !UNITY
		public static UInt32 UnpackUInt32( Stream source )
		{
			ValidateStream( source );
#if !UNITY
			Contract.EndContractBlock();
#endif // !UNITY

			return UnpackUInt32Core( source );
		}
		
		private static UInt32 UnpackUInt32Core( Stream source )
		{
			using ( var unpacker = Unpacker.Create( source, false ) )
			{
				UnpackOne( unpacker );
				VerifyIsScalar( unpacker );
				try
				{
					return ( UInt32 )unpacker.LastReadData;
				}
				catch( InvalidOperationException ex )
				{
					throw NewTypeMismatchException( typeof( UInt32 ), ex );
				}
			}
		}
		///	<summary>
		///		Unpacks <see cref="System.Int64" /> value from the head of specified byte array.
		///	</summary>
		///	<param name="source">The byte array which contains Message Pack binary stream.</param>
		///	<returns>
		///		The <see cref="UnpackingResult{T}"/> of <see cref="System.Int64" /> which contains unpacked <see cref="System.Int64" /> value and processed bytes count.
		///	</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="source"/> is <c>null</c>.
		///	</exception>
		/// <exception cref="ArgumentException">
		///		<paramref name="source"/> is empty.
		///	</exception>
		/// <exception cref="UnpackException">
		///		<paramref name="source"/> is not valid MessagePack stream.
		///	</exception>
		/// <exception cref="MessageTypeException">
		///		The unpacked result in the <paramref name="source"/> is not compatible to <see cref="System.Int64" />.
		///	</exception>
		/// <remarks>
		///		<para>
		/// 		Invocation of this method is equivalant to call <see cref="UnpackInt64(byte[], int)"/> with <c>offset</c> is <c>0</c>.
		///		</para>
		///		<para>
		///			When the type of packed value is not known, use <see cref="UnpackObject(byte[])"/> instead.
		///		</para>
		///	</remarks>
		public static UnpackingResult<Int64> UnpackInt64( byte[] source )
		{
			return UnpackInt64( source, 0 );
		}

		///	<summary>
		///		Unpacks <see cref="System.Int64" /> value from the specified byte array.
		///	</summary>
		///	<param name="source">The byte array which contains Message Pack binary stream.</param>
		///	<param name="offset">The offset to be unpacking start with.</param>
		///	<returns>
		///		The <see cref="UnpackingResult{T}"/> of <see cref="System.Int64" /> which contains unpacked <see cref="System.Int64" /> value and processed bytes count.
		///	</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="source"/> is <c>null</c>.
		///	</exception>
		/// <exception cref="ArgumentException">
		///		<paramref name="source"/> is empty.
		///		Or, the length of <paramref name="source"/> is not grator than <paramref name="offset"/>.
		///	</exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///		<paramref name="offset"/> is negative value.
		///	</exception>
		/// <exception cref="UnpackException">
		///		<paramref name="source"/> is not valid MessagePack stream.
		///	</exception>
		/// <exception cref="MessageTypeException">
		///		The unpacked result in the <paramref name="source"/> is not compatible to <see cref="System.Int64" />.
		///	</exception>
		/// <remarks>
		///		When the type of packed value is not known, use <see cref="UnpackObject(byte[], int)"/> instead.
		///	</remarks>
		public static UnpackingResult<Int64> UnpackInt64( byte[] source, int offset )
		{
			ValidateByteArray( source, offset );
#if !UNITY
			Contract.EndContractBlock();
#endif // !UNITY

			using( var stream = new MemoryStream( source ) )
			{
				stream.Position = offset;
				var value = UnpackInt64Core( stream );
				return new UnpackingResult<Int64>( value, unchecked( ( int )( stream.Position - offset ) ) );
			}
		}

		///	<summary>
		///		Unpacks <see cref="System.Int64" /> value from the specified <see cref="Stream"/>.
		///	</summary>
		///	<param name="source">The <see cref="Stream"/> which contains Message Pack binary stream.</param>
		///	<returns>
		///		The unpacked <see cref="System.Int64" /> value.
		///	</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="source"/> is <c>null</c>.
		///	</exception>
		/// <exception cref="ArgumentException">
		///		The <see cref="P:Stream.CanRead"/> of <paramref name="source"/> is <c>false</c>.
		///	</exception>
		/// <exception cref="UnpackException">
		///		<paramref name="source"/> is not valid MessagePack stream.
		///		Note that the state of <paramref name="source"/> will be unpredictable espicially it is not seekable.
		///	</exception>
		/// <exception cref="MessageTypeException">
		///		The unpacked result in the <paramref name="source"/> is not compatible to <see cref="System.Int64" />.
		///		Note that the state of <paramref name="source"/> will be unpredictable espicially it is not seekable.
		///	</exception>
		/// <remarks>
		///		<para>
		/// 		The processed bytes count can be calculated via <see cref="P:Stream.Position"/> of <paramref name="source"/> when the <see cref="P:Stream.CanSeek" /> is <c>true</c>.
		///		</para>
		///		<para>
		///			When the type of packed value is not known, use <see cref="UnpackObject(Stream)"/> instead.
		///		</para>
		///	</remarks>
		public static Int64 UnpackInt64( Stream source )
		{
			ValidateStream( source );
#if !UNITY
			Contract.EndContractBlock();
#endif // !UNITY

			return UnpackInt64Core( source );
		}
		
		private static Int64 UnpackInt64Core( Stream source )
		{
			using ( var unpacker = Unpacker.Create( source, false ) )
			{
				UnpackOne( unpacker );
				VerifyIsScalar( unpacker );
				try
				{
					return ( Int64 )unpacker.LastReadData;
				}
				catch( InvalidOperationException ex )
				{
					throw NewTypeMismatchException( typeof( Int64 ), ex );
				}
			}
		}
		///	<summary>
		///		Unpacks <see cref="System.UInt64" /> value from the head of specified byte array.
		///	</summary>
		///	<param name="source">The byte array which contains Message Pack binary stream.</param>
		///	<returns>
		///		The <see cref="UnpackingResult{T}"/> of <see cref="System.UInt64" /> which contains unpacked <see cref="System.UInt64" /> value and processed bytes count.
		///	</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="source"/> is <c>null</c>.
		///	</exception>
		/// <exception cref="ArgumentException">
		///		<paramref name="source"/> is empty.
		///	</exception>
		/// <exception cref="UnpackException">
		///		<paramref name="source"/> is not valid MessagePack stream.
		///	</exception>
		/// <exception cref="MessageTypeException">
		///		The unpacked result in the <paramref name="source"/> is not compatible to <see cref="System.UInt64" />.
		///	</exception>
		/// <remarks>
		///		<para>
		/// 		Invocation of this method is equivalant to call <see cref="UnpackUInt64(byte[], int)"/> with <c>offset</c> is <c>0</c>.
		///		</para>
		///		<para>
		///			When the type of packed value is not known, use <see cref="UnpackObject(byte[])"/> instead.
		///		</para>
		///	</remarks>
#if !UNITY
		[CLSCompliant( false )]
#endif // !UNITY
		public static UnpackingResult<UInt64> UnpackUInt64( byte[] source )
		{
			return UnpackUInt64( source, 0 );
		}

		///	<summary>
		///		Unpacks <see cref="System.UInt64" /> value from the specified byte array.
		///	</summary>
		///	<param name="source">The byte array which contains Message Pack binary stream.</param>
		///	<param name="offset">The offset to be unpacking start with.</param>
		///	<returns>
		///		The <see cref="UnpackingResult{T}"/> of <see cref="System.UInt64" /> which contains unpacked <see cref="System.UInt64" /> value and processed bytes count.
		///	</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="source"/> is <c>null</c>.
		///	</exception>
		/// <exception cref="ArgumentException">
		///		<paramref name="source"/> is empty.
		///		Or, the length of <paramref name="source"/> is not grator than <paramref name="offset"/>.
		///	</exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///		<paramref name="offset"/> is negative value.
		///	</exception>
		/// <exception cref="UnpackException">
		///		<paramref name="source"/> is not valid MessagePack stream.
		///	</exception>
		/// <exception cref="MessageTypeException">
		///		The unpacked result in the <paramref name="source"/> is not compatible to <see cref="System.UInt64" />.
		///	</exception>
		/// <remarks>
		///		When the type of packed value is not known, use <see cref="UnpackObject(byte[], int)"/> instead.
		///	</remarks>
#if !UNITY
		[CLSCompliant( false )]
#endif // !UNITY
		public static UnpackingResult<UInt64> UnpackUInt64( byte[] source, int offset )
		{
			ValidateByteArray( source, offset );
#if !UNITY
			Contract.EndContractBlock();
#endif // !UNITY

			using( var stream = new MemoryStream( source ) )
			{
				stream.Position = offset;
				var value = UnpackUInt64Core( stream );
				return new UnpackingResult<UInt64>( value, unchecked( ( int )( stream.Position - offset ) ) );
			}
		}

		///	<summary>
		///		Unpacks <see cref="System.UInt64" /> value from the specified <see cref="Stream"/>.
		///	</summary>
		///	<param name="source">The <see cref="Stream"/> which contains Message Pack binary stream.</param>
		///	<returns>
		///		The unpacked <see cref="System.UInt64" /> value.
		///	</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="source"/> is <c>null</c>.
		///	</exception>
		/// <exception cref="ArgumentException">
		///		The <see cref="P:Stream.CanRead"/> of <paramref name="source"/> is <c>false</c>.
		///	</exception>
		/// <exception cref="UnpackException">
		///		<paramref name="source"/> is not valid MessagePack stream.
		///		Note that the state of <paramref name="source"/> will be unpredictable espicially it is not seekable.
		///	</exception>
		/// <exception cref="MessageTypeException">
		///		The unpacked result in the <paramref name="source"/> is not compatible to <see cref="System.UInt64" />.
		///		Note that the state of <paramref name="source"/> will be unpredictable espicially it is not seekable.
		///	</exception>
		/// <remarks>
		///		<para>
		/// 		The processed bytes count can be calculated via <see cref="P:Stream.Position"/> of <paramref name="source"/> when the <see cref="P:Stream.CanSeek" /> is <c>true</c>.
		///		</para>
		///		<para>
		///			When the type of packed value is not known, use <see cref="UnpackObject(Stream)"/> instead.
		///		</para>
		///	</remarks>
#if !UNITY
		[CLSCompliant( false )]
#endif // !UNITY
		public static UInt64 UnpackUInt64( Stream source )
		{
			ValidateStream( source );
#if !UNITY
			Contract.EndContractBlock();
#endif // !UNITY

			return UnpackUInt64Core( source );
		}
		
		private static UInt64 UnpackUInt64Core( Stream source )
		{
			using ( var unpacker = Unpacker.Create( source, false ) )
			{
				UnpackOne( unpacker );
				VerifyIsScalar( unpacker );
				try
				{
					return ( UInt64 )unpacker.LastReadData;
				}
				catch( InvalidOperationException ex )
				{
					throw NewTypeMismatchException( typeof( UInt64 ), ex );
				}
			}
		}
		///	<summary>
		///		Unpacks <see cref="System.Single" /> value from the head of specified byte array.
		///	</summary>
		///	<param name="source">The byte array which contains Message Pack binary stream.</param>
		///	<returns>
		///		The <see cref="UnpackingResult{T}"/> of <see cref="System.Single" /> which contains unpacked <see cref="System.Single" /> value and processed bytes count.
		///	</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="source"/> is <c>null</c>.
		///	</exception>
		/// <exception cref="ArgumentException">
		///		<paramref name="source"/> is empty.
		///	</exception>
		/// <exception cref="UnpackException">
		///		<paramref name="source"/> is not valid MessagePack stream.
		///	</exception>
		/// <exception cref="MessageTypeException">
		///		The unpacked result in the <paramref name="source"/> is not compatible to <see cref="System.Single" />.
		///	</exception>
		/// <remarks>
		///		<para>
		/// 		Invocation of this method is equivalant to call <see cref="UnpackSingle(byte[], int)"/> with <c>offset</c> is <c>0</c>.
		///		</para>
		///		<para>
		///			When the type of packed value is not known, use <see cref="UnpackObject(byte[])"/> instead.
		///		</para>
		///	</remarks>
		public static UnpackingResult<Single> UnpackSingle( byte[] source )
		{
			return UnpackSingle( source, 0 );
		}

		///	<summary>
		///		Unpacks <see cref="System.Single" /> value from the specified byte array.
		///	</summary>
		///	<param name="source">The byte array which contains Message Pack binary stream.</param>
		///	<param name="offset">The offset to be unpacking start with.</param>
		///	<returns>
		///		The <see cref="UnpackingResult{T}"/> of <see cref="System.Single" /> which contains unpacked <see cref="System.Single" /> value and processed bytes count.
		///	</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="source"/> is <c>null</c>.
		///	</exception>
		/// <exception cref="ArgumentException">
		///		<paramref name="source"/> is empty.
		///		Or, the length of <paramref name="source"/> is not grator than <paramref name="offset"/>.
		///	</exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///		<paramref name="offset"/> is negative value.
		///	</exception>
		/// <exception cref="UnpackException">
		///		<paramref name="source"/> is not valid MessagePack stream.
		///	</exception>
		/// <exception cref="MessageTypeException">
		///		The unpacked result in the <paramref name="source"/> is not compatible to <see cref="System.Single" />.
		///	</exception>
		/// <remarks>
		///		When the type of packed value is not known, use <see cref="UnpackObject(byte[], int)"/> instead.
		///	</remarks>
		public static UnpackingResult<Single> UnpackSingle( byte[] source, int offset )
		{
			ValidateByteArray( source, offset );
#if !UNITY
			Contract.EndContractBlock();
#endif // !UNITY

			using( var stream = new MemoryStream( source ) )
			{
				stream.Position = offset;
				var value = UnpackSingleCore( stream );
				return new UnpackingResult<Single>( value, unchecked( ( int )( stream.Position - offset ) ) );
			}
		}

		///	<summary>
		///		Unpacks <see cref="System.Single" /> value from the specified <see cref="Stream"/>.
		///	</summary>
		///	<param name="source">The <see cref="Stream"/> which contains Message Pack binary stream.</param>
		///	<returns>
		///		The unpacked <see cref="System.Single" /> value.
		///	</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="source"/> is <c>null</c>.
		///	</exception>
		/// <exception cref="ArgumentException">
		///		The <see cref="P:Stream.CanRead"/> of <paramref name="source"/> is <c>false</c>.
		///	</exception>
		/// <exception cref="UnpackException">
		///		<paramref name="source"/> is not valid MessagePack stream.
		///		Note that the state of <paramref name="source"/> will be unpredictable espicially it is not seekable.
		///	</exception>
		/// <exception cref="MessageTypeException">
		///		The unpacked result in the <paramref name="source"/> is not compatible to <see cref="System.Single" />.
		///		Note that the state of <paramref name="source"/> will be unpredictable espicially it is not seekable.
		///	</exception>
		/// <remarks>
		///		<para>
		/// 		The processed bytes count can be calculated via <see cref="P:Stream.Position"/> of <paramref name="source"/> when the <see cref="P:Stream.CanSeek" /> is <c>true</c>.
		///		</para>
		///		<para>
		///			When the type of packed value is not known, use <see cref="UnpackObject(Stream)"/> instead.
		///		</para>
		///	</remarks>
		public static Single UnpackSingle( Stream source )
		{
			ValidateStream( source );
#if !UNITY
			Contract.EndContractBlock();
#endif // !UNITY

			return UnpackSingleCore( source );
		}
		
		private static Single UnpackSingleCore( Stream source )
		{
			using ( var unpacker = Unpacker.Create( source, false ) )
			{
				UnpackOne( unpacker );
				VerifyIsScalar( unpacker );
				try
				{
					return ( Single )unpacker.LastReadData;
				}
				catch( InvalidOperationException ex )
				{
					throw NewTypeMismatchException( typeof( Single ), ex );
				}
			}
		}
		///	<summary>
		///		Unpacks <see cref="System.Double" /> value from the head of specified byte array.
		///	</summary>
		///	<param name="source">The byte array which contains Message Pack binary stream.</param>
		///	<returns>
		///		The <see cref="UnpackingResult{T}"/> of <see cref="System.Double" /> which contains unpacked <see cref="System.Double" /> value and processed bytes count.
		///	</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="source"/> is <c>null</c>.
		///	</exception>
		/// <exception cref="ArgumentException">
		///		<paramref name="source"/> is empty.
		///	</exception>
		/// <exception cref="UnpackException">
		///		<paramref name="source"/> is not valid MessagePack stream.
		///	</exception>
		/// <exception cref="MessageTypeException">
		///		The unpacked result in the <paramref name="source"/> is not compatible to <see cref="System.Double" />.
		///	</exception>
		/// <remarks>
		///		<para>
		/// 		Invocation of this method is equivalant to call <see cref="UnpackDouble(byte[], int)"/> with <c>offset</c> is <c>0</c>.
		///		</para>
		///		<para>
		///			When the type of packed value is not known, use <see cref="UnpackObject(byte[])"/> instead.
		///		</para>
		///	</remarks>
		public static UnpackingResult<Double> UnpackDouble( byte[] source )
		{
			return UnpackDouble( source, 0 );
		}

		///	<summary>
		///		Unpacks <see cref="System.Double" /> value from the specified byte array.
		///	</summary>
		///	<param name="source">The byte array which contains Message Pack binary stream.</param>
		///	<param name="offset">The offset to be unpacking start with.</param>
		///	<returns>
		///		The <see cref="UnpackingResult{T}"/> of <see cref="System.Double" /> which contains unpacked <see cref="System.Double" /> value and processed bytes count.
		///	</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="source"/> is <c>null</c>.
		///	</exception>
		/// <exception cref="ArgumentException">
		///		<paramref name="source"/> is empty.
		///		Or, the length of <paramref name="source"/> is not grator than <paramref name="offset"/>.
		///	</exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///		<paramref name="offset"/> is negative value.
		///	</exception>
		/// <exception cref="UnpackException">
		///		<paramref name="source"/> is not valid MessagePack stream.
		///	</exception>
		/// <exception cref="MessageTypeException">
		///		The unpacked result in the <paramref name="source"/> is not compatible to <see cref="System.Double" />.
		///	</exception>
		/// <remarks>
		///		When the type of packed value is not known, use <see cref="UnpackObject(byte[], int)"/> instead.
		///	</remarks>
		public static UnpackingResult<Double> UnpackDouble( byte[] source, int offset )
		{
			ValidateByteArray( source, offset );
#if !UNITY
			Contract.EndContractBlock();
#endif // !UNITY

			using( var stream = new MemoryStream( source ) )
			{
				stream.Position = offset;
				var value = UnpackDoubleCore( stream );
				return new UnpackingResult<Double>( value, unchecked( ( int )( stream.Position - offset ) ) );
			}
		}

		///	<summary>
		///		Unpacks <see cref="System.Double" /> value from the specified <see cref="Stream"/>.
		///	</summary>
		///	<param name="source">The <see cref="Stream"/> which contains Message Pack binary stream.</param>
		///	<returns>
		///		The unpacked <see cref="System.Double" /> value.
		///	</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="source"/> is <c>null</c>.
		///	</exception>
		/// <exception cref="ArgumentException">
		///		The <see cref="P:Stream.CanRead"/> of <paramref name="source"/> is <c>false</c>.
		///	</exception>
		/// <exception cref="UnpackException">
		///		<paramref name="source"/> is not valid MessagePack stream.
		///		Note that the state of <paramref name="source"/> will be unpredictable espicially it is not seekable.
		///	</exception>
		/// <exception cref="MessageTypeException">
		///		The unpacked result in the <paramref name="source"/> is not compatible to <see cref="System.Double" />.
		///		Note that the state of <paramref name="source"/> will be unpredictable espicially it is not seekable.
		///	</exception>
		/// <remarks>
		///		<para>
		/// 		The processed bytes count can be calculated via <see cref="P:Stream.Position"/> of <paramref name="source"/> when the <see cref="P:Stream.CanSeek" /> is <c>true</c>.
		///		</para>
		///		<para>
		///			When the type of packed value is not known, use <see cref="UnpackObject(Stream)"/> instead.
		///		</para>
		///	</remarks>
		public static Double UnpackDouble( Stream source )
		{
			ValidateStream( source );
#if !UNITY
			Contract.EndContractBlock();
#endif // !UNITY

			return UnpackDoubleCore( source );
		}
		
		private static Double UnpackDoubleCore( Stream source )
		{
			using ( var unpacker = Unpacker.Create( source, false ) )
			{
				UnpackOne( unpacker );
				VerifyIsScalar( unpacker );
				try
				{
					return ( Double )unpacker.LastReadData;
				}
				catch( InvalidOperationException ex )
				{
					throw NewTypeMismatchException( typeof( Double ), ex );
				}
			}
		}
	}
}