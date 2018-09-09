#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2018 FUJIWARA, Yusuke and contributors
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
// Contributors:
//    Samuel Cragg
//
#endregion -- License Terms --

namespace MsgPack.Serialization
{
	/// <summary>
	///		Represents compatibility level.
	/// </summary>
	public enum SerializationCompatibilityLevel
	{
		/// <summary>
		///		Use latest feature. Almost backward compatible, but some compatibities are broken.
		/// </summary>
		Latest = 0,

		/// <summary>
		///		Compatible for version 0.5.x or former.
		/// </summary>
		Version0_5,

		/// <summary>
		///		Compatible for version 0.6.x, 0.7.x, 0.8.x, and 0.9.x.
		/// </summary>
		Version0_9
	}
}
