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
using System.Runtime.InteropServices;

namespace MsgPack
{
	// This file generated from MessagePackObject.tt T4Template.
	// Do not modify this file. Edit MessagePackObject.tt instead.

	/// <summary>
	///		Represents deserialized object of MsgPack.
	/// </summary>
	[StructLayout( LayoutKind.Auto )]
	public partial struct MessagePackObject : IEquatable<MessagePackObject>, IPackable
	{
		#region -- Constructors --
		/// <summary>
		///		Initializes a new instance of the <see cref="MessagePackObject"/> type which wraps <see cref="Boolean"/> instance.
		/// </summary>
		/// <param name="value">A <see cref="MessagePackObject"/> value to be wrapped.</param>
		public MessagePackObject( Boolean value )
		{
			// trick: Avoid long boilerplate initialization.
			this = new MessagePackObject();
			this._value = value ? ( ulong )1 : 0;
			this._handleOrTypeCode = _booleanTypeCode;
		}
		/// <summary>
		///		Initializes a new instance of the <see cref="MessagePackObject"/> type which wraps <see cref="Byte"/> instance.
		/// </summary>
		/// <param name="value">A <see cref="MessagePackObject"/> value to be wrapped.</param>
		public MessagePackObject( Byte value )
		{
			// trick: Avoid long boilerplate initialization.
			this = new MessagePackObject();
			this._value = value;
			this._handleOrTypeCode = _byteTypeCode;
		}
		/// <summary>
		///		Initializes a new instance of the <see cref="MessagePackObject"/> type which wraps <see cref="SByte"/> instance.
		/// </summary>
		/// <param name="value">A <see cref="MessagePackObject"/> value to be wrapped.</param>
#if !UNITY
		[CLSCompliant( false )]
#endif // !UNITY
		public MessagePackObject( SByte value )
		{
			// trick: Avoid long boilerplate initialization.
			this = new MessagePackObject();
			this._value = unchecked( ( ulong )value );
			this._handleOrTypeCode = _sbyteTypeCode;
		}
		/// <summary>
		///		Initializes a new instance of the <see cref="MessagePackObject"/> type which wraps <see cref="Int16"/> instance.
		/// </summary>
		/// <param name="value">A <see cref="MessagePackObject"/> value to be wrapped.</param>
		public MessagePackObject( Int16 value )
		{
			// trick: Avoid long boilerplate initialization.
			this = new MessagePackObject();
			this._value = unchecked( ( ulong )value );
			this._handleOrTypeCode = _int16TypeCode;
		}
		/// <summary>
		///		Initializes a new instance of the <see cref="MessagePackObject"/> type which wraps <see cref="UInt16"/> instance.
		/// </summary>
		/// <param name="value">A <see cref="MessagePackObject"/> value to be wrapped.</param>
#if !UNITY
		[CLSCompliant( false )]
#endif // !UNITY
		public MessagePackObject( UInt16 value )
		{
			// trick: Avoid long boilerplate initialization.
			this = new MessagePackObject();
			this._value = value;
			this._handleOrTypeCode = _uint16TypeCode;
		}
		/// <summary>
		///		Initializes a new instance of the <see cref="MessagePackObject"/> type which wraps <see cref="Int32"/> instance.
		/// </summary>
		/// <param name="value">A <see cref="MessagePackObject"/> value to be wrapped.</param>
		public MessagePackObject( Int32 value )
		{
			// trick: Avoid long boilerplate initialization.
			this = new MessagePackObject();
			this._value = unchecked( ( ulong )value );
			this._handleOrTypeCode = _int32TypeCode;
		}
		/// <summary>
		///		Initializes a new instance of the <see cref="MessagePackObject"/> type which wraps <see cref="UInt32"/> instance.
		/// </summary>
		/// <param name="value">A <see cref="MessagePackObject"/> value to be wrapped.</param>
#if !UNITY
		[CLSCompliant( false )]
#endif // !UNITY
		public MessagePackObject( UInt32 value )
		{
			// trick: Avoid long boilerplate initialization.
			this = new MessagePackObject();
			this._value = value;
			this._handleOrTypeCode = _uint32TypeCode;
		}
		/// <summary>
		///		Initializes a new instance of the <see cref="MessagePackObject"/> type which wraps <see cref="Int64"/> instance.
		/// </summary>
		/// <param name="value">A <see cref="MessagePackObject"/> value to be wrapped.</param>
		public MessagePackObject( Int64 value )
		{
			// trick: Avoid long boilerplate initialization.
			this = new MessagePackObject();
			this._value = unchecked( ( ulong )value );
			this._handleOrTypeCode = _int64TypeCode;
		}
		/// <summary>
		///		Initializes a new instance of the <see cref="MessagePackObject"/> type which wraps <see cref="UInt64"/> instance.
		/// </summary>
		/// <param name="value">A <see cref="MessagePackObject"/> value to be wrapped.</param>
#if !UNITY
		[CLSCompliant( false )]
#endif // !UNITY
		public MessagePackObject( UInt64 value )
		{
			// trick: Avoid long boilerplate initialization.
			this = new MessagePackObject();
			this._value = value;
			this._handleOrTypeCode = _uint64TypeCode;
		}
		/// <summary>
		///		Initializes a new instance of the <see cref="MessagePackObject"/> type which wraps <see cref="Single"/> instance.
		/// </summary>
		/// <param name="value">A <see cref="MessagePackObject"/> value to be wrapped.</param>
		public MessagePackObject( Single value )
		{
			// trick: Avoid long boilerplate initialization.
			this = new MessagePackObject();
			var bytes = BitConverter.GetBytes( value );
			unchecked
			{
#pragma warning disable 0675
				if( BitConverter.IsLittleEndian )
				{
					this._value |=  ( ulong )( bytes[ 3 ] << 24 );
					this._value |=  ( ulong )( bytes[ 2 ] << 16 );
					this._value |=  ( ulong )( bytes[ 1 ] << 8 );
					this._value |=  bytes[ 0 ];
				}
				else
				{
					this._value |=  ( ulong )( bytes[ 0 ] << 24 );
					this._value |=  ( ulong )( bytes[ 1 ] << 16 );
					this._value |=  ( ulong )( bytes[ 2 ] << 8 );
					this._value |=  bytes[ 3 ];
				}
#pragma warning restore 0675
			}
			this._handleOrTypeCode = _singleTypeCode;
		}
		/// <summary>
		///		Initializes a new instance of the <see cref="MessagePackObject"/> type which wraps <see cref="Double"/> instance.
		/// </summary>
		/// <param name="value">A <see cref="MessagePackObject"/> value to be wrapped.</param>
		public MessagePackObject( Double value )
		{
			// trick: Avoid long boilerplate initialization.
			this = new MessagePackObject();
			this._value = unchecked( ( ulong )BitConverter.DoubleToInt64Bits( value ) );
			this._handleOrTypeCode = _doubleTypeCode;
		}
		/// <summary>
		///		Initializes a new instance of the <see cref="MessagePackObject"/> type which wraps <see cref="String"/> instance.
		/// </summary>
		/// <param name="value">A <see cref="MessagePackObject"/> value to be wrapped.</param>
		public MessagePackObject( String value )
		{
			// trick: Avoid long boilerplate initialization.
			this = new MessagePackObject();
			if ( value == null )
			{
				this._handleOrTypeCode = null;
			}
			else
			{
				this._handleOrTypeCode = new MessagePackString( value );
			}
		}
		/// <summary>
		///		Initializes a new instance of the <see cref="Byte"/>[] type which wraps <see cref="Byte" />[] instance with specified manner.
		/// </summary>
		/// <param name="value">A bytes array to be wrapped.</param>
		/// <remarks>
		///		This constructor invokes <see cref="MessagePackObject(Byte[],Boolean)" /> with <c>false</c>, that means if you pass tha bytes array which is valid utf-8, resulting object can be <see cref="String" />,
		///		and its <see cref="UnderlyingType" /> should be <see cref="String" />.
		/// </remarks>
		public MessagePackObject( Byte[] value )
			: this( value, false ) { }

