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
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.IO;

namespace MsgPack
{
	// This file generated from Unpacking.tt T4Template.
	// Do not modify this file. Edit Unpacking.tt instead.

	static partial class Unpacking
	{
		/// <summary>
		///		Unpack <see cref="Single"/> directly from current position of specified Stream source.
		/// </summary>
		/// <param name="source">Source Stream.</param>
		/// <returns>Converted <see cref="Single"/>.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
		/// <exception cref="MessageTypeException">Read value from <paramref name="source"/> is not compatible for <see cref="Single"/>.</exception>
		public static Single UnpackSingle( Stream source )
		{
			if( source == null )
			{
				throw new ArgumentNullException( "source" );
			}


			Contract.EndContractBlock();

			int typeCode = source.ReadByte();
			if( typeCode == -1 )
			{
				throw new UnpackException( "Type header not found." );
			}
			switch ( typeCode )
			{
				case 0xca:  // float
				{
					return ReadSingle( source );
				}
				// case 0xcb:  // double
				// {
				//	return ReadDouble( source );
				// }
				default:
				{
					throw new MessageTypeException( "Not Single." );
				}
			}
		}

		/// <summary>
		///		Unpack <see cref="Single"/> directly from current position of specified byte[] source.
		/// </summary>
		/// <param name="source">Source byte[].</param>
		/// <returns>Converted <see cref="Single"/>.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
		/// <exception cref="MessageTypeException">Read value from <paramref name="source"/> is not compatible for <see cref="Single"/>.</exception>
		public static UnpackArrayResult<Single> UnpackSingle( byte[] source )
		{
			return UnpackSingle( source, 0 );
		}

		/// <summary>
		///		Unpack <see cref="Single"/> directly from specified position of specified byte[] source.
		/// </summary>
		/// <param name="source">Source byte[].</param>
		/// <param name="offset">Offset of source binary.</param>
		/// <returns>Converted <see cref="Single"/>.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="offset"/> is negative.</exception>
		/// <exception cref="MessageTypeException">Read value from <paramref name="source"/> is not compatible for <see cref="Single"/>.</exception>
		public static UnpackArrayResult<Single> UnpackSingle( byte[] source, int offset )
		{
			if( source == null )
			{
				throw new ArgumentNullException( "source" );
			}

			if( offset < 0 )
			{
				throw new ArgumentOutOfRangeException( "offset", String.Format( CultureInfo.CurrentCulture, "'{0}' is negative.", "offset" )  );
			}

			Contract.EndContractBlock();

			if( source.Length <= offset )
			{
				throw new UnpackException( "Type header not found." );
			}

			int typeCode = source[ offset ];
			switch ( typeCode )
			{
				case 0xca:  // float
				{
					return new UnpackArrayResult<Single>( ReadSingle( source, offset + 1 ), offset + sizeof( float ) + 1 );
				}
				// case 0xcb:  // double
				// {
				//	return ReadDouble( source, offset );
				// }
				default:
				{
					throw new MessageTypeException( "Not Single." );
				}
			}
		}

		/// <summary>
		///		Unpack <see cref="Double"/> directly from current position of specified Stream source.
		/// </summary>
		/// <param name="source">Source Stream.</param>
		/// <returns>Converted <see cref="Double"/>.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
		/// <exception cref="MessageTypeException">Read value from <paramref name="source"/> is not compatible for <see cref="Double"/>.</exception>
		public static Double UnpackDouble( Stream source )
		{
			if( source == null )
			{
				throw new ArgumentNullException( "source" );
			}


			Contract.EndContractBlock();

			int typeCode = source.ReadByte();
			if( typeCode == -1 )
			{
				throw new UnpackException( "Type header not found." );
			}
			switch ( typeCode )
			{
				case 0xca:  // float
				{
					return ReadSingle( source );
				}
				case 0xcb:  // double
				{
					return ReadDouble( source );
				}
				default:
				{
					throw new MessageTypeException( "Not Double." );
				}
			}
		}

