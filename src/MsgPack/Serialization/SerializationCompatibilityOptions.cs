#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2012 FUJIWARA, Yusuke
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
	///		Represents compatibility options of serialization runtime.
	/// </summary>
	public sealed class SerializationCompatibilityOptions
	{
		/// <summary>
		///		Gets or sets a value indicating whether <c>System.Runtime.Serialization.DataMemberAttribute.Order</c> should be started with 1 instead of 0.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if <c>System.Runtime.Serialization.DataMemberAttribute.Order</c> should be started with 1 instead of 0; otherwise, <c>false</c>.
		/// 	Default is <c>false</c>.
		/// </value>
		/// <remarks>
		///		Using this value, you can switch between MessagePack for CLI and ProtoBuf.NET seamlessly.
		/// </remarks>
		public bool OneBoundDataMemberOrder
		{
			get;
			set;
		}

		private PackerCompatibilityOptions _packerCompatibilityOptions;

		/// <summary>
		///		Gets or sets the <see cref="PackerCompatibilityOptions"/>.
		/// </summary>
		/// <value>
		///		The <see cref="PackerCompatibilityOptions"/>. The default is <see cref="F:PackerCompatibilityOptions.Classic"/>.
		/// </value>
		/// <remarks>
		///		<note>
		///			Changing this property value does not affect already built serializers -- especially built-in (default) serializers.
		///			You must specify <see cref="T:PackerCompatibilityOptions"/> enumeration to the constructor of <see cref="SerializationContext"/> to
		///			change built-in serializers' behavior.
		///		</note>
		/// </remarks>
		public PackerCompatibilityOptions PackerCompatibilityOptions
		{
			get { return this._packerCompatibilityOptions; }
			set { this._packerCompatibilityOptions = value; }
		}

		internal SerializationCompatibilityOptions()
		{
			this._packerCompatibilityOptions = PackerCompatibilityOptions.Classic;
		}
	}
}
