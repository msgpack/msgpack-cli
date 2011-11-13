#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010 FUJIWARA, Yusuke
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
using System.Runtime.Serialization;
using System.Xml;

namespace MsgPack.Serialization
{
// TODO: IMPL in the future...
	/// <summary>
	///		Allows MessagePack is used from WCF.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class DataContractMessagePackSerializer<T> : XmlObjectSerializer
	{
		/*
		 * Strategy
		 * XMLInfoSet -> MessagePackObject tree
		 * Defines MessagePackObjectTreeReader : XmlDictionaryReader, and MessagePackObjectTreeWriter : XmlDictionaryWriter
		 */

		public override bool IsStartObject( XmlDictionaryReader reader )
		{
			throw new NotImplementedException();
		}

		public override object ReadObject( XmlDictionaryReader reader, bool verifyObjectName )
		{
			throw new NotImplementedException();
		}

		public override void WriteEndObject( XmlDictionaryWriter writer )
		{
			throw new NotImplementedException();
		}

		public sealed override void WriteObjectContent( XmlDictionaryWriter writer, object graph )
		{
			throw new NotImplementedException();
		}

		public sealed override void WriteStartObject( XmlDictionaryWriter writer, object graph )
		{
			throw new NotImplementedException();
		}
	}
}
