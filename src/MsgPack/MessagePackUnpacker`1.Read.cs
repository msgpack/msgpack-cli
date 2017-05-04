
#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2017 FUJIWARA, Yusuke
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
#if FEATURE_TAP
using System.Threading;
using System.Threading.Tasks;
#endif // FEATURE_TAP

namespace MsgPack
{
	// This file was generated from MessagePackUnpacker`1.tt and MessagePackUnpacker.Read.ttinclude T4Template.
	// Do not modify this file. Edit MessagePackUnpacker`1.tt and MessagePackUnpacker.Read.ttinclude instead.

	partial class MessagePackUnpacker<TReader>
	{
		// <WriteReadBody typeName="Boolean" isAsync="False">
		public override bool ReadBoolean( out Boolean result )
		{
			// <WriteReadBodyCore typeName="Boolean" nullability="Value" isAsync="False">
			byte header;
			long integral;
			float real32;
			double real64;
			var type = this.ReadValue( out header, out integral, out real32, out real64 );
			switch( type )
			{
				case ReadValueResult.Eof:
				{
					// <Fail resultVariable="result" typeName="Boolean" isAsync="False">
					result = default( Boolean );
					return false;
					// </Fail>
				}
				// <WriteReadScalarCore type="System.Boolean" isNullable="False" valueVariable="i: integral, r4: real32, r8: real64, a: asyncResult" resultVariable="result" isAsync="False">
				case ReadValueResult.Boolean:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="integral != 0" isAsync="False">
					result = integral != 0;
					return true;
					// </Success>
				}
				// </WriteReadScalarCore>
				default:
				{
					this.ThrowTypeException( typeof( Boolean ), header );
					// Never reach
					// <Fail resultVariable="result" typeName="Boolean" isAsync="False">
					result = default( Boolean );
					return false;
					// </Fail>
				}
			}
			// </WriteReadBodyCore>
		} 
		// </WriteReadBody>
		// <WriteReadBody typeName="Boolean" isAsync="True">

#if FEATURE_TAP

		public async override Task<AsyncReadResult<Boolean>> ReadBooleanAsync( CancellationToken cancellationToken )
		{
			// <WriteReadBodyCore typeName="Boolean" nullability="Value" isAsync="True">
			var asyncResult = await this.ReadValueAsync( cancellationToken ).ConfigureAwait( false );
			var type = asyncResult.type;
			switch( type )
			{
				case ReadValueResult.Eof:
				{
					// <Fail resultVariable="result" typeName="Boolean" isAsync="True">
					return AsyncReadResult.Fail<Boolean>();
					// </Fail>
				}
				// <WriteReadScalarCore type="System.Boolean" isNullable="False" valueVariable="i: integral, r4: real32, r8: real64, a: asyncResult" resultVariable="result" isAsync="True">
				case ReadValueResult.Boolean:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="asyncResult.integral != 0" isAsync="True">
					return AsyncReadResult.Success( asyncResult.integral != 0 );
					// </Success>
				}
				// </WriteReadScalarCore>
				default:
				{
					this.ThrowTypeException( typeof( Boolean ), asyncResult.header );
					// Never reach
					// <Fail resultVariable="result" typeName="Boolean" isAsync="True">
					return AsyncReadResult.Fail<Boolean>();
					// </Fail>
				}
			}
			// </WriteReadBodyCore>
		} 

#endif // FEATURE_TAP

		// </WriteReadBody>
		// <WriteReadBody typeName="NullableBoolean" isAsync="False">
		public override bool ReadNullableBoolean( out Boolean? result )
		{
			// <WriteReadBodyCore typeName="Boolean" nullability="Nullable" isAsync="False">
			byte header;
			long integral;
			float real32;
			double real64;
			var type = this.ReadValue( out header, out integral, out real32, out real64 );
			switch( type )
			{
				case ReadValueResult.Eof:
				{
					// <Fail resultVariable="result" typeName="Boolean?" isAsync="False">
					result = default( Boolean? );
					return false;
					// </Fail>
				}
				case ReadValueResult.Nil:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="default( Boolean? )" isAsync="False">
					result = default( Boolean? );
					return true;
					// </Success>
				}
				// <WriteReadScalarCore type="System.Boolean" isNullable="True" valueVariable="i: integral, r4: real32, r8: real64, a: asyncResult" resultVariable="result" isAsync="False">
				case ReadValueResult.Boolean:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="Boolean?" resultVariable="result" expression="integral != 0" isAsync="False">
					result = integral != 0;
					return true;
					// </Success>
				}
				// </WriteReadScalarCore>
				default:
				{
					this.ThrowTypeException( typeof( Boolean? ), header );
					// Never reach
					// <Fail resultVariable="result" typeName="Boolean?" isAsync="False">
					result = default( Boolean? );
					return false;
					// </Fail>
				}
			}
			// </WriteReadBodyCore>
		} 
		// </WriteReadBody>
		// <WriteReadBody typeName="NullableBoolean" isAsync="True">

#if FEATURE_TAP

		public async override Task<AsyncReadResult<Boolean?>> ReadNullableBooleanAsync( CancellationToken cancellationToken )
		{
			// <WriteReadBodyCore typeName="Boolean" nullability="Nullable" isAsync="True">
			var asyncResult = await this.ReadValueAsync( cancellationToken ).ConfigureAwait( false );
			var type = asyncResult.type;
			switch( type )
			{
				case ReadValueResult.Eof:
				{
					// <Fail resultVariable="result" typeName="Boolean?" isAsync="True">
					return AsyncReadResult.Fail<Boolean?>();
					// </Fail>
				}
				case ReadValueResult.Nil:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="default( Boolean? )" isAsync="True">
					return AsyncReadResult.Success( default( Boolean? ) );
					// </Success>
				}
				// <WriteReadScalarCore type="System.Boolean" isNullable="True" valueVariable="i: integral, r4: real32, r8: real64, a: asyncResult" resultVariable="result" isAsync="True">
				case ReadValueResult.Boolean:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="Boolean?" resultVariable="result" expression="asyncResult.integral != 0" isAsync="True">
					return AsyncReadResult.Success<Boolean?>( asyncResult.integral != 0 );
					// </Success>
				}
				// </WriteReadScalarCore>
				default:
				{
					this.ThrowTypeException( typeof( Boolean? ), asyncResult.header );
					// Never reach
					// <Fail resultVariable="result" typeName="Boolean?" isAsync="True">
					return AsyncReadResult.Fail<Boolean?>();
					// </Fail>
				}
			}
			// </WriteReadBodyCore>
		} 

#endif // FEATURE_TAP

		// </WriteReadBody>
		// <WriteReadBody typeName="Byte" isAsync="False">
		public override bool ReadByte( out Byte result )
		{
			// <WriteReadBodyCore typeName="Byte" nullability="Value" isAsync="False">
			byte header;
			long integral;
			float real32;
			double real64;
			var type = this.ReadValue( out header, out integral, out real32, out real64 );
			switch( type )
			{
				case ReadValueResult.Eof:
				{
					// <Fail resultVariable="result" typeName="Byte" isAsync="False">
					result = default( Byte );
					return false;
					// </Fail>
				}
				// <WriteReadScalarCore type="System.Byte" isNullable="False" valueVariable="i: integral, r4: real32, r8: real64, a: asyncResult" resultVariable="result" isAsync="False">
				case ReadValueResult.Byte:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="unchecked( ( Byte )integral )" isAsync="False">
					result = unchecked( ( Byte )integral );
					return true;
					// </Success>
				}
				case ReadValueResult.SByte:
				case ReadValueResult.Int16:
				case ReadValueResult.Int32:
				case ReadValueResult.Int64:
				case ReadValueResult.UInt16:
				case ReadValueResult.UInt32:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="checked( ( Byte )integral )" isAsync="False">
					result = checked( ( Byte )integral );
					return true;
					// </Success>
				}
				case ReadValueResult.UInt64:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="checked( ( Byte )( UInt64 )integral )" isAsync="False">
					result = checked( ( Byte )( UInt64 )integral );
					return true;
					// </Success>
				}
				case ReadValueResult.Single:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="checked( ( Byte )real32 )" isAsync="False">
					result = checked( ( Byte )real32 );
					return true;
					// </Success>
				}
				case ReadValueResult.Double:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="checked( ( Byte )real64 )" isAsync="False">
					result = checked( ( Byte )real64 );
					return true;
					// </Success>
				}
				// </WriteReadScalarCore>
				default:
				{
					this.ThrowTypeException( typeof( Byte ), header );
					// Never reach
					// <Fail resultVariable="result" typeName="Byte" isAsync="False">
					result = default( Byte );
					return false;
					// </Fail>
				}
			}
			// </WriteReadBodyCore>
		} 
		// </WriteReadBody>
		// <WriteReadBody typeName="Byte" isAsync="True">

#if FEATURE_TAP

		public async override Task<AsyncReadResult<Byte>> ReadByteAsync( CancellationToken cancellationToken )
		{
			// <WriteReadBodyCore typeName="Byte" nullability="Value" isAsync="True">
			var asyncResult = await this.ReadValueAsync( cancellationToken ).ConfigureAwait( false );
			var type = asyncResult.type;
			switch( type )
			{
				case ReadValueResult.Eof:
				{
					// <Fail resultVariable="result" typeName="Byte" isAsync="True">
					return AsyncReadResult.Fail<Byte>();
					// </Fail>
				}
				// <WriteReadScalarCore type="System.Byte" isNullable="False" valueVariable="i: integral, r4: real32, r8: real64, a: asyncResult" resultVariable="result" isAsync="True">
				case ReadValueResult.Byte:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="unchecked( ( Byte )asyncResult.integral )" isAsync="True">
					return AsyncReadResult.Success( unchecked( ( Byte )asyncResult.integral ) );
					// </Success>
				}
				case ReadValueResult.SByte:
				case ReadValueResult.Int16:
				case ReadValueResult.Int32:
				case ReadValueResult.Int64:
				case ReadValueResult.UInt16:
				case ReadValueResult.UInt32:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="checked( ( Byte )asyncResult.integral )" isAsync="True">
					return AsyncReadResult.Success( checked( ( Byte )asyncResult.integral ) );
					// </Success>
				}
				case ReadValueResult.UInt64:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="checked( ( Byte )( UInt64 )asyncResult.integral )" isAsync="True">
					return AsyncReadResult.Success( checked( ( Byte )( UInt64 )asyncResult.integral ) );
					// </Success>
				}
				case ReadValueResult.Single:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="checked( ( Byte )asyncResult.real32 )" isAsync="True">
					return AsyncReadResult.Success( checked( ( Byte )asyncResult.real32 ) );
					// </Success>
				}
				case ReadValueResult.Double:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="checked( ( Byte )asyncResult.real64 )" isAsync="True">
					return AsyncReadResult.Success( checked( ( Byte )asyncResult.real64 ) );
					// </Success>
				}
				// </WriteReadScalarCore>
				default:
				{
					this.ThrowTypeException( typeof( Byte ), asyncResult.header );
					// Never reach
					// <Fail resultVariable="result" typeName="Byte" isAsync="True">
					return AsyncReadResult.Fail<Byte>();
					// </Fail>
				}
			}
			// </WriteReadBodyCore>
		} 

#endif // FEATURE_TAP

		// </WriteReadBody>
		// <WriteReadBody typeName="NullableByte" isAsync="False">
		public override bool ReadNullableByte( out Byte? result )
		{
			// <WriteReadBodyCore typeName="Byte" nullability="Nullable" isAsync="False">
			byte header;
			long integral;
			float real32;
			double real64;
			var type = this.ReadValue( out header, out integral, out real32, out real64 );
			switch( type )
			{
				case ReadValueResult.Eof:
				{
					// <Fail resultVariable="result" typeName="Byte?" isAsync="False">
					result = default( Byte? );
					return false;
					// </Fail>
				}
				case ReadValueResult.Nil:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="default( Byte? )" isAsync="False">
					result = default( Byte? );
					return true;
					// </Success>
				}
				// <WriteReadScalarCore type="System.Byte" isNullable="True" valueVariable="i: integral, r4: real32, r8: real64, a: asyncResult" resultVariable="result" isAsync="False">
				case ReadValueResult.Byte:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="Byte?" resultVariable="result" expression="unchecked( ( Byte )integral )" isAsync="False">
					result = unchecked( ( Byte )integral );
					return true;
					// </Success>
				}
				case ReadValueResult.SByte:
				case ReadValueResult.Int16:
				case ReadValueResult.Int32:
				case ReadValueResult.Int64:
				case ReadValueResult.UInt16:
				case ReadValueResult.UInt32:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="Byte?" resultVariable="result" expression="checked( ( Byte )integral )" isAsync="False">
					result = checked( ( Byte )integral );
					return true;
					// </Success>
				}
				case ReadValueResult.UInt64:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="Byte?" resultVariable="result" expression="checked( ( Byte )( UInt64 )integral )" isAsync="False">
					result = checked( ( Byte )( UInt64 )integral );
					return true;
					// </Success>
				}
				case ReadValueResult.Single:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="Byte?" resultVariable="result" expression="checked( ( Byte )real32 )" isAsync="False">
					result = checked( ( Byte )real32 );
					return true;
					// </Success>
				}
				case ReadValueResult.Double:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="Byte?" resultVariable="result" expression="checked( ( Byte )real64 )" isAsync="False">
					result = checked( ( Byte )real64 );
					return true;
					// </Success>
				}
				// </WriteReadScalarCore>
				default:
				{
					this.ThrowTypeException( typeof( Byte? ), header );
					// Never reach
					// <Fail resultVariable="result" typeName="Byte?" isAsync="False">
					result = default( Byte? );
					return false;
					// </Fail>
				}
			}
			// </WriteReadBodyCore>
		} 
		// </WriteReadBody>
		// <WriteReadBody typeName="NullableByte" isAsync="True">

#if FEATURE_TAP

		public async override Task<AsyncReadResult<Byte?>> ReadNullableByteAsync( CancellationToken cancellationToken )
		{
			// <WriteReadBodyCore typeName="Byte" nullability="Nullable" isAsync="True">
			var asyncResult = await this.ReadValueAsync( cancellationToken ).ConfigureAwait( false );
			var type = asyncResult.type;
			switch( type )
			{
				case ReadValueResult.Eof:
				{
					// <Fail resultVariable="result" typeName="Byte?" isAsync="True">
					return AsyncReadResult.Fail<Byte?>();
					// </Fail>
				}
				case ReadValueResult.Nil:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="default( Byte? )" isAsync="True">
					return AsyncReadResult.Success( default( Byte? ) );
					// </Success>
				}
				// <WriteReadScalarCore type="System.Byte" isNullable="True" valueVariable="i: integral, r4: real32, r8: real64, a: asyncResult" resultVariable="result" isAsync="True">
				case ReadValueResult.Byte:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="Byte?" resultVariable="result" expression="unchecked( ( Byte )asyncResult.integral )" isAsync="True">
					return AsyncReadResult.Success<Byte?>( unchecked( ( Byte )asyncResult.integral ) );
					// </Success>
				}
				case ReadValueResult.SByte:
				case ReadValueResult.Int16:
				case ReadValueResult.Int32:
				case ReadValueResult.Int64:
				case ReadValueResult.UInt16:
				case ReadValueResult.UInt32:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="Byte?" resultVariable="result" expression="checked( ( Byte )asyncResult.integral )" isAsync="True">
					return AsyncReadResult.Success<Byte?>( checked( ( Byte )asyncResult.integral ) );
					// </Success>
				}
				case ReadValueResult.UInt64:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="Byte?" resultVariable="result" expression="checked( ( Byte )( UInt64 )asyncResult.integral )" isAsync="True">
					return AsyncReadResult.Success<Byte?>( checked( ( Byte )( UInt64 )asyncResult.integral ) );
					// </Success>
				}
				case ReadValueResult.Single:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="Byte?" resultVariable="result" expression="checked( ( Byte )asyncResult.real32 )" isAsync="True">
					return AsyncReadResult.Success<Byte?>( checked( ( Byte )asyncResult.real32 ) );
					// </Success>
				}
				case ReadValueResult.Double:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="Byte?" resultVariable="result" expression="checked( ( Byte )asyncResult.real64 )" isAsync="True">
					return AsyncReadResult.Success<Byte?>( checked( ( Byte )asyncResult.real64 ) );
					// </Success>
				}
				// </WriteReadScalarCore>
				default:
				{
					this.ThrowTypeException( typeof( Byte? ), asyncResult.header );
					// Never reach
					// <Fail resultVariable="result" typeName="Byte?" isAsync="True">
					return AsyncReadResult.Fail<Byte?>();
					// </Fail>
				}
			}
			// </WriteReadBodyCore>
		} 

#endif // FEATURE_TAP

		// </WriteReadBody>
		// <WriteReadBody typeName="SByte" isAsync="False">
		public override bool ReadSByte( out SByte result )
		{
			// <WriteReadBodyCore typeName="SByte" nullability="Value" isAsync="False">
			byte header;
			long integral;
			float real32;
			double real64;
			var type = this.ReadValue( out header, out integral, out real32, out real64 );
			switch( type )
			{
				case ReadValueResult.Eof:
				{
					// <Fail resultVariable="result" typeName="SByte" isAsync="False">
					result = default( SByte );
					return false;
					// </Fail>
				}
				// <WriteReadScalarCore type="System.SByte" isNullable="False" valueVariable="i: integral, r4: real32, r8: real64, a: asyncResult" resultVariable="result" isAsync="False">
				case ReadValueResult.SByte:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="unchecked( ( SByte )integral )" isAsync="False">
					result = unchecked( ( SByte )integral );
					return true;
					// </Success>
				}
				case ReadValueResult.Int16:
				case ReadValueResult.Int32:
				case ReadValueResult.Int64:
				case ReadValueResult.Byte:
				case ReadValueResult.UInt16:
				case ReadValueResult.UInt32:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="checked( ( SByte )integral )" isAsync="False">
					result = checked( ( SByte )integral );
					return true;
					// </Success>
				}
				case ReadValueResult.UInt64:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="checked( ( SByte )( UInt64 )integral )" isAsync="False">
					result = checked( ( SByte )( UInt64 )integral );
					return true;
					// </Success>
				}
				case ReadValueResult.Single:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="checked( ( SByte )real32 )" isAsync="False">
					result = checked( ( SByte )real32 );
					return true;
					// </Success>
				}
				case ReadValueResult.Double:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="checked( ( SByte )real64 )" isAsync="False">
					result = checked( ( SByte )real64 );
					return true;
					// </Success>
				}
				// </WriteReadScalarCore>
				default:
				{
					this.ThrowTypeException( typeof( SByte ), header );
					// Never reach
					// <Fail resultVariable="result" typeName="SByte" isAsync="False">
					result = default( SByte );
					return false;
					// </Fail>
				}
			}
			// </WriteReadBodyCore>
		} 
		// </WriteReadBody>
		// <WriteReadBody typeName="SByte" isAsync="True">