		/// <summary>
		///		Unpack <see cref="Double"/> directly from current position of specified byte[] source.
		/// </summary>
		/// <param name="source">Source byte[].</param>
		/// <returns>Converted <see cref="Double"/>.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
		/// <exception cref="MessageTypeException">Read value from <paramref name="source"/> is not compatible for <see cref="Double"/>.</exception>
		public static UnpackArrayResult<Double> UnpackDouble( byte[] source )
		{
			return UnpackDouble( source, 0 );
		}

		/// <summary>
		///		Unpack <see cref="Double"/> directly from specified position of specified byte[] source.
		/// </summary>
		/// <param name="source">Source byte[].</param>
		/// <param name="offset">Offset of source binary.</param>
		/// <returns>Converted <see cref="Double"/>.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="offset"/> is negative.</exception>
		/// <exception cref="MessageTypeException">Read value from <paramref name="source"/> is not compatible for <see cref="Double"/>.</exception>
		public static UnpackArrayResult<Double> UnpackDouble( byte[] source, int offset )
		{
			if( source == null )
			{
				throw new ArgumentNullException( "source" );
			}

			if( offset < 0 )
			{
				throw new ArgumentOutOfRangeException( "offset", String.Format( CultureInfo.CurrentCulture, "'{0}' is negative.", "offset" ) );
			}

			Contract.EndContractBlock();

			if( source.Length <= offset )
			{
				throw new UnpackException( "Type header not found." );
			}

			int typeCode = source[ offset ];
			switch ( typeCode )
			{
				case 0xca:  // float
				{
					return new UnpackArrayResult<Double>( ReadSingle( source, offset + 1 ), offset + sizeof( float ) + 1 );
				}
				case 0xcb:  // double
				{
					return new UnpackArrayResult<Double>( ReadDouble( source, offset + 1 ), offset + sizeof( double ) + 1 );
				}
				default:
				{
					throw new MessageTypeException( "Not Double." );
				}
			}
		}

		/// <summary>
		///		Unpack nil directly from current position of specified Stream source.
		/// </summary>
		/// <param name="source">Source Stream.</param>
		/// <returns>Converted null instance.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
		/// <exception cref="MessageTypeException">Read value from <paramref name="source"/> is not nil.</exception>
		public static Object UnpackNull( Stream source )
		{
			if( !TryUnpackNullCore( source ) )
			{
				throw new MessageTypeException( "Not nil." );
			}

			return null;
		}

		/// <summary>
		///		Unpack nil directly from current position of specified Stream source.
		/// </summary>
		/// <param name="source">Source Stream.</param>
		/// <returns>If value is null then true.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
		public static bool TryUnpackNull( Stream source )

		{
			if( source == null )
			{
				throw new ArgumentNullException( "source" );
			}

			return TryUnpackNullCore( source );
		}

		internal static bool TryUnpackNullCore( Stream source )
		{
			int typeCode = source.ReadByte();
			if( typeCode == -1 )
			{
				throw new UnpackException( "Type header not found." );
			}
			if( typeCode == 0xc0 )
			{
				// success
				return true;
			}

			return false;
		}

		/// <summary>
		///		Unpack nil directly from current position of specified byte[] source.
		/// </summary>
		/// <param name="source">Source byte[].</param>
		/// <returns>Converted null instance.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
		/// <exception cref="MessageTypeException">Read value from <paramref name="source"/> is not nil.</exception>
		public static UnpackArrayResult<Object> UnpackNull( byte[] source )
		{
			return UnpackNull( source, 0 );
		}