		/// <summary>
		///		Initializes a new instance of the <see cref="Byte"/>[] type which wraps <see cref="Byte" />[] instance with specified manner.
		/// </summary>
		/// <param name="value">A bytes array to be wrapped.</param>
		/// <param name="isBinary"><c>true</c> if <paramref name="value"/> always should be binary; <c>false</c>, otherwise.</param>
		/// <remarks>
		///		When the <paramref name="isBinary" /> is <c>true</c>, then resulting object represents binary even if the <paramref name="value"/> is valid utf-8 sequence,
		///		that is, its <see cref="UnderlyingType" /> should be <see cref="Byte" />[].
		///		On the other hand, when  contrast, the <paramref name="isBinary" /> is <c>false</c>, and if the <paramref name="value"/> is valid utf-8, 
		///		then the resulting object can be <see cref="String" />,
		///		and its <see cref="UnderlyingType" /> should be <see cref="String" />.
		/// </remarks>
		public MessagePackObject( Byte[] value, bool isBinary )
		{
			// trick: Avoid long boilerplate initialization.
			this = new MessagePackObject();
			if ( value == null )
			{
				this._handleOrTypeCode = null;
			}
			else
			{
				this._handleOrTypeCode = new MessagePackString( value, isBinary );
			}
		}
		/// <summary>
		///		Initializes a new instance of the <see cref="MessagePackObject"/> type which wraps <see cref="MessagePackExtendedTypeObject"/> instance.
		/// </summary>
		/// <param name="value">A <see cref="MessagePackObject"/> value to be wrapped.</param>
		public MessagePackObject( MessagePackExtendedTypeObject value )
		{
			// trick: Avoid long boilerplate initialization.
			this = new MessagePackObject();
			this._value = value.TypeCode;
			this._handleOrTypeCode = value.Body;
		}

		#endregion -- Constructors --

		#region -- Primitive Type Conversion Methods --

		/// <summary>
		///		Convert this instance to <see cref="Boolean" /> instance.
		/// </summary>
		/// <returns><see cref="Boolean" /> instance corresponds to this instance.</returns>
		public Boolean AsBoolean()
		{
			if( this.IsNil )
			{
				ThrowCannotBeNilAs<Boolean>();
			}
			var typeCode = this._handleOrTypeCode as ValueTypeCode;
			if( typeCode == null || typeCode.TypeCode != MessagePackValueTypeCode.Boolean )
			{
				ThrowInvalidTypeAs<bool>( this );
			}
			
			return this._value != 0;
		}

		/// <summary>
		///		Convert this instance to <see cref="Byte" /> instance.
		/// </summary>
		/// <returns><see cref="Byte" /> instance corresponds to this instance.</returns>
		public Byte AsByte()
		{
			if( this.IsNil )
			{
				ThrowCannotBeNilAs<Byte>();
			}
			var typeCode = this._handleOrTypeCode as ValueTypeCode;
			if( typeCode == null )
			{
				ThrowInvalidTypeAs<Byte>( this );
			}
			
			if( typeCode.IsInteger )
			{
				if( typeCode.IsSigned )
				{
					const long minValue = ( long )Byte.MinValue;
					const long maxValue = ( long )Byte.MaxValue;
					
					long asInt64 = unchecked( ( long )this._value );
					if( asInt64 < minValue || maxValue < asInt64 )
					{
						ThrowInvalidTypeAs<Byte>( this );
					}

					return unchecked( ( Byte )asInt64 );
				}
				else
				{
					const ulong maxValue = unchecked( ( ulong )Byte.MaxValue );
					if( maxValue < this._value )
					{
						ThrowInvalidTypeAs<Byte>( this );
					}
					
					return ( Byte )this._value;
				}
			}
			else if( typeCode.TypeCode == MessagePackValueTypeCode.Double )
			{
				return ( Byte )BitConverter.Int64BitsToDouble( unchecked( ( long )this._value ) );
			}
			else if( typeCode.TypeCode == MessagePackValueTypeCode.Single )
			{
				return ( Byte )BitConverter.ToSingle( BitConverter.GetBytes( this._value ), 0 );
			}
			else
			{
				ThrowInvalidTypeAs<Byte>( this );
				return default( Byte ); // never reached
			}
		}

		/// <summary>
		///		Convert this instance to <see cref="SByte" /> instance.
		/// </summary>
		/// <returns><see cref="SByte" /> instance corresponds to this instance.</returns>
#if !UNITY
		[CLSCompliant( false )]
#endif // !UNITY
		public SByte AsSByte()
		{
			if( this.IsNil )
			{
				ThrowCannotBeNilAs<SByte>();
			}
			var typeCode = this._handleOrTypeCode as ValueTypeCode;
			if( typeCode == null )
			{
				ThrowInvalidTypeAs<SByte>( this );
			}
			
			if( typeCode.IsInteger )
			{
				if( typeCode.IsSigned )
				{
					const long minValue = ( long )SByte.MinValue;
					const long maxValue = ( long )SByte.MaxValue;
					
					long asInt64 = unchecked( ( long )this._value );
					if( asInt64 < minValue || maxValue < asInt64 )
					{
						ThrowInvalidTypeAs<SByte>( this );
					}

					return unchecked( ( SByte )asInt64 );
				}
				else
				{
					const ulong maxValue = unchecked( ( ulong )SByte.MaxValue );
					if( maxValue < this._value )
					{
						ThrowInvalidTypeAs<SByte>( this );
					}
					
					return ( SByte )this._value;
				}
			}
			else if( typeCode.TypeCode == MessagePackValueTypeCode.Double )
			{
				return ( SByte )BitConverter.Int64BitsToDouble( unchecked( ( long )this._value ) );
			}
			else if( typeCode.TypeCode == MessagePackValueTypeCode.Single )
			{
				return ( SByte )BitConverter.ToSingle( BitConverter.GetBytes( this._value ), 0 );
			}
			else
			{
				ThrowInvalidTypeAs<SByte>( this );
				return default( SByte ); // never reached
			}
		}

