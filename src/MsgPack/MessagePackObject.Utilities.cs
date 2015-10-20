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
#if NETFX_CORE
using System.Reflection;
#endif
using System.Runtime.Serialization;
using System.Text;

namespace MsgPack
{
#if !SILVERLIGHT && !NETFX_CORE
	[Serializable]
#endif
	partial struct MessagePackObject
	{
		#region -- Type Code Constants --

		private static readonly ValueTypeCode _sbyteTypeCode = new ValueTypeCode( typeof( sbyte ), MessagePackValueTypeCode.Int8 );
		private static readonly ValueTypeCode _byteTypeCode = new ValueTypeCode( typeof( byte ), MessagePackValueTypeCode.UInt8 );
		private static readonly ValueTypeCode _int16TypeCode = new ValueTypeCode( typeof( short ), MessagePackValueTypeCode.Int16 );
		private static readonly ValueTypeCode _uint16TypeCode = new ValueTypeCode( typeof( ushort ), MessagePackValueTypeCode.UInt16 );
		private static readonly ValueTypeCode _int32TypeCode = new ValueTypeCode( typeof( int ), MessagePackValueTypeCode.Int32 );
		private static readonly ValueTypeCode _uint32TypeCode = new ValueTypeCode( typeof( uint ), MessagePackValueTypeCode.UInt32 );
		private static readonly ValueTypeCode _int64TypeCode = new ValueTypeCode( typeof( long ), MessagePackValueTypeCode.Int64 );
		private static readonly ValueTypeCode _uint64TypeCode = new ValueTypeCode( typeof( ulong ), MessagePackValueTypeCode.UInt64 );
		private static readonly ValueTypeCode _singleTypeCode = new ValueTypeCode( typeof( float ), MessagePackValueTypeCode.Single );
		private static readonly ValueTypeCode _doubleTypeCode = new ValueTypeCode( typeof( double ), MessagePackValueTypeCode.Double );
		private static readonly ValueTypeCode _booleanTypeCode = new ValueTypeCode( typeof( bool ), MessagePackValueTypeCode.Boolean );

		#endregion -- Type Code Constants --

		/// <summary>
		///		Instance represents nil. This is equal to default value.
		/// </summary>
		public static readonly MessagePackObject Nil = new MessagePackObject();

		#region -- Type Code Fields & Properties --

		private object _handleOrTypeCode;

		/// <summary>
		///		Get whether this instance represents nil.
		/// </summary>
		/// <value>If this instance represents nil object, then true.</value>
		public bool IsNil
		{
			get { return this._handleOrTypeCode == null; }
		}

		private ulong _value;

		#endregion -- Type Code Fields & Properties --

		#region -- Constructors --

		/// <summary>
		///		Initializes a new instance wraps <see cref="IList&lt;MessagePackObject&gt;"/>.
		/// </summary>
		/// <param name="value">
		///		The collection to be copied.
		/// </param>
		public MessagePackObject( IList<MessagePackObject> value )
			: this( value, false ) { }

		/// <summary>
		///		Initializes a new instance wraps <see cref="IList&lt;MessagePackObject&gt;"/>.
		/// </summary>
		/// <param name="value">
		///		The collection to be copied or used.
		/// </param>
		/// <param name="isImmutable">
		///		<c>true</c> if the <paramref name="value"/> is immutable collection;
		///		othereise, <c>false</c>.
		/// </param>
		/// <remarks>
		///		When the collection is truely immutable or dedicated, you can specify <c>true</c> to the <paramref name="isImmutable"/>.
		///		When <paramref name="isImmutable"/> is <c>true</c>, this constructor does not copy its contents,
		///		or copies its contents otherwise.
		///		<note>
		///			Note that both of IReadOnlyList and <see cref="System.Collections.ObjectModel.ReadOnlyCollection{T}"/> is NOT immutable
		///			because the modification to the underlying collection will be reflected to the read-only collection.
		///		</note>
		/// </remarks>
		public MessagePackObject( IList<MessagePackObject> value, bool isImmutable )
		{
			// trick: Avoid long boilerplate initialization. See "CLR via C#".
			this = new MessagePackObject();
			if ( isImmutable )
			{
				this._handleOrTypeCode = value;
			}
			else
			{
				if ( value == null )
				{
					this._handleOrTypeCode = null;
				}
				else
				{
					var copy = new MessagePackObject[ value.Count ];
					value.CopyTo( copy, 0 );
					this._handleOrTypeCode = copy;
				}
			}
		}

		/// <summary>
		///		Initializes a new instance wraps <see cref="MessagePackObjectDictionary"/>.
		/// </summary>
		/// <param name="value">
		///		The dictitonary to be copied.
		/// </param>
		public MessagePackObject( MessagePackObjectDictionary value )
			: this( value, false ) { }

		/// <summary>
		///		Initializes a new instance wraps <see cref="MessagePackObjectDictionary"/>.
		/// </summary>
		/// <param name="value">
		///		The dictitonary to be copied or used.
		/// </param>
		/// <param name="isImmutable">
		///		<c>true</c> if the <paramref name="value"/> is immutable collection;
		///		othereise, <c>false</c>.
		/// </param>
		/// <remarks>
		///		When the collection is truely immutable or dedicated, you can specify <c>true</c> to the <paramref name="isImmutable"/>.
		///		When <paramref name="isImmutable"/> is <c>true</c>, this constructor does not copy its contents,
		///		or copies its contents otherwise.
		///		<note>
		///			Note that both of IReadOnlyDictionary and ReadOnlyDictionary is NOT immutable
		///			because the modification to the underlying collection will be reflected to the read-only collection.
		///		</note>
		/// </remarks>
		public MessagePackObject( MessagePackObjectDictionary value, bool isImmutable )
		{
			// trick: Avoid long boilerplate initialization. See "CLR via C#".
			this = new MessagePackObject();
			if ( isImmutable )
			{
				this._handleOrTypeCode = value;
			}
			else
			{
				if ( value == null )
				{
					this._handleOrTypeCode = null;
				}
				else
				{
					this._handleOrTypeCode = new MessagePackObjectDictionary( value );
				}
			}
		}

