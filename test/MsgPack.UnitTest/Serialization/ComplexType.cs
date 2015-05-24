#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2015 FUJIWARA, Yusuke
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
#if !MSTEST
using NUnit.Framework;
#else
using TestFixtureAttribute = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestClassAttribute;
using TestAttribute = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestMethodAttribute;
using TimeoutAttribute = NUnit.Framework.TimeoutAttribute;
using Assert = NUnit.Framework.Assert;
using Is = NUnit.Framework.Is;
#endif

namespace MsgPack.Serialization
{
	public class ComplexType : IVerifiable
	{
		[MessagePackMember( 0 )]
		public Uri Source { get; set; }
		[MessagePackMember( 2 )]
		public DateTime TimeStamp { get; set; }
		[MessagePackMember( 1 )]
		public byte[] Data { get; set; }

		private readonly Dictionary<DateTime, string> _history;

		[MessagePackMember( 3 )]
		public Dictionary<DateTime, string> History
		{
			get { return this._history; }
		}

		// Issue #62
		[MessagePackMember( 4 )]
		public List<int> Points
		{
			get;
			private set;
		}

		public ComplexType()
		{
			this._history = new Dictionary<DateTime, string>( DictionaryTestHelper.GetEqualityComparer<DateTime>() );
			this.Points = new List<int>();
		}

		public void Verify( Stream stream )
		{
			stream.Position = 0;
			var data = Unpacking.UnpackObject( stream );
			NUnit.Framework.Assert.That( data, Is.Not.Null );
			if ( data.IsDictionary )
			{
				var map = data.AsDictionary();
				NUnit.Framework.Assert.That( map.ContainsKey( "Source" ) );
				NUnit.Framework.Assert.That( this.Source, Is.Not.Null );
				NUnit.Framework.Assert.That( map[ "Source" ].AsString(), Is.EqualTo( this.Source.ToString() ) );
				NUnit.Framework.Assert.That( map.ContainsKey( "TimeStamp" ) );
				NUnit.Framework.Assert.That( DateTime.FromBinary( map[ "TimeStamp" ].AsInt64() ), Is.EqualTo( this.TimeStamp ) );
				NUnit.Framework.Assert.That( map.ContainsKey( "Data" ) );
				NUnit.Framework.Assert.That( map[ "Data" ].AsBinary(), Is.EqualTo( this.Data ) );
				NUnit.Framework.Assert.That( map.ContainsKey( "History" ) );
				NUnit.Framework.Assert.That( map[ "History" ].AsDictionary().Count, Is.EqualTo( this.History.Count ) );
				NUnit.Framework.Assert.That( map.ContainsKey( "Points" ) );
				NUnit.Framework.Assert.That( map[ "Points" ].AsList().Count, Is.EqualTo( this.Points.Count ) );
			}
			else
			{
				var array = data.AsList();
				NUnit.Framework.Assert.That( this.Source, Is.Not.Null );
				NUnit.Framework.Assert.That( array[ 0 ].AsString(), Is.EqualTo( this.Source.ToString() ) );
				NUnit.Framework.Assert.That( DateTime.FromBinary( array[ 2 ].AsInt64() ), Is.EqualTo( this.TimeStamp ) );
				NUnit.Framework.Assert.That( array[ 1 ].AsBinary(), Is.EqualTo( this.Data ) );
				NUnit.Framework.Assert.That( array[ 3 ].AsDictionary().Count, Is.EqualTo( this.History.Count ) );
				NUnit.Framework.Assert.That( array[ 4 ].AsList().Count, Is.EqualTo( this.Points.Count ) );
			}
		}
	}
}