		/// <summary>
		///		Convert this instance to <see cref="Int16" /> instance.
		/// </summary>
		/// <returns><see cref="Int16" /> instance corresponds to this instance.</returns>
		public Int16 AsInt16()
		{
			if( this.IsNil )
			{
				ThrowCannotBeNilAs<Int16>();
			}
			var typeCode = this._handleOrTypeCode as ValueTypeCode;
			if( typeCode == null )
			{
				ThrowInvalidTypeAs<Int16>( this );
			}
			
			if( typeCode.IsInteger )
			{
				if( typeCode.IsSigned )
				{
					const long minValue = ( long )Int16.MinValue;
					const long maxValue = ( long )Int16.MaxValue;
					
					long asInt64 = unchecked( ( long )this._value );
					if( asInt64 < minValue || maxValue < asInt64 )
					{
						ThrowInvalidTypeAs<Int16>( this );
					}

					return unchecked( ( Int16 )asInt64 );
				}
				else
				{
					const ulong maxValue = unchecked( ( ulong )Int16.MaxValue );
					if( maxValue < this._value )
					{
						ThrowInvalidTypeAs<Int16>( this );
					}
					
					return ( Int16 )this._value;
				}
			}
			else if( typeCode.TypeCode == MessagePackValueTypeCode.Double )
			{
				return ( Int16 )BitConverter.Int64BitsToDouble( unchecked( ( long )this._value ) );
			}
			else if( typeCode.TypeCode == MessagePackValueTypeCode.Single )
			{
				return ( Int16 )BitConverter.ToSingle( BitConverter.GetBytes( this._value ), 0 );
			}
			else
			{
				ThrowInvalidTypeAs<Int16>( this );
				return default( Int16 ); // never reached
			}
		}

		/// <summary>
		///		Convert this instance to <see cref="UInt16" /> instance.
		/// </summary>
		/// <returns><see cref="UInt16" /> instance corresponds to this instance.</returns>
#if !UNITY
		[CLSCompliant( false )]
#endif // !UNITY
		public UInt16 AsUInt16()
		{
			if( this.IsNil )
			{
				ThrowCannotBeNilAs<UInt16>();
			}
			var typeCode = this._handleOrTypeCode as ValueTypeCode;
			if( typeCode == null )
			{
				ThrowInvalidTypeAs<UInt16>( this );
			}
			
			if( typeCode.IsInteger )
			{
				const ulong maxValue = unchecked( ( ulong )UInt16.MaxValue );
				
				if( maxValue < this._value )
				{
					// Overflow or negative.
					ThrowInvalidTypeAs<UInt16>( this );
				}

				return unchecked( ( UInt16 )this._value );
			}
			else if( typeCode.TypeCode == MessagePackValueTypeCode.Double )
			{
				return ( UInt16 )BitConverter.Int64BitsToDouble( unchecked( ( long )this._value ) );
			}
			else if( typeCode.TypeCode == MessagePackValueTypeCode.Single )
			{
				return ( UInt16 )BitConverter.ToSingle( BitConverter.GetBytes( this._value ), 0 );
			}
			else
			{
				ThrowInvalidTypeAs<UInt16>( this );
				return default( UInt16 ); // never reached
			}
		}

		/// <summary>
		///		Convert this instance to <see cref="Int32" /> instance.
		/// </summary>
		/// <returns><see cref="Int32" /> instance corresponds to this instance.</returns>
		public Int32 AsInt32()
		{
			if( this.IsNil )
			{
				ThrowCannotBeNilAs<Int32>();
			}
			var typeCode = this._handleOrTypeCode as ValueTypeCode;
			if( typeCode == null )
			{
				ThrowInvalidTypeAs<Int32>( this );
			}
			
			if( typeCode.IsInteger )
			{
				if( typeCode.IsSigned )
				{
					const long minValue = ( long )Int32.MinValue;
					const long maxValue = ( long )Int32.MaxValue;
					
					long asInt64 = unchecked( ( long )this._value );
					if( asInt64 < minValue || maxValue < asInt64 )
					{
						ThrowInvalidTypeAs<Int32>( this );
					}

					return unchecked( ( Int32 )asInt64 );
				}
				else
				{
					const ulong maxValue = unchecked( ( ulong )Int32.MaxValue );
					if( maxValue < this._value )
					{
						ThrowInvalidTypeAs<Int32>( this );
					}
					
					return ( Int32 )this._value;
				}
			}
			else if( typeCode.TypeCode == MessagePackValueTypeCode.Double )
			{
				return ( Int32 )BitConverter.Int64BitsToDouble( unchecked( ( long )this._value ) );
			}
			else if( typeCode.TypeCode == MessagePackValueTypeCode.Single )
			{
				return ( Int32 )BitConverter.ToSingle( BitConverter.GetBytes( this._value ), 0 );
			}
			else
			{
				ThrowInvalidTypeAs<Int32>( this );
				return default( Int32 ); // never reached
			}
		}

		/// <summary>
		///		Convert this instance to <see cref="UInt32" /> instance.
		/// </summary>
		/// <returns><see cref="UInt32" /> instance corresponds to this instance.</returns>
#if !UNITY
		[CLSCompliant( false )]
#endif // !UNITY
		public UInt32 AsUInt32()
		{
			if( this.IsNil )
			{
				ThrowCannotBeNilAs<UInt32>();
			}
			var typeCode = this._handleOrTypeCode as ValueTypeCode;
			if( typeCode == null )
			{
				ThrowInvalidTypeAs<UInt32>( this );
			}
			
			if( typeCode.IsInteger )
			{
				const ulong maxValue = unchecked( ( ulong )UInt32.MaxValue );
				
				if( maxValue < this._value )
				{
					// Overflow or negative.
					ThrowInvalidTypeAs<UInt32>( this );
				}

				return unchecked( ( UInt32 )this._value );
			}
			else if( typeCode.TypeCode == MessagePackValueTypeCode.Double )
			{
				return ( UInt32 )BitConverter.Int64BitsToDouble( unchecked( ( long )this._value ) );
			}
			else if( typeCode.TypeCode == MessagePackValueTypeCode.Single )
			{
				return ( UInt32 )BitConverter.ToSingle( BitConverter.GetBytes( this._value ), 0 );
			}
			else
			{
				ThrowInvalidTypeAs<UInt32>( this );
				return default( UInt32 ); // never reached
			}
		}

		/// <summary>
		///		Convert this instance to <see cref="Int64" /> instance.
		/// </summary>
		/// <returns><see cref="Int64" /> instance corresponds to this instance.</returns>
		public Int64 AsInt64()
		{
			if( this.IsNil )
			{
				ThrowCannotBeNilAs<Int64>();
			}
			var typeCode = this._handleOrTypeCode as ValueTypeCode;
			if( typeCode == null )
			{
				ThrowInvalidTypeAs<Int64>( this );
			}
			
			if( typeCode.IsInteger )
			{
				if( typeCode.IsSigned )
				{
					return unchecked( ( long )this._value );
				}
				else
				{
					const ulong maxValue = unchecked( ( ulong )Int64.MaxValue );
					if( maxValue < this._value )
					{
						ThrowInvalidTypeAs<Int64>( this );
					}
					
					return ( Int64 )this._value;
				}
			}
			else if( typeCode.TypeCode == MessagePackValueTypeCode.Double )
			{
				return ( Int64 )BitConverter.Int64BitsToDouble( unchecked( ( long )this._value ) );
			}
			else if( typeCode.TypeCode == MessagePackValueTypeCode.Single )
			{
				return ( Int64 )BitConverter.ToSingle( BitConverter.GetBytes( this._value ), 0 );
			}
			else
			{
				ThrowInvalidTypeAs<Int64>( this );
				return default( Int64 ); // never reached
			}
		}

