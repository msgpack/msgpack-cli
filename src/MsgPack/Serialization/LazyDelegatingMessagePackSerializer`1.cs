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
using System.Globalization;

namespace MsgPack.Serialization
{
	/// <summary>
	///		Lazy initialized serializer which delegates actual work for the other serializer implementation.
	/// </summary>
	/// <typeparam name="T">
	///		The type of target type.
	/// </typeparam>
	/// <remarks>
	///		This serializer is intended to support self-composit structure like directories or XML nodes.
	/// </remarks>
	internal sealed class LazyDelegatingMessagePackSerializer<T> : MessagePackSerializer<T>
	{
		private readonly object _providerParameter;
		private MessagePackSerializer<T> _delegated;

		/// <summary>
		///		Initializes a new instance of the <see cref="LazyDelegatingMessagePackSerializer&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="ownerContext">
		///		The serialization context to support lazy retrieval.
		///	</param>
		/// <param name="providerParameter">A provider parameter to be passed in future.</param>
		public LazyDelegatingMessagePackSerializer( SerializationContext ownerContext, object providerParameter )
			: base( ownerContext )
		{
			this._providerParameter = providerParameter;
		}

		private MessagePackSerializer<T> GetDelegatedSerializer()
		{
			var result = this._delegated;
			if ( result == null )
			{
				result = this.OwnerContext.GetSerializer<T>( this._providerParameter );

				if ( result is LazyDelegatingMessagePackSerializer<T> )
				{
					throw new InvalidOperationException(
						String.Format(
							CultureInfo.CurrentCulture,
							"MessagePack serializer for the type '{0}' is not constructed yet.",
							typeof( T )
						)
					);
				}

				// Duplicated assignment is accepttable.
				this._delegated = result;
			}

			return result;
		}

		protected internal override void PackToCore( Packer packer, T objectTree )
		{
			this.GetDelegatedSerializer().PackToCore( packer, objectTree );
		}

		protected internal override T UnpackFromCore( Unpacker unpacker )
		{
			return this.GetDelegatedSerializer().UnpackFromCore( unpacker );
		}

		protected internal override void UnpackToCore( Unpacker unpacker, T collection )
		{
			this.GetDelegatedSerializer().UnpackToCore( unpacker, collection );
		}

		public override string ToString()
		{
			return this.GetDelegatedSerializer().ToString();
		}
	}
}