		/// <summary>
		///		Initializes a new instance wraps <see cref="MessagePackString"/>.
		/// </summary>
		/// <param name="messagePackString"><see cref="MessagePackString"/> which represents byte array or UTF-8 encoded string.</param>
		internal MessagePackObject( MessagePackString messagePackString )
		{
			// trick: Avoid long boilerplate initialization. See "CLR via C#".
			this = new MessagePackObject();
			this._handleOrTypeCode = messagePackString;
		}

		#endregion -- Constructors --

		#region -- Structure Methods --

		/// <summary>
		///		Compare two instances are equal.
		/// </summary>
		/// <param name="obj"><see cref="MessagePackObject"/> instance.</param>
		/// <returns>
		///		If <paramref name="obj"/> is <see cref="MessagePackObject"/> and its value is equal to this instance, then true.
		///		Otherwise false.
		/// </returns>
		public override bool Equals( Object obj )
		{
			MessagePackObjectDictionary asDictionary;
			if ( ReferenceEquals( obj, null ) )
			{
				return this.IsNil;
			}
			else if ( ( asDictionary = obj as MessagePackObjectDictionary ) != null )
			{
				return this.Equals( new MessagePackObject( asDictionary ) );
			}
			else if ( !( obj is MessagePackObject ) )
			{
				return false;
			}
			else
			{
				return this.Equals( ( MessagePackObject )obj );
			}
		}

		/// <summary>
		///		Compare two instances are equal.
		/// </summary>
		/// <param name="other"><see cref="MessagePackObject"/> instance.</param>
		/// <returns>
		///		Whether value of <paramref name="other"/> is equal to this instance or not.
		/// </returns>
		public bool Equals( MessagePackObject other )
		{
			if ( this._handleOrTypeCode == null )
			{
				return other._handleOrTypeCode == null;
			}
			else if ( other._handleOrTypeCode == null )
			{
				return false;
			}

			var valueTypeCode = this._handleOrTypeCode as ValueTypeCode;
			if ( valueTypeCode != null )
			{
				var otherValuetypeCode = other._handleOrTypeCode as ValueTypeCode;
				if ( otherValuetypeCode == null )
				{
					return false;
				}

				return this.EqualsWhenValueType( other, valueTypeCode, otherValuetypeCode );
			}

			{
				var asMps = this._handleOrTypeCode as MessagePackString;
				if ( asMps != null )
				{
					return asMps.Equals( other._handleOrTypeCode as MessagePackString );
				}
			}

			{
				var asArray = this._handleOrTypeCode as IList<MessagePackObject>;
				if ( asArray != null )
				{
					var otherAsArray = other._handleOrTypeCode as IList<MessagePackObject>;
					if ( otherAsArray == null )
					{
						return false;
					}

					return asArray.SequenceEqual( otherAsArray, MessagePackObjectEqualityComparer.Instance );
				}
			}

			{
				var asMap = this._handleOrTypeCode as IDictionary<MessagePackObject, MessagePackObject>;
				if ( asMap != null )
				{
					var otherAsMap = other._handleOrTypeCode as IDictionary<MessagePackObject, MessagePackObject>;
					if ( otherAsMap == null )
					{
						return false;
					}

					if ( asMap.Count != otherAsMap.Count )
					{
						return false;
					}

					foreach ( var kv in asMap )
					{
						MessagePackObject value;
						if ( !otherAsMap.TryGetValue( kv.Key, out value ) )
						{
							return false;
						}

						if ( kv.Value != value )
						{
							return false;
						}
					}

					return true;
				}
			}

			{
				var asExtendedTypeObjectBody = this._handleOrTypeCode as byte[];
				if ( asExtendedTypeObjectBody != null )
				{
					var otherAsExtendedTypeObjectBody = other._handleOrTypeCode as byte[];
					if ( otherAsExtendedTypeObjectBody == null )
					{
						return false;
					}

					unchecked
					{
						return MessagePackExtendedTypeObject.Unpack( ( byte )this._value, asExtendedTypeObjectBody ) ==
							   MessagePackExtendedTypeObject.Unpack( ( byte )other._value, otherAsExtendedTypeObjectBody );
					}
				}
			}

#if DEBUG && !UNITY
			Contract.Assert( false, String.Format( "Unknown handle type this:'{0}'(value: '{1}'), other:'{2}'(value: '{3}')", this._handleOrTypeCode.GetType(), this._handleOrTypeCode, other._handleOrTypeCode.GetType(), other._handleOrTypeCode ) );
#endif // DEBUG && !UNITY
			return this._handleOrTypeCode.Equals( other._handleOrTypeCode );
		}

		private bool EqualsWhenValueType(
			MessagePackObject other,
			ValueTypeCode valueTypeCode,
			ValueTypeCode otherValuetypeCode )
		{
			if ( valueTypeCode.TypeCode == MessagePackValueTypeCode.Boolean )
			{
				if ( otherValuetypeCode.TypeCode != MessagePackValueTypeCode.Boolean )
				{
					return false;
				}

				return ( bool ) this == ( bool ) other;
			}
			else if ( otherValuetypeCode.TypeCode == MessagePackValueTypeCode.Boolean )
			{
				return false;
			}

			if ( valueTypeCode.IsInteger )
			{
				if ( otherValuetypeCode.IsInteger )
				{
					return IntegerIntegerEquals( this._value, valueTypeCode, other._value, otherValuetypeCode );
				}
				else if ( otherValuetypeCode.TypeCode == MessagePackValueTypeCode.Single )
				{
					return IntegerSingleEquals( this, other );
				}
				else if ( otherValuetypeCode.TypeCode == MessagePackValueTypeCode.Double )
				{
					return IntegerDoubleEquals( this, other );
				}
			}
			else if ( valueTypeCode.TypeCode == MessagePackValueTypeCode.Double )
			{
				if ( otherValuetypeCode.IsInteger )
				{
					return IntegerDoubleEquals( other, this );
				}
				else if ( otherValuetypeCode.TypeCode == MessagePackValueTypeCode.Single )
				{
					return ( double ) this == ( float ) other;
				}
				else if ( otherValuetypeCode.TypeCode == MessagePackValueTypeCode.Double )
				{
					// Cannot compare _value because there might be not normalized.
					return ( double ) this == ( double ) other;
				}
			}
			else if ( valueTypeCode.TypeCode == MessagePackValueTypeCode.Single )
			{
				if ( otherValuetypeCode.IsInteger )
				{
					return IntegerSingleEquals( other, this );
				}
				else if ( otherValuetypeCode.TypeCode == MessagePackValueTypeCode.Single )
				{
					// Cannot compare _value because there might be not normalized.
					return ( float ) this == ( float ) other;
				}
				else if ( otherValuetypeCode.TypeCode == MessagePackValueTypeCode.Double )
				{
					return ( float ) this == ( double ) other;
				}
			}

			return false;
		}

