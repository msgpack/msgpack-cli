 
#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2014 FUJIWARA, Yusuke
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

namespace MsgPack.Serialization.DefaultSerializers
{
	// This file generated from ArraySerializer.Primitives.tt T4Template.
	// Do not modify this file. Edit DefaultMarshalers.tt instead.
	partial class ArraySerializer
	{
		private static readonly Dictionary<Type, Func<SerializationContext,object>> _arraySerializerFactories =
			InitializeArraySerializerFactories();

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "False positive:Lambda expression" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "False positive:Lambda expression" )]
		private static Dictionary<Type, Func<SerializationContext,object>> InitializeArraySerializerFactories()
		{
			return
				new Dictionary<Type, Func<SerializationContext,object>>( 25 )
				{
					{ typeof( SByte[] ), context => new SByteArraySerializer( context ) },
					{ typeof( SByte?[] ),context => new NullableSByteArraySerializer( context ) },
					{ typeof( Int16[] ), context => new Int16ArraySerializer( context ) },
					{ typeof( Int16?[] ),context => new NullableInt16ArraySerializer( context ) },
					{ typeof( Int32[] ), context => new Int32ArraySerializer( context ) },
					{ typeof( Int32?[] ),context => new NullableInt32ArraySerializer( context ) },
					{ typeof( Int64[] ), context => new Int64ArraySerializer( context ) },
					{ typeof( Int64?[] ),context => new NullableInt64ArraySerializer( context ) },
					{ typeof( Byte[] ), context => new ByteArraySerializer( context ) },
					{ typeof( Byte?[] ),context => new NullableByteArraySerializer( context ) },
					{ typeof( UInt16[] ), context => new UInt16ArraySerializer( context ) },
					{ typeof( UInt16?[] ),context => new NullableUInt16ArraySerializer( context ) },
					{ typeof( UInt32[] ), context => new UInt32ArraySerializer( context ) },
					{ typeof( UInt32?[] ),context => new NullableUInt32ArraySerializer( context ) },
					{ typeof( UInt64[] ), context => new UInt64ArraySerializer( context ) },
					{ typeof( UInt64?[] ),context => new NullableUInt64ArraySerializer( context ) },
					{ typeof( Single[] ), context => new SingleArraySerializer( context ) },
					{ typeof( Single?[] ),context => new NullableSingleArraySerializer( context ) },
					{ typeof( Double[] ), context => new DoubleArraySerializer( context ) },
					{ typeof( Double?[] ),context => new NullableDoubleArraySerializer( context ) },
					{ typeof( Boolean[] ), context => new BooleanArraySerializer( context ) },
					{ typeof( Boolean?[] ),context => new NullableBooleanArraySerializer( context ) },
					{ typeof( string[] ), context => new StringArraySerializer( context ) },
					{ typeof( byte[][] ), context => new BinaryArraySerializer( context ) },
					{ typeof( MessagePackObject[] ), context => new MessagePackObjectArraySerializer( context ) },
				};
		}
	}


internal sealed class SByteArraySerializer : MessagePackSerializer<SByte[]>
{
	public SByteArraySerializer( SerializationContext ownerContext )
		: base ( ownerContext ) { }

	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "By design" )]
	protected internal override void PackToCore( Packer packer, SByte[] objectTree )
	{
		packer.PackArrayHeader( objectTree.Length );
		foreach ( var item in objectTree )
		{
			packer.Pack( item );
		}
	}

	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
	protected internal override SByte[] UnpackFromCore( Unpacker unpacker )
	{
		if ( !unpacker.IsArrayHeader )
		{
			throw SerializationExceptions.NewIsNotArrayHeader();
		}

		var count = UnpackHelpers.GetItemsCount( unpacker );
		var result = new SByte[ count ];
		UnpackToCore( unpacker, result, count );
		return result;
	}

	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
	protected internal override void UnpackToCore( Unpacker unpacker, SByte[] collection )
	{
		if ( !unpacker.IsArrayHeader )
		{
			throw SerializationExceptions.NewIsNotArrayHeader();
		}

		UnpackToCore( unpacker, collection, UnpackHelpers.GetItemsCount( unpacker ) );
	}

	private static void UnpackToCore( Unpacker unpacker, SByte[] collection, int count )
	{
		for ( int i = 0; i < count; i++ )
		{
			SByte item;
			if ( !unpacker.ReadSByte( out item ) )
			{
				throw SerializationExceptions.NewMissingItem( i );
			}

			collection[ i ] = item;
		}
	}
}

