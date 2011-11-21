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
using System.Numerics;

namespace MsgPack.Serialization.DefaultSerializers
{
	internal sealed class System_Numerics_ComplexMessageSerializer : MessagePackSerializer<Complex>
	{
		public System_Numerics_ComplexMessageSerializer() { }

		protected sealed override void PackToCore( Packer packer, Complex objectTree )
		{
			packer.PackMapHeader( 2 );
			packer.PackString( "Real" );
			packer.Pack( objectTree.Real );
			packer.PackString( "Imaginary" );
			packer.Pack( objectTree.Imaginary );
		}

		protected sealed override Complex UnpackFromCore( Unpacker unpacker )
		{
			double real = 0;
			double imaginary = 0;
			bool isRealFound = false;
			bool isImaginaryFound = false;
			using ( var subTreeUnpacker = unpacker.ReadSubtree() )
			{
				while ( subTreeUnpacker.Read() )
				{
					if ( !subTreeUnpacker.Data.HasValue )
					{
						throw SerializationExceptions.NewUnexpectedEndOfStream();
					}

					switch ( subTreeUnpacker.Data.Value.AsString() )
					{
						case "Real":
						{
							if ( !subTreeUnpacker.Read() )
							{
								throw SerializationExceptions.NewUnexpectedEndOfStream();
							}

							isRealFound = true;
							real = subTreeUnpacker.Data.Value.AsDouble();
							break;
						}
						case "Imaginary":
						{
							if ( !subTreeUnpacker.Read() )
							{
								throw SerializationExceptions.NewUnexpectedEndOfStream();
							}

							isImaginaryFound = true;
							imaginary = subTreeUnpacker.Data.Value.AsDouble();
							break;
						}
					}
				}
			}

			if ( !isRealFound )
			{
				throw SerializationExceptions.NewMissingProperty( "Real" );
			}

			if ( !isImaginaryFound )
			{
				throw SerializationExceptions.NewMissingProperty( "Imaginary" );
			}

			return new Complex( real, imaginary );
		}
	}
}
