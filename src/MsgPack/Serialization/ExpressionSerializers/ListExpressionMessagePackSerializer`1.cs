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
using System.Linq.Expressions;

namespace MsgPack.Serialization.ExpressionSerializers
{
	/// <summary>
	///		<see cref="SequenceExpressionMessagePackSerializer{T}"/> for a collection which has <c>Add</c> method.
	/// </summary>
	/// <typeparam name="T">The type of element.</typeparam>
	internal sealed class ListExpressionMessagePackSerializer<T> : SequenceExpressionMessagePackSerializer<T>
	{
		private readonly Func<int, T> _createInstanceWithCapacity;
		private readonly Func<T> _createInstance;

		public ListExpressionMessagePackSerializer( SerializationContext context, CollectionTraits traits )
			: base( context, traits )
		{
			Type type = typeof( T );
			if( type.GetIsAbstract() )
			{
				type = context.DefaultCollectionTypes.GetConcreteType( typeof( T ) ) ?? type;
			}

			if ( type.IsArray )
			{
				var capacityParameter = Expression.Parameter( typeof( int ), "length" );
				this._createInstanceWithCapacity =
					Expression.Lambda<Func<int, T>>(
							Expression.NewArrayBounds( type.GetElementType(), capacityParameter ),
							capacityParameter
						).Compile();
				this._createInstance = null;
			}
			else if ( type.GetIsAbstract() )
			{
				this._createInstance = () => { throw SerializationExceptions.NewNotSupportedBecauseCannotInstanciateAbstractType( type ); };
				this._createInstanceWithCapacity = null;
			}
			else
			{
				var constructor = ExpressionSerializerLogics.GetCollectionConstructor( context, type );
				if ( constructor == null )
				{
					this._createInstance = () => { throw SerializationExceptions.NewTargetDoesNotHavePublicDefaultConstructorNorInitialCapacity( type ); };
					this._createInstanceWithCapacity = null;
				}
				else
				{
					if ( constructor.GetParameters().Length == 1 )
					{
						this._createInstance = null;

						var capacityParameter = Expression.Parameter( typeof( int ), "parameter" );
						this._createInstanceWithCapacity =
							Expression.Lambda<Func<int, T>>(
								Expression.New( constructor, capacityParameter ),
								capacityParameter
							).Compile();
					}
					else
					{
						this._createInstanceWithCapacity = null;
						this._createInstance =
							Expression.Lambda<Func<T>>(
								Expression.New( constructor )
							).Compile();
					}
				}
			}
		}

		protected internal override T UnpackFromCore( Unpacker unpacker )
		{
			if ( !unpacker.IsArrayHeader )
			{
				throw SerializationExceptions.NewIsNotArrayHeader();
			}

			var instance = this._createInstanceWithCapacity == null ? this._createInstance() : this._createInstanceWithCapacity( UnpackHelpers.GetItemsCount( unpacker ) );
			this.UnpackToCore( unpacker, instance );
			return instance;
		}
	}
}
