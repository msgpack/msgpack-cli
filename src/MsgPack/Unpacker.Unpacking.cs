 
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
	// This file was generated from Unpacker.Unpacking.tt and StreamingUnapkcerBase.ttinclude T4Template.
	// Do not modify this file. Edit Unpacker.Unpacking.tt and StreamingUnapkcerBase.ttinclude instead.

	partial class Unpacker
	{
		/// <summary>
		///		Reads next <see cref="Boolean" /> value from current stream.
		///	</summary>
		/// <param name="result">
		///		The <see cref="Boolean" /> value read from current stream to be stored when operation is succeeded.
		/// </param>
		/// <returns>
		///		<c>true</c> if expected value was read from stream; <c>false</c> if no more data on the stream.
		///		Note that this method throws exception for unexpected state. See exceptions section.
		/// </returns>
		/// <exception cref="InvalidMessagePackStreamException">
		///		Cannot read a value because the underlying stream unexpectedly ends.
		/// </exception>
		/// <exception cref="MessageTypeException">
		///		A value read from data source is not compatible for the <see cref="Boolean" /> type.
		/// </exception>
#if !UNITY
		[CLSCompliant( false )]
#endif // !UNITY
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "0#", Justification = "Using nullable return value is very slow" )]
		public virtual bool ReadBoolean( out Boolean result )
		{
			if( !this.Read() )
			{
				result = default( Boolean );
				return false;
			}

			result = this.LastReadData.AsBoolean();
			return true;
		}

		/// <summary>
		///		Reads next nullable <see cref="Boolean" /> value from current stream.
		///	</summary>
		/// <returns>
		///		The nullable <see cref="Boolean" /> value read from current data source successfully.
		/// </returns>
		/// <exception cref="InvalidMessagePackStreamException">
		///		Cannot read a value because the underlying stream unexpectedly ends.
		/// </exception>
		/// <exception cref="MessageTypeException">
		///		A value read from data source is not compatible for the nullable <see cref="Boolean" /> type.
		/// </exception>
#if !UNITY
		[CLSCompliant( false )]
#endif // !UNITY
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "0#", Justification = "Adopting same pattern as non-nullables" )]
		public virtual bool ReadNullableBoolean( out Boolean? result )
		{
			if( !this.Read() )
			{
				result = null;
				return false;
			}

			result = this.LastReadData.IsNil ? default( Boolean? ) : this.LastReadData.AsBoolean();
			return true;
		}

		/// <summary>
		///		Reads next <see cref="Byte" /> value from current stream.
		///	</summary>
		/// <param name="result">
		///		The <see cref="Byte" /> value read from current stream to be stored when operation is succeeded.
		/// </param>
		/// <returns>
		///		<c>true</c> if expected value was read from stream; <c>false</c> if no more data on the stream.
		///		Note that this method throws exception for unexpected state. See exceptions section.
		/// </returns>
		/// <exception cref="InvalidMessagePackStreamException">
		///		Cannot read a value because the underlying stream unexpectedly ends.
		/// </exception>
		/// <exception cref="MessageTypeException">
		///		A value read from data source is not compatible for the <see cref="Byte" /> type.
		/// </exception>
#if !UNITY
		[CLSCompliant( false )]
#endif // !UNITY
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "0#", Justification = "Using nullable return value is very slow" )]
		public virtual bool ReadByte( out Byte result )
		{
			if( !this.Read() )
			{
				result = default( Byte );
				return false;
			}

			result = this.LastReadData.AsByte();
			return true;
		}

		/// <summary>
		///		Reads next nullable <see cref="Byte" /> value from current stream.
		///	</summary>
		/// <returns>
		///		The nullable <see cref="Byte" /> value read from current data source successfully.
		/// </returns>
		/// <exception cref="InvalidMessagePackStreamException">
		///		Cannot read a value because the underlying stream unexpectedly ends.
		/// </exception>
		/// <exception cref="MessageTypeException">
		///		A value read from data source is not compatible for the nullable <see cref="Byte" /> type.
		/// </exception>
#if !UNITY
		[CLSCompliant( false )]
