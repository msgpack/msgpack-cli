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
using System.Globalization;

namespace MsgPack.Serialization.DefaultSerializers
{
	/// <summary>
	///		Provides runtime selection ability for <see cref="DateTime"/> serialization.
	/// </summary>
	internal class DateTimeMessagePackSerializerProvider : MessagePackSerializerProvider
	{
		private readonly IMessagePackSingleObjectSerializer _unixEpoc;
		private readonly IMessagePackSingleObjectSerializer _native;

		public DateTimeMessagePackSerializerProvider( SerializationContext context, bool isNullable )
		{
			if ( isNullable )
			{
#if !UNITY
				this._unixEpoc =
					new NullableMessagePackSerializer<DateTime>( context, new UnixEpocDateTimeMessagePackSerializer( context ) );
				this._native =
					new NullableMessagePackSerializer<DateTime>( context, new NativeDateTimeMessagePackSerializer( context ) );
#else
				this._unixEpoc =
					new NullableMessagePackSerializer( context, typeof( DateTime? ), new UnixEpocDateTimeMessagePackSerializer( context ) );
				this._native =
					new NullableMessagePackSerializer( context, typeof( DateTime? ), new NativeDateTimeMessagePackSerializer( context ) );
#endif // !UNITY
			}
			else
			{
				this._unixEpoc = new UnixEpocDateTimeMessagePackSerializer( context );
				this._native = new NativeDateTimeMessagePackSerializer( context );
			}
		}

		public override object Get( SerializationContext context, object providerParameter )
		{
			if ( providerParameter is DateTimeConversionMethod )
			{
				switch ( ( DateTimeConversionMethod )providerParameter )
				{
					case DateTimeConversionMethod.Native:
					{
						return this._native;
					}
					case DateTimeConversionMethod.UnixEpoc:
					{
						return this._unixEpoc;
					}
				}
			}

			switch ( context.DefaultDateTimeConversionMethod )
			{
				case DateTimeConversionMethod.Native:
				{
					return this._native;
				}
				case DateTimeConversionMethod.UnixEpoc:
				{
					return this._unixEpoc;
				}
				default:
				{
					throw new NotSupportedException(
						String.Format(
							CultureInfo.CurrentCulture,
							"Unknown {0} value '{1:G}'({1:D})",
							typeof( DateTimeConversionMethod ),
							context.DefaultDateTimeConversionMethod
						)
					);
				}
			}
		}
	}
}