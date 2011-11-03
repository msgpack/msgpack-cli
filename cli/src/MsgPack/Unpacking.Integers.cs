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
using System.Diagnostics.Contracts;
using System.Globalization;
using System.IO;

namespace MsgPack
{
	// This file generated from Unpacking.Intgers.tt T4Template.
	// Do not modify this file. Edit Unpacking.Intgers.tt instead.

	static partial class Unpacking
	{

		/// <summary>
		///		Unpack <see cref="Byte"/> directly from current position of specified Stream source.
		/// </summary>
		/// <param name="source">Source Stream.</param>
		/// <returns>Converted <see cref="Byte"/>.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
		/// <exception cref="MessageTypeException">Read value from <paramref name="source"/> is not compatible for <see cref="Byte"/>.</exception>
		public static Byte UnpackByte( Stream source )
		{
			if( source == null )
			{
				throw new ArgumentNullException( "source" );
			}

			int readTypeCode = source.ReadByte();
			if( readTypeCode < 0 )
			{
				throw new UnpackException( "Cannot read type header." );
			}

			byte typeCode = unchecked( ( byte )readTypeCode );

			Contract.EndContractBlock();

			unchecked
			{
				var resultAsUInt8 = TryUnpackByte( source, typeCode );
				if ( resultAsUInt8.HasValue )
				{
					return resultAsUInt8.Value;
				}
				else
				{
					var resultAsInt8 = TryUnpackSByte( source, typeCode );
					if ( resultAsInt8.HasValue && resultAsInt8.Value >= 0 )
					{
						return ( Byte )resultAsInt8.Value;
					}
				}
			}
			throw new MessageTypeException( "Not Byte." );
		}

		/// <summary>
		///		Unpack <see cref="SByte"/> directly from current position of specified Stream source.
		/// </summary>
		/// <param name="source">Source Stream.</param>
		/// <returns>Converted <see cref="SByte"/>.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
		/// <exception cref="MessageTypeException">Read value from <paramref name="source"/> is not compatible for <see cref="SByte"/>.</exception>
		[CLSCompliant( false )]
		public static SByte UnpackSByte( Stream source )
		{
			if( source == null )
			{
				throw new ArgumentNullException( "source" );
			}

			int readTypeCode = source.ReadByte();
			if( readTypeCode < 0 )
			{
				throw new UnpackException( "Cannot read type header." );
			}

			byte typeCode = unchecked( ( byte )readTypeCode );

			Contract.EndContractBlock();

			unchecked
			{
				var resultAsInt8 = TryUnpackSByte( source, typeCode );
				if ( resultAsInt8.HasValue )
				{
					return resultAsInt8.Value;
				}
				else
				{
					var resultAsUInt8 = TryUnpackByte( source, typeCode );
					if ( resultAsUInt8.HasValue && resultAsInt8.Value <= SByte.MaxValue )
					{
						return ( SByte )resultAsUInt8.Value;
					}
				}
			}
			throw new MessageTypeException( "Not SByte." );
		}

		/// <summary>
		///		Unpack <see cref="Int16"/> directly from current position of specified Stream source.
		/// </summary>
		/// <param name="source">Source Stream.</param>
		/// <returns>Converted <see cref="Int16"/>.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
		/// <exception cref="MessageTypeException">Read value from <paramref name="source"/> is not compatible for <see cref="Int16"/>.</exception>
		public static Int16 UnpackInt16( Stream source )
		{
			if( source == null )
			{
				throw new ArgumentNullException( "source" );
			}

			int readTypeCode = source.ReadByte();
			if( readTypeCode < 0 )
			{
				throw new UnpackException( "Cannot read type header." );
			}

			byte typeCode = unchecked( ( byte )readTypeCode );

			Contract.EndContractBlock();

			unchecked
			{
				var resultAsInt8 = TryUnpackSByte( source, typeCode );
				if ( resultAsInt8.HasValue )
				{
					return resultAsInt8.Value;
				}
				else
				{
					var resultAsUInt8 = TryUnpackByte( source, typeCode );
					if ( resultAsUInt8.HasValue )
					{
						return resultAsUInt8.Value;
					}
					else
					{
						var resultAsInt16 = TryUnpackInt16( source, typeCode );
						if ( resultAsInt16.HasValue )
						{
							return resultAsInt16.Value;
						}
						else
						{
							var resultAsUInt16 = TryUnpackUInt16( source, typeCode );
							if ( resultAsUInt16.HasValue && resultAsUInt16.Value <= Int16.MaxValue )
							{
								return ( Int16 )resultAsUInt16.Value;
							}
							
						}
					}
				}
			}
			throw new MessageTypeException( "Not Int16." );
		}

		/// <summary>
		///		Unpack <see cref="UInt16"/> directly from current position of specified Stream source.
		/// </summary>
		/// <param name="source">Source Stream.</param>
		/// <returns>Converted <see cref="UInt16"/>.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
		/// <exception cref="MessageTypeException">Read value from <paramref name="source"/> is not compatible for <see cref="UInt16"/>.</exception>
		[CLSCompliant( false )]
		public static UInt16 UnpackUInt16( Stream source )
		{
			if( source == null )
			{
				throw new ArgumentNullException( "source" );
			}

			int readTypeCode = source.ReadByte();
			if( readTypeCode < 0 )
			{
				throw new UnpackException( "Cannot read type header." );
			}

			byte typeCode = unchecked( ( byte )readTypeCode );

			Contract.EndContractBlock();

			unchecked
			{
				var resultAsUInt8 = TryUnpackByte( source, typeCode );
				if ( resultAsUInt8.HasValue )
				{
					return resultAsUInt8.Value;
				}
				else
				{
					var resultAsInt8 = TryUnpackSByte( source, typeCode );
					if ( resultAsInt8.HasValue && resultAsInt8.Value >= 0 )
					{
						return ( UInt16 )resultAsInt8.Value;
					}
					else
					{
						var resultAsUInt16 = TryUnpackUInt16( source, typeCode );
						if ( resultAsUInt16.HasValue )
						{
							return resultAsUInt16.Value;
						}
						else
						{
							var resultAsInt16 = TryUnpackInt16( source, typeCode );
							if ( resultAsInt16.HasValue && resultAsInt16.Value >= 0 )
							{
								return ( UInt16 )resultAsInt16.Value;
							}
	
						}
					}
				}
			}
			throw new MessageTypeException( "Not UInt16." );
		}