#endif // !UNITY
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "0#", Justification = "Adopting same pattern as non-nullables" )]
		public virtual bool ReadNullableByte( out Byte? result )
		{
			if( !this.Read() )
			{
				result = null;
				return false;
			}

			result = this.LastReadData.IsNil ? default( Byte? ) : this.LastReadData.AsByte();
			return true;
		}

		/// <summary>
		///		Reads next <see cref="SByte" /> value from current stream.
		///	</summary>
		/// <param name="result">
		///		The <see cref="SByte" /> value read from current stream to be stored when operation is succeeded.
		/// </param>
		/// <returns>
		///		<c>true</c> if expected value was read from stream; <c>false</c> if no more data on the stream.
		///		Note that this method throws exception for unexpected state. See exceptions section.
		/// </returns>
		/// <exception cref="InvalidMessagePackStreamException">
		///		Cannot read a value because the underlying stream unexpectedly ends.
		/// </exception>
		/// <exception cref="MessageTypeException">
		///		A value read from data source is not compatible for the <see cref="SByte" /> type.
		/// </exception>
#if !UNITY
		[CLSCompliant( false )]
#endif // !UNITY
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "0#", Justification = "Using nullable return value is very slow" )]
		public virtual bool ReadSByte( out SByte result )
		{
			if( !this.Read() )
			{
				result = default( SByte );
				return false;
			}

			result = this.LastReadData.AsSByte();
			return true;
		}

		/// <summary>
		///		Reads next nullable <see cref="SByte" /> value from current stream.
		///	</summary>
		/// <returns>
		///		The nullable <see cref="SByte" /> value read from current data source successfully.
		/// </returns>
		/// <exception cref="InvalidMessagePackStreamException">
		///		Cannot read a value because the underlying stream unexpectedly ends.
		/// </exception>
		/// <exception cref="MessageTypeException">
		///		A value read from data source is not compatible for the nullable <see cref="SByte" /> type.
		/// </exception>
#if !UNITY
		[CLSCompliant( false )]
#endif // !UNITY
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "0#", Justification = "Adopting same pattern as non-nullables" )]
		public virtual bool ReadNullableSByte( out SByte? result )
		{
			if( !this.Read() )
			{
				result = null;
				return false;
			}

			result = this.LastReadData.IsNil ? default( SByte? ) : this.LastReadData.AsSByte();
			return true;
		}

		/// <summary>
		///		Reads next <see cref="Int16" /> value from current stream.
		///	</summary>
		/// <param name="result">
		///		The <see cref="Int16" /> value read from current stream to be stored when operation is succeeded.
		/// </param>
		/// <returns>
		///		<c>true</c> if expected value was read from stream; <c>false</c> if no more data on the stream.
		///		Note that this method throws exception for unexpected state. See exceptions section.
		/// </returns>
		/// <exception cref="InvalidMessagePackStreamException">
		///		Cannot read a value because the underlying stream unexpectedly ends.
		/// </exception>
		/// <exception cref="MessageTypeException">
		///		A value read from data source is not compatible for the <see cref="Int16" /> type.
		/// </exception>
#if !UNITY
		[CLSCompliant( false )]
#endif // !UNITY
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "0#", Justification = "Using nullable return value is very slow" )]
		public virtual bool ReadInt16( out Int16 result )
		{
			if( !this.Read() )
			{
				result = default( Int16 );
				return false;
			}

			result = this.LastReadData.AsInt16();
			return true;
		}

		/// <summary>
		///		Reads next nullable <see cref="Int16" /> value from current stream.
		///	</summary>
		/// <returns>
		///		The nullable <see cref="Int16" /> value read from current data source successfully.
		/// </returns>
		/// <exception cref="InvalidMessagePackStreamException">
		///		Cannot read a value because the underlying stream unexpectedly ends.
		/// </exception>
		/// <exception cref="MessageTypeException">
		///		A value read from data source is not compatible for the nullable <see cref="Int16" /> type.
		/// </exception>
#if !UNITY
		[CLSCompliant( false )]
