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
			var entries =
				typeof( TObject ).FindMembers(
					MemberTypes.Field | MemberTypes.Property,
					BindingFlags.Public | BindingFlags.Instance,
					GetMemberFilter( option ), null
				).Select( member => new SerializingMember( member ) )
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
		protected abstract MessagePackSerializer<TObject> CreateSerializer( SerializingMember[] entries );

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
					return FilterAllButNotNonSerialized;
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

		private static bool FilterAllButNotNonSerialized( MemberInfo member, object filterCriteria )
		{
#if SILVERLIGHT
			return true;
#else
			return !Attribute.IsDefined( member, typeof( NonSerializedAttribute ) );
#endif
		}

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