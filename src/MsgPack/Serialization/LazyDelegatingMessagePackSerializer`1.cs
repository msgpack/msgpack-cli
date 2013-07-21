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
		private readonly SerializationContext _context;
		private MessagePackSerializer<T> _delegated;

		/// <summary>
		///		Initializes a new instance of the <see cref="LazyDelegatingMessagePackSerializer&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="context">
		///		The serialization context to support lazy retrieval.
		///	</param>
		public LazyDelegatingMessagePackSerializer( SerializationContext context )
			: base( ( context ?? SerializationContext.Default ).CompatibilityOptions.PackerCompatibilityOptions )
		{
			this._context = context;
		}

		private MessagePackSerializer<T> GetDelegatedSerializer()
		{
			var result = this._delegated;
			if ( result == null )
			{
				result = this._context.GetSerializer<T>();
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

		protected internal sealed override void PackToCore( Packer packer, T objectTree )
		{
			this.GetDelegatedSerializer().PackToCore( packer, objectTree );
		}

		protected internal sealed override T UnpackFromCore( Unpacker unpacker )
		{
			return this.GetDelegatedSerializer().UnpackFromCore( unpacker );
		}

		protected internal sealed override void UnpackToCore( Unpacker unpacker, T collection )
		{
			this.GetDelegatedSerializer().UnpackToCore( unpacker, collection );
		}

		public override string ToString()
		{
			return this.GetDelegatedSerializer().ToString();
		}
	}
}