		private static bool IntegerIntegerEquals( ulong left, ValueTypeCode leftTypeCode, ulong right, ValueTypeCode rightTypeCode )
		{
			if ( leftTypeCode.IsSigned )
			{
				if ( rightTypeCode.IsSigned )
				{
					return left == right;
				}
				else
				{
					var leftAsInt64 = unchecked( ( long )left );
					if ( leftAsInt64 < 0L )
					{
						return false;
					}

					return left == right;
				}
			}
			else
			{
				if ( rightTypeCode.IsSigned )
				{
					var rightAsInt64 = unchecked( ( long )right );
					if ( rightAsInt64 < 0L )
					{
						return false;
					}

					return left == right;
				}
				else
				{
					return left == right;
				}
			}
		}

		private static bool IntegerSingleEquals( MessagePackObject integer, MessagePackObject real )
		{
#if DEBUG && !UNITY
			Contract.Assert( integer._handleOrTypeCode as ValueTypeCode != null, "integer._handleOrTypeCode as ValueTypeCode != null" );
#endif // DEBUG && !UNITY
			if ( ( integer._handleOrTypeCode as ValueTypeCode ).IsSigned )
			{
				return unchecked( ( long )integer._value ) == ( float )real;
			}
			else
			{
				return integer._value == ( float )real;
			}
		}

		private static bool IntegerDoubleEquals( MessagePackObject integer, MessagePackObject real )
		{
#if DEBUG && !UNITY
			Contract.Assert( integer._handleOrTypeCode as ValueTypeCode != null, "integer._handleOrTypeCode as ValueTypeCode != null" );
#endif // DEBUG && !UNITY
			if ( ( integer._handleOrTypeCode as ValueTypeCode ).IsSigned )
			{
				return unchecked( ( long )integer._value ) == ( double )real;
			}
			else
			{
				return integer._value == ( double )real;
			}
		}

		/// <summary>
		///		Get hash code of this instance.
		/// </summary>
		/// <returns>Hash code of this instance.</returns>
		public override int GetHashCode()
		{
			// ReSharper disable NonReadonlyFieldInGetHashCode
			if ( this._handleOrTypeCode == null )
			{
				return 0;
			}

			if ( this._handleOrTypeCode is ValueTypeCode )
			{
				return this._value.GetHashCode();
			}

			{
				var asMps = this._handleOrTypeCode as MessagePackString;
				if ( asMps != null )
				{
					return asMps.GetHashCode();
				}
			}

			{
				var asArray = this._handleOrTypeCode as IList<MessagePackObject>;
				if ( asArray != null )
				{
					// TODO: big array support...
					return asArray.Aggregate( 0, ( hash, item ) => hash ^ item.GetHashCode() );
				}
			}

			{
				var asMap = this._handleOrTypeCode as IDictionary<MessagePackObject, MessagePackObject>;
				if ( asMap != null )
				{
					// TODO: big map support...
					return asMap.Aggregate( 0, ( hash, item ) => hash ^ item.GetHashCode() );
				}
			}

			{
				var asExtendedTypeObjectBody = this._handleOrTypeCode as byte[];
				if ( asExtendedTypeObjectBody != null )
				{
					unchecked
					{
						return MessagePackExtendedTypeObject.Unpack( ( byte )this._value, asExtendedTypeObjectBody ).GetHashCode();
					}
				}
			}

			{
#if !UNITY
				Contract.Assert( false, String.Format( "(this._handleOrTypeCode is string) but {0}", this._handleOrTypeCode.GetType() ) );
#endif // !UNITY
				return 0;
			}
			// ReSharper restore NonReadonlyFieldInGetHashCode
		}

		/// <summary>
		/// 	Returns a string that represents the current object.
		/// </summary>
		/// <returns>
		/// 	A string that represents the current object.
		/// </returns>
		/// <remarks>
		///		<note>
		///			DO NOT use this value programmically. 
		///			The purpose of this method is informational, so format of this value subject to change.
		///		</note>
		/// </remarks>
		public override string ToString()
		{
			var buffer = new StringBuilder();
			ToString( buffer, false );
			return buffer.ToString();
		}