		/// <summary>
		///		Unpack <see cref="Int32"/> directly from current position of specified Stream source.
		/// </summary>
		/// <param name="source">Source Stream.</param>
		/// <returns>Converted <see cref="Int32"/>.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
		/// <exception cref="MessageTypeException">Read value from <paramref name="source"/> is not compatible for <see cref="Int32"/>.</exception>
		public static Int32 UnpackInt32( Stream source )
		{
			if( source == null )
			{
				throw new ArgumentNullException( "source" );
			}

			int readTypeCode = source.ReadByte();
			if( readTypeCode < 0 )
			{
				throw new UnpackException( "Cannot read type header." );
			}

			byte typeCode = unchecked( ( byte )readTypeCode );

			Contract.EndContractBlock();

			unchecked
			{
				var resultAsInt8 = TryUnpackSByte( source, typeCode );
				if ( resultAsInt8.HasValue )
				{
					return resultAsInt8.Value;
				}
				else
				{
					var resultAsUInt8 = TryUnpackByte( source, typeCode );
					if ( resultAsUInt8.HasValue )
					{
						return resultAsUInt8.Value;
					}
					else
					{
						var resultAsInt16 = TryUnpackInt16( source, typeCode );
						if ( resultAsInt16.HasValue )
						{
							return resultAsInt16.Value;
						}
						else
						{
							var resultAsUInt16 = TryUnpackUInt16( source, typeCode );
							if ( resultAsUInt16.HasValue )
							{
								return resultAsUInt16.Value;
							}
							else
							{
								var resultAsInt32 = TryUnpackInt32( source, typeCode );
								if ( resultAsInt32.HasValue )
								{
									return resultAsInt32.Value;
								}
								else
								{
									var resultAsUInt32 = TryUnpackUInt32( source, typeCode );
									if ( resultAsUInt32.HasValue && resultAsUInt32.Value <= Int32.MaxValue )
									{
										return ( Int32 )resultAsUInt32.Value;
									}
								}
							}
						}
					}
				}
			}
			throw new MessageTypeException( "Not Int32." );
		}

		/// <summary>
		///		Unpack <see cref="UInt32"/> directly from current position of specified Stream source.
		/// </summary>
		/// <param name="source">Source Stream.</param>
		/// <returns>Converted <see cref="UInt32"/>.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
		/// <exception cref="MessageTypeException">Read value from <paramref name="source"/> is not compatible for <see cref="UInt32"/>.</exception>
		[CLSCompliant( false )]
		public static UInt32 UnpackUInt32( Stream source )
		{
			if( source == null )
			{
				throw new ArgumentNullException( "source" );
			}

			int readTypeCode = source.ReadByte();
			if( readTypeCode < 0 )
			{
				throw new UnpackException( "Cannot read type header." );
			}

			byte typeCode = unchecked( ( byte )readTypeCode );

			Contract.EndContractBlock();

			unchecked
			{
				var resultAsUInt8 = TryUnpackByte( source, typeCode );
				if ( resultAsUInt8.HasValue )
				{
					return resultAsUInt8.Value;
				}
				else
				{
					var resultAsInt8 = TryUnpackSByte( source, typeCode );
					if ( resultAsInt8.HasValue && resultAsInt8.Value >= 0 )
					{
						return ( UInt32 )resultAsInt8.Value;
					}
					else
					{
						var resultAsUInt16 = TryUnpackUInt16( source, typeCode );
						if ( resultAsUInt16.HasValue )
						{
							return resultAsUInt16.Value;
						}
						else
						{
							var resultAsInt16 = TryUnpackInt16( source, typeCode );
							if ( resultAsInt16.HasValue && resultAsInt16.Value >= 0 )
							{
								return ( UInt32 )resultAsInt16.Value;
							}
							else
							{
								var resultAsUInt32 = TryUnpackUInt32( source, typeCode );
								if ( resultAsUInt32.HasValue )
								{
									return resultAsUInt32.Value;
								}
								else
								{
									var resultAsInt32 = TryUnpackInt32( source, typeCode );
									if ( resultAsInt32.HasValue && resultAsInt32.Value >= 0 )
									{
										return ( UInt32 )resultAsInt32.Value;
									}
								}
							}
						}
					}
				}
			}
			throw new MessageTypeException( "Not UInt32." );
		}

		/// <summary>
		///		Unpack <see cref="Int64"/> directly from current position of specified Stream source.
		/// </summary>
		/// <param name="source">Source Stream.</param>
		/// <returns>Converted <see cref="Int64"/>.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
		/// <exception cref="MessageTypeException">Read value from <paramref name="source"/> is not compatible for <see cref="Int64"/>.</exception>
		public static Int64 UnpackInt64( Stream source )
		{
			if( source == null )
			{
				throw new ArgumentNullException( "source" );
			}

			int readTypeCode = source.ReadByte();
			if( readTypeCode < 0 )
			{
				throw new UnpackException( "Cannot read type header." );
			}

			byte typeCode = unchecked( ( byte )readTypeCode );

			Contract.EndContractBlock();

			unchecked
			{
				var resultAsInt8 = TryUnpackSByte( source, typeCode );
				if ( resultAsInt8.HasValue )
				{
					return resultAsInt8.Value;
				}
				else
				{
					var resultAsUInt8 = TryUnpackByte( source, typeCode );
					if ( resultAsUInt8.HasValue )
					{
						return resultAsUInt8.Value;
					}
					else
					{
						var resultAsInt16 = TryUnpackInt16( source, typeCode );
						if ( resultAsInt16.HasValue )
						{
							return resultAsInt16.Value;
						}
						else
						{
							var resultAsUInt16 = TryUnpackUInt16( source, typeCode );
							if ( resultAsUInt16.HasValue )
							{
								return resultAsUInt16.Value;
							}
							else
							{
								var resultAsInt32 = TryUnpackInt32( source, typeCode );
								if ( resultAsInt32.HasValue )
								{
									return resultAsInt32.Value;
								}
								else
								{
									var resultAsUInt32 = TryUnpackUInt32( source, typeCode );
									if ( resultAsUInt32.HasValue )
									{
										return resultAsUInt32.Value;
									}
									else
									{
										var resultAsInt64 = TryUnpackInt64( source, typeCode );
										if ( resultAsInt64.HasValue )
										{
											return resultAsInt64.Value;
										}
										else
										{
											var resultAsUInt64 = TryUnpackUInt64( source, typeCode );
											if ( resultAsUInt64.HasValue && resultAsUInt64.Value <= Int64.MaxValue )
											{
												return ( Int64 )resultAsUInt64.Value;
											}
										}
									}
								}
							}
						}
					}
				}
			}
			throw new MessageTypeException( "Not Int64." );
		}

