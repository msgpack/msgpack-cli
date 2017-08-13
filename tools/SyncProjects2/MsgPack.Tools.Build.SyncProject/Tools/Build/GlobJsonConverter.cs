#region -- License Terms --
//  MessagePack for CLI
// 
//  Copyright (C) 2015 FUJIWARA, Yusuke
// 
//     Licensed under the Apache License, Version 2.0 (the "License");
//     you may not use this file except in compliance with the License.
//     You may obtain a copy of the License at
// 
//         http://www.apache.org/licenses/LICENSE-2.0
// 
//     Unless required by applicable law or agreed to in writing, software
//     distributed under the License is distributed on an "AS IS" BASIS,
//     WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//     See the License for the specific language governing permissions and
//     limitations under the License.
#endregion

using System;
using System.Diagnostics;
using Newtonsoft.Json;

namespace MsgPack.Tools.Build
{
	/// <summary>
	///		<see cref="JsonConverter"/> for <see cref="Glob"/> structure.
	/// </summary>
	internal sealed class GlobJsonConverter : JsonConverter
	{
		public GlobJsonConverter() { }

		public override void WriteJson( JsonWriter writer, object value, JsonSerializer serializer )
		{
			var target = ( Glob )value;

			writer.WriteStartObject();
			writer.WritePropertyName( "type" );
			writer.WriteValue( target.Type );
			writer.WritePropertyName( "path" );
			writer.WriteValue( target.Path );
			writer.WriteEndObject();
		}

		public override object ReadJson( JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer )
		{
			Debug.Assert( reader.TokenType == JsonToken.StartObject );
			var path = default( string );
			var type = default( GlobType? );
			var lastDepth = reader.Depth;

			while ( reader.Read() && reader.Depth > lastDepth )
			{
				Debug.Assert( reader.TokenType == JsonToken.PropertyName );
				switch ( reader.Value as string )
				{
					case "path":
					{
						path = reader.ReadAsString();
						break;
					}
					case "type":
					{
						type = ( GlobType )Enum.Parse( typeof( GlobType ), reader.ReadAsString(), ignoreCase: true );
						break;
					}
				}
			}

			if ( path == null || type == null )
			{
				throw new JsonSerializationException( "Both of 'path' and 'type' are required." );
			}

			return new Glob( path, type.Value );
		}

		public override bool CanConvert( Type objectType )
			=> objectType == typeof( Glob );
	}
}
