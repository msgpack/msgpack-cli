#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2015-2016 FUJIWARA, Yusuke
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
using System.Linq;
#if FEATURE_TAP
using System.Threading.Tasks;
#endif // FEATURE_TAP

namespace MsgPack.Serialization.AbstractSerializers
{
	// ReSharper disable once UnusedTypeParameter
	partial class SerializerBuilder<TContext, TConstruct>
	{
		private void BuildNullableSerializer( TContext context, Type underlyingType )
		{
			this.BuildNullablePackTo( context, underlyingType, false );
			this.BuildNullableUnpackFrom( context, underlyingType, false );
#if FEATURE_TAP
			if ( this.WithAsync( context ) )
			{
				this.BuildNullablePackTo( context, underlyingType, true );
				this.BuildNullableUnpackFrom( context, underlyingType, true );
			}
#endif // FEATURE_TAP
		}

		private void BuildNullablePackTo( TContext context, Type underlyingType, bool isAsync )
		{
			// null was handled in PackTo() method.
			/*
			 * 	this._valueSerializer.PackToCore( packer, objectTree.Value );
			 */

			var methodName = 
#if FEATURE_TAP
				isAsync ? MethodName.PackToAsyncCore : 
#endif // FEATURE_TAP
				MethodName.PackToCore;
			context.BeginMethodOverride( methodName );
			context.EndMethodOverride(
				methodName,
				this.EmitSerializeItemExpressionCore(
					context,
					context.Packer,
					underlyingType,
					this.EmitGetProperty( context, context.PackToTarget, this.TargetType.GetProperty( "Value" ), false ),
					null,
					null,
					isAsync
				)
			);
		}

		private void BuildNullableUnpackFrom( TContext context, Type underlyingType, bool isAsync )
		{
			// nil was handled in UnpackFrom() method.
			/*
			 *	return this._valueSerializer.UnpackFromCore( unpacker );
			 */

			var methodName =
#if FEATURE_TAP
				isAsync ? MethodName.UnpackFromAsyncCore : 
#endif // FEATURE_TAP
				MethodName.UnpackFromCore;
#if FEATURE_TAP
			var asyncMethodReturnType = typeof( Task<> ).MakeGenericType( underlyingType );
#endif // FEATURE_TAP

			context.BeginMethodOverride( methodName );

			var result = this.DeclareLocal( context, this.TargetType, "result" );

			var methodBody =
				this.EmitInvokeMethodExpression(
					context,
					this.EmitGetSerializerExpression( context, underlyingType, null, null ),
#if FEATURE_TAP
					isAsync 
					? typeof( MessagePackSerializer<> ).MakeGenericType( underlyingType ).GetMethods()
						.Single( m => m.IsPublic && m.Name == "UnpackFromAsync" && m.ReturnType == asyncMethodReturnType && m.GetParameterTypes().SequenceEqual( SerializerBuilderHelper.UnpackFromAsyncParameterTypes ) ) :
#endif // FEATURE_TAP
					typeof( MessagePackSerializer<> ).MakeGenericType( underlyingType ).GetMethod( "UnpackFrom" ),
					context.Unpacker
				);


			context.EndMethodOverride( 
				methodName, 
				this.EmitRetrunStatement( 
					context, 
#if FEATURE_TAP
					isAsync ?
					// Use helper for Task<T> -> Task<T?>
					this.EmitInvokeMethodExpression(
						context,
						null,
						Metadata._UnpackHelpers.ToNullable1Method.MakeGenericMethod( underlyingType ),
						methodBody
					) :
#endif // FEATURE_TAP
					// Use Nullable<T> constructor for T -> T?
					this.EmitCreateNewObjectExpression(
						context,
						result,
						this.TargetType.GetConstructors().Single( c => c.GetParameters().Length == 1 ),
						methodBody
					)
				)
			);
		}
	}
}
