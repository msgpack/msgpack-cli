#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2017 FUJIWARA, Yusuke
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

#if CORE_CLR || UNITY || NETSTANDARD1_1
using Contract = MsgPack.MPContract;
#else
#endif // CORE_CLR || UNITY || NETSTANDARD1_1
using System.Text;
#if FEATURE_TAP
#endif // FEATURE_TAP

namespace MsgPack
{
	/// <summary>
	///		Common encoding extensions for streaming encoding/decoding.
	/// </summary>
	internal static class EncodingExtensions
	{
		public static /* unsafe */ bool EncodeString( this Encoder source, char[]/* char* */ chars, ref int charsOffset, ref int charsLength, byte[] /* byte* */ buffer, int bufferOffset, int bufferCount, out int bytesUsed )
		{
			bool isCompleted;
			int charsUsed;
			source.Convert(
				chars,
				charsOffset,
				charsLength,
				buffer,
				bufferOffset,
				bufferCount,
				false,
				out charsUsed,
				out bytesUsed,
				out isCompleted
			);

			charsOffset += charsUsed;
			charsLength -= charsUsed;

			return isCompleted;
		}

		public static bool DecodeString( this Decoder source, byte[] bytes, int bytesOffset, int bytesLength, char[] buffer, StringBuilder result )
		{
			bool isCompleted;

			// Loop
			do
			{
				int bytesUsed;
				int charsUsed;
				source.Convert(
					bytes,
					bytesOffset,
					bytesLength,
					buffer,
					0,
					buffer.Length,
					false,
					out bytesUsed,
					out charsUsed,
					out isCompleted
				);

				result.Append( buffer, 0, charsUsed );
				bytesOffset += bytesUsed;
				bytesLength -= bytesUsed;
			} while ( bytesLength > 0 );

			return isCompleted;
		}
	}
}