		/// <summary>
		///		Convert this instance to <see cref="UInt64" /> instance.
		/// </summary>
		/// <returns><see cref="UInt64" /> instance corresponds to this instance.</returns>
#if !UNITY
		[CLSCompliant( false )]
#endif // !UNITY
		public UInt64 AsUInt64()
		{
			if( this.IsNil )
			{
				ThrowCannotBeNilAs<UInt64>();
			}
			var typeCode = this._handleOrTypeCode as ValueTypeCode;
			if( typeCode == null )
			{
				ThrowInvalidTypeAs<UInt64>( this );
			}
			
			if( typeCode.IsInteger )
			{
				if( typeCode.IsSigned )
				{
					if( Int64.MaxValue < this._value )
					{
						// Negative.
						ThrowInvalidTypeAs<UInt64>( this );
					}
				}

				return this._value;
			}
			else if( typeCode.TypeCode == MessagePackValueTypeCode.Double )
			{
				return ( UInt64 )BitConverter.Int64BitsToDouble( unchecked( ( long )this._value ) );
			}
			else if( typeCode.TypeCode == MessagePackValueTypeCode.Single )
			{
				return ( UInt64 )BitConverter.ToSingle( BitConverter.GetBytes( this._value ), 0 );
			}
			else
			{
				ThrowInvalidTypeAs<UInt64>( this );
				return default( UInt64 ); // never reached
			}
		}

		/// <summary>
		///		Convert this instance to <see cref="Single" /> instance.
		/// </summary>
		/// <returns><see cref="Single" /> instance corresponds to this instance.</returns>
		public Single AsSingle()
		{
			if( this.IsNil )
			{
				ThrowCannotBeNilAs<Single>();
			}
			var typeCode = this._handleOrTypeCode as ValueTypeCode;
			if( typeCode == null )
			{
				ThrowInvalidTypeAs<float>( this );
			}
			
			if( typeCode.IsInteger )
			{
				if( typeCode.IsSigned )
				{
					return ( float )( unchecked( ( long )this._value ) );
				}
				else
				{
					return ( float )this._value;
				}
			}
			else if( typeCode.TypeCode == MessagePackValueTypeCode.Double )
			{
				return ( Single )BitConverter.Int64BitsToDouble( unchecked( ( long )this._value ) );
			}
			else if( typeCode.TypeCode == MessagePackValueTypeCode.Single )
			{
				return ( Single )BitConverter.ToSingle( BitConverter.GetBytes( this._value ), 0 );
			}
			else
			{
				ThrowInvalidTypeAs<float>( this );
				return default( float ); // never reached
			}
		}

		/// <summary>
		///		Convert this instance to <see cref="Double" /> instance.
		/// </summary>
		/// <returns><see cref="Double" /> instance corresponds to this instance.</returns>
		public Double AsDouble()
		{
			if( this.IsNil )
			{
				ThrowCannotBeNilAs<Double>();
			}
			var typeCode = this._handleOrTypeCode as ValueTypeCode;
			if( typeCode == null )
			{
				ThrowInvalidTypeAs<double>( this );
			}
			
			if( typeCode.IsInteger )
			{
				if( typeCode.IsSigned )
				{
					return ( double )( unchecked( ( long )this._value ) );
				}
				else
				{
					return ( double )this._value;
				}
			}
			else if( typeCode.TypeCode == MessagePackValueTypeCode.Double )
			{
				return ( Double )BitConverter.Int64BitsToDouble( unchecked( ( long )this._value ) );
			}
			else if( typeCode.TypeCode == MessagePackValueTypeCode.Single )
			{
				return ( Double )BitConverter.ToSingle( BitConverter.GetBytes( this._value ), 0 );
			}
			else
			{
				ThrowInvalidTypeAs<double>( this );
				return default( double ); // never reached
			}
		}

		/// <summary>
		///		Convert this instance to <see cref="String" /> instance.
		/// </summary>
		/// <returns><see cref="String" /> instance corresponds to this instance.</returns>
		public String AsString()
		{
			VerifyUnderlyingType<MessagePackString>( this, null );

			if( this._handleOrTypeCode == null )
			{
				// nil
				return null;
			}

			var asString = this._handleOrTypeCode as MessagePackString;
#if !UNITY && DEBUG
			Contract.Assert( asString != null, "asString != null" );
#endif // !UNITY && DEBUG
			return asString.GetString();
		}

		/// <summary>
		///		Convert this instance to <see cref="Byte" />[] instance.
		/// </summary>
		/// <returns><see cref="Byte" />[] instance corresponds to this instance.</returns>
		public Byte[] AsBinary()
		{
			VerifyUnderlyingType<MessagePackString>( this, null  );

			if( this._handleOrTypeCode == null )
			{
				// nil
				return null;
			}

			var asString = this._handleOrTypeCode as MessagePackString;
#if !UNITY && DEBUG
			Contract.Assert( asString != null, "asString != null" );
#endif // !UNITY && DEBUG
			return asString.GetBytes();
		}

		/// <summary>
		///		Convert this instance to <see cref="MessagePackExtendedTypeObject" /> instance.
		/// </summary>
		/// <returns><see cref="MessagePackExtendedTypeObject" /> instance corresponds to this instance.</returns>
		public MessagePackExtendedTypeObject AsMessagePackExtendedTypeObject()
		{
			if( this.IsNil )
			{
				ThrowCannotBeNilAs<MessagePackExtendedTypeObject>();
			}
			VerifyUnderlyingType<MessagePackExtendedTypeObject>( this, null  );

			return MessagePackExtendedTypeObject.Unpack( unchecked( ( byte )this._value ), this._handleOrTypeCode as byte[] );
		}

		#endregion -- Primitive Type Conversion Methods --

		#region -- Conversion Operator Overloads --


		/// <summary>
		///		Convert <see cref="Boolean" />instance to <see cref="MessagePackObject"/> instance.
		/// </summary>
		/// <param name="value"><see cref="Boolean" /> instance.</param>
		/// <returns><see cref="MessagePackObject"/> instance corresponds to <paramref name="value"/>.</returns>
		public static implicit operator MessagePackObject( Boolean value )
		{
			MessagePackObject result;
			// trick: Avoid long boilerplate initialization.
			result = new MessagePackObject();
			result._value = value ? ( ulong )1 : 0;
			result._handleOrTypeCode = _booleanTypeCode;
			return result;
		}