		/// <summary>
		///		Unpack nil directly from specified position of specified byte[] source.
		/// </summary>
		/// <param name="source">Source byte[].</param>
		/// <param name="offset">Offset of source binary.</param>
		/// <returns>Converted null instance.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="offset"/> is negative.</exception>
		/// <exception cref="MessageTypeException">Read value from <paramref name="source"/> is not nil.</exception>
		public static UnpackArrayResult<Object> UnpackNull( byte[] source, int offset )
		{
			if( !TryUnpackNullCore( source, offset ) )
			{
				throw new MessageTypeException( "Not nil." );
			}

			return new UnpackArrayResult<Object>( null, 1 + offset );
		}

		/// <summary>
		///		Unpack nil directly from current position of specified byte[] source.
		/// </summary>
		/// <param name="source">Source byte[].</param>
		/// <returns>If value is null then true.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
		/// <exception cref="MessageTypeException">Read value from <paramref name="source"/> is not nil.</exception>
		public static bool TryUnpackNull( byte[] source )
		{
			return TryUnpackNull( source, 0 );
		}

		/// <summary>
		///		Unpack nil directly from specified position of specified byte[] source.
		/// </summary>
		/// <param name="source">Source byte[].</param>
		/// <param name="offset">Offset of source binary.</param>
		/// <returns>If value is null then true.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="offset"/> is negative.</exception>
		public static bool TryUnpackNull( byte[] source, int offset )

		{
			if( source == null )
			{
				throw new ArgumentNullException( "source" );
			}

			if( offset < 0 )
			{
				throw new ArgumentOutOfRangeException( "offset", String.Format( CultureInfo.CurrentCulture, "'{0}' is negative.", "offset" )  );
			}

			Contract.EndContractBlock();

			return TryUnpackNullCore( source, offset );
		}

		internal static bool TryUnpackNullCore( byte[] source, int offset )
		{
			if( source.Length <= offset )
			{
				throw new UnpackException( "Type header not found." );
			}

			int typeCode = source[ offset ];
			if( typeCode == 0xc0 )
			{
				// success
				return true;
			}

			return false;
		}

		/// <summary>
		///		Unpack <see cref="Boolean"/> directly from current position of specified Stream source.
		/// </summary>
		/// <param name="source">Source Stream.</param>
		/// <returns>Converted <see cref="Boolean"/> value.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
		public static bool UnpackBoolean( Stream source )

		{
			if( source == null )
			{
				throw new ArgumentNullException( "source" );
			}


			Contract.EndContractBlock();

			int typeCode = source.ReadByte();
			if( typeCode == -1 )
			{
				throw new UnpackException( "Type header not found." );
			}
			if( typeCode == 0xc2 )
			{
				// success
				return false;
			}

			if( typeCode == 0xc3 )
			{
				// success
				return true;
			}

			throw new MessageTypeException( "Not Boolean." );
		}
		/// <summary>
		///		Unpack <see cref="Boolean"/> directly from current position of specified byte[] source.
		/// </summary>
		/// <param name="source">Source byte[].</param>
		/// <returns>Converted <see cref="Boolean"/> value.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
		/// <exception cref="MessageTypeException">Read value from <paramref name="source"/> is not nil.</exception>
		public static UnpackArrayResult<Boolean> UnpackBoolean( byte[] source )
		{
			return UnpackBoolean( source, 0 );
		}

		/// <summary>
		///		Unpack <see cref="Boolean"/> directly from specified position of specified byte[] source.
		/// </summary>
		/// <param name="source">Source byte[].</param>
		/// <param name="offset">Offset of source binary.</param>
		/// <returns>Converted <see cref="Boolean"/> value.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="offset"/> is negative.</exception>
		/// <exception cref="MessageTypeException"><paramref name="source"/> is not <see cref="Boolean"/>.</exception>
		public static UnpackArrayResult<bool> UnpackBoolean( byte[] source, int offset )