#if FEATURE_TAP

		public async override Task<AsyncReadResult<SByte>> ReadSByteAsync( CancellationToken cancellationToken )
		{
			// <WriteReadBodyCore typeName="SByte" nullability="Value" isAsync="True">
			var asyncResult = await this.ReadValueAsync( cancellationToken ).ConfigureAwait( false );
			var type = asyncResult.type;
			switch( type )
			{
				case ReadValueResult.Eof:
				{
					// <Fail resultVariable="result" typeName="SByte" isAsync="True">
					return AsyncReadResult.Fail<SByte>();
					// </Fail>
				}
				// <WriteReadScalarCore type="System.SByte" isNullable="False" valueVariable="i: integral, r4: real32, r8: real64, a: asyncResult" resultVariable="result" isAsync="True">
				case ReadValueResult.SByte:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="unchecked( ( SByte )asyncResult.integral )" isAsync="True">
					return AsyncReadResult.Success( unchecked( ( SByte )asyncResult.integral ) );
					// </Success>
				}
				case ReadValueResult.Int16:
				case ReadValueResult.Int32:
				case ReadValueResult.Int64:
				case ReadValueResult.Byte:
				case ReadValueResult.UInt16:
				case ReadValueResult.UInt32:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="checked( ( SByte )asyncResult.integral )" isAsync="True">
					return AsyncReadResult.Success( checked( ( SByte )asyncResult.integral ) );
					// </Success>
				}
				case ReadValueResult.UInt64:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="checked( ( SByte )( UInt64 )asyncResult.integral )" isAsync="True">
					return AsyncReadResult.Success( checked( ( SByte )( UInt64 )asyncResult.integral ) );
					// </Success>
				}
				case ReadValueResult.Single:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="checked( ( SByte )asyncResult.real32 )" isAsync="True">
					return AsyncReadResult.Success( checked( ( SByte )asyncResult.real32 ) );
					// </Success>
				}
				case ReadValueResult.Double:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="checked( ( SByte )asyncResult.real64 )" isAsync="True">
					return AsyncReadResult.Success( checked( ( SByte )asyncResult.real64 ) );
					// </Success>
				}
				// </WriteReadScalarCore>
				default:
				{
					this.ThrowTypeException( typeof( SByte ), asyncResult.header );
					// Never reach
					// <Fail resultVariable="result" typeName="SByte" isAsync="True">
					return AsyncReadResult.Fail<SByte>();
					// </Fail>
				}
			}
			// </WriteReadBodyCore>
		} 

#endif // FEATURE_TAP

		// </WriteReadBody>
		// <WriteReadBody typeName="NullableSByte" isAsync="False">
		public override bool ReadNullableSByte( out SByte? result )
		{
			// <WriteReadBodyCore typeName="SByte" nullability="Nullable" isAsync="False">
			byte header;
			long integral;
			float real32;
			double real64;
			var type = this.ReadValue( out header, out integral, out real32, out real64 );
			switch( type )
			{
				case ReadValueResult.Eof:
				{
					// <Fail resultVariable="result" typeName="SByte?" isAsync="False">
					result = default( SByte? );
					return false;
					// </Fail>
				}
				case ReadValueResult.Nil:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="default( SByte? )" isAsync="False">
					result = default( SByte? );
					return true;
					// </Success>
				}
				// <WriteReadScalarCore type="System.SByte" isNullable="True" valueVariable="i: integral, r4: real32, r8: real64, a: asyncResult" resultVariable="result" isAsync="False">
				case ReadValueResult.SByte:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="SByte?" resultVariable="result" expression="unchecked( ( SByte )integral )" isAsync="False">
					result = unchecked( ( SByte )integral );
					return true;
					// </Success>
				}
				case ReadValueResult.Int16:
				case ReadValueResult.Int32:
				case ReadValueResult.Int64:
				case ReadValueResult.Byte:
				case ReadValueResult.UInt16:
				case ReadValueResult.UInt32:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="SByte?" resultVariable="result" expression="checked( ( SByte )integral )" isAsync="False">
					result = checked( ( SByte )integral );
					return true;
					// </Success>
				}
				case ReadValueResult.UInt64:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="SByte?" resultVariable="result" expression="checked( ( SByte )( UInt64 )integral )" isAsync="False">
					result = checked( ( SByte )( UInt64 )integral );
					return true;
					// </Success>
				}
				case ReadValueResult.Single:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="SByte?" resultVariable="result" expression="checked( ( SByte )real32 )" isAsync="False">
					result = checked( ( SByte )real32 );
					return true;
					// </Success>
				}
				case ReadValueResult.Double:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="SByte?" resultVariable="result" expression="checked( ( SByte )real64 )" isAsync="False">
					result = checked( ( SByte )real64 );
					return true;
					// </Success>
				}
				// </WriteReadScalarCore>
				default:
				{
					this.ThrowTypeException( typeof( SByte? ), header );
					// Never reach
					// <Fail resultVariable="result" typeName="SByte?" isAsync="False">
					result = default( SByte? );
					return false;
					// </Fail>
				}
			}
			// </WriteReadBodyCore>
		} 
		// </WriteReadBody>
		// <WriteReadBody typeName="NullableSByte" isAsync="True">

#if FEATURE_TAP

		public async override Task<AsyncReadResult<SByte?>> ReadNullableSByteAsync( CancellationToken cancellationToken )
		{
			// <WriteReadBodyCore typeName="SByte" nullability="Nullable" isAsync="True">
			var asyncResult = await this.ReadValueAsync( cancellationToken ).ConfigureAwait( false );
			var type = asyncResult.type;
			switch( type )
			{
				case ReadValueResult.Eof:
				{
					// <Fail resultVariable="result" typeName="SByte?" isAsync="True">
					return AsyncReadResult.Fail<SByte?>();
					// </Fail>
				}
				case ReadValueResult.Nil:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="default( SByte? )" isAsync="True">
					return AsyncReadResult.Success( default( SByte? ) );
					// </Success>
				}
				// <WriteReadScalarCore type="System.SByte" isNullable="True" valueVariable="i: integral, r4: real32, r8: real64, a: asyncResult" resultVariable="result" isAsync="True">
				case ReadValueResult.SByte:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="SByte?" resultVariable="result" expression="unchecked( ( SByte )asyncResult.integral )" isAsync="True">
					return AsyncReadResult.Success<SByte?>( unchecked( ( SByte )asyncResult.integral ) );
					// </Success>
				}
				case ReadValueResult.Int16:
				case ReadValueResult.Int32:
				case ReadValueResult.Int64:
				case ReadValueResult.Byte:
				case ReadValueResult.UInt16:
				case ReadValueResult.UInt32:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="SByte?" resultVariable="result" expression="checked( ( SByte )asyncResult.integral )" isAsync="True">
					return AsyncReadResult.Success<SByte?>( checked( ( SByte )asyncResult.integral ) );
					// </Success>
				}
				case ReadValueResult.UInt64:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="SByte?" resultVariable="result" expression="checked( ( SByte )( UInt64 )asyncResult.integral )" isAsync="True">
					return AsyncReadResult.Success<SByte?>( checked( ( SByte )( UInt64 )asyncResult.integral ) );
					// </Success>
				}
				case ReadValueResult.Single:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="SByte?" resultVariable="result" expression="checked( ( SByte )asyncResult.real32 )" isAsync="True">
					return AsyncReadResult.Success<SByte?>( checked( ( SByte )asyncResult.real32 ) );
					// </Success>
				}
				case ReadValueResult.Double:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="SByte?" resultVariable="result" expression="checked( ( SByte )asyncResult.real64 )" isAsync="True">
					return AsyncReadResult.Success<SByte?>( checked( ( SByte )asyncResult.real64 ) );
					// </Success>
				}
				// </WriteReadScalarCore>
				default:
				{
					this.ThrowTypeException( typeof( SByte? ), asyncResult.header );
					// Never reach
					// <Fail resultVariable="result" typeName="SByte?" isAsync="True">
					return AsyncReadResult.Fail<SByte?>();
					// </Fail>
				}
			}
			// </WriteReadBodyCore>
		} 

#endif // FEATURE_TAP

		// </WriteReadBody>
		// <WriteReadBody typeName="Int16" isAsync="False">
		public override bool ReadInt16( out Int16 result )
		{
			// <WriteReadBodyCore typeName="Int16" nullability="Value" isAsync="False">
			byte header;
			long integral;
			float real32;
			double real64;
			var type = this.ReadValue( out header, out integral, out real32, out real64 );
			switch( type )
			{
				case ReadValueResult.Eof:
				{
					// <Fail resultVariable="result" typeName="Int16" isAsync="False">
					result = default( Int16 );
					return false;
					// </Fail>
				}
				// <WriteReadScalarCore type="System.Int16" isNullable="False" valueVariable="i: integral, r4: real32, r8: real64, a: asyncResult" resultVariable="result" isAsync="False">
				case ReadValueResult.Int16:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="unchecked( ( Int16 )integral )" isAsync="False">
					result = unchecked( ( Int16 )integral );
					return true;
					// </Success>
				}
				case ReadValueResult.SByte:
				case ReadValueResult.Int32:
				case ReadValueResult.Int64:
				case ReadValueResult.Byte:
				case ReadValueResult.UInt16:
				case ReadValueResult.UInt32:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="checked( ( Int16 )integral )" isAsync="False">
					result = checked( ( Int16 )integral );
					return true;
					// </Success>
				}
				case ReadValueResult.UInt64:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="checked( ( Int16 )( UInt64 )integral )" isAsync="False">
					result = checked( ( Int16 )( UInt64 )integral );
					return true;
					// </Success>
				}
				case ReadValueResult.Single:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="checked( ( Int16 )real32 )" isAsync="False">
					result = checked( ( Int16 )real32 );
					return true;
					// </Success>
				}
				case ReadValueResult.Double:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="checked( ( Int16 )real64 )" isAsync="False">
					result = checked( ( Int16 )real64 );
					return true;
					// </Success>
				}
				// </WriteReadScalarCore>
				default:
				{
					this.ThrowTypeException( typeof( Int16 ), header );
					// Never reach
					// <Fail resultVariable="result" typeName="Int16" isAsync="False">
					result = default( Int16 );
					return false;
					// </Fail>
				}
			}
			// </WriteReadBodyCore>
		} 
		// </WriteReadBody>
		// <WriteReadBody typeName="Int16" isAsync="True">

#if FEATURE_TAP

		public async override Task<AsyncReadResult<Int16>> ReadInt16Async( CancellationToken cancellationToken )
		{
			// <WriteReadBodyCore typeName="Int16" nullability="Value" isAsync="True">
			var asyncResult = await this.ReadValueAsync( cancellationToken ).ConfigureAwait( false );
			var type = asyncResult.type;
			switch( type )
			{
				case ReadValueResult.Eof:
				{
					// <Fail resultVariable="result" typeName="Int16" isAsync="True">
					return AsyncReadResult.Fail<Int16>();
					// </Fail>
				}
				// <WriteReadScalarCore type="System.Int16" isNullable="False" valueVariable="i: integral, r4: real32, r8: real64, a: asyncResult" resultVariable="result" isAsync="True">
				case ReadValueResult.Int16:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="unchecked( ( Int16 )asyncResult.integral )" isAsync="True">
					return AsyncReadResult.Success( unchecked( ( Int16 )asyncResult.integral ) );
					// </Success>
				}
				case ReadValueResult.SByte:
				case ReadValueResult.Int32:
				case ReadValueResult.Int64:
				case ReadValueResult.Byte:
				case ReadValueResult.UInt16:
				case ReadValueResult.UInt32:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="checked( ( Int16 )asyncResult.integral )" isAsync="True">
					return AsyncReadResult.Success( checked( ( Int16 )asyncResult.integral ) );
					// </Success>
				}
				case ReadValueResult.UInt64:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="checked( ( Int16 )( UInt64 )asyncResult.integral )" isAsync="True">
					return AsyncReadResult.Success( checked( ( Int16 )( UInt64 )asyncResult.integral ) );
					// </Success>
				}
				case ReadValueResult.Single:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="checked( ( Int16 )asyncResult.real32 )" isAsync="True">
					return AsyncReadResult.Success( checked( ( Int16 )asyncResult.real32 ) );
					// </Success>
				}
				case ReadValueResult.Double:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="checked( ( Int16 )asyncResult.real64 )" isAsync="True">
					return AsyncReadResult.Success( checked( ( Int16 )asyncResult.real64 ) );
					// </Success>
				}
				// </WriteReadScalarCore>
				default:
				{
					this.ThrowTypeException( typeof( Int16 ), asyncResult.header );
					// Never reach
					// <Fail resultVariable="result" typeName="Int16" isAsync="True">
					return AsyncReadResult.Fail<Int16>();
					// </Fail>
				}
			}
			// </WriteReadBodyCore>
		} 

#endif // FEATURE_TAP

		// </WriteReadBody>
		// <WriteReadBody typeName="NullableInt16" isAsync="False">
		public override bool ReadNullableInt16( out Int16? result )
		{
			// <WriteReadBodyCore typeName="Int16" nullability="Nullable" isAsync="False">
			byte header;
			long integral;
			float real32;
			double real64;
			var type = this.ReadValue( out header, out integral, out real32, out real64 );
			switch( type )
			{
				case ReadValueResult.Eof:
				{
					// <Fail resultVariable="result" typeName="Int16?" isAsync="False">
					result = default( Int16? );
					return false;
					// </Fail>
				}
				case ReadValueResult.Nil:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="default( Int16? )" isAsync="False">
					result = default( Int16? );
					return true;
					// </Success>
				}
				// <WriteReadScalarCore type="System.Int16" isNullable="True" valueVariable="i: integral, r4: real32, r8: real64, a: asyncResult" resultVariable="result" isAsync="False">
				case ReadValueResult.Int16:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="Int16?" resultVariable="result" expression="unchecked( ( Int16 )integral )" isAsync="False">
					result = unchecked( ( Int16 )integral );
					return true;
					// </Success>
				}
				case ReadValueResult.SByte:
				case ReadValueResult.Int32:
				case ReadValueResult.Int64:
				case ReadValueResult.Byte:
				case ReadValueResult.UInt16:
				case ReadValueResult.UInt32:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="Int16?" resultVariable="result" expression="checked( ( Int16 )integral )" isAsync="False">
					result = checked( ( Int16 )integral );
					return true;
					// </Success>
				}
				case ReadValueResult.UInt64:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="Int16?" resultVariable="result" expression="checked( ( Int16 )( UInt64 )integral )" isAsync="False">
					result = checked( ( Int16 )( UInt64 )integral );
					return true;
					// </Success>
				}
				case ReadValueResult.Single:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="Int16?" resultVariable="result" expression="checked( ( Int16 )real32 )" isAsync="False">
					result = checked( ( Int16 )real32 );
					return true;
					// </Success>
				}
				case ReadValueResult.Double:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="Int16?" resultVariable="result" expression="checked( ( Int16 )real64 )" isAsync="False">
					result = checked( ( Int16 )real64 );
					return true;
					// </Success>
				}
				// </WriteReadScalarCore>
				default:
				{
					this.ThrowTypeException( typeof( Int16? ), header );
					// Never reach
					// <Fail resultVariable="result" typeName="Int16?" isAsync="False">
					result = default( Int16? );
					return false;
					// </Fail>
				}
			}
			// </WriteReadBodyCore>
		} 
		// </WriteReadBody>
		// <WriteReadBody typeName="NullableInt16" isAsync="True">

#if FEATURE_TAP

		public async override Task<AsyncReadResult<Int16?>> ReadNullableInt16Async( CancellationToken cancellationToken )
		{
			// <WriteReadBodyCore typeName="Int16" nullability="Nullable" isAsync="True">
			var asyncResult = await this.ReadValueAsync( cancellationToken ).ConfigureAwait( false );
			var type = asyncResult.type;
			switch( type )
			{
				case ReadValueResult.Eof:
				{
					// <Fail resultVariable="result" typeName="Int16?" isAsync="True">
					return AsyncReadResult.Fail<Int16?>();
					// </Fail>
				}
				case ReadValueResult.Nil:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="default( Int16? )" isAsync="True">
					return AsyncReadResult.Success( default( Int16? ) );
					// </Success>
				}
				// <WriteReadScalarCore type="System.Int16" isNullable="True" valueVariable="i: integral, r4: real32, r8: real64, a: asyncResult" resultVariable="result" isAsync="True">
				case ReadValueResult.Int16:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="Int16?" resultVariable="result" expression="unchecked( ( Int16 )asyncResult.integral )" isAsync="True">
					return AsyncReadResult.Success<Int16?>( unchecked( ( Int16 )asyncResult.integral ) );
					// </Success>
				}
				case ReadValueResult.SByte:
				case ReadValueResult.Int32:
				case ReadValueResult.Int64:
				case ReadValueResult.Byte:
				case ReadValueResult.UInt16:
				case ReadValueResult.UInt32:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="Int16?" resultVariable="result" expression="checked( ( Int16 )asyncResult.integral )" isAsync="True">
					return AsyncReadResult.Success<Int16?>( checked( ( Int16 )asyncResult.integral ) );
					// </Success>
				}
				case ReadValueResult.UInt64:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="Int16?" resultVariable="result" expression="checked( ( Int16 )( UInt64 )asyncResult.integral )" isAsync="True">
					return AsyncReadResult.Success<Int16?>( checked( ( Int16 )( UInt64 )asyncResult.integral ) );
					// </Success>
				}
				case ReadValueResult.Single:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="Int16?" resultVariable="result" expression="checked( ( Int16 )asyncResult.real32 )" isAsync="True">
					return AsyncReadResult.Success<Int16?>( checked( ( Int16 )asyncResult.real32 ) );
					// </Success>
				}
				case ReadValueResult.Double:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="Int16?" resultVariable="result" expression="checked( ( Int16 )asyncResult.real64 )" isAsync="True">
					return AsyncReadResult.Success<Int16?>( checked( ( Int16 )asyncResult.real64 ) );
					// </Success>
				}
				// </WriteReadScalarCore>
				default:
				{
					this.ThrowTypeException( typeof( Int16? ), asyncResult.header );
					// Never reach
					// <Fail resultVariable="result" typeName="Int16?" isAsync="True">
					return AsyncReadResult.Fail<Int16?>();
					// </Fail>
				}
			}
			// </WriteReadBodyCore>
		} 