internal sealed class Int16ArraySerializer : MessagePackSerializer<Int16[]>
{
	public Int16ArraySerializer( SerializationContext ownerContext )
		: base ( ownerContext ) { }

	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "By design" )]
	protected internal override void PackToCore( Packer packer, Int16[] objectTree )
	{
		packer.PackArrayHeader( objectTree.Length );
		foreach ( var item in objectTree )
		{
			packer.Pack( item );
		}
	}

	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
	protected internal override Int16[] UnpackFromCore( Unpacker unpacker )
	{
		if ( !unpacker.IsArrayHeader )
		{
			throw SerializationExceptions.NewIsNotArrayHeader();
		}

		var count = UnpackHelpers.GetItemsCount( unpacker );
		var result = new Int16[ count ];
		UnpackToCore( unpacker, result, count );
		return result;
	}

	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
	protected internal override void UnpackToCore( Unpacker unpacker, Int16[] collection )
	{
		if ( !unpacker.IsArrayHeader )
		{
			throw SerializationExceptions.NewIsNotArrayHeader();
		}

		UnpackToCore( unpacker, collection, UnpackHelpers.GetItemsCount( unpacker ) );
	}

	private static void UnpackToCore( Unpacker unpacker, Int16[] collection, int count )
	{
		for ( int i = 0; i < count; i++ )
		{
			Int16 item;
			if ( !unpacker.ReadInt16( out item ) )
			{
				throw SerializationExceptions.NewMissingItem( i );
			}

			collection[ i ] = item;
		}
	}
}

internal sealed class Int32ArraySerializer : MessagePackSerializer<Int32[]>
{
	public Int32ArraySerializer( SerializationContext ownerContext )
		: base ( ownerContext ) { }

	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "By design" )]
	protected internal override void PackToCore( Packer packer, Int32[] objectTree )
	{
		packer.PackArrayHeader( objectTree.Length );
		foreach ( var item in objectTree )
		{
			packer.Pack( item );
		}
	}

	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
	protected internal override Int32[] UnpackFromCore( Unpacker unpacker )
	{
		if ( !unpacker.IsArrayHeader )
		{
			throw SerializationExceptions.NewIsNotArrayHeader();
		}

		var count = UnpackHelpers.GetItemsCount( unpacker );
		var result = new Int32[ count ];
		UnpackToCore( unpacker, result, count );
		return result;
	}

	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
	protected internal override void UnpackToCore( Unpacker unpacker, Int32[] collection )
	{
		if ( !unpacker.IsArrayHeader )
		{
			throw SerializationExceptions.NewIsNotArrayHeader();
		}

		UnpackToCore( unpacker, collection, UnpackHelpers.GetItemsCount( unpacker ) );
	}

	private static void UnpackToCore( Unpacker unpacker, Int32[] collection, int count )
	{
		for ( int i = 0; i < count; i++ )
		{
			Int32 item;
			if ( !unpacker.ReadInt32( out item ) )
			{
				throw SerializationExceptions.NewMissingItem( i );
			}

			collection[ i ] = item;
		}
	}
}

internal sealed class Int64ArraySerializer : MessagePackSerializer<Int64[]>
{
	public Int64ArraySerializer( SerializationContext ownerContext )
		: base ( ownerContext ) { }

	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "By design" )]
	protected internal override void PackToCore( Packer packer, Int64[] objectTree )
	{
		packer.PackArrayHeader( objectTree.Length );
		foreach ( var item in objectTree )
		{
			packer.Pack( item );
		}
	}

	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
	protected internal override Int64[] UnpackFromCore( Unpacker unpacker )
	{
		if ( !unpacker.IsArrayHeader )
		{
			throw SerializationExceptions.NewIsNotArrayHeader();
		}

		var count = UnpackHelpers.GetItemsCount( unpacker );
		var result = new Int64[ count ];
		UnpackToCore( unpacker, result, count );
		return result;
	}

	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
	protected internal override void UnpackToCore( Unpacker unpacker, Int64[] collection )
	{
		if ( !unpacker.IsArrayHeader )
		{
			throw SerializationExceptions.NewIsNotArrayHeader();
		}

		UnpackToCore( unpacker, collection, UnpackHelpers.GetItemsCount( unpacker ) );
	}

	private static void UnpackToCore( Unpacker unpacker, Int64[] collection, int count )
	{
		for ( int i = 0; i < count; i++ )
		{
			Int64 item;
			if ( !unpacker.ReadInt64( out item ) )
			{
				throw SerializationExceptions.NewMissingItem( i );
			}

			collection[ i ] = item;
		}
	}
}

internal sealed class ByteArraySerializer : MessagePackSerializer<Byte[]>
{
	public ByteArraySerializer( SerializationContext ownerContext )
		: base ( ownerContext ) { }

	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "By design" )]
	protected internal override void PackToCore( Packer packer, Byte[] objectTree )
	{
		packer.PackArrayHeader( objectTree.Length );
		foreach ( var item in objectTree )
		{
			packer.Pack( item );
		}
	}

	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
	protected internal override Byte[] UnpackFromCore( Unpacker unpacker )
	{
		if ( !unpacker.IsArrayHeader )
		{
			throw SerializationExceptions.NewIsNotArrayHeader();
		}

		var count = UnpackHelpers.GetItemsCount( unpacker );
		var result = new Byte[ count ];
		UnpackToCore( unpacker, result, count );
		return result;
	}

	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
	protected internal override void UnpackToCore( Unpacker unpacker, Byte[] collection )
	{
		if ( !unpacker.IsArrayHeader )
		{
			throw SerializationExceptions.NewIsNotArrayHeader();
		}

		UnpackToCore( unpacker, collection, UnpackHelpers.GetItemsCount( unpacker ) );
	}

	private static void UnpackToCore( Unpacker unpacker, Byte[] collection, int count )
	{
		for ( int i = 0; i < count; i++ )
		{
			Byte item;
			if ( !unpacker.ReadByte( out item ) )
			{
				throw SerializationExceptions.NewMissingItem( i );
			}

			collection[ i ] = item;
		}
	}
}

