#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2013 FUJIWARA, Yusuke
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
	///		Defines internal common interface for serializer generator configuration objects.
	/// </summary>
	internal interface ISerializerGeneratorConfiguration
	{
		/// <summary>
		///		Gets or sets the output directory for generating artifacts.
		/// </summary>
		/// <value>
		///		The output directory for generating artifacts.
		///		Specifying <c>null</c> causes reset to the default location.
		/// </value>
		/// <exception cref="ArgumentException">Specified value is not valid for directory path.</exception>
		string OutputDirectory { get; set; }

		/// <summary>
		///		Gets or sets the serialization method to pack object.
		/// </summary>
		/// <value>
		///		A value of <see cref="SerializationMethod"/>.
		/// </value>
		/// <exception cref="ArgumentOutOfRangeException">Specified value is not valid  <see cref="SerializationMethod"/>.</exception>
		SerializationMethod SerializationMethod { get; set; }

		/// <summary>
		///		Validates this instance state.
		/// </summary>
		/// <exception cref="InvalidOperationException">This object is not in valid state.</exception>
		void Validate();
	}
}