#endif // FEATURE_TAP

		// </WriteReadBody>
		// <WriteReadBody typeName="UInt16" isAsync="False">
		public override bool ReadUInt16( out UInt16 result )
		{
			// <WriteReadBodyCore typeName="UInt16" nullability="Value" isAsync="False">
			byte header;
			long integral;
			float real32;
			double real64;
			var type = this.ReadValue( out header, out integral, out real32, out real64 );
			switch( type )
			{
				case ReadValueResult.Eof:
				{
					// <Fail resultVariable="result" typeName="UInt16" isAsync="False">
					result = default( UInt16 );
					return false;
					// </Fail>
				}
				// <WriteReadScalarCore type="System.UInt16" isNullable="False" valueVariable="i: integral, r4: real32, r8: real64, a: asyncResult" resultVariable="result" isAsync="False">
				case ReadValueResult.UInt16:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="unchecked( ( UInt16 )integral )" isAsync="False">
					result = unchecked( ( UInt16 )integral );
					return true;
					// </Success>
				}
				case ReadValueResult.SByte:
				case ReadValueResult.Int16:
				case ReadValueResult.Int32:
				case ReadValueResult.Int64:
				case ReadValueResult.Byte:
				case ReadValueResult.UInt32:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="checked( ( UInt16 )integral )" isAsync="False">
					result = checked( ( UInt16 )integral );
					return true;
					// </Success>
				}
				case ReadValueResult.UInt64:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="checked( ( UInt16 )( UInt64 )integral )" isAsync="False">
					result = checked( ( UInt16 )( UInt64 )integral );
					return true;
					// </Success>
				}
				case ReadValueResult.Single:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="checked( ( UInt16 )real32 )" isAsync="False">
					result = checked( ( UInt16 )real32 );
					return true;
					// </Success>
				}
				case ReadValueResult.Double:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="checked( ( UInt16 )real64 )" isAsync="False">
					result = checked( ( UInt16 )real64 );
					return true;
					// </Success>
				}
				// </WriteReadScalarCore>
				default:
				{
					this.ThrowTypeException( typeof( UInt16 ), header );
					// Never reach
					// <Fail resultVariable="result" typeName="UInt16" isAsync="False">
					result = default( UInt16 );
					return false;
					// </Fail>
				}
			}
			// </WriteReadBodyCore>
		} 
		// </WriteReadBody>
		// <WriteReadBody typeName="UInt16" isAsync="True">

#if FEATURE_TAP

		public async override Task<AsyncReadResult<UInt16>> ReadUInt16Async( CancellationToken cancellationToken )
		{
			// <WriteReadBodyCore typeName="UInt16" nullability="Value" isAsync="True">
			var asyncResult = await this.ReadValueAsync( cancellationToken ).ConfigureAwait( false );
			var type = asyncResult.type;
			switch( type )
			{
				case ReadValueResult.Eof:
				{
					// <Fail resultVariable="result" typeName="UInt16" isAsync="True">
					return AsyncReadResult.Fail<UInt16>();
					// </Fail>
				}
				// <WriteReadScalarCore type="System.UInt16" isNullable="False" valueVariable="i: integral, r4: real32, r8: real64, a: asyncResult" resultVariable="result" isAsync="True">
				case ReadValueResult.UInt16:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="unchecked( ( UInt16 )asyncResult.integral )" isAsync="True">
					return AsyncReadResult.Success( unchecked( ( UInt16 )asyncResult.integral ) );
					// </Success>
				}
				case ReadValueResult.SByte:
				case ReadValueResult.Int16:
				case ReadValueResult.Int32:
				case ReadValueResult.Int64:
				case ReadValueResult.Byte:
				case ReadValueResult.UInt32:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="checked( ( UInt16 )asyncResult.integral )" isAsync="True">
					return AsyncReadResult.Success( checked( ( UInt16 )asyncResult.integral ) );
					// </Success>
				}
				case ReadValueResult.UInt64:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="checked( ( UInt16 )( UInt64 )asyncResult.integral )" isAsync="True">
					return AsyncReadResult.Success( checked( ( UInt16 )( UInt64 )asyncResult.integral ) );
					// </Success>
				}
				case ReadValueResult.Single:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="checked( ( UInt16 )asyncResult.real32 )" isAsync="True">
					return AsyncReadResult.Success( checked( ( UInt16 )asyncResult.real32 ) );
					// </Success>
				}
				case ReadValueResult.Double:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="checked( ( UInt16 )asyncResult.real64 )" isAsync="True">
					return AsyncReadResult.Success( checked( ( UInt16 )asyncResult.real64 ) );
					// </Success>
				}
				// </WriteReadScalarCore>
				default:
				{
					this.ThrowTypeException( typeof( UInt16 ), asyncResult.header );
					// Never reach
					// <Fail resultVariable="result" typeName="UInt16" isAsync="True">
					return AsyncReadResult.Fail<UInt16>();
					// </Fail>
				}
			}
			// </WriteReadBodyCore>
		} 

#endif // FEATURE_TAP

		// </WriteReadBody>
		// <WriteReadBody typeName="NullableUInt16" isAsync="False">
		public override bool ReadNullableUInt16( out UInt16? result )
		{
			// <WriteReadBodyCore typeName="UInt16" nullability="Nullable" isAsync="False">
			byte header;
			long integral;
			float real32;
			double real64;
			var type = this.ReadValue( out header, out integral, out real32, out real64 );
			switch( type )
			{
				case ReadValueResult.Eof:
				{
					// <Fail resultVariable="result" typeName="UInt16?" isAsync="False">
					result = default( UInt16? );
					return false;
					// </Fail>
				}
				case ReadValueResult.Nil:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="default( UInt16? )" isAsync="False">
					result = default( UInt16? );
					return true;
					// </Success>
				}
				// <WriteReadScalarCore type="System.UInt16" isNullable="True" valueVariable="i: integral, r4: real32, r8: real64, a: asyncResult" resultVariable="result" isAsync="False">
				case ReadValueResult.UInt16:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="UInt16?" resultVariable="result" expression="unchecked( ( UInt16 )integral )" isAsync="False">
					result = unchecked( ( UInt16 )integral );
					return true;
					// </Success>
				}
				case ReadValueResult.SByte:
				case ReadValueResult.Int16:
				case ReadValueResult.Int32:
				case ReadValueResult.Int64:
				case ReadValueResult.Byte:
				case ReadValueResult.UInt32:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="UInt16?" resultVariable="result" expression="checked( ( UInt16 )integral )" isAsync="False">
					result = checked( ( UInt16 )integral );
					return true;
					// </Success>
				}
				case ReadValueResult.UInt64:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="UInt16?" resultVariable="result" expression="checked( ( UInt16 )( UInt64 )integral )" isAsync="False">
					result = checked( ( UInt16 )( UInt64 )integral );
					return true;
					// </Success>
				}
				case ReadValueResult.Single:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="UInt16?" resultVariable="result" expression="checked( ( UInt16 )real32 )" isAsync="False">
					result = checked( ( UInt16 )real32 );
					return true;
					// </Success>
				}
				case ReadValueResult.Double:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="UInt16?" resultVariable="result" expression="checked( ( UInt16 )real64 )" isAsync="False">
					result = checked( ( UInt16 )real64 );
					return true;
					// </Success>
				}
				// </WriteReadScalarCore>
				default:
				{
					this.ThrowTypeException( typeof( UInt16? ), header );
					// Never reach
					// <Fail resultVariable="result" typeName="UInt16?" isAsync="False">
					result = default( UInt16? );
					return false;
					// </Fail>
				}
			}
			// </WriteReadBodyCore>
		} 
		// </WriteReadBody>
		// <WriteReadBody typeName="NullableUInt16" isAsync="True">

#if FEATURE_TAP

		public async override Task<AsyncReadResult<UInt16?>> ReadNullableUInt16Async( CancellationToken cancellationToken )
		{
			// <WriteReadBodyCore typeName="UInt16" nullability="Nullable" isAsync="True">
			var asyncResult = await this.ReadValueAsync( cancellationToken ).ConfigureAwait( false );
			var type = asyncResult.type;
			switch( type )
			{
				case ReadValueResult.Eof:
				{
					// <Fail resultVariable="result" typeName="UInt16?" isAsync="True">
					return AsyncReadResult.Fail<UInt16?>();
					// </Fail>
				}
				case ReadValueResult.Nil:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="default( UInt16? )" isAsync="True">
					return AsyncReadResult.Success( default( UInt16? ) );
					// </Success>
				}
				// <WriteReadScalarCore type="System.UInt16" isNullable="True" valueVariable="i: integral, r4: real32, r8: real64, a: asyncResult" resultVariable="result" isAsync="True">
				case ReadValueResult.UInt16:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="UInt16?" resultVariable="result" expression="unchecked( ( UInt16 )asyncResult.integral )" isAsync="True">
					return AsyncReadResult.Success<UInt16?>( unchecked( ( UInt16 )asyncResult.integral ) );
					// </Success>
				}
				case ReadValueResult.SByte:
				case ReadValueResult.Int16:
				case ReadValueResult.Int32:
				case ReadValueResult.Int64:
				case ReadValueResult.Byte:
				case ReadValueResult.UInt32:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="UInt16?" resultVariable="result" expression="checked( ( UInt16 )asyncResult.integral )" isAsync="True">
					return AsyncReadResult.Success<UInt16?>( checked( ( UInt16 )asyncResult.integral ) );
					// </Success>
				}
				case ReadValueResult.UInt64:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="UInt16?" resultVariable="result" expression="checked( ( UInt16 )( UInt64 )asyncResult.integral )" isAsync="True">
					return AsyncReadResult.Success<UInt16?>( checked( ( UInt16 )( UInt64 )asyncResult.integral ) );
					// </Success>
				}
				case ReadValueResult.Single:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="UInt16?" resultVariable="result" expression="checked( ( UInt16 )asyncResult.real32 )" isAsync="True">
					return AsyncReadResult.Success<UInt16?>( checked( ( UInt16 )asyncResult.real32 ) );
					// </Success>
				}
				case ReadValueResult.Double:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="UInt16?" resultVariable="result" expression="checked( ( UInt16 )asyncResult.real64 )" isAsync="True">
					return AsyncReadResult.Success<UInt16?>( checked( ( UInt16 )asyncResult.real64 ) );
					// </Success>
				}
				// </WriteReadScalarCore>
				default:
				{
					this.ThrowTypeException( typeof( UInt16? ), asyncResult.header );
					// Never reach
					// <Fail resultVariable="result" typeName="UInt16?" isAsync="True">
					return AsyncReadResult.Fail<UInt16?>();
					// </Fail>
				}
			}
			// </WriteReadBodyCore>
		} 

#endif // FEATURE_TAP

		// </WriteReadBody>
		// <WriteReadBody typeName="Int32" isAsync="False">
		public override bool ReadInt32( out Int32 result )
		{
			// <WriteReadBodyCore typeName="Int32" nullability="Value" isAsync="False">
			byte header;
			long integral;
			float real32;
			double real64;
			var type = this.ReadValue( out header, out integral, out real32, out real64 );
			switch( type )
			{
				case ReadValueResult.Eof:
				{
					// <Fail resultVariable="result" typeName="Int32" isAsync="False">
					result = default( Int32 );
					return false;
					// </Fail>
				}
				// <WriteReadScalarCore type="System.Int32" isNullable="False" valueVariable="i: integral, r4: real32, r8: real64, a: asyncResult" resultVariable="result" isAsync="False">
				case ReadValueResult.Int32:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="unchecked( ( Int32 )integral )" isAsync="False">
					result = unchecked( ( Int32 )integral );
					return true;
					// </Success>
				}
				case ReadValueResult.SByte:
				case ReadValueResult.Int16:
				case ReadValueResult.Int64:
				case ReadValueResult.Byte:
				case ReadValueResult.UInt16:
				case ReadValueResult.UInt32:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="checked( ( Int32 )integral )" isAsync="False">
					result = checked( ( Int32 )integral );
					return true;
					// </Success>
				}
				case ReadValueResult.UInt64:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="checked( ( Int32 )( UInt64 )integral )" isAsync="False">
					result = checked( ( Int32 )( UInt64 )integral );
					return true;
					// </Success>
				}
				case ReadValueResult.Single:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="checked( ( Int32 )real32 )" isAsync="False">
					result = checked( ( Int32 )real32 );
					return true;
					// </Success>
				}
				case ReadValueResult.Double:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="checked( ( Int32 )real64 )" isAsync="False">
					result = checked( ( Int32 )real64 );
					return true;
					// </Success>
				}
				// </WriteReadScalarCore>
				default:
				{
					this.ThrowTypeException( typeof( Int32 ), header );
					// Never reach
					// <Fail resultVariable="result" typeName="Int32" isAsync="False">
					result = default( Int32 );
					return false;
					// </Fail>
				}
			}
			// </WriteReadBodyCore>
		} 
		// </WriteReadBody>
		// <WriteReadBody typeName="Int32" isAsync="True">

#if FEATURE_TAP

		public async override Task<AsyncReadResult<Int32>> ReadInt32Async( CancellationToken cancellationToken )
		{
			// <WriteReadBodyCore typeName="Int32" nullability="Value" isAsync="True">
			var asyncResult = await this.ReadValueAsync( cancellationToken ).ConfigureAwait( false );
			var type = asyncResult.type;
			switch( type )
			{
				case ReadValueResult.Eof:
				{
					// <Fail resultVariable="result" typeName="Int32" isAsync="True">
					return AsyncReadResult.Fail<Int32>();
					// </Fail>
				}
				// <WriteReadScalarCore type="System.Int32" isNullable="False" valueVariable="i: integral, r4: real32, r8: real64, a: asyncResult" resultVariable="result" isAsync="True">
				case ReadValueResult.Int32:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="unchecked( ( Int32 )asyncResult.integral )" isAsync="True">
					return AsyncReadResult.Success( unchecked( ( Int32 )asyncResult.integral ) );
					// </Success>
				}
				case ReadValueResult.SByte:
				case ReadValueResult.Int16:
				case ReadValueResult.Int64:
				case ReadValueResult.Byte:
				case ReadValueResult.UInt16:
				case ReadValueResult.UInt32:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="checked( ( Int32 )asyncResult.integral )" isAsync="True">
					return AsyncReadResult.Success( checked( ( Int32 )asyncResult.integral ) );
					// </Success>
				}
				case ReadValueResult.UInt64:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="checked( ( Int32 )( UInt64 )asyncResult.integral )" isAsync="True">
					return AsyncReadResult.Success( checked( ( Int32 )( UInt64 )asyncResult.integral ) );
					// </Success>
				}
				case ReadValueResult.Single:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="checked( ( Int32 )asyncResult.real32 )" isAsync="True">
					return AsyncReadResult.Success( checked( ( Int32 )asyncResult.real32 ) );
					// </Success>
				}
				case ReadValueResult.Double:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="checked( ( Int32 )asyncResult.real64 )" isAsync="True">
					return AsyncReadResult.Success( checked( ( Int32 )asyncResult.real64 ) );
					// </Success>
				}
				// </WriteReadScalarCore>
				default:
				{
					this.ThrowTypeException( typeof( Int32 ), asyncResult.header );
					// Never reach
					// <Fail resultVariable="result" typeName="Int32" isAsync="True">
					return AsyncReadResult.Fail<Int32>();
					// </Fail>
				}
			}
			// </WriteReadBodyCore>
		} 

#endif // FEATURE_TAP

		// </WriteReadBody>
		// <WriteReadBody typeName="NullableInt32" isAsync="False">
		public override bool ReadNullableInt32( out Int32? result )
		{
			// <WriteReadBodyCore typeName="Int32" nullability="Nullable" isAsync="False">
			byte header;
			long integral;
			float real32;
			double real64;
			var type = this.ReadValue( out header, out integral, out real32, out real64 );
			switch( type )
			{
				case ReadValueResult.Eof:
				{
					// <Fail resultVariable="result" typeName="Int32?" isAsync="False">
					result = default( Int32? );
					return false;
					// </Fail>
				}
				case ReadValueResult.Nil:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="default( Int32? )" isAsync="False">
					result = default( Int32? );
					return true;
					// </Success>
				}
				// <WriteReadScalarCore type="System.Int32" isNullable="True" valueVariable="i: integral, r4: real32, r8: real64, a: asyncResult" resultVariable="result" isAsync="False">
				case ReadValueResult.Int32:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="Int32?" resultVariable="result" expression="unchecked( ( Int32 )integral )" isAsync="False">
					result = unchecked( ( Int32 )integral );
					return true;
					// </Success>
				}
				case ReadValueResult.SByte:
				case ReadValueResult.Int16:
				case ReadValueResult.Int64:
				case ReadValueResult.Byte:
				case ReadValueResult.UInt16:
				case ReadValueResult.UInt32:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="Int32?" resultVariable="result" expression="checked( ( Int32 )integral )" isAsync="False">
					result = checked( ( Int32 )integral );
					return true;
					// </Success>
				}
				case ReadValueResult.UInt64:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="Int32?" resultVariable="result" expression="checked( ( Int32 )( UInt64 )integral )" isAsync="False">
					result = checked( ( Int32 )( UInt64 )integral );
					return true;
					// </Success>
				}
				case ReadValueResult.Single:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="Int32?" resultVariable="result" expression="checked( ( Int32 )real32 )" isAsync="False">
					result = checked( ( Int32 )real32 );
					return true;
					// </Success>
				}
				case ReadValueResult.Double:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="Int32?" resultVariable="result" expression="checked( ( Int32 )real64 )" isAsync="False">
					result = checked( ( Int32 )real64 );
					return true;
					// </Success>
				}
				// </WriteReadScalarCore>
				default:
				{
					this.ThrowTypeException( typeof( Int32? ), header );
					// Never reach
					// <Fail resultVariable="result" typeName="Int32?" isAsync="False">
					result = default( Int32? );
					return false;
					// </Fail>
				}
			}
			// </WriteReadBodyCore>
		} 
		// </WriteReadBody>
		// <WriteReadBody typeName="NullableInt32" isAsync="True">