#endif // !UNITY
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "0#", Justification = "Adopting same pattern as non-nullables" )]
		public virtual bool ReadNullableInt16( out Int16? result )
		{
			if( !this.Read() )
			{
				result = null;
				return false;
			}

			result = this.LastReadData.IsNil ? default( Int16? ) : this.LastReadData.AsInt16();
			return true;
		}

		/// <summary>
		///		Reads next <see cref="UInt16" /> value from current stream.
		///	</summary>
		/// <param name="result">
		///		The <see cref="UInt16" /> value read from current stream to be stored when operation is succeeded.
		/// </param>
		/// <returns>
		///		<c>true</c> if expected value was read from stream; <c>false</c> if no more data on the stream.
		///		Note that this method throws exception for unexpected state. See exceptions section.
		/// </returns>
		/// <exception cref="InvalidMessagePackStreamException">
		///		Cannot read a value because the underlying stream unexpectedly ends.
		/// </exception>
		/// <exception cref="MessageTypeException">
		///		A value read from data source is not compatible for the <see cref="UInt16" /> type.
		/// </exception>
#if !UNITY
		[CLSCompliant( false )]
#endif // !UNITY
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "0#", Justification = "Using nullable return value is very slow" )]
		public virtual bool ReadUInt16( out UInt16 result )
		{
			if( !this.Read() )
			{
				result = default( UInt16 );
				return false;
			}

			result = this.LastReadData.AsUInt16();
			return true;
		}

		/// <summary>
		///		Reads next nullable <see cref="UInt16" /> value from current stream.
		///	</summary>
		/// <returns>
		///		The nullable <see cref="UInt16" /> value read from current data source successfully.
		/// </returns>
		/// <exception cref="InvalidMessagePackStreamException">
		///		Cannot read a value because the underlying stream unexpectedly ends.
		/// </exception>
		/// <exception cref="MessageTypeException">
		///		A value read from data source is not compatible for the nullable <see cref="UInt16" /> type.
		/// </exception>
#if !UNITY
		[CLSCompliant( false )]
#endif // !UNITY
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "0#", Justification = "Adopting same pattern as non-nullables" )]
		public virtual bool ReadNullableUInt16( out UInt16? result )
		{
			if( !this.Read() )
			{
				result = null;
				return false;
			}

			result = this.LastReadData.IsNil ? default( UInt16? ) : this.LastReadData.AsUInt16();
			return true;
		}

		/// <summary>
		///		Reads next <see cref="Int32" /> value from current stream.
		///	</summary>
		/// <param name="result">
		///		The <see cref="Int32" /> value read from current stream to be stored when operation is succeeded.
		/// </param>
		/// <returns>
		///		<c>true</c> if expected value was read from stream; <c>false</c> if no more data on the stream.
		///		Note that this method throws exception for unexpected state. See exceptions section.
		/// </returns>
		/// <exception cref="InvalidMessagePackStreamException">
		///		Cannot read a value because the underlying stream unexpectedly ends.
		/// </exception>
		/// <exception cref="MessageTypeException">
		///		A value read from data source is not compatible for the <see cref="Int32" /> type.
		/// </exception>
#if !UNITY
		[CLSCompliant( false )]
#endif // !UNITY
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "0#", Justification = "Using nullable return value is very slow" )]
		public virtual bool ReadInt32( out Int32 result )
		{
			if( !this.Read() )
			{
				result = default( Int32 );
				return false;
			}

			result = this.LastReadData.AsInt32();
			return true;
		}

		/// <summary>
		///		Reads next nullable <see cref="Int32" /> value from current stream.
		///	</summary>
		/// <returns>
		///		The nullable <see cref="Int32" /> value read from current data source successfully.
		/// </returns>
		/// <exception cref="InvalidMessagePackStreamException">
		///		Cannot read a value because the underlying stream unexpectedly ends.
		/// </exception>
		/// <exception cref="MessageTypeException">
		///		A value read from data source is not compatible for the nullable <see cref="Int32" /> type.
		/// </exception>
#if !UNITY
		[CLSCompliant( false )]