		/// <summary>
		///		Unpack <see cref="UInt64"/> directly from current position of specified Stream source.
		/// </summary>
		/// <param name="source">Source Stream.</param>
		/// <returns>Converted <see cref="UInt64"/>.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
		/// <exception cref="MessageTypeException">Read value from <paramref name="source"/> is not compatible for <see cref="UInt64"/>.</exception>
		[CLSCompliant( false )]
		public static UInt64 UnpackUInt64( Stream source )
		{
			if( source == null )
			{
				throw new ArgumentNullException( "source" );
			}

			int readTypeCode = source.ReadByte();
			if( readTypeCode < 0 )
			{
				throw new UnpackException( "Cannot read type header." );
			}

			byte typeCode = unchecked( ( byte )readTypeCode );

			Contract.EndContractBlock();

			unchecked
			{
				var resultAsUInt8 = TryUnpackByte( source, typeCode );
				if ( resultAsUInt8.HasValue )
				{
					return resultAsUInt8.Value;
				}
				else
				{
					var resultAsInt8 = TryUnpackSByte( source, typeCode );
					if ( resultAsInt8.HasValue && resultAsInt8.Value >= 0 )
					{
						return ( UInt64 )resultAsInt8.Value;
					}
					else
					{
						var resultAsUInt16 = TryUnpackUInt16( source, typeCode );
						if ( resultAsUInt16.HasValue )
						{
							return resultAsUInt16.Value;
						}
						else
						{
							var resultAsInt16 = TryUnpackInt16( source, typeCode );
							if ( resultAsInt16.HasValue && resultAsInt16.Value >= 0 )
							{
								return ( UInt64 )resultAsInt16.Value;
							}
							else
							{
								var resultAsUInt32 = TryUnpackUInt32( source, typeCode );
								if ( resultAsUInt32.HasValue )
								{
									return resultAsUInt32.Value;
								}
								else
								{
									var resultAsInt32 = TryUnpackInt32( source, typeCode );
									if ( resultAsInt32.HasValue && resultAsInt32.Value >= 0 )
									{
										return ( UInt64 )resultAsInt32.Value;
									}
									else
									{
										var resultAsUInt64 = TryUnpackUInt64( source, typeCode );
										if ( resultAsUInt64.HasValue )
										{
											return resultAsUInt64.Value;
										}
										else
										{
											var resultAsInt64 = TryUnpackInt64( source, typeCode );
											if ( resultAsInt64.HasValue && resultAsInt64.Value >= 0 )
											{
												return ( UInt64 )resultAsInt64.Value;
											}
										}
									}
								}
							}
						}
					}
				}
			}
			throw new MessageTypeException( "Not UInt64." );
		}

		/// <summary>
		///		Unpack <see cref="Byte"/> directly from start of specified byte[] source.
		/// </summary>
		/// <param name="source">Source byte[].</param>
		/// <returns>Converted <see cref="Byte"/>.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
		/// <exception cref="MessageTypeException">Read value from <paramref name="source"/> is not compatible for <see cref="Byte"/>.</exception>
		public static UnpackArrayResult<Byte> UnpackByte( byte[] source )
		{
			return UnpackByte( source, 0 );
		}

		/// <summary>
		///		Unpack <see cref="Byte"/> directly from specified position of specified byte[] source.
		/// </summary>
		/// <param name="source">Source byte[].</param>
		/// <param name="offset">Offset of source binary.</param>
		/// <returns>Converted <see cref="Byte"/>.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="offset"/> is negative.</exception>
		/// <exception cref="MessageTypeException">Read value from <paramref name="source"/> is not compatible for <see cref="Byte"/>.</exception>
		public static UnpackArrayResult<Byte> UnpackByte( byte[] source, int offset )
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

			unchecked
			{
				var resultAsUInt8 = TryUnpackByte( source, offset );
				if ( resultAsUInt8.HasValue )
				{
					return new UnpackArrayResult<Byte>( resultAsUInt8.Value.Value, resultAsUInt8.Value.NewOffset );
				}
				else
				{
					var resultAsInt8 = TryUnpackSByte( source, offset );
					if ( resultAsInt8.HasValue && resultAsInt8.Value.Value >= 0 )
					{
						return new UnpackArrayResult<Byte>( ( Byte )resultAsInt8.Value.Value, resultAsInt8.Value.NewOffset );
					}
				}
			}
			throw new MessageTypeException( "Not Byte." );
		}

		/// <summary>
		///		Unpack <see cref="SByte"/> directly from start of specified byte[] source.
		/// </summary>
		/// <param name="source">Source byte[].</param>
		/// <returns>Converted <see cref="SByte"/>.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
		/// <exception cref="MessageTypeException">Read value from <paramref name="source"/> is not compatible for <see cref="SByte"/>.</exception>
		[CLSCompliant( false )]
		public static UnpackArrayResult<SByte> UnpackSByte( byte[] source )
		{
			return UnpackSByte( source, 0 );
		}

		/// <summary>
		///		Unpack <see cref="SByte"/> directly from specified position of specified byte[] source.
		/// </summary>
		/// <param name="source">Source byte[].</param>
		/// <param name="offset">Offset of source binary.</param>
		/// <returns>Converted <see cref="SByte"/>.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="offset"/> is negative.</exception>
		/// <exception cref="MessageTypeException">Read value from <paramref name="source"/> is not compatible for <see cref="SByte"/>.</exception>
		[CLSCompliant( false )]
		public static UnpackArrayResult<SByte> UnpackSByte( byte[] source, int offset )
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

