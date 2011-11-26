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
using System.Reflection;
using System.ComponentModel;
using System.Collections;

namespace MsgPack.Serialization
{
	/// <summary>
	///		<strong>This is intened to MsgPack for CLI internal use. Do not use this type from application directly.</strong>
	///		Represents serialization context information for internal serialization logic.
	/// </summary>
	public sealed class SerializationContext
	{
		[Obsolete]
		internal static readonly MethodInfo MarshalTo1Method = typeof( SerializationContext ).GetMethod( "MarshalTo" );
		[Obsolete]
		internal static readonly MethodInfo UnmarshalFrom1Method = typeof( SerializationContext ).GetMethod( "UnmarshalFrom" );
		[Obsolete]
		internal static readonly MethodInfo MarshalArrayTo1Method = typeof( SerializationContext ).GetMethod( "MarshalArrayTo" );
		[Obsolete]
		internal static readonly MethodInfo UnmarshalArrayTo1Method = typeof( SerializationContext ).GetMethod( "UnmarshalArrayTo" );

		private static readonly SerializationContext _default = new SerializationContext( SerializerRepository.Default );

		private readonly SerializerRepository _serializers;

		/// <summary>
		///		Gets the current <see cref="SerializerRepository"/>.
		/// </summary>
		/// <value>
		///		The  current <see cref="SerializerRepository"/>.
		/// </value>
		public SerializerRepository Serializers
		{
			get { return this._serializers; }
		}

		/// <summary>
		///		Initializes a new instance of the <see cref="SerializationContext"/> class with copy of <see cref="SerializerRepository.Default"/>.
		/// </summary>
		public SerializationContext()
			: this( new SerializerRepository( SerializerRepository.Default ) )
		{

		}

		internal SerializationContext( SerializerRepository serializers )
		{
			this._serializers = serializers;
		}

		/// <summary>
		///		Gets the <see cref="MessagePackSerializer{T}"/> with this instance.
		/// </summary>
		/// <typeparam name="T">Type of serialization/deserialization target.</typeparam>
		/// <returns>
		///		<see cref="MessagePackSerializer{T}"/>.
		///		If there is exiting one, returns it.
		///		Else the new instance will be created.
		/// </returns>
		/// <remarks>
		///		This method automatically register new instance via <see cref="M:SerializationRepository.Register{T}"/>.
		/// </remarks>
		public MessagePackSerializer<T> GetSerializer<T>()
		{
			var serializer = this._serializers.Get<T>( this );
			if ( serializer == null )
			{
				// TODO: Configurable
				serializer = new AutoMessagePackSerializer<T>( this );
				if ( !this._serializers.Register<T>( serializer ) )
				{
					serializer = this._serializers.Get<T>( this );
				}
			}

			return serializer;
		}
	}
}
