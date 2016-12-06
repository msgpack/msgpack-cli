 
#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2016 FUJIWARA, Yusuke
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

#if !NETSTANDARD1_1
using System;
using System.Numerics;
#if FEATURE_TAP
using System.Threading;
using System.Threading.Tasks;
#endif // FEATURE_TAP

namespace MsgPack.Serialization.DefaultSerializers
{
	// This file generated from SimdTypeSerializer.tt T4Template.
	// Do not modify this file. Edit SimdTypeSerializer.tt instead.

	// ReSharper disable InconsistentNaming
	// ReSharper disable RedundantNameQualifier
	// ReSharper disable RedundantCast

	internal sealed class System_Numerics_Vector2MessagePackSerializer : MessagePackSerializer<Vector2>
	{
		public System_Numerics_Vector2MessagePackSerializer( SerializationContext ownerContext ) 
			: base( ownerContext, SerializerCapabilities.PackTo | SerializerCapabilities.UnpackFrom ) { }

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override void PackToCore( Packer packer, Vector2 objectTree )
		{
			packer.PackArrayHeader( 2 );
			packer.Pack( objectTree.X );
			packer.Pack( objectTree.Y );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override Vector2 UnpackFromCore( Unpacker unpacker )
		{
			if ( !unpacker.IsArrayHeader )
			{
				SerializationExceptions.ThrowInvalidArrayItemsCount( unpacker, typeof( Vector2 ), 2 );
			}

			var length = UnpackHelpers.GetItemsCount( unpacker );

			if ( length != 2 )
			{
				SerializationExceptions.ThrowInvalidArrayItemsCount( unpacker, typeof( Vector2 ), 2 );
			}

			float x;
			if ( !unpacker.ReadSingle( out x ) )
			{
				SerializationExceptions.ThrowMissingItem( 0, unpacker );
			}

			float y;
			if ( !unpacker.ReadSingle( out y ) )
			{
				SerializationExceptions.ThrowMissingItem( 1, unpacker );
			}


			return new Vector2( x, y );
		}

#if FEATURE_TAP

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override async Task PackToAsyncCore( Packer packer, Vector2 objectTree, CancellationToken cancellationToken )
		{
			await packer.PackArrayHeaderAsync( 2, cancellationToken ).ConfigureAwait( false );
			await packer.PackAsync( objectTree.X, cancellationToken ).ConfigureAwait( false );
			await packer.PackAsync( objectTree.Y, cancellationToken ).ConfigureAwait( false );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override async Task<Vector2> UnpackFromAsyncCore( Unpacker unpacker, CancellationToken cancellationToken )
		{
			if ( !unpacker.IsArrayHeader )
			{
				SerializationExceptions.ThrowInvalidArrayItemsCount( unpacker, typeof( Vector2 ), 2 );
			}

			var length = unpacker.LastReadData.AsInt64();

			if ( length != 2 )
			{
				SerializationExceptions.ThrowInvalidArrayItemsCount( unpacker, typeof( Vector2 ), 2 );
			}

			var x = await unpacker.ReadSingleAsync( cancellationToken ).ConfigureAwait( false );
			if ( !x.Success )
			{
				SerializationExceptions.ThrowMissingItem( 0, unpacker );
			}

			var y = await unpacker.ReadSingleAsync( cancellationToken ).ConfigureAwait( false );
			if ( !y.Success )
			{
				SerializationExceptions.ThrowMissingItem( 1, unpacker );
			}


			return new Vector2( x.Value, y.Value );
		}

#endif // FEATURE_TAP

	}
	internal sealed class System_Numerics_Vector3MessagePackSerializer : MessagePackSerializer<Vector3>
	{
		public System_Numerics_Vector3MessagePackSerializer( SerializationContext ownerContext ) 
			: base( ownerContext, SerializerCapabilities.PackTo | SerializerCapabilities.UnpackFrom ) { }

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override void PackToCore( Packer packer, Vector3 objectTree )
		{
			packer.PackArrayHeader( 3 );
			packer.Pack( objectTree.X );
			packer.Pack( objectTree.Y );
			packer.Pack( objectTree.Z );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override Vector3 UnpackFromCore( Unpacker unpacker )
		{
			if ( !unpacker.IsArrayHeader )
			{
				SerializationExceptions.ThrowInvalidArrayItemsCount( unpacker, typeof( Vector3 ), 3 );
			}

			var length = UnpackHelpers.GetItemsCount( unpacker );

			if ( length != 3 )
			{
				SerializationExceptions.ThrowInvalidArrayItemsCount( unpacker, typeof( Vector3 ), 3 );
			}

			float x;
			if ( !unpacker.ReadSingle( out x ) )
			{
				SerializationExceptions.ThrowMissingItem( 0, unpacker );
			}

			float y;
			if ( !unpacker.ReadSingle( out y ) )
			{
				SerializationExceptions.ThrowMissingItem( 1, unpacker );
			}

			float z;
			if ( !unpacker.ReadSingle( out z ) )
			{
				SerializationExceptions.ThrowMissingItem( 2, unpacker );
			}


			return new Vector3( x, y, z );
		}

#if FEATURE_TAP

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override async Task PackToAsyncCore( Packer packer, Vector3 objectTree, CancellationToken cancellationToken )
		{
			await packer.PackArrayHeaderAsync( 3, cancellationToken ).ConfigureAwait( false );
			await packer.PackAsync( objectTree.X, cancellationToken ).ConfigureAwait( false );
			await packer.PackAsync( objectTree.Y, cancellationToken ).ConfigureAwait( false );
			await packer.PackAsync( objectTree.Z, cancellationToken ).ConfigureAwait( false );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override async Task<Vector3> UnpackFromAsyncCore( Unpacker unpacker, CancellationToken cancellationToken )
		{
			if ( !unpacker.IsArrayHeader )
			{
				SerializationExceptions.ThrowInvalidArrayItemsCount( unpacker, typeof( Vector3 ), 3 );
			}

			var length = unpacker.LastReadData.AsInt64();

			if ( length != 3 )
			{
				SerializationExceptions.ThrowInvalidArrayItemsCount( unpacker, typeof( Vector3 ), 3 );
			}

			var x = await unpacker.ReadSingleAsync( cancellationToken ).ConfigureAwait( false );
			if ( !x.Success )
			{
				SerializationExceptions.ThrowMissingItem( 0, unpacker );
			}

			var y = await unpacker.ReadSingleAsync( cancellationToken ).ConfigureAwait( false );
			if ( !y.Success )
			{
				SerializationExceptions.ThrowMissingItem( 1, unpacker );
			}

			var z = await unpacker.ReadSingleAsync( cancellationToken ).ConfigureAwait( false );
			if ( !z.Success )
			{
				SerializationExceptions.ThrowMissingItem( 2, unpacker );
			}


			return new Vector3( x.Value, y.Value, z.Value );
		}

#endif // FEATURE_TAP

	}
	internal sealed class System_Numerics_Vector4MessagePackSerializer : MessagePackSerializer<Vector4>
	{
		public System_Numerics_Vector4MessagePackSerializer( SerializationContext ownerContext ) 
			: base( ownerContext, SerializerCapabilities.PackTo | SerializerCapabilities.UnpackFrom ) { }

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override void PackToCore( Packer packer, Vector4 objectTree )
		{
			packer.PackArrayHeader( 4 );
			packer.Pack( objectTree.X );
			packer.Pack( objectTree.Y );
			packer.Pack( objectTree.Z );
			packer.Pack( objectTree.W );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override Vector4 UnpackFromCore( Unpacker unpacker )
		{
			if ( !unpacker.IsArrayHeader )
			{
				SerializationExceptions.ThrowInvalidArrayItemsCount( unpacker, typeof( Vector4 ), 4 );
			}

			var length = UnpackHelpers.GetItemsCount( unpacker );

			if ( length != 4 )
			{
				SerializationExceptions.ThrowInvalidArrayItemsCount( unpacker, typeof( Vector4 ), 4 );
			}

			float x;
			if ( !unpacker.ReadSingle( out x ) )
			{
				SerializationExceptions.ThrowMissingItem( 0, unpacker );
			}

			float y;
			if ( !unpacker.ReadSingle( out y ) )
			{
				SerializationExceptions.ThrowMissingItem( 1, unpacker );
			}

			float z;
			if ( !unpacker.ReadSingle( out z ) )
			{
				SerializationExceptions.ThrowMissingItem( 2, unpacker );
			}

			float w;
			if ( !unpacker.ReadSingle( out w ) )
			{
				SerializationExceptions.ThrowMissingItem( 3, unpacker );
			}


			return new Vector4( x, y, z, w );
		}

#if FEATURE_TAP

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override async Task PackToAsyncCore( Packer packer, Vector4 objectTree, CancellationToken cancellationToken )
		{
			await packer.PackArrayHeaderAsync( 4, cancellationToken ).ConfigureAwait( false );
			await packer.PackAsync( objectTree.X, cancellationToken ).ConfigureAwait( false );
			await packer.PackAsync( objectTree.Y, cancellationToken ).ConfigureAwait( false );
			await packer.PackAsync( objectTree.Z, cancellationToken ).ConfigureAwait( false );
			await packer.PackAsync( objectTree.W, cancellationToken ).ConfigureAwait( false );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override async Task<Vector4> UnpackFromAsyncCore( Unpacker unpacker, CancellationToken cancellationToken )
		{
			if ( !unpacker.IsArrayHeader )
			{
				SerializationExceptions.ThrowInvalidArrayItemsCount( unpacker, typeof( Vector4 ), 4 );
			}

			var length = unpacker.LastReadData.AsInt64();

			if ( length != 4 )
			{
				SerializationExceptions.ThrowInvalidArrayItemsCount( unpacker, typeof( Vector4 ), 4 );
			}

			var x = await unpacker.ReadSingleAsync( cancellationToken ).ConfigureAwait( false );
			if ( !x.Success )
			{
				SerializationExceptions.ThrowMissingItem( 0, unpacker );
			}

			var y = await unpacker.ReadSingleAsync( cancellationToken ).ConfigureAwait( false );
			if ( !y.Success )
			{
				SerializationExceptions.ThrowMissingItem( 1, unpacker );
			}

			var z = await unpacker.ReadSingleAsync( cancellationToken ).ConfigureAwait( false );
			if ( !z.Success )
			{
				SerializationExceptions.ThrowMissingItem( 2, unpacker );
			}

			var w = await unpacker.ReadSingleAsync( cancellationToken ).ConfigureAwait( false );
			if ( !w.Success )
			{
				SerializationExceptions.ThrowMissingItem( 3, unpacker );
			}


			return new Vector4( x.Value, y.Value, z.Value, w.Value );
		}

#endif // FEATURE_TAP

	}
	internal sealed class System_Numerics_PlaneMessagePackSerializer : MessagePackSerializer<Plane>
	{
		public System_Numerics_PlaneMessagePackSerializer( SerializationContext ownerContext ) 
			: base( ownerContext, SerializerCapabilities.PackTo | SerializerCapabilities.UnpackFrom ) { }

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override void PackToCore( Packer packer, Plane objectTree )
		{
			packer.PackArrayHeader( 4 );
			packer.Pack( objectTree.Normal.X );
			packer.Pack( objectTree.Normal.Y );
			packer.Pack( objectTree.Normal.Z );
			packer.Pack( objectTree.D );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override Plane UnpackFromCore( Unpacker unpacker )
		{
			if ( !unpacker.IsArrayHeader )
			{
				SerializationExceptions.ThrowInvalidArrayItemsCount( unpacker, typeof( Plane ), 4 );
			}

			var length = UnpackHelpers.GetItemsCount( unpacker );

			if ( length != 4 )
			{
				SerializationExceptions.ThrowInvalidArrayItemsCount( unpacker, typeof( Plane ), 4 );
			}

			float normal_X;
			if ( !unpacker.ReadSingle( out normal_X ) )
			{
				SerializationExceptions.ThrowMissingItem( 0, unpacker );
			}

			float normal_Y;
			if ( !unpacker.ReadSingle( out normal_Y ) )
			{
				SerializationExceptions.ThrowMissingItem( 1, unpacker );
			}

			float normal_Z;
			if ( !unpacker.ReadSingle( out normal_Z ) )
			{
				SerializationExceptions.ThrowMissingItem( 2, unpacker );
			}

			float d;
			if ( !unpacker.ReadSingle( out d ) )
			{
				SerializationExceptions.ThrowMissingItem( 3, unpacker );
			}


			return new Plane( normal_X, normal_Y, normal_Z, d );
		}

#if FEATURE_TAP

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override async Task PackToAsyncCore( Packer packer, Plane objectTree, CancellationToken cancellationToken )
		{
			await packer.PackArrayHeaderAsync( 4, cancellationToken ).ConfigureAwait( false );
			await packer.PackAsync( objectTree.Normal.X, cancellationToken ).ConfigureAwait( false );
			await packer.PackAsync( objectTree.Normal.Y, cancellationToken ).ConfigureAwait( false );
			await packer.PackAsync( objectTree.Normal.Z, cancellationToken ).ConfigureAwait( false );
			await packer.PackAsync( objectTree.D, cancellationToken ).ConfigureAwait( false );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override async Task<Plane> UnpackFromAsyncCore( Unpacker unpacker, CancellationToken cancellationToken )
		{
			if ( !unpacker.IsArrayHeader )
			{
				SerializationExceptions.ThrowInvalidArrayItemsCount( unpacker, typeof( Plane ), 4 );
			}

			var length = unpacker.LastReadData.AsInt64();

			if ( length != 4 )
			{
				SerializationExceptions.ThrowInvalidArrayItemsCount( unpacker, typeof( Plane ), 4 );
			}

			var normal_X = await unpacker.ReadSingleAsync( cancellationToken ).ConfigureAwait( false );
			if ( !normal_X.Success )
			{
				SerializationExceptions.ThrowMissingItem( 0, unpacker );
			}

			var normal_Y = await unpacker.ReadSingleAsync( cancellationToken ).ConfigureAwait( false );
			if ( !normal_Y.Success )
			{
				SerializationExceptions.ThrowMissingItem( 1, unpacker );
			}

			var normal_Z = await unpacker.ReadSingleAsync( cancellationToken ).ConfigureAwait( false );
			if ( !normal_Z.Success )
			{
				SerializationExceptions.ThrowMissingItem( 2, unpacker );
			}

			var d = await unpacker.ReadSingleAsync( cancellationToken ).ConfigureAwait( false );
			if ( !d.Success )
			{
				SerializationExceptions.ThrowMissingItem( 3, unpacker );
			}


			return new Plane( normal_X.Value, normal_Y.Value, normal_Z.Value, d.Value );
		}

#endif // FEATURE_TAP

	}
	internal sealed class System_Numerics_QuaternionMessagePackSerializer : MessagePackSerializer<Quaternion>
	{
		public System_Numerics_QuaternionMessagePackSerializer( SerializationContext ownerContext ) 
			: base( ownerContext, SerializerCapabilities.PackTo | SerializerCapabilities.UnpackFrom ) { }

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override void PackToCore( Packer packer, Quaternion objectTree )
		{
			packer.PackArrayHeader( 4 );
			packer.Pack( objectTree.X );
			packer.Pack( objectTree.Y );
			packer.Pack( objectTree.Z );
			packer.Pack( objectTree.W );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override Quaternion UnpackFromCore( Unpacker unpacker )
		{
			if ( !unpacker.IsArrayHeader )
			{
				SerializationExceptions.ThrowInvalidArrayItemsCount( unpacker, typeof( Quaternion ), 4 );
			}

			var length = UnpackHelpers.GetItemsCount( unpacker );

			if ( length != 4 )
			{
				SerializationExceptions.ThrowInvalidArrayItemsCount( unpacker, typeof( Quaternion ), 4 );
			}

			float x;
			if ( !unpacker.ReadSingle( out x ) )
			{
				SerializationExceptions.ThrowMissingItem( 0, unpacker );
			}

			float y;
			if ( !unpacker.ReadSingle( out y ) )
			{
				SerializationExceptions.ThrowMissingItem( 1, unpacker );
			}

			float z;
			if ( !unpacker.ReadSingle( out z ) )
			{
				SerializationExceptions.ThrowMissingItem( 2, unpacker );
			}

			float w;
			if ( !unpacker.ReadSingle( out w ) )
			{
				SerializationExceptions.ThrowMissingItem( 3, unpacker );
			}


			return new Quaternion( x, y, z, w );
		}

#if FEATURE_TAP

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override async Task PackToAsyncCore( Packer packer, Quaternion objectTree, CancellationToken cancellationToken )
		{
			await packer.PackArrayHeaderAsync( 4, cancellationToken ).ConfigureAwait( false );
			await packer.PackAsync( objectTree.X, cancellationToken ).ConfigureAwait( false );
			await packer.PackAsync( objectTree.Y, cancellationToken ).ConfigureAwait( false );
			await packer.PackAsync( objectTree.Z, cancellationToken ).ConfigureAwait( false );
			await packer.PackAsync( objectTree.W, cancellationToken ).ConfigureAwait( false );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override async Task<Quaternion> UnpackFromAsyncCore( Unpacker unpacker, CancellationToken cancellationToken )
		{
			if ( !unpacker.IsArrayHeader )
			{
				SerializationExceptions.ThrowInvalidArrayItemsCount( unpacker, typeof( Quaternion ), 4 );
			}

			var length = unpacker.LastReadData.AsInt64();

			if ( length != 4 )
			{
				SerializationExceptions.ThrowInvalidArrayItemsCount( unpacker, typeof( Quaternion ), 4 );
			}

			var x = await unpacker.ReadSingleAsync( cancellationToken ).ConfigureAwait( false );
			if ( !x.Success )
			{
				SerializationExceptions.ThrowMissingItem( 0, unpacker );
			}

			var y = await unpacker.ReadSingleAsync( cancellationToken ).ConfigureAwait( false );
			if ( !y.Success )
			{
				SerializationExceptions.ThrowMissingItem( 1, unpacker );
			}

			var z = await unpacker.ReadSingleAsync( cancellationToken ).ConfigureAwait( false );
			if ( !z.Success )
			{
				SerializationExceptions.ThrowMissingItem( 2, unpacker );
			}

			var w = await unpacker.ReadSingleAsync( cancellationToken ).ConfigureAwait( false );
			if ( !w.Success )
			{
				SerializationExceptions.ThrowMissingItem( 3, unpacker );
			}


			return new Quaternion( x.Value, y.Value, z.Value, w.Value );
		}

#endif // FEATURE_TAP

	}
	internal sealed class System_Numerics_Matrix3x2MessagePackSerializer : MessagePackSerializer<Matrix3x2>
	{
		public System_Numerics_Matrix3x2MessagePackSerializer( SerializationContext ownerContext ) 
			: base( ownerContext, SerializerCapabilities.PackTo | SerializerCapabilities.UnpackFrom ) { }

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override void PackToCore( Packer packer, Matrix3x2 objectTree )
		{
			packer.PackArrayHeader( 6 );
			packer.Pack( objectTree.M11 );
			packer.Pack( objectTree.M12 );
			packer.Pack( objectTree.M21 );
			packer.Pack( objectTree.M22 );
			packer.Pack( objectTree.M31 );
			packer.Pack( objectTree.M32 );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override Matrix3x2 UnpackFromCore( Unpacker unpacker )
		{
			if ( !unpacker.IsArrayHeader )
			{
				SerializationExceptions.ThrowInvalidArrayItemsCount( unpacker, typeof( Matrix3x2 ), 6 );
			}

			var length = UnpackHelpers.GetItemsCount( unpacker );

			if ( length != 6 )
			{
				SerializationExceptions.ThrowInvalidArrayItemsCount( unpacker, typeof( Matrix3x2 ), 6 );
			}

			float m11;
			if ( !unpacker.ReadSingle( out m11 ) )
			{
				SerializationExceptions.ThrowMissingItem( 0, unpacker );
			}

			float m12;
			if ( !unpacker.ReadSingle( out m12 ) )
			{
				SerializationExceptions.ThrowMissingItem( 1, unpacker );
			}

			float m21;
			if ( !unpacker.ReadSingle( out m21 ) )
			{
				SerializationExceptions.ThrowMissingItem( 2, unpacker );
			}

			float m22;
			if ( !unpacker.ReadSingle( out m22 ) )
			{
				SerializationExceptions.ThrowMissingItem( 3, unpacker );
			}

			float m31;
			if ( !unpacker.ReadSingle( out m31 ) )
			{
				SerializationExceptions.ThrowMissingItem( 4, unpacker );
			}

			float m32;
			if ( !unpacker.ReadSingle( out m32 ) )
			{
				SerializationExceptions.ThrowMissingItem( 5, unpacker );
			}


			return new Matrix3x2( m11, m12, m21, m22, m31, m32 );
		}

#if FEATURE_TAP

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override async Task PackToAsyncCore( Packer packer, Matrix3x2 objectTree, CancellationToken cancellationToken )
		{
			await packer.PackArrayHeaderAsync( 6, cancellationToken ).ConfigureAwait( false );
			await packer.PackAsync( objectTree.M11, cancellationToken ).ConfigureAwait( false );
			await packer.PackAsync( objectTree.M12, cancellationToken ).ConfigureAwait( false );
			await packer.PackAsync( objectTree.M21, cancellationToken ).ConfigureAwait( false );
			await packer.PackAsync( objectTree.M22, cancellationToken ).ConfigureAwait( false );
			await packer.PackAsync( objectTree.M31, cancellationToken ).ConfigureAwait( false );
			await packer.PackAsync( objectTree.M32, cancellationToken ).ConfigureAwait( false );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override async Task<Matrix3x2> UnpackFromAsyncCore( Unpacker unpacker, CancellationToken cancellationToken )
		{
			if ( !unpacker.IsArrayHeader )
			{
				SerializationExceptions.ThrowInvalidArrayItemsCount( unpacker, typeof( Matrix3x2 ), 6 );
			}

			var length = unpacker.LastReadData.AsInt64();

			if ( length != 6 )
			{
				SerializationExceptions.ThrowInvalidArrayItemsCount( unpacker, typeof( Matrix3x2 ), 6 );
			}

			var m11 = await unpacker.ReadSingleAsync( cancellationToken ).ConfigureAwait( false );
			if ( !m11.Success )
			{
				SerializationExceptions.ThrowMissingItem( 0, unpacker );
			}

			var m12 = await unpacker.ReadSingleAsync( cancellationToken ).ConfigureAwait( false );
			if ( !m12.Success )
			{
				SerializationExceptions.ThrowMissingItem( 1, unpacker );
			}

			var m21 = await unpacker.ReadSingleAsync( cancellationToken ).ConfigureAwait( false );
			if ( !m21.Success )
			{
				SerializationExceptions.ThrowMissingItem( 2, unpacker );
			}

			var m22 = await unpacker.ReadSingleAsync( cancellationToken ).ConfigureAwait( false );
			if ( !m22.Success )
			{
				SerializationExceptions.ThrowMissingItem( 3, unpacker );
			}

			var m31 = await unpacker.ReadSingleAsync( cancellationToken ).ConfigureAwait( false );
			if ( !m31.Success )
			{
				SerializationExceptions.ThrowMissingItem( 4, unpacker );
			}

			var m32 = await unpacker.ReadSingleAsync( cancellationToken ).ConfigureAwait( false );
			if ( !m32.Success )
			{
				SerializationExceptions.ThrowMissingItem( 5, unpacker );
			}


			return new Matrix3x2( m11.Value, m12.Value, m21.Value, m22.Value, m31.Value, m32.Value );
		}

#endif // FEATURE_TAP

	}
	internal sealed class System_Numerics_Matrix4x4MessagePackSerializer : MessagePackSerializer<Matrix4x4>
	{
		public System_Numerics_Matrix4x4MessagePackSerializer( SerializationContext ownerContext ) 
			: base( ownerContext, SerializerCapabilities.PackTo | SerializerCapabilities.UnpackFrom ) { }

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override void PackToCore( Packer packer, Matrix4x4 objectTree )
		{
			packer.PackArrayHeader( 16 );
			packer.Pack( objectTree.M11 );
			packer.Pack( objectTree.M12 );
			packer.Pack( objectTree.M13 );
			packer.Pack( objectTree.M14 );
			packer.Pack( objectTree.M21 );
			packer.Pack( objectTree.M22 );
			packer.Pack( objectTree.M23 );
			packer.Pack( objectTree.M24 );
			packer.Pack( objectTree.M31 );
			packer.Pack( objectTree.M32 );
			packer.Pack( objectTree.M33 );
			packer.Pack( objectTree.M34 );
			packer.Pack( objectTree.M41 );
			packer.Pack( objectTree.M42 );
			packer.Pack( objectTree.M43 );
			packer.Pack( objectTree.M44 );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override Matrix4x4 UnpackFromCore( Unpacker unpacker )
		{
			if ( !unpacker.IsArrayHeader )
			{
				SerializationExceptions.ThrowInvalidArrayItemsCount( unpacker, typeof( Matrix4x4 ), 16 );
			}

			var length = UnpackHelpers.GetItemsCount( unpacker );

			if ( length != 16 )
			{
				SerializationExceptions.ThrowInvalidArrayItemsCount( unpacker, typeof( Matrix4x4 ), 16 );
			}

			float m11;
			if ( !unpacker.ReadSingle( out m11 ) )
			{
				SerializationExceptions.ThrowMissingItem( 0, unpacker );
			}

			float m12;
			if ( !unpacker.ReadSingle( out m12 ) )
			{
				SerializationExceptions.ThrowMissingItem( 1, unpacker );
			}

			float m13;
			if ( !unpacker.ReadSingle( out m13 ) )
			{
				SerializationExceptions.ThrowMissingItem( 2, unpacker );
			}

			float m14;
			if ( !unpacker.ReadSingle( out m14 ) )
			{
				SerializationExceptions.ThrowMissingItem( 3, unpacker );
			}

			float m21;
			if ( !unpacker.ReadSingle( out m21 ) )
			{
				SerializationExceptions.ThrowMissingItem( 4, unpacker );
			}

			float m22;
			if ( !unpacker.ReadSingle( out m22 ) )
			{
				SerializationExceptions.ThrowMissingItem( 5, unpacker );
			}

			float m23;
			if ( !unpacker.ReadSingle( out m23 ) )
			{
				SerializationExceptions.ThrowMissingItem( 6, unpacker );
			}

			float m24;
			if ( !unpacker.ReadSingle( out m24 ) )
			{
				SerializationExceptions.ThrowMissingItem( 7, unpacker );
			}

			float m31;
			if ( !unpacker.ReadSingle( out m31 ) )
			{
				SerializationExceptions.ThrowMissingItem( 8, unpacker );
			}

			float m32;
			if ( !unpacker.ReadSingle( out m32 ) )
			{
				SerializationExceptions.ThrowMissingItem( 9, unpacker );
			}

			float m33;
			if ( !unpacker.ReadSingle( out m33 ) )
			{
				SerializationExceptions.ThrowMissingItem( 10, unpacker );
			}

			float m34;
			if ( !unpacker.ReadSingle( out m34 ) )
			{
				SerializationExceptions.ThrowMissingItem( 11, unpacker );
			}

			float m41;
			if ( !unpacker.ReadSingle( out m41 ) )
			{
				SerializationExceptions.ThrowMissingItem( 12, unpacker );
			}

			float m42;
			if ( !unpacker.ReadSingle( out m42 ) )
			{
				SerializationExceptions.ThrowMissingItem( 13, unpacker );
			}

			float m43;
			if ( !unpacker.ReadSingle( out m43 ) )
			{
				SerializationExceptions.ThrowMissingItem( 14, unpacker );
			}

			float m44;
			if ( !unpacker.ReadSingle( out m44 ) )
			{
				SerializationExceptions.ThrowMissingItem( 15, unpacker );
			}


			return new Matrix4x4( m11, m12, m13, m14, m21, m22, m23, m24, m31, m32, m33, m34, m41, m42, m43, m44 );
		}

#if FEATURE_TAP

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override async Task PackToAsyncCore( Packer packer, Matrix4x4 objectTree, CancellationToken cancellationToken )
		{
			await packer.PackArrayHeaderAsync( 16, cancellationToken ).ConfigureAwait( false );
			await packer.PackAsync( objectTree.M11, cancellationToken ).ConfigureAwait( false );
			await packer.PackAsync( objectTree.M12, cancellationToken ).ConfigureAwait( false );
			await packer.PackAsync( objectTree.M13, cancellationToken ).ConfigureAwait( false );
			await packer.PackAsync( objectTree.M14, cancellationToken ).ConfigureAwait( false );
			await packer.PackAsync( objectTree.M21, cancellationToken ).ConfigureAwait( false );
			await packer.PackAsync( objectTree.M22, cancellationToken ).ConfigureAwait( false );
			await packer.PackAsync( objectTree.M23, cancellationToken ).ConfigureAwait( false );
			await packer.PackAsync( objectTree.M24, cancellationToken ).ConfigureAwait( false );
			await packer.PackAsync( objectTree.M31, cancellationToken ).ConfigureAwait( false );
			await packer.PackAsync( objectTree.M32, cancellationToken ).ConfigureAwait( false );
			await packer.PackAsync( objectTree.M33, cancellationToken ).ConfigureAwait( false );
			await packer.PackAsync( objectTree.M34, cancellationToken ).ConfigureAwait( false );
			await packer.PackAsync( objectTree.M41, cancellationToken ).ConfigureAwait( false );
			await packer.PackAsync( objectTree.M42, cancellationToken ).ConfigureAwait( false );
			await packer.PackAsync( objectTree.M43, cancellationToken ).ConfigureAwait( false );
			await packer.PackAsync( objectTree.M44, cancellationToken ).ConfigureAwait( false );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Validated by caller in base class" )]
		protected internal override async Task<Matrix4x4> UnpackFromAsyncCore( Unpacker unpacker, CancellationToken cancellationToken )
		{
			if ( !unpacker.IsArrayHeader )
			{
				SerializationExceptions.ThrowInvalidArrayItemsCount( unpacker, typeof( Matrix4x4 ), 16 );
			}

			var length = unpacker.LastReadData.AsInt64();

			if ( length != 16 )
			{
				SerializationExceptions.ThrowInvalidArrayItemsCount( unpacker, typeof( Matrix4x4 ), 16 );
			}

			var m11 = await unpacker.ReadSingleAsync( cancellationToken ).ConfigureAwait( false );
			if ( !m11.Success )
			{
				SerializationExceptions.ThrowMissingItem( 0, unpacker );
			}

			var m12 = await unpacker.ReadSingleAsync( cancellationToken ).ConfigureAwait( false );
			if ( !m12.Success )
			{
				SerializationExceptions.ThrowMissingItem( 1, unpacker );
			}

			var m13 = await unpacker.ReadSingleAsync( cancellationToken ).ConfigureAwait( false );
			if ( !m13.Success )
			{
				SerializationExceptions.ThrowMissingItem( 2, unpacker );
			}

			var m14 = await unpacker.ReadSingleAsync( cancellationToken ).ConfigureAwait( false );
			if ( !m14.Success )
			{
				SerializationExceptions.ThrowMissingItem( 3, unpacker );
			}

			var m21 = await unpacker.ReadSingleAsync( cancellationToken ).ConfigureAwait( false );
			if ( !m21.Success )
			{
				SerializationExceptions.ThrowMissingItem( 4, unpacker );
			}

			var m22 = await unpacker.ReadSingleAsync( cancellationToken ).ConfigureAwait( false );
			if ( !m22.Success )
			{
				SerializationExceptions.ThrowMissingItem( 5, unpacker );
			}

			var m23 = await unpacker.ReadSingleAsync( cancellationToken ).ConfigureAwait( false );
			if ( !m23.Success )
			{
				SerializationExceptions.ThrowMissingItem( 6, unpacker );
			}

			var m24 = await unpacker.ReadSingleAsync( cancellationToken ).ConfigureAwait( false );
			if ( !m24.Success )
			{
				SerializationExceptions.ThrowMissingItem( 7, unpacker );
			}

			var m31 = await unpacker.ReadSingleAsync( cancellationToken ).ConfigureAwait( false );
			if ( !m31.Success )
			{
				SerializationExceptions.ThrowMissingItem( 8, unpacker );
			}

			var m32 = await unpacker.ReadSingleAsync( cancellationToken ).ConfigureAwait( false );
			if ( !m32.Success )
			{
				SerializationExceptions.ThrowMissingItem( 9, unpacker );
			}

			var m33 = await unpacker.ReadSingleAsync( cancellationToken ).ConfigureAwait( false );
			if ( !m33.Success )
			{
				SerializationExceptions.ThrowMissingItem( 10, unpacker );
			}

			var m34 = await unpacker.ReadSingleAsync( cancellationToken ).ConfigureAwait( false );
			if ( !m34.Success )
			{
				SerializationExceptions.ThrowMissingItem( 11, unpacker );
			}

			var m41 = await unpacker.ReadSingleAsync( cancellationToken ).ConfigureAwait( false );
			if ( !m41.Success )
			{
				SerializationExceptions.ThrowMissingItem( 12, unpacker );
			}

			var m42 = await unpacker.ReadSingleAsync( cancellationToken ).ConfigureAwait( false );
			if ( !m42.Success )
			{
				SerializationExceptions.ThrowMissingItem( 13, unpacker );
			}

			var m43 = await unpacker.ReadSingleAsync( cancellationToken ).ConfigureAwait( false );
			if ( !m43.Success )
			{
				SerializationExceptions.ThrowMissingItem( 14, unpacker );
			}

			var m44 = await unpacker.ReadSingleAsync( cancellationToken ).ConfigureAwait( false );
			if ( !m44.Success )
			{
				SerializationExceptions.ThrowMissingItem( 15, unpacker );
			}


			return new Matrix4x4( m11.Value, m12.Value, m13.Value, m14.Value, m21.Value, m22.Value, m23.Value, m24.Value, m31.Value, m32.Value, m33.Value, m34.Value, m41.Value, m42.Value, m43.Value, m44.Value );
		}

#endif // FEATURE_TAP

	}

	// ReSharper restore RedundantCast
	// ReSharper restore RedundantNameQualifier
	// ReSharper restore InconsistentNaming
}
#endif // !NETSTANDARD1_1
