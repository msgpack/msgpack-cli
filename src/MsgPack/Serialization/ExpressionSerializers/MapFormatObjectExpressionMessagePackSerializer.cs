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

using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MsgPack.Serialization.ExpressionSerializers
{
	/// <summary>
	///		<see cref="ObjectExpressionMessagePackSerializer{T}"/> for map format stream.
	/// </summary>
	/// <typeparam name="T">The type of target type.</typeparam>
	internal class MapFormatObjectExpressionMessagePackSerializer<T> : ObjectExpressionMessagePackSerializer<T>
	{
		public MapFormatObjectExpressionMessagePackSerializer( SerializationContext context, SerializingMember[] members )
			: base( context, members ) { }

		protected override void PackToCoreOverride( Packer packer, T objectTree )
		{
			packer.PackMapHeader( this.MemberSerializers.Length );

			for ( int i = 0; i < this.MemberSerializers.Length; i++ )
			{
				if( this.MemberNames[i]==null )
				{
					// Skip missing member.
					continue;
				}

				packer.PackString( this.MemberNames[ i ] );
				this.MemberSerializers[ i ].PackTo( packer, this.MemberGetters[ i ]( objectTree ) );
			}
		}
	}
}
