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
using System.Collections.Generic;
#if FEATURE_TAP
using System.Threading;
#endif // FEATURE_TAP

namespace MsgPack.Serialization.AbstractSerializers
{
	internal class DynamicUnpackingContext
	{
		private readonly Dictionary<string, object> _bag;

		public object Get( string key )
		{
			object result;
			this._bag.TryGetValue( key, out result );
			return result;
		}

		public void Set( string key, object value )
		{
			this._bag[ key ] = value;
		}

#if FEATURE_TAP

		public CancellationToken CancellationToken
		{
			get; 
			private set;
		}

#endif // FEATURE_TAP

		public DynamicUnpackingContext( int capacity )
		{
			this._bag = new Dictionary<string, object>( capacity );
		}

#if FEATURE_TAP
		public DynamicUnpackingContext( int capacity, CancellationToken cancellationToken )
			:this( capacity )
		{
			this.CancellationToken = cancellationToken;
		}
#endif // FEATURE_TAP
	}
}