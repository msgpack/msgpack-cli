#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2012 FUJIWARA, Yusuke
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
using System.Globalization;
using System.Reflection;
using System.Runtime.Serialization;

namespace MsgPack.Serialization.DefaultSerializers
{
	// This file generated from DefaultSerializers.tt T4Template.
	// Do not modify this file. Edit DefaultMarshalers.tt instead.

	internal sealed class System_DateTimeMessagePackSerializer : MessagePackSerializer< System.DateTime >
	{
		protected internal sealed override void PackToCore( Packer packer, System.DateTime value )
		{
			packer.Pack( MessagePackConvert.FromDateTime( value ) );
		}

		protected internal sealed override  System.DateTime UnpackFromCore( Unpacker unpacker )
		{
			try
			{
				return MessagePackConvert.ToDateTime( unpacker.Data.Value.AsInt64() ); 
			}
			catch( ArgumentException ex )
			{
				throw new SerializationException( String.Format( CultureInfo.CurrentCulture, "The unpacked value is not expected type. {0}", ex.Message ), ex );
			}
			catch( InvalidOperationException ex )
			{
				throw new SerializationException( String.Format( CultureInfo.CurrentCulture, "The unpacked value is not expected type. {0}", ex.Message ), ex );
			}
		}
	}

	internal sealed class System_DateTimeOffsetMessagePackSerializer : MessagePackSerializer< System.DateTimeOffset >
	{
		protected internal sealed override void PackToCore( Packer packer, System.DateTimeOffset value )
		{
			packer.Pack( MessagePackConvert.FromDateTimeOffset( value ) );
		}

		protected internal sealed override  System.DateTimeOffset UnpackFromCore( Unpacker unpacker )
		{
			try
			{
				return MessagePackConvert.ToDateTimeOffset( unpacker.Data.Value.AsInt64() ); 
			}
			catch( ArgumentException ex )
			{
				throw new SerializationException( String.Format( CultureInfo.CurrentCulture, "The unpacked value is not expected type. {0}", ex.Message ), ex );
			}
			catch( InvalidOperationException ex )
			{
				throw new SerializationException( String.Format( CultureInfo.CurrentCulture, "The unpacked value is not expected type. {0}", ex.Message ), ex );
			}
		}
	}

	internal sealed class System_BooleanMessagePackSerializer : MessagePackSerializer< System.Boolean >
	{
		protected internal sealed override void PackToCore( Packer packer, System.Boolean value )
		{
			packer.Pack( value );
		}

		protected internal sealed override  System.Boolean UnpackFromCore( Unpacker unpacker )
		{
			try
			{
				return unpacker.Data.Value.AsBoolean();
			}
			catch( InvalidOperationException ex )
			{
				throw new SerializationException( String.Format( CultureInfo.CurrentCulture, "The unpacked value is not '{0}' type. {1}", typeof( System.Boolean ), ex.Message ) );
			}
		}
	}

	internal sealed class System_ByteMessagePackSerializer : MessagePackSerializer< System.Byte >
	{
		protected internal sealed override void PackToCore( Packer packer, System.Byte value )
		{
			packer.Pack( value );
		}

		protected internal sealed override  System.Byte UnpackFromCore( Unpacker unpacker )
		{
			try
			{
				return unpacker.Data.Value.AsByte();
			}
			catch( InvalidOperationException ex )
			{
				throw new SerializationException( String.Format( CultureInfo.CurrentCulture, "The unpacked value is not '{0}' type. {1}", typeof( System.Byte ), ex.Message ) );
			}
		}
	}

	internal sealed class System_CharMessagePackSerializer : MessagePackSerializer< System.Char >
	{
		protected internal sealed override void PackToCore( Packer packer, System.Char value )
		{
			packer.Pack( ( System.UInt16 )value );
		}

		protected internal sealed override  System.Char UnpackFromCore( Unpacker unpacker )
		{
			try
			{
				return ( System.Char ) unpacker.Data.Value.AsUInt16(); 
			}
			catch( ArgumentException ex )
			{
				throw new SerializationException( String.Format( CultureInfo.CurrentCulture, "The unpacked value is not expected type. {0}", ex.Message ), ex );
			}
			catch( InvalidOperationException ex )
			{
				throw new SerializationException( String.Format( CultureInfo.CurrentCulture, "The unpacked value is not expected type. {0}", ex.Message ), ex );
			}
		}
	}

	internal sealed class System_DecimalMessagePackSerializer : MessagePackSerializer< System.Decimal >
	{
		protected internal sealed override void PackToCore( Packer packer, System.Decimal value )
		{
			packer.PackString( value.ToString( "G", CultureInfo.InvariantCulture ) );
		}

