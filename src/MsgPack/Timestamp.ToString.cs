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

using System;
using System.Globalization;
using System.Text;

namespace MsgPack
{
	partial struct Timestamp : IFormattable
	{
		/// <summary>
		///		Returns a <see cref="String"/> representation of this instance with the default format and the default format provider.
		/// </summary>
		/// <returns>
		///		A <see cref="String"/> representation of this instance.
		/// </returns>
		/// <remarks>
		///		<para>
		///			As of recommendation of the msgpack specification and consistency with <see cref="DateTime"/> and <see cref="DateTimeOffset"/>,
		///			this overload uses <c>"o"</c> for the <c>format</c> parameter and <c>null</c> for <c>formatProvider</c> parameter.
		///		</para>
		///		<para>
		///			The round trip format is <c>yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffffffff'Z'</c> which 'fffffffff' nanoseconds. 
		///		</para>
		/// </remarks>
		public override string ToString()
		{
			return this.ToString(null, null);
		}

		/// <summary>
		///		Returns a <see cref="String"/> representation of this instance with the default format and the specified format provider.
		/// </summary>
		/// <param name="formatProvider">
		///		An <see cref="IFormatProvider"/> to provide culture specific format information.
		///		You can specify <c>null</c> for default behavior, which uses <see cref="CultureInfo.CurrentCulture"/>.
		///	</param>
		/// <returns>
		///		A <see cref="String"/> representation of this instance.
		/// </returns>
		/// <remarks>
		///		<para>
		///			As of recommendation of the msgpack specification and consistency with <see cref="DateTime"/> and <see cref="DateTimeOffset"/>,
		///			this overload uses <c>"o"</c> for <c>format</c> parameter.
		///		</para>
		///		<para>
		///			The round trip format is <c>yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffffffff'Z'</c> which 'fffffffff' nanoseconds. 
		///		</para>
		/// </remarks>
		public string ToString(IFormatProvider formatProvider)
		{
			return this.ToString(null, formatProvider);
		}

		/// <summary>
		///		Returns a <see cref="String"/> representation of this instance with the specified format and the default format provider.
		/// </summary>
		/// <param name="format">
		///		A format string to specify output format. You can specify <c>null</c> for default behavior, which is interpreted as <c>"o"</c>.
		/// </param>
		/// <returns>
		///		A <see cref="String"/> representation of this instance.
		/// </returns>
		/// <remarks>
		///		<para>
		///			Currently, only <c>"o"</c> and <c>"O"</c> (ISO 8601 like round trip format) and <c>"s"</c> (ISO 8601 format) are supported.
		///			Other standard date time format and any custom date time format are not supported.
		///		</para>
		///		<para>
		///			The round trip format is <c>yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffffffff'Z'</c> which 'fffffffff' nanoseconds. 
		///		</para>
		///		<para>
		///			As of recommendation of the msgpack specification and consistency with <see cref="DateTime"/> and <see cref="DateTimeOffset"/>,
		///			this overload uses <c>null</c> for <c>formatProvider</c> parameter.
		///		</para>
		/// </remarks>
		public string ToString(string format)
		{
			return this.ToString(format, null);
		}

#warning TODO: TryFormat

		/// <summary>
		///		Returns a <see cref="String"/> representation of this instance with the default format and the specified format provider.
		/// </summary>
		/// <param name="format">
		///		A format string to specify output format. You can specify <c>null</c> for default behavior, which is interpreted as <c>"o"</c>.
		/// </param>
		/// <param name="formatProvider">
		///		An <see cref="IFormatProvider"/> to provide culture specific format information.
		///		You can specify <c>null</c> for default behavior, which uses <see cref="CultureInfo.CurrentCulture"/>.
		///	</param>
		/// <returns>
		///		A <see cref="String"/> representation of this instance.
		/// </returns>
		/// <exception cref="ArgumentException">
		///		<paramref name="format"/> is not valid.
		/// </exception>
		/// <remarks>
		///		<para>
		///			Currently, only <c>"o"</c> and <c>"O"</c> (ISO 8601 like round trip format) and <c>"s"</c> (ISO 8601 format) are supported.
		///			Other standard date time format and any custom date time format are not supported.
		///		</para>
		///		<para>
		///			The round trip format is <c>yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffffffff'Z'</c> which 'fffffffff' nanoseconds. 
		///		</para>
		///		<para>
		///			As of recommendation of the msgpack specification and consistency with <see cref="DateTime"/> and <see cref="DateTimeOffset"/>,
		///			the default value of the <paramref name="format"/> is <c>"o"</c> (ISO 8601 like round-trip format)
		///			and the default value of the <paramref name="formatProvider"/> is <c>null</c> (<see cref="CultureInfo.CurrentCulture"/>.
		///			If you want to ensure interoperability for other implementation, specify <c>"s"</c> and <see cref="CultureInfo.InvariantCulture"/> resepectively.
		///		</para>
		/// </remarks>
		public string ToString(string? format, IFormatProvider? formatProvider)
		{
			var value = new Value(this);
			return TimestampStringConverter.ToString(format, formatProvider, ref value);
		}
	}
}
