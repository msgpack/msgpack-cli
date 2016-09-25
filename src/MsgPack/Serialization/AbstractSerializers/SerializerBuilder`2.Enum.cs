#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2014-2016 FUJIWARA, Yusuke
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
#if CORE_CLR || NETSTANDARD1_1
using Contract = MsgPack.MPContract;
#else
using System.Diagnostics.Contracts;
#endif // CORE_CLR || NETSTANDARD1_1
#if FEATURE_TAP
using System.Threading;
#endif // FEATURE_TAP

namespace MsgPack.Serialization.AbstractSerializers
{
	partial class SerializerBuilder<TContext, TConstruct>
	{
		protected void BuildEnumSerializer( TContext context )
		{
#if DEBUG
			Contract.Assert( this.TargetType.GetIsEnum() );
#endif // DEBUG
			var underlyingType = Enum.GetUnderlyingType( this.TargetType );
#if DEBUG
			Contract.Assert( underlyingType != null, "Underlying type of " + this.TargetType + " is null." );
#endif // DEBUG

			this.BuildPackUnderlyingValueTo( context, underlyingType, false );
			this.BuildUnpackFromUnderlyingValue( context, underlyingType );

#if FEATURE_TAP
			if ( this.WithAsync( context ) )
			{
				this.BuildPackUnderlyingValueTo( context, underlyingType, true );
			}
#endif // FEATURE_TAP
		}

		private void BuildPackUnderlyingValueTo( TContext context, Type underlyingType, bool isAsync )
		{
			var methodName =
#if FEATURE_TAP
				 isAsync ? MethodName.PackUnderlyingValueToAsync :
#endif // FEATURE_TAP
				 MethodName.PackUnderlyingValueTo;

			context.BeginMethodOverride( methodName );

			var invocation =
#if FEATURE_TAP
				isAsync
					? this.EmitRetrunStatement(
						context,
						this.EmitInvokeMethodExpression(
							context,
							this.ReferArgument( context, TypeDefinition.PackerType, "packer", 1 ),
							typeof( Packer ).GetMethod( "PackAsync", new[] { underlyingType, typeof( CancellationToken) } ),
							this.EmitEnumToUnderlyingCastExpression( context, underlyingType, this.ReferArgument( context, this.TargetType, "enumValue", 2 ) ),
							this.ReferArgument( context,TypeDefinition.CancellationTokenType, "cancellationToken", 3 )
						)
					) :
#endif // FEATURE_TAP
					this.EmitInvokeVoidMethod(
						context,
						this.ReferArgument( context, TypeDefinition.PackerType, "packer", 1 ),
						typeof( Packer ).GetMethod( "Pack", new[] { underlyingType } ),
						this.EmitEnumToUnderlyingCastExpression( context, underlyingType, this.ReferArgument( context, this.TargetType, "enumValue", 2 ) )
					);

			context.EndMethodOverride( methodName, invocation);
		}

		private void BuildUnpackFromUnderlyingValue( TContext context, Type underlyingType )
		{
			context.BeginMethodOverride( MethodName.UnpackFromUnderlyingValue );

			context.EndMethodOverride(
				MethodName.UnpackFromUnderlyingValue,
				this.EmitRetrunStatement(
					context,
					this.EmitEnumFromUnderlyingCastExpression(
						context,
						this.TargetType,
						this.EmitInvokeMethodExpression(
							context,
							this.ReferArgument( context, TypeDefinition.MessagePackObjectType, "messagePackObject", 1 ),
							typeof( MessagePackObject ).GetMethod( "As" + underlyingType.Name, ReflectionAbstractions.EmptyTypes )
						)
					)
				)
			);
		}

		protected abstract TConstruct EmitEnumToUnderlyingCastExpression( TContext context, Type underlyingType, TConstruct enumValue );

		protected abstract TConstruct EmitEnumFromUnderlyingCastExpression( TContext context, Type enumType, TConstruct underlyingValue );
	}
}