internal sealed class UInt16ArraySerializer : MessagePackSerializer<UInt16[]>
{
	public UInt16ArraySerializer( SerializationContext ownerContext )
		: base ( ownerContext ) { }

	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "By design" )]
	protected internal override void PackToCore( Packer packer, UInt16[] objectTree )
	{
		packer.PackArrayHeader( objectTree.Length );
		foreach ( var item in objectTree )
		{
			packer.Pack( item );
		}
	}

	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
	protected internal override UInt16[] UnpackFromCore( Unpacker unpacker )
	{
		if ( !unpacker.IsArrayHeader )
		{
			throw SerializationExceptions.NewIsNotArrayHeader();
		}

		var count = UnpackHelpers.GetItemsCount( unpacker );
		var result = new UInt16[ count ];
		UnpackToCore( unpacker, result, count );
		return result;
	}

	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
	protected internal override void UnpackToCore( Unpacker unpacker, UInt16[] collection )
	{
		if ( !unpacker.IsArrayHeader )
		{
			throw SerializationExceptions.NewIsNotArrayHeader();
		}

		UnpackToCore( unpacker, collection, UnpackHelpers.GetItemsCount( unpacker ) );
	}

	private static void UnpackToCore( Unpacker unpacker, UInt16[] collection, int count )
	{
		for ( int i = 0; i < count; i++ )
		{
			UInt16 item;
			if ( !unpacker.ReadUInt16( out item ) )
			{
				throw SerializationExceptions.NewMissingItem( i );
			}

			collection[ i ] = item;
		}
	}
}

internal sealed class UInt32ArraySerializer : MessagePackSerializer<UInt32[]>
{
	public UInt32ArraySerializer( SerializationContext ownerContext )
		: base ( ownerContext ) { }

	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "By design" )]
	protected internal override void PackToCore( Packer packer, UInt32[] objectTree )
	{
		packer.PackArrayHeader( objectTree.Length );
		foreach ( var item in objectTree )
		{
			packer.Pack( item );
		}
	}

	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
	protected internal override UInt32[] UnpackFromCore( Unpacker unpacker )
	{
		if ( !unpacker.IsArrayHeader )
		{
			throw SerializationExceptions.NewIsNotArrayHeader();
		}

		var count = UnpackHelpers.GetItemsCount( unpacker );
		var result = new UInt32[ count ];
		UnpackToCore( unpacker, result, count );
		return result;
	}

	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
	protected internal override void UnpackToCore( Unpacker unpacker, UInt32[] collection )
	{
		if ( !unpacker.IsArrayHeader )
		{
			throw SerializationExceptions.NewIsNotArrayHeader();
		}

		UnpackToCore( unpacker, collection, UnpackHelpers.GetItemsCount( unpacker ) );
	}

	private static void UnpackToCore( Unpacker unpacker, UInt32[] collection, int count )
	{
		for ( int i = 0; i < count; i++ )
		{
			UInt32 item;
			if ( !unpacker.ReadUInt32( out item ) )
			{
				throw SerializationExceptions.NewMissingItem( i );
			}

			collection[ i ] = item;
		}
	}
}

internal sealed class UInt64ArraySerializer : MessagePackSerializer<UInt64[]>
{
	public UInt64ArraySerializer( SerializationContext ownerContext )
		: base ( ownerContext ) { }

	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "By design" )]
	protected internal override void PackToCore( Packer packer, UInt64[] objectTree )
	{
		packer.PackArrayHeader( objectTree.Length );
		foreach ( var item in objectTree )
		{
			packer.Pack( item );
		}
	}

	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
	protected internal override UInt64[] UnpackFromCore( Unpacker unpacker )
	{
		if ( !unpacker.IsArrayHeader )
		{
			throw SerializationExceptions.NewIsNotArrayHeader();
		}

		var count = UnpackHelpers.GetItemsCount( unpacker );
		var result = new UInt64[ count ];
		UnpackToCore( unpacker, result, count );
		return result;
	}

	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
	protected internal override void UnpackToCore( Unpacker unpacker, UInt64[] collection )
	{
		if ( !unpacker.IsArrayHeader )
		{
			throw SerializationExceptions.NewIsNotArrayHeader();
		}

		UnpackToCore( unpacker, collection, UnpackHelpers.GetItemsCount( unpacker ) );
	}

	private static void UnpackToCore( Unpacker unpacker, UInt64[] collection, int count )
	{
		for ( int i = 0; i < count; i++ )
		{
			UInt64 item;
			if ( !unpacker.ReadUInt64( out item ) )
			{
				throw SerializationExceptions.NewMissingItem( i );
			}

			collection[ i ] = item;
		}
	}
}