#if FEATURE_TAP

		public async override Task<AsyncReadResult<Int32?>> ReadNullableInt32Async( CancellationToken cancellationToken )
		{
			// <WriteReadBodyCore typeName="Int32" nullability="Nullable" isAsync="True">
			var asyncResult = await this.ReadValueAsync( cancellationToken ).ConfigureAwait( false );
			var type = asyncResult.type;
			switch( type )
			{
				case ReadValueResult.Eof:
				{
					// <Fail resultVariable="result" typeName="Int32?" isAsync="True">
					return AsyncReadResult.Fail<Int32?>();
					// </Fail>
				}
				case ReadValueResult.Nil:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="default( Int32? )" isAsync="True">
					return AsyncReadResult.Success( default( Int32? ) );
					// </Success>
				}
				// <WriteReadScalarCore type="System.Int32" isNullable="True" valueVariable="i: integral, r4: real32, r8: real64, a: asyncResult" resultVariable="result" isAsync="True">
				case ReadValueResult.Int32:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="Int32?" resultVariable="result" expression="unchecked( ( Int32 )asyncResult.integral )" isAsync="True">
					return AsyncReadResult.Success<Int32?>( unchecked( ( Int32 )asyncResult.integral ) );
					// </Success>
				}
				case ReadValueResult.SByte:
				case ReadValueResult.Int16:
				case ReadValueResult.Int64:
				case ReadValueResult.Byte:
				case ReadValueResult.UInt16:
				case ReadValueResult.UInt32:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="Int32?" resultVariable="result" expression="checked( ( Int32 )asyncResult.integral )" isAsync="True">
					return AsyncReadResult.Success<Int32?>( checked( ( Int32 )asyncResult.integral ) );
					// </Success>
				}
				case ReadValueResult.UInt64:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="Int32?" resultVariable="result" expression="checked( ( Int32 )( UInt64 )asyncResult.integral )" isAsync="True">
					return AsyncReadResult.Success<Int32?>( checked( ( Int32 )( UInt64 )asyncResult.integral ) );
					// </Success>
				}
				case ReadValueResult.Single:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="Int32?" resultVariable="result" expression="checked( ( Int32 )asyncResult.real32 )" isAsync="True">
					return AsyncReadResult.Success<Int32?>( checked( ( Int32 )asyncResult.real32 ) );
					// </Success>
				}
				case ReadValueResult.Double:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="Int32?" resultVariable="result" expression="checked( ( Int32 )asyncResult.real64 )" isAsync="True">
					return AsyncReadResult.Success<Int32?>( checked( ( Int32 )asyncResult.real64 ) );
					// </Success>
				}
				// </WriteReadScalarCore>
				default:
				{
					this.ThrowTypeException( typeof( Int32? ), asyncResult.header );
					// Never reach
					// <Fail resultVariable="result" typeName="Int32?" isAsync="True">
					return AsyncReadResult.Fail<Int32?>();
					// </Fail>
				}
			}
			// </WriteReadBodyCore>
		} 

#endif // FEATURE_TAP

		// </WriteReadBody>
		// <WriteReadBody typeName="UInt32" isAsync="False">
		public override bool ReadUInt32( out UInt32 result )
		{
			// <WriteReadBodyCore typeName="UInt32" nullability="Value" isAsync="False">
			byte header;
			long integral;
			float real32;
			double real64;
			var type = this.ReadValue( out header, out integral, out real32, out real64 );
			switch( type )
			{
				case ReadValueResult.Eof:
				{
					// <Fail resultVariable="result" typeName="UInt32" isAsync="False">
					result = default( UInt32 );
					return false;
					// </Fail>
				}
				// <WriteReadScalarCore type="System.UInt32" isNullable="False" valueVariable="i: integral, r4: real32, r8: real64, a: asyncResult" resultVariable="result" isAsync="False">
				case ReadValueResult.UInt32:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="unchecked( ( UInt32 )integral )" isAsync="False">
					result = unchecked( ( UInt32 )integral );
					return true;
					// </Success>
				}
				case ReadValueResult.SByte:
				case ReadValueResult.Int16:
				case ReadValueResult.Int32:
				case ReadValueResult.Int64:
				case ReadValueResult.Byte:
				case ReadValueResult.UInt16:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="checked( ( UInt32 )integral )" isAsync="False">
					result = checked( ( UInt32 )integral );
					return true;
					// </Success>
				}
				case ReadValueResult.UInt64:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="checked( ( UInt32 )( UInt64 )integral )" isAsync="False">
					result = checked( ( UInt32 )( UInt64 )integral );
					return true;
					// </Success>
				}
				case ReadValueResult.Single:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="checked( ( UInt32 )real32 )" isAsync="False">
					result = checked( ( UInt32 )real32 );
					return true;
					// </Success>
				}
				case ReadValueResult.Double:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="checked( ( UInt32 )real64 )" isAsync="False">
					result = checked( ( UInt32 )real64 );
					return true;
					// </Success>
				}
				// </WriteReadScalarCore>
				default:
				{
					this.ThrowTypeException( typeof( UInt32 ), header );
					// Never reach
					// <Fail resultVariable="result" typeName="UInt32" isAsync="False">
					result = default( UInt32 );
					return false;
					// </Fail>
				}
			}
			// </WriteReadBodyCore>
		} 
		// </WriteReadBody>
		// <WriteReadBody typeName="UInt32" isAsync="True">

#if FEATURE_TAP

		public async override Task<AsyncReadResult<UInt32>> ReadUInt32Async( CancellationToken cancellationToken )
		{
			// <WriteReadBodyCore typeName="UInt32" nullability="Value" isAsync="True">
			var asyncResult = await this.ReadValueAsync( cancellationToken ).ConfigureAwait( false );
			var type = asyncResult.type;
			switch( type )
			{
				case ReadValueResult.Eof:
				{
					// <Fail resultVariable="result" typeName="UInt32" isAsync="True">
					return AsyncReadResult.Fail<UInt32>();
					// </Fail>
				}
				// <WriteReadScalarCore type="System.UInt32" isNullable="False" valueVariable="i: integral, r4: real32, r8: real64, a: asyncResult" resultVariable="result" isAsync="True">
				case ReadValueResult.UInt32:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="unchecked( ( UInt32 )asyncResult.integral )" isAsync="True">
					return AsyncReadResult.Success( unchecked( ( UInt32 )asyncResult.integral ) );
					// </Success>
				}
				case ReadValueResult.SByte:
				case ReadValueResult.Int16:
				case ReadValueResult.Int32:
				case ReadValueResult.Int64:
				case ReadValueResult.Byte:
				case ReadValueResult.UInt16:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="checked( ( UInt32 )asyncResult.integral )" isAsync="True">
					return AsyncReadResult.Success( checked( ( UInt32 )asyncResult.integral ) );
					// </Success>
				}
				case ReadValueResult.UInt64:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="checked( ( UInt32 )( UInt64 )asyncResult.integral )" isAsync="True">
					return AsyncReadResult.Success( checked( ( UInt32 )( UInt64 )asyncResult.integral ) );
					// </Success>
				}
				case ReadValueResult.Single:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="checked( ( UInt32 )asyncResult.real32 )" isAsync="True">
					return AsyncReadResult.Success( checked( ( UInt32 )asyncResult.real32 ) );
					// </Success>
				}
				case ReadValueResult.Double:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="checked( ( UInt32 )asyncResult.real64 )" isAsync="True">
					return AsyncReadResult.Success( checked( ( UInt32 )asyncResult.real64 ) );
					// </Success>
				}
				// </WriteReadScalarCore>
				default:
				{
					this.ThrowTypeException( typeof( UInt32 ), asyncResult.header );
					// Never reach
					// <Fail resultVariable="result" typeName="UInt32" isAsync="True">
					return AsyncReadResult.Fail<UInt32>();
					// </Fail>
				}
			}
			// </WriteReadBodyCore>
		} 

#endif // FEATURE_TAP

		// </WriteReadBody>
		// <WriteReadBody typeName="NullableUInt32" isAsync="False">
		public override bool ReadNullableUInt32( out UInt32? result )
		{
			// <WriteReadBodyCore typeName="UInt32" nullability="Nullable" isAsync="False">
			byte header;
			long integral;
			float real32;
			double real64;
			var type = this.ReadValue( out header, out integral, out real32, out real64 );
			switch( type )
			{
				case ReadValueResult.Eof:
				{
					// <Fail resultVariable="result" typeName="UInt32?" isAsync="False">
					result = default( UInt32? );
					return false;
					// </Fail>
				}
				case ReadValueResult.Nil:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="default( UInt32? )" isAsync="False">
					result = default( UInt32? );
					return true;
					// </Success>
				}
				// <WriteReadScalarCore type="System.UInt32" isNullable="True" valueVariable="i: integral, r4: real32, r8: real64, a: asyncResult" resultVariable="result" isAsync="False">
				case ReadValueResult.UInt32:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="UInt32?" resultVariable="result" expression="unchecked( ( UInt32 )integral )" isAsync="False">
					result = unchecked( ( UInt32 )integral );
					return true;
					// </Success>
				}
				case ReadValueResult.SByte:
				case ReadValueResult.Int16:
				case ReadValueResult.Int32:
				case ReadValueResult.Int64:
				case ReadValueResult.Byte:
				case ReadValueResult.UInt16:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="UInt32?" resultVariable="result" expression="checked( ( UInt32 )integral )" isAsync="False">
					result = checked( ( UInt32 )integral );
					return true;
					// </Success>
				}
				case ReadValueResult.UInt64:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="UInt32?" resultVariable="result" expression="checked( ( UInt32 )( UInt64 )integral )" isAsync="False">
					result = checked( ( UInt32 )( UInt64 )integral );
					return true;
					// </Success>
				}
				case ReadValueResult.Single:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="UInt32?" resultVariable="result" expression="checked( ( UInt32 )real32 )" isAsync="False">
					result = checked( ( UInt32 )real32 );
					return true;
					// </Success>
				}
				case ReadValueResult.Double:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="UInt32?" resultVariable="result" expression="checked( ( UInt32 )real64 )" isAsync="False">
					result = checked( ( UInt32 )real64 );
					return true;
					// </Success>
				}
				// </WriteReadScalarCore>
				default:
				{
					this.ThrowTypeException( typeof( UInt32? ), header );
					// Never reach
					// <Fail resultVariable="result" typeName="UInt32?" isAsync="False">
					result = default( UInt32? );
					return false;
					// </Fail>
				}
			}
			// </WriteReadBodyCore>
		} 
		// </WriteReadBody>
		// <WriteReadBody typeName="NullableUInt32" isAsync="True">

#if FEATURE_TAP

		public async override Task<AsyncReadResult<UInt32?>> ReadNullableUInt32Async( CancellationToken cancellationToken )
		{
			// <WriteReadBodyCore typeName="UInt32" nullability="Nullable" isAsync="True">
			var asyncResult = await this.ReadValueAsync( cancellationToken ).ConfigureAwait( false );
			var type = asyncResult.type;
			switch( type )
			{
				case ReadValueResult.Eof:
				{
					// <Fail resultVariable="result" typeName="UInt32?" isAsync="True">
					return AsyncReadResult.Fail<UInt32?>();
					// </Fail>
				}
				case ReadValueResult.Nil:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="default( UInt32? )" isAsync="True">
					return AsyncReadResult.Success( default( UInt32? ) );
					// </Success>
				}
				// <WriteReadScalarCore type="System.UInt32" isNullable="True" valueVariable="i: integral, r4: real32, r8: real64, a: asyncResult" resultVariable="result" isAsync="True">
				case ReadValueResult.UInt32:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="UInt32?" resultVariable="result" expression="unchecked( ( UInt32 )asyncResult.integral )" isAsync="True">
					return AsyncReadResult.Success<UInt32?>( unchecked( ( UInt32 )asyncResult.integral ) );
					// </Success>
				}
				case ReadValueResult.SByte:
				case ReadValueResult.Int16:
				case ReadValueResult.Int32:
				case ReadValueResult.Int64:
				case ReadValueResult.Byte:
				case ReadValueResult.UInt16:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="UInt32?" resultVariable="result" expression="checked( ( UInt32 )asyncResult.integral )" isAsync="True">
					return AsyncReadResult.Success<UInt32?>( checked( ( UInt32 )asyncResult.integral ) );
					// </Success>
				}
				case ReadValueResult.UInt64:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="UInt32?" resultVariable="result" expression="checked( ( UInt32 )( UInt64 )asyncResult.integral )" isAsync="True">
					return AsyncReadResult.Success<UInt32?>( checked( ( UInt32 )( UInt64 )asyncResult.integral ) );
					// </Success>
				}
				case ReadValueResult.Single:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="UInt32?" resultVariable="result" expression="checked( ( UInt32 )asyncResult.real32 )" isAsync="True">
					return AsyncReadResult.Success<UInt32?>( checked( ( UInt32 )asyncResult.real32 ) );
					// </Success>
				}
				case ReadValueResult.Double:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="UInt32?" resultVariable="result" expression="checked( ( UInt32 )asyncResult.real64 )" isAsync="True">
					return AsyncReadResult.Success<UInt32?>( checked( ( UInt32 )asyncResult.real64 ) );
					// </Success>
				}
				// </WriteReadScalarCore>
				default:
				{
					this.ThrowTypeException( typeof( UInt32? ), asyncResult.header );
					// Never reach
					// <Fail resultVariable="result" typeName="UInt32?" isAsync="True">
					return AsyncReadResult.Fail<UInt32?>();
					// </Fail>
				}
			}
			// </WriteReadBodyCore>
		} 

#endif // FEATURE_TAP

		// </WriteReadBody>
		// <WriteReadBody typeName="Int64" isAsync="False">
		public override bool ReadInt64( out Int64 result )
		{
			// <WriteReadBodyCore typeName="Int64" nullability="Value" isAsync="False">
			byte header;
			long integral;
			float real32;
			double real64;
			var type = this.ReadValue( out header, out integral, out real32, out real64 );
			switch( type )
			{
				case ReadValueResult.Eof:
				{
					// <Fail resultVariable="result" typeName="Int64" isAsync="False">
					result = default( Int64 );
					return false;
					// </Fail>
				}
				// <WriteReadScalarCore type="System.Int64" isNullable="False" valueVariable="i: integral, r4: real32, r8: real64, a: asyncResult" resultVariable="result" isAsync="False">
				case ReadValueResult.Int64:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="integral" isAsync="False">
					result = integral;
					return true;
					// </Success>
				}
				case ReadValueResult.SByte:
				case ReadValueResult.Int16:
				case ReadValueResult.Int32:
				case ReadValueResult.Byte:
				case ReadValueResult.UInt16:
				case ReadValueResult.UInt32:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="integral" isAsync="False">
					result = integral;
					return true;
					// </Success>
				}
				case ReadValueResult.UInt64:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="checked( ( Int64 )( UInt64 )integral )" isAsync="False">
					result = checked( ( Int64 )( UInt64 )integral );
					return true;
					// </Success>
				}
				case ReadValueResult.Single:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="checked( ( Int64 )real32 )" isAsync="False">
					result = checked( ( Int64 )real32 );
					return true;
					// </Success>
				}
				case ReadValueResult.Double:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="checked( ( Int64 )real64 )" isAsync="False">
					result = checked( ( Int64 )real64 );
					return true;
					// </Success>
				}
				// </WriteReadScalarCore>
				default:
				{
					this.ThrowTypeException( typeof( Int64 ), header );
					// Never reach
					// <Fail resultVariable="result" typeName="Int64" isAsync="False">
					result = default( Int64 );
					return false;
					// </Fail>
				}
			}
			// </WriteReadBodyCore>
		} 
		// </WriteReadBody>
		// <WriteReadBody typeName="Int64" isAsync="True">

#if FEATURE_TAP

		public async override Task<AsyncReadResult<Int64>> ReadInt64Async( CancellationToken cancellationToken )
		{
			// <WriteReadBodyCore typeName="Int64" nullability="Value" isAsync="True">
			var asyncResult = await this.ReadValueAsync( cancellationToken ).ConfigureAwait( false );
			var type = asyncResult.type;
			switch( type )
			{
				case ReadValueResult.Eof:
				{
					// <Fail resultVariable="result" typeName="Int64" isAsync="True">
					return AsyncReadResult.Fail<Int64>();
					// </Fail>
				}
				// <WriteReadScalarCore type="System.Int64" isNullable="False" valueVariable="i: integral, r4: real32, r8: real64, a: asyncResult" resultVariable="result" isAsync="True">
				case ReadValueResult.Int64:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="asyncResult.integral" isAsync="True">
					return AsyncReadResult.Success( asyncResult.integral );
					// </Success>
				}
				case ReadValueResult.SByte:
				case ReadValueResult.Int16:
				case ReadValueResult.Int32:
				case ReadValueResult.Byte:
				case ReadValueResult.UInt16:
				case ReadValueResult.UInt32:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="asyncResult.integral" isAsync="True">
					return AsyncReadResult.Success( asyncResult.integral );
					// </Success>
				}
				case ReadValueResult.UInt64:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="checked( ( Int64 )( UInt64 )asyncResult.integral )" isAsync="True">
					return AsyncReadResult.Success( checked( ( Int64 )( UInt64 )asyncResult.integral ) );
					// </Success>
				}
				case ReadValueResult.Single:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="checked( ( Int64 )asyncResult.real32 )" isAsync="True">
					return AsyncReadResult.Success( checked( ( Int64 )asyncResult.real32 ) );
					// </Success>
				}
				case ReadValueResult.Double:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="checked( ( Int64 )asyncResult.real64 )" isAsync="True">
					return AsyncReadResult.Success( checked( ( Int64 )asyncResult.real64 ) );
					// </Success>
				}
				// </WriteReadScalarCore>
				default:
				{
					this.ThrowTypeException( typeof( Int64 ), asyncResult.header );
					// Never reach
					// <Fail resultVariable="result" typeName="Int64" isAsync="True">
					return AsyncReadResult.Fail<Int64>();
					// </Fail>
				}
			}
			// </WriteReadBodyCore>
		} 

