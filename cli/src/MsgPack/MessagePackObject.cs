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
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;

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
		///		Initialize new instance wraps <see cref="Boolean"/> instance.
		/// </summary>
		public MessagePackObject( Boolean value )
		{
			// trick: Avoid long boilerplate initialization. See "CLR via C#".
			this = new MessagePackObject();
			this._value = value ? ( ulong )1 : 0;
			this._handleOrTypeCode = _booleanTypeCode;
		}

		/// <summary>
		///		Initialize new instance wraps <see cref="Byte"/> instance.
		/// </summary>
		public MessagePackObject( Byte value )
		{
			// trick: Avoid long boilerplate initialization. See "CLR via C#".
			this = new MessagePackObject();
			this._value = value;
			this._handleOrTypeCode = _byteTypeCode;
		}

		/// <summary>
		///		Initialize new instance wraps <see cref="SByte"/> instance.
		/// </summary>
		[CLSCompliant( false )]
		public MessagePackObject( SByte value )
		{
			// trick: Avoid long boilerplate initialization. See "CLR via C#".
			this = new MessagePackObject();
			this._value = unchecked( ( ulong )value );
			this._handleOrTypeCode = _sbyteTypeCode;
		}

		/// <summary>
		///		Initialize new instance wraps <see cref="Int16"/> instance.
		/// </summary>
		public MessagePackObject( Int16 value )
		{
			// trick: Avoid long boilerplate initialization. See "CLR via C#".
			this = new MessagePackObject();
			this._value = unchecked( ( ulong )value );
			this._handleOrTypeCode = _int16TypeCode;
		}

		/// <summary>
		///		Initialize new instance wraps <see cref="UInt16"/> instance.
		/// </summary>
		[CLSCompliant( false )]
		public MessagePackObject( UInt16 value )
		{
			// trick: Avoid long boilerplate initialization. See "CLR via C#".
			this = new MessagePackObject();
			this._value = value;
			this._handleOrTypeCode = _uint16TypeCode;
		}

		/// <summary>
		///		Initialize new instance wraps <see cref="Int32"/> instance.
		/// </summary>
		public MessagePackObject( Int32 value )
		{
			// trick: Avoid long boilerplate initialization. See "CLR via C#".
			this = new MessagePackObject();
			this._value = unchecked( ( ulong )value );
			this._handleOrTypeCode = _int32TypeCode;
		}

		/// <summary>
		///		Initialize new instance wraps <see cref="UInt32"/> instance.
		/// </summary>
		[CLSCompliant( false )]
		public MessagePackObject( UInt32 value )
		{
			// trick: Avoid long boilerplate initialization. See "CLR via C#".
			this = new MessagePackObject();
			this._value = value;
			this._handleOrTypeCode = _uint32TypeCode;
		}

		/// <summary>
		///		Initialize new instance wraps <see cref="Int64"/> instance.
		/// </summary>
		public MessagePackObject( Int64 value )
		{
			// trick: Avoid long boilerplate initialization. See "CLR via C#".
			this = new MessagePackObject();
			this._value = unchecked( ( ulong )value );
			this._handleOrTypeCode = _int64TypeCode;
		}

		/// <summary>
		///		Initialize new instance wraps <see cref="UInt64"/> instance.
		/// </summary>
		[CLSCompliant( false )]
		public MessagePackObject( UInt64 value )
		{
			// trick: Avoid long boilerplate initialization. See "CLR via C#".
			this = new MessagePackObject();
			this._value = value;
			this._handleOrTypeCode = _uint64TypeCode;
		}

		/// <summary>
		///		Initialize new instance wraps <see cref="Single"/> instance.
		/// </summary>
		public MessagePackObject( Single value )
		{
			// trick: Avoid long boilerplate initialization. See "CLR via C#".
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
		///		Initialize new instance wraps <see cref="Double"/> instance.
		/// </summary>
		public MessagePackObject( Double value )
		{
			// trick: Avoid long boilerplate initialization. See "CLR via C#".
			this = new MessagePackObject();
			this._value = unchecked( ( ulong )BitConverter.DoubleToInt64Bits( value ) );
			this._handleOrTypeCode = _doubleTypeCode;
		}

#if !SILVERLIGHT
		private MessagePackObject( SerializationInfo info, StreamingContext context )
		{
			if ( info == null )
			{
				throw new ArgumentNullException( "info" );
			}

			Contract.EndContractBlock();

			this = new MessagePackObject();
			switch ( ( TypeCode )info.GetValue( "TypeCode", typeof( TypeCode ) ) )
			{
				case TypeCode.Empty:
				{
					this._handleOrTypeCode = null;
					return;
				}
				case TypeCode.Object:
				{
					this._handleOrTypeCode = info.GetValue( "Value", typeof( object ) );
					return;
				}
				case TypeCode.Boolean:
				{
					this._handleOrTypeCode = _booleanTypeCode;
					this._value = info.GetUInt64( "Value" );
					return;
				}
				case TypeCode.Byte:
				{
					this._handleOrTypeCode = _byteTypeCode;
					this._value = info.GetUInt64( "Value" );
					return;
				}
				case TypeCode.SByte:
				{
					this._handleOrTypeCode = _sbyteTypeCode;
					this._value = info.GetUInt64( "Value" );
					return;
				}
				case TypeCode.Int16:
				{
					this._handleOrTypeCode = _int16TypeCode;
					this._value = info.GetUInt64( "Value" );
					return;
				}
				case TypeCode.UInt16:
				{
					this._handleOrTypeCode = _uint16TypeCode;
					this._value = info.GetUInt64( "Value" );
					return;
				}
				case TypeCode.Int32:
				{
					this._handleOrTypeCode = _int32TypeCode;
					this._value = info.GetUInt64( "Value" );
					return;
				}
				case TypeCode.UInt32:
				{
					this._handleOrTypeCode = _uint32TypeCode;
					this._value = info.GetUInt64( "Value" );
					return;
				}
				case TypeCode.Int64:
				{
					this._handleOrTypeCode = _int64TypeCode;
					this._value = info.GetUInt64( "Value" );
					return;
				}
				case TypeCode.UInt64:
				{
					this._handleOrTypeCode = _uint64TypeCode;
					this._value = info.GetUInt64( "Value" );
					return;
				}
				case TypeCode.Single:
				{
					this._handleOrTypeCode = _singleTypeCode;
					this._value = info.GetUInt64( "Value" );
					return;
				}
				case TypeCode.Double:
				{
					this._handleOrTypeCode = _doubleTypeCode;
					this._value = info.GetUInt64( "Value" );
					return;
				}
				default:
				{
					throw new SerializationException( String.Format( CultureInfo.CurrentCulture, "Unknown type code {0}",( TypeCode )info.GetValue( "TypeCode", typeof( TypeCode ) ) ) );
				}
			}
		}

#endif
		#endregion -- Constructors --


		#region -- Primitive Type Conversion Methods --

		/// <summary>
		///		Convert this instance to <see cref="Boolean"/> instance.
		/// </summary>
		/// <returns><see cref="Boolean"/> instance corresponds to this instance.</returns>
		public Boolean AsBoolean()
		{
			return ( Boolean )this;
		}

		/// <summary>
		///		Convert this instance to <see cref="Byte"/> instance.
		/// </summary>
		/// <returns><see cref="Byte"/> instance corresponds to this instance.</returns>
		public Byte AsByte()
		{
			return ( Byte )this;
		}

		/// <summary>
		///		Convert this instance to <see cref="SByte"/> instance.
		/// </summary>
		/// <returns><see cref="SByte"/> instance corresponds to this instance.</returns>
		[CLSCompliant( false )]
		public SByte AsSByte()
		{
			return ( SByte )this;
		}

		/// <summary>
		///		Convert this instance to <see cref="Int16"/> instance.
		/// </summary>
		/// <returns><see cref="Int16"/> instance corresponds to this instance.</returns>
		public Int16 AsInt16()
		{
			return ( Int16 )this;
		}

		/// <summary>
		///		Convert this instance to <see cref="UInt16"/> instance.
		/// </summary>
		/// <returns><see cref="UInt16"/> instance corresponds to this instance.</returns>
		[CLSCompliant( false )]
		public UInt16 AsUInt16()
		{
			return ( UInt16 )this;
		}

		/// <summary>
		///		Convert this instance to <see cref="Int32"/> instance.
		/// </summary>
		/// <returns><see cref="Int32"/> instance corresponds to this instance.</returns>
		public Int32 AsInt32()
		{
			return ( Int32 )this;
		}

		/// <summary>
		///		Convert this instance to <see cref="UInt32"/> instance.
		/// </summary>
		/// <returns><see cref="UInt32"/> instance corresponds to this instance.</returns>
		[CLSCompliant( false )]
		public UInt32 AsUInt32()
		{
			return ( UInt32 )this;
		}

		/// <summary>
		///		Convert this instance to <see cref="Int64"/> instance.
		/// </summary>
		/// <returns><see cref="Int64"/> instance corresponds to this instance.</returns>
		public Int64 AsInt64()
		{
			return ( Int64 )this;
		}

		/// <summary>
		///		Convert this instance to <see cref="UInt64"/> instance.
		/// </summary>
		/// <returns><see cref="UInt64"/> instance corresponds to this instance.</returns>
		[CLSCompliant( false )]
		public UInt64 AsUInt64()
		{
			return ( UInt64 )this;
		}

		/// <summary>
		///		Convert this instance to <see cref="Single"/> instance.
		/// </summary>
		/// <returns><see cref="Single"/> instance corresponds to this instance.</returns>
		public Single AsSingle()
		{
			return ( Single )this;
		}

		/// <summary>
		///		Convert this instance to <see cref="Double"/> instance.
		/// </summary>
		/// <returns><see cref="Double"/> instance corresponds to this instance.</returns>
		public Double AsDouble()
		{
			return ( Double )this;
		}

		#endregion -- Primitive Type Conversion Methods --

#if !SILVERLIGHT
		private static bool AddPrimitiveToSerializationInfo( SerializationInfo info, string name, MessagePackObject value )
		{
			if( value.IsNil )
			{
				info.AddValue( name, null );
				return true;
			}
	
			else if( value._handleOrTypeCode == _booleanTypeCode )
			{
				info.AddValue( name, value.AsBoolean() );
				return true;
			}
	
			else if( value._handleOrTypeCode == _byteTypeCode )
			{
				info.AddValue( name, value.AsByte() );
				return true;
			}
	
			else if( value._handleOrTypeCode == _sbyteTypeCode )
			{
				info.AddValue( name, value.AsSByte() );
				return true;
			}
	
			else if( value._handleOrTypeCode == _int16TypeCode )
			{
				info.AddValue( name, value.AsInt16() );
				return true;
			}
	
			else if( value._handleOrTypeCode == _uint16TypeCode )
			{
				info.AddValue( name, value.AsUInt16() );
				return true;
			}
	
			else if( value._handleOrTypeCode == _int32TypeCode )
			{
				info.AddValue( name, value.AsInt32() );
				return true;
			}
	
			else if( value._handleOrTypeCode == _uint32TypeCode )
			{
				info.AddValue( name, value.AsUInt32() );
				return true;
			}
	
			else if( value._handleOrTypeCode == _int64TypeCode )
			{
				info.AddValue( name, value.AsInt64() );
				return true;
			}
	
			else if( value._handleOrTypeCode == _uint64TypeCode )
			{
				info.AddValue( name, value.AsUInt64() );
				return true;
			}
	
			else if( value._handleOrTypeCode == _singleTypeCode )
			{
				info.AddValue( name, value.AsSingle() );
				return true;
			}
	
			else if( value._handleOrTypeCode == _doubleTypeCode )
			{
				info.AddValue( name, value.AsDouble() );
				return true;
			}
			return false;
		}

#endif

		#region -- Conversion Operator Overloads --


		/// <summary>
		///		Convert <see cref="Boolean"/> instance to <see cref="MessagePackObject"/> instance.
		/// </summary>
		/// <param name="value"><see cref="Boolean"/> instance.</param>
		/// <returns><see cref="MessagePackObject"/> instance corresponds to <paramref name="value"/>.</returns>
		public static implicit operator MessagePackObject( Boolean value )
		{
			return new MessagePackObject( value );
		}

		/// <summary>
		///		Convert <see cref="Byte"/> instance to <see cref="MessagePackObject"/> instance.
		/// </summary>
		/// <param name="value"><see cref="Byte"/> instance.</param>
		/// <returns><see cref="MessagePackObject"/> instance corresponds to <paramref name="value"/>.</returns>
		public static implicit operator MessagePackObject( Byte value )
		{
			return new MessagePackObject( value );
		}

		/// <summary>
		///		Convert <see cref="SByte"/> instance to <see cref="MessagePackObject"/> instance.
		/// </summary>
		/// <param name="value"><see cref="SByte"/> instance.</param>
		/// <returns><see cref="MessagePackObject"/> instance corresponds to <paramref name="value"/>.</returns>
		[CLSCompliant( false )]
		public static implicit operator MessagePackObject( SByte value )
		{
			return new MessagePackObject( value );
		}

		/// <summary>
		///		Convert <see cref="Int16"/> instance to <see cref="MessagePackObject"/> instance.
		/// </summary>
		/// <param name="value"><see cref="Int16"/> instance.</param>
		/// <returns><see cref="MessagePackObject"/> instance corresponds to <paramref name="value"/>.</returns>
		public static implicit operator MessagePackObject( Int16 value )
		{
			return new MessagePackObject( value );
		}

		/// <summary>
		///		Convert <see cref="UInt16"/> instance to <see cref="MessagePackObject"/> instance.
		/// </summary>
		/// <param name="value"><see cref="UInt16"/> instance.</param>
		/// <returns><see cref="MessagePackObject"/> instance corresponds to <paramref name="value"/>.</returns>
		[CLSCompliant( false )]
		public static implicit operator MessagePackObject( UInt16 value )
		{
			return new MessagePackObject( value );
		}

		/// <summary>
		///		Convert <see cref="Int32"/> instance to <see cref="MessagePackObject"/> instance.
		/// </summary>
		/// <param name="value"><see cref="Int32"/> instance.</param>
		/// <returns><see cref="MessagePackObject"/> instance corresponds to <paramref name="value"/>.</returns>
		public static implicit operator MessagePackObject( Int32 value )
		{
			return new MessagePackObject( value );
		}

		/// <summary>
		///		Convert <see cref="UInt32"/> instance to <see cref="MessagePackObject"/> instance.
		/// </summary>
		/// <param name="value"><see cref="UInt32"/> instance.</param>
		/// <returns><see cref="MessagePackObject"/> instance corresponds to <paramref name="value"/>.</returns>
		[CLSCompliant( false )]
		public static implicit operator MessagePackObject( UInt32 value )
		{
			return new MessagePackObject( value );
		}

		/// <summary>
		///		Convert <see cref="Int64"/> instance to <see cref="MessagePackObject"/> instance.
		/// </summary>
		/// <param name="value"><see cref="Int64"/> instance.</param>
		/// <returns><see cref="MessagePackObject"/> instance corresponds to <paramref name="value"/>.</returns>
		public static implicit operator MessagePackObject( Int64 value )
		{
			return new MessagePackObject( value );
		}

		/// <summary>
		///		Convert <see cref="UInt64"/> instance to <see cref="MessagePackObject"/> instance.
		/// </summary>
		/// <param name="value"><see cref="UInt64"/> instance.</param>
		/// <returns><see cref="MessagePackObject"/> instance corresponds to <paramref name="value"/>.</returns>
		[CLSCompliant( false )]
		public static implicit operator MessagePackObject( UInt64 value )
		{
			return new MessagePackObject( value );
		}

		/// <summary>
		///		Convert <see cref="Single"/> instance to <see cref="MessagePackObject"/> instance.
		/// </summary>
		/// <param name="value"><see cref="Single"/> instance.</param>
		/// <returns><see cref="MessagePackObject"/> instance corresponds to <paramref name="value"/>.</returns>
		public static implicit operator MessagePackObject( Single value )
		{
			return new MessagePackObject( value );
		}

		/// <summary>
		///		Convert <see cref="Double"/> instance to <see cref="MessagePackObject"/> instance.
		/// </summary>
		/// <param name="value"><see cref="Double"/> instance.</param>
		/// <returns><see cref="MessagePackObject"/> instance corresponds to <paramref name="value"/>.</returns>
		public static implicit operator MessagePackObject( Double value )
		{
			return new MessagePackObject( value );
		}


		/// <summary>
		///		Convert this instance to <see cref="Boolean"/> instance.
		/// </summary>
		/// <param name="value"><see cref="MessagePackObject"/> instance.</param>
		/// <returns><see cref="Boolean"/> instance corresponds to <paramref name="value"/>.</returns>
		public static explicit operator Boolean( MessagePackObject value )
		{
			VerifyUnderlyingType<Boolean>( value, "value" );

			return value._value != 0;
		}

		/// <summary>
		///		Convert this instance to <see cref="Byte"/> instance.
		/// </summary>
		/// <param name="value"><see cref="MessagePackObject"/> instance.</param>
		/// <returns><see cref="Byte"/> instance corresponds to <paramref name="value"/>.</returns>
		public static explicit operator Byte( MessagePackObject value )
		{
			VerifyUnderlyingType<Byte>( value, "value" );

			// It's safe since already verified based on type code.
			return unchecked( ( Byte )value._value );
		}

		/// <summary>
		///		Convert this instance to <see cref="SByte"/> instance.
		/// </summary>
		/// <param name="value"><see cref="MessagePackObject"/> instance.</param>
		/// <returns><see cref="SByte"/> instance corresponds to <paramref name="value"/>.</returns>
		[CLSCompliant( false )]
		public static explicit operator SByte( MessagePackObject value )
		{
			VerifyUnderlyingType<SByte>( value, "value" );

			// It's safe since already verified based on type code.
			return unchecked( ( SByte )value._value );
		}

		/// <summary>
		///		Convert this instance to <see cref="Int16"/> instance.
		/// </summary>
		/// <param name="value"><see cref="MessagePackObject"/> instance.</param>
		/// <returns><see cref="Int16"/> instance corresponds to <paramref name="value"/>.</returns>
		public static explicit operator Int16( MessagePackObject value )
		{
			VerifyUnderlyingType<Int16>( value, "value" );

			// It's safe since already verified based on type code.
			return unchecked( ( Int16 )value._value );
		}

		/// <summary>
		///		Convert this instance to <see cref="UInt16"/> instance.
		/// </summary>
		/// <param name="value"><see cref="MessagePackObject"/> instance.</param>
		/// <returns><see cref="UInt16"/> instance corresponds to <paramref name="value"/>.</returns>
		[CLSCompliant( false )]
		public static explicit operator UInt16( MessagePackObject value )
		{
			VerifyUnderlyingType<UInt16>( value, "value" );

			// It's safe since already verified based on type code.
			return unchecked( ( UInt16 )value._value );
		}

		/// <summary>
		///		Convert this instance to <see cref="Int32"/> instance.
		/// </summary>
		/// <param name="value"><see cref="MessagePackObject"/> instance.</param>
		/// <returns><see cref="Int32"/> instance corresponds to <paramref name="value"/>.</returns>
		public static explicit operator Int32( MessagePackObject value )
		{
			VerifyUnderlyingType<Int32>( value, "value" );

			// It's safe since already verified based on type code.
			return unchecked( ( Int32 )value._value );
		}

		/// <summary>
		///		Convert this instance to <see cref="UInt32"/> instance.
		/// </summary>
		/// <param name="value"><see cref="MessagePackObject"/> instance.</param>
		/// <returns><see cref="UInt32"/> instance corresponds to <paramref name="value"/>.</returns>
		[CLSCompliant( false )]
		public static explicit operator UInt32( MessagePackObject value )
		{
			VerifyUnderlyingType<UInt32>( value, "value" );

			// It's safe since already verified based on type code.
			return unchecked( ( UInt32 )value._value );
		}

		/// <summary>
		///		Convert this instance to <see cref="Int64"/> instance.
		/// </summary>
		/// <param name="value"><see cref="MessagePackObject"/> instance.</param>
		/// <returns><see cref="Int64"/> instance corresponds to <paramref name="value"/>.</returns>
		public static explicit operator Int64( MessagePackObject value )
		{
			VerifyUnderlyingType<Int64>( value, "value" );

			// It's safe since already verified based on type code.
			return unchecked( ( Int64 )value._value );
		}

		/// <summary>
		///		Convert this instance to <see cref="UInt64"/> instance.
		/// </summary>
		/// <param name="value"><see cref="MessagePackObject"/> instance.</param>
		/// <returns><see cref="UInt64"/> instance corresponds to <paramref name="value"/>.</returns>
		[CLSCompliant( false )]
		public static explicit operator UInt64( MessagePackObject value )
		{
			VerifyUnderlyingType<UInt64>( value, "value" );

			return value._value;
		}

		/// <summary>
		///		Convert this instance to <see cref="Single"/> instance.
		/// </summary>
		/// <param name="value"><see cref="MessagePackObject"/> instance.</param>
		/// <returns><see cref="Single"/> instance corresponds to <paramref name="value"/>.</returns>
		public static explicit operator Single( MessagePackObject value )
		{
			VerifyUnderlyingType<Single>( value, "value" );

			return BitConverter.ToSingle( BitConverter.GetBytes( value._value ), 0 );
		}

		/// <summary>
		///		Convert this instance to <see cref="Double"/> instance.
		/// </summary>
		/// <param name="value"><see cref="MessagePackObject"/> instance.</param>
		/// <returns><see cref="Double"/> instance corresponds to <paramref name="value"/>.</returns>
		public static explicit operator Double( MessagePackObject value )
		{
			VerifyUnderlyingType<Double>( value, "value" );

			return BitConverter.Int64BitsToDouble( unchecked( ( long )value._value ) );
		}

		#endregion -- Conversion Operator Overloads --
	}
}