internal sealed class SingleArraySerializer : MessagePackSerializer<Single[]>
{
	public SingleArraySerializer( SerializationContext ownerContext )
		: base ( ownerContext ) { }

	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "By design" )]
	protected internal override void PackToCore( Packer packer, Single[] objectTree )
	{
		packer.PackArrayHeader( objectTree.Length );
		foreach ( var item in objectTree )
		{
			packer.Pack( item );
		}
	}

	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
	protected internal override Single[] UnpackFromCore( Unpacker unpacker )
	{
		if ( !unpacker.IsArrayHeader )
		{
			throw SerializationExceptions.NewIsNotArrayHeader();
		}

		var count = UnpackHelpers.GetItemsCount( unpacker );
		var result = new Single[ count ];
		UnpackToCore( unpacker, result, count );
		return result;
	}

	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
	protected internal override void UnpackToCore( Unpacker unpacker, Single[] collection )
	{
		if ( !unpacker.IsArrayHeader )
		{
			throw SerializationExceptions.NewIsNotArrayHeader();
		}

		UnpackToCore( unpacker, collection, UnpackHelpers.GetItemsCount( unpacker ) );
	}

	private static void UnpackToCore( Unpacker unpacker, Single[] collection, int count )
	{
		for ( int i = 0; i < count; i++ )
		{
			Single item;
			if ( !unpacker.ReadSingle( out item ) )
			{
				throw SerializationExceptions.NewMissingItem( i );
			}

			collection[ i ] = item;
		}
	}
}

internal sealed class DoubleArraySerializer : MessagePackSerializer<Double[]>
{
	public DoubleArraySerializer( SerializationContext ownerContext )
		: base ( ownerContext ) { }

	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "By design" )]
	protected internal override void PackToCore( Packer packer, Double[] objectTree )
	{
		packer.PackArrayHeader( objectTree.Length );
		foreach ( var item in objectTree )
		{
			packer.Pack( item );
		}
	}

	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
	protected internal override Double[] UnpackFromCore( Unpacker unpacker )
	{
		if ( !unpacker.IsArrayHeader )
		{
			throw SerializationExceptions.NewIsNotArrayHeader();
		}

		var count = UnpackHelpers.GetItemsCount( unpacker );
		var result = new Double[ count ];
		UnpackToCore( unpacker, result, count );
		return result;
	}

	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
	protected internal override void UnpackToCore( Unpacker unpacker, Double[] collection )
	{
		if ( !unpacker.IsArrayHeader )
		{
			throw SerializationExceptions.NewIsNotArrayHeader();
		}

		UnpackToCore( unpacker, collection, UnpackHelpers.GetItemsCount( unpacker ) );
	}

	private static void UnpackToCore( Unpacker unpacker, Double[] collection, int count )
	{
		for ( int i = 0; i < count; i++ )
		{
			Double item;
			if ( !unpacker.ReadDouble( out item ) )
			{
				throw SerializationExceptions.NewMissingItem( i );
			}

			collection[ i ] = item;
		}
	}
}

internal sealed class BooleanArraySerializer : MessagePackSerializer<Boolean[]>
{
	public BooleanArraySerializer( SerializationContext ownerContext )
		: base ( ownerContext ) { }

	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "By design" )]
	protected internal override void PackToCore( Packer packer, Boolean[] objectTree )
	{
		packer.PackArrayHeader( objectTree.Length );
		foreach ( var item in objectTree )
		{
			packer.Pack( item );
		}
	}

	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
	protected internal override Boolean[] UnpackFromCore( Unpacker unpacker )
	{
		if ( !unpacker.IsArrayHeader )
		{
			throw SerializationExceptions.NewIsNotArrayHeader();
		}

		var count = UnpackHelpers.GetItemsCount( unpacker );
		var result = new Boolean[ count ];
		UnpackToCore( unpacker, result, count );
		return result;
	}

	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
	protected internal override void UnpackToCore( Unpacker unpacker, Boolean[] collection )
	{
		if ( !unpacker.IsArrayHeader )
		{
			throw SerializationExceptions.NewIsNotArrayHeader();
		}

		UnpackToCore( unpacker, collection, UnpackHelpers.GetItemsCount( unpacker ) );
	}

	private static void UnpackToCore( Unpacker unpacker, Boolean[] collection, int count )
	{
		for ( int i = 0; i < count; i++ )
		{
			Boolean item;
			if ( !unpacker.ReadBoolean( out item ) )
			{
				throw SerializationExceptions.NewMissingItem( i );
			}

			collection[ i ] = item;
		}
	}
}

internal sealed class NullableSByteArraySerializer : MessagePackSerializer<SByte?[]>
{
	public NullableSByteArraySerializer( SerializationContext ownerContext )
		: base ( ownerContext ) { }

	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "By design" )]
	protected internal override void PackToCore( Packer packer, SByte?[] objectTree )
	{
		packer.PackArrayHeader( objectTree.Length );
		foreach ( var item in objectTree )
		{
			packer.Pack( item );
		}
	}

	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
	protected internal override SByte?[] UnpackFromCore( Unpacker unpacker )
	{
		if ( !unpacker.IsArrayHeader )
		{
			throw SerializationExceptions.NewIsNotArrayHeader();
		}

		var count = UnpackHelpers.GetItemsCount( unpacker );
		var result = new SByte?[ count ];
		UnpackToCore( unpacker, result, count );
		return result;
	}

	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
	protected internal override void UnpackToCore( Unpacker unpacker, SByte?[] collection )
	{
		if ( !unpacker.IsArrayHeader )
		{
			throw SerializationExceptions.NewIsNotArrayHeader();
		}

		UnpackToCore( unpacker, collection, UnpackHelpers.GetItemsCount( unpacker ) );
	}

	private static void UnpackToCore( Unpacker unpacker, SByte?[] collection, int count )
	{
		for ( int i = 0; i < count; i++ )
		{
			SByte? item;
			if ( !unpacker.ReadNullableSByte( out item ) )
			{
				throw SerializationExceptions.NewMissingItem( i );
			}

			collection[ i ] = item;
		}
	}
}

