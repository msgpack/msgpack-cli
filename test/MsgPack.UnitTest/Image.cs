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

//    This file is copy of MessagePack for Java.
#endregion -- License Terms --

using System;
using System.Collections.Generic;
using System.Globalization;

namespace MsgPack
{
	public class Image : IPackable, IUnpackable
	{
		public String uri = "";
		public String title = "";
		public int width = 0;
		public int height = 0;
		public int size = 0;

		public void PackToMessage( Packer pk, PackingOptions options )
		{
			pk.PackArrayHeader( 5 );
			pk.PackString( uri );
			pk.PackString( title );
			pk.Pack( width );
			pk.Pack( height );
			pk.Pack( size );
		}

		public void UnpackFromMessage( Unpacker unpacker )
		{
			if ( !unpacker.IsArrayHeader || unpacker.LastReadData.AsInt32() != 5 )
			{
				throw new ArgumentException( "Must be 5 element array." );
			}

			var asList = unpacker.UnpackSubtree().Value.AsList();

			uri = asList[ 0 ].AsString();
			title = asList[ 1 ].AsString();
			width = asList[ 2 ].AsInt32();
			height = asList[ 3 ].AsInt32();
			size = asList[ 4 ].AsInt32();
		}

		public override bool Equals( Object obj )
		{
			if ( !( obj is Image ) )
			{
				return false;
			}
			return Equals( obj as Image );
		}

		public bool Equals( Image obj )
		{
			return
				uri == obj.uri &&
				title == obj.title &&
				width == obj.width &&
				height == obj.height &&
				size == obj.size;
		}

		public override int GetHashCode()
		{
			return
				( this.uri == null ? 0 : this.uri.GetHashCode() )
				^ ( this.title == null ? 0 : this.title.GetHashCode() )
				^ this.width.GetHashCode()
				^ this.height.GetHashCode()
				^ this.size.GetHashCode();
		}

		public override string ToString()
		{
			return
				String.Format(
					CultureInfo.InvariantCulture,
					"uri: {0}, title: {1}, width: {2}, height: {3}, size: {4}",
					this.uri,
					this.title,
					this.width,
					this.height,
					this.size
				);
		}
	}

}