		protected internal sealed override  System.Decimal UnpackFromCore( Unpacker unpacker )
		{
			try
			{
				return System.Decimal.Parse( unpacker.Data.Value.AsString(), CultureInfo.InvariantCulture ); 
			}
			catch( ArgumentException ex )
			{
				throw new SerializationException( String.Format( CultureInfo.CurrentCulture, "The unpacked value is not expected type. {0}", ex.Message ), ex );
			}
			catch( InvalidOperationException ex )
			{
				throw new SerializationException( String.Format( CultureInfo.CurrentCulture, "The unpacked value is not expected type. {0}", ex.Message ), ex );
			}
		}
	}

	internal sealed class System_DoubleMessagePackSerializer : MessagePackSerializer< System.Double >
	{
		protected internal sealed override void PackToCore( Packer packer, System.Double value )
		{
			packer.Pack( value );
		}

		protected internal sealed override  System.Double UnpackFromCore( Unpacker unpacker )
		{
			try
			{
				return unpacker.Data.Value.AsDouble();
			}
			catch( InvalidOperationException ex )
			{
				throw new SerializationException( String.Format( CultureInfo.CurrentCulture, "The unpacked value is not '{0}' type. {1}", typeof( System.Double ), ex.Message ) );
			}
		}
	}

	internal sealed class System_GuidMessagePackSerializer : MessagePackSerializer< System.Guid >
	{
		protected internal sealed override void PackToCore( Packer packer, System.Guid value )
		{
			packer.PackRaw( value.ToByteArray() );
		}

		protected internal sealed override  System.Guid UnpackFromCore( Unpacker unpacker )
		{
			try
			{
				return new System.Guid( unpacker.Data.Value.AsBinary() ); 
			}
			catch( ArgumentException ex )
			{
				throw new SerializationException( String.Format( CultureInfo.CurrentCulture, "The unpacked value is not expected type. {0}", ex.Message ), ex );
			}
			catch( InvalidOperationException ex )
			{
				throw new SerializationException( String.Format( CultureInfo.CurrentCulture, "The unpacked value is not expected type. {0}", ex.Message ), ex );
			}
		}
	}

	internal sealed class System_Int16MessagePackSerializer : MessagePackSerializer< System.Int16 >
	{
		protected internal sealed override void PackToCore( Packer packer, System.Int16 value )
		{
			packer.Pack( value );
		}

		protected internal sealed override  System.Int16 UnpackFromCore( Unpacker unpacker )
		{
			try
			{
				return unpacker.Data.Value.AsInt16();
			}
			catch( InvalidOperationException ex )
			{
				throw new SerializationException( String.Format( CultureInfo.CurrentCulture, "The unpacked value is not '{0}' type. {1}", typeof( System.Int16 ), ex.Message ) );
			}
		}
	}

	internal sealed class System_Int32MessagePackSerializer : MessagePackSerializer< System.Int32 >
	{
		protected internal sealed override void PackToCore( Packer packer, System.Int32 value )
		{
			packer.Pack( value );
		}

		protected internal sealed override  System.Int32 UnpackFromCore( Unpacker unpacker )
		{
			try
			{
				return unpacker.Data.Value.AsInt32();
			}
			catch( InvalidOperationException ex )
			{
				throw new SerializationException( String.Format( CultureInfo.CurrentCulture, "The unpacked value is not '{0}' type. {1}", typeof( System.Int32 ), ex.Message ) );
			}
		}
	}

	internal sealed class System_Int64MessagePackSerializer : MessagePackSerializer< System.Int64 >
	{
		protected internal sealed override void PackToCore( Packer packer, System.Int64 value )
		{
			packer.Pack( value );
		}

		protected internal sealed override  System.Int64 UnpackFromCore( Unpacker unpacker )
		{
			try
			{
				return unpacker.Data.Value.AsInt64();
			}
			catch( InvalidOperationException ex )
			{
				throw new SerializationException( String.Format( CultureInfo.CurrentCulture, "The unpacked value is not '{0}' type. {1}", typeof( System.Int64 ), ex.Message ) );
			}
		}
	}

	internal sealed class System_SByteMessagePackSerializer : MessagePackSerializer< System.SByte >
	{
		protected internal sealed override void PackToCore( Packer packer, System.SByte value )
		{
			packer.Pack( value );
		}

		protected internal sealed override  System.SByte UnpackFromCore( Unpacker unpacker )
		{
			try
			{
				return unpacker.Data.Value.AsSByte();
			}
			catch( InvalidOperationException ex )
			{
				throw new SerializationException( String.Format( CultureInfo.CurrentCulture, "The unpacked value is not '{0}' type. {1}", typeof( System.SByte ), ex.Message ) );
			}
		}
	}

	internal sealed class System_SingleMessagePackSerializer : MessagePackSerializer< System.Single >
	{
		protected internal sealed override void PackToCore( Packer packer, System.Single value )
		{
			packer.Pack( value );
		}

		protected internal sealed override  System.Single UnpackFromCore( Unpacker unpacker )
		{
			try
			{
				return unpacker.Data.Value.AsSingle();
			}
			catch( InvalidOperationException ex )
			{
				throw new SerializationException( String.Format( CultureInfo.CurrentCulture, "The unpacked value is not '{0}' type. {1}", typeof( System.Single ), ex.Message ) );
			}
		}
	}

