#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010 FUJIWARA, Yusuke
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
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace MsgPack.Serialization
{
#warning TODO:comment
	/// <summary>
	///		Build serializer for <typeparamref name="TObject"/>.
	/// </summary>
	/// <typeparam name="TObject">Object to be serialized/deserialized.</typeparam>
	internal abstract class SerializerBuilder<TObject>
	{
		public bool CreateProcedures( SerializationMemberOption option, out Action<Packer, TObject, SerializationContext> packing, out Func<Unpacker, SerializationContext, TObject> unpacking )
		{
			var entries =
				typeof( TObject ).FindMembers(
					MemberTypes.Field | MemberTypes.Property,
					BindingFlags.Public | BindingFlags.Instance,
					GetMemberFileter( option ), null
				).Select( member => new SerlializingMember( member ) )
				.OrderBy( member => member.Contract.Order )
				.ToArray();

			if ( entries.Length == 0 )
			{
				throw SerializationExceptions.NewNoSerializableFieldsException( typeof( TObject ) );
			}

			return this.CreateProcedures( entries, out packing, out unpacking );
		}

		protected abstract bool CreateProcedures( SerlializingMember[] entries, out Action<Packer, TObject, SerializationContext> packing, out Func<Unpacker, SerializationContext, TObject> unpacking );

		private static MemberFilter GetMemberFileter( SerializationMemberOption option )
		{
			switch ( option )
			{
				case SerializationMemberOption.OptIn:
				{
					return FilterOnlyDataMember;
				}
				case SerializationMemberOption.OptOut:
				{
					return FilterAllBuNotNonSerialized;
				}
				default:
				{
					throw new ArgumentOutOfRangeException( "option" );
				}
			}
		}

		private static bool FilterOnlyDataMember( MemberInfo member, object filterCriteria )
		{
			return Attribute.IsDefined( member, typeof( DataMemberAttribute ) );
		}

		private static bool FilterAllBuNotNonSerialized( MemberInfo member, object filterCriteria )
		{
			return !Attribute.IsDefined( member, typeof( NonSerializedAttribute ) );
		}

		protected bool CreateProcedures( FieldInfo field, DataMemberContract contract, out Action<Packer, TObject, SerializationContext> packing, out Action<Unpacker, TObject, SerializationContext> unpacking )
		{
			return this.CreateProceduresCore( field, field.FieldType, !field.IsInitOnly, contract, out packing, out unpacking );
		}

		protected bool CreateProcedures( PropertyInfo property, DataMemberContract contract, out Action<Packer, TObject, SerializationContext> packing, out Action<Unpacker, TObject, SerializationContext> unpacking )
		{
			return this.CreateProceduresCore( property, property.PropertyType, property.CanWrite, contract, out packing, out unpacking );
		}

		private bool CreateProceduresCore( MemberInfo member, Type memberType, bool canWrite, DataMemberContract contract, out Action<Packer, TObject, SerializationContext> packing, out Action<Unpacker, TObject, SerializationContext> unpacking )
		{
			switch ( Type.GetTypeCode( memberType ) )
			{
				case TypeCode.DBNull:
				case TypeCode.Empty:
				{
					Tracer.Emit.TraceEvent( Tracer.EventType.UnsupportedType, Tracer.EventId.UnsupportedType, "Field type '{0}' does not supported.", memberType );
					packing = null;
					unpacking = null;
					return false;
				}
			}

			if ( canWrite )
			{
				//packing = this.CreatePacking( member, memberType, contract );
				//unpacking = this.CreateUnpacking( member, memberType, contract );
				return this.CreateObjectProcedures( member, memberType, contract, out packing, out unpacking );
			}

			if ( memberType.IsValueType )
			{
				Tracer.Emit.TraceEvent( Tracer.EventType.ReadOnlyValueTypeMember, Tracer.EventId.ReadOnlyValueTypeMember, "Field {0} is read only and its type '{1}' is value type.", member.Name, member.ReflectedType );
				packing = null;
				unpacking = null;
				return false;
			}

			var collectionTrait = memberType.GetCollectionTraits();
			switch ( collectionTrait.CollectionType )
			{
				case CollectionKind.NotCollection:
				{
					return this.CreateObjectProcedures( member, memberType, contract, out packing, out unpacking );
				}
				case CollectionKind.Map:
				{
					return this.CreateMapProcedures( member, memberType, contract, collectionTrait, out packing, out unpacking );
				}
				case CollectionKind.Array:
				{
					return this.CreateArrayProcedures( member, memberType, contract, collectionTrait, out packing, out unpacking );
				}
				default:
				{
					// error
					packing = null;
					unpacking = null;
					return false;
				}
			}
		}

		[Obsolete]
		protected abstract Action<Packer, TObject, SerializationContext> CreatePacking( MemberInfo member, Type memberType, DataMemberContract contract );

		[Obsolete]
		protected abstract Action<Unpacker, TObject, SerializationContext> CreateUnpacking( MemberInfo member, Type memberType, DataMemberContract contract );

		protected abstract bool CreateArrayProcedures( MemberInfo member, Type memberType, DataMemberContract contract, CollectionTraits traits, out Action<Packer, TObject, SerializationContext> packing, out Action<Unpacker, TObject, SerializationContext> unpacking );

		protected abstract bool CreateMapProcedures( MemberInfo member, Type memberType, DataMemberContract contract, CollectionTraits traits, out Action<Packer, TObject, SerializationContext> packing, out Action<Unpacker, TObject, SerializationContext> unpacking );

		protected abstract bool CreateObjectProcedures( MemberInfo member, Type memberType, DataMemberContract contract, out Action<Packer, TObject, SerializationContext> packing, out Action<Unpacker, TObject, SerializationContext> unpacking );
	}
}