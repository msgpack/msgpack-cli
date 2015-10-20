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

#if UNITY_5 || UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_WII || UNITY_IPHONE || UNITY_ANDROID || UNITY_PS3 || UNITY_XBOX360 || UNITY_FLASH || UNITY_BKACKBERRY || UNITY_WINRT
#define UNITY
#endif

using System;
#if !UNITY || MSGPACK_UNITY_FULL
using System.ComponentModel;
#endif // !UNITY || MSGPACK_UNITY_FULL
#if !SILVERLIGHT || WINDOWS_PHONE
using System.Runtime.InteropServices.ComTypes;
#endif // !SILVERLIGHT || WINDOWS_PHONE

namespace MsgPack.Serialization
{
	/// <summary>
	///		<strong>This is intened to MsgPack for CLI internal use. Do not use this type from application directly.</strong>
	///		Helper methods for date time message pack serializer.
	/// </summary>
#if !UNITY || MSGPACK_UNITY_FULL
	[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
	public static class DateTimeMessagePackSerializerHelpers
	{
		/// <summary>
		///		Determines <see cref="DateTimeConversionMethod"/> for the target.
		/// </summary>
		/// <param name="context">Context information.</param>
		/// <param name="dateTimeMemberConversionMethod">The method argued by the member.</param>
		/// <returns>Determined <see cref="DateTimeConversionMethod"/> for the target.</returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="context"/> is <c>null</c>.
		/// </exception>
#if !UNITY || MSGPACK_UNITY_FULL
		[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
		public static DateTimeConversionMethod DetermineDateTimeConversionMethod(
			SerializationContext context,
			DateTimeMemberConversionMethod dateTimeMemberConversionMethod 
		)
		{
			if ( context == null )
			{
				throw new ArgumentNullException( "context" );
			}

			switch ( dateTimeMemberConversionMethod )
			{
				case DateTimeMemberConversionMethod.Native:
				{
					return DateTimeConversionMethod.Native;
				}
				case DateTimeMemberConversionMethod.UnixEpoc:
				{
					return DateTimeConversionMethod.UnixEpoc;
				}
				default:
				{
					return context.DefaultDateTimeConversionMethod;
				}
			}
		}

		internal static bool IsDateTime( Type dateTimeType )
		{
			return
				dateTimeType == typeof( DateTime )
				|| dateTimeType == typeof( DateTime? )
#if ( !SILVERLIGHT || WINDOWS_PHONE ) && !XAMIOS && !XAMDROID && !UNITY
				|| dateTimeType == typeof( FILETIME )
				|| dateTimeType == typeof( FILETIME? )
#endif // ( !SILVERLIGHT || WINDOWS_PHONE ) && !XAMIOS && !XAMDROID && !UNITY
				// DateTimeOffset? is not have to be treat specially.
				|| dateTimeType == typeof( DateTimeOffset );
		}
	}
}