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

using System;
#if FEATURE_TAP
using System.Threading;
using System.Threading.Tasks;
#endif // FEATURE_TAP

namespace MsgPack.Serialization.DefaultSerializers
{
	// ReSharper disable once InconsistentNaming
	internal sealed class System_VersionMessagePackSerializer : MessagePackSerializer<Version>
	{
		public System_VersionMessagePackSerializer( SerializationContext ownerContext )
			: base( ownerContext, SerializerCapabilities.PackTo | SerializerCapabilities.UnpackFrom ) { }

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated internally" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "Validated internally" )]
		protected internal override void PackToCore( Packer packer, Version objectTree )
		{
			packer.PackArrayHeader( 4 );
			packer.Pack( objectTree.Major );
			packer.Pack( objectTree.Minor );
			packer.Pack( objectTree.Build );
			packer.Pack( objectTree.Revision );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated internally" )]
		protected internal override Version UnpackFromCore( Unpacker unpacker )
		{
			if ( !unpacker.IsArrayHeader )
			{
				SerializationExceptions.ThrowInvalidArrayItemsCount( unpacker, typeof( Version ), 4 );
			}

			long length = UnpackHelpers.GetItemsCount( unpacker );
			if ( length != 4 )
			{
				SerializationExceptions.ThrowInvalidArrayItemsCount( unpacker, typeof( Version ), 4 );
			}

			int major, minor, build, revision;
			if ( !unpacker.ReadInt32( out major ) )
			{
				SerializationExceptions.ThrowMissingItem( 0, unpacker );
			}

			if ( !unpacker.ReadInt32( out minor ) )
			{
				SerializationExceptions.ThrowMissingItem( 1, unpacker );
			}

			if ( !unpacker.ReadInt32( out build ) )
			{
				SerializationExceptions.ThrowMissingItem( 2, unpacker );
			}

			if ( !unpacker.ReadInt32( out revision ) )
			{
				SerializationExceptions.ThrowMissingItem( 3, unpacker );
			}

			if (build < 0 && revision < 0)
			{
				return new Version(major, minor);
			}
			else if (revision < 0)
			{
				return new Version(major, minor, build);
			}

			return new Version( major, minor, build, revision );
		}

#if FEATURE_TAP

		protected internal override async Task PackToAsyncCore( Packer packer, Version objectTree, CancellationToken cancellationToken )
		{
			await packer.PackArrayHeaderAsync( 4, cancellationToken ).ConfigureAwait( false );
			await packer.PackAsync( objectTree.Major, cancellationToken ).ConfigureAwait( false );
			await packer.PackAsync( objectTree.Minor, cancellationToken ).ConfigureAwait( false );
			await packer.PackAsync( objectTree.Build, cancellationToken ).ConfigureAwait( false );
			await packer.PackAsync( objectTree.Revision, cancellationToken ).ConfigureAwait( false );
		}

		protected internal override async Task<Version> UnpackFromAsyncCore( Unpacker unpacker, CancellationToken cancellationToken )
		{
			if ( !unpacker.IsArrayHeader )
			{
				SerializationExceptions.ThrowInvalidArrayItemsCount( unpacker, typeof( Version ), 4 );
			}

			long length = unpacker.LastReadData.AsInt64();
			if ( length != 4 )
			{
				SerializationExceptions.ThrowInvalidArrayItemsCount( unpacker, typeof( Version ), 4 );
			}

			var major = await unpacker.ReadInt32Async( cancellationToken ).ConfigureAwait( false );
			if ( !major.Success )
			{
				SerializationExceptions.ThrowMissingItem( 0, unpacker );
			}

			var minor = await unpacker.ReadInt32Async( cancellationToken ).ConfigureAwait( false );
			if ( !minor.Success )
			{
				SerializationExceptions.ThrowMissingItem( 1, unpacker );
			}

			var build = await unpacker.ReadInt32Async( cancellationToken ).ConfigureAwait( false );
			if ( !build.Success )
			{
				SerializationExceptions.ThrowMissingItem( 2, unpacker );
			}

			var revision = await unpacker.ReadInt32Async( cancellationToken ).ConfigureAwait( false );
			if ( !revision.Success )
			{
				SerializationExceptions.ThrowMissingItem( 3, unpacker );
			}

			if (build.Value < 0 && revision.Value < 0)
			{
				return new Version(major.Value, minor.Value);
			}
			else if (revision.Value < 0)
			{
				return new Version(major.Value, minor.Value, build.Value);
			}

			return new Version( major.Value, minor.Value, build.Value, revision.Value );
		}

#endif // FEATURE_TAP

	}
}
