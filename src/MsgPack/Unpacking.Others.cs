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
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
		///		Unpacks the array from the head of specified byte array.
		///	</summary>
		///	<param name="source">The byte array which contains Message Pack binary stream.</param>
		///	<returns>
		///		The <see cref="UnpackingResult{T}"/> of <see cref="IList{T}" /> which contains unpacked the array and processed bytes count.
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
		///		The unpacked result in the <paramref name="source"/> is not compatible to <see cref="IList{T}" />.
		///	</exception>
		/// <exception cref="MessageNotSupportedException">
		///		The items count of the underlying collection body is over <see cref="Int32.MaxValue"/>.
		///	</exception>
		/// <remarks>
		///		<para>
		/// 		Invocation of this method is equivalant to call <see cref="UnpackArray(byte[], int)"/> with <c>offset</c> is <c>0</c>.
		///		</para>
		///		<para>
		///			When the type of packed value is not known, use <see cref="UnpackObject(byte[])"/> instead.
		///		</para>
		///	</remarks>
		[SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Generic collection as type argument is acceptable." )]
		public static UnpackingResult<IList<MessagePackObject>> UnpackArray( byte[] source )
		{
			return UnpackArray( source, 0 );
		}

		///	<summary>
		///		Unpacks the array from the specified byte array.
		///	</summary>
		///	<param name="source">The byte array which contains Message Pack binary stream.</param>
		///	<param name="offset">The offset to be unpacking start with.</param>
		///	<returns>
		///		The <see cref="UnpackingResult{T}"/> of <see cref="IList{T}" /> which contains unpacked the array and processed bytes count.
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
		///		The unpacked result in the <paramref name="source"/> is not compatible to <see cref="IList{T}" />.
		///	</exception>
		/// <exception cref="MessageNotSupportedException">
		///		The items count of the underlying collection body is over <see cref="Int32.MaxValue"/>.
		///	</exception>
		/// <remarks>
		///		When the type of packed value is not known, use <see cref="UnpackObject(byte[], int)"/> instead.
		///	</remarks>
		[SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Generic collection as type argument is acceptable." )]
		public static UnpackingResult<IList<MessagePackObject>> UnpackArray( byte[] source, int offset )
		{
			ValidateByteArray( source, offset );
#if !UNITY
			Contract.EndContractBlock();
#endif // !UNITY

			using( var stream = new MemoryStream( source ) )
			{
				stream.Position = offset;
				var value = UnpackArrayCore( stream );
				return new UnpackingResult<IList<MessagePackObject>>( value, unchecked( ( int )( stream.Position - offset ) ) );
			}
		}
		
		///	<summary>
		///		Unpacks the array value from the specified <see cref="Stream"/>.
		///	</summary>
		///	<param name="source">The <see cref="Stream"/> which contains Message Pack binary stream.</param>
		///	<returns>
		///		The unpacked the array value.
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
		///		The unpacked result in the <paramref name="source"/> is not compatible to <see cref="IList{T}" />.
		///		Note that the state of <paramref name="source"/> will be unpredictable espicially it is not seekable.
		///	</exception>
		/// <exception cref="MessageNotSupportedException">
		///		The items count of the underlying collection body is over <see cref="Int32.MaxValue"/>.
		///	</exception>
		/// <remarks>
		///		<para>
		/// 		The processed bytes count can be calculated via <see cref="P:Stream.Position"/> of <paramref name="source"/> when the <see cref="P:Stream.CanSeek" /> is <c>true</c>.
		///		</para>
		///		<para>
		///			When the type of packed value is not known, use <see cref="UnpackObject(Stream)"/> instead.
		///		</para>
		///	</remarks>
		public static IList<MessagePackObject> UnpackArray( Stream source )
		{
			ValidateStream( source );
#if !UNITY
			Contract.EndContractBlock();
#endif // !UNITY

			return UnpackArrayCore( source );
		}

		///	<summary>
		///		Unpacks length of the array from the head of specified byte array.
		///	</summary>
		///	<param name="source">The byte array which contains Message Pack binary stream.</param>
		///	<returns>
		///		The <see cref="UnpackingResult{T}"/> of nullable <see cref="Int64" /> which contains unpacked length of the array and processed bytes count.
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
		///		The unpacked result in the <paramref name="source"/> is not compatible to nullable <see cref="Int64" />.
		///	</exception>
		/// <remarks>
		///		<para>
		/// 		Invocation of this method is equivalant to call <see cref="UnpackArrayLength(byte[], int)"/> with <c>offset</c> is <c>0</c>.
		///		</para>
		///		<para>
		///			When the type of packed value is not known, use <see cref="UnpackObject(byte[])"/> instead.
		///		</para>
		///	</remarks>
		[SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Nullable<T> as type argument is acceptable." )]
		public static UnpackingResult<Int64?> UnpackArrayLength( byte[] source )
		{
			return UnpackArrayLength( source, 0 );
		}

		///	<summary>
		///		Unpacks length of the array from the specified byte array.
		///	</summary>
		///	<param name="source">The byte array which contains Message Pack binary stream.</param>
		///	<param name="offset">The offset to be unpacking start with.</param>
		///	<returns>
		///		The <see cref="UnpackingResult{T}"/> of nullable <see cref="Int64" /> which contains unpacked length of the array and processed bytes count.
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
		///		The unpacked result in the <paramref name="source"/> is not compatible to nullable <see cref="Int64" />.
		///	</exception>
		/// <remarks>
		///		When the type of packed value is not known, use <see cref="UnpackObject(byte[], int)"/> instead.
		///	</remarks>
		[SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Nullable<T> as type argument is acceptable." )]
		public static UnpackingResult<Int64?> UnpackArrayLength( byte[] source, int offset )
		{
			ValidateByteArray( source, offset );
#if !UNITY
			Contract.EndContractBlock();
#endif // !UNITY

			using( var stream = new MemoryStream( source ) )
			{
				stream.Position = offset;
				var value = UnpackArrayLengthCore( stream );
				return new UnpackingResult<Int64?>( value, unchecked( ( int )( stream.Position - offset ) ) );
			}
		}
		
		///	<summary>
		///		Unpacks length of the array value from the specified <see cref="Stream"/>.
		///	</summary>
		///	<param name="source">The <see cref="Stream"/> which contains Message Pack binary stream.</param>
		///	<returns>
		///		The unpacked length of the array value.
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
		///		The unpacked result in the <paramref name="source"/> is not compatible to nullable <see cref="Int64" />.
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
		public static Int64? UnpackArrayLength( Stream source )
		{
			ValidateStream( source );
#if !UNITY
			Contract.EndContractBlock();
#endif // !UNITY

			return UnpackArrayLengthCore( source );
		}

		///	<summary>
		///		Unpacks the dictionary from the head of specified byte array.
		///	</summary>
		///	<param name="source">The byte array which contains Message Pack binary stream.</param>
		///	<returns>
		///		The <see cref="UnpackingResult{T}"/> of <see cref="MessagePackObjectDictionary" /> which contains unpacked the dictionary and processed bytes count.
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
		///		The unpacked result in the <paramref name="source"/> is not compatible to <see cref="MessagePackObjectDictionary" />.
		///	</exception>
		/// <exception cref="MessageNotSupportedException">
		///		The items count of the underlying collection body is over <see cref="Int32.MaxValue"/>.
		///	</exception>
		/// <remarks>
		///		<para>
		/// 		Invocation of this method is equivalant to call <see cref="UnpackDictionary(byte[], int)"/> with <c>offset</c> is <c>0</c>.
		///		</para>
		///		<para>
		///			When the type of packed value is not known, use <see cref="UnpackObject(byte[])"/> instead.
		///		</para>
		///	</remarks>
		public static UnpackingResult<MessagePackObjectDictionary> UnpackDictionary( byte[] source )
		{
			return UnpackDictionary( source, 0 );
		}

		///	<summary>
		///		Unpacks the dictionary from the specified byte array.
		///	</summary>
		///	<param name="source">The byte array which contains Message Pack binary stream.</param>
		///	<param name="offset">The offset to be unpacking start with.</param>
		///	<returns>
		///		The <see cref="UnpackingResult{T}"/> of <see cref="MessagePackObjectDictionary" /> which contains unpacked the dictionary and processed bytes count.
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
		///		The unpacked result in the <paramref name="source"/> is not compatible to <see cref="MessagePackObjectDictionary" />.
		///	</exception>
		/// <exception cref="MessageNotSupportedException">
		///		The items count of the underlying collection body is over <see cref="Int32.MaxValue"/>.
		///	</exception>
		/// <remarks>
		///		When the type of packed value is not known, use <see cref="UnpackObject(byte[], int)"/> instead.
		///	</remarks>
		public static UnpackingResult<MessagePackObjectDictionary> UnpackDictionary( byte[] source, int offset )
		{
			ValidateByteArray( source, offset );
#if !UNITY
			Contract.EndContractBlock();
#endif // !UNITY

			using( var stream = new MemoryStream( source ) )
			{
				stream.Position = offset;
				var value = UnpackDictionaryCore( stream );
				return new UnpackingResult<MessagePackObjectDictionary>( value, unchecked( ( int )( stream.Position - offset ) ) );
			}
		}
		
		///	<summary>
		///		Unpacks the dictionary value from the specified <see cref="Stream"/>.
		///	</summary>
		///	<param name="source">The <see cref="Stream"/> which contains Message Pack binary stream.</param>
		///	<returns>
		///		The unpacked the dictionary value.
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
		///		The unpacked result in the <paramref name="source"/> is not compatible to <see cref="MessagePackObjectDictionary" />.
		///		Note that the state of <paramref name="source"/> will be unpredictable espicially it is not seekable.
		///	</exception>
		/// <exception cref="MessageNotSupportedException">
		///		The items count of the underlying collection body is over <see cref="Int32.MaxValue"/>.
		///	</exception>
		/// <remarks>
		///		<para>
		/// 		The processed bytes count can be calculated via <see cref="P:Stream.Position"/> of <paramref name="source"/> when the <see cref="P:Stream.CanSeek" /> is <c>true</c>.
		///		</para>
		///		<para>
		///			When the type of packed value is not known, use <see cref="UnpackObject(Stream)"/> instead.
		///		</para>
		///	</remarks>
		public static MessagePackObjectDictionary UnpackDictionary( Stream source )
		{
			ValidateStream( source );
#if !UNITY
			Contract.EndContractBlock();
#endif // !UNITY

			return UnpackDictionaryCore( source );
		}

		///	<summary>
		///		Unpacks count of the dictionary entries from the head of specified byte array.
		///	</summary>
		///	<param name="source">The byte array which contains Message Pack binary stream.</param>
		///	<returns>
		///		The <see cref="UnpackingResult{T}"/> of nullable <see cref="Int64" /> which contains unpacked count of the dictionary entries and processed bytes count.
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
		///		The unpacked result in the <paramref name="source"/> is not compatible to nullable <see cref="Int64" />.
		///	</exception>
		/// <remarks>
		///		<para>
		/// 		Invocation of this method is equivalant to call <see cref="UnpackDictionaryCount(byte[], int)"/> with <c>offset</c> is <c>0</c>.
		///		</para>
		///		<para>
		///			When the type of packed value is not known, use <see cref="UnpackObject(byte[])"/> instead.
		///		</para>
		///	</remarks>
		[SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Nullable<T> as type argument is acceptable." )]
		public static UnpackingResult<Int64?> UnpackDictionaryCount( byte[] source )
		{
			return UnpackDictionaryCount( source, 0 );
		}

		///	<summary>
		///		Unpacks count of the dictionary entries from the specified byte array.
		///	</summary>
		///	<param name="source">The byte array which contains Message Pack binary stream.</param>
		///	<param name="offset">The offset to be unpacking start with.</param>
		///	<returns>
		///		The <see cref="UnpackingResult{T}"/> of nullable <see cref="Int64" /> which contains unpacked count of the dictionary entries and processed bytes count.
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
		///		The unpacked result in the <paramref name="source"/> is not compatible to nullable <see cref="Int64" />.
		///	</exception>
		/// <remarks>
		///		When the type of packed value is not known, use <see cref="UnpackObject(byte[], int)"/> instead.
		///	</remarks>
		[SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Nullable<T> as type argument is acceptable." )]
		public static UnpackingResult<Int64?> UnpackDictionaryCount( byte[] source, int offset )
		{
			ValidateByteArray( source, offset );
#if !UNITY
			Contract.EndContractBlock();
#endif // !UNITY

			using( var stream = new MemoryStream( source ) )
			{
				stream.Position = offset;
				var value = UnpackDictionaryCountCore( stream );
				return new UnpackingResult<Int64?>( value, unchecked( ( int )( stream.Position - offset ) ) );
			}
		}
		
		///	<summary>
		///		Unpacks count of the dictionary entries value from the specified <see cref="Stream"/>.
		///	</summary>
		///	<param name="source">The <see cref="Stream"/> which contains Message Pack binary stream.</param>
		///	<returns>
		///		The unpacked count of the dictionary entries value.
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
		///		The unpacked result in the <paramref name="source"/> is not compatible to nullable <see cref="Int64" />.
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
		public static Int64? UnpackDictionaryCount( Stream source )
		{
			ValidateStream( source );
#if !UNITY
			Contract.EndContractBlock();
#endif // !UNITY

			return UnpackDictionaryCountCore( source );
		}

		///	<summary>
		///		Unpacks the raw binary from the head of specified byte array.
		///	</summary>
		///	<param name="source">The byte array which contains Message Pack binary stream.</param>
		///	<returns>
		///		The <see cref="UnpackingResult{T}"/> of <see cref="byte" />[] which contains unpacked the raw binary and processed bytes count.
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
		///		The unpacked result in the <paramref name="source"/> is not compatible to <see cref="byte" />[].
		///	</exception>
		/// <exception cref="MessageNotSupportedException">
		///		The items count of the underlying collection body is over <see cref="Int32.MaxValue"/>.
		///	</exception>
		/// <remarks>
		///		<para>
		/// 		Invocation of this method is equivalant to call <see cref="UnpackBinary(byte[], int)"/> with <c>offset</c> is <c>0</c>.
		///		</para>
		///		<para>
		///			When the type of packed value is not known, use <see cref="UnpackObject(byte[])"/> instead.
		///		</para>
		///	</remarks>
		public static UnpackingResult<byte[]> UnpackBinary( byte[] source )
		{
			return UnpackBinary( source, 0 );
		}

		///	<summary>
		///		Unpacks the raw binary from the specified byte array.
		///	</summary>
		///	<param name="source">The byte array which contains Message Pack binary stream.</param>
		///	<param name="offset">The offset to be unpacking start with.</param>
		///	<returns>
		///		The <see cref="UnpackingResult{T}"/> of <see cref="byte" />[] which contains unpacked the raw binary and processed bytes count.
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
		///		The unpacked result in the <paramref name="source"/> is not compatible to <see cref="byte" />[].
		///	</exception>
		/// <exception cref="MessageNotSupportedException">
		///		The items count of the underlying collection body is over <see cref="Int32.MaxValue"/>.
		///	</exception>
		/// <remarks>
		///		When the type of packed value is not known, use <see cref="UnpackObject(byte[], int)"/> instead.
		///	</remarks>
		public static UnpackingResult<byte[]> UnpackBinary( byte[] source, int offset )
		{
			ValidateByteArray( source, offset );
#if !UNITY
			Contract.EndContractBlock();
#endif // !UNITY

			using( var stream = new MemoryStream( source ) )
			{
				stream.Position = offset;
				var value = UnpackBinaryCore( stream );
				return new UnpackingResult<byte[]>( value, unchecked( ( int )( stream.Position - offset ) ) );
			}
		}
		
		///	<summary>
		///		Unpacks the raw binary value from the specified <see cref="Stream"/>.
		///	</summary>
		///	<param name="source">The <see cref="Stream"/> which contains Message Pack binary stream.</param>
		///	<returns>
		///		The unpacked the raw binary value.
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
		///		The unpacked result in the <paramref name="source"/> is not compatible to <see cref="byte" />[].
		///		Note that the state of <paramref name="source"/> will be unpredictable espicially it is not seekable.
		///	</exception>
		/// <exception cref="MessageNotSupportedException">
		///		The items count of the underlying collection body is over <see cref="Int32.MaxValue"/>.
		///	</exception>
		/// <remarks>
		///		<para>
		/// 		The processed bytes count can be calculated via <see cref="P:Stream.Position"/> of <paramref name="source"/> when the <see cref="P:Stream.CanSeek" /> is <c>true</c>.
		///		</para>
		///		<para>
		///			When the type of packed value is not known, use <see cref="UnpackObject(Stream)"/> instead.
		///		</para>
		///	</remarks>
		public static byte[] UnpackBinary( Stream source )
		{
			ValidateStream( source );
#if !UNITY
			Contract.EndContractBlock();
#endif // !UNITY

			return UnpackBinaryCore( source );
		}

		///	<summary>
		///		Unpacks the boolean from the head of specified byte array.
		///	</summary>
		///	<param name="source">The byte array which contains Message Pack binary stream.</param>
		///	<returns>
		///		The <see cref="UnpackingResult{T}"/> of <see cref="bool" /> which contains unpacked the boolean and processed bytes count.
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
		///		The unpacked result in the <paramref name="source"/> is not compatible to <see cref="bool" />.
		///	</exception>
		/// <remarks>
		///		<para>
		/// 		Invocation of this method is equivalant to call <see cref="UnpackBoolean(byte[], int)"/> with <c>offset</c> is <c>0</c>.
		///		</para>
		///		<para>
		///			When the type of packed value is not known, use <see cref="UnpackObject(byte[])"/> instead.
		///		</para>
		///	</remarks>
		public static UnpackingResult<bool> UnpackBoolean( byte[] source )
		{
			return UnpackBoolean( source, 0 );
		}

		///	<summary>
		///		Unpacks the boolean from the specified byte array.
		///	</summary>
		///	<param name="source">The byte array which contains Message Pack binary stream.</param>
		///	<param name="offset">The offset to be unpacking start with.</param>
		///	<returns>
		///		The <see cref="UnpackingResult{T}"/> of <see cref="bool" /> which contains unpacked the boolean and processed bytes count.
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
		///		The unpacked result in the <paramref name="source"/> is not compatible to <see cref="bool" />.
		///	</exception>
		/// <remarks>
		///		When the type of packed value is not known, use <see cref="UnpackObject(byte[], int)"/> instead.
		///	</remarks>
		public static UnpackingResult<bool> UnpackBoolean( byte[] source, int offset )
		{
			ValidateByteArray( source, offset );
#if !UNITY
			Contract.EndContractBlock();
#endif // !UNITY

			using( var stream = new MemoryStream( source ) )
			{
				stream.Position = offset;
				var value = UnpackBooleanCore( stream );
				return new UnpackingResult<bool>( value, unchecked( ( int )( stream.Position - offset ) ) );
			}
		}
		
		///	<summary>
		///		Unpacks the boolean value from the specified <see cref="Stream"/>.
		///	</summary>
		///	<param name="source">The <see cref="Stream"/> which contains Message Pack binary stream.</param>
		///	<returns>
		///		The unpacked the boolean value.
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
		///		The unpacked result in the <paramref name="source"/> is not compatible to <see cref="bool" />.
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
		public static bool UnpackBoolean( Stream source )
		{
			ValidateStream( source );
#if !UNITY
			Contract.EndContractBlock();
#endif // !UNITY

			return UnpackBooleanCore( source );
		}

		///	<summary>
		///		Unpacks the nil from the head of specified byte array.
		///	</summary>
		///	<param name="source">The byte array which contains Message Pack binary stream.</param>
		///	<returns>
		///		The <see cref="UnpackingResult{T}"/> of <see cref="object" /> which contains unpacked the nil and processed bytes count.
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
		///		The unpacked result in the <paramref name="source"/> is not compatible to <see cref="object" />.
		///	</exception>
		/// <remarks>
		///		<para>
		/// 		Invocation of this method is equivalant to call <see cref="UnpackNull(byte[], int)"/> with <c>offset</c> is <c>0</c>.
		///		</para>
		///		<para>
		///			When the type of packed value is not known, use <see cref="UnpackObject(byte[])"/> instead.
		///		</para>
		///	</remarks>
		public static UnpackingResult<object> UnpackNull( byte[] source )
		{
			return UnpackNull( source, 0 );
		}

		///	<summary>
		///		Unpacks the nil from the specified byte array.
		///	</summary>
		///	<param name="source">The byte array which contains Message Pack binary stream.</param>
		///	<param name="offset">The offset to be unpacking start with.</param>
		///	<returns>
		///		The <see cref="UnpackingResult{T}"/> of <see cref="object" /> which contains unpacked the nil and processed bytes count.
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
		///		The unpacked result in the <paramref name="source"/> is not compatible to <see cref="object" />.
		///	</exception>
		/// <remarks>
		///		When the type of packed value is not known, use <see cref="UnpackObject(byte[], int)"/> instead.
		///	</remarks>
		public static UnpackingResult<object> UnpackNull( byte[] source, int offset )
		{
			ValidateByteArray( source, offset );
#if !UNITY
			Contract.EndContractBlock();
#endif // !UNITY

			using( var stream = new MemoryStream( source ) )
			{
				stream.Position = offset;
				var value = UnpackNullCore( stream );
				return new UnpackingResult<object>( value, unchecked( ( int )( stream.Position - offset ) ) );
			}
		}
		
		///	<summary>
		///		Unpacks the nil value from the specified <see cref="Stream"/>.
		///	</summary>
		///	<param name="source">The <see cref="Stream"/> which contains Message Pack binary stream.</param>
		///	<returns>
		///		The unpacked the nil value.
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
		///		The unpacked result in the <paramref name="source"/> is not compatible to <see cref="object" />.
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
		public static object UnpackNull( Stream source )
		{
			ValidateStream( source );
#if !UNITY
			Contract.EndContractBlock();
#endif // !UNITY

			return UnpackNullCore( source );
		}

		///	<summary>
		///		Unpacks the <see cref="MessagePackObject" /> which represents the value which has MessagePack type semantics. from the head of specified byte array.
		///	</summary>
		///	<param name="source">The byte array which contains Message Pack binary stream.</param>
		///	<returns>
		///		The <see cref="UnpackingResult{T}"/> of <see cref="MessagePackObject" /> which contains unpacked the <see cref="MessagePackObject" /> which represents the value which has MessagePack type semantics. and processed bytes count.
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
		///		The unpacked result in the <paramref name="source"/> is not compatible to <see cref="MessagePackObject" />.
		///	</exception>
		/// <remarks>
		///		<para>
		/// 		Invocation of this method is equivalant to call <see cref="UnpackObject(byte[], int)"/> with <c>offset</c> is <c>0</c>.
		///		</para>
		///		<para>
		///			When the type of packed value is not known, use <see cref="UnpackObject(byte[])"/> instead.
		///		</para>
		///	</remarks>
		public static UnpackingResult<MessagePackObject> UnpackObject( byte[] source )
		{
			return UnpackObject( source, 0 );
		}

		///	<summary>
		///		Unpacks the <see cref="MessagePackObject" /> which represents the value which has MessagePack type semantics. from the specified byte array.
		///	</summary>
		///	<param name="source">The byte array which contains Message Pack binary stream.</param>
		///	<param name="offset">The offset to be unpacking start with.</param>
		///	<returns>
		///		The <see cref="UnpackingResult{T}"/> of <see cref="MessagePackObject" /> which contains unpacked the <see cref="MessagePackObject" /> which represents the value which has MessagePack type semantics. and processed bytes count.
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
		///		The unpacked result in the <paramref name="source"/> is not compatible to <see cref="MessagePackObject" />.
		///	</exception>
		/// <remarks>
		///		When the type of packed value is not known, use <see cref="UnpackObject(byte[], int)"/> instead.
		///	</remarks>
		public static UnpackingResult<MessagePackObject> UnpackObject( byte[] source, int offset )
		{
			ValidateByteArray( source, offset );
#if !UNITY
			Contract.EndContractBlock();
#endif // !UNITY

			using( var stream = new MemoryStream( source ) )
			{
				stream.Position = offset;
				var value = UnpackObjectCore( stream );
				return new UnpackingResult<MessagePackObject>( value, unchecked( ( int )( stream.Position - offset ) ) );
			}
		}
		
		///	<summary>
		///		Unpacks the <see cref="MessagePackObject" /> which represents the value which has MessagePack type semantics. value from the specified <see cref="Stream"/>.
		///	</summary>
		///	<param name="source">The <see cref="Stream"/> which contains Message Pack binary stream.</param>
		///	<returns>
		///		The unpacked the <see cref="MessagePackObject" /> which represents the value which has MessagePack type semantics. value.
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
		///		The unpacked result in the <paramref name="source"/> is not compatible to <see cref="MessagePackObject" />.
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
		public static MessagePackObject UnpackObject( Stream source )
		{
			ValidateStream( source );
#if !UNITY
			Contract.EndContractBlock();
#endif // !UNITY

			return UnpackObjectCore( source );
		}

		///	<summary>
		///		Unpacks the <see cref="MessagePackExtendedTypeObject" /> which represents the extended type value. from the head of specified byte array.
		///	</summary>
		///	<param name="source">The byte array which contains Message Pack binary stream.</param>
		///	<returns>
		///		The <see cref="UnpackingResult{T}"/> of <see cref="MessagePackExtendedTypeObject" /> which contains unpacked the <see cref="MessagePackExtendedTypeObject" /> which represents the extended type value. and processed bytes count.
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
		///		The unpacked result in the <paramref name="source"/> is not compatible to <see cref="MessagePackExtendedTypeObject" />.
		///	</exception>
		/// <exception cref="MessageNotSupportedException">
		///		The items count of the underlying collection body is over <see cref="Int32.MaxValue"/>.
		///	</exception>
		/// <remarks>
		///		<para>
		/// 		Invocation of this method is equivalant to call <see cref="UnpackExtendedTypeObject(byte[], int)"/> with <c>offset</c> is <c>0</c>.
		///		</para>
		///		<para>
		///			When the type of packed value is not known, use <see cref="UnpackObject(byte[])"/> instead.
		///		</para>
		///	</remarks>
		public static UnpackingResult<MessagePackExtendedTypeObject> UnpackExtendedTypeObject( byte[] source )
		{
			return UnpackExtendedTypeObject( source, 0 );
		}

		///	<summary>
		///		Unpacks the <see cref="MessagePackExtendedTypeObject" /> which represents the extended type value. from the specified byte array.
		///	</summary>
		///	<param name="source">The byte array which contains Message Pack binary stream.</param>
		///	<param name="offset">The offset to be unpacking start with.</param>
		///	<returns>
		///		The <see cref="UnpackingResult{T}"/> of <see cref="MessagePackExtendedTypeObject" /> which contains unpacked the <see cref="MessagePackExtendedTypeObject" /> which represents the extended type value. and processed bytes count.
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
		///		The unpacked result in the <paramref name="source"/> is not compatible to <see cref="MessagePackExtendedTypeObject" />.
		///	</exception>
		/// <exception cref="MessageNotSupportedException">
		///		The items count of the underlying collection body is over <see cref="Int32.MaxValue"/>.
		///	</exception>
		/// <remarks>
		///		When the type of packed value is not known, use <see cref="UnpackObject(byte[], int)"/> instead.
		///	</remarks>
		public static UnpackingResult<MessagePackExtendedTypeObject> UnpackExtendedTypeObject( byte[] source, int offset )
		{
			ValidateByteArray( source, offset );
#if !UNITY
			Contract.EndContractBlock();
#endif // !UNITY

			using( var stream = new MemoryStream( source ) )
			{
				stream.Position = offset;
				var value = UnpackExtendedTypeObjectCore( stream );
				return new UnpackingResult<MessagePackExtendedTypeObject>( value, unchecked( ( int )( stream.Position - offset ) ) );
			}
		}
		
		///	<summary>
		///		Unpacks the <see cref="MessagePackExtendedTypeObject" /> which represents the extended type value. value from the specified <see cref="Stream"/>.
		///	</summary>
		///	<param name="source">The <see cref="Stream"/> which contains Message Pack binary stream.</param>
		///	<returns>
		///		The unpacked the <see cref="MessagePackExtendedTypeObject" /> which represents the extended type value. value.
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
		///		The unpacked result in the <paramref name="source"/> is not compatible to <see cref="MessagePackExtendedTypeObject" />.
		///		Note that the state of <paramref name="source"/> will be unpredictable espicially it is not seekable.
		///	</exception>
		/// <exception cref="MessageNotSupportedException">
		///		The items count of the underlying collection body is over <see cref="Int32.MaxValue"/>.
		///	</exception>
		/// <remarks>
		///		<para>
		/// 		The processed bytes count can be calculated via <see cref="P:Stream.Position"/> of <paramref name="source"/> when the <see cref="P:Stream.CanSeek" /> is <c>true</c>.
		///		</para>
		///		<para>
		///			When the type of packed value is not known, use <see cref="UnpackObject(Stream)"/> instead.
		///		</para>
		///	</remarks>
		public static MessagePackExtendedTypeObject UnpackExtendedTypeObject( Stream source )
		{
			ValidateStream( source );
#if !UNITY
			Contract.EndContractBlock();
#endif // !UNITY

			return UnpackExtendedTypeObjectCore( source );
		}

	}
}