#endif // FEATURE_TAP

		// </WriteReadBody>
		// <WriteReadBody typeName="NullableInt64" isAsync="False">
		public override bool ReadNullableInt64( out Int64? result )
		{
			// <WriteReadBodyCore typeName="Int64" nullability="Nullable" isAsync="False">
			byte header;
			long integral;
			float real32;
			double real64;
			var type = this.ReadValue( out header, out integral, out real32, out real64 );
			switch( type )
			{
				case ReadValueResult.Eof:
				{
					// <Fail resultVariable="result" typeName="Int64?" isAsync="False">
					result = default( Int64? );
					return false;
					// </Fail>
				}
				case ReadValueResult.Nil:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="default( Int64? )" isAsync="False">
					result = default( Int64? );
					return true;
					// </Success>
				}
				// <WriteReadScalarCore type="System.Int64" isNullable="True" valueVariable="i: integral, r4: real32, r8: real64, a: asyncResult" resultVariable="result" isAsync="False">
				case ReadValueResult.Int64:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="Int64?" resultVariable="result" expression="integral" isAsync="False">
					result = integral;
					return true;
					// </Success>
				}
				case ReadValueResult.SByte:
				case ReadValueResult.Int16:
				case ReadValueResult.Int32:
				case ReadValueResult.Byte:
				case ReadValueResult.UInt16:
				case ReadValueResult.UInt32:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="Int64?" resultVariable="result" expression="integral" isAsync="False">
					result = integral;
					return true;
					// </Success>
				}
				case ReadValueResult.UInt64:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="Int64?" resultVariable="result" expression="checked( ( Int64 )( UInt64 )integral )" isAsync="False">
					result = checked( ( Int64 )( UInt64 )integral );
					return true;
					// </Success>
				}
				case ReadValueResult.Single:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="Int64?" resultVariable="result" expression="checked( ( Int64 )real32 )" isAsync="False">
					result = checked( ( Int64 )real32 );
					return true;
					// </Success>
				}
				case ReadValueResult.Double:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="Int64?" resultVariable="result" expression="checked( ( Int64 )real64 )" isAsync="False">
					result = checked( ( Int64 )real64 );
					return true;
					// </Success>
				}
				// </WriteReadScalarCore>
				default:
				{
					this.ThrowTypeException( typeof( Int64? ), header );
					// Never reach
					// <Fail resultVariable="result" typeName="Int64?" isAsync="False">
					result = default( Int64? );
					return false;
					// </Fail>
				}
			}
			// </WriteReadBodyCore>
		} 
		// </WriteReadBody>
		// <WriteReadBody typeName="NullableInt64" isAsync="True">

#if FEATURE_TAP

		public async override Task<AsyncReadResult<Int64?>> ReadNullableInt64Async( CancellationToken cancellationToken )
		{
			// <WriteReadBodyCore typeName="Int64" nullability="Nullable" isAsync="True">
			var asyncResult = await this.ReadValueAsync( cancellationToken ).ConfigureAwait( false );
			var type = asyncResult.type;
			switch( type )
			{
				case ReadValueResult.Eof:
				{
					// <Fail resultVariable="result" typeName="Int64?" isAsync="True">
					return AsyncReadResult.Fail<Int64?>();
					// </Fail>
				}
				case ReadValueResult.Nil:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="default( Int64? )" isAsync="True">
					return AsyncReadResult.Success( default( Int64? ) );
					// </Success>
				}
				// <WriteReadScalarCore type="System.Int64" isNullable="True" valueVariable="i: integral, r4: real32, r8: real64, a: asyncResult" resultVariable="result" isAsync="True">
				case ReadValueResult.Int64:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="Int64?" resultVariable="result" expression="asyncResult.integral" isAsync="True">
					return AsyncReadResult.Success<Int64?>( asyncResult.integral );
					// </Success>
				}
				case ReadValueResult.SByte:
				case ReadValueResult.Int16:
				case ReadValueResult.Int32:
				case ReadValueResult.Byte:
				case ReadValueResult.UInt16:
				case ReadValueResult.UInt32:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="Int64?" resultVariable="result" expression="asyncResult.integral" isAsync="True">
					return AsyncReadResult.Success<Int64?>( asyncResult.integral );
					// </Success>
				}
				case ReadValueResult.UInt64:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="Int64?" resultVariable="result" expression="checked( ( Int64 )( UInt64 )asyncResult.integral )" isAsync="True">
					return AsyncReadResult.Success<Int64?>( checked( ( Int64 )( UInt64 )asyncResult.integral ) );
					// </Success>
				}
				case ReadValueResult.Single:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="Int64?" resultVariable="result" expression="checked( ( Int64 )asyncResult.real32 )" isAsync="True">
					return AsyncReadResult.Success<Int64?>( checked( ( Int64 )asyncResult.real32 ) );
					// </Success>
				}
				case ReadValueResult.Double:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="Int64?" resultVariable="result" expression="checked( ( Int64 )asyncResult.real64 )" isAsync="True">
					return AsyncReadResult.Success<Int64?>( checked( ( Int64 )asyncResult.real64 ) );
					// </Success>
				}
				// </WriteReadScalarCore>
				default:
				{
					this.ThrowTypeException( typeof( Int64? ), asyncResult.header );
					// Never reach
					// <Fail resultVariable="result" typeName="Int64?" isAsync="True">
					return AsyncReadResult.Fail<Int64?>();
					// </Fail>
				}
			}
			// </WriteReadBodyCore>
		} 

#endif // FEATURE_TAP

		// </WriteReadBody>
		// <WriteReadBody typeName="UInt64" isAsync="False">
		public override bool ReadUInt64( out UInt64 result )
		{
			// <WriteReadBodyCore typeName="UInt64" nullability="Value" isAsync="False">
			byte header;
			long integral;
			float real32;
			double real64;
			var type = this.ReadValue( out header, out integral, out real32, out real64 );
			switch( type )
			{
				case ReadValueResult.Eof:
				{
					// <Fail resultVariable="result" typeName="UInt64" isAsync="False">
					result = default( UInt64 );
					return false;
					// </Fail>
				}
				// <WriteReadScalarCore type="System.UInt64" isNullable="False" valueVariable="i: integral, r4: real32, r8: real64, a: asyncResult" resultVariable="result" isAsync="False">
				case ReadValueResult.UInt64:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="unchecked( ( UInt64 )integral )" isAsync="False">
					result = unchecked( ( UInt64 )integral );
					return true;
					// </Success>
				}
				case ReadValueResult.SByte:
				case ReadValueResult.Int16:
				case ReadValueResult.Int32:
				case ReadValueResult.Int64:
				case ReadValueResult.Byte:
				case ReadValueResult.UInt16:
				case ReadValueResult.UInt32:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="checked( ( UInt64 )integral )" isAsync="False">
					result = checked( ( UInt64 )integral );
					return true;
					// </Success>
				}
				case ReadValueResult.Single:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="checked( ( UInt64 )real32 )" isAsync="False">
					result = checked( ( UInt64 )real32 );
					return true;
					// </Success>
				}
				case ReadValueResult.Double:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="checked( ( UInt64 )real64 )" isAsync="False">
					result = checked( ( UInt64 )real64 );
					return true;
					// </Success>
				}
				// </WriteReadScalarCore>
				default:
				{
					this.ThrowTypeException( typeof( UInt64 ), header );
					// Never reach
					// <Fail resultVariable="result" typeName="UInt64" isAsync="False">
					result = default( UInt64 );
					return false;
					// </Fail>
				}
			}
			// </WriteReadBodyCore>
		} 
		// </WriteReadBody>
		// <WriteReadBody typeName="UInt64" isAsync="True">

#if FEATURE_TAP

		public async override Task<AsyncReadResult<UInt64>> ReadUInt64Async( CancellationToken cancellationToken )
		{
			// <WriteReadBodyCore typeName="UInt64" nullability="Value" isAsync="True">
			var asyncResult = await this.ReadValueAsync( cancellationToken ).ConfigureAwait( false );
			var type = asyncResult.type;
			switch( type )
			{
				case ReadValueResult.Eof:
				{
					// <Fail resultVariable="result" typeName="UInt64" isAsync="True">
					return AsyncReadResult.Fail<UInt64>();
					// </Fail>
				}
				// <WriteReadScalarCore type="System.UInt64" isNullable="False" valueVariable="i: integral, r4: real32, r8: real64, a: asyncResult" resultVariable="result" isAsync="True">
				case ReadValueResult.UInt64:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="unchecked( ( UInt64 )asyncResult.integral )" isAsync="True">
					return AsyncReadResult.Success( unchecked( ( UInt64 )asyncResult.integral ) );
					// </Success>
				}
				case ReadValueResult.SByte:
				case ReadValueResult.Int16:
				case ReadValueResult.Int32:
				case ReadValueResult.Int64:
				case ReadValueResult.Byte:
				case ReadValueResult.UInt16:
				case ReadValueResult.UInt32:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="checked( ( UInt64 )asyncResult.integral )" isAsync="True">
					return AsyncReadResult.Success( checked( ( UInt64 )asyncResult.integral ) );
					// </Success>
				}
				case ReadValueResult.Single:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="checked( ( UInt64 )asyncResult.real32 )" isAsync="True">
					return AsyncReadResult.Success( checked( ( UInt64 )asyncResult.real32 ) );
					// </Success>
				}
				case ReadValueResult.Double:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="checked( ( UInt64 )asyncResult.real64 )" isAsync="True">
					return AsyncReadResult.Success( checked( ( UInt64 )asyncResult.real64 ) );
					// </Success>
				}
				// </WriteReadScalarCore>
				default:
				{
					this.ThrowTypeException( typeof( UInt64 ), asyncResult.header );
					// Never reach
					// <Fail resultVariable="result" typeName="UInt64" isAsync="True">
					return AsyncReadResult.Fail<UInt64>();
					// </Fail>
				}
			}
			// </WriteReadBodyCore>
		} 

#endif // FEATURE_TAP

		// </WriteReadBody>
		// <WriteReadBody typeName="NullableUInt64" isAsync="False">
		public override bool ReadNullableUInt64( out UInt64? result )
		{
			// <WriteReadBodyCore typeName="UInt64" nullability="Nullable" isAsync="False">
			byte header;
			long integral;
			float real32;
			double real64;
			var type = this.ReadValue( out header, out integral, out real32, out real64 );
			switch( type )
			{
				case ReadValueResult.Eof:
				{
					// <Fail resultVariable="result" typeName="UInt64?" isAsync="False">
					result = default( UInt64? );
					return false;
					// </Fail>
				}
				case ReadValueResult.Nil:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="default( UInt64? )" isAsync="False">
					result = default( UInt64? );
					return true;
					// </Success>
				}
				// <WriteReadScalarCore type="System.UInt64" isNullable="True" valueVariable="i: integral, r4: real32, r8: real64, a: asyncResult" resultVariable="result" isAsync="False">
				case ReadValueResult.UInt64:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="UInt64?" resultVariable="result" expression="unchecked( ( UInt64 )integral )" isAsync="False">
					result = unchecked( ( UInt64 )integral );
					return true;
					// </Success>
				}
				case ReadValueResult.SByte:
				case ReadValueResult.Int16:
				case ReadValueResult.Int32:
				case ReadValueResult.Int64:
				case ReadValueResult.Byte:
				case ReadValueResult.UInt16:
				case ReadValueResult.UInt32:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="UInt64?" resultVariable="result" expression="checked( ( UInt64 )integral )" isAsync="False">
					result = checked( ( UInt64 )integral );
					return true;
					// </Success>
				}
				case ReadValueResult.Single:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="UInt64?" resultVariable="result" expression="checked( ( UInt64 )real32 )" isAsync="False">
					result = checked( ( UInt64 )real32 );
					return true;
					// </Success>
				}
				case ReadValueResult.Double:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="UInt64?" resultVariable="result" expression="checked( ( UInt64 )real64 )" isAsync="False">
					result = checked( ( UInt64 )real64 );
					return true;
					// </Success>
				}
				// </WriteReadScalarCore>
				default:
				{
					this.ThrowTypeException( typeof( UInt64? ), header );
					// Never reach
					// <Fail resultVariable="result" typeName="UInt64?" isAsync="False">
					result = default( UInt64? );
					return false;
					// </Fail>
				}
			}
			// </WriteReadBodyCore>
		} 
		// </WriteReadBody>
		// <WriteReadBody typeName="NullableUInt64" isAsync="True">

#if FEATURE_TAP

		public async override Task<AsyncReadResult<UInt64?>> ReadNullableUInt64Async( CancellationToken cancellationToken )
		{
			// <WriteReadBodyCore typeName="UInt64" nullability="Nullable" isAsync="True">
			var asyncResult = await this.ReadValueAsync( cancellationToken ).ConfigureAwait( false );
			var type = asyncResult.type;
			switch( type )
			{
				case ReadValueResult.Eof:
				{
					// <Fail resultVariable="result" typeName="UInt64?" isAsync="True">
					return AsyncReadResult.Fail<UInt64?>();
					// </Fail>
				}
				case ReadValueResult.Nil:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="default( UInt64? )" isAsync="True">
					return AsyncReadResult.Success( default( UInt64? ) );
					// </Success>
				}
				// <WriteReadScalarCore type="System.UInt64" isNullable="True" valueVariable="i: integral, r4: real32, r8: real64, a: asyncResult" resultVariable="result" isAsync="True">
				case ReadValueResult.UInt64:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="UInt64?" resultVariable="result" expression="unchecked( ( UInt64 )asyncResult.integral )" isAsync="True">
					return AsyncReadResult.Success<UInt64?>( unchecked( ( UInt64 )asyncResult.integral ) );
					// </Success>
				}
				case ReadValueResult.SByte:
				case ReadValueResult.Int16:
				case ReadValueResult.Int32:
				case ReadValueResult.Int64:
				case ReadValueResult.Byte:
				case ReadValueResult.UInt16:
				case ReadValueResult.UInt32:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="UInt64?" resultVariable="result" expression="checked( ( UInt64 )asyncResult.integral )" isAsync="True">
					return AsyncReadResult.Success<UInt64?>( checked( ( UInt64 )asyncResult.integral ) );
					// </Success>
				}
				case ReadValueResult.Single:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="UInt64?" resultVariable="result" expression="checked( ( UInt64 )asyncResult.real32 )" isAsync="True">
					return AsyncReadResult.Success<UInt64?>( checked( ( UInt64 )asyncResult.real32 ) );
					// </Success>
				}
				case ReadValueResult.Double:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="UInt64?" resultVariable="result" expression="checked( ( UInt64 )asyncResult.real64 )" isAsync="True">
					return AsyncReadResult.Success<UInt64?>( checked( ( UInt64 )asyncResult.real64 ) );
					// </Success>
				}
				// </WriteReadScalarCore>
				default:
				{
					this.ThrowTypeException( typeof( UInt64? ), asyncResult.header );
					// Never reach
					// <Fail resultVariable="result" typeName="UInt64?" isAsync="True">
					return AsyncReadResult.Fail<UInt64?>();
					// </Fail>
				}
			}
			// </WriteReadBodyCore>
		} 

#endif // FEATURE_TAP

		// </WriteReadBody>
		// <WriteReadBody typeName="Single" isAsync="False">
		public override bool ReadSingle( out Single result )
		{
			// <WriteReadBodyCore typeName="Single" nullability="Value" isAsync="False">
			byte header;
			long integral;
			float real32;
			double real64;
			var type = this.ReadValue( out header, out integral, out real32, out real64 );
			switch( type )
			{
				case ReadValueResult.Eof:
				{
					// <Fail resultVariable="result" typeName="Single" isAsync="False">
					result = default( Single );
					return false;
					// </Fail>
				}
				// <WriteReadScalarCore type="System.Single" isNullable="False" valueVariable="i: integral, r4: real32, r8: real64, a: asyncResult" resultVariable="result" isAsync="False">
				case ReadValueResult.Single:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="real32" isAsync="False">
					result = real32;
					return true;
					// </Success>
				}
				case ReadValueResult.SByte:
				case ReadValueResult.Int16:
				case ReadValueResult.Int32:
				case ReadValueResult.Int64:
				case ReadValueResult.Byte:
				case ReadValueResult.UInt16:
				case ReadValueResult.UInt32:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="integral" isAsync="False">
					result = integral;
					return true;
					// </Success>
				}
				case ReadValueResult.UInt64:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="unchecked( ( UInt64 )integral )" isAsync="False">
					result = unchecked( ( UInt64 )integral );
					return true;
					// </Success>
				}
				case ReadValueResult.Double:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="checked( ( Single )real64 )" isAsync="False">
					result = checked( ( Single )real64 );
					return true;
					// </Success>
				}
				// </WriteReadScalarCore>
				default:
				{
					this.ThrowTypeException( typeof( Single ), header );
					// Never reach
					// <Fail resultVariable="result" typeName="Single" isAsync="False">
					result = default( Single );
					return false;
					// </Fail>
				}
			}
			// </WriteReadBodyCore>
		} 
		// </WriteReadBody>
		// <WriteReadBody typeName="Single" isAsync="True">

#if FEATURE_TAP

		public async override Task<AsyncReadResult<Single>> ReadSingleAsync( CancellationToken cancellationToken )
		{
			// <WriteReadBodyCore typeName="Single" nullability="Value" isAsync="True">
			var asyncResult = await this.ReadValueAsync( cancellationToken ).ConfigureAwait( false );
			var type = asyncResult.type;
			switch( type )
			{
				case ReadValueResult.Eof:
				{
					// <Fail resultVariable="result" typeName="Single" isAsync="True">
					return AsyncReadResult.Fail<Single>();
					// </Fail>
				}
				// <WriteReadScalarCore type="System.Single" isNullable="False" valueVariable="i: integral, r4: real32, r8: real64, a: asyncResult" resultVariable="result" isAsync="True">
				case ReadValueResult.Single:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="asyncResult.real32" isAsync="True">
					return AsyncReadResult.Success( asyncResult.real32 );
					// </Success>
				}
				case ReadValueResult.SByte:
				case ReadValueResult.Int16:
				case ReadValueResult.Int32:
				case ReadValueResult.Int64:
				case ReadValueResult.Byte:
				case ReadValueResult.UInt16:
				case ReadValueResult.UInt32:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="checked( ( Single )asyncResult.integral )" isAsync="True">
					return AsyncReadResult.Success( checked( ( Single )asyncResult.integral ) );
					// </Success>
				}
				case ReadValueResult.UInt64:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="checked( ( Single )( UInt64 )asyncResult.integral )" isAsync="True">
					return AsyncReadResult.Success( checked( ( Single )( UInt64 )asyncResult.integral ) );
					// </Success>
				}
				case ReadValueResult.Double:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="checked( ( Single )asyncResult.real64 )" isAsync="True">
					return AsyncReadResult.Success( checked( ( Single )asyncResult.real64 ) );
					// </Success>
				}
				// </WriteReadScalarCore>
				default:
				{
					this.ThrowTypeException( typeof( Single ), asyncResult.header );
					// Never reach
					// <Fail resultVariable="result" typeName="Single" isAsync="True">
					return AsyncReadResult.Fail<Single>();
					// </Fail>
				}
			}
			// </WriteReadBodyCore>
		} 

