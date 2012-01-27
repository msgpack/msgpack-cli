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
using System.Diagnostics.Contracts;
using System.Globalization;
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
		private readonly SerializationContext _context;

		public SerializationContext Context
		{
			get { return this._context; }
		}

		protected SerializerBuilder( SerializationContext context )
		{
			this._context = context;
		}

		/// <summary>
		///		Creates serializer for <typeparamref name="TObject"/>.
		/// </summary>
		/// <returns>
		///		<see cref="MessagePackSerializer{T}"/>. This value will not be <c>null</c>.
		/// </returns>
		public MessagePackSerializer<TObject> CreateSerializer()
		{
			var entries = GetTargetMembers().OrderBy( item => item.Contract.Id ).ToArray();

			if ( entries.Length == 0 )
			{
				throw SerializationExceptions.NewNoSerializableFieldsException( typeof( TObject ) );
			}

			if ( entries.All( item => item.Contract.Id == DataMemberContract.UnspecifiedId ) )
			{
				// Alphabetical order.
				return this.CreateSerializer( entries.OrderBy( item => item.Contract.Name ).ToArray() );
			}
			else
			{
				// ID order.
				var maxId = entries.Max( item => item.Contract.Id );
				var result = new List<SerializingMember>( maxId + 1 );
				for ( int source = 0, destination = 0; source < entries.Length; source++, destination++ )
				{
					Contract.Assert( entries[ source ].Contract.Id >= 0 );

					if ( entries[ source ].Contract.Id < destination )
					{
						throw new SerializationException( String.Format( CultureInfo.CurrentCulture, "The member ID '{0}' is duplicated in the '{1}' type.", entries[ source ].Contract.Id, typeof( TObject ) ) );
					}

					while ( entries[ source ].Contract.Id > destination )
					{
						result.Add( new SerializingMember() );
						destination++;
					}

					result.Add( entries[ source ] );
				}

				return this.CreateSerializer( result.ToArray() );
			}
		}

		private IEnumerable<SerializingMember> GetTargetMembers()
		{
			var members =
				typeof( TObject ).FindMembers(
					MemberTypes.Field | MemberTypes.Property,
					BindingFlags.Public | BindingFlags.Instance,
					( member, criteria ) => true,
					null
				);

			var filtered = members.Where( item => Attribute.IsDefined( item, typeof( MessagePackMemberAttribute ) ) ).ToArray();
			if ( filtered.Length > 0 )
			{
				return
					filtered.Select( member =>
						new SerializingMember(
							member,
							new DataMemberContract( member, Attribute.GetCustomAttribute( member, typeof( MessagePackMemberAttribute ) ) as MessagePackMemberAttribute )
						)
					);
			}

			if ( Attribute.IsDefined( typeof( TObject ), typeof( DataContractAttribute ) ) )
			{
				return
					members.Where( item => Attribute.IsDefined( item, typeof( DataMemberAttribute ) ) )
					.Select( member =>
						new SerializingMember(
							member,
							new DataMemberContract( member, Attribute.GetCustomAttribute( member, typeof( DataMemberAttribute ) ) as DataMemberAttribute )
						)
					);
			}

			return
				members.Where( item => !Attribute.IsDefined( item, typeof( NonSerializedAttribute ) ) )
				.Select( member => new SerializingMember( member, new DataMemberContract( member ) ) );
		}

		/// <summary>
		///		Creates serializer for <typeparamref name="TObject"/>.
		/// </summary>
		/// <param name="entries">Serialization target members. This will not be <c>null</c> nor empty.</param>
		/// <returns>
		///		<see cref="MessagePackSerializer{T}"/>. This value will not be <c>null</c>.
		/// </returns>
		protected abstract MessagePackSerializer<TObject> CreateSerializer( SerializingMember[] entries );


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

		/// <summary>
		///		Creates serializer as <typeparamref name="TObject"/> is tuple type.
		/// </summary>
		/// <returns>
		///		<see cref="MessagePackSerializer{T}"/>. 
		///		This value will not be <c>null</c>.
		/// </returns>
		public abstract MessagePackSerializer<TObject> CreateTupleSerializer();
	}
}