		/// <summary>
		///		Convert <see cref="Byte" />instance to <see cref="MessagePackObject"/> instance.
		/// </summary>
		/// <param name="value"><see cref="Byte" /> instance.</param>
		/// <returns><see cref="MessagePackObject"/> instance corresponds to <paramref name="value"/>.</returns>
		public static implicit operator MessagePackObject( Byte value )
		{
			MessagePackObject result;
			// trick: Avoid long boilerplate initialization.
			result = new MessagePackObject();
			result._value = value;
			result._handleOrTypeCode = _byteTypeCode;
			return result;
		}

		/// <summary>
		///		Convert <see cref="SByte" />instance to <see cref="MessagePackObject"/> instance.
		/// </summary>
		/// <param name="value"><see cref="SByte" /> instance.</param>
		/// <returns><see cref="MessagePackObject"/> instance corresponds to <paramref name="value"/>.</returns>
#if !UNITY
		[CLSCompliant( false )]
#endif // !UNITY
		public static implicit operator MessagePackObject( SByte value )
		{
			MessagePackObject result;
			// trick: Avoid long boilerplate initialization.
			result = new MessagePackObject();
			result._value = unchecked( ( ulong )value );
			result._handleOrTypeCode = _sbyteTypeCode;
			return result;
		}

		/// <summary>
		///		Convert <see cref="Int16" />instance to <see cref="MessagePackObject"/> instance.
		/// </summary>
		/// <param name="value"><see cref="Int16" /> instance.</param>
		/// <returns><see cref="MessagePackObject"/> instance corresponds to <paramref name="value"/>.</returns>
		public static implicit operator MessagePackObject( Int16 value )
		{
			MessagePackObject result;
			// trick: Avoid long boilerplate initialization.
			result = new MessagePackObject();
			result._value = unchecked( ( ulong )value );
			result._handleOrTypeCode = _int16TypeCode;
			return result;
		}

		/// <summary>
		///		Convert <see cref="UInt16" />instance to <see cref="MessagePackObject"/> instance.
		/// </summary>
		/// <param name="value"><see cref="UInt16" /> instance.</param>
		/// <returns><see cref="MessagePackObject"/> instance corresponds to <paramref name="value"/>.</returns>
#if !UNITY
		[CLSCompliant( false )]
#endif // !UNITY
		public static implicit operator MessagePackObject( UInt16 value )
		{
			MessagePackObject result;
			// trick: Avoid long boilerplate initialization.
			result = new MessagePackObject();
			result._value = value;
			result._handleOrTypeCode = _uint16TypeCode;
			return result;
		}

		/// <summary>
		///		Convert <see cref="Int32" />instance to <see cref="MessagePackObject"/> instance.
		/// </summary>
		/// <param name="value"><see cref="Int32" /> instance.</param>
		/// <returns><see cref="MessagePackObject"/> instance corresponds to <paramref name="value"/>.</returns>
		public static implicit operator MessagePackObject( Int32 value )
		{
			MessagePackObject result;
			// trick: Avoid long boilerplate initialization.
			result = new MessagePackObject();
			result._value = unchecked( ( ulong )value );
			result._handleOrTypeCode = _int32TypeCode;
			return result;
		}

		/// <summary>
		///		Convert <see cref="UInt32" />instance to <see cref="MessagePackObject"/> instance.
		/// </summary>
		/// <param name="value"><see cref="UInt32" /> instance.</param>
		/// <returns><see cref="MessagePackObject"/> instance corresponds to <paramref name="value"/>.</returns>
#if !UNITY
		[CLSCompliant( false )]
#endif // !UNITY
		public static implicit operator MessagePackObject( UInt32 value )
		{
			MessagePackObject result;
			// trick: Avoid long boilerplate initialization.
			result = new MessagePackObject();
			result._value = value;
			result._handleOrTypeCode = _uint32TypeCode;
			return result;
		}

		/// <summary>
		///		Convert <see cref="Int64" />instance to <see cref="MessagePackObject"/> instance.
		/// </summary>
		/// <param name="value"><see cref="Int64" /> instance.</param>
		/// <returns><see cref="MessagePackObject"/> instance corresponds to <paramref name="value"/>.</returns>
		public static implicit operator MessagePackObject( Int64 value )
		{
			MessagePackObject result;
			// trick: Avoid long boilerplate initialization.
			result = new MessagePackObject();
			result._value = unchecked( ( ulong )value );
			result._handleOrTypeCode = _int64TypeCode;
			return result;
		}

		/// <summary>
		///		Convert <see cref="UInt64" />instance to <see cref="MessagePackObject"/> instance.
		/// </summary>
		/// <param name="value"><see cref="UInt64" /> instance.</param>
		/// <returns><see cref="MessagePackObject"/> instance corresponds to <paramref name="value"/>.</returns>
#if !UNITY
		[CLSCompliant( false )]
#endif // !UNITY
		public static implicit operator MessagePackObject( UInt64 value )
		{
			MessagePackObject result;
			// trick: Avoid long boilerplate initialization.
			result = new MessagePackObject();
			result._value = value;
			result._handleOrTypeCode = _uint64TypeCode;
			return result;
		}

		/// <summary>
		///		Convert <see cref="Single" />instance to <see cref="MessagePackObject"/> instance.
		/// </summary>
		/// <param name="value"><see cref="Single" /> instance.</param>
		/// <returns><see cref="MessagePackObject"/> instance corresponds to <paramref name="value"/>.</returns>
		public static implicit operator MessagePackObject( Single value )
		{
			MessagePackObject result;
			// trick: Avoid long boilerplate initialization.
			result = new MessagePackObject();
			var bytes = BitConverter.GetBytes( value );
			unchecked
			{
#pragma warning disable 0675
				if( BitConverter.IsLittleEndian )
				{
					result._value |=  ( ulong )( bytes[ 3 ] << 24 );
					result._value |=  ( ulong )( bytes[ 2 ] << 16 );
					result._value |=  ( ulong )( bytes[ 1 ] << 8 );
					result._value |=  bytes[ 0 ];
				}
				else
				{
					result._value |=  ( ulong )( bytes[ 0 ] << 24 );
					result._value |=  ( ulong )( bytes[ 1 ] << 16 );
					result._value |=  ( ulong )( bytes[ 2 ] << 8 );
					result._value |=  bytes[ 3 ];
				}
#pragma warning restore 0675
			}
			result._handleOrTypeCode = _singleTypeCode;
			return result;
		}

		/// <summary>
		///		Convert <see cref="Double" />instance to <see cref="MessagePackObject"/> instance.
		/// </summary>
		/// <param name="value"><see cref="Double" /> instance.</param>
		/// <returns><see cref="MessagePackObject"/> instance corresponds to <paramref name="value"/>.</returns>
		public static implicit operator MessagePackObject( Double value )
		{
			MessagePackObject result;
			// trick: Avoid long boilerplate initialization.
			result = new MessagePackObject();
			result._value = unchecked( ( ulong )BitConverter.DoubleToInt64Bits( value ) );
			result._handleOrTypeCode = _doubleTypeCode;
			return result;
		}