		private void ToString( StringBuilder buffer, bool isJson )
		{
			if ( this._handleOrTypeCode == null )
			{
				if ( isJson )
				{
					buffer.Append( "null" );
				}

				return;
			}

			ValueTypeCode valueTypeCode;
			if ( ( valueTypeCode = this._handleOrTypeCode as ValueTypeCode ) != null )
			{
				switch ( valueTypeCode.TypeCode )
				{
					case MessagePackValueTypeCode.Boolean:
					{
						if ( isJson )
						{
							buffer.Append( this.AsBoolean() ? "true" : "false" );
						}
						else
						{
							buffer.Append( this.AsBoolean() );
						}

						break;
					}
					case MessagePackValueTypeCode.Double:
					{
						buffer.Append( this.AsDouble().ToString( CultureInfo.InvariantCulture ) );
						break;
					}
					case MessagePackValueTypeCode.Single:
					{
						buffer.Append( this.AsSingle().ToString( CultureInfo.InvariantCulture ) );
						break;
					}
					default:
					{
						if ( valueTypeCode.IsSigned )
						{
							buffer.Append( unchecked( ( long )( this._value ) ).ToString( CultureInfo.InvariantCulture ) );
						}
						else
						{
							buffer.Append( this._value.ToString( CultureInfo.InvariantCulture ) );
						}

						break;
					}
				}

				return;
			}

			{
				var asArray = this._handleOrTypeCode as IList<MessagePackObject>;
				if ( asArray != null )
				{
					// TODO: big array support...
					buffer.Append( '[' );
					if ( asArray.Count > 0 )
					{
						for ( int i = 0; i < asArray.Count; i++ )
						{
							if ( i > 0 )
							{
								buffer.Append( ',' );
							}

							buffer.Append( ' ' );
							asArray[ i ].ToString( buffer, true );
						}

						buffer.Append( ' ' );
					}

					buffer.Append( ']' );
					return;
				}
			}

			{
				var asMap = this._handleOrTypeCode as IDictionary<MessagePackObject, MessagePackObject>;
				if ( asMap != null )
				{
					// TODO: big map support...
					buffer.Append( '{' );
					if ( asMap.Count > 0 )
					{
						bool isFirst = true;
						foreach ( var entry in asMap )
						{
							if ( isFirst )
							{
								isFirst = false;
							}
							else
							{
								buffer.Append( ',' );
							}

							buffer.Append( ' ' );
							entry.Key.ToString( buffer, true );
							buffer.Append( ' ' ).Append( ':' ).Append( ' ' );
							entry.Value.ToString( buffer, true );
						}

						buffer.Append( ' ' );
					}

					buffer.Append( '}' );
					return;
				}
			}

			{
				var asBinary = this._handleOrTypeCode as MessagePackString;
				if ( asBinary != null )
				{
					ToStringBinary( buffer, isJson, asBinary );
					return;
				}
			}

			{
				var asExtendedTypeObjectBody = this._handleOrTypeCode as byte[];
				if ( asExtendedTypeObjectBody != null )
				{
					MessagePackExtendedTypeObject.Unpack( ( byte )this._value, asExtendedTypeObjectBody ).ToString( buffer, isJson );
					return;
				}
			}

			// may be string
#if !UNITY
			Contract.Assert( false, String.Format( "(this._handleOrTypeCode is string) but {0}", this._handleOrTypeCode.GetType() ) );
#endif // !UNITY
			// ReSharper disable HeuristicUnreachableCode
			if ( isJson )
			{
				buffer.Append( '"' ).Append( this._handleOrTypeCode ).Append( '"' );
			}
			else
			{
				buffer.Append( this._handleOrTypeCode );
			}
			// ReSharper restore HeuristicUnreachableCode
		}

		private static void ToStringBinary( StringBuilder buffer, bool isJson, MessagePackString asBinary )
		{
			// TODO: big array support...
			var asString = asBinary.TryGetString();
			if ( asString != null )
			{
				if ( isJson )
				{
					buffer.Append( '"' );
					foreach ( var c in asString )
					{
						switch ( c )
						{
							case '"':
							{
								buffer.Append( '\\' ).Append( '"' );
								break;
							}
							case '\\':
							{
								buffer.Append( '\\' ).Append( '\\' );
								break;
							}
							case '/':
							{
								buffer.Append( '\\' ).Append( '/' );
								break;
							}
							case '\b':
							{
								buffer.Append( '\\' ).Append( 'b' );
								break;
							}
							case '\f':
							{
								buffer.Append( '\\' ).Append( 'f' );
								break;
							}
							case '\n':
							{
								buffer.Append( '\\' ).Append( 'n' );
								break;
							}
							case '\r':
							{
								buffer.Append( '\\' ).Append( 'r' );
								break;
							}
							case '\t':
							{
								buffer.Append( '\\' ).Append( 't' );
								break;
							}
							case ' ':
							{
								buffer.Append( ' ' );
								break;
							}
							default:
							{
								switch ( CharUnicodeInfo.GetUnicodeCategory( c ) )
								{
									case UnicodeCategory.Control:
									case UnicodeCategory.OtherNotAssigned:
									case UnicodeCategory.Format:
									case UnicodeCategory.LineSeparator:
									case UnicodeCategory.ParagraphSeparator:
									case UnicodeCategory.SpaceSeparator:
									case UnicodeCategory.PrivateUse:
									case UnicodeCategory.Surrogate:
									{
										buffer.Append( '\\' ).Append( 'u' ).Append( ( ( ushort ) c ).ToString( "X", CultureInfo.InvariantCulture ) );
										break;
									}
									default:
									{
										buffer.Append( c );
										break;
									}
								}

								break;
							}
						}
					}

					buffer.Append( '"' );
				}
				else
				{
					buffer.Append( asString );
				}

				return;
			}

			var asBlob = asBinary.UnsafeGetBuffer();
			if ( asBlob != null )
			{
				if ( isJson )
				{
					buffer.Append( '"' );
					Binary.ToHexString( asBlob, buffer );
					buffer.Append( '"' );
				}
				else
				{
					Binary.ToHexString( asBlob, buffer );
				}
			}
		}

		#endregion -- Structure Methods --

		#region -- Type Of Methods --

