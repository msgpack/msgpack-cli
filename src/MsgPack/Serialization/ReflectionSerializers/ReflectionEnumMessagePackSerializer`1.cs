#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2014 FUJIWARA, Yusuke
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

namespace MsgPack.Serialization.ReflectionSerializers
{
	/// <summary>
	///		Implements reflection-based enum serializer for restricted platforms.
	/// </summary>
	internal class ReflectionEnumMessagePackSerializer<T> : EnumMessagePackSerializer<T>
		where T : struct
	{
		public ReflectionEnumMessagePackSerializer( SerializationContext context )
			: base(
				context, 
				EnumMessagePackSerializerHelpers.DetermineEnumSerializationMethod(
					context,
					typeof( T ),
					EnumMemberSerializationMethod.Default
				)
			) { }

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
		protected internal override void PackUnderlyingValueTo( Packer packer, T enumValue )
		{
			packer.Pack( UInt64.Parse( ( ( IFormattable ) enumValue ).ToString( "D", CultureInfo.InvariantCulture ), CultureInfo.InvariantCulture ) );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
		protected internal override T UnpackFromUnderlyingValue( MessagePackObject messagePackObject )
		{
			return ( T )Enum.Parse( typeof( T ), messagePackObject.ToString(), false );
		}
	}
}