		{
			if( source == null )
			{
				throw new ArgumentNullException( "source" );
			}

			if( offset < 0 )
			{
				throw new ArgumentOutOfRangeException( "offset", String.Format( CultureInfo.CurrentCulture, "'{0}' is negative.", "offset" )  );
			}

			Contract.EndContractBlock();

			if( source.Length <= offset )
			{
				throw new UnpackException( "Type header not found." );
			}

			int typeCode = source[ offset ];
			if( typeCode == 0xc2 )
			{
				// success
				return new UnpackArrayResult<bool>( false, offset + 1 );
			}

			if( typeCode == 0xc3 )
			{
				// success
				return new UnpackArrayResult<bool>( true, offset + 1 );
			}

			throw new MessageTypeException( "Not Boolean." );
		}
		/// <summary>
		///		Unpack array length directly from current position of specified Stream source.
		/// </summary>
		/// <param name="source">Source Stream.</param>
		/// <returns>Converted array length value.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
		public static Int64 UnpackArrayLength( Stream source )
		{
			if( source == null )
			{
				throw new ArgumentNullException( "source" );
			}


			Contract.EndContractBlock();

			int typeCode = source.ReadByte();
			if( typeCode == -1 )
			{
				throw new UnpackException( "Type header not found." );
			}
			if ( ( typeCode & 0xf0 ) == 0x90 )  // FixArray
			{
				return ( typeCode & 0x0f );

			}

			switch ( typeCode )
			{
				case 0xdc : // array 16
				{
					return UnpackUInt16Body( source );
				}
				case 0xdd : // array 32
				{
					return UnpackUInt32Body( source );
				}
				default:
				{
					throw new MessageTypeException( "Not array." );
				}
			}
		}
		/// <summary>
		///		Unpack array length directly from current position of specified byte[] source.
		/// </summary>
		/// <param name="source">Source byte[].</param>
		/// <returns>Converted array length value.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
		/// <exception cref="MessageTypeException">Read value from <paramref name="source"/> is not nil.</exception>
		public static UnpackArrayResult<long> UnpackArrayLength( byte[] source )
		{
			return UnpackArrayLength( source, 0 );
		}

		/// <summary>
		///		Unpack array length directly from specified position of specified byte[] source.
		/// </summary>
		/// <param name="source">Source byte[].</param>
		/// <param name="offset">Offset of source binary.</param>
		/// <returns>Converted array length value.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="offset"/> is negative.</exception>
		/// <exception cref="MessageTypeException"><paramref name="source"/> is not <see cref="Boolean"/>.</exception>
		public static UnpackArrayResult<Int64> UnpackArrayLength( byte[] source, int offset )
		{
			if( source == null )
			{
				throw new ArgumentNullException( "source" );
			}

			if( offset < 0 )
			{
				throw new ArgumentOutOfRangeException( "offset", String.Format( CultureInfo.CurrentCulture, "'{0}' is negative.", "offset" )  );
			}

			Contract.EndContractBlock();

			if( source.Length <= offset )
			{
				throw new UnpackException( "Type header not found." );
			}

			int typeCode = source[ offset ];
			if ( ( typeCode & 0xf0 ) == 0x90 )  // FixArray
			{
				return new UnpackArrayResult<Int64>( typeCode & 0x0f, 1 );
			}

			switch ( typeCode )
			{
				case 0xdc : // array 16
				{
					return new UnpackArrayResult<Int64>( UnpackUInt16Body( source, offset + 1 ), offset + sizeof( ushort ) + 1 );
				}
				case 0xdd : // array 32
				{
					return new UnpackArrayResult<Int64>( UnpackUInt32Body( source, offset + 1 ), offset + sizeof( uint ) + 1 );
				}
				default:
				{
					throw new MessageTypeException( "Not array." );
				}
			}
		}
		/// <summary>
		///		Unpack map count directly from current position of specified Stream source.
		/// </summary>
		/// <param name="source">Source Stream.</param>
		/// <returns>Converted map count value.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
		public static Int64 UnpackDictionaryCount( Stream source )
		{
			if( source == null )
			{
				throw new ArgumentNullException( "source" );
			}


			Contract.EndContractBlock();

			int typeCode = source.ReadByte();
			if( typeCode == -1 )
			{
				throw new UnpackException( "Type header not found." );
			}
			if ( ( typeCode & 0xf0 ) == 0x80 )  // FixMap
			{
				return ( typeCode & 0x0f );

			}

			switch ( typeCode )
			{
				case 0xde : // map 16
				{
					return UnpackUInt16Body( source );
				}
				case 0xdf : // map 32
				{
					return UnpackUInt32Body( source );
				}
				default:
				{
					throw new MessageTypeException( "Not map." );
				}
			}
		}