		/// <summary>
		///		Determine whether the underlying value of this instance is specified type or not.
		/// </summary>
		/// <typeparam name="T">Target type.</typeparam>
		/// <returns>If the underlying value of this instance is <typeparamref name="T"/> then true, otherwise false.</returns>
		[SuppressMessage( "Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "There are no meaningful parameter." )]
		public bool? IsTypeOf<T>()
		{
			return this.IsTypeOf( typeof( T ) );
		}

		/// <summary>
		///		Determine whether the underlying value of this instance is specified type or not.
		/// </summary>
		/// <param name="type">Target type.</param>
		/// <returns>If the underlying value of this instance is <paramref name="type"/> then true, otherwise false.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="type"/> is null.</exception>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "Switch" )]
		public bool? IsTypeOf( Type type )
		{
			if ( type == null )
			{
				throw new ArgumentNullException( "type" );
			}

#if !UNITY
			Contract.EndContractBlock();
#endif // !UNITY


			if ( this._handleOrTypeCode == null )
			{
#if NETFX_CORE
				return ( type.GetTypeInfo().IsValueType && Nullable.GetUnderlyingType( type ) == null ) ? false : default( bool? );
#else
				return ( type.IsValueType && Nullable.GetUnderlyingType( type ) == null ) ? false : default( bool? );
#endif
			}

			var typeCode = this._handleOrTypeCode as ValueTypeCode;
			if ( typeCode == null )
			{
				if ( type == typeof( MessagePackExtendedTypeObject ) )
				{
					return this._handleOrTypeCode is byte[];
				}

				if ( type == typeof( string ) || type == typeof( IList<char> ) || type == typeof( IEnumerable<char> ) )
				{
					var asMessagePackString = this._handleOrTypeCode as MessagePackString;
					return asMessagePackString != null && asMessagePackString.GetUnderlyingType() == typeof( string );
				}

				if ( type == typeof( byte[] ) || type == typeof( IList<byte> ) || type == typeof( IEnumerable<byte> ) )
				{
					return this._handleOrTypeCode is MessagePackString;
				}

				// Can IEnumerable<byte>
				if ( typeof( IEnumerable<MessagePackObject> ).IsAssignableFrom( type )
					&& this._handleOrTypeCode is MessagePackString )
				{
					// Raw is NOT array.
					return false;
				}

				return type.IsAssignableFrom( this._handleOrTypeCode.GetType() );
			}

			// Lifting support.
#if NETFX_CORE
			switch ( WinRTCompatibility.GetTypeCode( type ) )
#else
			switch ( Type.GetTypeCode( type ) )
#endif
			{
				case TypeCode.SByte:
				{
					return typeCode.IsInteger && ( this._value < 0x80 || ( 0xFFFFFFFFFFFFFF80 <= this._value && typeCode.IsSigned ) );
				}
				case TypeCode.Byte:
				{
					return typeCode.IsInteger && this._value < 0x100;
				}
				case TypeCode.Int16:
				{
					return typeCode.IsInteger && ( this._value < 0x8000 || ( 0xFFFFFFFFFFFF8000 <= this._value && typeCode.IsSigned ) );
				}
				case TypeCode.UInt16:
				{
					return typeCode.IsInteger && this._value < 0x10000;
				}
				case TypeCode.Int32:
				{
					return typeCode.IsInteger && ( this._value < 0x80000000 || ( 0xFFFFFFFF80000000 <= this._value && typeCode.IsSigned ) );
				}
				case TypeCode.UInt32:
				{
					return typeCode.IsInteger && this._value < 0x100000000L;
				}
				case TypeCode.Int64:
				{
					return typeCode.IsInteger && ( this._value < 0x8000000000000000L || typeCode.IsSigned );
				}
				case TypeCode.UInt64:
				{
					return typeCode.IsInteger && ( this._value < 0x8000000000000000L || !typeCode.IsSigned );
				}
				case TypeCode.Double:
				{
					return
						typeCode.Type == typeof( float )
						|| typeCode.Type == typeof( double );
				}
			}

			return typeCode.Type == type;
		}

		/// <summary>
		///		Get the value indicates whether this instance wraps raw binary (or string) or not.
		/// </summary>
		/// <value>This instance wraps raw binary (or string) then true.</value>
		public bool IsRaw
		{
			get { return !this.IsNil && ( this._handleOrTypeCode is MessagePackString ); }
		}

		/// <summary>
		///		Get the value indicates whether this instance wraps list (array) or not.
		/// </summary>
		/// <value>This instance wraps list (array) then true.</value>
		public bool IsList
		{
			get { return !this.IsNil && this._handleOrTypeCode is IList<MessagePackObject>; }
		}

		/// <summary>
		///		Get the value indicates whether this instance wraps list (array) or not.
		/// </summary>
		/// <value>This instance wraps list (array) then true.</value>
		public bool IsArray
		{
			get { return this.IsList; }
		}

		/// <summary>
		///		Get the value indicates whether this instance wraps dictionary (map) or not.
		/// </summary>
		/// <value>This instance wraps dictionary (map) then true.</value>
		public bool IsDictionary
		{
			get { return !this.IsNil && this._handleOrTypeCode is IDictionary<MessagePackObject, MessagePackObject>; }
		}

		/// <summary>
		///		Get the value indicates whether this instance wraps dictionary (map) or not.
		/// </summary>
		/// <value>This instance wraps dictionary (map) then true.</value>
		public bool IsMap
		{
			get { return this.IsDictionary; }
		}

		/// <summary>
		///		Get underlying type of this instance.
		/// </summary>
		/// <returns>Underlying <see cref="Type"/>.</returns>
		public Type UnderlyingType
		{
			get
			{
				if ( this._handleOrTypeCode == null )
				{
					return null;
				}

				var typeCode = this._handleOrTypeCode as ValueTypeCode;
				if ( typeCode == null )
				{
					var asMps = this._handleOrTypeCode as MessagePackString;
					if ( asMps != null )
					{
						return asMps.GetUnderlyingType();
					}
					else
					{
						return this._handleOrTypeCode.GetType();
					}
				}
				else
				{
					return typeCode.Type;
				}
			}
		}

		#endregion -- Type Of Methods --

		/// <summary>
		///		Pack this instance itself using specified <see cref="Packer"/>.
		/// </summary>
		/// <param name="packer"><see cref="Packer"/>.</param>
		/// <param name="options">Packing options. This value can be null.</param>
		/// <exception cref="ArgumentNullException"><paramref name="packer"/> is null.</exception>
		public void PackToMessage( Packer packer, PackingOptions options )
		{
			if ( packer == null )
			{
				throw new ArgumentNullException( "packer" );
			}

#if !UNITY
			Contract.EndContractBlock();
#endif // !UNITY


			if ( this._handleOrTypeCode == null )
			{
				packer.PackNull();
				return;
			}

			var typeCode = this._handleOrTypeCode as ValueTypeCode;
			if ( typeCode == null )
			{
				MessagePackString asString;
				IList<MessagePackObject> asList;
				IDictionary<MessagePackObject, MessagePackObject> asDictionary;
				byte[] asExtendedTypeObjectBody;
				if ( ( asString = this._handleOrTypeCode as MessagePackString ) != null )
				{
					packer.PackRaw( asString.GetBytes() );
				}
				else if ( ( asList = this._handleOrTypeCode as IList<MessagePackObject> ) != null )
				{
					packer.PackArrayHeader( asList.Count );
					foreach ( var item in asList )
					{
						item.PackToMessage( packer, options );
					}
				}
				else if ( ( asDictionary = this._handleOrTypeCode as IDictionary<MessagePackObject, MessagePackObject> ) != null )
				{
					packer.PackMapHeader( asDictionary.Count );
					foreach ( var item in asDictionary )
					{
						item.Key.PackToMessage( packer, options );
						item.Value.PackToMessage( packer, options );
					}
				}
				else if( ( asExtendedTypeObjectBody = this._handleOrTypeCode as byte[] ) != null )
				{
					packer.PackExtendedTypeValue( unchecked( ( byte ) this._value ), asExtendedTypeObjectBody );
				}
				else
				{
					throw new SerializationException( "Failed to pack this object." );
				}

				return;
			}

			switch ( typeCode.TypeCode )
			{
				case MessagePackValueTypeCode.Single:
				{
					packer.Pack( ( float )this );
					return;
				}
				case MessagePackValueTypeCode.Double:
				{
					packer.Pack( ( double )this );
					return;
				}
				case MessagePackValueTypeCode.Int8:
				{
					packer.Pack( ( sbyte )this );
					return;
				}
				case MessagePackValueTypeCode.Int16:
				{
					packer.Pack( ( short )this );
					return;
				}
				case MessagePackValueTypeCode.Int32:
				{
					packer.Pack( ( int )this );
					return;
				}
				case MessagePackValueTypeCode.Int64:
				{
					packer.Pack( ( long )this );
					return;
				}
				case MessagePackValueTypeCode.UInt8:
				{
					packer.Pack( ( byte )this );
					return;
				}
				case MessagePackValueTypeCode.UInt16:
				{
					packer.Pack( ( ushort )this );
					return;
				}
				case MessagePackValueTypeCode.UInt32:
				{
					packer.Pack( ( uint )this );
					return;
				}
				case MessagePackValueTypeCode.UInt64:
				{
					packer.Pack( this._value );
					return;
				}
				case MessagePackValueTypeCode.Boolean:
				{
					packer.Pack( this._value != 0 );
					return;
				}
				default:
				{
					throw new SerializationException( "Failed to pack this object." );
				}
			}
		}

		#region -- Primitive Type Conversion Methods --

		/// <summary>
		///		Gets the underlying value as string encoded with specified <see cref="Encoding"/>.
		/// </summary>
		/// <returns>
		///		The string.
		///		Note that some <see cref="Encoding"/> returns <c>null</c> if the binary is not valid encoded string.
		///	</returns>
		public string AsString( Encoding encoding )
		{
			if ( encoding == null )
			{
				throw new ArgumentNullException( "encoding" );
			}

#if !UNITY
			Contract.EndContractBlock();
#endif // !UNITY


			if ( this.IsNil )
			{
				return null;
			}

			VerifyUnderlyingType<MessagePackString>( this, null );

			try
			{
				var asMessagePackString = this._handleOrTypeCode as MessagePackString;
#if !UNITY && DEBUG
				Contract.Assert( asMessagePackString != null, "asMessagePackString != null" );
#endif // !UNITY && DEBUG

				if ( encoding is UTF8Encoding )
				{
					return asMessagePackString.GetString();
				}

				return encoding.GetString( asMessagePackString.UnsafeGetBuffer(), 0, asMessagePackString.UnsafeGetBuffer().Length );
			}
			catch ( ArgumentException ex )
			{
				throw new InvalidOperationException( String.Format( CultureInfo.CurrentCulture, "Not '{0}' string.", encoding.WebName ), ex );
			}
		}

		/// <summary>
		///		Get underlying value as UTF8 string.
		/// </summary>
		/// <returns>Underlying raw binary.</returns>
		public string AsStringUtf8()
		{
			return this.AsString( MessagePackConvert.Utf8NonBomStrict );
		}

		/// <summary>
		///		Get underlying value as UTF-16 string.
		/// </summary>
		/// <returns>Underlying string.</returns>
		/// <remarks>
		///		This method detects BOM. If BOM is not exist, them bytes should be Big-Endian UTF-16.
		/// </remarks>
		public string AsStringUtf16()
		{
			VerifyUnderlyingType<byte[]>( this, null );
#if !UNITY
			Contract.EndContractBlock();
#endif // !UNITY


			if ( this.IsNil )
			{
				return null;
			}

			try
			{
				MessagePackString asMessagePackString = this._handleOrTypeCode as MessagePackString;
#if !UNITY && DEBUG
				Contract.Assert( asMessagePackString != null, "asMessagePackString != null" );
#endif // !UNITY && DEBUG

				if ( asMessagePackString.UnsafeGetString() != null )
				{
					return asMessagePackString.UnsafeGetString();
				}

				byte[] asBytes = asMessagePackString.UnsafeGetBuffer();

				if ( asBytes.Length == 0 )
				{
					return String.Empty;
				}

				if ( asBytes.Length % 2 != 0 )
				{
					throw new InvalidOperationException( "Not UTF-16 string." );
				}

				if ( asBytes[ 0 ] == 0xff && asBytes[ 1 ] == 0xfe )
				{
					return Encoding.Unicode.GetString( asBytes, 2, asBytes.Length - 2 );
				}
				else if ( asBytes[ 0 ] == 0xfe && asBytes[ 1 ] == 0xff )
				{
					return Encoding.BigEndianUnicode.GetString( asBytes, 2, asBytes.Length - 2 );
				}
				else
				{
					return Encoding.BigEndianUnicode.GetString( asBytes, 0, asBytes.Length );
				}
			}
			catch ( ArgumentException ex )
			{
				throw new InvalidOperationException( "Not UTF-16 string.", ex );
			}
		}

		/// <summary>
		///		Get underlying value as UTF-16 charcter array.
		/// </summary>
		/// <returns>Underlying string.</returns>
		public char[] AsCharArray()
		{
			// TODO: More efficient
			return this.AsString().ToCharArray();
		}

		#endregion -- Primitive Type Conversion Methods --

		#region -- Container Type Conversion Methods --

		/// <summary>
		///		Get underlying value as <see cref="IEnumerable&lt;MessagePackObject&gt;"/>.
		/// </summary>
		/// <returns>Underlying <see cref="IEnumerable&lt;MessagePackObject&gt;"/>.</returns>
		public IEnumerable<MessagePackObject> AsEnumerable()
		{
			if ( this.IsNil )
			{
				return null;
			}

			VerifyUnderlyingType<IEnumerable<MessagePackObject>>( this, null );

			return this._handleOrTypeCode as IEnumerable<MessagePackObject>;
		}

		/// <summary>
		///		Get underlying value as <see cref="IList&lt;MessagePackObject&gt;"/>.
		/// </summary>
		/// <returns>Underlying <see cref="IList&lt;MessagePackObject&gt;"/>.</returns>
		public IList<MessagePackObject> AsList()
		{
			if ( this.IsNil )
			{
				return null;
			}

			VerifyUnderlyingType<IList<MessagePackObject>>( this, null );

			return this._handleOrTypeCode as IList<MessagePackObject>;
		}

		/// <summary>
		///		Get underlying value as <see cref="MessagePackObjectDictionary"/>.
		/// </summary>
		/// <returns>Underlying <see cref="MessagePackObjectDictionary"/>.</returns>
		public MessagePackObjectDictionary AsDictionary()
		{
			VerifyUnderlyingType<MessagePackObjectDictionary>( this, null );

			return this._handleOrTypeCode as MessagePackObjectDictionary;
		}

		#endregion -- Container Type Conversion Methods --

		#region -- Utility Methods --

		private static void VerifyUnderlyingType<T>( MessagePackObject instance, string parameterName )
		{
			if ( instance.IsNil )
			{
#if NETFX_CORE
				if ( !typeof( T ).GetTypeInfo().IsValueType || Nullable.GetUnderlyingType( typeof( T ) ) != null )
#else
				if ( !typeof( T ).IsValueType || Nullable.GetUnderlyingType( typeof( T ) ) != null )
#endif
				{
					return;
				}

				// TODO: localize
				if ( parameterName != null )
				{
					throw new ArgumentException( String.Format( CultureInfo.CurrentCulture, "Do not convert nil MessagePackObject to {0}.", typeof( T ) ), parameterName );
				}
				else
				{
					ThrowCannotBeNilAs<T>();
				}
			}

			if ( !instance.IsTypeOf<T>().GetValueOrDefault() )
			{
				if ( parameterName != null )
				{
					throw new ArgumentException( String.Format( CultureInfo.CurrentCulture, "Do not convert {0} MessagePackObject to {1}.", instance.UnderlyingType, typeof( T ) ), parameterName );
				}
				else
				{
					ThrowInvalidTypeAs<T>( instance );
				}
			}
		}

		private static void ThrowCannotBeNilAs<T>()
		{
			throw new InvalidOperationException( String.Format( CultureInfo.CurrentCulture, "Do not convert nil MessagePackObject to {0}.", typeof( T ) ) );
		}

		private static void ThrowInvalidTypeAs<T>( MessagePackObject instance )
		{
			if ( instance._handleOrTypeCode is ValueTypeCode )
			{
				throw new InvalidOperationException( String.Format( CultureInfo.CurrentCulture, "Do not convert {0} (binary:0x{2:x}) MessagePackObject to {1}.", instance.UnderlyingType, typeof( T ), instance._value ) );
			}
			else
			{
				throw new InvalidOperationException( String.Format( CultureInfo.CurrentCulture, "Do not convert {0} MessagePackObject to {1}.", instance.UnderlyingType, typeof( T ) ) );
			}
		}

		#endregion -- Utility Methods --

		/// <summary>
		///		Wraps specified object as <see cref="MessagePackObject"/> recursively.
		/// </summary>
		/// <param name="boxedValue">Object to be wrapped.</param>
		/// <returns><see cref="MessagePackObject"/> wrapps <paramref name="boxedValue"/>.</returns>
		/// <exception cref="MessageTypeException">
		///		<paramref name="boxedValue"/> is not primitive value type, list of <see cref="MessagePackObject"/>,
		///		dictionary of <see cref="MessagePackObject"/>, <see cref="String"/>, <see cref="Byte"/>[], or null.
		/// </exception>
		public static MessagePackObject FromObject( object boxedValue )
		{
			byte[] asByteArray;
			string asString;
			IEnumerable<byte> asByteEnumerable;
			IEnumerable<char> asCharEnumerable;
			IEnumerable<MessagePackObject> asEnumerable;
			MessagePackObjectDictionary asDictionary;

			// Nullable<T> is boxed as null or underlying value type, 
			// so ( obj is Nullable<T> ) is always false.
			if ( boxedValue == null )
			{
				return Nil;
			}
			else if ( boxedValue is MessagePackObject )
			{
				return ( MessagePackObject )boxedValue;
			}
			else if ( boxedValue is sbyte )
			{
				return ( sbyte )boxedValue;
			}
			else if ( boxedValue is byte )
			{
				return ( byte )boxedValue;
			}
			else if ( boxedValue is short )
			{
				return ( short )boxedValue;
			}
			else if ( boxedValue is ushort )
			{
				return ( ushort )boxedValue;
			}
			else if ( boxedValue is int )
			{
				return ( int )boxedValue;
			}
			else if ( boxedValue is uint )
			{
				return ( uint )boxedValue;
			}
			else if ( boxedValue is long )
			{
				return ( long )boxedValue;
			}
			else if ( boxedValue is ulong )
			{
				return ( ulong )boxedValue;
			}
			else if ( boxedValue is float )
			{
				return ( float )boxedValue;
			}
			else if ( boxedValue is double )
			{
				return ( double )boxedValue;
			}
			else if ( boxedValue is bool )
			{
				return ( bool )boxedValue;
			}
			else if ( ( asByteArray = boxedValue as byte[] ) != null )
			{
				return new MessagePackObject( asByteArray );
			}
			else if ( ( asString = boxedValue as string ) != null )
			{
				return new MessagePackObject( asString );
			}
			else if ( ( asByteEnumerable = boxedValue as IEnumerable<byte> ) != null )
			{
				return new MessagePackObject( ( asByteEnumerable ).ToArray() );
			}
			else if ( ( asCharEnumerable = boxedValue as IEnumerable<char> ) != null )
			{
				return new MessagePackObject( new String( ( asCharEnumerable ).ToArray() ) );
			}
			else if ( ( asEnumerable = boxedValue as IEnumerable<MessagePackObject> ) != null )
			{
				var asList = boxedValue as IList<MessagePackObject>;
				if ( asList != null )
				{
					return new MessagePackObject( asList, false );
				}
				else
				{
					return new MessagePackObject( ( asEnumerable ).ToArray(), true );
				}
			}
			else if ( ( asDictionary = boxedValue as MessagePackObjectDictionary ) != null )
			{
				return new MessagePackObject( asDictionary, false );
			}
			else if ( boxedValue is MessagePackExtendedTypeObject )
			{
				return new MessagePackObject( ( MessagePackExtendedTypeObject )boxedValue );
			}

			throw new MessageTypeException( String.Format( CultureInfo.CurrentCulture, "Type '{0}' is not supported.", boxedValue.GetType() ) );
		}

		/// <summary>
		///		Get boxed underlying value for this object.
		/// </summary>
		/// <returns>Boxed underlying value for this object.</returns>
		public object ToObject()
		{
			if ( this._handleOrTypeCode == null )
			{
				return null;
			}

			var asType = this._handleOrTypeCode as ValueTypeCode;
			if ( asType == null )
			{
				var asBinary = this._handleOrTypeCode as MessagePackString;
				if ( asBinary != null )
				{
					var asString = asBinary.TryGetString();
					if ( asString != null )
					{
						return asString;
					}

					return asBinary.UnsafeGetBuffer();
				}

				var asList = this._handleOrTypeCode as IList<MessagePackObject>;
				if ( asList != null )
				{
					return asList;
				}

				var asDictionary = this._handleOrTypeCode as IDictionary<MessagePackObject, MessagePackObject>;
				if ( asDictionary != null )
				{
					return asDictionary;
				}

				var asExtendedTypeObject = this._handleOrTypeCode as byte[];
				if ( asExtendedTypeObject != null )
				{
					return MessagePackExtendedTypeObject.Unpack( unchecked( ( byte ) this._value ), asExtendedTypeObject );
				}

#if !UNITY
				Contract.Assert( false, "Unknwon type:" + this._handleOrTypeCode );
#endif // !UNITY
				return null;
			}
			else
			{
				switch ( asType.TypeCode )
				{
					case MessagePackValueTypeCode.Boolean:
					{
						return this.AsBoolean();
					}
					case MessagePackValueTypeCode.Int8:
					{
						return this.AsSByte();
					}
					case MessagePackValueTypeCode.Int16:
					{
						return this.AsInt16();
					}
					case MessagePackValueTypeCode.Int32:
					{
						return this.AsInt32();
					}
					case MessagePackValueTypeCode.Int64:
					{
						return this.AsInt64();
					}
					case MessagePackValueTypeCode.UInt8:
					{
						return this.AsByte();
					}
					case MessagePackValueTypeCode.UInt16:
					{
						return this.AsUInt16();
					}
					case MessagePackValueTypeCode.UInt32:
					{
						return this.AsUInt32();
					}
					case MessagePackValueTypeCode.UInt64:
					{
						return this.AsUInt64();
					}
					case MessagePackValueTypeCode.Single:
					{
						return this.AsSingle();
					}
					case MessagePackValueTypeCode.Double:
					{
						return this.AsDouble();
					}
					default:
					{
#if !UNITY
						Contract.Assert( false, "Unknwon type code:" + asType.TypeCode );
#endif // !UNITY
						// ReSharper disable once HeuristicUnreachableCode
						return null;
					}
				}
			}
		}

		#region -- Structure Operator Overloads --

		/// <summary>
		///		Compare two instances are equal.
		/// </summary>
		/// <param name="left"><see cref="MessagePackObject"/> instance.</param>
		/// <param name="right"><see cref="MessagePackObject"/> instance.</param>
		/// <returns>
		///		Whether value of <paramref name="left"/> and <paramref name="right"/> are equal each other or not.
		/// </returns>
		public static bool operator ==( MessagePackObject left, MessagePackObject right )
		{
			return left.Equals( right );
		}

		/// <summary>
		///		Compare two instances are not equal.
		/// </summary>
		/// <param name="left"><see cref="MessagePackObject"/> instance.</param>
		/// <param name="right"><see cref="MessagePackObject"/> instance.</param>
		/// <returns>
		///		Whether value of <paramref name="left"/> and <paramref name="right"/> are not equal each other or are equal.
		/// </returns>
		public static bool operator !=( MessagePackObject left, MessagePackObject right )
		{
			return !left.Equals( right );
		}

		#endregion -- Structure Operator Overloads --


		#region -- Conversion Operator Overloads --

		/// <summary>
		///		Convert <see cref="MessagePackObject"/>[] instance to <see cref="MessagePackObject"/> instance.
		/// </summary>
		/// <param name="value"><see cref="MessagePackObject"/>[] instance.</param>
		/// <returns><see cref="MessagePackObject"/> instance corresponds to <paramref name="value"/>.</returns>
		public static implicit operator MessagePackObject( MessagePackObject[] value )
		{
			return new MessagePackObject( value, false );
		}

		#endregion -- Conversion Operator Overloads --

#if !SILVERLIGHT && !NETFX_CORE
		[Serializable]
#endif
		private enum MessagePackValueTypeCode
		{
			// ReSharper disable once UnusedMember.Local
			Nil = 0,
			Int8 = 1,
			Int16 = 3,
			Int32 = 5,
			Int64 = 7,
			UInt8 = 2,
			UInt16 = 4,
			UInt32 = 6,
			UInt64 = 8,
			Boolean = 10,
			Single = 11,
			Double = 13,
			Object = 16
		}

#if !SILVERLIGHT && !NETFX_CORE
		[Serializable]
#endif
		private sealed class ValueTypeCode
		{
			private readonly MessagePackValueTypeCode _typeCode;

			public MessagePackValueTypeCode TypeCode
			{
				get { return this._typeCode; }
			}

			public bool IsSigned
			{
				get { return ( ( int )this._typeCode ) % 2 != 0; }
			}

			public bool IsInteger
			{
				get { return ( ( int )this._typeCode ) < 10; }
			}

			private readonly Type _type;

			public Type Type
			{
				get { return this._type; }
			}

			internal ValueTypeCode( Type type, MessagePackValueTypeCode typeCode )
			{
				this._type = type;
				this._typeCode = typeCode;
			}

			public override string ToString()
			{
				// For debuggability.
				return this._typeCode == MessagePackValueTypeCode.Object ? this._type.FullName : this._typeCode.ToString();
			}
		}
	}
}
