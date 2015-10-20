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

#if !NETFX_40 && !NETFX_35 && !UNITY && !SILVERLIGHT
#define NETFX_45
#endif // !NETFX_40 && !NETFX_35 && !UNITY && !SILVERLIGHT
using System;
using System.Collections.Generic;
#if !UNITY
using System.Diagnostics.Contracts;
#endif // !UNITY
using System.Globalization;
using System.Linq;
#if NETFX_45
using System.Threading;
#endif // NETFX_45

namespace MsgPack.Serialization
{
	/// <summary>
	///		Implements mapping table between known ext type codes and names.
	/// </summary>
	/// <remarks>
	///		Well-known (pre-defined) ext type names are defined in <see cref="KnownExtTypeName"/>, and their default mapped codes are found in <see cref="KnownExtTypeCode"/>.
	/// </remarks>
	/// <threadsafety instance="true" static="true" />
	public sealed class ExtTypeCodeMapping : IEnumerable<KeyValuePair<string, byte>>
	{
		private readonly object _syncRoot;
		private readonly Dictionary<string, byte> _index;
		private readonly Dictionary<byte, string> _types;

		/// <summary>
		///		Gets a mapped byte to the specified ext type name.
		/// </summary>
		/// <param name="name">The name of the ext type.</param>
		/// <returns>
		///		The byte code for specified ext type in the current context.
		/// </returns>
		/// <exception cref="ArgumentNullException"><paramref name="name"/> is <c>null</c>.</exception>
		/// <exception cref="ArgumentException"><paramref name="name"/> is empty.</exception>
		/// <exception cref="KeyNotFoundException"><paramref name="name"/> is not registered as known ext type name.</exception>
		public byte this[ string name ]
		{
			get
			{
				ValidateName( name );

				lock ( this._syncRoot )
				{
					byte code;
					if ( !this._index.TryGetValue( name, out code ) )
					{
						throw new KeyNotFoundException(
							String.Format( CultureInfo.CurrentCulture, "Ext type '{0}' is not found.", name )
						);
					}

					return code;
				}
			}
		}

		internal ExtTypeCodeMapping()
		{
			this._syncRoot = new object();
			this._index = new Dictionary<string, byte>( 2 );
			this._types = new Dictionary<byte, string>( 2 );
			this.Add( KnownExtTypeName.MultidimensionalArray, KnownExtTypeCode.MultidimensionalArray );
		}

		/// <summary>
		///		Adds the known ext type mapping.
		/// </summary>
		/// <param name="name">The name of the ext type.</param>
		/// <param name="typeCode">The ext type code to be mapped.</param>
		/// <returns>
		///		<c>true</c> if <paramref name="name"/> AND <paramref name="typeCode"/> were not registered and then newly registered; <c>false</c>, otherwise.
		/// </returns>
		/// <exception cref="ArgumentNullException"><paramref name="name"/> is <c>null</c>.</exception>
		/// <exception cref="ArgumentException"><paramref name="name"/> is empty.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="typeCode"/> is greater than 0x7F.</exception>
		public bool Add( string name, byte typeCode )
		{
			ValidateName( name );
			ValidateTypeCode( typeCode );

			lock ( this._syncRoot )
			{
				try
				{
					this._types.Add( typeCode, name );
				}
				catch ( ArgumentException )
				{
					return false;
				}

				this._index[ name ] = typeCode;
				return true;
			}
		}

		/// <summary>
		///		Removes the mapping with specified name.
		/// </summary>
		/// <param name="name">The name of the ext type.</param>
		/// <returns>
		///		<c>true</c> if <paramref name="name"/> was registered and has been removed successfully; <c>false</c>, otherwise.
		/// </returns>
		/// <exception cref="ArgumentNullException"><paramref name="name"/> is <c>null</c>.</exception>
		/// <exception cref="ArgumentException"><paramref name="name"/> is empty.</exception>
		public bool Remove( string name )
		{
			ValidateName( name );

			lock ( this._syncRoot )
			{
				byte typeCode;
				if ( !this._index.TryGetValue( name, out typeCode ) )
				{
					return false;
				}

				this.RemoveCore( name, typeCode );
				return true;
			}
		}

		/// <summary>
		///		Removes the mapping with specified code.
		/// </summary>
		/// <param name="typeCode">The type code of the ext type.</param>
		/// <returns>
		///		<c>true</c> if <paramref name="typeCode"/> was registered and has been removed successfully; <c>false</c>, otherwise.
		/// </returns>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="typeCode"/> is greater than 0x7F.</exception>
		public bool Remove( byte typeCode )
		{
			ValidateTypeCode( typeCode );

			lock ( this._syncRoot )
			{
				string name;
				if ( !this._types.TryGetValue( typeCode, out name ) )
				{
					return false;
				}

				this.RemoveCore( name, typeCode );
				return true;
			}
		}

		private void RemoveCore( string name, byte typeCode )
		{
#if DEBUG && NETFX_45
			Contract.Assert( Monitor.IsEntered( this._syncRoot ) );
#endif // DEBUG && NETFX_45
			var shouldBeTrue = this._types.Remove( typeCode );
#if DEBUG && !UNITY
			Contract.Assert( shouldBeTrue );
#endif // DEBUG && !UNITY
			shouldBeTrue = this._index.Remove( name );
#if DEBUG && !UNITY
			Contract.Assert( shouldBeTrue );
#endif // DEBUG && !UNITY
		}

		/// <summary>
		///		Clears all mappings.
		/// </summary>
		public void Clear()
		{
			lock ( this._syncRoot )
			{
				this._types.Clear();
				this._index.Clear();
			}
		}

		/// <summary>
		///		Returns an enumerator that iterates through the collection.
		/// </summary>
		/// <returns>
		///		A <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.
		/// </returns>
		/// <remarks>
		///		This method causes internal collection copying, so this makes O(n) time.
		/// </remarks>
		public IEnumerator<KeyValuePair<string, byte>> GetEnumerator()
		{
			List<KeyValuePair<string, byte>> list;
			lock ( this._syncRoot )
			{
				list = this._index.ToList();
			}

			return list.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		private static void ValidateName( string name )
		{
			if ( name == null )
			{
				throw new ArgumentNullException( "name" );
			}

			if ( name.Length ==0 )
			{
				throw new ArgumentException( String.Format( CultureInfo.CurrentCulture, "A parameter cannot be empty." ), "name" );
			}
		}

		private static void ValidateTypeCode( byte typeCode )
		{
			if ( typeCode > 0x7F )
			{
				throw new ArgumentOutOfRangeException( "typeCode", "Ext type code must be between 0 and 0x7F." );
			}
		}
	}
}