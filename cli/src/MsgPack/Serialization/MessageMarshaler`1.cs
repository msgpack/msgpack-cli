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
using System.Runtime.Serialization;
using System.Globalization;

namespace MsgPack.Serialization
{
	/// <summary>
	///		Implements message marshaling. Marshaling usually treat 'simple value' type like Value Types, <see cref="Uri"/>, <see cref="Version"/>, etc.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public abstract class MessageMarshaler<T>
	{
		private static readonly bool _isNullable =
			!typeof( T ).IsValueType
			|| ( typeof( T ).IsGenericType && typeof( T ).GetGenericTypeDefinition().TypeHandle.Equals( typeof( Nullable<> ).TypeHandle ) );

		public void MarshalTo( Packer packer, T value )
		{
			if ( packer == null )
			{
				throw new ArgumentNullException( "packer" );
			}

			if ( value == null )
			{
				packer.PackNull();
				return;
			}

			this.MarshalToCore( packer, value );
		}

		protected abstract void MarshalToCore( Packer packer, T value );

		public T UnmarshalFrom( Unpacker unpacker )
		{
			if ( unpacker == null )
			{
				throw new ArgumentNullException( "unpacker" );
			}

			if ( unpacker.IsInStart )
			{
				if ( !unpacker.Read() )
				{
					throw SerializationExceptions.NewUnexpectedEndOfStream();
				}
			}

			return this.UnmarshalFromCore( unpacker );
		}

		protected abstract T UnmarshalFromCore( Unpacker unpacker );
	}
}
