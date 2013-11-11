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
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Reflection;


namespace MsgPack.Serialization.AbstractSerializers
{
	[ContractClassFor( typeof( SerializerBuilder<,,> ) )]
	internal class SerializerBuilderContract<TContext, TConstruct, TObject> : SerializerBuilder<TContext, TConstruct, TObject>
		where TContext : SerializerGenerationContext<TConstruct>
		where TConstruct : class, ICodeConstruct
	{
		public SerializerBuilderContract() { }

		protected override TContext CreateCodeGenerationContextForSerializerCreation( SerializationContext context )
		{
			Contract.Requires( context != null );
			Contract.Ensures( Contract.Result<TContext>() != null );
			return default( TContext );
		}

		protected override ISerializerCodeGenerationContext CreateGenerationContextForCodeGenerationCore( SerializationContext context )
		{
			Contract.Requires( context != null );
			Contract.Ensures( Contract.Result<TContext>() != null );
			return default( ISerializerCodeGenerationContext );
		}

		protected override void BuildSerializerCodeCore( ISerializerCodeGenerationContext context )
		{
			Contract.Requires( context != null );
		}

		protected override Func<SerializationContext, MessagePackSerializer<TObject>> CreateSerializerConstructor( TContext codeGenerationContext )
		{
			Contract.Requires( codeGenerationContext != null );
			Contract.Ensures( Contract.Result<Func<SerializationContext, MessagePackSerializer<TObject>>>() != null );
			return default( Func<SerializationContext, MessagePackSerializer<TObject>> );
		}

		protected override void EmitMethodPrologue( TContext context, SerializerMethod method )
		{
			Contract.Requires( context != null );
			Contract.Requires( Enum.IsDefined( typeof( SerializationMethod ), method ) );
		}

		protected override void EmitMethodEpilogue( TContext context, SerializerMethod method, TConstruct construct )
		{
			Contract.Requires( context != null );
			Contract.Requires( Enum.IsDefined( typeof( SerializationMethod ), method ) );
			Contract.Requires( construct != null );
		}

		protected override TConstruct EmitSequentialStatements( TContext context, Type contextType, IEnumerable<TConstruct> statements )
		{
			Contract.Requires( context != null );
			Contract.Requires( contextType != null );
			Contract.Requires( statements != null );
			Contract.Ensures( Contract.Result<TConstruct>() != null );
			Contract.Ensures( Contract.Result<TConstruct>().ContextType == typeof( void ) );
			return default( TConstruct );
		}

		protected override TConstruct EmitStatementExpression( TContext context, TConstruct statement, TConstruct contextExpression )
		{
			Contract.Requires( context != null );
			Contract.Requires( statement != null );
			Contract.Requires( contextExpression != null );
			Contract.Ensures( Contract.Result<TConstruct>() != null );
			Contract.Ensures( Contract.Result<TConstruct>().ContextType == contextExpression.ContextType );
			return default( TConstruct );
		}

		protected override TConstruct MakeNullLiteral( TContext context, Type contextType )
		{
			Contract.Requires( context != null );
			Contract.Requires( contextType != null );
			Contract.Ensures( Contract.Result<TConstruct>() != null );
			Contract.Ensures( Contract.Result<TConstruct>().ContextType == contextType );
			return default( TConstruct );
		}

		protected override TConstruct MakeInt32Literal( TContext context, int constant )
		{
			Contract.Requires( context != null );
			Contract.Ensures( Contract.Result<TConstruct>() != null );
			Contract.Ensures( Contract.Result<TConstruct>().ContextType == typeof( int ) );
			return default( TConstruct );
		}

		protected override TConstruct MakeInt64Literal( TContext context, long constant )
		{
			Contract.Requires( context != null );
			Contract.Ensures( Contract.Result<TConstruct>() != null );
			Contract.Ensures( Contract.Result<TConstruct>().ContextType == typeof( long ) );
			return default( TConstruct );
		}

		protected override TConstruct MakeStringLiteral( TContext context, string constant )
		{
			Contract.Requires( context != null );
			Contract.Ensures( Contract.Result<TConstruct>() != null );
			Contract.Ensures( Contract.Result<TConstruct>().ContextType == typeof( string ) );
			return default( TConstruct );
		}

		protected override TConstruct EmitThisReferenceExpression( TContext context )
		{
			Contract.Requires( context != null );
			Contract.Ensures( Contract.Result<TConstruct>() != null );
			Contract.Ensures(
				typeof( MessagePackSerializer<TObject> ).IsAssignableFrom( Contract.Result<TConstruct>().ContextType )
			);
			return default( TConstruct );
		}

		protected override TConstruct EmitBoxExpression( TContext context, Type valueType, TConstruct value )
		{
			Contract.Requires( context != null );
			Contract.Requires( valueType != null );
			Contract.Requires( value != null );
			Contract.Requires( value.ContextType == valueType );
			Contract.Ensures( Contract.Result<TConstruct>() != null );
			Contract.Ensures( Contract.Result<TConstruct>().ContextType == typeof( object ) );
			return default( TConstruct );
		}

		protected override TConstruct EmitNotExpression( TContext context, TConstruct booleanExpression )
		{
			Contract.Requires( context != null );
			Contract.Requires( booleanExpression != null );
			Contract.Requires( booleanExpression.ContextType == typeof( bool ) );
			Contract.Ensures( Contract.Result<TConstruct>() != null );
			Contract.Ensures( Contract.Result<TConstruct>().ContextType == typeof( bool ) );
			return default( TConstruct );
		}

		protected override TConstruct EmitEqualsExpression( TContext context, TConstruct left, TConstruct right )
		{
			Contract.Requires( context != null );
			Contract.Requires( left != null );
			Contract.Requires( right != null );
			Contract.Ensures( Contract.Result<TConstruct>() != null );
			Contract.Ensures( Contract.Result<TConstruct>().ContextType == typeof( bool ) );
			return default( TConstruct );
		}

		protected override TConstruct EmitNotEqualsExpression( TContext context, TConstruct left, TConstruct right )
		{
			Contract.Requires( context != null );
			Contract.Requires( left != null );
			Contract.Requires( right != null );
			Contract.Ensures( Contract.Result<TConstruct>() != null );
			Contract.Ensures( Contract.Result<TConstruct>().ContextType == typeof( bool ) );
			return default( TConstruct );
		}

		protected override TConstruct EmitGreaterThanExpression( TContext context, TConstruct left, TConstruct right )
		{
			Contract.Requires( context != null );
			Contract.Requires( left != null );
			Contract.Requires( right != null );
			Contract.Ensures( Contract.Result<TConstruct>() != null );
			Contract.Ensures( Contract.Result<TConstruct>().ContextType == typeof( bool ) );
			return default( TConstruct );
		}

		protected override TConstruct EmitLessThanExpression( TContext context, TConstruct left, TConstruct right )
		{
			Contract.Requires( context != null );
			Contract.Requires( left != null );
			Contract.Requires( right != null );
			Contract.Ensures( Contract.Result<TConstruct>() != null );
			Contract.Ensures( Contract.Result<TConstruct>().ContextType == typeof( bool ) );
			return default( TConstruct );
		}

		protected override TConstruct EmitIncrementExpression( TContext context, TConstruct int32Value )
		{
			Contract.Requires( context != null );
			Contract.Requires( int32Value != null );
			Contract.Requires( int32Value.ContextType == typeof( int ) );
			Contract.Ensures( Contract.Result<TConstruct>() != null );
			Contract.Ensures( Contract.Result<TConstruct>().ContextType == typeof( int ) );
			return default( TConstruct );
		}

		protected override TConstruct EmitTypeOfExpression( TContext context, Type type )
		{
			Contract.Requires( context != null );
			Contract.Requires( type != null );
			Contract.Ensures( Contract.Result<TConstruct>() != null );
			Contract.Ensures( Contract.Result<TConstruct>().ContextType == typeof( Type ) );
			return default( TConstruct );
		}

		protected override TConstruct DeclareLocal( TContext context, Type type, string name )
		{
			Contract.Requires( context != null );
			Contract.Requires( type != null );
			Contract.Ensures( Contract.Result<TConstruct>() != null );
			Contract.Ensures( Contract.Result<TConstruct>().ContextType == type );
			return default( TConstruct );
		}

		protected override TConstruct EmitInvokeVoidMethod( TContext context, TConstruct instance, MethodInfo method, params TConstruct[] arguments )
		{
			Contract.Requires( context != null );
			Contract.Requires( instance == null && method.IsStatic );
			Contract.Requires( instance != null && !method.IsStatic );
			Contract.Requires( method != null );
			Contract.Requires(
				( arguments == null && method.GetParameters().Length == 0 ) ||
				( arguments.Length == method.GetParameters().Length )
			);
			Contract.Ensures( Contract.Result<TConstruct>() != null );
			Contract.Ensures( Contract.Result<TConstruct>().ContextType == typeof( void ) );
			return default( TConstruct );
		}

		protected override TConstruct EmitCreateNewObjectExpression( TContext context, TConstruct variable, ConstructorInfo constructor, params TConstruct[] arguments )
		{
			Contract.Requires( context != null );
			Contract.Requires( ( variable != null ) == constructor.DeclaringType.IsValueType );
			Contract.Requires( constructor != null );
			Contract.Requires( variable.ContextType == constructor.DeclaringType );
			Contract.Requires(
				( arguments == null && constructor.GetParameters().Length == 0 ) ||
				( arguments.Length == constructor.GetParameters().Length )
			);
			Contract.Ensures( Contract.Result<TConstruct>() != null );
			Contract.Ensures( Contract.Result<TConstruct>().ContextType == typeof( void ) );
			return default( TConstruct );
		}

		protected override TConstruct EmitCreateNewArrayExpression( TContext context, Type elementType, int length, IEnumerable<TConstruct> initialElements )
		{
			Contract.Requires( context != null );
			Contract.Requires( elementType != null );
			Contract.Requires( length >= 0 );
			Contract.Requires( initialElements != null );
			Contract.Ensures( Contract.Result<TConstruct>() != null );
			Contract.Ensures( Contract.Result<TConstruct>().ContextType == elementType.MakeArrayType() );
			return default( TConstruct );
		}

		protected override TConstruct EmitInvokeMethodExpression( TContext context, TConstruct instance, MethodInfo method, IEnumerable<TConstruct> arguments )
		{
			Contract.Requires( context != null );
			Contract.Requires( instance == null && method.IsStatic );
			Contract.Requires( instance != null && !method.IsStatic );
			Contract.Requires( method != null );
			Contract.Requires( arguments != null );
			Contract.Ensures( Contract.Result<TConstruct>() != null );
			Contract.Ensures( Contract.Result<TConstruct>().ContextType == method.ReturnType );
			return default( TConstruct );
		}

		protected override TConstruct EmitGetPropretyExpression( TContext context, TConstruct instance, PropertyInfo property )
		{
			Contract.Requires( context != null );
			Contract.Requires( property != null );
			Contract.Requires( property.GetGetMethod( false ) != null );
			Contract.Requires( instance == null && property.GetGetMethod( false ).IsStatic );
			Contract.Requires( instance != null && !property.GetGetMethod( false ).IsStatic );
			Contract.Ensures( Contract.Result<TConstruct>() != null );
			Contract.Ensures( Contract.Result<TConstruct>().ContextType == property.PropertyType );
			return default( TConstruct );
		}

		protected override TConstruct EmitGetFieldExpression( TContext context, TConstruct instance, FieldInfo field )
		{
			Contract.Requires( context != null );
			Contract.Requires( instance == null && field.IsStatic );
			Contract.Requires( instance != null && !field.IsStatic );
			Contract.Requires( field != null );
			Contract.Ensures( Contract.Result<TConstruct>() != null );
			Contract.Ensures( Contract.Result<TConstruct>().ContextType == field.FieldType );
			return default( TConstruct );
		}

		protected override TConstruct EmitSetProprety( TContext context, TConstruct instance, PropertyInfo property, TConstruct value )
		{
			Contract.Requires( context != null );
			Contract.Requires( property != null );
			Contract.Requires( property.GetSetMethod( false ) != null );
			Contract.Requires( instance == null && property.GetSetMethod( false ).IsStatic );
			Contract.Requires( instance != null && !property.GetSetMethod( false ).IsStatic );
			Contract.Requires( value != null );
			Contract.Requires(
				property.GetSetMethod( false ).GetParameters()[ 0 ].ParameterType.IsAssignableFrom( value.ContextType )
			);
			Contract.Ensures( Contract.Result<TConstruct>() != null );
			Contract.Ensures( Contract.Result<TConstruct>().ContextType == typeof( void ) );
			return default( TConstruct );
		}

		protected override TConstruct EmitSetField( TContext context, TConstruct instance, FieldInfo field, TConstruct value )
		{
			Contract.Requires( context != null );
			Contract.Requires( field != null );
			Contract.Requires( instance == null && field.IsStatic );
			Contract.Requires( instance != null && !field.IsStatic );
			Contract.Requires( value != null );
			Contract.Requires(
				field.FieldType.IsAssignableFrom( value.ContextType )
			);
			Contract.Ensures( Contract.Result<TConstruct>() != null );
			Contract.Ensures( Contract.Result<TConstruct>().ContextType == typeof( void ) );
			return default( TConstruct );
		}

		protected override TConstruct EmitLoadVariableExpression( TContext context, TConstruct variable )
		{
			Contract.Requires( context != null );
			Contract.Requires( variable != null );
			Contract.Ensures( Contract.Result<TConstruct>() != null );
			Contract.Ensures( Contract.Result<TConstruct>().ContextType == variable.ContextType );
			return default( TConstruct );
		}

		protected override TConstruct EmitStoreVariableStatement( TContext context, TConstruct variable, TConstruct value )
		{
			Contract.Requires( context != null );
			Contract.Requires( variable != null );
			Contract.Ensures( Contract.Result<TConstruct>() != null );
			Contract.Ensures( Contract.Result<TConstruct>().ContextType == typeof( void ) );
			return default( TConstruct );
		}

		protected override TConstruct EmitThrowExpression( TContext context, Type expressionType, TConstruct exceptionExpression )
		{
			Contract.Requires( context != null );
			Contract.Requires( exceptionExpression != null );
			Contract.Requires( typeof( Exception ).IsAssignableFrom( exceptionExpression.ContextType ) );
			Contract.Requires( expressionType != null );
			Contract.Ensures( Contract.Result<TConstruct>() != null );
			Contract.Ensures( Contract.Result<TConstruct>().ContextType == expressionType );
			return default( TConstruct );
		}

		protected override TConstruct EmitTryFinallyExpression( TContext context, TConstruct tryExpression, TConstruct finallyStatement )
		{
			Contract.Requires( context != null );
			Contract.Requires( tryExpression != null );
			Contract.Requires( finallyStatement != null );
			Contract.Ensures( Contract.Result<TConstruct>() != null );
			Contract.Ensures( Contract.Result<TConstruct>().ContextType == tryExpression.ContextType );
			return default( TConstruct );
		}

		protected override TConstruct EmitGetSerializerExpression( TContext context, Type targetType )
		{
			Contract.Requires( context != null );
			Contract.Requires( targetType != null );
			Contract.Ensures( Contract.Result<TConstruct>() != null );
			Contract.Ensures( Contract.Result<TConstruct>().ContextType == typeof( MessagePackSerializer<> ).MakeGenericType( targetType ) );
			return default( TConstruct );
		}

		protected override TConstruct EmitConditionalExpression( TContext context, TConstruct conditionExpression, TConstruct thenExpression, TConstruct elseExpression )
		{
			Contract.Requires( context != null );
			Contract.Requires( conditionExpression != null );
			Contract.Requires( conditionExpression.ContextType == typeof( bool ) );
			Contract.Requires( thenExpression != null );
			Contract.Requires( elseExpression == null || thenExpression.ContextType == elseExpression.ContextType );
			Contract.Ensures( Contract.Result<TConstruct>() != null );
			Contract.Ensures( Contract.Result<TConstruct>().ContextType == thenExpression.ContextType );
			return default( TConstruct );
		}

		protected override TConstruct EmitAndConditionalExpression( TContext context, IList<TConstruct> conditionExpressions, TConstruct thenExpression, TConstruct elseExpression )
		{
			Contract.Requires( context != null );
			Contract.Requires( conditionExpressions != null );
			Contract.Requires( conditionExpressions.Count > 0 );
			Contract.Requires( Contract.ForAll( conditionExpressions, x => x.ContextType == typeof( bool ) ) );
			Contract.Requires( thenExpression != null );
			Contract.Requires( elseExpression == null || thenExpression.ContextType == elseExpression.ContextType );
			Contract.Ensures( Contract.Result<TConstruct>() != null );
			Contract.Ensures( Contract.Result<TConstruct>().ContextType == thenExpression.ContextType );
			return default( TConstruct );
		}

		protected override TConstruct EmitStringSwitchStatement( TContext context, TConstruct target, IDictionary<string, TConstruct> cases )
		{
			Contract.Requires( context != null );
			Contract.Requires( target != null );
			Contract.Requires( cases != null );
			Contract.Requires( cases.Count > 0 );
			Contract.Ensures( Contract.Result<TConstruct>() != null );
			Contract.Ensures( Contract.Result<TConstruct>().ContextType == typeof( void ) );
			return default( TConstruct );
		}

		protected override TConstruct EmitForLoop( TContext context, TConstruct count, Func<ForLoopContext, TConstruct> loopBodyEmitter )
		{
			Contract.Requires( context != null );
			Contract.Requires( count != null );
			Contract.Requires( loopBodyEmitter != null );
			Contract.Ensures( Contract.Result<TConstruct>() != null );
			Contract.Ensures( Contract.Result<TConstruct>().ContextType == typeof( void ) );
			return default( TConstruct );
		}

		protected override TConstruct EmitForEachLoop( TContext context, CollectionTraits collectionTraits, TConstruct collection, Func<TConstruct, TConstruct> loopBodyEmitter )
		{
			Contract.Requires( context != null );
			Contract.Requires( collection != null );
			Contract.Requires( loopBodyEmitter != null );
			Contract.Ensures( Contract.Result<TConstruct>() != null );
			Contract.Ensures( Contract.Result<TConstruct>().ContextType == typeof( void ) );
			return default( TConstruct );
		}
	}
}