		/// <summary>
		///		Convert <see cref="String" />instance to <see cref="MessagePackObject"/> instance.
		/// </summary>
		/// <param name="value"><see cref="String" /> instance.</param>
		/// <returns><see cref="MessagePackObject"/> instance corresponds to <paramref name="value"/>.</returns>
		public static implicit operator MessagePackObject( String value )
		{
			MessagePackObject result;
			// trick: Avoid long boilerplate initialization.
			result = new MessagePackObject();
			if ( value == null )
			{
				result._handleOrTypeCode = null;
			}
			else
			{
				result._handleOrTypeCode = new MessagePackString( value );
			}
			return result;
		}

		/// <summary>
		///		Convert <see cref="Byte" />[]instance to <see cref="MessagePackObject"/> instance.
		/// </summary>
		/// <param name="value"><see cref="Byte" />[] instance.</param>
		/// <returns><see cref="MessagePackObject"/> instance corresponds to <paramref name="value"/>.</returns>
		public static implicit operator MessagePackObject( Byte[] value )
		{
			MessagePackObject result;
			// trick: Avoid long boilerplate initialization.
			result = new MessagePackObject();
			if ( value == null )
			{
				result._handleOrTypeCode = null;
			}
			else
			{
				result._handleOrTypeCode = new MessagePackString( value, false );
			}
			return result;
		}

		/// <summary>
		///		Convert <see cref="MessagePackExtendedTypeObject" />instance to <see cref="MessagePackObject"/> instance.
		/// </summary>
		/// <param name="value"><see cref="MessagePackExtendedTypeObject" /> instance.</param>
		/// <returns><see cref="MessagePackObject"/> instance corresponds to <paramref name="value"/>.</returns>
		public static implicit operator MessagePackObject( MessagePackExtendedTypeObject value )
		{
			MessagePackObject result;
			// trick: Avoid long boilerplate initialization.
			result = new MessagePackObject();
			result._value = value.TypeCode;
			result._handleOrTypeCode = value.Body;
			return result;
		}


		/// <summary>
		///		Convert this instance to <see cref="Boolean" /> instance.
		/// </summary>
		/// <param name="value"><see cref="MessagePackObject"/> instance.</param>
		/// <returns><see cref="Boolean" /> instance corresponds to <paramref name="value"/>.</returns>
		public static explicit operator Boolean( MessagePackObject value )
		{
			if( value.IsNil )
			{
				ThrowCannotBeNilAs<Boolean>();
			}
			var typeCode = value._handleOrTypeCode as ValueTypeCode;
			if( typeCode == null || typeCode.TypeCode != MessagePackValueTypeCode.Boolean )
			{
				ThrowInvalidTypeAs<bool>( value );
			}
			
			return value._value != 0;
		}

		/// <summary>
		///		Convert this instance to <see cref="Byte" /> instance.
		/// </summary>
		/// <param name="value"><see cref="MessagePackObject"/> instance.</param>
		/// <returns><see cref="Byte" /> instance corresponds to <paramref name="value"/>.</returns>
		public static explicit operator Byte( MessagePackObject value )
		{
			if( value.IsNil )
			{
				ThrowCannotBeNilAs<Byte>();
			}
			var typeCode = value._handleOrTypeCode as ValueTypeCode;
			if( typeCode == null )
			{
				ThrowInvalidTypeAs<Byte>( value );
			}
			
			if( typeCode.IsInteger )
			{
				if( typeCode.IsSigned )
				{
					const long minValue = ( long )Byte.MinValue;
					const long maxValue = ( long )Byte.MaxValue;
					
					long asInt64 = unchecked( ( long )value._value );
					if( asInt64 < minValue || maxValue < asInt64 )
					{
						ThrowInvalidTypeAs<Byte>( value );
					}

					return unchecked( ( Byte )asInt64 );
				}
				else
				{
					const ulong maxValue = unchecked( ( ulong )Byte.MaxValue );
					if( maxValue < value._value )
					{
						ThrowInvalidTypeAs<Byte>( value );
					}
					
					return ( Byte )value._value;
				}
			}
			else if( typeCode.TypeCode == MessagePackValueTypeCode.Double )
			{
				return ( Byte )BitConverter.Int64BitsToDouble( unchecked( ( long )value._value ) );
			}
			else if( typeCode.TypeCode == MessagePackValueTypeCode.Single )
			{
				return ( Byte )BitConverter.ToSingle( BitConverter.GetBytes( value._value ), 0 );
			}
			else
			{
				ThrowInvalidTypeAs<Byte>( value );
				return default( Byte ); // never reached
			}
		}

		/// <summary>
		///		Convert this instance to <see cref="SByte" /> instance.
		/// </summary>
		/// <param name="value"><see cref="MessagePackObject"/> instance.</param>
		/// <returns><see cref="SByte" /> instance corresponds to <paramref name="value"/>.</returns>
#if !UNITY
		[CLSCompliant( false )]
#endif // !UNITY
		public static explicit operator SByte( MessagePackObject value )
		{
			if( value.IsNil )
			{
				ThrowCannotBeNilAs<SByte>();
			}
			var typeCode = value._handleOrTypeCode as ValueTypeCode;
			if( typeCode == null )
			{
				ThrowInvalidTypeAs<SByte>( value );
			}
			
			if( typeCode.IsInteger )
			{
				if( typeCode.IsSigned )
				{
					const long minValue = ( long )SByte.MinValue;
					const long maxValue = ( long )SByte.MaxValue;
					
					long asInt64 = unchecked( ( long )value._value );
					if( asInt64 < minValue || maxValue < asInt64 )
					{
						ThrowInvalidTypeAs<SByte>( value );
					}

					return unchecked( ( SByte )asInt64 );
				}
				else
				{
					const ulong maxValue = unchecked( ( ulong )SByte.MaxValue );
					if( maxValue < value._value )
					{
						ThrowInvalidTypeAs<SByte>( value );
					}
					
					return ( SByte )value._value;
				}
			}
			else if( typeCode.TypeCode == MessagePackValueTypeCode.Double )
			{
				return ( SByte )BitConverter.Int64BitsToDouble( unchecked( ( long )value._value ) );
			}
			else if( typeCode.TypeCode == MessagePackValueTypeCode.Single )
			{
				return ( SByte )BitConverter.ToSingle( BitConverter.GetBytes( value._value ), 0 );
			}
			else
			{
				ThrowInvalidTypeAs<SByte>( value );
				return default( SByte ); // never reached
			}
		}

