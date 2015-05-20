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

namespace MsgPack.Serialization
{
	/// <summary>
	///		Defines behavior of built-in serializers to conversion of <see cref="DateTime"/> value.
	/// </summary>
	public enum DateTimeConversionMethod
	{
		/// <summary>
		///		Uses <see cref="DateTime.Ticks"/> context, that is, Gregorian 0000-01-01 based, 100 nano seconds resolution. This value also preserves <see cref="DateTimeKind"/>.
		/// </summary>
		/// <remarks>
		///		As of 0.6, this value has been become default. This option prevents accidental data loss.
		/// </remarks>
		Native = 0,

		/// <summary>
		///		Uses Unix epoc context, that is, Gregirian 1970-01-01 based, milliseconds resolution.
		/// </summary>
		/// <remarks>
		///		Many binding such as Java uses this resolution, so this option gives maximom interoperability.
		/// </remarks>
		UnixEpoc = 1
	}
}