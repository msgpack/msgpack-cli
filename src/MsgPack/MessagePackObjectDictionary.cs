#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2015 FUJIWARA, Yusuke
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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
#if !UNITY
#if XAMIOS || XAMDROID
using Contract = MsgPack.MPContract;
#else
using System.Diagnostics.Contracts;
#endif // XAMIOS || XAMDROID
#endif // !UNITY
using System.Globalization;
using System.Linq;

namespace MsgPack
{
	/// <summary>
	///		Implements <see cref="IDictionary{TKey,TValue}"/> for <see cref="MessagePackObject"/>.
	/// </summary>
	/// <remarks>
	///		This dictionary handles <see cref="MessagePackObject"/> type semantics for the key.
	///		Additionally, this dictionary implements 'freezing' feature. 
	///		For details, see <see cref="IsFrozen"/>, <see cref="Freeze"/>, and <see cref="AsFrozen"/>.
	/// </remarks>
#if !SILVERLIGHT && !NETFX_CORE
	[Serializable]
#endif
	[DebuggerTypeProxy( typeof( DictionaryDebuggerProxy<,> ) )]
	public partial class MessagePackObjectDictionary :
		IDictionary<MessagePackObject, MessagePackObject>, IDictionary
	{
		private const int Threashold = 10;
		private const int ListInitialCapacity = Threashold;
		private const int DictionaryInitialCapacity = Threashold * 2;

		private List<MessagePackObject> _keys;
		private List<MessagePackObject> _values;
		private Dictionary<MessagePackObject, MessagePackObject> _dictionary;
		private int _version;

		private bool _isFrozen;

		/// <summary>
		///		Gets a value indicating whether this instance is frozen.
		/// </summary>
		/// <value>
		///		<c>true</c> if this instance is frozen; otherwise, <c>false</c>.
		/// </value>
		/// <remarks>
		///		This operation is an O(1) operation.
		/// </remarks>
		public bool IsFrozen
		{
			get { return this._isFrozen; }
		}

		/// <summary>
		///		Gets the number of elements contained in the <see cref="MessagePackObjectDictionary"/>.
		/// </summary>
		/// <returns>
		///		The number of elements contained in the <see cref="MessagePackObjectDictionary"/>.
		/// </returns>
		/// <remarks>
		///		This operation is an O(1) operation.
		/// </remarks>
		public int Count
		{
			get
			{
#if !UNITY
				this.AssertInvariant();
#endif // !UNITY
				return this._dictionary == null ? this._keys.Count : this._dictionary.Count;
			}
		}

		/// <summary>
		///		Gets or sets the element with the specified key.
		/// </summary>
		/// <value>
		///		The element with the specified key.
		/// </value>
		/// <param name="key">Key for geting or seting value.</param>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="key"/> is <see cref="MessagePackObject.Nil"/>.
		/// </exception>
		/// <exception cref="T:System.Collections.Generic.KeyNotFoundException">
		///		The property is retrieved and <paramref name="key"/> is not found.
		/// </exception>
		/// <exception cref="InvalidOperationException">
		///		The property is set and this instance is frozen.
		/// </exception>
		///	<remarks>
		///		<para>
		///			Note that tiny integers are considered equal regardless of its CLI <see cref="Type"/>,
		///			and UTF-8 encoded bytes are considered equals to <see cref="String"/>.
		///		</para>
		///		<para>
		///			This method approaches an O(1) operation.
		///		</para>
		///	</remarks>
		public MessagePackObject this[ MessagePackObject key ]
		{
			get
			{
				if ( key.IsNil )
				{
					ThrowKeyNotNilException( "key" );
				}

#if !UNITY
				Contract.EndContractBlock();
#endif // !UNITY


				MessagePackObject result;
				if ( !this.TryGetValue( key, out result ) )
				{
					throw new KeyNotFoundException( String.Format( CultureInfo.CurrentCulture, "Key '{0}'({1} type) does not exist in this dictionary.", key, key.UnderlyingType ) );
				}

				return result;
			}
			set
			{
				if ( key.IsNil )
				{
					ThrowKeyNotNilException( "key" );
				}

				this.VerifyIsNotFrozen();

#if !UNITY
				Contract.EndContractBlock();

				this.AssertInvariant();
#endif // !UNITY
				this.AddCore( key, value, true );
			}
		}

#if !UNITY
		/// <summary>
		///		Gets an <see cref="KeySet"/> containing the keys of the <see cref="MessagePackObjectDictionary"/>.
		/// </summary>
		/// <returns>
		///		An <see cref="KeySet"/> containing the keys of the object.
		///		This value will not be <c>null</c>.
		/// </returns>
		/// <remarks>
		///		This operation is an O(1) operation.
		/// </remarks>
		public KeySet Keys
		{
			get
			{
				this.AssertInvariant();
				return new KeySet( this );
			}
		}
#else
		/// <summary>
		///		Gets an <see cref="KeyCollection"/> containing the keys of the <see cref="MessagePackObjectDictionary"/>.
		/// </summary>
		/// <returns>
		///		An <see cref="KeyCollection"/> containing the keys of the object.
		///		This value will not be <c>null</c>.
		/// </returns>
		/// <remarks>
		///		This operation is an O(1) operation.
		/// </remarks>
		public KeyCollection Keys
		{
			get
			{
#if !UNITY
				this.AssertInvariant();
#endif // !UNITY
				return new KeyCollection( this );
			}
		}
#endif // !UNITY

		/// <summary>
		///		Gets an <see cref="ValueCollection"/> containing the values of the <see cref="MessagePackObjectDictionary"/>.
		/// </summary>
		/// <returns>
		///		An <see cref="ValueCollection"/> containing the values of the object.
		///		This value will not be <c>null</c>.
		/// </returns>
		/// <remarks>
		///		This operation is an O(1) operation.
		/// </remarks>
		public ValueCollection Values
		{
			get
			{
#if !UNITY
				this.AssertInvariant();
#endif // !UNITY
				return new ValueCollection( this );
			}
		}

		ICollection<MessagePackObject> IDictionary<MessagePackObject, MessagePackObject>.Keys
		{
			get { return this.Keys; }
		}

		ICollection<MessagePackObject> IDictionary<MessagePackObject, MessagePackObject>.Values
		{
			get { return this.Values; }
		}

		[SuppressMessage( "Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Justification = "Child types should never call this property." )]
		bool ICollection<KeyValuePair<MessagePackObject, MessagePackObject>>.IsReadOnly
		{
			get { return this.IsFrozen; }
		}

		[SuppressMessage( "Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Justification = "Child types should never call this property." )]
		bool IDictionary.IsFixedSize
		{
			get { return this.IsFrozen; }
		}

		[SuppressMessage( "Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Justification = "Child types should never call this property." )]
		bool IDictionary.IsReadOnly
		{
			get { return this.IsFrozen; }
		}

		ICollection IDictionary.Keys
		{
			get { return this._keys; }
		}

		ICollection IDictionary.Values
		{
			get { return this.Values; }
		}

		[SuppressMessage( "Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Justification = "Child types should never call this property." )]
		object IDictionary.this[ object key ]
		{
			get
			{
				if ( key == null )
				{
					throw new ArgumentNullException( "key" );
				}

#if !UNITY
				Contract.EndContractBlock();
#endif // !UNITY


				var typedKey = ValidateObjectArgument( key, "key" );
				if ( typedKey.IsNil )
				{
					ThrowKeyNotNilException( "key" );
				}

				MessagePackObject value;
				if ( !this.TryGetValue( typedKey, out value ) )
				{
					return null;
				}

				return value;
			}
			set
			{
				if ( key == null )
				{
					throw new ArgumentNullException( "key" );
				}

				this.VerifyIsNotFrozen();

#if !UNITY
				Contract.EndContractBlock();
#endif // !UNITY


				var typedKey = ValidateObjectArgument( key, "key" );
				if ( typedKey.IsNil )
				{
					ThrowKeyNotNilException( "key" );
				}

				this[ typedKey ] = ValidateObjectArgument( value, "value" );
			}
		}

		[SuppressMessage( "Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Justification = "Child types should never call this property." )]
		bool ICollection.IsSynchronized
		{
			get { return false; }
		}

		[SuppressMessage( "Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Justification = "Child types should never call this property." )]
		object ICollection.SyncRoot
		{
			get { return this; }
		}

		/// <summary>
		/// Initializes an empty new instance of the <see cref="MessagePackObjectDictionary"/> class with default capacity.
		/// </summary>
		/// <remarks>
		///		This operation is an O(1) operation.
		/// </remarks>
		public MessagePackObjectDictionary()
		{
			this._keys = new List<MessagePackObject>( ListInitialCapacity );
			this._values = new List<MessagePackObject>( ListInitialCapacity );
		}

		/// <summary>
		/// Initializes an empty new instance of the <see cref="MessagePackObjectDictionary"/> class with specified initial capacity.
		/// </summary>
		/// <param name="initialCapacity">The initial capacity.</param>
		/// <exception cref="ArgumentOutOfRangeException">
		///		<paramref name="initialCapacity"/> is negative.
		/// </exception>
		/// <remarks>
		///		This operation is an O(1) operation.
		/// </remarks>
		public MessagePackObjectDictionary( int initialCapacity )
		{
			if ( initialCapacity < 0 )
			{
				throw new ArgumentOutOfRangeException( "initialCapacity" );
			}

#if !UNITY
			Contract.EndContractBlock();
#endif // !UNITY


			if ( initialCapacity <= Threashold )
			{
				this._keys = new List<MessagePackObject>( initialCapacity );
				this._values = new List<MessagePackObject>( initialCapacity );
			}
			else
			{
				this._dictionary = new Dictionary<MessagePackObject, MessagePackObject>( initialCapacity, MessagePackObjectEqualityComparer.Instance );
			}
		}

		/// <summary>
		///		Initializes a new instance of the <see cref="MessagePackObjectDictionary"/> class.
		/// </summary>
		/// <param name="dictionary">The dictionary to be copied from.</param>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="dictionary"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="ArgumentException">
		///		Failed to copy from <paramref name="dictionary"/>.
		/// </exception>
		/// <remarks>
		///		This constructor takes <em>O(N)</em> time, <em>N</em> is <see cref="P:ICollection{T}.Count"/> of <paramref name="dictionary"/>.
		///		Initial capacity will be <see cref="P:ICollection{T}.Count"/> of <paramref name="dictionary"/>.
		/// </remarks>
		public MessagePackObjectDictionary( IDictionary<MessagePackObject, MessagePackObject> dictionary )
		{
			if ( dictionary == null )
			{
				throw new ArgumentNullException( "dictionary" );
			}

#if !UNITY
			Contract.EndContractBlock();
#endif // !UNITY


			if ( dictionary.Count <= Threashold )
			{
				this._keys = new List<MessagePackObject>( dictionary.Count );
				this._values = new List<MessagePackObject>( dictionary.Count );
			}
			else
			{
				this._dictionary = new Dictionary<MessagePackObject, MessagePackObject>( dictionary.Count, MessagePackObjectEqualityComparer.Instance );
			}

			try
			{
				foreach ( var kv in dictionary )
				{
					this.AddCore( kv.Key, kv.Value, false );
				}
			}
			catch ( ArgumentException ex )
			{
#if SILVERLIGHT
				throw new ArgumentException( "Failed to copy specified dictionary.", ex );
#else
				throw new ArgumentException( "Failed to copy specified dictionary.", "dictionary", ex );
#endif
			}
		}

		private static void ThrowKeyNotNilException( string parameterName )
		{
			throw new ArgumentNullException( parameterName, "Key cannot be nil." );
		}

		private static void ThrowDuplicatedKeyException( MessagePackObject key, string parameterName )
		{
			throw new ArgumentException( String.Format( CultureInfo.CurrentCulture, "Key '{0}'({1} type) already exists in this dictionary.", key, key.UnderlyingType ), parameterName );
		}

		private void VerifyIsNotFrozen()
		{
			if ( this._isFrozen )
			{
				throw new InvalidOperationException( "This dictionary is frozen." );
			}
		}

#if !UNITY
		[Conditional( "DEBUG" )]
		private void AssertInvariant()
		{
			if ( this._dictionary == null )
			{
				Contract.Assert( this._keys != null, "this._keys != null" );
				Contract.Assert( this._values != null, "this._values != null" );
				Contract.Assert( this._keys.Count == this._values.Count, "this._keys.Count == this._values.Count" );
				Contract.Assert(
					this._keys.Distinct( MessagePackObjectEqualityComparer.Instance ).Count() == this._keys.Count,
					"this._keys.Distinct( MessagePackObjectEqualityComparer.Instance ).Count() == this._keys.Count"
				);
			}
			else
			{
				Contract.Assert( this._keys == null, "this._keys == null" );
				Contract.Assert( this._values == null, "this._values == null" );
			}
		}
#endif // !UNITY

		private static MessagePackObject ValidateObjectArgument( object obj, string parameterName )
		{
			var result = TryValidateObjectArgument( obj );
			if ( result == null )
			{
				throw new ArgumentException( String.Format( CultureInfo.CurrentCulture, "Cannot convert '{1}' to {0}.", typeof( MessagePackObject ).Name, obj.GetType() ), parameterName );
			}

			return result.Value;
		}

		private static MessagePackObject? TryValidateObjectArgument( object value )
		{
			if ( value == null )
			{
				return MessagePackObject.Nil;
			}

			if ( value is MessagePackObject )
			{
				return ( MessagePackObject )value;
			}

			byte[] asBytes;
			if ( ( asBytes = value as byte[] ) != null )
			{
				return asBytes;
			}

			string asString;
			if ( ( asString = value as string ) != null )
			{
				return asString;
			}

			MessagePackString asMessagePackString;
			if ( ( asMessagePackString = value as MessagePackString ) != null )
			{
				return new MessagePackObject( asMessagePackString );
			}

#if NETFX_CORE
			switch ( WinRTCompatibility.GetTypeCode( value.GetType() ) )
#else
			switch ( Type.GetTypeCode( value.GetType() ) )
#endif // NETFX_CORE
			{
				case TypeCode.Boolean:
				{
					return ( bool )value;
				}
				case TypeCode.Byte:
				{
					return ( byte )value;
				}
				case TypeCode.DateTime:
				{
					return MessagePackConvert.FromDateTime( ( DateTime )value );
				}
				case TypeCode.DBNull:
				case TypeCode.Empty:
				{
					return MessagePackObject.Nil;
				}
				case TypeCode.Double:
				{
					return ( double )value;
				}
				case TypeCode.Int16:
				{
					return ( short )value;
				}
				case TypeCode.Int32:
				{
					return ( int )value;
				}
				case TypeCode.Int64:
				{
					return ( long )value;
				}
				case TypeCode.SByte:
				{
					return ( sbyte )value;
				}
				case TypeCode.Single:
				{
					return ( float )value;
				}
				case TypeCode.String:
				{
					return value.ToString();
				}
				case TypeCode.UInt16:
				{
					return ( ushort )value;
				}
				case TypeCode.UInt32:
				{
					return ( uint )value;
				}
				case TypeCode.UInt64:
				{
					return ( ulong )value;
				}
				// ReSharper disable RedundantCaseLabel
				case TypeCode.Char:
				case TypeCode.Decimal:
				case TypeCode.Object:
				// ReSharper restore RedundantCaseLabel
				default:
				{
					return null;
				}
			}
		}

		/// <summary>
		///		Determines whether the <see cref="MessagePackObjectDictionary"/> contains an element with the specified key.
		/// </summary>
		/// <param name="key">The key to locate in the <see cref="MessagePackObjectDictionary"/>.</param>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="key"/> is <see cref="MessagePackObject.Nil"/>.
		/// </exception>
		/// <returns>
		///		<c>true</c> if the <see cref="MessagePackObjectDictionary"/> contains an element with the key; otherwise, <c>false</c>.
		/// </returns>
		/// <remarks>
		///		This method approaches an O(1) operation.
		/// </remarks>
		public bool ContainsKey( MessagePackObject key )
		{
			if ( key.IsNil )
			{
				ThrowKeyNotNilException( "key" );
			}

#if !UNITY
			Contract.EndContractBlock();
#endif // !UNITY


#if !UNITY
				this.AssertInvariant();
#endif // !UNITY
			return
				this._dictionary == null
				? this._keys.Contains( key, MessagePackObjectEqualityComparer.Instance )
				: this._dictionary.ContainsKey( key );
		}

		/// <summary>
		///		Determines whether the <see cref="MessagePackObjectDictionary"/> contains an element with the specified value.
		/// </summary>
		/// <param name="value">The value to locate in the <see cref="MessagePackObjectDictionary"/>.</param>
		/// <returns>
		///		<c>true</c> if the <see cref="MessagePackObjectDictionary"/> contains an element with the value; otherwise, <c>false</c>.
		/// </returns>
		/// <remarks>
		///		This method approaches an O(<em>N</em>) operation where <em>N</em> is <see cref="Count"/>.
		/// </remarks>
		public bool ContainsValue( MessagePackObject value )
		{
#if !UNITY
				this.AssertInvariant();
#endif // !UNITY
			return
				this._dictionary == null 
				? this._values.Contains( value, MessagePackObjectEqualityComparer.Instance ) 
#if !UNITY
				: this._dictionary.ContainsValue( value );
#else
				: this._dictionary.Values.Contains( value, MessagePackObjectEqualityComparer.Instance );
#endif // !UNITY
		}

		[SuppressMessage( "Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Justification = "Child types should never call this property." )]
		bool ICollection<KeyValuePair<MessagePackObject, MessagePackObject>>.Contains( KeyValuePair<MessagePackObject, MessagePackObject> item )
		{
			MessagePackObject value;
			if ( !this.TryGetValue( item.Key, out value ) )
			{
				return false;
			}

			return item.Value == value;
		}

		[SuppressMessage( "Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Justification = "Child types should never call this property." )]
		bool IDictionary.Contains( object key )
		{
			// ReSharper disable HeuristicUnreachableCode
			// ReSharper disable once ConditionIsAlwaysTrueOrFalse
			if ( key == null )
			{
				return false;
			}
			// ReSharper restore HeuristicUnreachableCode

			var typedKey = TryValidateObjectArgument( key );

			if ( typedKey.GetValueOrDefault().IsNil )
			{
				return false;
			}
			{
				return this.ContainsKey( typedKey.GetValueOrDefault() );
			}
		}

		/// <summary>
		///		Gets the value associated with the specified key.
		/// </summary>
		/// <param name="key">
		///		The key whose value to get.
		///	</param>
		/// <param name="value">
		///		When this method returns, the value associated with the specified key, if the key is found; 
		///		otherwise, the default value for the type of the <paramref name="value"/> parameter. 
		///		This parameter is passed uninitialized.
		///	</param>
		/// <returns>
		///		<c>true</c> if this dictionary contains an element with the specified key; otherwise, <c>false</c>.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="key"/> is <see cref="MessagePackObject.Nil"/>.
		/// </exception>
		///	<remarks>
		///		<para>
		///			Note that tiny integers are considered equal regardless of its CLI <see cref="Type"/>,
		///			and UTF-8 encoded bytes are considered equals to <see cref="String"/>.
		///		</para>
		///		<para>
		///			This method approaches an O(1) operation.
		///		</para>
		///	</remarks>
		public bool TryGetValue( MessagePackObject key, out MessagePackObject value )
		{
			if ( key.IsNil )
			{
				ThrowKeyNotNilException( "key" );
			}

#if !UNITY
			Contract.EndContractBlock();

			this.AssertInvariant();
#endif // !UNITY

			if ( this._dictionary == null )
			{
				int index = this._keys.FindIndex( item => item == key );
				if ( index < 0 )
				{
					value = MessagePackObject.Nil;
					return false;
				}
				else
				{
					value = this._values[ index ];
					return true;
				}
			}
			else
			{
				return this._dictionary.TryGetValue( key, out value );
			}
		}

		/// <summary>
		///		Adds the specified key and value to the dictionary.
		/// </summary>
		/// <param name="key">
		///		The key of the element to add.
		/// </param>
		/// <param name="value">
		///		The value of the element to add. The value can be <c>null</c> for reference types.
		/// </param>
		/// <returns>
		///		An element with the same key already does not exist in the dictionary and sucess to add then newly added node;
		///		otherwise <c>null</c>.
		/// </returns>
		/// <exception cref="ArgumentException">
		///		<paramref name="key"/> already exists in this dictionary.
		///		Note that tiny integers are considered equal regardless of its CLI <see cref="Type"/>,
		///		and UTF-8 encoded bytes are considered equals to <see cref="String"/>.
		/// </exception>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="key"/> is <see cref="MessagePackObject.Nil"/>.
		/// </exception>
		/// <remarks>
		///		If <see cref="Count"/> is less than the capacity, this method approaches an O(1) operation.
		///		If the capacity must be increased to accommodate the new element, 
		///		this method becomes an O(<em>N</em>) operation, where <em>N</em> is <see cref="Count"/>. 
		/// </remarks>
		public void Add( MessagePackObject key, MessagePackObject value )
		{
			if ( key.IsNil )
			{
				ThrowKeyNotNilException( "key" );
			}

			this.VerifyIsNotFrozen();

#if !UNITY
			Contract.EndContractBlock();
#endif // !UNITY


			this.AddCore( key, value, false );
		}

		private void AddCore( MessagePackObject key, MessagePackObject value, bool allowOverwrite )
		{
#if !UNITY
			Contract.Assert( !key.IsNil, "!key.IsNil" );
#endif // !UNITY

			if ( this._dictionary == null )
			{
				if ( this._keys.Count < Threashold )
				{
					int index = this._keys.FindIndex( item => item == key );
					if ( index < 0 )
					{
						this._keys.Add( key );
						this._values.Add( value );
					}
					else
					{
						if ( !allowOverwrite )
						{
							ThrowDuplicatedKeyException( key, "key" );
						}

						this._values[ index ] = value;
					}

					unchecked
					{
						this._version++;
					}
					return;
				}

				if ( this._keys.Count == Threashold && allowOverwrite )
				{
					int index = this._keys.FindIndex( item => item == key );
					if ( 0 <= index )
					{
						this._values[ index ] = value;
						unchecked
						{
							this._version++;
						}
						return;
					}
				}

				// Swith to hashtable base
				this._dictionary = new Dictionary<MessagePackObject, MessagePackObject>( DictionaryInitialCapacity, MessagePackObjectEqualityComparer.Instance );

				for ( int i = 0; i < this._keys.Count; i++ )
				{
					this._dictionary.Add( this._keys[ i ], this._values[ i ] );
				}

				this._keys = null;
				this._values = null;
			}

			if ( allowOverwrite )
			{
				this._dictionary[ key ] = value;
			}
			else
			{
				try
				{
					this._dictionary.Add( key, value );
				}
				catch ( ArgumentException )
				{
					ThrowDuplicatedKeyException( key, "key" );
				}
			}

			unchecked
			{
				this._version++;
			}
		}

		void ICollection<KeyValuePair<MessagePackObject, MessagePackObject>>.Add( KeyValuePair<MessagePackObject, MessagePackObject> item )
		{
			if ( item.Key.IsNil )
			{
				ThrowKeyNotNilException( "key" );
			}

			this.VerifyIsNotFrozen();

#if !UNITY
			Contract.EndContractBlock();
#endif // !UNITY


			this.AddCore( item.Key, item.Value, false );
		}

		void IDictionary.Add( object key, object value )
		{
			if ( key == null )
			{
				throw new ArgumentNullException( "key" );
			}

			this.VerifyIsNotFrozen();

#if !UNITY
			Contract.EndContractBlock();
#endif // !UNITY


			var typedKey = ValidateObjectArgument( key, "key" );
			if ( typedKey.IsNil )
			{
				ThrowKeyNotNilException( "key" );
			}

			this.AddCore( typedKey, ValidateObjectArgument( value, "value" ), false );
		}

		/// <summary>
		/// Removes the element with the specified key from the <see cref="MessagePackObjectDictionary"/>.
		/// </summary>
		/// <param name="key">The key of the element to remove.</param>
		/// <returns>
		///		<c>true</c> if the element is successfully removed; otherwise, <c>false</c>. 
		///		This method also returns false if <paramref name="key"/> was not found in the original <see cref="MessagePackObjectDictionary"/>.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="key"/> is <see cref="MessagePackObject.Nil"/>.
		/// </exception>
		/// <remarks>
		///		This method approaches an O(1) operation.
		/// </remarks>
		public bool Remove( MessagePackObject key )
		{
			if ( key.IsNil )
			{
				ThrowKeyNotNilException( "key" );
			}

			this.VerifyIsNotFrozen();

#if !UNITY
			Contract.EndContractBlock();
#endif // !UNITY


			return this.RemoveCore( key, default( MessagePackObject ), false );
		}

		private bool RemoveCore( MessagePackObject key, MessagePackObject value, bool checkValue )
		{
#if !UNITY
			Contract.Assert( !key.IsNil, "!key.IsNil" );
			this.AssertInvariant();
#endif // !UNITY

			if ( this._dictionary == null )
			{
				int index = this._keys.FindIndex( item => item == key );
				if ( index < 0 )
				{
					return false;
				}

				if ( checkValue && this._values[ index ] != value )
				{
					return false;
				}

				this._keys.RemoveAt( index );
				this._values.RemoveAt( index );
			}
			else
			{
				if ( checkValue )
				{
					if ( !( this._dictionary as ICollection<KeyValuePair<MessagePackObject, MessagePackObject>> ).Remove(
						new KeyValuePair<MessagePackObject, MessagePackObject>( key, value ) ) )
					{
						return false;
					}
				}
				else
				{
					if ( !this._dictionary.Remove( key ) )
					{
						return false;
					}
				}
			}

			unchecked
			{
				this._version++;
			}

			return true;
		}

		bool ICollection<KeyValuePair<MessagePackObject, MessagePackObject>>.Remove( KeyValuePair<MessagePackObject, MessagePackObject> item )
		{
			if ( item.Key.IsNil )
			{
				ThrowKeyNotNilException( "key" );
			}

			this.VerifyIsNotFrozen();

#if !UNITY
			Contract.EndContractBlock();
#endif // !UNITY


			return this.RemoveCore( item.Key, item.Value, true );
		}

		void IDictionary.Remove( object key )
		{
			if ( key == null )
			{
				throw new ArgumentNullException( "key" );
			}

			this.VerifyIsNotFrozen();

#if !UNITY
			Contract.EndContractBlock();
#endif // !UNITY


			var typedKey = ValidateObjectArgument( key, "key" );
			if ( typedKey.IsNil )
			{
				ThrowKeyNotNilException( "key" );
			}

			this.RemoveCore( typedKey, default( MessagePackObject ), false );
		}

		/// <summary>
		///		Removes all items from the <see cref="MessagePackObjectDictionary"/>.
		/// </summary>
		/// <remarks>
		///		This method approaches an O(<em>N</em>) operation, where <em>N</em> is <see cref="Count"/>.
		/// </remarks>
		public void Clear()
		{
			this.VerifyIsNotFrozen();

#if !UNITY
			this.AssertInvariant();
#endif // !UNITY

			if ( this._dictionary == null )
			{
				this._keys.Clear();
				this._values.Clear();
			}
			else
			{
				this._dictionary.Clear();
			}

			unchecked
			{
				this._version++;
			}
		}

		[SuppressMessage( "Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Justification = "Child types should never call this property." )]
		void ICollection<KeyValuePair<MessagePackObject, MessagePackObject>>.CopyTo( KeyValuePair<MessagePackObject, MessagePackObject>[] array, int arrayIndex )
		{
			CollectionOperation.CopyTo( this, this.Count, 0, array, arrayIndex, this.Count );
		}

		[SuppressMessage( "Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Justification = "Child types should never call this property." )]
		void ICollection.CopyTo( Array array, int index )
		{
			DictionaryEntry[] asDictionaryEntries;
			if ( ( asDictionaryEntries = array as DictionaryEntry[] ) != null )
			{
				CollectionOperation.CopyTo( this, this.Count, 0, asDictionaryEntries, index, array.Length, kv => new DictionaryEntry( kv.Key, kv.Value ) );
				return;
			}

			CollectionOperation.CopyTo( this, this.Count, array, index );
		}

		/// <summary>
		///		Returns an enumerator that iterates through the <see cref="MessagePackObjectDictionary"/>.
		/// </summary>
		/// <returns>
		///		Returns an enumerator that iterates through the <see cref="MessagePackObjectDictionary"/>.
		/// </returns>
		/// <remarks>
		///		This method is an O(1) operation.
		/// </remarks>
		public Enumerator GetEnumerator()
		{
			return new Enumerator( this );
		}

		IEnumerator<KeyValuePair<MessagePackObject, MessagePackObject>> IEnumerable<KeyValuePair<MessagePackObject, MessagePackObject>>.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		IDictionaryEnumerator IDictionary.GetEnumerator()
		{
			// Avoid tricky casting error.
			return new DictionaryEnumerator( this );
		}

		/// <summary>
		///		Freezes this instance.
		/// </summary>
		/// <returns>
		///		This instance itself.
		///		This value will not be <c>null</c> and its <see cref="IsFrozen"/> is <c>true</c>.
		/// </returns>
		/// <remarks>
		///		This method freezes this instance itself.
		///		This operation is an O(1) operation.
		/// </remarks>
		public MessagePackObjectDictionary Freeze()
		{
			this._isFrozen = true;
			return this;
		}

		/// <summary>
		///		Gets a copy of this instance as frozen instance.
		/// </summary>
		/// <returns>
		///		New <see cref="MessagePackObjectDictionary"/> instance which contains same items as this instance.
		///		This value will not be <c>null</c> and its <see cref="IsFrozen"/> is <c>true</c>.
		/// </returns>
		/// <remarks>
		///		This method does not freeze this instance itself.
		///		This operation is an O(<em>N</em>) operation where <em>O(N)</em> <see cref="Count"/> of items.
		/// </remarks>
		public MessagePackObjectDictionary AsFrozen()
		{
			return new MessagePackObjectDictionary( this ).Freeze();
		}
	}
}
