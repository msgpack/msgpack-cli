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
	///		Implements common features expression tree based serializer builders.
	/// </summary>
	/// <typeparam name="TObject">The type of the serialization target.</typeparam>
	internal class ExpressionSerializerBuilder<TObject> : SerializerBuilder<TObject>
	{
		public ExpressionSerializerBuilder( SerializationContext context ) : base( context ) { }

		protected override MessagePackSerializer<TObject> CreateSerializer( SerializingMember[] entries )
		{
			if ( this.Context.SerializationMethod == SerializationMethod.Array )
			{
				return new ArrayFormatObjectExpressionMessagePackSerializer<TObject>( this.Context, entries );
			}
			else
			{
				return new MapFormatObjectExpressionMessagePackSerializer<TObject>( this.Context, entries );
			}
		}

		public override MessagePackSerializer<TObject> CreateArraySerializer()
		{
			var traits = typeof( TObject ).GetCollectionTraits();
			if ( typeof( TObject ).IsArray )
			{
				return new ArrayExpressionMessagePackSerializer<TObject>( this.Context, traits );
			}
			else
			{
				return new ListExpressionMessagePackSerializer<TObject>( this.Context, traits );
			}
		}

		public override MessagePackSerializer<TObject> CreateMapSerializer()
		{
			return new MapExpressionMessagePackSerializer<TObject>( this.Context, typeof( TObject ).GetCollectionTraits() );
		}

		public override MessagePackSerializer<TObject> CreateTupleSerializer()
		{
#if WINDOWS_PHONE
			throw new PlatformNotSupportedException();
#else
			return new TupleExpressionMessagePackSerializer<TObject>( this.Context );
#endif
		}
	}
}