#endif // !UNITY
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "0#", Justification = "Adopting same pattern as non-nullables" )]
		public virtual bool ReadNullableInt32( out Int32? result )
		{
			if( !this.Read() )
			{
				result = null;
				return false;
			}

			result = this.LastReadData.IsNil ? default( Int32? ) : this.LastReadData.AsInt32();
			return true;
		}

		/// <summary>
		///		Reads next <see cref="UInt32" /> value from current stream.
		///	</summary>
		/// <param name="result">
		///		The <see cref="UInt32" /> value read from current stream to be stored when operation is succeeded.
		/// </param>
		/// <returns>
		///		<c>true</c> if expected value was read from stream; <c>false</c> if no more data on the stream.
		///		Note that this method throws exception for unexpected state. See exceptions section.
		/// </returns>
		/// <exception cref="InvalidMessagePackStreamException">
		///		Cannot read a value because the underlying stream unexpectedly ends.
		/// </exception>
		/// <exception cref="MessageTypeException">
		///		A value read from data source is not compatible for the <see cref="UInt32" /> type.
		/// </exception>
#if !UNITY
		[CLSCompliant( false )]
#endif // !UNITY
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "0#", Justification = "Using nullable return value is very slow" )]
		public virtual bool ReadUInt32( out UInt32 result )
		{
			if( !this.Read() )
			{
				result = default( UInt32 );
				return false;
			}

			result = this.LastReadData.AsUInt32();
			return true;
		}

		/// <summary>
		///		Reads next nullable <see cref="UInt32" /> value from current stream.
		///	</summary>
		/// <returns>
		///		The nullable <see cref="UInt32" /> value read from current data source successfully.
		/// </returns>
		/// <exception cref="InvalidMessagePackStreamException">
		///		Cannot read a value because the underlying stream unexpectedly ends.
		/// </exception>
		/// <exception cref="MessageTypeException">
		///		A value read from data source is not compatible for the nullable <see cref="UInt32" /> type.
		/// </exception>
#if !UNITY
		[CLSCompliant( false )]
#endif // !UNITY
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "0#", Justification = "Adopting same pattern as non-nullables" )]
		public virtual bool ReadNullableUInt32( out UInt32? result )
		{
			if( !this.Read() )
			{
				result = null;
				return false;
			}

			result = this.LastReadData.IsNil ? default( UInt32? ) : this.LastReadData.AsUInt32();
			return true;
		}

		/// <summary>
		///		Reads next <see cref="Int64" /> value from current stream.
		///	</summary>
		/// <param name="result">
		///		The <see cref="Int64" /> value read from current stream to be stored when operation is succeeded.
		/// </param>
		/// <returns>
		///		<c>true</c> if expected value was read from stream; <c>false</c> if no more data on the stream.
		///		Note that this method throws exception for unexpected state. See exceptions section.
		/// </returns>
		/// <exception cref="InvalidMessagePackStreamException">
		///		Cannot read a value because the underlying stream unexpectedly ends.
		/// </exception>
		/// <exception cref="MessageTypeException">
		///		A value read from data source is not compatible for the <see cref="Int64" /> type.
		/// </exception>
#if !UNITY
		[CLSCompliant( false )]
#endif // !UNITY
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "0#", Justification = "Using nullable return value is very slow" )]
		public virtual bool ReadInt64( out Int64 result )
		{
			if( !this.Read() )
			{
				result = default( Int64 );
				return false;
			}

			result = this.LastReadData.AsInt64();
			return true;
		}

		/// <summary>
		///		Reads next nullable <see cref="Int64" /> value from current stream.
		///	</summary>
		/// <returns>
		///		The nullable <see cref="Int64" /> value read from current data source successfully.
		/// </returns>
		/// <exception cref="InvalidMessagePackStreamException">
		///		Cannot read a value because the underlying stream unexpectedly ends.
		/// </exception>
		/// <exception cref="MessageTypeException">
		///		A value read from data source is not compatible for the nullable <see cref="Int64" /> type.
		/// </exception>
#if !UNITY
		[CLSCompliant( false )]
#endif // !UNITY
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "0#", Justification = "Adopting same pattern as non-nullables" )]
		public virtual bool ReadNullableInt64( out Int64? result )
		{
			if( !this.Read() )
			{
				result = null;
				return false;
			}

			result = this.LastReadData.IsNil ? default( Int64? ) : this.LastReadData.AsInt64();
			return true;
		}

		/// <summary>
		///		Reads next <see cref="UInt64" /> value from current stream.
		///	</summary>
		/// <param name="result">
		///		The <see cref="UInt64" /> value read from current stream to be stored when operation is succeeded.
		/// </param>
		/// <returns>
		///		<c>true</c> if expected value was read from stream; <c>false</c> if no more data on the stream.
		///		Note that this method throws exception for unexpected state. See exceptions section.
		/// </returns>
		/// <exception cref="InvalidMessagePackStreamException">
		///		Cannot read a value because the underlying stream unexpectedly ends.
		/// </exception>
		/// <exception cref="MessageTypeException">
		///		A value read from data source is not compatible for the <see cref="UInt64" /> type.
		/// </exception>
