#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2015 FUJIWARA, Yusuke
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
using System.IO;

namespace MsgPack.Serialization.AbstractSerializers
{
	/// <summary>
	///		Represents dictionary key to remember fields which store dependent serializer instance.
	/// </summary>
	internal sealed class SerializerFieldKey : IEquatable<SerializerFieldKey>
	{
		/// <summary>
		///		Type of serializing/deserializing type. 
		/// </summary>
		public readonly RuntimeTypeHandle TypeHandle;

		/// <summary>
		///		Enum serialization method for specific member.
		/// </summary>
		public readonly EnumMemberSerializationMethod EnumSerializationMethod;

		/// <summary>
		///		DateTime conversion method for specific member.
		/// </summary>
		public readonly DateTimeMemberConversionMethod DateTimeConversionMethod;

		private readonly ComparablePolymorphismSchema _schema;

		/// <summary>
		///		<see cref="PolymorphismSchema"/> for specific member. <c>null</c> for non-polymorphic member.
		/// </summary>
		public PolymorphismSchema PolymorphismSchema { get { return this._schema.Value; } }

		public SerializerFieldKey( Type targetType, EnumMemberSerializationMethod enumMemberSerializationMethod, DateTimeMemberConversionMethod dateTimeConversionMethod, PolymorphismSchema polymorphismSchema )
		{
			this.TypeHandle = targetType.TypeHandle;
			this.EnumSerializationMethod = enumMemberSerializationMethod;
			this.DateTimeConversionMethod = dateTimeConversionMethod;
			this._schema = new ComparablePolymorphismSchema( polymorphismSchema );
		}

		public bool Equals( SerializerFieldKey other )
		{
			// ReSharper disable once ImpureMethodCallOnReadonlyValueField
			return
				other != null
				&& this.TypeHandle.Equals( other.TypeHandle )
				&& this.EnumSerializationMethod == other.EnumSerializationMethod
				&& this.DateTimeConversionMethod == other.DateTimeConversionMethod
				&& this._schema.Equals( other._schema );
		}

		public override bool Equals( object obj )
		{
			return this.Equals( obj as SerializerFieldKey );
		}

		public override int GetHashCode()
		{
			return this.TypeHandle.GetHashCode() ^ this.EnumSerializationMethod.GetHashCode() ^ this.DateTimeConversionMethod.GetHashCode() ^ this._schema.GetHashCode();
		}

		/// <summary>
		///		Comparable <see cref="PolymorphismSchema"/>.
		/// </summary>
		/// <remarks>
		///		<see cref="SerializerFieldKey"/> must use <see cref="PolymorphismSchema"/> to distinct between shared serializer and non-sharable serializer because of its polymorphism.
		/// </remarks>
		private struct ComparablePolymorphismSchema : IEquatable<ComparablePolymorphismSchema>
		{
			// Helper for fast comparison, msgpack serialized PolymorphismSchema.
			private readonly MessagePackString _key;
			private readonly PolymorphismSchema _value;

			public PolymorphismSchema Value { get { return this._value; } }

			public ComparablePolymorphismSchema( PolymorphismSchema value )
			{
				this._value = value;
				this._key = new MessagePackString( Pack( value ), true );
			}

			[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Usage", "CA2202:DoNotDisposeObjectsMultipleTimes", Justification = "Avoided via ownsStream: false" )]
			private static byte[] Pack( PolymorphismSchema value )
			{
				using ( var buffer = new MemoryStream() )
				using ( var packer = Packer.Create( buffer, PackerCompatibilityOptions.None, ownsStream: false ) )
				{
					Pack( packer, value );
					return buffer.ToArray();
				}
			}

			private static void Pack( Packer packer, PolymorphismSchema value )
			{
				if ( value == null )
				{
					packer.PackNull();
					return;
				}

				packer.PackArrayHeader( 4 );
				if ( value.TargetType == null )
				{
					packer.PackNull();
				}
				else
				{
					packer.PackString( value.TargetType.AssemblyQualifiedName );
				}
				packer.Pack( ( int )value.ChildrenType );

				packer.PackMapHeader( value.CodeTypeMapping.Count );
				foreach ( var mapping in value.CodeTypeMapping )
				{
					packer.PackString( mapping.Key );
					packer.PackString( mapping.Value.AssemblyQualifiedName );
				}

				packer.PackArrayHeader( value.ChildSchemaList.Count );
				foreach ( var childSchema in value.ChildSchemaList )
				{
					Pack( packer, childSchema );
				}
			}

			public override bool Equals( object obj )
			{
				if ( ( obj is ComparablePolymorphismSchema ) )
				{
					return this.Equals( ( ComparablePolymorphismSchema ) obj );
				}

				return false;
			}

			public bool Equals( ComparablePolymorphismSchema other )
			{
				if ( this._key == null )
				{
					return other._key == null;
				}
				else
				{
					return this._key.Equals( other._key );
				}
			}

			public override int GetHashCode()
			{
				return this._key.GetHashCode();
			}
		}
	}
}