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
using System.Numerics;
#if FEATURE_TAP
using System.Threading;
using System.Threading.Tasks;
#endif // FEATURE_TAP

namespace MsgPack.Serialization.DefaultSerializers
{
	// ReSharper disable once InconsistentNaming
	internal sealed class System_Numerics_ComplexMessagePackSerializer : MessagePackSerializer<Complex>
	{
		public System_Numerics_ComplexMessagePackSerializer( SerializationContext ownerContext )
			: base( ownerContext, SerializerCapabilities.PackTo | SerializerCapabilities.UnpackFrom ) { }

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated internally" )]
		protected internal override void PackToCore( Packer packer, Complex objectTree )
		{
			packer.PackArrayHeader( 2 );
			packer.Pack( objectTree.Real );
			packer.Pack( objectTree.Imaginary );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated internally" )]
		protected internal override Complex UnpackFromCore( Unpacker unpacker )
		{
			if ( !unpacker.IsArrayHeader )
			{
				SerializationExceptions.ThrowInvalidArrayItemsCount( unpacker, typeof( Complex ), 2 );
			}

			long length = UnpackHelpers.GetItemsCount( unpacker );
			if ( length != 2 )
			{
				SerializationExceptions.ThrowInvalidArrayItemsCount( unpacker, typeof( Complex ), 2 );
			}

			double real, imaginary;
			if ( !unpacker.ReadDouble( out real ) )
			{
				SerializationExceptions.ThrowUnexpectedEndOfStream( unpacker );
			}


			if ( !unpacker.ReadDouble( out imaginary ) )
			{
				SerializationExceptions.ThrowUnexpectedEndOfStream( unpacker );
			}

			return new Complex( real, imaginary );
		}

#if FEATURE_TAP

		protected internal override async Task PackToAsyncCore( Packer packer, Complex objectTree, CancellationToken cancellationToken )
		{
			await packer.PackArrayHeaderAsync( 2, cancellationToken ).ConfigureAwait( false );
			await packer.PackAsync( objectTree.Real, cancellationToken ).ConfigureAwait( false );
			await packer.PackAsync( objectTree.Imaginary, cancellationToken ).ConfigureAwait( false );
		}

		protected internal override async Task<Complex> UnpackFromAsyncCore( Unpacker unpacker, CancellationToken cancellationToken )
		{
			var real = await unpacker.ReadDoubleAsync( cancellationToken ).ConfigureAwait( false );
			if ( !real.Success )
			{
				SerializationExceptions.ThrowUnexpectedEndOfStream( unpacker );
			}

			var imaginary = await unpacker.ReadDoubleAsync( cancellationToken ).ConfigureAwait( false );
			if ( !imaginary.Success )
			{
				SerializationExceptions.ThrowUnexpectedEndOfStream( unpacker );
			}

			return new Complex( real.Value, imaginary.Value );
		}

#endif // FEATURE_TAP

	}
}