	internal sealed class System_TimeSpanMessagePackSerializer : MessagePackSerializer< System.TimeSpan >
	{
		protected internal sealed override void PackToCore( Packer packer, System.TimeSpan value )
		{
			packer.Pack( value.Ticks );
		}

		protected internal sealed override  System.TimeSpan UnpackFromCore( Unpacker unpacker )
		{
			System.Int64 ctorArgument;
			try
			{
				ctorArgument = unpacker.Data.Value.AsInt64();
			}
			catch( InvalidOperationException ex )
			{
				throw new SerializationException( String.Format( CultureInfo.CurrentCulture, "The unpacked value is not '{0}' type. {1}", typeof( System.Int64 ), ex.Message ) );
			}

			return new System.TimeSpan( ctorArgument );
		}
	}

	internal sealed class System_UInt16MessagePackSerializer : MessagePackSerializer< System.UInt16 >
	{
		protected internal sealed override void PackToCore( Packer packer, System.UInt16 value )
		{
			packer.Pack( value );
		}

		protected internal sealed override  System.UInt16 UnpackFromCore( Unpacker unpacker )
		{
			try
			{
				return unpacker.Data.Value.AsUInt16();
			}
			catch( InvalidOperationException ex )
			{
				throw new SerializationException( String.Format( CultureInfo.CurrentCulture, "The unpacked value is not '{0}' type. {1}", typeof( System.UInt16 ), ex.Message ) );
			}
		}
	}

	internal sealed class System_UInt32MessagePackSerializer : MessagePackSerializer< System.UInt32 >
	{
		protected internal sealed override void PackToCore( Packer packer, System.UInt32 value )
		{
			packer.Pack( value );
		}

		protected internal sealed override  System.UInt32 UnpackFromCore( Unpacker unpacker )
		{
			try
			{
				return unpacker.Data.Value.AsUInt32();
			}
			catch( InvalidOperationException ex )
			{
				throw new SerializationException( String.Format( CultureInfo.CurrentCulture, "The unpacked value is not '{0}' type. {1}", typeof( System.UInt32 ), ex.Message ) );
			}
		}
	}

	internal sealed class System_UInt64MessagePackSerializer : MessagePackSerializer< System.UInt64 >
	{
		protected internal sealed override void PackToCore( Packer packer, System.UInt64 value )
		{
			packer.Pack( value );
		}

		protected internal sealed override  System.UInt64 UnpackFromCore( Unpacker unpacker )
		{
			try
			{
				return unpacker.Data.Value.AsUInt64();
			}
			catch( InvalidOperationException ex )
			{
				throw new SerializationException( String.Format( CultureInfo.CurrentCulture, "The unpacked value is not '{0}' type. {1}", typeof( System.UInt64 ), ex.Message ) );
			}
		}
	}

#if !SILVERLIGHT && !NETFX_CORE
	internal sealed class System_Collections_Specialized_BitVector32MessagePackSerializer : MessagePackSerializer< System.Collections.Specialized.BitVector32 >
	{
		protected internal sealed override void PackToCore( Packer packer, System.Collections.Specialized.BitVector32 value )
		{
			packer.Pack( value.Data );
		}

		protected internal sealed override  System.Collections.Specialized.BitVector32 UnpackFromCore( Unpacker unpacker )
		{
			System.Int32 ctorArgument;
			try
			{
				ctorArgument = unpacker.Data.Value.AsInt32();
			}
			catch( InvalidOperationException ex )
			{
				throw new SerializationException( String.Format( CultureInfo.CurrentCulture, "The unpacked value is not '{0}' type. {1}", typeof( System.Int32 ), ex.Message ) );
			}

			return new System.Collections.Specialized.BitVector32( ctorArgument );
		}
	}
#endif // !SILVERLIGHT && !NETFX_CORE

#if !WINDOWS_PHONE && !NETFX_35
	internal sealed class System_Numerics_BigIntegerMessagePackSerializer : MessagePackSerializer< System.Numerics.BigInteger >
	{
		protected internal sealed override void PackToCore( Packer packer, System.Numerics.BigInteger value )
		{
			packer.PackRaw( value.ToByteArray() );
		}

		protected internal sealed override  System.Numerics.BigInteger UnpackFromCore( Unpacker unpacker )
		{
			try
			{
				return new System.Numerics.BigInteger( unpacker.Data.Value.AsBinary() ); 
			}
			catch( ArgumentException ex )
			{
				throw new SerializationException( String.Format( CultureInfo.CurrentCulture, "The unpacked value is not expected type. {0}", ex.Message ), ex );
			}
			catch( InvalidOperationException ex )
			{
				throw new SerializationException( String.Format( CultureInfo.CurrentCulture, "The unpacked value is not expected type. {0}", ex.Message ), ex );
			}
		}
	}
#endif // !WINDOWS_PHONE && !NETFX_35
}
