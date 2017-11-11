#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2016 FUJIWARA, Yusuke
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
using System.Globalization;
using System.Runtime.Serialization;
#if FEATURE_TAP
using System.Threading;
using System.Threading.Tasks;
#endif // FEATURE_TAP

namespace MsgPack.Serialization.DefaultSerializers
{
	// This file generated from DefaultSerializers.tt T4Template.
	// Do not modify this file. Edit DefaultSerializers.tt instead.

	// ReSharper disable InconsistentNaming
	// ReSharper disable RedundantNameQualifier
	// ReSharper disable RedundantCast

	internal sealed class System_BooleanMessagePackSerializer : MessagePackSerializer< System.Boolean >
	{
		public System_BooleanMessagePackSerializer( SerializationContext ownerContext )
			: base( ownerContext, SerializerCapabilities.PackTo | SerializerCapabilities.UnpackFrom ) { }

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override void PackToCore( Packer packer, System.Boolean value )
		{
			packer.Pack( value );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override System.Boolean UnpackFromCore( Unpacker unpacker )
		{
			try
			{
				return unpacker.LastReadData.AsBoolean();
			}
			catch( InvalidOperationException ex )
			{
				throw new SerializationException( String.Format( CultureInfo.CurrentCulture, "The unpacked value is not '{0}' type. {1}", typeof( System.Boolean ), ex.Message ) );
			}
		}

#if FEATURE_TAP

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override async Task PackToAsyncCore( Packer packer, System.Boolean value, CancellationToken cancellationToken )
		{
			await packer.PackAsync( value, cancellationToken ).ConfigureAwait( false );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Transfers all catched exceptions." )]
		protected internal override Task<System.Boolean> UnpackFromAsyncCore( Unpacker unpacker, CancellationToken cancellationToken )
		{
			var tcs = new TaskCompletionSource<System.Boolean>();
			try
			{
				tcs.SetResult( this.UnpackFromCore( unpacker ) );
			}
			catch( Exception ex )
			{
				tcs.SetException( ex );
			}

			return tcs.Task;
		}

#endif // FEATURE_TAP

	}

	internal sealed class System_ByteMessagePackSerializer : MessagePackSerializer< System.Byte >
	{
		public System_ByteMessagePackSerializer( SerializationContext ownerContext )
			: base( ownerContext, SerializerCapabilities.PackTo | SerializerCapabilities.UnpackFrom ) { }

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override void PackToCore( Packer packer, System.Byte value )
		{
			packer.Pack( value );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override System.Byte UnpackFromCore( Unpacker unpacker )
		{
			try
			{
				return unpacker.LastReadData.AsByte();
			}
			catch( InvalidOperationException ex )
			{
				throw new SerializationException( String.Format( CultureInfo.CurrentCulture, "The unpacked value is not '{0}' type. {1}", typeof( System.Byte ), ex.Message ) );
			}
		}

#if FEATURE_TAP

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override async Task PackToAsyncCore( Packer packer, System.Byte value, CancellationToken cancellationToken )
		{
			await packer.PackAsync( value, cancellationToken ).ConfigureAwait( false );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Transfers all catched exceptions." )]
		protected internal override Task<System.Byte> UnpackFromAsyncCore( Unpacker unpacker, CancellationToken cancellationToken )
		{
			var tcs = new TaskCompletionSource<System.Byte>();
			try
			{
				tcs.SetResult( this.UnpackFromCore( unpacker ) );
			}
			catch( Exception ex )
			{
				tcs.SetException( ex );
			}

			return tcs.Task;
		}

#endif // FEATURE_TAP

	}

	internal sealed class System_CharMessagePackSerializer : MessagePackSerializer< System.Char >
	{
		public System_CharMessagePackSerializer( SerializationContext ownerContext )
			: base( ownerContext, SerializerCapabilities.PackTo | SerializerCapabilities.UnpackFrom ) { }

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override void PackToCore( Packer packer, System.Char value )
		{
			packer.Pack( ( System.UInt16 )value );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override System.Char UnpackFromCore( Unpacker unpacker )
		{
			try
			{
				return ( System.Char ) unpacker.LastReadData.AsUInt16(); 
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

#if FEATURE_TAP

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override async Task PackToAsyncCore( Packer packer, System.Char value, CancellationToken cancellationToken )
		{
			await packer.PackAsync( ( System.UInt16 )value, cancellationToken ).ConfigureAwait( false );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Transfers all catched exceptions." )]
		protected internal override Task<System.Char> UnpackFromAsyncCore( Unpacker unpacker, CancellationToken cancellationToken )
		{
			var tcs = new TaskCompletionSource<System.Char>();
			try
			{
				tcs.SetResult( this.UnpackFromCore( unpacker ) );
			}
			catch( Exception ex )
			{
				tcs.SetException( ex );
			}

			return tcs.Task;
		}

#endif // FEATURE_TAP

	}

	internal sealed class System_DecimalMessagePackSerializer : MessagePackSerializer< System.Decimal >
	{
		public System_DecimalMessagePackSerializer( SerializationContext ownerContext )
			: base( ownerContext, SerializerCapabilities.PackTo | SerializerCapabilities.UnpackFrom ) { }

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override void PackToCore( Packer packer, System.Decimal value )
		{
			packer.PackString( value.ToString( "G", CultureInfo.InvariantCulture ) );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override System.Decimal UnpackFromCore( Unpacker unpacker )
		{
			try
			{
				return System.Decimal.Parse( unpacker.LastReadData.AsString(), CultureInfo.InvariantCulture ); 
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

#if FEATURE_TAP

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override async Task PackToAsyncCore( Packer packer, System.Decimal value, CancellationToken cancellationToken )
		{
			await packer.PackStringAsync( value.ToString( "G", CultureInfo.InvariantCulture ), cancellationToken ).ConfigureAwait( false );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Transfers all catched exceptions." )]
		protected internal override Task<System.Decimal> UnpackFromAsyncCore( Unpacker unpacker, CancellationToken cancellationToken )
		{
			var tcs = new TaskCompletionSource<System.Decimal>();
			try
			{
				tcs.SetResult( this.UnpackFromCore( unpacker ) );
			}
			catch( Exception ex )
			{
				tcs.SetException( ex );
			}

			return tcs.Task;
		}

#endif // FEATURE_TAP

	}

	internal sealed class System_DoubleMessagePackSerializer : MessagePackSerializer< System.Double >
	{
		public System_DoubleMessagePackSerializer( SerializationContext ownerContext )
			: base( ownerContext, SerializerCapabilities.PackTo | SerializerCapabilities.UnpackFrom ) { }

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override void PackToCore( Packer packer, System.Double value )
		{
			packer.Pack( value );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override System.Double UnpackFromCore( Unpacker unpacker )
		{
			try
			{
				return unpacker.LastReadData.AsDouble();
			}
			catch( InvalidOperationException ex )
			{
				throw new SerializationException( String.Format( CultureInfo.CurrentCulture, "The unpacked value is not '{0}' type. {1}", typeof( System.Double ), ex.Message ) );
			}
		}

#if FEATURE_TAP

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override async Task PackToAsyncCore( Packer packer, System.Double value, CancellationToken cancellationToken )
		{
			await packer.PackAsync( value, cancellationToken ).ConfigureAwait( false );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Transfers all catched exceptions." )]
		protected internal override Task<System.Double> UnpackFromAsyncCore( Unpacker unpacker, CancellationToken cancellationToken )
		{
			var tcs = new TaskCompletionSource<System.Double>();
			try
			{
				tcs.SetResult( this.UnpackFromCore( unpacker ) );
			}
			catch( Exception ex )
			{
				tcs.SetException( ex );
			}

			return tcs.Task;
		}

#endif // FEATURE_TAP

	}

	internal sealed class System_GuidMessagePackSerializer : MessagePackSerializer< System.Guid >
	{
		public System_GuidMessagePackSerializer( SerializationContext ownerContext )
			: base( ownerContext, SerializerCapabilities.PackTo | SerializerCapabilities.UnpackFrom ) { }

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override void PackToCore( Packer packer, System.Guid value )
		{
			packer.PackBinary( value.ToByteArray() );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override System.Guid UnpackFromCore( Unpacker unpacker )
		{
			try
			{
				return new System.Guid( unpacker.LastReadData.AsBinary() ); 
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

#if FEATURE_TAP

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override async Task PackToAsyncCore( Packer packer, System.Guid value, CancellationToken cancellationToken )
		{
			await packer.PackBinaryAsync( value.ToByteArray(), cancellationToken ).ConfigureAwait( false );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Transfers all catched exceptions." )]
		protected internal override Task<System.Guid> UnpackFromAsyncCore( Unpacker unpacker, CancellationToken cancellationToken )
		{
			var tcs = new TaskCompletionSource<System.Guid>();
			try
			{
				tcs.SetResult( this.UnpackFromCore( unpacker ) );
			}
			catch( Exception ex )
			{
				tcs.SetException( ex );
			}

			return tcs.Task;
		}

#endif // FEATURE_TAP

	}

	internal sealed class System_Int16MessagePackSerializer : MessagePackSerializer< System.Int16 >
	{
		public System_Int16MessagePackSerializer( SerializationContext ownerContext )
			: base( ownerContext, SerializerCapabilities.PackTo | SerializerCapabilities.UnpackFrom ) { }

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override void PackToCore( Packer packer, System.Int16 value )
		{
			packer.Pack( value );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override System.Int16 UnpackFromCore( Unpacker unpacker )
		{
			try
			{
				return unpacker.LastReadData.AsInt16();
			}
			catch( InvalidOperationException ex )
			{
				throw new SerializationException( String.Format( CultureInfo.CurrentCulture, "The unpacked value is not '{0}' type. {1}", typeof( System.Int16 ), ex.Message ) );
			}
		}

#if FEATURE_TAP

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override async Task PackToAsyncCore( Packer packer, System.Int16 value, CancellationToken cancellationToken )
		{
			await packer.PackAsync( value, cancellationToken ).ConfigureAwait( false );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Transfers all catched exceptions." )]
		protected internal override Task<System.Int16> UnpackFromAsyncCore( Unpacker unpacker, CancellationToken cancellationToken )
		{
			var tcs = new TaskCompletionSource<System.Int16>();
			try
			{
				tcs.SetResult( this.UnpackFromCore( unpacker ) );
			}
			catch( Exception ex )
			{
				tcs.SetException( ex );
			}

			return tcs.Task;
		}

#endif // FEATURE_TAP

	}

	internal sealed class System_Int32MessagePackSerializer : MessagePackSerializer< System.Int32 >
	{
		public System_Int32MessagePackSerializer( SerializationContext ownerContext )
			: base( ownerContext, SerializerCapabilities.PackTo | SerializerCapabilities.UnpackFrom ) { }

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override void PackToCore( Packer packer, System.Int32 value )
		{
			packer.Pack( value );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override System.Int32 UnpackFromCore( Unpacker unpacker )
		{
			try
			{
				return unpacker.LastReadData.AsInt32();
			}
			catch( InvalidOperationException ex )
			{
				throw new SerializationException( String.Format( CultureInfo.CurrentCulture, "The unpacked value is not '{0}' type. {1}", typeof( System.Int32 ), ex.Message ) );
			}
		}

#if FEATURE_TAP

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override async Task PackToAsyncCore( Packer packer, System.Int32 value, CancellationToken cancellationToken )
		{
			await packer.PackAsync( value, cancellationToken ).ConfigureAwait( false );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Transfers all catched exceptions." )]
		protected internal override Task<System.Int32> UnpackFromAsyncCore( Unpacker unpacker, CancellationToken cancellationToken )
		{
			var tcs = new TaskCompletionSource<System.Int32>();
			try
			{
				tcs.SetResult( this.UnpackFromCore( unpacker ) );
			}
			catch( Exception ex )
			{
				tcs.SetException( ex );
			}

			return tcs.Task;
		}

#endif // FEATURE_TAP

	}

	internal sealed class System_Int64MessagePackSerializer : MessagePackSerializer< System.Int64 >
	{
		public System_Int64MessagePackSerializer( SerializationContext ownerContext )
			: base( ownerContext, SerializerCapabilities.PackTo | SerializerCapabilities.UnpackFrom ) { }

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override void PackToCore( Packer packer, System.Int64 value )
		{
			packer.Pack( value );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override System.Int64 UnpackFromCore( Unpacker unpacker )
		{
			try
			{
				return unpacker.LastReadData.AsInt64();
			}
			catch( InvalidOperationException ex )
			{
				throw new SerializationException( String.Format( CultureInfo.CurrentCulture, "The unpacked value is not '{0}' type. {1}", typeof( System.Int64 ), ex.Message ) );
			}
		}

#if FEATURE_TAP

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override async Task PackToAsyncCore( Packer packer, System.Int64 value, CancellationToken cancellationToken )
		{
			await packer.PackAsync( value, cancellationToken ).ConfigureAwait( false );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Transfers all catched exceptions." )]
		protected internal override Task<System.Int64> UnpackFromAsyncCore( Unpacker unpacker, CancellationToken cancellationToken )
		{
			var tcs = new TaskCompletionSource<System.Int64>();
			try
			{
				tcs.SetResult( this.UnpackFromCore( unpacker ) );
			}
			catch( Exception ex )
			{
				tcs.SetException( ex );
			}

			return tcs.Task;
		}

#endif // FEATURE_TAP

	}

	internal sealed class System_SByteMessagePackSerializer : MessagePackSerializer< System.SByte >
	{
		public System_SByteMessagePackSerializer( SerializationContext ownerContext )
			: base( ownerContext, SerializerCapabilities.PackTo | SerializerCapabilities.UnpackFrom ) { }

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override void PackToCore( Packer packer, System.SByte value )
		{
			packer.Pack( value );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override System.SByte UnpackFromCore( Unpacker unpacker )
		{
			try
			{
				return unpacker.LastReadData.AsSByte();
			}
			catch( InvalidOperationException ex )
			{
				throw new SerializationException( String.Format( CultureInfo.CurrentCulture, "The unpacked value is not '{0}' type. {1}", typeof( System.SByte ), ex.Message ) );
			}
		}

#if FEATURE_TAP

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override async Task PackToAsyncCore( Packer packer, System.SByte value, CancellationToken cancellationToken )
		{
			await packer.PackAsync( value, cancellationToken ).ConfigureAwait( false );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Transfers all catched exceptions." )]
		protected internal override Task<System.SByte> UnpackFromAsyncCore( Unpacker unpacker, CancellationToken cancellationToken )
		{
			var tcs = new TaskCompletionSource<System.SByte>();
			try
			{
				tcs.SetResult( this.UnpackFromCore( unpacker ) );
			}
			catch( Exception ex )
			{
				tcs.SetException( ex );
			}

			return tcs.Task;
		}

#endif // FEATURE_TAP

	}

	internal sealed class System_SingleMessagePackSerializer : MessagePackSerializer< System.Single >
	{
		public System_SingleMessagePackSerializer( SerializationContext ownerContext )
			: base( ownerContext, SerializerCapabilities.PackTo | SerializerCapabilities.UnpackFrom ) { }

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override void PackToCore( Packer packer, System.Single value )
		{
			packer.Pack( value );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override System.Single UnpackFromCore( Unpacker unpacker )
		{
			try
			{
				return unpacker.LastReadData.AsSingle();
			}
			catch( InvalidOperationException ex )
			{
				throw new SerializationException( String.Format( CultureInfo.CurrentCulture, "The unpacked value is not '{0}' type. {1}", typeof( System.Single ), ex.Message ) );
			}
		}

#if FEATURE_TAP

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override async Task PackToAsyncCore( Packer packer, System.Single value, CancellationToken cancellationToken )
		{
			await packer.PackAsync( value, cancellationToken ).ConfigureAwait( false );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Transfers all catched exceptions." )]
		protected internal override Task<System.Single> UnpackFromAsyncCore( Unpacker unpacker, CancellationToken cancellationToken )
		{
			var tcs = new TaskCompletionSource<System.Single>();
			try
			{
				tcs.SetResult( this.UnpackFromCore( unpacker ) );
			}
			catch( Exception ex )
			{
				tcs.SetException( ex );
			}

			return tcs.Task;
		}

#endif // FEATURE_TAP

	}

	internal sealed class System_TimeSpanMessagePackSerializer : MessagePackSerializer< System.TimeSpan >
	{
		public System_TimeSpanMessagePackSerializer( SerializationContext ownerContext )
			: base( ownerContext, SerializerCapabilities.PackTo | SerializerCapabilities.UnpackFrom ) { }

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override void PackToCore( Packer packer, System.TimeSpan value )
		{
			packer.Pack( value.Ticks );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override System.TimeSpan UnpackFromCore( Unpacker unpacker )
		{
			System.Int64 ctorArgument;
			try
			{
				ctorArgument = unpacker.LastReadData.AsInt64();
			}
			catch( InvalidOperationException ex )
			{
				throw new SerializationException( String.Format( CultureInfo.CurrentCulture, "The unpacked value is not '{0}' type. {1}", typeof( System.Int64 ), ex.Message ) );
			}

			return new System.TimeSpan( ctorArgument );
		}

#if FEATURE_TAP

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override async Task PackToAsyncCore( Packer packer, System.TimeSpan value, CancellationToken cancellationToken )
		{
			await packer.PackAsync( value.Ticks, cancellationToken ).ConfigureAwait( false );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Transfers all catched exceptions." )]
		protected internal override Task<System.TimeSpan> UnpackFromAsyncCore( Unpacker unpacker, CancellationToken cancellationToken )
		{
			var tcs = new TaskCompletionSource<System.TimeSpan>();
			try
			{
				tcs.SetResult( this.UnpackFromCore( unpacker ) );
			}
			catch( Exception ex )
			{
				tcs.SetException( ex );
			}

			return tcs.Task;
		}

#endif // FEATURE_TAP

	}

	internal sealed class System_UInt16MessagePackSerializer : MessagePackSerializer< System.UInt16 >
	{
		public System_UInt16MessagePackSerializer( SerializationContext ownerContext )
			: base( ownerContext, SerializerCapabilities.PackTo | SerializerCapabilities.UnpackFrom ) { }

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override void PackToCore( Packer packer, System.UInt16 value )
		{
			packer.Pack( value );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override System.UInt16 UnpackFromCore( Unpacker unpacker )
		{
			try
			{
				return unpacker.LastReadData.AsUInt16();
			}
			catch( InvalidOperationException ex )
			{
				throw new SerializationException( String.Format( CultureInfo.CurrentCulture, "The unpacked value is not '{0}' type. {1}", typeof( System.UInt16 ), ex.Message ) );
			}
		}

#if FEATURE_TAP

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override async Task PackToAsyncCore( Packer packer, System.UInt16 value, CancellationToken cancellationToken )
		{
			await packer.PackAsync( value, cancellationToken ).ConfigureAwait( false );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Transfers all catched exceptions." )]
		protected internal override Task<System.UInt16> UnpackFromAsyncCore( Unpacker unpacker, CancellationToken cancellationToken )
		{
			var tcs = new TaskCompletionSource<System.UInt16>();
			try
			{
				tcs.SetResult( this.UnpackFromCore( unpacker ) );
			}
			catch( Exception ex )
			{
				tcs.SetException( ex );
			}

			return tcs.Task;
		}

#endif // FEATURE_TAP

	}

	internal sealed class System_UInt32MessagePackSerializer : MessagePackSerializer< System.UInt32 >
	{
		public System_UInt32MessagePackSerializer( SerializationContext ownerContext )
			: base( ownerContext, SerializerCapabilities.PackTo | SerializerCapabilities.UnpackFrom ) { }

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override void PackToCore( Packer packer, System.UInt32 value )
		{
			packer.Pack( value );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override System.UInt32 UnpackFromCore( Unpacker unpacker )
		{
			try
			{
				return unpacker.LastReadData.AsUInt32();
			}
			catch( InvalidOperationException ex )
			{
				throw new SerializationException( String.Format( CultureInfo.CurrentCulture, "The unpacked value is not '{0}' type. {1}", typeof( System.UInt32 ), ex.Message ) );
			}
		}

#if FEATURE_TAP

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override async Task PackToAsyncCore( Packer packer, System.UInt32 value, CancellationToken cancellationToken )
		{
			await packer.PackAsync( value, cancellationToken ).ConfigureAwait( false );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Transfers all catched exceptions." )]
		protected internal override Task<System.UInt32> UnpackFromAsyncCore( Unpacker unpacker, CancellationToken cancellationToken )
		{
			var tcs = new TaskCompletionSource<System.UInt32>();
			try
			{
				tcs.SetResult( this.UnpackFromCore( unpacker ) );
			}
			catch( Exception ex )
			{
				tcs.SetException( ex );
			}

			return tcs.Task;
		}

#endif // FEATURE_TAP

	}

	internal sealed class System_UInt64MessagePackSerializer : MessagePackSerializer< System.UInt64 >
	{
		public System_UInt64MessagePackSerializer( SerializationContext ownerContext )
			: base( ownerContext, SerializerCapabilities.PackTo | SerializerCapabilities.UnpackFrom ) { }

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override void PackToCore( Packer packer, System.UInt64 value )
		{
			packer.Pack( value );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override System.UInt64 UnpackFromCore( Unpacker unpacker )
		{
			try
			{
				return unpacker.LastReadData.AsUInt64();
			}
			catch( InvalidOperationException ex )
			{
				throw new SerializationException( String.Format( CultureInfo.CurrentCulture, "The unpacked value is not '{0}' type. {1}", typeof( System.UInt64 ), ex.Message ) );
			}
		}

#if FEATURE_TAP

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override async Task PackToAsyncCore( Packer packer, System.UInt64 value, CancellationToken cancellationToken )
		{
			await packer.PackAsync( value, cancellationToken ).ConfigureAwait( false );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Transfers all catched exceptions." )]
		protected internal override Task<System.UInt64> UnpackFromAsyncCore( Unpacker unpacker, CancellationToken cancellationToken )
		{
			var tcs = new TaskCompletionSource<System.UInt64>();
			try
			{
				tcs.SetResult( this.UnpackFromCore( unpacker ) );
			}
			catch( Exception ex )
			{
				tcs.SetException( ex );
			}

			return tcs.Task;
		}

#endif // FEATURE_TAP

	}

#if !SILVERLIGHT
#if !NETSTANDARD1_1
#if !UNITY || MSGPACK_UNITY_FULL
	internal sealed class System_Collections_Specialized_BitVector32MessagePackSerializer : MessagePackSerializer< System.Collections.Specialized.BitVector32 >
	{
		public System_Collections_Specialized_BitVector32MessagePackSerializer( SerializationContext ownerContext )
			: base( ownerContext, SerializerCapabilities.PackTo | SerializerCapabilities.UnpackFrom ) { }

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override void PackToCore( Packer packer, System.Collections.Specialized.BitVector32 value )
		{
			packer.Pack( value.Data );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override System.Collections.Specialized.BitVector32 UnpackFromCore( Unpacker unpacker )
		{
			System.Int32 ctorArgument;
			try
			{
				ctorArgument = unpacker.LastReadData.AsInt32();
			}
			catch( InvalidOperationException ex )
			{
				throw new SerializationException( String.Format( CultureInfo.CurrentCulture, "The unpacked value is not '{0}' type. {1}", typeof( System.Int32 ), ex.Message ) );
			}

			return new System.Collections.Specialized.BitVector32( ctorArgument );
		}

#if FEATURE_TAP

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override async Task PackToAsyncCore( Packer packer, System.Collections.Specialized.BitVector32 value, CancellationToken cancellationToken )
		{
			await packer.PackAsync( value.Data, cancellationToken ).ConfigureAwait( false );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Transfers all catched exceptions." )]
		protected internal override Task<System.Collections.Specialized.BitVector32> UnpackFromAsyncCore( Unpacker unpacker, CancellationToken cancellationToken )
		{
			var tcs = new TaskCompletionSource<System.Collections.Specialized.BitVector32>();
			try
			{
				tcs.SetResult( this.UnpackFromCore( unpacker ) );
			}
			catch( Exception ex )
			{
				tcs.SetException( ex );
			}

			return tcs.Task;
		}

#endif // FEATURE_TAP

	}
#endif // !NETSTANDARD1_1
#endif // !SILVERLIGHT
#endif // !UNITY || MSGPACK_UNITY_FULL

#if !WINDOWS_PHONE
#if !NET35 && !UNITY
#if !UNITY || MSGPACK_UNITY_FULL
	internal sealed class System_Numerics_BigIntegerMessagePackSerializer : MessagePackSerializer< System.Numerics.BigInteger >
	{
		public System_Numerics_BigIntegerMessagePackSerializer( SerializationContext ownerContext )
			: base( ownerContext, SerializerCapabilities.PackTo | SerializerCapabilities.UnpackFrom ) { }

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override void PackToCore( Packer packer, System.Numerics.BigInteger value )
		{
			packer.PackBinary( value.ToByteArray() );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override System.Numerics.BigInteger UnpackFromCore( Unpacker unpacker )
		{
			try
			{
				return new System.Numerics.BigInteger( unpacker.LastReadData.AsBinary() ); 
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

#if FEATURE_TAP

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override async Task PackToAsyncCore( Packer packer, System.Numerics.BigInteger value, CancellationToken cancellationToken )
		{
			await packer.PackBinaryAsync( value.ToByteArray(), cancellationToken ).ConfigureAwait( false );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Transfers all catched exceptions." )]
		protected internal override Task<System.Numerics.BigInteger> UnpackFromAsyncCore( Unpacker unpacker, CancellationToken cancellationToken )
		{
			var tcs = new TaskCompletionSource<System.Numerics.BigInteger>();
			try
			{
				tcs.SetResult( this.UnpackFromCore( unpacker ) );
			}
			catch( Exception ex )
			{
				tcs.SetException( ex );
			}

			return tcs.Task;
		}

#endif // FEATURE_TAP

	}
#endif // !NET35 && !UNITY
#endif // !WINDOWS_PHONE
#endif // !UNITY || MSGPACK_UNITY_FULL
	// ReSharper restore RedundantCast
	// ReSharper restore RedundantNameQualifier
	// ReSharper restore InconsistentNaming
}
