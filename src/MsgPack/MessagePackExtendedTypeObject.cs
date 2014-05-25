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
using System.Text;

namespace MsgPack
{
	/// <summary>
	///		Represents Message Pack extended type object.
	/// </summary>
	public struct MessagePackExtendedTypeObject : IEquatable<MessagePackExtendedTypeObject>
	{
		/// <summary>
		///		A type code of this object.
		/// </summary>
		private readonly byte _typeCode;

		/// <summary>
		///		Gets a type code of this object.
		/// </summary>
		/// <value>
		///		A type code. Note that values over <see cref="SByte.MaxValue"/> are reserved for MsgPack spec itself.
		/// </value>
		public byte TypeCode
		{
			get { return this._typeCode; }
		}

		/// <summary>
		///		A binary value portion of this object.
		/// </summary>
		private readonly byte[] _body;

		/// <summary>
		///		Gets a binary value portion of this object.
		/// </summary>
		/// <value>
		///		A binary value portion of this object. This value will not be null.
		/// </value>
		internal byte[] Body
		{
			get { return this._body ?? Binary.Empty; }
		}

		/// <summary>
		///		Gets a copy of the binary value portion of this object.
		/// </summary>
		/// <value>
		///		A copy of the binary value portion of this object. This value will not be null.
		/// </value>
		public byte[] GetBody()
		{
			return this._body == null ? Binary.Empty : this._body.Clone() as byte[];
		}

		/// <summary>
		///		Gets a value indicating whether this instance is valid.
		/// </summary>
		/// <value>
		///   <c>true</c> if this instance is valid; otherwise, <c>false</c>.
		/// </value>
		public bool IsValid
		{
			get { return this._body != null; }
		}


		// For unpacking
		private MessagePackExtendedTypeObject( byte[] body, byte unpackedTypeCode )
		{
			if ( body == null )
			{
				throw new ArgumentNullException( "body" );
			}

			this._typeCode = unpackedTypeCode;
			this._body = body;
		}


		/// <summary>
		///		Initializes a new instance of the <see cref="MessagePackExtendedTypeObject"/> struct.
		/// </summary>
		/// <param name="typeCode">A type code of this extension object.</param>
		/// <param name="body">A binary value portion.</param>
		/// <exception cref="System.ArgumentException">
		///		The <paramref name="typeCode"/> is over 127. Higher values are reserved for MessagePack format specification.
		/// </exception>
		/// <exception cref="System.ArgumentNullException">The <paramref name="body"/> is <c>null</c>.</exception>
		public MessagePackExtendedTypeObject( byte typeCode, byte[] body )
		{
			if ( typeCode > 0x7F )
			{
				throw new ArgumentException(
					"A typeCode must be less than 128 because higher values are reserved for MessagePack format specification.",
					"typeCode" );
			}

			if ( body == null )
			{
				throw new ArgumentNullException( "body" );
			}

			this._typeCode = typeCode;
			this._body = body;
		}

		/// <summary>
		///		Creates a new instance of the <see cref="MessagePackExtendedTypeObject"/> struct.
		/// </summary>
		/// <param name="typeCode">A type code of this extension object.</param>
		/// <param name="body">A binary value portion.</param>
		/// <exception cref="System.ArgumentNullException">The <paramref name="body"/> is <c>null</c>.</exception>
		/// <remarks>
		///		This method allows reserved type code. It means that this method does not throw exception when the <paramref name="typeCode"/> is reserved value (greater then 0x7F).
		/// </remarks>
		public static MessagePackExtendedTypeObject Unpack( byte typeCode, byte[] body )
		{
			return new MessagePackExtendedTypeObject( body, typeCode );
		}

		/// <summary>
		///		Returns a <see cref="System.String" /> that represents this instance.
		/// </summary>
		/// <returns>
		///		A <see cref="System.String" /> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			if ( this._body == null )
			{
				return String.Empty;
			}

			var buffer = new StringBuilder( 7 + this._body.Length * 2 );
			this.ToString( buffer, false );
			return buffer.ToString();
		}

