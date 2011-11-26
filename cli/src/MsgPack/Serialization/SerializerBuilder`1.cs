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
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace MsgPack.Serialization
{
	/// <summary>
	///		Build serializer for <typeparamref name="TObject"/>.
	/// </summary>
	/// <typeparam name="TObject">Object to be serialized/deserialized.</typeparam>
	internal abstract class SerializerBuilder<TObject>
	{
		// TODO: boolean base -> Exception base

		private readonly SerializationContext _context;

		public SerializationContext Context
		{
			get { return this._context; }
		}

		protected SerializerBuilder() : this( new SerializationContext() ) { }

		protected SerializerBuilder( SerializationContext context )
		{
			this._context = context;
		}

		/// <summary>
		///		Creates serializer for <typeparamref name="TObject"/>.
		/// </summary>
		/// <param name="option">Option to control members binding.</param>
		/// <returns>
		///		<see cref="MessagePackSerializer{T}"/>. This value will not be <c>null</c>.
		/// </returns>
		public MessagePackSerializer<TObject> CreateSerializer( SerializationMemberOption option )
		{
			var entries =
				typeof( TObject ).FindMembers(
					MemberTypes.Field | MemberTypes.Property,
					BindingFlags.Public | BindingFlags.Instance,
					GetMemberFilter( option ), null
				).Select( member => new SerlializingMember( member ) )
				.OrderBy( member => member.Contract.Order )
				.ToArray();

			if ( entries.Length == 0 )
			{
				throw SerializationExceptions.NewNoSerializableFieldsException( typeof( TObject ) );
			}

			return this.CreateSerializer( entries );
		}

		/// <summary>
		///		Creates serializer for <typeparamref name="TObject"/>.
		/// </summary>
		/// <param name="entries">Serialization target members. This will not be <c>null</c> nor empty.</param>
		/// <returns>
		///		<see cref="MessagePackSerializer{T}"/>. This value will not be <c>null</c>.
		/// </returns>
		protected abstract MessagePackSerializer<TObject> CreateSerializer( SerlializingMember[] entries );

		private static MemberFilter GetMemberFilter( SerializationMemberOption option )
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
		
		/// <summary>
		///		Creates serializer to serialize/deserialize specified member and returns its
		/// </summary>
		/// <param name="member">Metadata of target member.</param>
		/// <param name="contract">Contract information of target member.</param>
		/// <returns>
		///		<see cref="ConstructorInfo"/> to instanciate <see cref="MessagePackSerializer{T}"/>. 
		///		This value will not be <c>null</c>.
		///		The signature is <c>T(<see cref="SerializationContext"/>)</c>.
		/// </returns>
		protected ConstructorInfo CreateSerializer( MemberInfo member, DataMemberContract contract )
		{
			PropertyInfo property;
			FieldInfo field;
			if ( ( property = member as PropertyInfo ) != null )
			{
				return this.CreateSerializerCore( property, property.PropertyType, property.CanWrite, contract );
			}
			else
			{
				field = member as FieldInfo;
				Contract.Assert( field != null );
				return this.CreateSerializerCore( field, field.FieldType, !field.IsInitOnly, contract );
			}
		}

		private ConstructorInfo CreateSerializerCore( MemberInfo member, Type memberType, bool canWrite, DataMemberContract contract )
		{
			switch ( Type.GetTypeCode( memberType ) )
			{
				case TypeCode.DBNull:
				case TypeCode.Empty:
				{
					Tracer.Emit.TraceEvent( Tracer.EventType.UnsupportedType, Tracer.EventId.UnsupportedType, "Field type '{0}' does not supported.", memberType );
					return null;
				}
			}

			if ( canWrite )
			{
				return this.CreateObjectSerializer( member, memberType );
			}

			if ( memberType.IsValueType )
			{
				Tracer.Emit.TraceEvent( Tracer.EventType.ReadOnlyValueTypeMember, Tracer.EventId.ReadOnlyValueTypeMember, "Field {0} is read only and its type '{1}' is value type.", member.Name, member.ReflectedType );
				return null;
			}

			var collectionTrait = memberType.GetCollectionTraits();
			switch ( collectionTrait.CollectionType )
			{
				case CollectionKind.NotCollection:
				{
					return this.CreateObjectSerializer( member, memberType );
				}
				case CollectionKind.Map:
				{
					return this.CreateMapSerializer( member, memberType );
				}
				case CollectionKind.Array:
				{
					return this.CreateArraySerializer( member, memberType );
				}
				default:
				{
					// error
					return null;
				}
			}
		}
		
		/// <summary>
		///		Creates serializer to serialize/deserialize specified array type member and returns its
		/// </summary>
		/// <param name="member">Metadata of target member.</param>
		/// <param name="memberType"><see cref="Type"/> of member value.</param>
		/// <returns>
		///		<see cref="ConstructorInfo"/> to instanciate <see cref="MessagePackSerializer{T}"/>. 
		///		This value will not be <c>null</c>.
		///		The signature is <c>T(<see cref="SerializationContext"/>)</c>.
		/// </returns>
		protected abstract ConstructorInfo CreateArraySerializer( MemberInfo member, Type memberType );

		/// <summary>
		///		Creates serializer to serialize/deserialize specified map type member and returns its
		/// </summary>
		/// <param name="member">Metadata of target member.</param>
		/// <param name="memberType"><see cref="Type"/> of member value.</param>
		/// <returns>
		///		<see cref="ConstructorInfo"/> to instanciate <see cref="MessagePackSerializer{T}"/>. 
		///		This value will not be <c>null</c>.
		///		The signature is <c>T(<see cref="SerializationContext"/>)</c>.
		/// </returns>
		protected abstract ConstructorInfo CreateMapSerializer( MemberInfo member, Type memberType );


		/// <summary>
		///		Creates serializer to serialize/deserialize specified object type member and returns its
		/// </summary>
		/// <param name="member">Metadata of target member.</param>
		/// <param name="memberType"><see cref="Type"/> of member value.</param>
		/// <returns>
		///		<see cref="ConstructorInfo"/> to instanciate <see cref="MessagePackSerializer{T}"/>. 
		///		This value will not be <c>null</c>.
		///		The signature is <c>T(<see cref="SerializationContext"/>)</c>.
		/// </returns>
		protected abstract ConstructorInfo CreateObjectSerializer( MemberInfo member, Type memberType );

		/// <summary>
		///		Creates serializer as <typeparamref name="TObject"/> is array type.
		/// </summary>
		/// <returns>
		///		<see cref="MessagePackSerializer{T}"/>. 
		///		This value will not be <c>null</c>.
		/// </returns>
		public abstract MessagePackSerializer<TObject> CreateArraySerializer();

		/// <summary>
		///		Creates serializer as <typeparamref name="TObject"/> is map type.
		/// </summary>
		/// <returns>
		///		<see cref="MessagePackSerializer{T}"/>. 
		///		This value will not be <c>null</c>.
		/// </returns>
		public abstract MessagePackSerializer<TObject> CreateMapSerializer();
	}
}