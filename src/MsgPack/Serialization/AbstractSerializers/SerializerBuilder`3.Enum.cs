#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2014 FUJIWARA, Yusuke
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
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;

namespace MsgPack.Serialization.AbstractSerializers
{
	partial class SerializerBuilder<TContext, TConstruct, TObject>
	{
		private static readonly Type[] StandardEnumSerializerConstructorTypes = { typeof( PackerCompatibilityOptions ), typeof( EnumSerializationMethod ), typeof( IDictionary<TObject, string> ), typeof( IDictionary<string, TObject> ) };

		private void BuildEnumSerializer( TContext context )
		{
			Contract.Assert( typeof( TObject ).GetIsEnum() );
			var underlyingType = Enum.GetUnderlyingType( typeof( TObject ) );
			Contract.Assert( underlyingType != null, "Underlying type of " + typeof( TObject ) + " is null." );

			this.BuildPackUnderlyingValueTo( context, underlyingType );
			this.BuildGetUnderlyingValueString( context, underlyingType );
			this.BuildUnpackFromUnderlyingValue( context, underlyingType );
			this.BuildTryParse( context, underlyingType );
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

		private void BuildGetUnderlyingValueString( TContext context, Type underlyingType )
		{
			this.EmitMethodPrologue(
				context,
				EnumSerializerMethod.GetUnderlyingValueString
			);

			TConstruct construct = null;
			try
			{
				construct =
					this.EmitRetrunStatement(
						context,
						this.EmitInvokeMethodExpression(
							context,
							this.EmitEnumToUnderlyingCastExpression(
								context,
								underlyingType,
								this.ReferArgument( context, typeof( TObject ), "enumValue", 1 )
							),
							underlyingType.GetMethod( "ToString", ReflectionAbstractions.EmptyTypes )
						)
					);
			}
			finally
			{
				this.EmitMethodEpilogue(
					context,
					EnumSerializerMethod.GetUnderlyingValueString,
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

		private void BuildTryParse( TContext context, Type underlyingType )
		{
			this.EmitMethodPrologue(
				context,
				EnumSerializerMethod.Parse
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
								null,
								underlyingType.GetMethod( "Parse", new[] { typeof( String ) } ),
								this.ReferArgument( context, typeof( String ), "value", 1 )
							)
						)
					);
			}
			finally
			{
				this.EmitMethodEpilogue(
					context,
					EnumSerializerMethod.Parse,
					construct
				);
			}
		}

		/// <summary>
		///		Builds enum serializer creation expression.
		/// </summary>
		/// <param name="context">The generation context.</param>
		/// <param name="enumType">The enum type of target member.</param>
		/// <param name="enumMemberSerializationMethod">The enum serialization method for target member.</param>
		/// <returns>
		///		Enum creation construct.
		/// </returns>
		/// <remarks>
		///		This method will be invoked via <see cref="EmitGetSerializerExpression"/> or constructor emitting.
		/// </remarks>
		protected TConstruct BuildCreateEnumSerializerInstance( TContext context, Type enumType, EnumMemberSerializationMethod enumMemberSerializationMethod )
		{
			var method = EnumMessagePackSerializerHelper.DetermineEnumSerializationMethod( context.SerializationContext, enumType, enumMemberSerializationMethod );

			var protoType = context.SerializationContext.GetSerializer<TObject>();

			return
				this.BuildCreateEnumSerializerInstance(
					context,
					protoType.GetType().GetConstructor( StandardEnumSerializerConstructorTypes ),
					method,
					Enum.GetNames( typeof( TObject ) ),
					Enum.GetValues( typeof( TObject ) ) as TObject[]
				);
		}

		private TConstruct BuildCreateEnumSerializerInstance(
			TContext context,
			ConstructorInfo serializerConstructor,
			EnumSerializationMethod method,
			IList<string> names,
			IList<TObject> values
			)
		{
			return
				this.EmitCreateNewObjectExpression(
					context,
					null, // serializer always reference type.
					serializerConstructor,
					this.EmitGetPropretyExpression(
						context,
						this.EmitThisReferenceExpression( context ),
						FromExpression.ToProperty( ( MessagePackSerializer<TObject> x ) => x.PackerCompatibilityOptions )
					),
					this.MakeEnumLiteral( context, typeof( EnumSerializationMethod ), method ),
					this.EmitCreateNewArrayExpression( context, typeof( string ), names.Count, names.Select( name => this.MakeStringLiteral( context, name ) ) ),
					this.EmitCreateNewArrayExpression( context, typeof( TObject ), values.Count, values.Select( value => this.MakeEnumLiteral( context, typeof( TObject ), value ) ) )
				);
		}

		protected abstract TConstruct EmitEnumToUnderlyingCastExpression( TContext context, Type underlyingType, TConstruct enumValue );

		protected abstract TConstruct EmitEnumFromUnderlyingCastExpression( TContext context, Type enumType, TConstruct underlyingValue );
	}
}