#if !UNITY
		[CLSCompliant( false )]
#endif // !UNITY
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "0#", Justification = "Using nullable return value is very slow" )]
		public virtual bool ReadUInt64( out UInt64 result )
		{
			if( !this.Read() )
			{
				result = default( UInt64 );
				return false;
			}

			result = this.LastReadData.AsUInt64();
			return true;
		}

		/// <summary>
		///		Reads next nullable <see cref="UInt64" /> value from current stream.
		///	</summary>
		/// <returns>
		///		The nullable <see cref="UInt64" /> value read from current data source successfully.
		/// </returns>
		/// <exception cref="InvalidMessagePackStreamException">
		///		Cannot read a value because the underlying stream unexpectedly ends.
		/// </exception>
		/// <exception cref="MessageTypeException">
		///		A value read from data source is not compatible for the nullable <see cref="UInt64" /> type.
		/// </exception>
#if !UNITY
		[CLSCompliant( false )]
#endif // !UNITY
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "0#", Justification = "Adopting same pattern as non-nullables" )]
		public virtual bool ReadNullableUInt64( out UInt64? result )
		{
			if( !this.Read() )
			{
				result = null;
				return false;
			}

			result = this.LastReadData.IsNil ? default( UInt64? ) : this.LastReadData.AsUInt64();
			return true;
		}

		/// <summary>
		///		Reads next <see cref="Single" /> value from current stream.
		///	</summary>
		/// <param name="result">
		///		The <see cref="Single" /> value read from current stream to be stored when operation is succeeded.
		/// </param>
		/// <returns>
		///		<c>true</c> if expected value was read from stream; <c>false</c> if no more data on the stream.
		///		Note that this method throws exception for unexpected state. See exceptions section.
		/// </returns>
		/// <exception cref="InvalidMessagePackStreamException">
		///		Cannot read a value because the underlying stream unexpectedly ends.
		/// </exception>
		/// <exception cref="MessageTypeException">
		///		A value read from data source is not compatible for the <see cref="Single" /> type.
		/// </exception>
#if !UNITY
		[CLSCompliant( false )]
#endif // !UNITY
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "0#", Justification = "Using nullable return value is very slow" )]
		public virtual bool ReadSingle( out Single result )
		{
			if( !this.Read() )
			{
				result = default( Single );
				return false;
			}

			result = this.LastReadData.AsSingle();
			return true;
		}

		/// <summary>
		///		Reads next nullable <see cref="Single" /> value from current stream.
		///	</summary>
		/// <returns>
		///		The nullable <see cref="Single" /> value read from current data source successfully.
		/// </returns>
		/// <exception cref="InvalidMessagePackStreamException">
		///		Cannot read a value because the underlying stream unexpectedly ends.
		/// </exception>
		/// <exception cref="MessageTypeException">
		///		A value read from data source is not compatible for the nullable <see cref="Single" /> type.
		/// </exception>
#if !UNITY
		[CLSCompliant( false )]
#endif // !UNITY
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "0#", Justification = "Adopting same pattern as non-nullables" )]
		public virtual bool ReadNullableSingle( out Single? result )
		{
			if( !this.Read() )
			{
				result = null;
				return false;
			}

			result = this.LastReadData.IsNil ? default( Single? ) : this.LastReadData.AsSingle();
			return true;
		}

		/// <summary>
		///		Reads next <see cref="Double" /> value from current stream.
		///	</summary>
		/// <param name="result">
		///		The <see cref="Double" /> value read from current stream to be stored when operation is succeeded.
		/// </param>
		/// <returns>
		///		<c>true</c> if expected value was read from stream; <c>false</c> if no more data on the stream.
		///		Note that this method throws exception for unexpected state. See exceptions section.
		/// </returns>
		/// <exception cref="InvalidMessagePackStreamException">
		///		Cannot read a value because the underlying stream unexpectedly ends.
		/// </exception>
		/// <exception cref="MessageTypeException">
		///		A value read from data source is not compatible for the <see cref="Double" /> type.
		/// </exception>