		/// <summary>
		///		Unpack map count directly from current position of specified byte[] source.
		/// </summary>
		/// <param name="source">Source byte[].</param>
		/// <returns>Converted map count value.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
		/// <exception cref="MessageTypeException">Read value from <paramref name="source"/> is not nil.</exception>
		public static UnpackArrayResult<long> UnpackDictionaryCount( byte[] source )
		{
			return UnpackDictionaryCount( source, 0 );
		}

		/// <summary>
		///		Unpack map count directly from specified position of specified byte[] source.
		/// </summary>
		/// <param name="source">Source byte[].</param>
		/// <param name="offset">Offset of source binary.</param>
		/// <returns>Converted map count value.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="offset"/> is negative.</exception>
		/// <exception cref="MessageTypeException"><paramref name="source"/> is not <see cref="Boolean"/>.</exception>
		public static UnpackArrayResult<Int64> UnpackDictionaryCount( byte[] source, int offset )
		{
			if( source == null )
			{
				throw new ArgumentNullException( "source" );
			}

			if( offset < 0 )
			{
				throw new ArgumentOutOfRangeException( "offset", String.Format( CultureInfo.CurrentCulture, "'{0}' is negative.", "offset" )  );
			}

			Contract.EndContractBlock();

			if( source.Length <= offset )
			{
				throw new UnpackException( "Type header not found." );
			}

			int typeCode = source[ offset ];
			if ( ( typeCode & 0xf0 ) == 0x80 )  // FixMap
			{
				return new UnpackArrayResult<Int64>( typeCode & 0x0f, offset + 1 );
			}

			switch ( typeCode )
			{
				case 0xde : // map 16
				{
					return new UnpackArrayResult<Int64>( UnpackUInt16Body( source, offset + 1 ), offset + sizeof( ushort ) + 1 );
				}
				case 0xdf : // map 32
				{
					return new UnpackArrayResult<Int64>( UnpackUInt32Body( source, offset + 1 ), offset + sizeof( uint ) + 1 );
				}
				default:
				{
					throw new MessageTypeException( "Not map." );
				}
			}
		}

		/// <summary>
		///		Unpack raw length directly from current position of specified Stream source.
		/// </summary>
		/// <param name="source">Source Stream.</param>
		/// <returns>Converted raw length value.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
		public static Int64 UnpackRawLength( Stream source )
		{
			if( source == null )
			{
				throw new ArgumentNullException( "source" );
			}


			Contract.EndContractBlock();

			int typeCode = source.ReadByte();
			if( typeCode == -1 )
			{
				throw new UnpackException( "Type header not found." );
			}
			if ( ( typeCode & 0xa0 ) == 0xa0 )  // FixRaw
			{
				return ( typeCode & 0x1f );

			}

			switch ( typeCode )
			{
				case 0xda : // raw 16
				{
					return UnpackUInt16Body( source );
				}
				case 0xdb : // raw 32
				{
					return UnpackUInt32Body( source );
				}
				default:
				{
					throw new MessageTypeException( "Not raw length." );
				}
			}
		}
		/// <summary>
		///		Unpack raw length directly from current position of specified byte[] source.
		/// </summary>
		/// <param name="source">Source byte[].</param>
		/// <returns>Converted raw length value.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
		/// <exception cref="MessageTypeException">Read value from <paramref name="source"/> is not nil.</exception>
		public static UnpackArrayResult<Int64> UnpackRawLength( byte[] source )
		{
			return UnpackRawLength( source, 0 );
		}

