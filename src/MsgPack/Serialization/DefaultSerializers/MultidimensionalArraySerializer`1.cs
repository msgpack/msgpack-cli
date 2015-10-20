#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2015 FUJIWARA, Yusuke
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
using System.Runtime.Serialization;

namespace MsgPack.Serialization.DefaultSerializers
{
#if !UNITY
	internal sealed class MultidimensionalArraySerializer<TArray, TItem> : MessagePackSerializer<TArray>
#else
	internal sealed class UnityMultidimensionalArraySerializer : NonGenericMessagePackSerializer
#endif // !UNITY
	{
#if !UNITY
		private readonly MessagePackSerializer<TItem> _itemSerializer;
		private readonly MessagePackSerializer<int[]> _int32ArraySerializer;
#else
		private readonly IMessagePackSingleObjectSerializer _itemSerializer;
		private readonly IMessagePackSingleObjectSerializer _int32ArraySerializer;
		private readonly Type _itemType;
#endif // !UNITY

#if !UNITY
		public MultidimensionalArraySerializer( SerializationContext ownerContext, PolymorphismSchema itemsSchema )
			: base( ownerContext )
		{
			this._itemSerializer = ownerContext.GetSerializer<TItem>( itemsSchema );
			this._int32ArraySerializer = ownerContext.GetSerializer<int[]>( itemsSchema );
		}
#else
		public UnityMultidimensionalArraySerializer( SerializationContext ownerContext, Type itemType, PolymorphismSchema itemsSchema )
			: base( ownerContext, itemType.MakeArrayType() )
		{
			this._itemSerializer = ownerContext.GetSerializer( itemType, itemsSchema );
			this._int32ArraySerializer = ownerContext.GetSerializer( typeof( int[] ), itemsSchema );
			this._itemType = itemType;
		}
#endif // !UNITY

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "By design" )]
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
#if !SILVERLIGHT && !NETFX_CORE
			var longLength = array.LongLength;
			if ( longLength > Int32.MaxValue )
			{
				throw new NotSupportedException( "Array length over 32bit is not supported." );
			}

			var totalLength = unchecked( ( int )longLength );
#else
			var totalLength = array.Length;
#endif // !SILVERLIGHT && !NETFX_CORE
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

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods",MessageId = "0", Justification = "By design" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Usage", "CA2202:DoNotDisposeObjectsMultipleTimes", Justification = "Avoided via ownsStream: false" )]
#if !UNITY
		protected internal override TArray UnpackFromCore( Unpacker unpacker )
#else
		protected internal override object UnpackFromCore( Unpacker unpacker )
#endif // !UNITY
		{
			if ( !unpacker.IsArrayHeader )
			{
				throw SerializationExceptions.NewIsNotArrayHeader();
			}

			if ( UnpackHelpers.GetItemsCount( unpacker ) != 2 )
			{
				throw new SerializationException( "Multidimensional array must be encoded as 2 element array." );
			}

			using ( var wholeUnpacker = unpacker.ReadSubtree() )
			{
				if ( !wholeUnpacker.Read() )
				{
					throw SerializationExceptions.NewUnexpectedEndOfStream();
				}

				MessagePackExtendedTypeObject metadata;
				try
				{
					metadata = wholeUnpacker.LastReadData.AsMessagePackExtendedTypeObject();
				}
				catch ( InvalidOperationException ex )
				{
					throw new SerializationException( "Multidimensional array must be encoded as ext type.", ex );
				}

				if ( metadata.TypeCode != this.OwnerContext.ExtTypeCodeMapping[ KnownExtTypeName.MultidimensionalArray ] )
				{
					throw new SerializationException(
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
						throw SerializationExceptions.NewUnexpectedEndOfStream();
					}

					if ( !metadataUnpacker.IsArrayHeader )
					{
						throw SerializationExceptions.NewIsNotArrayHeader();
					}

					if ( UnpackHelpers.GetItemsCount( metadataUnpacker ) != 2 )
					{
						throw new SerializationException( "Multidimensional metadata array must be encoded as 2 element array." );
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
					throw SerializationExceptions.NewUnexpectedEndOfStream();
				}

				if ( !wholeUnpacker.IsArrayHeader )
				{
					throw SerializationExceptions.NewIsNotArrayHeader();
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
									throw SerializationExceptions.NewUnexpectedEndOfStream();
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
				throw SerializationExceptions.NewUnexpectedEndOfStream();
			}

			if ( !metadataUnpacker.IsArrayHeader )
			{
				throw SerializationExceptions.NewIsNotArrayHeader();
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
				throw SerializationExceptions.NewUnexpectedEndOfStream();
			}

			if ( !metadataUnpacker.IsArrayHeader )
			{
				throw SerializationExceptions.NewIsNotArrayHeader();
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
	}
}