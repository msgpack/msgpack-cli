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
// Contributors:
//    Takeshi KIRIYA
//
#endregion -- License Terms --

using System;
using System.Linq;

namespace MsgPack.Serialization.AbstractSerializers
{
	partial class SerializerBuilder<TContext, TConstruct, TObject>
	{
		private void BuildNullableSerializer( TContext context, Type underlyingType )
		{
			this.BuildNullablePackTo( context, underlyingType );
			this.BuildNullableUnpackFrom( context, underlyingType );
		}

		private void BuildNullablePackTo( TContext context, Type underlyingType )
		{
			// null was handled in PackTo() method.
			/*
			 * 	this._valueSerializer.PackToCore( packer, objectTree.Value );
			 */

			this.EmitMethodPrologue( context, SerializerMethod.PackToCore );
			TConstruct construct = null;
			try
			{
				construct =
					this.EmitSerializeItemExpressionCore(
						context,
						context.Packer,
						underlyingType,
						this.EmitGetProperty( context, context.PackToTarget, typeof( TObject ).GetProperty( "Value" ), false ),
						null,
						null
					);
			}
			finally
			{
				this.EmitMethodEpilogue( context, SerializerMethod.PackToCore, construct );
			}
		}

		private void BuildNullableUnpackFrom( TContext context, Type underlyingType )
		{
			// nil was handled in UnpackFrom() method.
			/*
			 *	return this._valueSerializer.UnpackFromCore( unpacker );
			 */

			this.EmitMethodPrologue( context, SerializerMethod.UnpackFromCore );
			TConstruct construct = null;
			try
			{
				var result = this.DeclareLocal( context, typeof( TObject ), "result" );
				construct =
					this.EmitRetrunStatement(
						context,
						this.EmitCreateNewObjectExpression(
							context,
							result,
							typeof( TObject ).GetConstructors().Single( c => c.GetParameters().Length == 1 ),
							this.EmitDeserializeItemExpressionCore(
								context,
								context.Unpacker,
								underlyingType,
								null,
								null
							)
						)
					);
			}
			finally
			{
				this.EmitMethodEpilogue( context, SerializerMethod.UnpackFromCore, construct );
			}
		}
	}
}