		/// <summary>
		///		Convert this instance to <see cref="Int16" /> instance.
		/// </summary>
		/// <param name="value"><see cref="MessagePackObject"/> instance.</param>
		/// <returns><see cref="Int16" /> instance corresponds to <paramref name="value"/>.</returns>
		public static explicit operator Int16( MessagePackObject value )
		{
			if( value.IsNil )
			{
				ThrowCannotBeNilAs<Int16>();
			}
			var typeCode = value._handleOrTypeCode as ValueTypeCode;
			if( typeCode == null )
			{
				ThrowInvalidTypeAs<Int16>( value );
			}
			
			if( typeCode.IsInteger )
			{
				if( typeCode.IsSigned )
				{
					const long minValue = ( long )Int16.MinValue;
					const long maxValue = ( long )Int16.MaxValue;
					
					long asInt64 = unchecked( ( long )value._value );
					if( asInt64 < minValue || maxValue < asInt64 )
					{
						ThrowInvalidTypeAs<Int16>( value );
					}

					return unchecked( ( Int16 )asInt64 );
				}
				else
				{
					const ulong maxValue = unchecked( ( ulong )Int16.MaxValue );
					if( maxValue < value._value )
					{
						ThrowInvalidTypeAs<Int16>( value );
					}
					
					return ( Int16 )value._value;
				}
			}
			else if( typeCode.TypeCode == MessagePackValueTypeCode.Double )
			{
				return ( Int16 )BitConverter.Int64BitsToDouble( unchecked( ( long )value._value ) );
			}
			else if( typeCode.TypeCode == MessagePackValueTypeCode.Single )
			{
				return ( Int16 )BitConverter.ToSingle( BitConverter.GetBytes( value._value ), 0 );
			}
			else
			{
				ThrowInvalidTypeAs<Int16>( value );
				return default( Int16 ); // never reached
			}
		}

		/// <summary>
		///		Convert this instance to <see cref="UInt16" /> instance.
		/// </summary>
		/// <param name="value"><see cref="MessagePackObject"/> instance.</param>
		/// <returns><see cref="UInt16" /> instance corresponds to <paramref name="value"/>.</returns>
#if !UNITY
		[CLSCompliant( false )]
#endif // !UNITY
		public static explicit operator UInt16( MessagePackObject value )
		{
			if( value.IsNil )
			{
				ThrowCannotBeNilAs<UInt16>();
			}
			var typeCode = value._handleOrTypeCode as ValueTypeCode;
			if( typeCode == null )
			{
				ThrowInvalidTypeAs<UInt16>( value );
			}
			
			if( typeCode.IsInteger )
			{
				const ulong maxValue = unchecked( ( ulong )UInt16.MaxValue );
				
				if( maxValue < value._value )
				{
					// Overflow or negative.
					ThrowInvalidTypeAs<UInt16>( value );
				}

				return unchecked( ( UInt16 )value._value );
			}
			else if( typeCode.TypeCode == MessagePackValueTypeCode.Double )
			{
				return ( UInt16 )BitConverter.Int64BitsToDouble( unchecked( ( long )value._value ) );
			}
			else if( typeCode.TypeCode == MessagePackValueTypeCode.Single )
			{
				return ( UInt16 )BitConverter.ToSingle( BitConverter.GetBytes( value._value ), 0 );
			}
			else
			{
				ThrowInvalidTypeAs<UInt16>( value );
				return default( UInt16 ); // never reached
			}
		}

		/// <summary>
		///		Convert this instance to <see cref="Int32" /> instance.
		/// </summary>
		/// <param name="value"><see cref="MessagePackObject"/> instance.</param>
		/// <returns><see cref="Int32" /> instance corresponds to <paramref name="value"/>.</returns>
		public static explicit operator Int32( MessagePackObject value )
		{
			if( value.IsNil )
			{
				ThrowCannotBeNilAs<Int32>();
			}
			var typeCode = value._handleOrTypeCode as ValueTypeCode;
			if( typeCode == null )
			{
				ThrowInvalidTypeAs<Int32>( value );
			}
			
			if( typeCode.IsInteger )
			{
				if( typeCode.IsSigned )
				{
					const long minValue = ( long )Int32.MinValue;
					const long maxValue = ( long )Int32.MaxValue;
					
					long asInt64 = unchecked( ( long )value._value );
					if( asInt64 < minValue || maxValue < asInt64 )
					{
						ThrowInvalidTypeAs<Int32>( value );
					}

					return unchecked( ( Int32 )asInt64 );
				}
				else
				{
					const ulong maxValue = unchecked( ( ulong )Int32.MaxValue );
					if( maxValue < value._value )
					{
						ThrowInvalidTypeAs<Int32>( value );
					}
					
					return ( Int32 )value._value;
				}
			}
			else if( typeCode.TypeCode == MessagePackValueTypeCode.Double )
			{
				return ( Int32 )BitConverter.Int64BitsToDouble( unchecked( ( long )value._value ) );
			}
			else if( typeCode.TypeCode == MessagePackValueTypeCode.Single )
			{
				return ( Int32 )BitConverter.ToSingle( BitConverter.GetBytes( value._value ), 0 );
			}
			else
			{
				ThrowInvalidTypeAs<Int32>( value );
				return default( Int32 ); // never reached
			}
		}

		/// <summary>
		///		Convert this instance to <see cref="UInt32" /> instance.
		/// </summary>
		/// <param name="value"><see cref="MessagePackObject"/> instance.</param>
		/// <returns><see cref="UInt32" /> instance corresponds to <paramref name="value"/>.</returns>
#if !UNITY
		[CLSCompliant( false )]
#endif // !UNITY
		public static explicit operator UInt32( MessagePackObject value )
		{
			if( value.IsNil )
			{
				ThrowCannotBeNilAs<UInt32>();
			}
			var typeCode = value._handleOrTypeCode as ValueTypeCode;
			if( typeCode == null )
			{
				ThrowInvalidTypeAs<UInt32>( value );
			}
			
			if( typeCode.IsInteger )
			{
				const ulong maxValue = unchecked( ( ulong )UInt32.MaxValue );
				
				if( maxValue < value._value )
				{
					// Overflow or negative.
					ThrowInvalidTypeAs<UInt32>( value );
				}

				return unchecked( ( UInt32 )value._value );
			}
			else if( typeCode.TypeCode == MessagePackValueTypeCode.Double )
			{
				return ( UInt32 )BitConverter.Int64BitsToDouble( unchecked( ( long )value._value ) );
			}
			else if( typeCode.TypeCode == MessagePackValueTypeCode.Single )
			{
				return ( UInt32 )BitConverter.ToSingle( BitConverter.GetBytes( value._value ), 0 );
			}
			else
			{
				ThrowInvalidTypeAs<UInt32>( value );
				return default( UInt32 ); // never reached
			}
		}

		/// <summary>
		///		Convert this instance to <see cref="Int64" /> instance.
		/// </summary>
		/// <param name="value"><see cref="MessagePackObject"/> instance.</param>
		/// <returns><see cref="Int64" /> instance corresponds to <paramref name="value"/>.</returns>
		public static explicit operator Int64( MessagePackObject value )
		{
			if( value.IsNil )
			{
				ThrowCannotBeNilAs<Int64>();
			}
			var typeCode = value._handleOrTypeCode as ValueTypeCode;
			if( typeCode == null )
			{
				ThrowInvalidTypeAs<Int64>( value );
			}
			
			if( typeCode.IsInteger )
			{
				if( typeCode.IsSigned )
				{
					return unchecked( ( long )value._value );
				}
				else
				{
					const ulong maxValue = unchecked( ( ulong )Int64.MaxValue );
					if( maxValue < value._value )
					{
						ThrowInvalidTypeAs<Int64>( value );
					}
					
					return ( Int64 )value._value;
				}
			}
			else if( typeCode.TypeCode == MessagePackValueTypeCode.Double )
			{
				return ( Int64 )BitConverter.Int64BitsToDouble( unchecked( ( long )value._value ) );
			}
			else if( typeCode.TypeCode == MessagePackValueTypeCode.Single )
			{
				return ( Int64 )BitConverter.ToSingle( BitConverter.GetBytes( value._value ), 0 );
			}
			else
			{
				ThrowInvalidTypeAs<Int64>( value );
				return default( Int64 ); // never reached
			}
		}