		/// <summary>
		///		Unpack raw length directly from specified position of specified byte[] source.
		/// </summary>
		/// <param name="source">Source byte[].</param>
		/// <param name="offset">Offset of source binary.</param>
		/// <returns>Converted raw length value.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="offset"/> is negative.</exception>
		/// <exception cref="MessageTypeException"><paramref name="source"/> is not <see cref="Boolean"/>.</exception>
		public static UnpackArrayResult<Int64> UnpackRawLength( byte[] source, int offset )
		{
			if( source == null )
			{
				throw new ArgumentNullException( "source" );
			}

			if( offset < 0 )
			{
				throw new ArgumentOutOfRangeException( "offset", String.Format( CultureInfo.CurrentCulture, "'{0}' is negative.", "offset" )  );
			}

			Contract.EndContractBlock();

			if( source.Length <= offset )
			{
				throw new UnpackException( "Type header not found." );
			}

			int typeCode = source[ offset ];
			if ( ( typeCode & 0xa0 ) == 0xa0 )  // FixRaw
			{
				return new UnpackArrayResult<Int64>( typeCode & 0x1f, offset + 1 );
			}

			switch ( typeCode )
			{
				case 0xda : // raw 16
				{
					return new UnpackArrayResult<Int64>( UnpackUInt16Body( source, offset + 1 ), offset + sizeof( ushort ) + 1 );
				}
				case 0xdb : // raw 32
				{
					return new UnpackArrayResult<Int64>( UnpackUInt32Body( source, offset + 1 ), offset + sizeof( uint ) + 1 );
				}
				default:
				{
					throw new MessageTypeException( "Not raw length." );
				}
			}
		}
		/// <summary>
		///		Unpack entire raw binary from specified Stream.
		/// </summary>
		/// <param name="source">Source Stream.</param>
		/// <returns>Converted raw length value.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
		/// <exception cref="MessageTypeException"><paramref name="source"/> is not raw.</exception>
		public static IEnumerable<byte> UnpackRaw( Stream source )
		{
			return UnpackRawBody( source, UnpackRawLength( source ) );
		}

		/// <summary>
		///		Unpack entire raw binary from specified Stream.
		/// </summary>
		/// <param name="source">Source Stream.</param>
		/// <param name="length">Length of raw binary.</param>
		/// <returns>Converted raw binary.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="length"/> is negative.</exception>
		public static IEnumerable<byte> UnpackRawBody( Stream source, long length )
		{
			if( source == null )
			{
				throw new ArgumentNullException( "source" );
			}

			if( length < 0L )
			{
				throw new ArgumentOutOfRangeException( "length", String.Format( CultureInfo.CurrentCulture, "'{0}' is negative.", "length" )  );
			}

			Contract.EndContractBlock();

			for( long i = 0; i < length; i++ )
			{
				int read = source.ReadByte();
				if( read < 0 )
				{
					throw new UnpackException( String.Format( CultureInfo.CurrentCulture, "End of stream. Expected length:{0}, but read length:{1}.", length, i ) );
				}
				yield return unchecked( ( byte )read );
			}
		}
		/// <summary>
		///		Unpack body of raw binary from specified byte[].
		/// </summary>
		/// <param name="source">Source byte[].</param>
		/// <param name="offset">Offset of source binary.</param>
		/// <param name="length">Length of raw binary.</param>
		/// <returns>Converted raw length value.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="offset"/> is negative.</exception>
		public static IEnumerable<byte> UnpackRawBody( byte[] source, int offset, long length )
		{
			Validation.ValidateBuffer( source, offset, length, "source", "length", true );

			Contract.EndContractBlock();

			for( long i = 0; i < length; i++ )
			{
				yield return source[ i + offset ];
			}
		}
	}
}