#endif // FEATURE_TAP

		// </WriteReadBody>
		// <WriteReadBody typeName="NullableSingle" isAsync="False">
		public override bool ReadNullableSingle( out Single? result )
		{
			// <WriteReadBodyCore typeName="Single" nullability="Nullable" isAsync="False">
			byte header;
			long integral;
			float real32;
			double real64;
			var type = this.ReadValue( out header, out integral, out real32, out real64 );
			switch( type )
			{
				case ReadValueResult.Eof:
				{
					// <Fail resultVariable="result" typeName="Single?" isAsync="False">
					result = default( Single? );
					return false;
					// </Fail>
				}
				case ReadValueResult.Nil:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="default( Single? )" isAsync="False">
					result = default( Single? );
					return true;
					// </Success>
				}
				// <WriteReadScalarCore type="System.Single" isNullable="True" valueVariable="i: integral, r4: real32, r8: real64, a: asyncResult" resultVariable="result" isAsync="False">
				case ReadValueResult.Single:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="Single?" resultVariable="result" expression="real32" isAsync="False">
					result = real32;
					return true;
					// </Success>
				}
				case ReadValueResult.SByte:
				case ReadValueResult.Int16:
				case ReadValueResult.Int32:
				case ReadValueResult.Int64:
				case ReadValueResult.Byte:
				case ReadValueResult.UInt16:
				case ReadValueResult.UInt32:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="Single?" resultVariable="result" expression="integral" isAsync="False">
					result = integral;
					return true;
					// </Success>
				}
				case ReadValueResult.UInt64:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="Single?" resultVariable="result" expression="unchecked( ( UInt64 )integral )" isAsync="False">
					result = unchecked( ( UInt64 )integral );
					return true;
					// </Success>
				}
				case ReadValueResult.Double:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="Single?" resultVariable="result" expression="checked( ( Single )real64 )" isAsync="False">
					result = checked( ( Single )real64 );
					return true;
					// </Success>
				}
				// </WriteReadScalarCore>
				default:
				{
					this.ThrowTypeException( typeof( Single? ), header );
					// Never reach
					// <Fail resultVariable="result" typeName="Single?" isAsync="False">
					result = default( Single? );
					return false;
					// </Fail>
				}
			}
			// </WriteReadBodyCore>
		} 
		// </WriteReadBody>
		// <WriteReadBody typeName="NullableSingle" isAsync="True">

#if FEATURE_TAP

		public async override Task<AsyncReadResult<Single?>> ReadNullableSingleAsync( CancellationToken cancellationToken )
		{
			// <WriteReadBodyCore typeName="Single" nullability="Nullable" isAsync="True">
			var asyncResult = await this.ReadValueAsync( cancellationToken ).ConfigureAwait( false );
			var type = asyncResult.type;
			switch( type )
			{
				case ReadValueResult.Eof:
				{
					// <Fail resultVariable="result" typeName="Single?" isAsync="True">
					return AsyncReadResult.Fail<Single?>();
					// </Fail>
				}
				case ReadValueResult.Nil:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="default( Single? )" isAsync="True">
					return AsyncReadResult.Success( default( Single? ) );
					// </Success>
				}
				// <WriteReadScalarCore type="System.Single" isNullable="True" valueVariable="i: integral, r4: real32, r8: real64, a: asyncResult" resultVariable="result" isAsync="True">
				case ReadValueResult.Single:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="Single?" resultVariable="result" expression="asyncResult.real32" isAsync="True">
					return AsyncReadResult.Success<Single?>( asyncResult.real32 );
					// </Success>
				}
				case ReadValueResult.SByte:
				case ReadValueResult.Int16:
				case ReadValueResult.Int32:
				case ReadValueResult.Int64:
				case ReadValueResult.Byte:
				case ReadValueResult.UInt16:
				case ReadValueResult.UInt32:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="Single?" resultVariable="result" expression="checked( ( Single )asyncResult.integral )" isAsync="True">
					return AsyncReadResult.Success<Single?>( checked( ( Single )asyncResult.integral ) );
					// </Success>
				}
				case ReadValueResult.UInt64:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="Single?" resultVariable="result" expression="checked( ( Single )( UInt64 )asyncResult.integral )" isAsync="True">
					return AsyncReadResult.Success<Single?>( checked( ( Single )( UInt64 )asyncResult.integral ) );
					// </Success>
				}
				case ReadValueResult.Double:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="Single?" resultVariable="result" expression="checked( ( Single )asyncResult.real64 )" isAsync="True">
					return AsyncReadResult.Success<Single?>( checked( ( Single )asyncResult.real64 ) );
					// </Success>
				}
				// </WriteReadScalarCore>
				default:
				{
					this.ThrowTypeException( typeof( Single? ), asyncResult.header );
					// Never reach
					// <Fail resultVariable="result" typeName="Single?" isAsync="True">
					return AsyncReadResult.Fail<Single?>();
					// </Fail>
				}
			}
			// </WriteReadBodyCore>
		} 

#endif // FEATURE_TAP

		// </WriteReadBody>
		// <WriteReadBody typeName="Double" isAsync="False">
		public override bool ReadDouble( out Double result )
		{
			// <WriteReadBodyCore typeName="Double" nullability="Value" isAsync="False">
			byte header;
			long integral;
			float real32;
			double real64;
			var type = this.ReadValue( out header, out integral, out real32, out real64 );
			switch( type )
			{
				case ReadValueResult.Eof:
				{
					// <Fail resultVariable="result" typeName="Double" isAsync="False">
					result = default( Double );
					return false;
					// </Fail>
				}
				// <WriteReadScalarCore type="System.Double" isNullable="False" valueVariable="i: integral, r4: real32, r8: real64, a: asyncResult" resultVariable="result" isAsync="False">
				case ReadValueResult.Double:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="real64" isAsync="False">
					result = real64;
					return true;
					// </Success>
				}
				case ReadValueResult.SByte:
				case ReadValueResult.Int16:
				case ReadValueResult.Int32:
				case ReadValueResult.Int64:
				case ReadValueResult.Byte:
				case ReadValueResult.UInt16:
				case ReadValueResult.UInt32:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="integral" isAsync="False">
					result = integral;
					return true;
					// </Success>
				}
				case ReadValueResult.UInt64:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="unchecked( ( UInt64 )integral )" isAsync="False">
					result = unchecked( ( UInt64 )integral );
					return true;
					// </Success>
				}
				case ReadValueResult.Single:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="real32" isAsync="False">
					result = real32;
					return true;
					// </Success>
				}
				// </WriteReadScalarCore>
				default:
				{
					this.ThrowTypeException( typeof( Double ), header );
					// Never reach
					// <Fail resultVariable="result" typeName="Double" isAsync="False">
					result = default( Double );
					return false;
					// </Fail>
				}
			}
			// </WriteReadBodyCore>
		} 
		// </WriteReadBody>
		// <WriteReadBody typeName="Double" isAsync="True">

#if FEATURE_TAP

		public async override Task<AsyncReadResult<Double>> ReadDoubleAsync( CancellationToken cancellationToken )
		{
			// <WriteReadBodyCore typeName="Double" nullability="Value" isAsync="True">
			var asyncResult = await this.ReadValueAsync( cancellationToken ).ConfigureAwait( false );
			var type = asyncResult.type;
			switch( type )
			{
				case ReadValueResult.Eof:
				{
					// <Fail resultVariable="result" typeName="Double" isAsync="True">
					return AsyncReadResult.Fail<Double>();
					// </Fail>
				}
				// <WriteReadScalarCore type="System.Double" isNullable="False" valueVariable="i: integral, r4: real32, r8: real64, a: asyncResult" resultVariable="result" isAsync="True">
				case ReadValueResult.Double:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="asyncResult.real64" isAsync="True">
					return AsyncReadResult.Success( asyncResult.real64 );
					// </Success>
				}
				case ReadValueResult.SByte:
				case ReadValueResult.Int16:
				case ReadValueResult.Int32:
				case ReadValueResult.Int64:
				case ReadValueResult.Byte:
				case ReadValueResult.UInt16:
				case ReadValueResult.UInt32:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="checked( ( Double )asyncResult.integral )" isAsync="True">
					return AsyncReadResult.Success( checked( ( Double )asyncResult.integral ) );
					// </Success>
				}
				case ReadValueResult.UInt64:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="checked( ( Double )( UInt64 )asyncResult.integral )" isAsync="True">
					return AsyncReadResult.Success( checked( ( Double )( UInt64 )asyncResult.integral ) );
					// </Success>
				}
				case ReadValueResult.Single:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="checked( ( Double )asyncResult.real32 )" isAsync="True">
					return AsyncReadResult.Success( checked( ( Double )asyncResult.real32 ) );
					// </Success>
				}
				// </WriteReadScalarCore>
				default:
				{
					this.ThrowTypeException( typeof( Double ), asyncResult.header );
					// Never reach
					// <Fail resultVariable="result" typeName="Double" isAsync="True">
					return AsyncReadResult.Fail<Double>();
					// </Fail>
				}
			}
			// </WriteReadBodyCore>
		} 

#endif // FEATURE_TAP

		// </WriteReadBody>
		// <WriteReadBody typeName="NullableDouble" isAsync="False">
		public override bool ReadNullableDouble( out Double? result )
		{
			// <WriteReadBodyCore typeName="Double" nullability="Nullable" isAsync="False">
			byte header;
			long integral;
			float real32;
			double real64;
			var type = this.ReadValue( out header, out integral, out real32, out real64 );
			switch( type )
			{
				case ReadValueResult.Eof:
				{
					// <Fail resultVariable="result" typeName="Double?" isAsync="False">
					result = default( Double? );
					return false;
					// </Fail>
				}
				case ReadValueResult.Nil:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="default( Double? )" isAsync="False">
					result = default( Double? );
					return true;
					// </Success>
				}
				// <WriteReadScalarCore type="System.Double" isNullable="True" valueVariable="i: integral, r4: real32, r8: real64, a: asyncResult" resultVariable="result" isAsync="False">
				case ReadValueResult.Double:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="Double?" resultVariable="result" expression="real64" isAsync="False">
					result = real64;
					return true;
					// </Success>
				}
				case ReadValueResult.SByte:
				case ReadValueResult.Int16:
				case ReadValueResult.Int32:
				case ReadValueResult.Int64:
				case ReadValueResult.Byte:
				case ReadValueResult.UInt16:
				case ReadValueResult.UInt32:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="Double?" resultVariable="result" expression="integral" isAsync="False">
					result = integral;
					return true;
					// </Success>
				}
				case ReadValueResult.UInt64:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="Double?" resultVariable="result" expression="unchecked( ( UInt64 )integral )" isAsync="False">
					result = unchecked( ( UInt64 )integral );
					return true;
					// </Success>
				}
				case ReadValueResult.Single:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="Double?" resultVariable="result" expression="real32" isAsync="False">
					result = real32;
					return true;
					// </Success>
				}
				// </WriteReadScalarCore>
				default:
				{
					this.ThrowTypeException( typeof( Double? ), header );
					// Never reach
					// <Fail resultVariable="result" typeName="Double?" isAsync="False">
					result = default( Double? );
					return false;
					// </Fail>
				}
			}
			// </WriteReadBodyCore>
		} 
		// </WriteReadBody>
		// <WriteReadBody typeName="NullableDouble" isAsync="True">

#if FEATURE_TAP

		public async override Task<AsyncReadResult<Double?>> ReadNullableDoubleAsync( CancellationToken cancellationToken )
		{
			// <WriteReadBodyCore typeName="Double" nullability="Nullable" isAsync="True">
			var asyncResult = await this.ReadValueAsync( cancellationToken ).ConfigureAwait( false );
			var type = asyncResult.type;
			switch( type )
			{
				case ReadValueResult.Eof:
				{
					// <Fail resultVariable="result" typeName="Double?" isAsync="True">
					return AsyncReadResult.Fail<Double?>();
					// </Fail>
				}
				case ReadValueResult.Nil:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="default( Double? )" isAsync="True">
					return AsyncReadResult.Success( default( Double? ) );
					// </Success>
				}
				// <WriteReadScalarCore type="System.Double" isNullable="True" valueVariable="i: integral, r4: real32, r8: real64, a: asyncResult" resultVariable="result" isAsync="True">
				case ReadValueResult.Double:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="Double?" resultVariable="result" expression="asyncResult.real64" isAsync="True">
					return AsyncReadResult.Success<Double?>( asyncResult.real64 );
					// </Success>
				}
				case ReadValueResult.SByte:
				case ReadValueResult.Int16:
				case ReadValueResult.Int32:
				case ReadValueResult.Int64:
				case ReadValueResult.Byte:
				case ReadValueResult.UInt16:
				case ReadValueResult.UInt32:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="Double?" resultVariable="result" expression="checked( ( Double )asyncResult.integral )" isAsync="True">
					return AsyncReadResult.Success<Double?>( checked( ( Double )asyncResult.integral ) );
					// </Success>
				}
				case ReadValueResult.UInt64:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="Double?" resultVariable="result" expression="checked( ( Double )( UInt64 )asyncResult.integral )" isAsync="True">
					return AsyncReadResult.Success<Double?>( checked( ( Double )( UInt64 )asyncResult.integral ) );
					// </Success>
				}
				case ReadValueResult.Single:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="Double?" resultVariable="result" expression="checked( ( Double )asyncResult.real32 )" isAsync="True">
					return AsyncReadResult.Success<Double?>( checked( ( Double )asyncResult.real32 ) );
					// </Success>
				}
				// </WriteReadScalarCore>
				default:
				{
					this.ThrowTypeException( typeof( Double? ), asyncResult.header );
					// Never reach
					// <Fail resultVariable="result" typeName="Double?" isAsync="True">
					return AsyncReadResult.Fail<Double?>();
					// </Fail>
				}
			}
			// </WriteReadBodyCore>
		} 

#endif // FEATURE_TAP

		// </WriteReadBody>
		// <WriteReadBody typeName="Binary" isAsync="False">
		public override bool ReadBinary( out Byte[] result )
		{
			// <WriteReadBodyCore typeName="Byte[]" nullability="Reference" isAsync="False">
			byte header;
			long integral;
			float real32;
			double real64;
			var type = this.ReadValue( out header, out integral, out real32, out real64 );
			switch( type )
			{
				case ReadValueResult.Eof:
				{
					// <Fail resultVariable="result" typeName="Byte[]" isAsync="False">
					result = default( Byte[] );
					return false;
					// </Fail>
				}
				case ReadValueResult.Nil:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="default( Byte[] )" isAsync="False">
					result = default( Byte[] );
					return true;
					// </Success>
				}
				// <WriteReadRawCore code="Binary" valueVariable="i: integral, r4: real32, r8: real64, a: asyncResult" resultVariable="result" isAsync="False">
				case ReadValueResult.String:
				case ReadValueResult.Binary:
				{
					// <Success type="(null)" resultVariable="result" expression="this.ReadBinaryCore( integral )" isAsync="False">
					result = this.ReadBinaryCore( integral );
					return true;
					// </Success>
				}
				// </WriteReadRawCore>
				default:
				{
					this.ThrowTypeException( typeof( Byte[] ), header );
					// Never reach
					// <Fail resultVariable="result" typeName="Byte[]" isAsync="False">
					result = default( Byte[] );
					return false;
					// </Fail>
				}
			}
			// </WriteReadBodyCore>
		} 
		// </WriteReadBody>
		// <WriteReadBody typeName="Binary" isAsync="True">

#if FEATURE_TAP

		public async override Task<AsyncReadResult<Byte[]>> ReadBinaryAsync( CancellationToken cancellationToken )
		{
			// <WriteReadBodyCore typeName="Byte[]" nullability="Reference" isAsync="True">
			var asyncResult = await this.ReadValueAsync( cancellationToken ).ConfigureAwait( false );
			var type = asyncResult.type;
			switch( type )
			{
				case ReadValueResult.Eof:
				{
					// <Fail resultVariable="result" typeName="Byte[]" isAsync="True">
					return AsyncReadResult.Fail<Byte[]>();
					// </Fail>
				}
				case ReadValueResult.Nil:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="default( Byte[] )" isAsync="True">
					return AsyncReadResult.Success( default( Byte[] ) );
					// </Success>
				}
				// <WriteReadRawCore code="Binary" valueVariable="i: integral, r4: real32, r8: real64, a: asyncResult" resultVariable="result" isAsync="True">
				case ReadValueResult.String:
				case ReadValueResult.Binary:
				{
					// <Success type="(null)" resultVariable="result" expression="await this.ReadBinaryAsyncCore( asyncResult.integral, cancellationToken ).ConfigureAwait( false )" isAsync="True">
					return AsyncReadResult.Success( await this.ReadBinaryAsyncCore( asyncResult.integral, cancellationToken ).ConfigureAwait( false ) );
					// </Success>
				}
				// </WriteReadRawCore>
				default:
				{
					this.ThrowTypeException( typeof( Byte[] ), asyncResult.header );
					// Never reach
					// <Fail resultVariable="result" typeName="Byte[]" isAsync="True">
					return AsyncReadResult.Fail<Byte[]>();
					// </Fail>
				}
			}
			// </WriteReadBodyCore>
		} 

