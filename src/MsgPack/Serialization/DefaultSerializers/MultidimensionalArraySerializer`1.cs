#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2015-2016 FUJIWARA, Yusuke
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
using System.IO;
#if FEATURE_TAP
using System.Threading;
using System.Threading.Tasks;
#endif // FEATURE_TAP

namespace MsgPack.Serialization.DefaultSerializers
{
	[Preserve( AllMembers = true )]
#if !UNITY
	internal sealed class MultidimensionalArraySerializer<TArray, TItem> : MessagePackSerializer<TArray>
#else
#warning TODO: Use generic collection if possible for maintenancibility.
	internal sealed class UnityMultidimensionalArraySerializer : NonGenericMessagePackSerializer
#endif // !UNITY
	{
#if !UNITY
		private readonly MessagePackSerializer<TItem> _itemSerializer;
		private readonly MessagePackSerializer<int[]> _int32ArraySerializer;
#else
		private readonly MessagePackSerializer _itemSerializer;
		private readonly MessagePackSerializer _int32ArraySerializer;
		private readonly Type _itemType;
#endif // !UNITY

#if !UNITY
		public MultidimensionalArraySerializer( SerializationContext ownerContext, PolymorphismSchema itemsSchema )
			: base( ownerContext, SerializerCapabilities.PackTo | SerializerCapabilities.UnpackFrom )
		{
			this._itemSerializer = ownerContext.GetSerializer<TItem>( itemsSchema );
			this._int32ArraySerializer = ownerContext.GetSerializer<int[]>( itemsSchema );
		}
#else
		public UnityMultidimensionalArraySerializer( SerializationContext ownerContext, Type itemType, PolymorphismSchema itemsSchema )
			: base( ownerContext, itemType.MakeArrayType(), SerializerCapabilities.PackTo | SerializerCapabilities.UnpackFrom )
		{
			this._itemSerializer = ownerContext.GetSerializer( itemType, itemsSchema );
			this._int32ArraySerializer = ownerContext.GetSerializer( typeof( int[] ), itemsSchema );
			this._itemType = itemType;
		}
#endif // !UNITY

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "Validated by caller in base class" )]
#if !UNITY
		protected internal override void PackToCore( Packer packer, TArray objectTree )
		{
			this.PackArrayCore( packer, ( Array )( object )objectTree );
		}
#else
		protected internal override void PackToCore( Packer packer, object objectTree )
		{
			this.PackArrayCore( packer, ( Array )objectTree );
		}
#endif // !UNITY

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Usage", "CA2202:DoNotDisposeObjectsMultipleTimes", Justification = "Avoided via ownsStream: false" )]
		private void PackArrayCore( Packer packer, Array array )
		{
#if !SILVERLIGHT && !NETSTANDARD1_1 && !NETSTANDARD1_3
			var longLength = array.LongLength;
			if ( longLength > Int32.MaxValue )
			{
				throw new NotSupportedException( "Array length over 32bit is not supported." );
			}

			var totalLength = unchecked( ( int )longLength );
#else
			var totalLength = array.Length;
#endif // !SILVERLIGHT && !NETSTANDARD1_1 && !NETSTANDARD1_3
			int[] lowerBounds, lengths;
			GetArrayMetadata( array, out lengths, out lowerBounds );

			packer.PackArrayHeader( 2 );

			using ( var buffer = new MemoryStream() )
			using ( var bodyPacker = Packer.Create( buffer, false ) )
			{
				bodyPacker.PackArrayHeader( 2 );
				this._int32ArraySerializer.PackTo( bodyPacker, lengths );
				this._int32ArraySerializer.PackTo( bodyPacker, lowerBounds );
				packer.PackExtendedTypeValue( this.OwnerContext.ExtTypeCodeMapping[ KnownExtTypeName.MultidimensionalArray ], buffer.ToArray() );
			}

			packer.PackArrayHeader( totalLength );
			ForEach(
				array,
				totalLength,
				lowerBounds,
				lengths,
#if !UNITY
				// ReSharper disable once AccessToDisposedClosure
				indices => this._itemSerializer.PackTo( packer, ( TItem )array.GetValue( indices ) )
#else
				// ReSharper disable once AccessToDisposedClosure
				indices => this._itemSerializer.PackTo( packer, array.GetValue( indices ) )
#endif // !UNITY
			);
		}

		private static void GetArrayMetadata( Array array, out int[] lengths, out int[] lowerBounds )
		{
			lowerBounds = new int[ array.Rank ];
			lengths = new int[ array.Rank ];
			for ( var dimension = 0; dimension < array.Rank; dimension++ )
			{
				lowerBounds[ dimension ] = array.GetLowerBound( dimension );
				lengths[ dimension ] = array.GetLength( dimension );
			}
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Usage", "CA2202:DoNotDisposeObjectsMultipleTimes", Justification = "Avoided via ownsStream: false" )]
#if !UNITY
		protected internal override TArray UnpackFromCore( Unpacker unpacker )
#else
		protected internal override object UnpackFromCore( Unpacker unpacker )
#endif // !UNITY
		{
			if ( !unpacker.IsArrayHeader )
			{
				SerializationExceptions.ThrowIsNotArrayHeader( unpacker );
			}

			if ( UnpackHelpers.GetItemsCount( unpacker ) != 2 )
			{
				SerializationExceptions.ThrowSerializationException( "Multidimensional array must be encoded as 2 element array." );
			}

			using ( var wholeUnpacker = unpacker.ReadSubtree() )
			{
				if ( !wholeUnpacker.Read() )
				{
					SerializationExceptions.ThrowUnexpectedEndOfStream( unpacker );
				}

				MessagePackExtendedTypeObject metadata;
				try
				{
					metadata = wholeUnpacker.LastReadData.AsMessagePackExtendedTypeObject();
				}
				catch ( InvalidOperationException ex )
				{
					SerializationExceptions.ThrowSerializationException( "Multidimensional array must be encoded as ext type.", ex );
					metadata = default ( MessagePackExtendedTypeObject ); // never reaches
				}

				if ( metadata.TypeCode != this.OwnerContext.ExtTypeCodeMapping[ KnownExtTypeName.MultidimensionalArray ] )
				{
					SerializationExceptions.ThrowSerializationException(
						String.Format(
							CultureInfo.CurrentCulture,
							"Multidimensional array must be encoded as ext type 0x{0:X2}.",
							this.OwnerContext.ExtTypeCodeMapping[ KnownExtTypeName.MultidimensionalArray ]
							)
						);
				}

				int[] lengths, lowerBounds;

				using ( var arrayMetadata = new MemoryStream( metadata.Body ) )
				using ( var metadataUnpacker = Unpacker.Create( arrayMetadata, false ) )
				{
					if ( !metadataUnpacker.Read() )
					{
						SerializationExceptions.ThrowUnexpectedEndOfStream( unpacker );
					}

					if ( !metadataUnpacker.IsArrayHeader )
					{
						SerializationExceptions.ThrowIsNotArrayHeader( unpacker );
					}

					if ( UnpackHelpers.GetItemsCount( metadataUnpacker ) != 2 )
					{
						SerializationExceptions.ThrowSerializationException( "Multidimensional metadata array must be encoded as 2 element array." );
					}

					this.ReadArrayMetadata( metadataUnpacker, out lengths, out lowerBounds );
				}

#if SILVERLIGHT
				// Simulate lowerbounds because Array.Initialize() in Silverlight does not support lowerbounds.
				var inflatedLengths = new int[ lengths.Length ];
				for ( var i = 0; i < lowerBounds.Length; i++ )
				{
					inflatedLengths[ i ] = lengths[ i ] + lowerBounds[ i ];
				}

#endif // SILVERLIGHT
				if ( !wholeUnpacker.Read() )
				{
					SerializationExceptions.ThrowUnexpectedEndOfStream( unpacker );
				}

				if ( !wholeUnpacker.IsArrayHeader )
				{
					SerializationExceptions.ThrowIsNotArrayHeader( unpacker );
				}

				using ( var arrayUnpacker = wholeUnpacker.ReadSubtree() )
				{
					var result =
						Array.CreateInstance(
#if !UNITY
							typeof( TItem ),
#else
							this._itemType,
#endif // !UNITY
#if !SILVERLIGHT
							lengths,
							lowerBounds
#else
							inflatedLengths
#endif // !SILVERLIGHT
						);

					var totalLength = UnpackHelpers.GetItemsCount( arrayUnpacker );
					if ( totalLength > 0 )
					{
						ForEach(
							result,
							totalLength,
							lowerBounds,
							lengths,
							indices =>
							{
								// ReSharper disable AccessToDisposedClosure
								if ( !arrayUnpacker.Read() )
								{
									SerializationExceptions.ThrowUnexpectedEndOfStream( unpacker );
								}

								result.SetValue(
									this._itemSerializer.UnpackFrom( arrayUnpacker ),
									indices
								);
								// ReSharper restore AccessToDisposedClosure
							}
						);
					}

#if !UNITY
					return ( TArray ) ( object ) result;
#else
					return result;
#endif // !UNITY
				}
			}
		}

		private void ReadArrayMetadata( Unpacker metadataUnpacker, out int[] lengths, out int[] lowerBounds )
		{
			if ( !metadataUnpacker.Read() )
			{
				SerializationExceptions.ThrowUnexpectedEndOfStream( metadataUnpacker );
			}

			if ( !metadataUnpacker.IsArrayHeader )
			{
				SerializationExceptions.ThrowIsNotArrayHeader( metadataUnpacker );
			}

			using ( var lengthsUnpacker = metadataUnpacker.ReadSubtree() )
			{
#if !UNITY
				lengths = this._int32ArraySerializer.UnpackFrom( lengthsUnpacker );
#else
				lengths = this._int32ArraySerializer.UnpackFrom( lengthsUnpacker ) as int[];
#endif // !UNITY
			}

			if ( !metadataUnpacker.Read() )
			{
				SerializationExceptions.ThrowUnexpectedEndOfStream( metadataUnpacker );
			}

			if ( !metadataUnpacker.IsArrayHeader )
			{
				SerializationExceptions.ThrowIsNotArrayHeader( metadataUnpacker );
			}

			using ( var lowerBoundsUnpacker = metadataUnpacker.ReadSubtree() )
			{
#if !UNITY
				lowerBounds = this._int32ArraySerializer.UnpackFrom( lowerBoundsUnpacker );
#else
				lowerBounds = this._int32ArraySerializer.UnpackFrom( lowerBoundsUnpacker ) as int[];
#endif // !UNITY
			}
		}

#if FEATURE_TAP

		protected internal override Task PackToAsyncCore( Packer packer, TArray objectTree, CancellationToken cancellationToken )
		{
			return this.PackArrayAsyncCore( packer, ( Array )( object )objectTree, cancellationToken );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Usage", "CA2202:DoNotDisposeObjectsMultipleTimes", Justification = "Avoided via ownsStream: false" )]
		private async Task PackArrayAsyncCore( Packer packer, Array array, CancellationToken cancellationToken )
		{
#if !NETSTANDARD1_1 && !NETSTANDARD1_3
			var longLength = array.LongLength;
			if ( longLength > Int32.MaxValue )
			{
				throw new NotSupportedException( "Array length over 32bit is not supported." );
			}

			var totalLength = unchecked( ( int )longLength );
#else
			var totalLength = array.Length;
#endif // !NETSTANDARD1_1 && !NETSTANDARD1_3
			int[] lowerBounds, lengths;
			GetArrayMetadata( array, out lengths, out lowerBounds );

			await packer.PackArrayHeaderAsync( 2, cancellationToken ).ConfigureAwait( false );

			using ( var buffer = new MemoryStream() )
			using ( var bodyPacker = Packer.Create( buffer, false ) )
			{
				await bodyPacker.PackArrayHeaderAsync( 2, cancellationToken ).ConfigureAwait( false );
				await this._int32ArraySerializer.PackToAsync( bodyPacker, lengths, cancellationToken ).ConfigureAwait( false );
				await this._int32ArraySerializer.PackToAsync( bodyPacker, lowerBounds, cancellationToken ).ConfigureAwait( false );
				await packer.PackExtendedTypeValueAsync( this.OwnerContext.ExtTypeCodeMapping[ KnownExtTypeName.MultidimensionalArray ], buffer.ToArray(), cancellationToken ).ConfigureAwait( false );
			}

			await packer.PackArrayHeaderAsync( totalLength, cancellationToken ).ConfigureAwait( false );
			ForEach(
				array,
				totalLength,
				lowerBounds,
				lengths,
				// ReSharper disable once AccessToDisposedClosure
				async indices => await this._itemSerializer.PackToAsync( packer, ( TItem )array.GetValue( indices ), cancellationToken ).ConfigureAwait( false )
			);
		}

		protected internal override async Task<TArray> UnpackFromAsyncCore( Unpacker unpacker, CancellationToken cancellationToken )
		{
			if ( !unpacker.IsArrayHeader )
			{
				SerializationExceptions.ThrowIsNotArrayHeader( unpacker );
			}

			if ( UnpackHelpers.GetItemsCount( unpacker ) != 2 )
			{
				SerializationExceptions.ThrowSerializationException( "Multidimensional array must be encoded as 2 element array." );
			}

			using ( var wholeUnpacker = unpacker.ReadSubtree() )
			{
				if ( !await wholeUnpacker.ReadAsync( cancellationToken ).ConfigureAwait( false ) )
				{
					SerializationExceptions.ThrowUnexpectedEndOfStream( unpacker );
				}

				MessagePackExtendedTypeObject metadata;
				try
				{
					metadata = wholeUnpacker.LastReadData.AsMessagePackExtendedTypeObject();
				}
				catch ( InvalidOperationException ex )
				{
					SerializationExceptions.ThrowSerializationException( "Multidimensional array must be encoded as ext type.", ex );
					metadata = default( MessagePackExtendedTypeObject ); // never reaches
				}

				if ( metadata.TypeCode != this.OwnerContext.ExtTypeCodeMapping[ KnownExtTypeName.MultidimensionalArray ] )
				{
					SerializationExceptions.ThrowSerializationException(
						String.Format(
							CultureInfo.CurrentCulture,
							"Multidimensional array must be encoded as ext type 0x{0:X2}.",
							this.OwnerContext.ExtTypeCodeMapping[ KnownExtTypeName.MultidimensionalArray ]
							)
						);
				}

				Tuple<int[], int[]> lengthsAndLowerBounds;

				using ( var arrayMetadata = new MemoryStream( metadata.Body ) )
				using ( var metadataUnpacker = Unpacker.Create( arrayMetadata, false ) )
				{
					if ( !metadataUnpacker.Read() )
					{
						SerializationExceptions.ThrowUnexpectedEndOfStream( unpacker );
					}

					if ( !metadataUnpacker.IsArrayHeader )
					{
						SerializationExceptions.ThrowIsNotArrayHeader( unpacker );
					}

					if ( UnpackHelpers.GetItemsCount( metadataUnpacker ) != 2 )
					{
						SerializationExceptions.ThrowSerializationException( "Multidimensional metadata array must be encoded as 2 element array." );
					}

					lengthsAndLowerBounds = await this.ReadArrayMetadataAsync( metadataUnpacker, cancellationToken ).ConfigureAwait( false );
				}

				if ( !await wholeUnpacker.ReadAsync( cancellationToken ).ConfigureAwait( false ) )
				{
					SerializationExceptions.ThrowUnexpectedEndOfStream( unpacker );
				}

				if ( !wholeUnpacker.IsArrayHeader )
				{
					SerializationExceptions.ThrowIsNotArrayHeader( unpacker );
				}

				using ( var arrayUnpacker = wholeUnpacker.ReadSubtree() )
				{
					var result =
						Array.CreateInstance( typeof( TItem ), lengthsAndLowerBounds.Item1, lengthsAndLowerBounds.Item2 );

					var totalLength = UnpackHelpers.GetItemsCount( arrayUnpacker );
					if ( totalLength > 0 )
					{
						await ForEachAsync(
							result,
							totalLength,
							lengthsAndLowerBounds.Item2,
							lengthsAndLowerBounds.Item1,
							async indices =>
							{
								// ReSharper disable AccessToDisposedClosure
								if ( !await arrayUnpacker.ReadAsync( cancellationToken ).ConfigureAwait( false ) )
								{
									SerializationExceptions.ThrowUnexpectedEndOfStream( unpacker );
								}

								result.SetValue(
									await this._itemSerializer.UnpackFromAsync( arrayUnpacker, cancellationToken ).ConfigureAwait( false ),
									indices
								);
								// ReSharper restore AccessToDisposedClosure
							}
						).ConfigureAwait( false );
					}

					return ( TArray )( object )result;
				}
			}
		}

		private async Task<Tuple<int[],int[]>> ReadArrayMetadataAsync( Unpacker metadataUnpacker, CancellationToken cancellationToken )
		{
			if ( !await metadataUnpacker.ReadAsync( cancellationToken ).ConfigureAwait( false ) )
			{
				SerializationExceptions.ThrowUnexpectedEndOfStream( metadataUnpacker );
			}

			if ( !metadataUnpacker.IsArrayHeader )
			{
				SerializationExceptions.ThrowIsNotArrayHeader( metadataUnpacker );
			}

			int[] lengths, lowerBounds;

			using ( var lengthsUnpacker = metadataUnpacker.ReadSubtree() )
			{
				lengths = await this._int32ArraySerializer.UnpackFromAsync( lengthsUnpacker, cancellationToken ).ConfigureAwait( false );
			}

			if ( !await metadataUnpacker.ReadAsync( cancellationToken ).ConfigureAwait( false ) )
			{
				SerializationExceptions.ThrowUnexpectedEndOfStream( metadataUnpacker );
			}

			if ( !metadataUnpacker.IsArrayHeader )
			{
				SerializationExceptions.ThrowIsNotArrayHeader( metadataUnpacker );
			}

			using ( var lowerBoundsUnpacker = metadataUnpacker.ReadSubtree() )
			{
				lowerBounds = await this._int32ArraySerializer.UnpackFromAsync( lowerBoundsUnpacker, cancellationToken ).ConfigureAwait( false );
			}

			return Tuple.Create( lengths, lowerBounds );
		}

#endif // FEATURE_TAP

		private static void ForEach( Array array, int totalLength, int[] lowerBounds, int[] lengths, Action<int[]> action )
		{
			var indices = new int[ array.Rank ];
			for ( var dimension = 0; dimension < array.Rank; dimension++ )
			{
				indices[ dimension ] = lowerBounds[ dimension ];
			}

			for ( var i = 0; i < totalLength; i++ )
			{
				action( indices );
				// Canculate indices with carrying up.
				var dimension = indices.Length - 1;
				for ( ; dimension >= 0; dimension-- )
				{
					if ( ( indices[ dimension ] + 1 ) < lengths[ dimension ] + lowerBounds[ dimension ] )
					{
						indices[ dimension ]++;
						break;
					}

					// Let's carry up, so set 0 to current dimension.
					indices[ dimension ] = lowerBounds[ dimension ];
				}
			}
		}

#if FEATURE_TAP

		private static async Task ForEachAsync( Array array, int totalLength, int[] lowerBounds, int[] lengths, Func<int[], Task> action )
		{
			var indices = new int[ array.Rank ];
			for ( var dimension = 0; dimension < array.Rank; dimension++ )
			{
				indices[ dimension ] = lowerBounds[ dimension ];
			}

			for ( var i = 0; i < totalLength; i++ )
			{
				await action( indices ).ConfigureAwait( false );
				// Canculate indices with carrying up.
				var dimension = indices.Length - 1;
				for ( ; dimension >= 0; dimension-- )
				{
					if ( ( indices[ dimension ] + 1 ) < lengths[ dimension ] + lowerBounds[ dimension ] )
					{
						indices[ dimension ]++;
						break;
					}

					// Let's carry up, so set 0 to current dimension.
					indices[ dimension ] = lowerBounds[ dimension ];
				}
			}
		}
#endif // FEATURE_TAP
	}
}