internal sealed class NullableInt16ArraySerializer : MessagePackSerializer<Int16?[]>
{
	public NullableInt16ArraySerializer( SerializationContext ownerContext )
		: base ( ownerContext ) { }

	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "By design" )]
	protected internal override void PackToCore( Packer packer, Int16?[] objectTree )
	{
		packer.PackArrayHeader( objectTree.Length );
		foreach ( var item in objectTree )
		{
			packer.Pack( item );
		}
	}

	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
	protected internal override Int16?[] UnpackFromCore( Unpacker unpacker )
	{
		if ( !unpacker.IsArrayHeader )
		{
			throw SerializationExceptions.NewIsNotArrayHeader();
		}

		var count = UnpackHelpers.GetItemsCount( unpacker );
		var result = new Int16?[ count ];
		UnpackToCore( unpacker, result, count );
		return result;
	}

	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
	protected internal override void UnpackToCore( Unpacker unpacker, Int16?[] collection )
	{
		if ( !unpacker.IsArrayHeader )
		{
			throw SerializationExceptions.NewIsNotArrayHeader();
		}

		UnpackToCore( unpacker, collection, UnpackHelpers.GetItemsCount( unpacker ) );
	}

	private static void UnpackToCore( Unpacker unpacker, Int16?[] collection, int count )
	{
		for ( int i = 0; i < count; i++ )
		{
			Int16? item;
			if ( !unpacker.ReadNullableInt16( out item ) )
			{
				throw SerializationExceptions.NewMissingItem( i );
			}

			collection[ i ] = item;
		}
	}
}

internal sealed class NullableInt32ArraySerializer : MessagePackSerializer<Int32?[]>
{
	public NullableInt32ArraySerializer( SerializationContext ownerContext )
		: base ( ownerContext ) { }

	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "By design" )]
	protected internal override void PackToCore( Packer packer, Int32?[] objectTree )
	{
		packer.PackArrayHeader( objectTree.Length );
		foreach ( var item in objectTree )
		{
			packer.Pack( item );
		}
	}

	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
	protected internal override Int32?[] UnpackFromCore( Unpacker unpacker )
	{
		if ( !unpacker.IsArrayHeader )
		{
			throw SerializationExceptions.NewIsNotArrayHeader();
		}

		var count = UnpackHelpers.GetItemsCount( unpacker );
		var result = new Int32?[ count ];
		UnpackToCore( unpacker, result, count );
		return result;
	}

	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
	protected internal override void UnpackToCore( Unpacker unpacker, Int32?[] collection )
	{
		if ( !unpacker.IsArrayHeader )
		{
			throw SerializationExceptions.NewIsNotArrayHeader();
		}

		UnpackToCore( unpacker, collection, UnpackHelpers.GetItemsCount( unpacker ) );
	}

	private static void UnpackToCore( Unpacker unpacker, Int32?[] collection, int count )
	{
		for ( int i = 0; i < count; i++ )
		{
			Int32? item;
			if ( !unpacker.ReadNullableInt32( out item ) )
			{
				throw SerializationExceptions.NewMissingItem( i );
			}

			collection[ i ] = item;
		}
	}
}

internal sealed class NullableInt64ArraySerializer : MessagePackSerializer<Int64?[]>
{
	public NullableInt64ArraySerializer( SerializationContext ownerContext )
		: base ( ownerContext ) { }

	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "By design" )]
	protected internal override void PackToCore( Packer packer, Int64?[] objectTree )
	{
		packer.PackArrayHeader( objectTree.Length );
		foreach ( var item in objectTree )
		{
			packer.Pack( item );
		}
	}

	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
	protected internal override Int64?[] UnpackFromCore( Unpacker unpacker )
	{
		if ( !unpacker.IsArrayHeader )
		{
			throw SerializationExceptions.NewIsNotArrayHeader();
		}

		var count = UnpackHelpers.GetItemsCount( unpacker );
		var result = new Int64?[ count ];
		UnpackToCore( unpacker, result, count );
		return result;
	}

	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
	protected internal override void UnpackToCore( Unpacker unpacker, Int64?[] collection )
	{
		if ( !unpacker.IsArrayHeader )
		{
			throw SerializationExceptions.NewIsNotArrayHeader();
		}

		UnpackToCore( unpacker, collection, UnpackHelpers.GetItemsCount( unpacker ) );
	}

	private static void UnpackToCore( Unpacker unpacker, Int64?[] collection, int count )
	{
		for ( int i = 0; i < count; i++ )
		{
			Int64? item;
			if ( !unpacker.ReadNullableInt64( out item ) )
			{
				throw SerializationExceptions.NewMissingItem( i );
			}

			collection[ i ] = item;
		}
	}
}

