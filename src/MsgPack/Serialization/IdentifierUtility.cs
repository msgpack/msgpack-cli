#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2018 FUJIWARA, Yusuke
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
using System.Text;
using MsgPack.Serialization.Reflection;

namespace MsgPack.Serialization
{
	/// <summary>
	///		Utilities related to member/type ID.
	/// </summary>
#if UNITY && DEBUG
	public
#else
	internal
#endif
	static class IdentifierUtility
	{
		public static string EscapeTypeName( Type type )
		{
			return EscapeTypeName( type.GetFullName() );
		}

		public static string EscapeTypeName( string fullName )
		{
			var result = new StringBuilder( fullName.Length );
			bool mayArray = false;
			foreach ( var c in fullName )
			{
				switch ( c )
				{
					case ' ':
					{
						mayArray = false;
						break;
					}
					case ',':
					{
						mayArray = false;
						result.Append( '_' );
						break;
					}
					case '[':
					{
						mayArray = true;
						break;
					}
					case ']':
					{
						if ( mayArray )
						{
							result.Append( "Array" );
							mayArray = false;
						}
						else
						{
							result.Append( '_' );
						}

						break;
					}
					case '*':
					{
						if ( !mayArray )
						{
							result.Append( "Pointer" );
						}

						break;
					}
					case '&':
					{
						if ( mayArray )
						{
							result.Append( '_' );
						}

						mayArray = false;

						result.Append( "Reference" );
						break;
					}
					case '+':
					case '.':
					case '`':
					{
						mayArray = false;
						result.Append( '_' );
						break;
					}
					default:
					{
						if ( mayArray )
						{
							result.Append( '_' );
						}

						mayArray = false;
						result.Append( c );
						break;
					}
				}
			}

			return result.ToString();
		}
	}
}