#endif // FEATURE_TAP

		// </WriteReadBody>
		// <WriteReadBody typeName="String" isAsync="False">
		public override bool ReadString( out String result )
		{
			// <WriteReadBodyCore typeName="String" nullability="Reference" isAsync="False">
			byte header;
			long integral;
			float real32;
			double real64;
			var type = this.ReadValue( out header, out integral, out real32, out real64 );
			switch( type )
			{
				case ReadValueResult.Eof:
				{
					// <Fail resultVariable="result" typeName="String" isAsync="False">
					result = default( String );
					return false;
					// </Fail>
				}
				case ReadValueResult.Nil:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="default( String )" isAsync="False">
					result = default( String );
					return true;
					// </Success>
				}
				// <WriteReadRawCore code="String" valueVariable="i: integral, r4: real32, r8: real64, a: asyncResult" resultVariable="result" isAsync="False">
				case ReadValueResult.String:
				case ReadValueResult.Binary:
				{
					// <Success type="(null)" resultVariable="result" expression="this.ReadStringCore( integral )" isAsync="False">
					result = this.ReadStringCore( integral );
					return true;
					// </Success>
				}
				// </WriteReadRawCore>
				default:
				{
					this.ThrowTypeException( typeof( String ), header );
					// Never reach
					// <Fail resultVariable="result" typeName="String" isAsync="False">
					result = default( String );
					return false;
					// </Fail>
				}
			}
			// </WriteReadBodyCore>
		} 
		// </WriteReadBody>
		// <WriteReadBody typeName="String" isAsync="True">

#if FEATURE_TAP

		public async override Task<AsyncReadResult<String>> ReadStringAsync( CancellationToken cancellationToken )
		{
			// <WriteReadBodyCore typeName="String" nullability="Reference" isAsync="True">
			var asyncResult = await this.ReadValueAsync( cancellationToken ).ConfigureAwait( false );
			var type = asyncResult.type;
			switch( type )
			{
				case ReadValueResult.Eof:
				{
					// <Fail resultVariable="result" typeName="String" isAsync="True">
					return AsyncReadResult.Fail<String>();
					// </Fail>
				}
				case ReadValueResult.Nil:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="default( String )" isAsync="True">
					return AsyncReadResult.Success( default( String ) );
					// </Success>
				}
				// <WriteReadRawCore code="String" valueVariable="i: integral, r4: real32, r8: real64, a: asyncResult" resultVariable="result" isAsync="True">
				case ReadValueResult.String:
				case ReadValueResult.Binary:
				{
					// <Success type="(null)" resultVariable="result" expression="await this.ReadStringAsyncCore( asyncResult.integral, cancellationToken ).ConfigureAwait( false )" isAsync="True">
					return AsyncReadResult.Success( await this.ReadStringAsyncCore( asyncResult.integral, cancellationToken ).ConfigureAwait( false ) );
					// </Success>
				}
				// </WriteReadRawCore>
				default:
				{
					this.ThrowTypeException( typeof( String ), asyncResult.header );
					// Never reach
					// <Fail resultVariable="result" typeName="String" isAsync="True">
					return AsyncReadResult.Fail<String>();
					// </Fail>
				}
			}
			// </WriteReadBodyCore>
		} 

#endif // FEATURE_TAP

		// </WriteReadBody>
		// <WriteReadBody typeName="Object" isAsync="False">
		public override bool ReadObject( bool isDeep, out MessagePackObject result )
		{
			// <WriteReadBodyCore typeName="MessagePackObject" nullability="Value" isAsync="False">
			byte header;
			long integral;
			float real32;
			double real64;
			var type = this.ReadValue( out header, out integral, out real32, out real64 );
			switch( type )
			{
				case ReadValueResult.Eof:
				{
					// <Fail resultVariable="result" typeName="MessagePackObject" isAsync="False">
					result = default( MessagePackObject );
					return false;
					// </Fail>
				}
				// <WriteReadObjectCore typeVariable="type" valueVariable="i: integral, r4: real32, r8: real64, a: asyncResult" resultVariable="result" isAsync="False">
				case ReadValueResult.Nil:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <SuccessObject resultVariable="result" expression="MessagePackObject.Nil" isAsync="False" withTypeVariable="False">
					result = MessagePackObject.Nil;
					this.Data = result;
					return true;
					// </SuccessObject>
				}
				case ReadValueResult.Boolean:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <SuccessObject resultVariable="result" expression="integral != 0" isAsync="False" withTypeVariable="True">
					result = integral != 0;
					this.Data = result;
					return true;
					// </SuccessObject>
				}
				case ReadValueResult.SByte:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <SuccessObject resultVariable="result" expression="unchecked( ( SByte )integral )" isAsync="False" withTypeVariable="True">
					result = unchecked( ( SByte )integral );
					this.Data = result;
					return true;
					// </SuccessObject>
				}
				case ReadValueResult.Int16:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <SuccessObject resultVariable="result" expression="unchecked( ( Int16 )integral )" isAsync="False" withTypeVariable="True">
					result = unchecked( ( Int16 )integral );
					this.Data = result;
					return true;
					// </SuccessObject>
				}
				case ReadValueResult.Int32:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <SuccessObject resultVariable="result" expression="unchecked( ( Int32 )integral )" isAsync="False" withTypeVariable="True">
					result = unchecked( ( Int32 )integral );
					this.Data = result;
					return true;
					// </SuccessObject>
				}
				case ReadValueResult.Int64:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <SuccessObject resultVariable="result" expression="integral" isAsync="False" withTypeVariable="True">
					result = integral;
					this.Data = result;
					return true;
					// </SuccessObject>
				}
				case ReadValueResult.Byte:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <SuccessObject resultVariable="result" expression="unchecked( ( Byte )integral )" isAsync="False" withTypeVariable="True">
					result = unchecked( ( Byte )integral );
					this.Data = result;
					return true;
					// </SuccessObject>
				}
				case ReadValueResult.UInt16:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <SuccessObject resultVariable="result" expression="unchecked( ( UInt16 )integral )" isAsync="False" withTypeVariable="True">
					result = unchecked( ( UInt16 )integral );
					this.Data = result;
					return true;
					// </SuccessObject>
				}
				case ReadValueResult.UInt32:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <SuccessObject resultVariable="result" expression="unchecked( ( UInt32 )integral )" isAsync="False" withTypeVariable="True">
					result = unchecked( ( UInt32 )integral );
					this.Data = result;
					return true;
					// </SuccessObject>
				}
				case ReadValueResult.UInt64:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <SuccessObject resultVariable="result" expression="unchecked( ( UInt64 )integral )" isAsync="False" withTypeVariable="True">
					result = unchecked( ( UInt64 )integral );
					this.Data = result;
					return true;
					// </SuccessObject>
				}
				case ReadValueResult.Single:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <SuccessObject resultVariable="result" expression="real32" isAsync="False" withTypeVariable="True">
					result = real32;
					this.Data = result;
					return true;
					// </SuccessObject>
				}
				case ReadValueResult.Double:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <SuccessObject resultVariable="result" expression="real64" isAsync="False" withTypeVariable="True">
					result = real64;
					this.Data = result;
					return true;
					// </SuccessObject>
				}
				case ReadValueResult.ArrayLength:
				{
					var length = unchecked( ( UInt32 )this.ReadArrayLengthCore( integral ) );
					if ( !isDeep )
					{
						// <SuccessObject resultVariable="result" expression="length" isAsync="False" withTypeVariable="True">
						result = length;
						this.Data = result;
						return true;
						// </SuccessObject>
					}
				
					this.CheckLength( length, ReadValueResult.ArrayLength );
					var collection = new List<MessagePackObject>( unchecked( ( int ) length ) );
					for( var i = 0; i < length; i++ )
					{
						MessagePackObject item;
						if( !this.ReadObject( /* isDeep */true, out item ) )
						{
							// <Fail resultVariable="result" typeName="MessagePackObject" isAsync="False">
							result = default( MessagePackObject );
							return false;
							// </Fail>
						}
				
						collection.Add( item );
					}
				
					{
						// <SuccessObject resultVariable="result" expression="new MessagePackObject( collection, /* isImmutable */true )" isAsync="False" withTypeVariable="True">
						result = new MessagePackObject( collection, /* isImmutable */true );
						this.Data = result;
						return true;
						// </SuccessObject>
					}
				}
				case ReadValueResult.MapLength:
				{
					var length = unchecked( ( UInt32 )this.ReadMapLengthCore( integral ) );
					if ( !isDeep )
					{
						// <SuccessObject resultVariable="result" expression="length" isAsync="False" withTypeVariable="True">
						result = length;
						this.Data = result;
						return true;
						// </SuccessObject>
					}
				
					this.CheckLength( length, ReadValueResult.MapLength );
					var collection = new MessagePackObjectDictionary( unchecked( ( int ) length ) );
					for( var i = 0; i < length; i++ )
					{
						MessagePackObject key;
						if( !this.ReadObject( /* isDeep */true, out key ) )
						{
							// <Fail resultVariable="result" typeName="MessagePackObject" isAsync="False">
							result = default( MessagePackObject );
							return false;
							// </Fail>
						}
				
						MessagePackObject value;
						if( !this.ReadObject( /* isDeep */true, out value ) )
						{
							// <Fail resultVariable="result" typeName="MessagePackObject" isAsync="False">
							result = default( MessagePackObject );
							return false;
							// </Fail>
						}
				
						collection.Add( key, value );
					}
				
					{
						// <SuccessObject resultVariable="result" expression="new MessagePackObject( collection, /* isImmutable */true )" isAsync="False" withTypeVariable="True">
						result = new MessagePackObject( collection, /* isImmutable */true );
						this.Data = result;
						return true;
						// </SuccessObject>
					}
				}
				
				case ReadValueResult.String:
				{
					// <SuccessObject resultVariable="result" expression="new MessagePackObject( new MessagePackString( this.ReadBinaryCore( integral ), false ) )" isAsync="False" withTypeVariable="False">
					result = new MessagePackObject( new MessagePackString( this.ReadBinaryCore( integral ), false ) );
					this.Data = result;
					return true;
					// </SuccessObject>
				}
				case ReadValueResult.Binary:
				{
					// <SuccessObject resultVariable="result" expression="new MessagePackObject( new MessagePackString( this.ReadBinaryCore( integral ), true ) )" isAsync="False" withTypeVariable="False">
					result = new MessagePackObject( new MessagePackString( this.ReadBinaryCore( integral ), true ) );
					this.Data = result;
					return true;
					// </SuccessObject>
				}
				// <WriteReadExtCore typeVariable="type" resultVariable="result" typeName="MessagePackObject" isAsync="False">
				case ReadValueResult.FixExt1:
				case ReadValueResult.FixExt2:
				case ReadValueResult.FixExt4:
				case ReadValueResult.FixExt8:
				case ReadValueResult.FixExt16:
				case ReadValueResult.Ext8:
				case ReadValueResult.Ext16:
				case ReadValueResult.Ext32:
				{
					// <SuccessObject resultVariable="result" expression="this.ReadMessagePackExtendedTypeObjectCore( type )" isAsync="False" withTypeVariable="True">
					result = this.ReadMessagePackExtendedTypeObjectCore( type );
					this.Data = result;
					return true;
					// </SuccessObject>
				}
				// </WriteReadExtCore>
				// </WriteReadObjectCore>
				default:
				{
					this.ThrowTypeException( typeof( MessagePackObject ), header );
					// Never reach
					// <Fail resultVariable="result" typeName="MessagePackObject" isAsync="False">
					result = default( MessagePackObject );
					return false;
					// </Fail>
				}
			}
			// </WriteReadBodyCore>
		} 
		// </WriteReadBody>
		// <WriteReadBody typeName="Object" isAsync="True">

#if FEATURE_TAP

		public async override Task<AsyncReadResult<MessagePackObject>> ReadObjectAsync( bool isDeep, CancellationToken cancellationToken )
		{
			// <WriteReadBodyCore typeName="MessagePackObject" nullability="Value" isAsync="True">
			var asyncResult = await this.ReadValueAsync( cancellationToken ).ConfigureAwait( false );
			var type = asyncResult.type;
			switch( type )
			{
				case ReadValueResult.Eof:
				{
					// <Fail resultVariable="result" typeName="MessagePackObject" isAsync="True">
					return AsyncReadResult.Fail<MessagePackObject>();
					// </Fail>
				}
				// <WriteReadObjectCore typeVariable="type" valueVariable="i: integral, r4: real32, r8: real64, a: asyncResult" resultVariable="result" isAsync="True">
				case ReadValueResult.Nil:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <SuccessObject resultVariable="result" expression="MessagePackObject.Nil" isAsync="True" withTypeVariable="False">
					var result = MessagePackObject.Nil;
					this.Data = result;
					return AsyncReadResult.Success( result );
					// </SuccessObject>
				}
				case ReadValueResult.Boolean:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <SuccessObject resultVariable="result" expression="asyncResult.integral != 0" isAsync="True" withTypeVariable="True">
					var result = asyncResult.integral != 0;
					this.Data = result;
					return AsyncReadResult.Success<MessagePackObject>( result );
					// </SuccessObject>
				}
				case ReadValueResult.SByte:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <SuccessObject resultVariable="result" expression="unchecked( ( SByte )asyncResult.integral )" isAsync="True" withTypeVariable="True">
					var result = unchecked( ( SByte )asyncResult.integral );
					this.Data = result;
					return AsyncReadResult.Success<MessagePackObject>( result );
					// </SuccessObject>
				}
				case ReadValueResult.Int16:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <SuccessObject resultVariable="result" expression="unchecked( ( Int16 )asyncResult.integral )" isAsync="True" withTypeVariable="True">
					var result = unchecked( ( Int16 )asyncResult.integral );
					this.Data = result;
					return AsyncReadResult.Success<MessagePackObject>( result );
					// </SuccessObject>
				}
				case ReadValueResult.Int32:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <SuccessObject resultVariable="result" expression="unchecked( ( Int32 )asyncResult.integral )" isAsync="True" withTypeVariable="True">
					var result = unchecked( ( Int32 )asyncResult.integral );
					this.Data = result;
					return AsyncReadResult.Success<MessagePackObject>( result );
					// </SuccessObject>
				}
				case ReadValueResult.Int64:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <SuccessObject resultVariable="result" expression="asyncResult.integral" isAsync="True" withTypeVariable="True">
					var result = asyncResult.integral;
					this.Data = result;
					return AsyncReadResult.Success<MessagePackObject>( result );
					// </SuccessObject>
				}
				case ReadValueResult.Byte:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <SuccessObject resultVariable="result" expression="unchecked( ( Byte )asyncResult.integral )" isAsync="True" withTypeVariable="True">
					var result = unchecked( ( Byte )asyncResult.integral );
					this.Data = result;
					return AsyncReadResult.Success<MessagePackObject>( result );
					// </SuccessObject>
				}
				case ReadValueResult.UInt16:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <SuccessObject resultVariable="result" expression="unchecked( ( UInt16 )asyncResult.integral )" isAsync="True" withTypeVariable="True">
					var result = unchecked( ( UInt16 )asyncResult.integral );
					this.Data = result;
					return AsyncReadResult.Success<MessagePackObject>( result );
					// </SuccessObject>
				}
				case ReadValueResult.UInt32:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <SuccessObject resultVariable="result" expression="unchecked( ( UInt32 )asyncResult.integral )" isAsync="True" withTypeVariable="True">
					var result = unchecked( ( UInt32 )asyncResult.integral );
					this.Data = result;
					return AsyncReadResult.Success<MessagePackObject>( result );
					// </SuccessObject>
				}
				case ReadValueResult.UInt64:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <SuccessObject resultVariable="result" expression="unchecked( ( UInt64 )asyncResult.integral )" isAsync="True" withTypeVariable="True">
					var result = unchecked( ( UInt64 )asyncResult.integral );
					this.Data = result;
					return AsyncReadResult.Success<MessagePackObject>( result );
					// </SuccessObject>
				}
				case ReadValueResult.Single:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <SuccessObject resultVariable="result" expression="asyncResult.real32" isAsync="True" withTypeVariable="True">
					var result = asyncResult.real32;
					this.Data = result;
					return AsyncReadResult.Success<MessagePackObject>( result );
					// </SuccessObject>
				}
				case ReadValueResult.Double:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <SuccessObject resultVariable="result" expression="asyncResult.real64" isAsync="True" withTypeVariable="True">
					var result = asyncResult.real64;
					this.Data = result;
					return AsyncReadResult.Success<MessagePackObject>( result );
					// </SuccessObject>
				}
				case ReadValueResult.ArrayLength:
				{
					// ReadArrayLengthCore does not perform I/O, so no ReadArrayLengthAsyncCore exists.
					var length = unchecked( ( UInt32 )this.ReadArrayLengthCore( asyncResult.integral ) );
					if ( !isDeep )
					{
						// <SuccessObject resultVariable="result" expression="length" isAsync="True" withTypeVariable="True">
						var result = length;
						this.Data = result;
						return AsyncReadResult.Success<MessagePackObject>( result );
						// </SuccessObject>
					}
				
					this.CheckLength( length, ReadValueResult.ArrayLength );
					var collection = new List<MessagePackObject>( unchecked( ( int ) length ) );
					for( var i = 0; i < length; i++ )
					{
						MessagePackObject item;
						if( !this.ReadObject( /* isDeep */true, out item ) )
						{
							// <Fail resultVariable="result" typeName="MessagePackObject" isAsync="True">
							return AsyncReadResult.Fail<MessagePackObject>();
							// </Fail>
						}
				
						collection.Add( item );
					}
				
					{
						// <SuccessObject resultVariable="result" expression="new MessagePackObject( collection, /* isImmutable */true )" isAsync="True" withTypeVariable="True">
						var result = new MessagePackObject( collection, /* isImmutable */true );
						this.Data = result;
						return AsyncReadResult.Success<MessagePackObject>( result );
						// </SuccessObject>
					}
				}
				case ReadValueResult.MapLength:
				{
					// ReadMapLengthCore does not perform I/O, so no ReadMapLengthAsyncCore exists.
					var length = unchecked( ( UInt32 )this.ReadMapLengthCore( asyncResult.integral ) );
					if ( !isDeep )
					{
						// <SuccessObject resultVariable="result" expression="length" isAsync="True" withTypeVariable="True">
						var result = length;
						this.Data = result;
						return AsyncReadResult.Success<MessagePackObject>( result );
						// </SuccessObject>
					}
				
					this.CheckLength( length, ReadValueResult.MapLength );
					var collection = new MessagePackObjectDictionary( unchecked( ( int ) length ) );
					for( var i = 0; i < length; i++ )
					{
						MessagePackObject key;
						if( !this.ReadObject( /* isDeep */true, out key ) )
						{
							// <Fail resultVariable="result" typeName="MessagePackObject" isAsync="True">
							return AsyncReadResult.Fail<MessagePackObject>();
							// </Fail>
						}
				
						MessagePackObject value;
						if( !this.ReadObject( /* isDeep */true, out value ) )
						{
							// <Fail resultVariable="result" typeName="MessagePackObject" isAsync="True">
							return AsyncReadResult.Fail<MessagePackObject>();
							// </Fail>
						}
				
						collection.Add( key, value );
					}
				
					{
						// <SuccessObject resultVariable="result" expression="new MessagePackObject( collection, /* isImmutable */true )" isAsync="True" withTypeVariable="True">
						var result = new MessagePackObject( collection, /* isImmutable */true );
						this.Data = result;
						return AsyncReadResult.Success<MessagePackObject>( result );
						// </SuccessObject>
					}
				}
				
				case ReadValueResult.String:
				{
					// <SuccessObject resultVariable="result" expression="new MessagePackObject( new MessagePackString( await this.ReadBinaryAsyncCore( asyncResult.integral, cancellationToken ).ConfigureAwait( false ), false ) )" isAsync="True" withTypeVariable="False">
					var result = new MessagePackObject( new MessagePackString( await this.ReadBinaryAsyncCore( asyncResult.integral, cancellationToken ).ConfigureAwait( false ), false ) );
					this.Data = result;
					return AsyncReadResult.Success( result );
					// </SuccessObject>
				}
				case ReadValueResult.Binary:
				{
					// <SuccessObject resultVariable="result" expression="new MessagePackObject( new MessagePackString( await this.ReadBinaryAsyncCore( asyncResult.integral, cancellationToken ).ConfigureAwait( false ), true ) )" isAsync="True" withTypeVariable="False">
					var result = new MessagePackObject( new MessagePackString( await this.ReadBinaryAsyncCore( asyncResult.integral, cancellationToken ).ConfigureAwait( false ), true ) );
					this.Data = result;
					return AsyncReadResult.Success( result );
					// </SuccessObject>
				}
				// <WriteReadExtCore typeVariable="type" resultVariable="result" typeName="MessagePackObject" isAsync="True">
				case ReadValueResult.FixExt1:
				case ReadValueResult.FixExt2:
				case ReadValueResult.FixExt4:
				case ReadValueResult.FixExt8:
				case ReadValueResult.FixExt16:
				case ReadValueResult.Ext8:
				case ReadValueResult.Ext16:
				case ReadValueResult.Ext32:
				{
					// <SuccessObject resultVariable="result" expression="await this.ReadMessagePackExtendedTypeObjectAsyncCore( type, cancellationToken ).ConfigureAwait( false )" isAsync="True" withTypeVariable="True">
					var result = await this.ReadMessagePackExtendedTypeObjectAsyncCore( type, cancellationToken ).ConfigureAwait( false );
					this.Data = result;
					return AsyncReadResult.Success<MessagePackObject>( result );
					// </SuccessObject>
				}
				// </WriteReadExtCore>
				// </WriteReadObjectCore>
				default:
				{
					this.ThrowTypeException( typeof( MessagePackObject ), asyncResult.header );
					// Never reach
					// <Fail resultVariable="result" typeName="MessagePackObject" isAsync="True">
					return AsyncReadResult.Fail<MessagePackObject>();
					// </Fail>
				}
			}
			// </WriteReadBodyCore>
		} 