#if !UNITY
		[CLSCompliant( false )]
#endif // !UNITY
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "0#", Justification = "Using nullable return value is very slow" )]
		public virtual bool ReadDouble( out Double result )
		{
			if( !this.Read() )
			{
				result = default( Double );
				return false;
			}

			result = this.LastReadData.AsDouble();
			return true;
		}

		/// <summary>
		///		Reads next nullable <see cref="Double" /> value from current stream.
		///	</summary>
		/// <returns>
		///		The nullable <see cref="Double" /> value read from current data source successfully.
		/// </returns>
		/// <exception cref="InvalidMessagePackStreamException">
		///		Cannot read a value because the underlying stream unexpectedly ends.
		/// </exception>
		/// <exception cref="MessageTypeException">
		///		A value read from data source is not compatible for the nullable <see cref="Double" /> type.
		/// </exception>
#if !UNITY
		[CLSCompliant( false )]
#endif // !UNITY
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "0#", Justification = "Adopting same pattern as non-nullables" )]
		public virtual bool ReadNullableDouble( out Double? result )
		{
			if( !this.Read() )
			{
				result = null;
				return false;
			}

			result = this.LastReadData.IsNil ? default( Double? ) : this.LastReadData.AsDouble();
			return true;
		}

		/// <summary>
		///		Reads next <see cref="MessagePackExtendedTypeObject" /> value from current stream.
		///	</summary>
		/// <param name="result">
		///		The <see cref="MessagePackExtendedTypeObject" /> value read from current stream to be stored when operation is succeeded.
		/// </param>
		/// <returns>
		///		<c>true</c> if expected value was read from stream; <c>false</c> if no more data on the stream.
		///		Note that this method throws exception for unexpected state. See exceptions section.
		/// </returns>
		/// <exception cref="InvalidMessagePackStreamException">
		///		Cannot read a value because the underlying stream unexpectedly ends.
		/// </exception>
		/// <exception cref="MessageTypeException">
		///		A value read from data source is not compatible for the <see cref="MessagePackExtendedTypeObject" /> type.
		/// </exception>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "0#", Justification = "Using nullable return value is very slow" )]
		public virtual bool ReadMessagePackExtendedTypeObject( out MessagePackExtendedTypeObject result )
		{
			if( !this.Read() )
			{
				result = default( MessagePackExtendedTypeObject );
				return false;
			}

			result = this.LastReadData.AsMessagePackExtendedTypeObject();
			return true;
		}

		/// <summary>
		///		Reads next nullable <see cref="MessagePackExtendedTypeObject" /> value from current stream.
		///	</summary>
		/// <returns>
		///		The nullable <see cref="MessagePackExtendedTypeObject" /> value read from current data source successfully.
		/// </returns>
		/// <exception cref="InvalidMessagePackStreamException">
		///		Cannot read a value because the underlying stream unexpectedly ends.
		/// </exception>
		/// <exception cref="MessageTypeException">
		///		A value read from data source is not compatible for the nullable <see cref="MessagePackExtendedTypeObject" /> type.
		/// </exception>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "0#", Justification = "Adopting same pattern as non-nullables" )]
		public virtual bool ReadNullableMessagePackExtendedTypeObject( out MessagePackExtendedTypeObject? result )
		{
			if( !this.Read() )
			{
				result = null;
				return false;
			}

			result = this.LastReadData.IsNil ? default( MessagePackExtendedTypeObject? ) : this.LastReadData.AsMessagePackExtendedTypeObject();
			return true;
		}


		/// <summary>
		///		Reads next array length value from current stream.
		///	</summary>
		/// <param name="result">
		///		The array length read from current stream to be stored when operation is succeeded.
		/// </param>
		/// <returns>
		///		<c>true</c> if expected value was read from stream; <c>false</c> if no more data on the stream.
		///		Note that this method throws exception for unexpected state. See exceptions section.
		/// </returns>
		/// <exception cref="InvalidMessagePackStreamException">
		///		Cannot read a value because the underlying stream unexpectedly ends.
		/// </exception>
		/// <exception cref="MessageTypeException">
		///		A value read from data source is not an array.
		/// </exception>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "0#", Justification = "Using nullable return value is very slow" )]
		public virtual bool ReadArrayLength( out long result )
		{
			if( !this.Read() )
			{
				result = 0;
				return false;
			}

			if( !this.IsArrayHeader )
			{
				throw new MessageTypeException( "Not in map header." );
			}

			result = this.LastReadData.AsInt64();
			return true;
		}

		/// <summary>
		///		Reads next map length value from current stream.
		///	</summary>
		/// <param name="result">
		///		The map length read from current stream to be stored when operation is succeeded.
		/// </param>
		/// <returns>
		///		<c>true</c> if expected value was read from stream; <c>false</c> if no more data on the stream.
		///		Note that this method throws exception for unexpected state. See exceptions section.
		/// </returns>
		/// <exception cref="InvalidMessagePackStreamException">
		///		Cannot read a value because the underlying stream unexpectedly ends.
		/// </exception>
		/// <exception cref="MessageTypeException">
		///		A value read from data source is not a map.
		/// </exception>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "0#", Justification = "Using nullable return value is very slow" )]
		public virtual bool ReadMapLength( out long result )
		{
			if( !this.Read())
			{
				result = 0;
				return false;
			}

			if( !this.IsMapHeader )
			{
				throw new MessageTypeException( "Not in map header." );
			}

			result = this.LastReadData.AsInt64();
			return true;
		}

		/// <summary>
		///		Reads next byte array value from current stream.
		///	</summary>
		/// <param name="result">
		///		The byte array read from current stream to be stored when operation is succeeded.
		/// </param>
		/// <returns>
		///		<c>true</c> if expected value was read from stream; <c>false</c> if no more data on the stream.
		///		Note that this method throws exception for unexpected state. See exceptions section.
		/// </returns>
		/// <exception cref="InvalidMessagePackStreamException">
		///		Cannot read a value because the underlying stream unexpectedly ends.
		/// </exception>
		/// <exception cref="MessageTypeException">
		///		A value read from data source is not a raw.
		/// </exception>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "0#", Justification = "Using nullable return value is very slow" )]
		public virtual bool ReadBinary( out byte[] result )
		{
			if( !this.Read() )
			{
				result = null;
				return false;
			}

			result = this.LastReadData.AsBinary();
			return true;
		}

		/// <summary>
		///		Reads next utf-8 encoded string value from current stream.
		///	</summary>
		/// <param name="result">
		///		The decoded utf-8 encoded string read from current stream to be stored when operation is succeeded.
		/// </param>
		/// <returns>
		///		<c>true</c> if expected value was read from stream; <c>false</c> if no more data on the stream.
		///		Note that this method throws exception for unexpected state. See exceptions section.
		/// </returns>
		/// <exception cref="InvalidMessagePackStreamException">
		///		Cannot read a value because the underlying stream unexpectedly ends.
		/// </exception>
		/// <exception cref="MessageTypeException">
		///		A value read from data source is not a raw.
		/// </exception>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "0#", Justification = "Using nullable return value is very slow" )]
		public virtual bool ReadString( out string result )
		{
			if( !this.Read() )
			{
				result = null;
				return false;
			}

			result = this.LastReadData.AsString();
			return true;
		}

		/// <summary>
		///		Reads next value from current stream.
		///	</summary>
		/// <param name="result">
		///		The <see cref="MessagePackObject"/> which represents a value read from current stream to be stored when operation is succeeded.
		/// </param>
		/// <returns>
		///		<c>true</c> if expected value was read from stream; <c>false</c> if no more data on the stream.
		///		Note that this method throws exception for unexpected state. See exceptions section.
		/// </returns>
		/// <exception cref="InvalidMessagePackStreamException">
		///		Cannot read a value because the underlying stream unexpectedly ends.
		/// </exception>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "0#", Justification = "Using nullable return value is very slow" )]
		public virtual bool ReadObject( out MessagePackObject result )
		{
			if( !this.Read() )
			{
				result = default( MessagePackObject );
				return false;
			}

			result = this.LastReadData;
			return true;
		}
	}
}