			unchecked
			{
				var resultAsInt8 = TryUnpackSByte( source, offset );
				if ( resultAsInt8.HasValue )
				{
					return new UnpackArrayResult<SByte>( resultAsInt8.Value.Value, resultAsInt8.Value.NewOffset );
				}
				else
				{
					var resultAsUInt8 = TryUnpackByte( source, offset );
					if ( resultAsUInt8.HasValue && resultAsInt8.Value.Value <= SByte.MaxValue )
					{
						return new UnpackArrayResult<SByte>( ( SByte )resultAsUInt8.Value.Value, resultAsUInt8.Value.NewOffset );
					}
				}
			}
			throw new MessageTypeException( "Not SByte." );
		}

		/// <summary>
		///		Unpack <see cref="Int16"/> directly from start of specified byte[] source.
		/// </summary>
		/// <param name="source">Source byte[].</param>
		/// <returns>Converted <see cref="Int16"/>.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
		/// <exception cref="MessageTypeException">Read value from <paramref name="source"/> is not compatible for <see cref="Int16"/>.</exception>
		public static UnpackArrayResult<Int16> UnpackInt16( byte[] source )
		{
			return UnpackInt16( source, 0 );
		}

		/// <summary>
		///		Unpack <see cref="Int16"/> directly from specified position of specified byte[] source.
		/// </summary>
		/// <param name="source">Source byte[].</param>
		/// <param name="offset">Offset of source binary.</param>
		/// <returns>Converted <see cref="Int16"/>.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="offset"/> is negative.</exception>
		/// <exception cref="MessageTypeException">Read value from <paramref name="source"/> is not compatible for <see cref="Int16"/>.</exception>
		public static UnpackArrayResult<Int16> UnpackInt16( byte[] source, int offset )
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

			unchecked
			{
				var resultAsInt8 = TryUnpackSByte( source, offset );
				if ( resultAsInt8.HasValue )
				{
					return new UnpackArrayResult<Int16>( resultAsInt8.Value.Value, resultAsInt8.Value.NewOffset );
				}
				else
				{
					var resultAsUInt8 = TryUnpackByte( source, offset );
					if ( resultAsUInt8.HasValue )
					{
						return new UnpackArrayResult<Int16>( resultAsUInt8.Value.Value, resultAsUInt8.Value.NewOffset );
					}
					else
					{
						var resultAsInt16 = TryUnpackInt16( source, offset );
						if ( resultAsInt16.HasValue )
						{
							return new UnpackArrayResult<Int16>( resultAsInt16.Value.Value, resultAsInt16.Value.NewOffset );
						}
						else
						{
							var resultAsUInt16 = TryUnpackUInt16( source, offset );
							if ( resultAsUInt16.HasValue && resultAsUInt16.Value.Value <= Int16.MaxValue )
							{
								return new UnpackArrayResult<Int16>( ( Int16 )resultAsUInt16.Value.Value, resultAsUInt16.Value.NewOffset );
							}
							
						}
					}
				}
			}
			throw new MessageTypeException( "Not Int16." );
		}

		/// <summary>
		///		Unpack <see cref="UInt16"/> directly from start of specified byte[] source.
		/// </summary>
		/// <param name="source">Source byte[].</param>
		/// <returns>Converted <see cref="UInt16"/>.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
		/// <exception cref="MessageTypeException">Read value from <paramref name="source"/> is not compatible for <see cref="UInt16"/>.</exception>
		[CLSCompliant( false )]
		public static UnpackArrayResult<UInt16> UnpackUInt16( byte[] source )
		{
			return UnpackUInt16( source, 0 );
		}

		/// <summary>
		///		Unpack <see cref="UInt16"/> directly from specified position of specified byte[] source.
		/// </summary>
		/// <param name="source">Source byte[].</param>
		/// <param name="offset">Offset of source binary.</param>
		/// <returns>Converted <see cref="UInt16"/>.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="offset"/> is negative.</exception>
		/// <exception cref="MessageTypeException">Read value from <paramref name="source"/> is not compatible for <see cref="UInt16"/>.</exception>
		[CLSCompliant( false )]
		public static UnpackArrayResult<UInt16> UnpackUInt16( byte[] source, int offset )
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

			unchecked
			{
				var resultAsUInt8 = TryUnpackByte( source, offset );
				if ( resultAsUInt8.HasValue )
				{
					return new UnpackArrayResult<UInt16>( resultAsUInt8.Value.Value, resultAsUInt8.Value.NewOffset );
				}
				else
				{
					var resultAsInt8 = TryUnpackSByte( source, offset );
					if ( resultAsInt8.HasValue && resultAsInt8.Value.Value >= 0 )
					{
						return new UnpackArrayResult<UInt16>( ( UInt16 )resultAsInt8.Value.Value, resultAsInt8.Value.NewOffset );
					}
					else
					{
						var resultAsUInt16 = TryUnpackUInt16( source, offset );
						if ( resultAsUInt16.HasValue )
						{
							return new UnpackArrayResult<UInt16>( resultAsUInt16.Value.Value, resultAsUInt16.Value.NewOffset );
						}
						else
						{
							var resultAsInt16 = TryUnpackInt16( source, offset );
							if ( resultAsInt16.HasValue && resultAsInt16.Value.Value >= 0 )
							{
								return new UnpackArrayResult<UInt16>( ( UInt16 )resultAsInt16.Value.Value, resultAsInt16.Value.NewOffset );
							}
	
						}
					}
				}
			}
			throw new MessageTypeException( "Not UInt16." );
		}

		/// <summary>
		///		Unpack <see cref="Int32"/> directly from start of specified byte[] source.
		/// </summary>
		/// <param name="source">Source byte[].</param>
		/// <returns>Converted <see cref="Int32"/>.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
		/// <exception cref="MessageTypeException">Read value from <paramref name="source"/> is not compatible for <see cref="Int32"/>.</exception>
		public static UnpackArrayResult<Int32> UnpackInt32( byte[] source )
		{
			return UnpackInt32( source, 0 );
		}

		/// <summary>
		///		Unpack <see cref="Int32"/> directly from specified position of specified byte[] source.
		/// </summary>
		/// <param name="source">Source byte[].</param>
		/// <param name="offset">Offset of source binary.</param>
		/// <returns>Converted <see cref="Int32"/>.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="offset"/> is negative.</exception>
		/// <exception cref="MessageTypeException">Read value from <paramref name="source"/> is not compatible for <see cref="Int32"/>.</exception>
		public static UnpackArrayResult<Int32> UnpackInt32( byte[] source, int offset )
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

			unchecked
			{
				var resultAsInt8 = TryUnpackSByte( source, offset );
				if ( resultAsInt8.HasValue )
				{
					return new UnpackArrayResult<Int32>( resultAsInt8.Value.Value, resultAsInt8.Value.NewOffset );
				}
				else
				{
					var resultAsUInt8 = TryUnpackByte( source, offset );
					if ( resultAsUInt8.HasValue )
					{
						return new UnpackArrayResult<Int32>( resultAsUInt8.Value.Value, resultAsUInt8.Value.NewOffset );
					}
					else
					{
						var resultAsInt16 = TryUnpackInt16( source, offset );
						if ( resultAsInt16.HasValue )
						{
							return new UnpackArrayResult<Int32>( resultAsInt16.Value.Value, resultAsInt16.Value.NewOffset );
						}
						else
						{
							var resultAsUInt16 = TryUnpackUInt16( source, offset );
							if ( resultAsUInt16.HasValue )
							{
								return new UnpackArrayResult<Int32>( resultAsUInt16.Value.Value, resultAsUInt16.Value.NewOffset );
							}
							else
							{
								var resultAsInt32 = TryUnpackInt32( source, offset );
								if ( resultAsInt32.HasValue )
								{
									return new UnpackArrayResult<Int32>( resultAsInt32.Value.Value, resultAsInt32.Value.NewOffset );
								}
								else
								{
									var resultAsUInt32 = TryUnpackUInt32( source, offset );
									if ( resultAsUInt32.HasValue && resultAsUInt32.Value.Value <= Int32.MaxValue )
									{
										return new UnpackArrayResult<Int32>( ( Int32 )resultAsUInt32.Value.Value, resultAsUInt32.Value.NewOffset );
									}
								}
							}
						}
					}
				}
			}
			throw new MessageTypeException( "Not Int32." );
		}

		/// <summary>
		///		Unpack <see cref="UInt32"/> directly from start of specified byte[] source.
		/// </summary>
		/// <param name="source">Source byte[].</param>
		/// <returns>Converted <see cref="UInt32"/>.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
		/// <exception cref="MessageTypeException">Read value from <paramref name="source"/> is not compatible for <see cref="UInt32"/>.</exception>
		[CLSCompliant( false )]
		public static UnpackArrayResult<UInt32> UnpackUInt32( byte[] source )
		{
			return UnpackUInt32( source, 0 );
		}

		/// <summary>
		///		Unpack <see cref="UInt32"/> directly from specified position of specified byte[] source.
		/// </summary>
		/// <param name="source">Source byte[].</param>
		/// <param name="offset">Offset of source binary.</param>
		/// <returns>Converted <see cref="UInt32"/>.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="offset"/> is negative.</exception>
		/// <exception cref="MessageTypeException">Read value from <paramref name="source"/> is not compatible for <see cref="UInt32"/>.</exception>
		[CLSCompliant( false )]
		public static UnpackArrayResult<UInt32> UnpackUInt32( byte[] source, int offset )
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

			unchecked
			{
				var resultAsUInt8 = TryUnpackByte( source, offset );
				if ( resultAsUInt8.HasValue )
				{
					return new UnpackArrayResult<UInt32>( resultAsUInt8.Value.Value, resultAsUInt8.Value.NewOffset );
				}
				else
				{
					var resultAsInt8 = TryUnpackSByte( source, offset );
					if ( resultAsInt8.HasValue && resultAsInt8.Value.Value >= 0 )
					{
						return new UnpackArrayResult<UInt32>( ( UInt32 )resultAsInt8.Value.Value, resultAsInt8.Value.NewOffset );
					}
					else
					{
						var resultAsUInt16 = TryUnpackUInt16( source, offset );
						if ( resultAsUInt16.HasValue )
						{
							return new UnpackArrayResult<UInt32>( resultAsUInt16.Value.Value, resultAsUInt16.Value.NewOffset );
						}
						else
						{
							var resultAsInt16 = TryUnpackInt16( source, offset );
							if ( resultAsInt16.HasValue && resultAsInt16.Value.Value >= 0 )
							{
								return new UnpackArrayResult<UInt32>( ( UInt32 )resultAsInt16.Value.Value, resultAsInt16.Value.NewOffset );
							}
							else
							{
								var resultAsUInt32 = TryUnpackUInt32( source, offset );
								if ( resultAsUInt32.HasValue )
								{
									return new UnpackArrayResult<UInt32>( resultAsUInt32.Value.Value, resultAsUInt32.Value.NewOffset );
								}
								else
								{
									var resultAsInt32 = TryUnpackInt32( source, offset );
									if ( resultAsInt32.HasValue && resultAsInt32.Value.Value >= 0 )
									{
										return new UnpackArrayResult<UInt32>( ( UInt32 )resultAsInt32.Value.Value, resultAsInt32.Value.NewOffset );
									}
								}
							}
						}
					}
				}
			}
			throw new MessageTypeException( "Not UInt32." );
		}

		/// <summary>
		///		Unpack <see cref="Int64"/> directly from start of specified byte[] source.
		/// </summary>
		/// <param name="source">Source byte[].</param>
		/// <returns>Converted <see cref="Int64"/>.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
		/// <exception cref="MessageTypeException">Read value from <paramref name="source"/> is not compatible for <see cref="Int64"/>.</exception>
		public static UnpackArrayResult<Int64> UnpackInt64( byte[] source )
		{
			return UnpackInt64( source, 0 );
		}

		/// <summary>
		///		Unpack <see cref="Int64"/> directly from specified position of specified byte[] source.
		/// </summary>
		/// <param name="source">Source byte[].</param>
		/// <param name="offset">Offset of source binary.</param>
		/// <returns>Converted <see cref="Int64"/>.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="offset"/> is negative.</exception>
		/// <exception cref="MessageTypeException">Read value from <paramref name="source"/> is not compatible for <see cref="Int64"/>.</exception>
		public static UnpackArrayResult<Int64> UnpackInt64( byte[] source, int offset )
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

			unchecked
			{
				var resultAsInt8 = TryUnpackSByte( source, offset );
				if ( resultAsInt8.HasValue )
				{
					return new UnpackArrayResult<Int64>( resultAsInt8.Value.Value, resultAsInt8.Value.NewOffset );
				}
				else
				{
					var resultAsUInt8 = TryUnpackByte( source, offset );
					if ( resultAsUInt8.HasValue )
					{
						return new UnpackArrayResult<Int64>( resultAsUInt8.Value.Value, resultAsUInt8.Value.NewOffset );
					}
					else
					{
						var resultAsInt16 = TryUnpackInt16( source, offset );
						if ( resultAsInt16.HasValue )
						{
							return new UnpackArrayResult<Int64>( resultAsInt16.Value.Value, resultAsInt16.Value.NewOffset );
						}
						else
						{
							var resultAsUInt16 = TryUnpackUInt16( source, offset );
							if ( resultAsUInt16.HasValue )
							{
								return new UnpackArrayResult<Int64>( resultAsUInt16.Value.Value, resultAsUInt16.Value.NewOffset );
							}
							else
							{
								var resultAsInt32 = TryUnpackInt32( source, offset );
								if ( resultAsInt32.HasValue )
								{
									return new UnpackArrayResult<Int64>( resultAsInt32.Value.Value, resultAsInt32.Value.NewOffset );
								}
								else
								{
									var resultAsUInt32 = TryUnpackUInt32( source, offset );
									if ( resultAsUInt32.HasValue )
									{
										return new UnpackArrayResult<Int64>( resultAsUInt32.Value.Value, resultAsUInt32.Value.NewOffset );
									}
									else
									{
										var resultAsInt64 = TryUnpackInt64( source, offset );
										if ( resultAsInt64.HasValue )
										{
											return resultAsInt64.Value;
										}
										else
										{
											var resultAsUInt64 = TryUnpackUInt64( source, offset );
											if ( resultAsUInt64.HasValue && resultAsUInt64.Value.Value <= Int64.MaxValue )
											{
												return new UnpackArrayResult<Int64>( ( Int64 )resultAsUInt64.Value.Value, resultAsUInt64.Value.NewOffset );
											}
										}
									}
								}
							}
						}
					}
				}
			}
			throw new MessageTypeException( "Not Int64." );
		}

		/// <summary>
		///		Unpack <see cref="UInt64"/> directly from start of specified byte[] source.
		/// </summary>
		/// <param name="source">Source byte[].</param>
		/// <returns>Converted <see cref="UInt64"/>.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
		/// <exception cref="MessageTypeException">Read value from <paramref name="source"/> is not compatible for <see cref="UInt64"/>.</exception>
		[CLSCompliant( false )]
		public static UnpackArrayResult<UInt64> UnpackUInt64( byte[] source )
		{
			return UnpackUInt64( source, 0 );
		}

		/// <summary>
		///		Unpack <see cref="UInt64"/> directly from specified position of specified byte[] source.
		/// </summary>
		/// <param name="source">Source byte[].</param>
		/// <param name="offset">Offset of source binary.</param>
		/// <returns>Converted <see cref="UInt64"/>.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="source"/> is null.</exception>
		/// <exception cref="ArgumentOutOfRangeException"><paramref name="offset"/> is negative.</exception>
		/// <exception cref="MessageTypeException">Read value from <paramref name="source"/> is not compatible for <see cref="UInt64"/>.</exception>
		[CLSCompliant( false )]
		public static UnpackArrayResult<UInt64> UnpackUInt64( byte[] source, int offset )
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

			unchecked
			{
				var resultAsUInt8 = TryUnpackByte( source, offset );
				if ( resultAsUInt8.HasValue )
				{
					return new UnpackArrayResult<UInt64>( resultAsUInt8.Value.Value, resultAsUInt8.Value.NewOffset );
				}
				else
				{
					var resultAsInt8 = TryUnpackSByte( source, offset );
					if ( resultAsInt8.HasValue && resultAsInt8.Value.Value >= 0 )
					{
						return new UnpackArrayResult<UInt64>( ( UInt64 )resultAsInt8.Value.Value, resultAsInt8.Value.NewOffset );
					}
					else
					{
						var resultAsUInt16 = TryUnpackUInt16( source, offset );
						if ( resultAsUInt16.HasValue )
						{
							return new UnpackArrayResult<UInt64>( resultAsUInt16.Value.Value, resultAsUInt16.Value.NewOffset );
						}
						else
						{
							var resultAsInt16 = TryUnpackInt16( source, offset );
							if ( resultAsInt16.HasValue && resultAsInt16.Value.Value >= 0 )
							{
								return new UnpackArrayResult<UInt64>( ( UInt64 )resultAsInt16.Value.Value, resultAsInt16.Value.NewOffset );
							}
							else
							{
								var resultAsUInt32 = TryUnpackUInt32( source, offset );
								if ( resultAsUInt32.HasValue )
								{
									return new UnpackArrayResult<UInt64>( resultAsUInt32.Value.Value, resultAsUInt32.Value.NewOffset );
								}
								else
								{
									var resultAsInt32 = TryUnpackInt32( source, offset );
									if ( resultAsInt32.HasValue && resultAsInt32.Value.Value >= 0 )
									{
										return new UnpackArrayResult<UInt64>( ( UInt64 )resultAsInt32.Value.Value, resultAsInt32.Value.NewOffset );
									}
									else
									{
										var resultAsUInt64 = TryUnpackUInt64( source, offset );
										if ( resultAsUInt64.HasValue )
										{
											return resultAsUInt64.Value;
										}
										else
										{
											var resultAsInt64 = TryUnpackInt64( source, offset );
											if ( resultAsInt64.HasValue && resultAsInt64.Value.Value >= 0 )
											{
												return new UnpackArrayResult<UInt64>( ( UInt64 )resultAsInt64.Value.Value, resultAsInt64.Value.NewOffset );
											}
										}
									}
								}
							}
						}
					}
				}
			}
			throw new MessageTypeException( "Not UInt64." );
		}

		internal static Byte? TryUnpackByte( Stream source, byte typeCode )
		{
			if ( ( typeCode & 0x80 ) == 0  )   // Positive Fixnum
			{
				// It is positive fixnum value itself.
				return unchecked( ( byte )typeCode );
			}
			if( typeCode != MessagePackCode.UnsignedInt8 )
			{
				return null;
			}

			return UnpackByteBody( source );
		}

		private static Byte UnpackByteBody( Stream source )
		{
			if( source.Length < source.Position + 1 )
			{
				throw new UnpackException( "Insufficient stream length." );
			}
			var byte0 = ( byte )source.ReadByte();
			return byte0;
		}

		internal static UnpackArrayResult<Byte>? TryUnpackByte( byte[] source, int offset )
		{
			if( source.Length <= offset )
			{
				throw new UnpackException( "Type header not found." );
			}

			int typeCode = source[ offset ];
			if ( ( typeCode & 0x80 ) == 0  )   // Positive Fixnum
			{
				// It is positive fixnum value itself.
				return new UnpackArrayResult<byte>( ( byte )typeCode, offset +1 );
			}
			if( typeCode != MessagePackCode.UnsignedInt8 )
			{
				return null;
			}

			return new UnpackArrayResult<Byte>( UnpackByteBody( source, offset + 1 ), offset + sizeof( byte ) + 1 );
		}

		private static Byte UnpackByteBody( byte[] source, int offset )
		{
			if( source.Length < offset + 1 )
			{
				throw new UnpackException( "Insufficient array buffer length." );
			}
			var byte0 = source[ offset + 0 ];
			return byte0;
		}

		internal static SByte? TryUnpackSByte( Stream source, byte typeCode )
		{
			if ( ( typeCode & 0x80 ) == 0 || ( typeCode & 0xe0 ) == 0xe0 )   // Fixnum
			{
				// It is positive or negative fixnum value itself.
				return unchecked( ( sbyte )typeCode );
			}
			if( typeCode != MessagePackCode.SignedInt8 )
			{
				return null;
			}

			return UnpackSByteBody( source );
		}

		private static SByte UnpackSByteBody( Stream source )
		{
			if( source.Length < source.Position + 1 )
			{
				throw new UnpackException( "Insufficient stream length." );
			}
			var byte0 = ( byte )source.ReadByte();
			return unchecked( ( sbyte )byte0 );
		}

		internal static UnpackArrayResult<SByte>? TryUnpackSByte( byte[] source, int offset )
		{
			if( source.Length <= offset )
			{
				throw new UnpackException( "Type header not found." );
			}

			int typeCode = source[ offset ];
			if ( ( typeCode & 0x80 ) == 0 || ( typeCode & 0xe0 ) == 0xe0 )   // Fixnum
			{
				// It is positive or negative fixnum value itself.
				return new UnpackArrayResult<sbyte>( ( sbyte )typeCode, offset + 1 );
			}
			if( typeCode != MessagePackCode.SignedInt8 )
			{
				return null;
			}

			return new UnpackArrayResult<SByte>( UnpackSByteBody( source, offset + 1 ), offset + sizeof( sbyte ) + 1 );
		}

		private static SByte UnpackSByteBody( byte[] source, int offset )
		{
			if( source.Length < offset + 1 )
			{
				throw new UnpackException( "Insufficient array buffer length." );
			}
			var byte0 = source[ offset + 0 ];
			return unchecked( ( sbyte )byte0 );
		}

		internal static Int16? TryUnpackInt16( Stream source, byte typeCode )
		{
			if( typeCode != MessagePackCode.SignedInt16 )
			{
				return null;
			}

			return UnpackInt16Body( source );
		}

		private static Int16 UnpackInt16Body( Stream source )
		{
			if( source.Length < source.Position + 2 )
			{
				throw new UnpackException( "Insufficient stream length." );
			}
			var byte0 = ( byte )source.ReadByte();
			var byte1 = ( byte )source.ReadByte();
			UInt16 result = 0;
			unchecked
			{
				result |= ( UInt16 )( ( UInt16 )byte0 << 8 );
				result |= ( UInt16 )( ( UInt16 )byte1  );
			}

			return ( Int16 )result;
		}

		internal static UnpackArrayResult<Int16>? TryUnpackInt16( byte[] source, int offset )
		{
			if( source.Length <= offset )
			{
				throw new UnpackException( "Type header not found." );
			}

			int typeCode = source[ offset ];
			if( typeCode != MessagePackCode.SignedInt16 )
			{
				return null;
			}

			return new UnpackArrayResult<Int16>( UnpackInt16Body( source, offset + 1 ), offset + sizeof( short ) + 1 );
		}

		private static Int16 UnpackInt16Body( byte[] source, int offset )
		{
			if( source.Length < offset + 2 )
			{
				throw new UnpackException( "Insufficient array buffer length." );
			}
			var byte0 = source[ offset + 0 ];
			var byte1 = source[ offset + 1 ];
			UInt16 result = 0;
			unchecked
			{
				result |= ( UInt16 )( ( UInt16 )byte0 << 8 );
				result |= ( UInt16 )( ( UInt16 )byte1  );
			}

			return ( Int16 )result;
		}

		internal static UInt16? TryUnpackUInt16( Stream source, byte typeCode )
		{
			if( typeCode != MessagePackCode.UnsignedInt16 )
			{
				return null;
			}

			return UnpackUInt16Body( source );
		}

		private static UInt16 UnpackUInt16Body( Stream source )
		{
			if( source.Length < source.Position + 2 )
			{
				throw new UnpackException( "Insufficient stream length." );
			}
			var byte0 = ( byte )source.ReadByte();
			var byte1 = ( byte )source.ReadByte();
			UInt16 result = 0;
			unchecked
			{
				result |= ( UInt16 )( ( UInt16 )byte0 << 8 );
				result |= ( UInt16 )( ( UInt16 )byte1  );
			}

			return ( UInt16 )result;
		}

		internal static UnpackArrayResult<UInt16>? TryUnpackUInt16( byte[] source, int offset )
		{
			if( source.Length <= offset )
			{
				throw new UnpackException( "Type header not found." );
			}

			int typeCode = source[ offset ];
			if( typeCode != MessagePackCode.UnsignedInt16 )
			{
				return null;
			}

			return new UnpackArrayResult<UInt16>( UnpackUInt16Body( source, offset + 1 ), offset + sizeof( ushort ) + 1 );
		}

		private static UInt16 UnpackUInt16Body( byte[] source, int offset )
		{
			if( source.Length < offset + 2 )
			{
				throw new UnpackException( "Insufficient array buffer length." );
			}
			var byte0 = source[ offset + 0 ];
			var byte1 = source[ offset + 1 ];
			UInt16 result = 0;
			unchecked
			{
				result |= ( UInt16 )( ( UInt16 )byte0 << 8 );
				result |= ( UInt16 )( ( UInt16 )byte1  );
			}

			return ( UInt16 )result;
		}

		internal static Int32? TryUnpackInt32( Stream source, byte typeCode )
		{
			if( typeCode != MessagePackCode.SignedInt32 )
			{
				return null;
			}

			return UnpackInt32Body( source );
		}

		private static Int32 UnpackInt32Body( Stream source )
		{
			if( source.Length < source.Position + 4 )
			{
				throw new UnpackException( "Insufficient stream length." );
			}
			var byte0 = ( byte )source.ReadByte();
			var byte1 = ( byte )source.ReadByte();
			var byte2 = ( byte )source.ReadByte();
			var byte3 = ( byte )source.ReadByte();
			UInt32 result = 0;
			unchecked
			{
				result |= ( UInt32 )( ( UInt32 )byte0 << 24 );
				result |= ( UInt32 )( ( UInt32 )byte1 << 16 );
				result |= ( UInt32 )( ( UInt32 )byte2 << 8 );
				result |= ( UInt32 )( ( UInt32 )byte3  );
			}

			return ( Int32 )result;
		}

		internal static UnpackArrayResult<Int32>? TryUnpackInt32( byte[] source, int offset )
		{
			if( source.Length <= offset )
			{
				throw new UnpackException( "Type header not found." );
			}

			int typeCode = source[ offset ];
			if( typeCode != MessagePackCode.SignedInt32 )
			{
				return null;
			}

			return new UnpackArrayResult<Int32>( UnpackInt32Body( source, offset + 1 ), offset + sizeof( int ) + 1 );
		}

		private static Int32 UnpackInt32Body( byte[] source, int offset )
		{
			if( source.Length < offset + 4 )
			{
				throw new UnpackException( "Insufficient array buffer length." );
			}
			var byte0 = source[ offset + 0 ];
			var byte1 = source[ offset + 1 ];
			var byte2 = source[ offset + 2 ];
			var byte3 = source[ offset + 3 ];
			UInt32 result = 0;
			unchecked
			{
				result |= ( UInt32 )( ( UInt32 )byte0 << 24 );
				result |= ( UInt32 )( ( UInt32 )byte1 << 16 );
				result |= ( UInt32 )( ( UInt32 )byte2 << 8 );
				result |= ( UInt32 )( ( UInt32 )byte3  );
			}

			return ( Int32 )result;
		}

		internal static UInt32? TryUnpackUInt32( Stream source, byte typeCode )
		{
			if( typeCode != MessagePackCode.UnsignedInt32 )
			{
				return null;
			}

			return UnpackUInt32Body( source );
		}

		private static UInt32 UnpackUInt32Body( Stream source )
		{
			if( source.Length < source.Position + 4 )
			{
				throw new UnpackException( "Insufficient stream length." );
			}
			var byte0 = ( byte )source.ReadByte();
			var byte1 = ( byte )source.ReadByte();
			var byte2 = ( byte )source.ReadByte();
			var byte3 = ( byte )source.ReadByte();
			UInt32 result = 0;
			unchecked
			{
				result |= ( UInt32 )( ( UInt32 )byte0 << 24 );
				result |= ( UInt32 )( ( UInt32 )byte1 << 16 );
				result |= ( UInt32 )( ( UInt32 )byte2 << 8 );
				result |= ( UInt32 )( ( UInt32 )byte3  );
			}

			return ( UInt32 )result;
		}

		internal static UnpackArrayResult<UInt32>? TryUnpackUInt32( byte[] source, int offset )
		{
			if( source.Length <= offset )
			{
				throw new UnpackException( "Type header not found." );
			}

			int typeCode = source[ offset ];
			if( typeCode != MessagePackCode.UnsignedInt32 )
			{
				return null;
			}

			return new UnpackArrayResult<UInt32>( UnpackUInt32Body( source, offset + 1 ), offset + sizeof( uint ) + 1 );
		}

		private static UInt32 UnpackUInt32Body( byte[] source, int offset )
		{
			if( source.Length < offset + 4 )
			{
				throw new UnpackException( "Insufficient array buffer length." );
			}
			var byte0 = source[ offset + 0 ];
			var byte1 = source[ offset + 1 ];
			var byte2 = source[ offset + 2 ];
			var byte3 = source[ offset + 3 ];
			UInt32 result = 0;
			unchecked
			{
				result |= ( UInt32 )( ( UInt32 )byte0 << 24 );
				result |= ( UInt32 )( ( UInt32 )byte1 << 16 );
				result |= ( UInt32 )( ( UInt32 )byte2 << 8 );
				result |= ( UInt32 )( ( UInt32 )byte3  );
			}

			return ( UInt32 )result;
		}

		internal static Int64? TryUnpackInt64( Stream source, byte typeCode )
		{
			if( typeCode != MessagePackCode.SignedInt64 )
			{
				return null;
			}

			return UnpackInt64Body( source );
		}

		private static Int64 UnpackInt64Body( Stream source )
		{
			if( source.Length < source.Position + 8 )
			{
				throw new UnpackException( "Insufficient stream length." );
			}
			var byte0 = ( byte )source.ReadByte();
			var byte1 = ( byte )source.ReadByte();
			var byte2 = ( byte )source.ReadByte();
			var byte3 = ( byte )source.ReadByte();
			var byte4 = ( byte )source.ReadByte();
			var byte5 = ( byte )source.ReadByte();
			var byte6 = ( byte )source.ReadByte();
			var byte7 = ( byte )source.ReadByte();
			UInt64 result = 0;
			unchecked
			{
				result |= ( UInt64 )( ( UInt64 )byte0 << 56 );
				result |= ( UInt64 )( ( UInt64 )byte1 << 48 );
				result |= ( UInt64 )( ( UInt64 )byte2 << 40 );
				result |= ( UInt64 )( ( UInt64 )byte3 << 32 );
				result |= ( UInt64 )( ( UInt64 )byte4 << 24 );
				result |= ( UInt64 )( ( UInt64 )byte5 << 16 );
				result |= ( UInt64 )( ( UInt64 )byte6 << 8 );
				result |= ( UInt64 )( ( UInt64 )byte7  );
			}

			return ( Int64 )result;
		}

		internal static UnpackArrayResult<Int64>? TryUnpackInt64( byte[] source, int offset )
		{
			if( source.Length <= offset )
			{
				throw new UnpackException( "Type header not found." );
			}

			int typeCode = source[ offset ];
			if( typeCode != MessagePackCode.SignedInt64 )
			{
				return null;
			}

			return new UnpackArrayResult<Int64>( UnpackInt64Body( source, offset + 1 ), offset + sizeof( long ) + 1 );
		}

		private static Int64 UnpackInt64Body( byte[] source, int offset )
		{
			if( source.Length < offset + 8 )
			{
				throw new UnpackException( "Insufficient array buffer length." );
			}
			var byte0 = source[ offset + 0 ];
			var byte1 = source[ offset + 1 ];
			var byte2 = source[ offset + 2 ];
			var byte3 = source[ offset + 3 ];
			var byte4 = source[ offset + 4 ];
			var byte5 = source[ offset + 5 ];
			var byte6 = source[ offset + 6 ];
			var byte7 = source[ offset + 7 ];
			UInt64 result = 0;
			unchecked
			{
				result |= ( UInt64 )( ( UInt64 )byte0 << 56 );
				result |= ( UInt64 )( ( UInt64 )byte1 << 48 );
				result |= ( UInt64 )( ( UInt64 )byte2 << 40 );
				result |= ( UInt64 )( ( UInt64 )byte3 << 32 );
				result |= ( UInt64 )( ( UInt64 )byte4 << 24 );
				result |= ( UInt64 )( ( UInt64 )byte5 << 16 );
				result |= ( UInt64 )( ( UInt64 )byte6 << 8 );
				result |= ( UInt64 )( ( UInt64 )byte7  );
			}

			return ( Int64 )result;
		}

		internal static UInt64? TryUnpackUInt64( Stream source, byte typeCode )
		{
			if( typeCode != MessagePackCode.UnsignedInt64 )
			{
				return null;
			}

			return UnpackUInt64Body( source );
		}

		private static UInt64 UnpackUInt64Body( Stream source )
		{
			if( source.Length < source.Position + 8 )
			{
				throw new UnpackException( "Insufficient stream length." );
			}
			var byte0 = ( byte )source.ReadByte();
			var byte1 = ( byte )source.ReadByte();
			var byte2 = ( byte )source.ReadByte();
			var byte3 = ( byte )source.ReadByte();
			var byte4 = ( byte )source.ReadByte();
			var byte5 = ( byte )source.ReadByte();
			var byte6 = ( byte )source.ReadByte();
			var byte7 = ( byte )source.ReadByte();
			UInt64 result = 0;
			unchecked
			{
				result |= ( UInt64 )( ( UInt64 )byte0 << 56 );
				result |= ( UInt64 )( ( UInt64 )byte1 << 48 );
				result |= ( UInt64 )( ( UInt64 )byte2 << 40 );
				result |= ( UInt64 )( ( UInt64 )byte3 << 32 );
				result |= ( UInt64 )( ( UInt64 )byte4 << 24 );
				result |= ( UInt64 )( ( UInt64 )byte5 << 16 );
				result |= ( UInt64 )( ( UInt64 )byte6 << 8 );
				result |= ( UInt64 )( ( UInt64 )byte7  );
			}

			return ( UInt64 )result;
		}

		internal static UnpackArrayResult<UInt64>? TryUnpackUInt64( byte[] source, int offset )
		{
			if( source.Length <= offset )
			{
				throw new UnpackException( "Type header not found." );
			}

			int typeCode = source[ offset ];
			if( typeCode != MessagePackCode.UnsignedInt64 )
			{
				return null;
			}

			return new UnpackArrayResult<UInt64>( UnpackUInt64Body( source, offset + 1 ), offset + sizeof( ulong ) + 1 );
		}

		private static UInt64 UnpackUInt64Body( byte[] source, int offset )
		{
			if( source.Length < offset + 8 )
			{
				throw new UnpackException( "Insufficient array buffer length." );
			}
			var byte0 = source[ offset + 0 ];
			var byte1 = source[ offset + 1 ];
			var byte2 = source[ offset + 2 ];
			var byte3 = source[ offset + 3 ];
			var byte4 = source[ offset + 4 ];
			var byte5 = source[ offset + 5 ];
			var byte6 = source[ offset + 6 ];
			var byte7 = source[ offset + 7 ];
			UInt64 result = 0;
			unchecked
			{
				result |= ( UInt64 )( ( UInt64 )byte0 << 56 );
				result |= ( UInt64 )( ( UInt64 )byte1 << 48 );
				result |= ( UInt64 )( ( UInt64 )byte2 << 40 );
				result |= ( UInt64 )( ( UInt64 )byte3 << 32 );
				result |= ( UInt64 )( ( UInt64 )byte4 << 24 );
				result |= ( UInt64 )( ( UInt64 )byte5 << 16 );
				result |= ( UInt64 )( ( UInt64 )byte6 << 8 );
				result |= ( UInt64 )( ( UInt64 )byte7  );
			}

			return ( UInt64 )result;
		}
	}
}
