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
#if DEBUG && !UNITY
using System.Diagnostics.Contracts;
#endif // DEBUG && !UNITY
using System.Globalization;

namespace MsgPack.Serialization
{
	/// <summary>
	/// 	Represents event information for <see cref="SerializationContext.ResolveSerializer" /> event.
	/// </summary>
	/// <seealso cref="SerializationContext.ResolveSerializer"/>
	public sealed class ResolveSerializerEventArgs : EventArgs
	{
		/// <summary>
		///		Gets the <see cref="SerializationContext" /> which raises this event.
		/// </summary>
		/// <value>
		///		The <see cref="SerializationContext" /> which raises this event. This value will not be <c>null</c>.
		/// </value>
		/// <remarks>
		///		A <c>sender</c> parameter of the event handler has same instance for this.
		/// </remarks>
		public SerializationContext Context { get; private set; }

		/// <summary>
		///		Gets the target type which is getting serializer.
		/// </summary>
		/// <value>
		///		The target type which is getting serializer. This value will not be <c>null</c>.
		/// </value>
		public Type TargetType { get; private set; }

		/// <summary>
		///		Gets the <see cref="PolymorphismSchema"/> which represents polymorphism information for the current member.
		/// </summary>
		/// <value>
		///		The <see cref="PolymorphismSchema"/> which represents polymorphism information for the current member. This value will not be <c>null</c>.
		/// </value>
		public PolymorphismSchema PolymorphismSchema { get; private set; }

		private IMessagePackSerializer _foundSerializer;

		/// <summary>
		///		Gets the found serializer the event subscriber specified.
		/// </summary>
		/// <returns>
		///		The found serializer the event subscriber specified. <c>null</c> represents default behavior is wanted.
		/// </returns>
		internal MessagePackSerializer<T> GetFoundSerializer<T>()
		{
#if DEBUG && !UNITY
			Contract.Assert( typeof( T ) == this.TargetType, typeof( T ) + "==" + this.TargetType );
#endif // DEBUG && !UNITY
			return ( MessagePackSerializer<T> ) this._foundSerializer;
		}

		/// <summary>
		///		Sets the serializer instance which can handle <see cref="TargetType" /> type instance correctly.
		/// </summary>
		/// <param name="foundSerializer">The serializer instance which can handle <see cref="TargetType" /> type instance correctly; <c>null</c> when you cannot provide appropriate serializer instance.</param>
		/// <remarks>
		///		If you decide to delegate serializer generation to MessagePack for CLI infrastructure, do not call this method in your event handler or specify <c>null</c> for <paramref name="foundSerializer"/>.
		/// </remarks>
		public void SetSerializer<T>( MessagePackSerializer<T> foundSerializer )
		{
			if ( typeof( T ) != this.TargetType )
			{
				throw new InvalidOperationException(
					String.Format(
						CultureInfo.CurrentCulture,
						"The serializer must be {0} type.",
						typeof( MessagePackSerializer<> ).MakeGenericType( this.TargetType )
					) 
				);
			}

			this._foundSerializer = foundSerializer;
		}

		internal ResolveSerializerEventArgs( SerializationContext context, Type targetType, PolymorphismSchema schema )
		{
			this.Context = context;
			this.TargetType = targetType;
			this.PolymorphismSchema = schema ?? PolymorphismSchema.Default;
		}
	}
}
