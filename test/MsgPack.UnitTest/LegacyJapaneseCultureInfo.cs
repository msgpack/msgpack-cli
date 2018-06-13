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

namespace MsgPack
{
	/// <summary>
	///     Custom <see cref="CultureInfo" /> which uses full width hiphen for negative sign.!--
	/// </summary>
#if UNITY // Enabled by copy script for Unity
	[Serializable]
#endif // UNITY
	internal sealed class LegacyJapaneseCultureInfo : CultureInfo
	{
		public LegacyJapaneseCultureInfo()
#if NETSTANDARD1_1 || NETSTANDARD1_3 || SILVERLIGHT
			: base( "ja-JP" )
#else
			: base( "ja-JP", true )
#endif // NETSTANDARD1_1 || NETSTANDARD1_3 || SILVERLIGHT
		{
			var numberFormatInfo = CultureInfo.InvariantCulture.NumberFormat.Clone() as NumberFormatInfo;
			numberFormatInfo.NegativeSign = "\uFF0D"; // Full width hiphen
			this.NumberFormat = NumberFormatInfo.ReadOnly( numberFormatInfo );
		}
	}
}
