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
	public class TypeWithMissingMessagePackMemberAttributeMember : IVerifiable
	{
		[MessagePackMember( 0 )]
		public int Field0 = 0;
		public int Field1 = 1;
		[MessagePackMember( 1 )]
		public int Field2 = 2;

		public void Verify( Stream stream )
		{
			stream.Position = 0;
			var data = Unpacking.UnpackObject( stream );
			NUnit.Framework.Assert.That( data, Is.Not.Null );
			if ( data.IsDictionary )
			{
				var map = data.AsDictionary();
				NUnit.Framework.Assert.That( map.ContainsKey( "Field0" ) );
				NUnit.Framework.Assert.That( map[ "Field0" ].Equals( this.Field0 ) );
				NUnit.Framework.Assert.That( map.ContainsKey( "Field2" ) );
				NUnit.Framework.Assert.That( map[ "Field2" ].Equals( this.Field2 ) );
			}
			else
			{
				var array = data.AsList();
				NUnit.Framework.Assert.That( array[ 0 ].Equals( this.Field0 ) );
				NUnit.Framework.Assert.That( array[ 1 ].Equals( this.Field2 ) );
			}
		}
	}
}