internal sealed class NullableByteArraySerializer : MessagePackSerializer<Byte?[]>
{
	public NullableByteArraySerializer( SerializationContext ownerContext )
		: base ( ownerContext ) { }

	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "By design" )]
	protected internal override void PackToCore( Packer packer, Byte?[] objectTree )
	{
		packer.PackArrayHeader( objectTree.Length );
		foreach ( var item in objectTree )
		{
			packer.Pack( item );
		}
	}

	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
	protected internal override Byte?[] UnpackFromCore( Unpacker unpacker )
	{
		if ( !unpacker.IsArrayHeader )
		{
			throw SerializationExceptions.NewIsNotArrayHeader();
		}

		var count = UnpackHelpers.GetItemsCount( unpacker );
		var result = new Byte?[ count ];
		UnpackToCore( unpacker, result, count );
		return result;
	}

	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
	protected internal override void UnpackToCore( Unpacker unpacker, Byte?[] collection )
	{
		if ( !unpacker.IsArrayHeader )
		{
			throw SerializationExceptions.NewIsNotArrayHeader();
		}

		UnpackToCore( unpacker, collection, UnpackHelpers.GetItemsCount( unpacker ) );
	}

	private static void UnpackToCore( Unpacker unpacker, Byte?[] collection, int count )
	{
		for ( int i = 0; i < count; i++ )
		{
			Byte? item;
			if ( !unpacker.ReadNullableByte( out item ) )
			{
				throw SerializationExceptions.NewMissingItem( i );
			}

			collection[ i ] = item;
		}
	}
}

internal sealed class NullableUInt16ArraySerializer : MessagePackSerializer<UInt16?[]>
{
	public NullableUInt16ArraySerializer( SerializationContext ownerContext )
		: base ( ownerContext ) { }

	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "By design" )]
	protected internal override void PackToCore( Packer packer, UInt16?[] objectTree )
	{
		packer.PackArrayHeader( objectTree.Length );
		foreach ( var item in objectTree )
		{
			packer.Pack( item );
		}
	}

	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
	protected internal override UInt16?[] UnpackFromCore( Unpacker unpacker )
	{
		if ( !unpacker.IsArrayHeader )
		{
			throw SerializationExceptions.NewIsNotArrayHeader();
		}

		var count = UnpackHelpers.GetItemsCount( unpacker );
		var result = new UInt16?[ count ];
		UnpackToCore( unpacker, result, count );
		return result;
	}

	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
	protected internal override void UnpackToCore( Unpacker unpacker, UInt16?[] collection )
	{
		if ( !unpacker.IsArrayHeader )
		{
			throw SerializationExceptions.NewIsNotArrayHeader();
		}

		UnpackToCore( unpacker, collection, UnpackHelpers.GetItemsCount( unpacker ) );
	}

	private static void UnpackToCore( Unpacker unpacker, UInt16?[] collection, int count )
	{
		for ( int i = 0; i < count; i++ )
		{
			UInt16? item;
			if ( !unpacker.ReadNullableUInt16( out item ) )
			{
				throw SerializationExceptions.NewMissingItem( i );
			}

			collection[ i ] = item;
		}
	}
}

internal sealed class NullableUInt32ArraySerializer : MessagePackSerializer<UInt32?[]>
{
	public NullableUInt32ArraySerializer( SerializationContext ownerContext )
		: base ( ownerContext ) { }

	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "By design" )]
	protected internal override void PackToCore( Packer packer, UInt32?[] objectTree )
	{
		packer.PackArrayHeader( objectTree.Length );
		foreach ( var item in objectTree )
		{
			packer.Pack( item );
		}
	}

	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
	protected internal override UInt32?[] UnpackFromCore( Unpacker unpacker )
	{
		if ( !unpacker.IsArrayHeader )
		{
			throw SerializationExceptions.NewIsNotArrayHeader();
		}

		var count = UnpackHelpers.GetItemsCount( unpacker );
		var result = new UInt32?[ count ];
		UnpackToCore( unpacker, result, count );
		return result;
	}

	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
	protected internal override void UnpackToCore( Unpacker unpacker, UInt32?[] collection )
	{
		if ( !unpacker.IsArrayHeader )
		{
			throw SerializationExceptions.NewIsNotArrayHeader();
		}

		UnpackToCore( unpacker, collection, UnpackHelpers.GetItemsCount( unpacker ) );
	}

	private static void UnpackToCore( Unpacker unpacker, UInt32?[] collection, int count )
	{
		for ( int i = 0; i < count; i++ )
		{
			UInt32? item;
			if ( !unpacker.ReadNullableUInt32( out item ) )
			{
				throw SerializationExceptions.NewMissingItem( i );
			}

			collection[ i ] = item;
		}
	}
}