		internal void ToString( StringBuilder buffer, bool isJson )
		{
			if ( isJson )
			{
				if ( this._body == null )
				{
					// Assume that JSON is used when a collection is stringified, so empty string should break entire JSON.
					buffer.Append( "null" );
					return;
				}

				buffer.Append( "{\"TypeCode\":" ).Append( this._typeCode ).Append( ", \"Body\":\"" );
				Binary.ToHexString( this._body, buffer );
				buffer.Append( "\"}" );
			}
			else
			{
				if ( this._body == null )
				{
					return;
				}

				buffer.Append( "[" ).Append( this._typeCode ).Append( "]" );
				Binary.ToHexString( this._body, buffer );
			}

		}

		/// <summary>
		///		Returns a hash code for this instance.
		/// </summary>
		/// <returns>
		///		A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
		/// </returns>
		public override int GetHashCode()
		{
			if ( this._body == null )
			{
				return 0;
			}

			unchecked
			{
				int hashCode = this._typeCode << 24;
				hashCode ^= this._body.Length;
				int hashCodeTargetLength = Math.Min( this._body.Length / 4, 8 );
				for ( int i = 0; i < hashCodeTargetLength; i++ )
				{
					uint temp = this._body[ i ];
					temp |= ( uint )( this._body[ i + 1 ] << 8 );
					temp |= ( uint )( this._body[ i + 2 ] << 16 );
					temp |= ( uint )( this._body[ i + 3 ] << 24 );

					hashCode ^= ( int )temp;
				}

				return hashCode;
			}
		}

		/// <summary>
		///		Determines whether the specified <see cref="System.Object" /> is equal to this instance.
		/// </summary>
		/// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
		/// <returns>
		///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
		/// </returns>
		public override bool Equals( object obj )
		{
			if ( !( obj is MessagePackExtendedTypeObject ) )
			{
				return false;
			}

			return this.Equals( ( MessagePackExtendedTypeObject )obj );
		}

		/// <summary>
		///		Determines whether the specified <see cref="MessagePackExtendedTypeObject" /> is equal to this instance.
		/// </summary>
		/// <param name="other">The <see cref="MessagePackExtendedTypeObject" /> to compare with this instance.</param>
		/// <returns>
		///   <c>true</c> if the specified <see cref="MessagePackExtendedTypeObject" /> is equal to this instance; otherwise, <c>false</c>.
		/// </returns>
		public bool Equals( MessagePackExtendedTypeObject other )
		{
			if ( this._typeCode != other._typeCode )
			{
				return false;
			}

			if ( ReferenceEquals( this._body, other._body ) )
			{
				return true;
			}

			if ( this._body.Length != other._body.Length )
			{
				return false;
			}

			for ( int i = 0; i < this._body.Length; i++ )
			{
				if ( this._body[ i ] != other._body[ i ] )
				{
					return false;
				}
			}

			return true;
		}

		/// <summary>
		///		Determines whether the specified <see cref="MessagePackExtendedTypeObject" />s are equal.
		/// </summary>
		/// <param name="left">A <see cref="MessagePackExtendedTypeObject" />.</param>
		/// <param name="right">A <see cref="MessagePackExtendedTypeObject" />.</param>
		/// <returns>
		///   <c>true</c> if the specified <see cref="MessagePackExtendedTypeObject" />s are equal; otherwise, <c>false</c>.
		/// </returns>
		public static bool operator ==( MessagePackExtendedTypeObject left, MessagePackExtendedTypeObject right )
		{
			return left.Equals( right );
		}

		/// <summary>
		///		Determines whether the specified <see cref="MessagePackExtendedTypeObject" />s are not equal.
		/// </summary>
		/// <param name="left">A <see cref="MessagePackExtendedTypeObject" />.</param>
		/// <param name="right">A <see cref="MessagePackExtendedTypeObject" />.</param>
		/// <returns>
		///   <c>true</c> if the specified <see cref="MessagePackExtendedTypeObject" />s are not equal; otherwise, <c>false</c>.
		/// </returns>
		public static bool operator !=( MessagePackExtendedTypeObject left, MessagePackExtendedTypeObject right )
		{
			return !left.Equals( right );
		}
	}
}
