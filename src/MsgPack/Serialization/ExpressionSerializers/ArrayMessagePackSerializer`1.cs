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

namespace MsgPack.Serialization.ExpressionSerializers
{
	/// <summary>
	///		<see cref="SequenceExpressionMessagePackSerializer{T}"/> for an array.
	/// </summary>
	/// <typeparam name="T">The type of element.</typeparam>
	internal sealed class ArrayExpressionMessagePackSerializer<T> : SequenceExpressionMessagePackSerializer<T>
	{
		public ArrayExpressionMessagePackSerializer( SerializationContext context, CollectionTraits traits ) : base( context, traits ) { }

		protected internal override T UnpackFromCore( Unpacker unpacker )
		{
			if ( !unpacker.IsArrayHeader )
			{
				throw SerializationExceptions.NewIsNotArrayHeader();
			}

			var instance = ( T )( object )Array.CreateInstance( this.Traits.ElementType, UnpackHelpers.GetItemsCount( unpacker ) );
			this.UnpackToCore( unpacker, instance );
			return instance;
		}
	}
}