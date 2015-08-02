#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2014-2015 FUJIWARA, Yusuke
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
using System.Diagnostics.Contracts;

namespace MsgPack.Serialization.AbstractSerializers
{
	partial class SerializerBuilder<TContext, TConstruct, TObject>
	{
		protected void BuildEnumSerializer( TContext context )
		{
			Contract.Assert( typeof( TObject ).GetIsEnum() );
			var underlyingType = Enum.GetUnderlyingType( typeof( TObject ) );
			Contract.Assert( underlyingType != null, "Underlying type of " + typeof( TObject ) + " is null." );

			this.BuildPackUnderlyingValueTo( context, underlyingType );
			this.BuildUnpackFromUnderlyingValue( context, underlyingType );
		}

		private void BuildPackUnderlyingValueTo( TContext context, Type underlyingType )
		{
			this.EmitMethodPrologue(
				context,
				EnumSerializerMethod.PackUnderlyingValueTo
			);

			TConstruct construct = null;
			try
			{
				construct =
					this.EmitInvokeVoidMethod(
						context,
						this.ReferArgument( context, typeof( Packer ), "packer", 1 ),
						typeof( Packer ).GetMethod( "Pack", new[] { underlyingType } ),
						this.EmitEnumToUnderlyingCastExpression( context, underlyingType, this.ReferArgument( context, typeof( TObject ), "enumValue", 2 ) )
					);
			}
			finally
			{
				this.EmitMethodEpilogue(
					context,
					EnumSerializerMethod.PackUnderlyingValueTo,
					construct
				);
			}
		}

		private void BuildUnpackFromUnderlyingValue( TContext context, Type underlyingType )
		{
			this.EmitMethodPrologue(
				context,
				EnumSerializerMethod.UnpackFromUnderlyingValue
			);

			TConstruct construct = null;
			try
			{
				construct =
					this.EmitRetrunStatement(
						context,
						this.EmitEnumFromUnderlyingCastExpression(
							context,
							typeof( TObject ),
							this.EmitInvokeMethodExpression(
								context,
								this.ReferArgument( context, typeof( MessagePackObject ), "messagePackObject", 1 ),
								typeof( MessagePackObject ).GetMethod( "As" + underlyingType.Name, ReflectionAbstractions.EmptyTypes )
							)
						)
					);
			}
			finally
			{
				this.EmitMethodEpilogue(
					context,
					EnumSerializerMethod.UnpackFromUnderlyingValue,
					construct
				);
			}
		}

		protected abstract TConstruct EmitEnumToUnderlyingCastExpression( TContext context, Type underlyingType, TConstruct enumValue );

		protected abstract TConstruct EmitEnumFromUnderlyingCastExpression( TContext context, Type enumType, TConstruct underlyingValue );
	}
}