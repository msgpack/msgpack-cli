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
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;

namespace MsgPack.Serialization
{
	public class ComplexType : IVerifiable
	{
		public Uri Source { get; set; }
		public DateTime TimeStamp { get; set; }
		public byte[] Data { get; set; }

		private readonly Dictionary<DateTime, string> _history = new Dictionary<DateTime, string>();

		public Dictionary<DateTime, string> History
		{
			get { return this._history; }
		}

		public void Assert( Stream stream )
		{
			stream.Position = 0;
			var data = Unpacking.UnpackObject( stream );
			NUnit.Framework.Assert.That( data, Is.Not.Null );
			var map = data.Value.AsDictionary();
			NUnit.Framework.Assert.That( map.ContainsKey( "Source" ) );
			NUnit.Framework.Assert.That( this.Source, Is.Not.Null );
			NUnit.Framework.Assert.That( map[ "Source" ].AsString(), Is.EqualTo( this.Source.ToString() ) );
			NUnit.Framework.Assert.That( map.ContainsKey( "TimeStamp" ) );
			NUnit.Framework.Assert.That( MessagePackConvert.ToDateTime( map[ "TimeStamp" ].AsInt64() ), Is.EqualTo( this.TimeStamp ) );
			NUnit.Framework.Assert.That( map.ContainsKey( "Data" ) );
			NUnit.Framework.Assert.That( map[ "Data" ].AsBinary(), Is.EqualTo( this.Data ) );
			NUnit.Framework.Assert.That( map.ContainsKey( "History" ) );
			NUnit.Framework.Assert.That( map[ "History" ].AsDictionary().Count, Is.EqualTo( this.History.Count ) );
		}
	}
}
