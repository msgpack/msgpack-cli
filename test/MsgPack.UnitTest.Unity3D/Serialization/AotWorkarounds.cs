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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MsgPack.Serialization
{
	/// <summary>
	///		Defines helper methods to avoid AOT issue of Unity AOT engine in unit testing.
	/// </summary>
	internal static class AotWorkarounds
	{
		public static void SetWorkaround( SerializationContext context )
		{
			context.Serializers.Register(
				new KeyValuePairOfStringAndDateTimeOffsetSerializer( context )
			);
		}
	}

	public class KeyValuePairOfStringAndDateTimeOffsetSerializer : MessagePackSerializer<KeyValuePair<string, DateTimeOffset>>
	{
		private readonly MessagePackSerializer<string> _keySerializer;
		private readonly MessagePackSerializer<DateTimeOffset> _valueSerializer;

		public KeyValuePairOfStringAndDateTimeOffsetSerializer( SerializationContext context )
			: base( context )
		{
			this._keySerializer = context.GetSerializer<string>();
			this._valueSerializer = context.GetSerializer<DateTimeOffset>();
		}


		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Asserted internally" )]
		protected internal override void PackToCore( Packer packer, KeyValuePair<string, DateTimeOffset> objectTree )
		{
			packer.PackArrayHeader( 2 );
			this._keySerializer.PackTo( packer, objectTree.Key );
			this._valueSerializer.PackTo( packer, objectTree.Value );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "Asserted internally" )]
		protected internal override KeyValuePair<string, DateTimeOffset> UnpackFromCore( Unpacker unpacker )
		{
			if ( !unpacker.Read() )
			{
				throw SerializationExceptions.NewUnexpectedEndOfStream();
			}

			var key =
				unpacker.LastReadData.IsNil ? null : this._keySerializer.UnpackFrom( unpacker );

			if ( !unpacker.Read() )
			{
				throw SerializationExceptions.NewUnexpectedEndOfStream();
			}


			var value = unpacker.LastReadData.IsNil ? default( DateTimeOffset ) : this._valueSerializer.UnpackFrom( unpacker );
			return new KeyValuePair<string, DateTimeOffset>( key, value );
		}
	}


}