		/// <summary>
		///		Convert this instance to <see cref="UInt64" /> instance.
		/// </summary>
		/// <param name="value"><see cref="MessagePackObject"/> instance.</param>
		/// <returns><see cref="UInt64" /> instance corresponds to <paramref name="value"/>.</returns>
#if !UNITY
		[CLSCompliant( false )]
#endif // !UNITY
		public static explicit operator UInt64( MessagePackObject value )
		{
			if( value.IsNil )
			{
				ThrowCannotBeNilAs<UInt64>();
			}
			var typeCode = value._handleOrTypeCode as ValueTypeCode;
			if( typeCode == null )
			{
				ThrowInvalidTypeAs<UInt64>( value );
			}
			
			if( typeCode.IsInteger )
			{
				if( typeCode.IsSigned )
				{
					if( Int64.MaxValue < value._value )
					{
						// Negative.
						ThrowInvalidTypeAs<UInt64>( value );
					}
				}

				return value._value;
			}
			else if( typeCode.TypeCode == MessagePackValueTypeCode.Double )
			{
				return ( UInt64 )BitConverter.Int64BitsToDouble( unchecked( ( long )value._value ) );
			}
			else if( typeCode.TypeCode == MessagePackValueTypeCode.Single )
			{
				return ( UInt64 )BitConverter.ToSingle( BitConverter.GetBytes( value._value ), 0 );
			}
			else
			{
				ThrowInvalidTypeAs<UInt64>( value );
				return default( UInt64 ); // never reached
			}
		}

		/// <summary>
		///		Convert this instance to <see cref="Single" /> instance.
		/// </summary>
		/// <param name="value"><see cref="MessagePackObject"/> instance.</param>
		/// <returns><see cref="Single" /> instance corresponds to <paramref name="value"/>.</returns>
		public static explicit operator Single( MessagePackObject value )
		{
			if( value.IsNil )
			{
				ThrowCannotBeNilAs<Single>();
			}
			var typeCode = value._handleOrTypeCode as ValueTypeCode;
			if( typeCode == null )
			{
				ThrowInvalidTypeAs<float>( value );
			}
			
			if( typeCode.IsInteger )
			{
				if( typeCode.IsSigned )
				{
					return ( float )( unchecked( ( long )value._value ) );
				}
				else
				{
					return ( float )value._value;
				}
			}
			else if( typeCode.TypeCode == MessagePackValueTypeCode.Double )
			{
				return ( Single )BitConverter.Int64BitsToDouble( unchecked( ( long )value._value ) );
			}
			else if( typeCode.TypeCode == MessagePackValueTypeCode.Single )
			{
				return ( Single )BitConverter.ToSingle( BitConverter.GetBytes( value._value ), 0 );
			}
			else
			{
				ThrowInvalidTypeAs<float>( value );
				return default( float ); // never reached
			}
		}

		/// <summary>
		///		Convert this instance to <see cref="Double" /> instance.
		/// </summary>
		/// <param name="value"><see cref="MessagePackObject"/> instance.</param>
		/// <returns><see cref="Double" /> instance corresponds to <paramref name="value"/>.</returns>
		public static explicit operator Double( MessagePackObject value )
		{
			if( value.IsNil )
			{
				ThrowCannotBeNilAs<Double>();
			}
			var typeCode = value._handleOrTypeCode as ValueTypeCode;
			if( typeCode == null )
			{
				ThrowInvalidTypeAs<double>( value );
			}
			
			if( typeCode.IsInteger )
			{
				if( typeCode.IsSigned )
				{
					return ( double )( unchecked( ( long )value._value ) );
				}
				else
				{
					return ( double )value._value;
				}
			}
			else if( typeCode.TypeCode == MessagePackValueTypeCode.Double )
			{
				return ( Double )BitConverter.Int64BitsToDouble( unchecked( ( long )value._value ) );
			}
			else if( typeCode.TypeCode == MessagePackValueTypeCode.Single )
			{
				return ( Double )BitConverter.ToSingle( BitConverter.GetBytes( value._value ), 0 );
			}
			else
			{
				ThrowInvalidTypeAs<double>( value );
				return default( double ); // never reached
			}
		}

		/// <summary>
		///		Convert this instance to <see cref="String" /> instance.
		/// </summary>
		/// <param name="value"><see cref="MessagePackObject"/> instance.</param>
		/// <returns><see cref="String" /> instance corresponds to <paramref name="value"/>.</returns>
		public static explicit operator String( MessagePackObject value )
		{
			VerifyUnderlyingType<MessagePackString>( value, "value" );

			if( value._handleOrTypeCode == null )
			{
				// nil
				return null;
			}

			var asString = value._handleOrTypeCode as MessagePackString;
#if !UNITY && DEBUG
			Contract.Assert( asString != null, "asString != null" );
#endif // !UNITY && DEBUG
			return asString.GetString();
		}

		/// <summary>
		///		Convert this instance to <see cref="Byte" />[] instance.
		/// </summary>
		/// <param name="value"><see cref="MessagePackObject"/> instance.</param>
		/// <returns><see cref="Byte" />[] instance corresponds to <paramref name="value"/>.</returns>
		public static explicit operator Byte[]( MessagePackObject value )
		{
			VerifyUnderlyingType<MessagePackString>( value, "value"  );

			if( value._handleOrTypeCode == null )
			{
				// nil
				return null;
			}

			var asString = value._handleOrTypeCode as MessagePackString;
#if !UNITY && DEBUG
			Contract.Assert( asString != null, "asString != null" );
#endif // !UNITY && DEBUG
			return asString.GetBytes();
		}

		/// <summary>
		///		Convert this instance to <see cref="MessagePackExtendedTypeObject" /> instance.
		/// </summary>
		/// <param name="value"><see cref="MessagePackObject"/> instance.</param>
		/// <returns><see cref="MessagePackExtendedTypeObject" /> instance corresponds to <paramref name="value"/>.</returns>
		public static explicit operator MessagePackExtendedTypeObject( MessagePackObject value )
		{
			if( value.IsNil )
			{
				ThrowCannotBeNilAs<MessagePackExtendedTypeObject>();
			}
			VerifyUnderlyingType<MessagePackExtendedTypeObject>( value, "value"  );

			return MessagePackExtendedTypeObject.Unpack( unchecked( ( byte )value._value ), value._handleOrTypeCode as byte[] );
		}

		#endregion -- Conversion Operator Overloads --
	}
}