internal sealed class NullableUInt64ArraySerializer : MessagePackSerializer<UInt64?[]>
{
	public NullableUInt64ArraySerializer( SerializationContext ownerContext )
		: base ( ownerContext ) { }

	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "By design" )]
	protected internal override void PackToCore( Packer packer, UInt64?[] objectTree )
	{
		packer.PackArrayHeader( objectTree.Length );
		foreach ( var item in objectTree )
		{
			packer.Pack( item );
		}
	}

	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
	protected internal override UInt64?[] UnpackFromCore( Unpacker unpacker )
	{
		if ( !unpacker.IsArrayHeader )
		{
			throw SerializationExceptions.NewIsNotArrayHeader();
		}

		var count = UnpackHelpers.GetItemsCount( unpacker );
		var result = new UInt64?[ count ];
		UnpackToCore( unpacker, result, count );
		return result;
	}

	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
	protected internal override void UnpackToCore( Unpacker unpacker, UInt64?[] collection )
	{
		if ( !unpacker.IsArrayHeader )
		{
			throw SerializationExceptions.NewIsNotArrayHeader();
		}

		UnpackToCore( unpacker, collection, UnpackHelpers.GetItemsCount( unpacker ) );
	}

	private static void UnpackToCore( Unpacker unpacker, UInt64?[] collection, int count )
	{
		for ( int i = 0; i < count; i++ )
		{
			UInt64? item;
			if ( !unpacker.ReadNullableUInt64( out item ) )
			{
				throw SerializationExceptions.NewMissingItem( i );
			}

			collection[ i ] = item;
		}
	}
}

internal sealed class NullableSingleArraySerializer : MessagePackSerializer<Single?[]>
{
	public NullableSingleArraySerializer( SerializationContext ownerContext )
		: base ( ownerContext ) { }

	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "By design" )]
	protected internal override void PackToCore( Packer packer, Single?[] objectTree )
	{
		packer.PackArrayHeader( objectTree.Length );
		foreach ( var item in objectTree )
		{
			packer.Pack( item );
		}
	}

	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
	protected internal override Single?[] UnpackFromCore( Unpacker unpacker )
	{
		if ( !unpacker.IsArrayHeader )
		{
			throw SerializationExceptions.NewIsNotArrayHeader();
		}

		var count = UnpackHelpers.GetItemsCount( unpacker );
		var result = new Single?[ count ];
		UnpackToCore( unpacker, result, count );
		return result;
	}

	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
	protected internal override void UnpackToCore( Unpacker unpacker, Single?[] collection )
	{
		if ( !unpacker.IsArrayHeader )
		{
			throw SerializationExceptions.NewIsNotArrayHeader();
		}

		UnpackToCore( unpacker, collection, UnpackHelpers.GetItemsCount( unpacker ) );
	}

	private static void UnpackToCore( Unpacker unpacker, Single?[] collection, int count )
	{
		for ( int i = 0; i < count; i++ )
		{
			Single? item;
			if ( !unpacker.ReadNullableSingle( out item ) )
			{
				throw SerializationExceptions.NewMissingItem( i );
			}

			collection[ i ] = item;
		}
	}
}

internal sealed class NullableDoubleArraySerializer : MessagePackSerializer<Double?[]>
{
	public NullableDoubleArraySerializer( SerializationContext ownerContext )
		: base ( ownerContext ) { }

	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "By design" )]
	protected internal override void PackToCore( Packer packer, Double?[] objectTree )
	{
		packer.PackArrayHeader( objectTree.Length );
		foreach ( var item in objectTree )
		{
			packer.Pack( item );
		}
	}

	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
	protected internal override Double?[] UnpackFromCore( Unpacker unpacker )
	{
		if ( !unpacker.IsArrayHeader )
		{
			throw SerializationExceptions.NewIsNotArrayHeader();
		}

		var count = UnpackHelpers.GetItemsCount( unpacker );
		var result = new Double?[ count ];
		UnpackToCore( unpacker, result, count );
		return result;
	}

	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
	protected internal override void UnpackToCore( Unpacker unpacker, Double?[] collection )
	{
		if ( !unpacker.IsArrayHeader )
		{
			throw SerializationExceptions.NewIsNotArrayHeader();
		}

		UnpackToCore( unpacker, collection, UnpackHelpers.GetItemsCount( unpacker ) );
	}

	private static void UnpackToCore( Unpacker unpacker, Double?[] collection, int count )
	{
		for ( int i = 0; i < count; i++ )
		{
			Double? item;
			if ( !unpacker.ReadNullableDouble( out item ) )
			{
				throw SerializationExceptions.NewMissingItem( i );
			}

			collection[ i ] = item;
		}
	}
}

internal sealed class NullableBooleanArraySerializer : MessagePackSerializer<Boolean?[]>
{
	public NullableBooleanArraySerializer( SerializationContext ownerContext )
		: base ( ownerContext ) { }

	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "By design" )]
	protected internal override void PackToCore( Packer packer, Boolean?[] objectTree )
	{
		packer.PackArrayHeader( objectTree.Length );
		foreach ( var item in objectTree )
		{
			packer.Pack( item );
		}
	}

	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
	protected internal override Boolean?[] UnpackFromCore( Unpacker unpacker )
	{
		if ( !unpacker.IsArrayHeader )
		{
			throw SerializationExceptions.NewIsNotArrayHeader();
		}

		var count = UnpackHelpers.GetItemsCount( unpacker );
		var result = new Boolean?[ count ];
		UnpackToCore( unpacker, result, count );
		return result;
	}

	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
	protected internal override void UnpackToCore( Unpacker unpacker, Boolean?[] collection )
	{
		if ( !unpacker.IsArrayHeader )
		{
			throw SerializationExceptions.NewIsNotArrayHeader();
		}

		UnpackToCore( unpacker, collection, UnpackHelpers.GetItemsCount( unpacker ) );
	}

	private static void UnpackToCore( Unpacker unpacker, Boolean?[] collection, int count )
	{
		for ( int i = 0; i < count; i++ )
		{
			Boolean? item;
			if ( !unpacker.ReadNullableBoolean( out item ) )
			{
				throw SerializationExceptions.NewMissingItem( i );
			}

			collection[ i ] = item;
		}
	}
}

