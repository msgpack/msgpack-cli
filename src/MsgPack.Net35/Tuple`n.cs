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
using System.Text;

namespace System
{
	// This file generated from Tuple.tt T4Template.
	// Do not modify this file. Edit Tuple.tt instead.

#if DEBUG
	internal static class Tuple
	{

		public static Tuple<T1, T2> Create<T1, T2>( T1 item1, T2 item2 )
		{
			return new Tuple<T1, T2>( item1, item2 );
		}

		public static Tuple<T1, T2, T3, T4> Create<T1, T2, T3, T4>( T1 item1, T2 item2, T3 item3, T4 item4 )
		{
			return new Tuple<T1, T2, T3, T4>( item1, item2, item3, item4 );
		}

		public static Tuple<T1, T2, T3, T4, T5> Create<T1, T2, T3, T4, T5>( T1 item1, T2 item2, T3 item3, T4 item4, T5 item5 )
		{
			return new Tuple<T1, T2, T3, T4, T5>( item1, item2, item3, item4, item5 );
		}
	}
	[Serializable]
	internal class Tuple<T1, T2>
	{
		private readonly T1 _item1;
		
		public T1 Item1
		{
			get { return this._item1; }
		}
		private readonly T2 _item2;
		
		public T2 Item2
		{
			get { return this._item2; }
		}
		public Tuple(
			T1 item1,
			T2 item2
		)
		{
			this._item1 = item1;
			this._item2 = item2;
		}
		
		public override string ToString()
		{
			var buffer = new StringBuilder();
			buffer.Append( '(' );
			buffer.Append( this._item1 );
			buffer.Append( ", " );
			buffer.Append( this._item2 );
			return buffer.ToString();
		}
	}
	[Serializable]
	internal class Tuple<T1, T2, T3, T4>
	{
		private readonly T1 _item1;
		
		public T1 Item1
		{
			get { return this._item1; }
		}
		private readonly T2 _item2;
		
		public T2 Item2
		{
			get { return this._item2; }
		}
		private readonly T3 _item3;
		
		public T3 Item3
		{
			get { return this._item3; }
		}
		private readonly T4 _item4;
		
		public T4 Item4
		{
			get { return this._item4; }
		}
		public Tuple(
			T1 item1,
			T2 item2,
			T3 item3,
			T4 item4
		)
		{
			this._item1 = item1;
			this._item2 = item2;
			this._item3 = item3;
			this._item4 = item4;
		}
		
		public override string ToString()
		{
			var buffer = new StringBuilder();
			buffer.Append( '(' );
			buffer.Append( this._item1 );
			buffer.Append( ", " );
			buffer.Append( this._item2 );
			buffer.Append( ", " );
			buffer.Append( this._item3 );
			buffer.Append( ", " );
			buffer.Append( this._item4 );
			return buffer.ToString();
		}
	}
	[Serializable]
	internal class Tuple<T1, T2, T3, T4, T5>
	{
		private readonly T1 _item1;
		
		public T1 Item1
		{
			get { return this._item1; }
		}
		private readonly T2 _item2;
		
		public T2 Item2
		{
			get { return this._item2; }
		}
		private readonly T3 _item3;
		
		public T3 Item3
		{
			get { return this._item3; }
		}
		private readonly T4 _item4;
		
		public T4 Item4
		{
			get { return this._item4; }
		}
		private readonly T5 _item5;
		
		public T5 Item5
		{
			get { return this._item5; }
		}
		public Tuple(
			T1 item1,
			T2 item2,
			T3 item3,
			T4 item4,
			T5 item5
		)
		{
			this._item1 = item1;
			this._item2 = item2;
			this._item3 = item3;
			this._item4 = item4;
			this._item5 = item5;
		}
		
		public override string ToString()
		{
			var buffer = new StringBuilder();
			buffer.Append( '(' );
			buffer.Append( this._item1 );
			buffer.Append( ", " );
			buffer.Append( this._item2 );
			buffer.Append( ", " );
			buffer.Append( this._item3 );
			buffer.Append( ", " );
			buffer.Append( this._item4 );
			buffer.Append( ", " );
			buffer.Append( this._item5 );
			return buffer.ToString();
		}
	}
#endif // DEBUG
}