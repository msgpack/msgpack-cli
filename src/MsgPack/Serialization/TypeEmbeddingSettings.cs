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
	///		Holds various settings related to .NET type embedding.
	/// </summary>
	public sealed class TypeEmbeddingSettings
	{
		/// <summary>
		///		The default type code for token which indicates the enclosing array holds complex object with encoded .NET type information.
		/// </summary>
		/// <remarks>
		///		Currently, this value is <c>127</c>(<c>0x7F</c>).
		/// </remarks>
		public const byte DefaultTypeEmbeddingIdentifier = 127;

		/// <summary>
		///		Gets or sets the type code for token which indicates the enclosing array holds complex object with encoded .NET type information.
		/// </summary>
		/// <value>
		///		The type code for the token. Default is <see cref="DefaultTypeEmbeddingIdentifier"/>.
		/// </value>
		public byte TypeEmbeddingIdentifier { get; set; }

		internal TypeEmbeddingSettings() { }
	}
}