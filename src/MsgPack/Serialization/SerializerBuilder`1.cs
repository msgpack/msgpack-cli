#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2012 FUJIWARA, Yusuke
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
	[ContractClass(typeof(SerializerBuilderContract<>))]
	internal abstract class SerializerBuilder<TObject>
	{
		private readonly SerializationContext _context;

		/// <summary>
		///		Gets the <see cref="SerializationContext"/>.
		/// </summary>
		/// <value>
		///		The <see cref="SerializationContext"/>.
		/// </value>
		public SerializationContext Context
		{
			get
			{
				Contract.Ensures( Contract.Result<SerializationContext>() != null );

				return this._context;
			}
		}

		/// <summary>
		///		Initializes a new instance of the <see cref="SerializerBuilder&lt;TObject&gt;"/> class.
		/// </summary>
		/// <param name="context">The <see cref="SerializationContext"/>.</param>
		protected SerializerBuilder( SerializationContext context )
		{
			Contract.Requires( context != null );

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
			Contract.Ensures( Contract.Result<MessagePackSerializer<TObject>>() != null );

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

				Contract.Assert( entries[ 0 ].Contract.Id >= 0 );

				if ( this.Context.CompatibilityOptions.OneBoundDataMemberOrder && entries[ 0 ].Contract.Id == 0 )
				{
					throw new NotSupportedException( "Cannot specify order value 0 on DataMemberAttribute when SerializationContext.CompatibilityOptions.OneBoundDataMemberOrder is set to true." );
				} 
				
				var maxId = entries.Max( item => item.Contract.Id );
				var result = new List<SerializingMember>( maxId + 1 );
				for ( int source = 0, destination = this.Context.CompatibilityOptions.OneBoundDataMemberOrder ? 1 : 0; source < entries.Length; source++, destination++ )
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

		private static IEnumerable<SerializingMember> GetTargetMembers()
		{
#if !NETFX_CORE
			var members =
				typeof( TObject ).FindMembers(
					MemberTypes.Field | MemberTypes.Property,
					BindingFlags.Public | BindingFlags.Instance,
					( member, criteria ) => true,
					null
				);
			var filtered = members.Where( item => Attribute.IsDefined( item, typeof( MessagePackMemberAttribute ) ) ).ToArray();
#else
			var members =
				typeof( TObject ).GetRuntimeFields().Where( f => f.IsPublic && !f.IsStatic ).OfType<MemberInfo>().Concat( typeof( TObject ).GetRuntimeProperties().Where( p => p.GetMethod != null && p.GetMethod.IsPublic && !p.GetMethod.IsStatic ) );
			var filtered = members.Where( item => item.IsDefined( typeof( MessagePackMemberAttribute ) ) ).ToArray();
#endif

			if ( filtered.Length > 0 )
			{
				return
					filtered.Select( member =>
						new SerializingMember(
							member,
							new DataMemberContract( member, member.GetCustomAttribute<MessagePackMemberAttribute>() )
						)
					);
			}

			if ( typeof( TObject ).IsDefined( typeof( DataContractAttribute ) ) )
			{
				return
					members.Where( item => item.IsDefined( typeof( DataMemberAttribute ) ) )
					.Select( member =>
						new SerializingMember(
							member,
							new DataMemberContract( member, member.GetCustomAttribute<DataMemberAttribute>() )
						)
					);
			}

#if SILVERLIGHT || NETFX_CORE
			return members.Select( member => new SerializingMember( member, new DataMemberContract( member ) ) );
#else
			return
				members.Where( item => !Attribute.IsDefined( item, typeof( NonSerializedAttribute ) ) )
				.Select( member => new SerializingMember( member, new DataMemberContract( member ) ) );
#endif
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

	[ContractClassFor(typeof(SerializerBuilder<>))]
	internal abstract class SerializerBuilderContract<T> : SerializerBuilder<T>
	{
		protected SerializerBuilderContract() : base( null ) { }

		protected override MessagePackSerializer<T> CreateSerializer( SerializingMember[] entries )
		{
			Contract.Requires( entries != null );
			Contract.Ensures( Contract.Result<MessagePackSerializer<T>>() != null );
			return null;
		}

		public override MessagePackSerializer<T> CreateArraySerializer()
		{
			Contract.Ensures( Contract.Result<MessagePackSerializer<T>>() != null );
			return null;
		}

		public override MessagePackSerializer<T> CreateMapSerializer()
		{
			Contract.Ensures( Contract.Result<MessagePackSerializer<T>>() != null );
			return null;
		}

		public override MessagePackSerializer<T> CreateTupleSerializer()
		{
			Contract.Ensures( Contract.Result<MessagePackSerializer<T>>() != null );
			return null;
		}
	}
}