internal sealed class StringArraySerializer : MessagePackSerializer<String[]>
{
	public StringArraySerializer( SerializationContext ownerContext )
		: base ( ownerContext ) { }

	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "By design" )]
	protected internal override void PackToCore( Packer packer, String[] objectTree )
	{
		packer.PackArrayHeader( objectTree.Length );
		foreach ( var item in objectTree )
		{
			packer.PackString( item );
		}
	}

	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
	protected internal override String[] UnpackFromCore( Unpacker unpacker )
	{
		if ( !unpacker.IsArrayHeader )
		{
			throw SerializationExceptions.NewIsNotArrayHeader();
		}

		var count = UnpackHelpers.GetItemsCount( unpacker );
		var result = new String[ count ];
		UnpackToCore( unpacker, result, count );
		return result;
	}

	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
	protected internal override void UnpackToCore( Unpacker unpacker, String[] collection )
	{
		if ( !unpacker.IsArrayHeader )
		{
			throw SerializationExceptions.NewIsNotArrayHeader();
		}

		UnpackToCore( unpacker, collection, UnpackHelpers.GetItemsCount( unpacker ) );
	}

	private static void UnpackToCore( Unpacker unpacker, String[] collection, int count )
	{
		for ( int i = 0; i < count; i++ )
		{
			String item;
			if ( !unpacker.ReadString( out item ) )
			{
				throw SerializationExceptions.NewMissingItem( i );
			}

			collection[ i ] = item;
		}
	}
}

internal sealed class BinaryArraySerializer : MessagePackSerializer<Byte[][]>
{
	public BinaryArraySerializer( SerializationContext ownerContext )
		: base ( ownerContext ) { }

	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "By design" )]
	protected internal override void PackToCore( Packer packer, Byte[][] objectTree )
	{
		packer.PackArrayHeader( objectTree.Length );
		foreach ( var item in objectTree )
		{
			packer.PackBinary( item );
		}
	}

	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
	protected internal override Byte[][] UnpackFromCore( Unpacker unpacker )
	{
		if ( !unpacker.IsArrayHeader )
		{
			throw SerializationExceptions.NewIsNotArrayHeader();
		}

		var count = UnpackHelpers.GetItemsCount( unpacker );
		var result = new Byte[ count ][];
		UnpackToCore( unpacker, result, count );
		return result;
	}

	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
	protected internal override void UnpackToCore( Unpacker unpacker, Byte[][] collection )
	{
		if ( !unpacker.IsArrayHeader )
		{
			throw SerializationExceptions.NewIsNotArrayHeader();
		}

		UnpackToCore( unpacker, collection, UnpackHelpers.GetItemsCount( unpacker ) );
	}

	private static void UnpackToCore( Unpacker unpacker, Byte[][] collection, int count )
	{
		for ( int i = 0; i < count; i++ )
		{
			Byte[] item;
			if ( !unpacker.ReadBinary( out item ) )
			{
				throw SerializationExceptions.NewMissingItem( i );
			}

			collection[ i ] = item;
		}
	}
}

internal sealed class MessagePackObjectArraySerializer : MessagePackSerializer<MessagePackObject[]>
{
	public MessagePackObjectArraySerializer( SerializationContext ownerContext )
		: base ( ownerContext ) { }

	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "1", Justification = "By design" )]
	protected internal override void PackToCore( Packer packer, MessagePackObject[] objectTree )
	{
		packer.PackArrayHeader( objectTree.Length );
		foreach ( var item in objectTree )
		{
			item.PackToMessage( packer, null );
		}
	}

	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
	protected internal override MessagePackObject[] UnpackFromCore( Unpacker unpacker )
	{
		if ( !unpacker.IsArrayHeader )
		{
			throw SerializationExceptions.NewIsNotArrayHeader();
		}

		var count = UnpackHelpers.GetItemsCount( unpacker );
		var result = new MessagePackObject[ count ];
		UnpackToCore( unpacker, result, count );
		return result;
	}

	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
	protected internal override void UnpackToCore( Unpacker unpacker, MessagePackObject[] collection )
	{
		if ( !unpacker.IsArrayHeader )
		{
			throw SerializationExceptions.NewIsNotArrayHeader();
		}

		UnpackToCore( unpacker, collection, UnpackHelpers.GetItemsCount( unpacker ) );
	}

	private static void UnpackToCore( Unpacker unpacker, MessagePackObject[] collection, int count )
	{
		for ( int i = 0; i < count; i++ )
		{
			MessagePackObject item;
			if ( !unpacker.ReadObject( out item ) )
			{
				throw SerializationExceptions.NewMissingItem( i );
			}

			collection[ i ] = item;
		}
	}
}

}