#endif // FEATURE_TAP

		// </WriteReadBody>
		// <WriteReadBody typeName="ArrayLength" isAsync="False">
		public override bool ReadArrayLength( out Int64 result )
		{
			// <WriteReadBodyCore typeName="Int64" nullability="Value" isAsync="False">
			byte header;
			long integral;
			float real32;
			double real64;
			var type = this.ReadValue( out header, out integral, out real32, out real64 );
			switch( type )
			{
				case ReadValueResult.Eof:
				{
					// <Fail resultVariable="result" typeName="Int64" isAsync="False">
					result = default( Int64 );
					return false;
					// </Fail>
				}
				// <WriteReadLengthCore code="ArrayLength" valueVariable="i: integral, r4: real32, r8: real64, a: asyncResult" resultVariable="result" isAsync="False">
				case ReadValueResult.ArrayLength:
				{
					// <SuccessWithLengthCheck code="ArrayLength" resultVariable="result" expression="this.ReadArrayLengthCore( integral)" isAsync="False">
					result = this.ReadArrayLengthCore( integral);
					this.CheckLength( result, ReadValueResult.ArrayLength );
					return true;
					// </SuccessWithLengthCheck>
				}
				// </WriteReadLengthCore>
				default:
				{
					this.ThrowTypeException( typeof( Int64 ), header );
					// Never reach
					// <Fail resultVariable="result" typeName="Int64" isAsync="False">
					result = default( Int64 );
					return false;
					// </Fail>
				}
			}
			// </WriteReadBodyCore>
		} 
		// </WriteReadBody>
		// <WriteReadBody typeName="ArrayLength" isAsync="True">

#if FEATURE_TAP

		public async override Task<AsyncReadResult<Int64>> ReadArrayLengthAsync( CancellationToken cancellationToken )
		{
			// <WriteReadBodyCore typeName="Int64" nullability="Value" isAsync="True">
			var asyncResult = await this.ReadValueAsync( cancellationToken ).ConfigureAwait( false );
			var type = asyncResult.type;
			switch( type )
			{
				case ReadValueResult.Eof:
				{
					// <Fail resultVariable="result" typeName="Int64" isAsync="True">
					return AsyncReadResult.Fail<Int64>();
					// </Fail>
				}
				// <WriteReadLengthCore code="ArrayLength" valueVariable="i: integral, r4: real32, r8: real64, a: asyncResult" resultVariable="result" isAsync="True">
				case ReadValueResult.ArrayLength:
				{
					// ReadArrayLengthCore does not perform I/O, so no ReadArrayLengthAsyncCore exists.
					// <SuccessWithLengthCheck code="ArrayLength" resultVariable="result" expression="this.ReadArrayLengthCore( asyncResult.integral)" isAsync="True">
					var result = this.ReadArrayLengthCore( asyncResult.integral);
					this.CheckLength( result, ReadValueResult.ArrayLength );
					return AsyncReadResult.Success( result );
					// </SuccessWithLengthCheck>
				}
				// </WriteReadLengthCore>
				default:
				{
					this.ThrowTypeException( typeof( Int64 ), asyncResult.header );
					// Never reach
					// <Fail resultVariable="result" typeName="Int64" isAsync="True">
					return AsyncReadResult.Fail<Int64>();
					// </Fail>
				}
			}
			// </WriteReadBodyCore>
		} 

#endif // FEATURE_TAP

		// </WriteReadBody>
		// <WriteReadBody typeName="MapLength" isAsync="False">
		public override bool ReadMapLength( out Int64 result )
		{
			// <WriteReadBodyCore typeName="Int64" nullability="Value" isAsync="False">
			byte header;
			long integral;
			float real32;
			double real64;
			var type = this.ReadValue( out header, out integral, out real32, out real64 );
			switch( type )
			{
				case ReadValueResult.Eof:
				{
					// <Fail resultVariable="result" typeName="Int64" isAsync="False">
					result = default( Int64 );
					return false;
					// </Fail>
				}
				// <WriteReadLengthCore code="MapLength" valueVariable="i: integral, r4: real32, r8: real64, a: asyncResult" resultVariable="result" isAsync="False">
				case ReadValueResult.MapLength:
				{
					// <SuccessWithLengthCheck code="MapLength" resultVariable="result" expression="this.ReadMapLengthCore( integral)" isAsync="False">
					result = this.ReadMapLengthCore( integral);
					this.CheckLength( result, ReadValueResult.MapLength );
					return true;
					// </SuccessWithLengthCheck>
				}
				// </WriteReadLengthCore>
				default:
				{
					this.ThrowTypeException( typeof( Int64 ), header );
					// Never reach
					// <Fail resultVariable="result" typeName="Int64" isAsync="False">
					result = default( Int64 );
					return false;
					// </Fail>
				}
			}
			// </WriteReadBodyCore>
		} 
		// </WriteReadBody>
		// <WriteReadBody typeName="MapLength" isAsync="True">

#if FEATURE_TAP

		public async override Task<AsyncReadResult<Int64>> ReadMapLengthAsync( CancellationToken cancellationToken )
		{
			// <WriteReadBodyCore typeName="Int64" nullability="Value" isAsync="True">
			var asyncResult = await this.ReadValueAsync( cancellationToken ).ConfigureAwait( false );
			var type = asyncResult.type;
			switch( type )
			{
				case ReadValueResult.Eof:
				{
					// <Fail resultVariable="result" typeName="Int64" isAsync="True">
					return AsyncReadResult.Fail<Int64>();
					// </Fail>
				}
				// <WriteReadLengthCore code="MapLength" valueVariable="i: integral, r4: real32, r8: real64, a: asyncResult" resultVariable="result" isAsync="True">
				case ReadValueResult.MapLength:
				{
					// ReadMapLengthCore does not perform I/O, so no ReadMapLengthAsyncCore exists.
					// <SuccessWithLengthCheck code="MapLength" resultVariable="result" expression="this.ReadMapLengthCore( asyncResult.integral)" isAsync="True">
					var result = this.ReadMapLengthCore( asyncResult.integral);
					this.CheckLength( result, ReadValueResult.MapLength );
					return AsyncReadResult.Success( result );
					// </SuccessWithLengthCheck>
				}
				// </WriteReadLengthCore>
				default:
				{
					this.ThrowTypeException( typeof( Int64 ), asyncResult.header );
					// Never reach
					// <Fail resultVariable="result" typeName="Int64" isAsync="True">
					return AsyncReadResult.Fail<Int64>();
					// </Fail>
				}
			}
			// </WriteReadBodyCore>
		} 

#endif // FEATURE_TAP

		// </WriteReadBody>
		// <WriteReadBody typeName="MessagePackExtendedTypeObject" isAsync="False">
		public override bool ReadMessagePackExtendedTypeObject( out MessagePackExtendedTypeObject result )
		{
			// <WriteReadBodyCore typeName="MessagePackExtendedTypeObject" nullability="Value" isAsync="False">
			byte header;
			long integral;
			float real32;
			double real64;
			var type = this.ReadValue( out header, out integral, out real32, out real64 );
			switch( type )
			{
				case ReadValueResult.Eof:
				{
					// <Fail resultVariable="result" typeName="MessagePackExtendedTypeObject" isAsync="False">
					result = default( MessagePackExtendedTypeObject );
					return false;
					// </Fail>
				}
				// <WriteReadExtCore typeVariable="type" resultVariable="result" typeName="MessagePackExtendedTypeObject" isAsync="False">
				case ReadValueResult.FixExt1:
				case ReadValueResult.FixExt2:
				case ReadValueResult.FixExt4:
				case ReadValueResult.FixExt8:
				case ReadValueResult.FixExt16:
				case ReadValueResult.Ext8:
				case ReadValueResult.Ext16:
				case ReadValueResult.Ext32:
				{
					// <Success type="MessagePackExtendedTypeObject" resultVariable="result" expression="this.ReadMessagePackExtendedTypeObjectCore( type )" isAsync="False">
					result = this.ReadMessagePackExtendedTypeObjectCore( type );
					return true;
					// </Success>
				}
				// </WriteReadExtCore>
				default:
				{
					this.ThrowTypeException( typeof( MessagePackExtendedTypeObject ), header );
					// Never reach
					// <Fail resultVariable="result" typeName="MessagePackExtendedTypeObject" isAsync="False">
					result = default( MessagePackExtendedTypeObject );
					return false;
					// </Fail>
				}
			}
			// </WriteReadBodyCore>
		} 
		// </WriteReadBody>
		// <WriteReadBody typeName="MessagePackExtendedTypeObject" isAsync="True">

#if FEATURE_TAP

		public async override Task<AsyncReadResult<MessagePackExtendedTypeObject>> ReadMessagePackExtendedTypeObjectAsync( CancellationToken cancellationToken )
		{
			// <WriteReadBodyCore typeName="MessagePackExtendedTypeObject" nullability="Value" isAsync="True">
			var asyncResult = await this.ReadValueAsync( cancellationToken ).ConfigureAwait( false );
			var type = asyncResult.type;
			switch( type )
			{
				case ReadValueResult.Eof:
				{
					// <Fail resultVariable="result" typeName="MessagePackExtendedTypeObject" isAsync="True">
					return AsyncReadResult.Fail<MessagePackExtendedTypeObject>();
					// </Fail>
				}
				// <WriteReadExtCore typeVariable="type" resultVariable="result" typeName="MessagePackExtendedTypeObject" isAsync="True">
				case ReadValueResult.FixExt1:
				case ReadValueResult.FixExt2:
				case ReadValueResult.FixExt4:
				case ReadValueResult.FixExt8:
				case ReadValueResult.FixExt16:
				case ReadValueResult.Ext8:
				case ReadValueResult.Ext16:
				case ReadValueResult.Ext32:
				{
					// <Success type="MessagePackExtendedTypeObject" resultVariable="result" expression="await this.ReadMessagePackExtendedTypeObjectAsyncCore( type, cancellationToken ).ConfigureAwait( false )" isAsync="True">
					return AsyncReadResult.Success<MessagePackExtendedTypeObject>( await this.ReadMessagePackExtendedTypeObjectAsyncCore( type, cancellationToken ).ConfigureAwait( false ) );
					// </Success>
				}
				// </WriteReadExtCore>
				default:
				{
					this.ThrowTypeException( typeof( MessagePackExtendedTypeObject ), asyncResult.header );
					// Never reach
					// <Fail resultVariable="result" typeName="MessagePackExtendedTypeObject" isAsync="True">
					return AsyncReadResult.Fail<MessagePackExtendedTypeObject>();
					// </Fail>
				}
			}
			// </WriteReadBodyCore>
		} 

#endif // FEATURE_TAP

		// </WriteReadBody>
		// <WriteReadBody typeName="NullableMessagePackExtendedTypeObject" isAsync="False">
		public override bool ReadNullableMessagePackExtendedTypeObject( out MessagePackExtendedTypeObject? result )
		{
			// <WriteReadBodyCore typeName="MessagePackExtendedTypeObject" nullability="Nullable" isAsync="False">
			byte header;
			long integral;
			float real32;
			double real64;
			var type = this.ReadValue( out header, out integral, out real32, out real64 );
			switch( type )
			{
				case ReadValueResult.Eof:
				{
					// <Fail resultVariable="result" typeName="MessagePackExtendedTypeObject?" isAsync="False">
					result = default( MessagePackExtendedTypeObject? );
					return false;
					// </Fail>
				}
				case ReadValueResult.Nil:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="default( MessagePackExtendedTypeObject? )" isAsync="False">
					result = default( MessagePackExtendedTypeObject? );
					return true;
					// </Success>
				}
				// <WriteReadExtCore typeVariable="type" resultVariable="result" typeName="MessagePackExtendedTypeObject?" isAsync="False">
				case ReadValueResult.FixExt1:
				case ReadValueResult.FixExt2:
				case ReadValueResult.FixExt4:
				case ReadValueResult.FixExt8:
				case ReadValueResult.FixExt16:
				case ReadValueResult.Ext8:
				case ReadValueResult.Ext16:
				case ReadValueResult.Ext32:
				{
					// <Success type="MessagePackExtendedTypeObject?" resultVariable="result" expression="this.ReadMessagePackExtendedTypeObjectCore( type )" isAsync="False">
					result = this.ReadMessagePackExtendedTypeObjectCore( type );
					return true;
					// </Success>
				}
				// </WriteReadExtCore>
				default:
				{
					this.ThrowTypeException( typeof( MessagePackExtendedTypeObject? ), header );
					// Never reach
					// <Fail resultVariable="result" typeName="MessagePackExtendedTypeObject?" isAsync="False">
					result = default( MessagePackExtendedTypeObject? );
					return false;
					// </Fail>
				}
			}
			// </WriteReadBodyCore>
		} 
		// </WriteReadBody>
		// <WriteReadBody typeName="NullableMessagePackExtendedTypeObject" isAsync="True">

#if FEATURE_TAP

		public async override Task<AsyncReadResult<MessagePackExtendedTypeObject?>> ReadNullableMessagePackExtendedTypeObjectAsync( CancellationToken cancellationToken )
		{
			// <WriteReadBodyCore typeName="MessagePackExtendedTypeObject" nullability="Nullable" isAsync="True">
			var asyncResult = await this.ReadValueAsync( cancellationToken ).ConfigureAwait( false );
			var type = asyncResult.type;
			switch( type )
			{
				case ReadValueResult.Eof:
				{
					// <Fail resultVariable="result" typeName="MessagePackExtendedTypeObject?" isAsync="True">
					return AsyncReadResult.Fail<MessagePackExtendedTypeObject?>();
					// </Fail>
				}
				case ReadValueResult.Nil:
				{
					// <OnReturnScalar>
					this.CollectionType = CollectionType.None;
					// </OnReturnScalar>
					// <Success type="(null)" resultVariable="result" expression="default( MessagePackExtendedTypeObject? )" isAsync="True">
					return AsyncReadResult.Success( default( MessagePackExtendedTypeObject? ) );
					// </Success>
				}
				// <WriteReadExtCore typeVariable="type" resultVariable="result" typeName="MessagePackExtendedTypeObject?" isAsync="True">
				case ReadValueResult.FixExt1:
				case ReadValueResult.FixExt2:
				case ReadValueResult.FixExt4:
				case ReadValueResult.FixExt8:
				case ReadValueResult.FixExt16:
				case ReadValueResult.Ext8:
				case ReadValueResult.Ext16:
				case ReadValueResult.Ext32:
				{
					// <Success type="MessagePackExtendedTypeObject?" resultVariable="result" expression="await this.ReadMessagePackExtendedTypeObjectAsyncCore( type, cancellationToken ).ConfigureAwait( false )" isAsync="True">
					return AsyncReadResult.Success<MessagePackExtendedTypeObject?>( await this.ReadMessagePackExtendedTypeObjectAsyncCore( type, cancellationToken ).ConfigureAwait( false ) );
					// </Success>
				}
				// </WriteReadExtCore>
				default:
				{
					this.ThrowTypeException( typeof( MessagePackExtendedTypeObject? ), asyncResult.header );
					// Never reach
					// <Fail resultVariable="result" typeName="MessagePackExtendedTypeObject?" isAsync="True">
					return AsyncReadResult.Fail<MessagePackExtendedTypeObject?>();
					// </Fail>
				}
			}
			// </WriteReadBodyCore>
		} 

#endif // FEATURE_TAP

		// </WriteReadBody>
	}
}

