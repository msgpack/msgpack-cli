#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2015 FUJIWARA, Yusuke
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
	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Contract class" )]
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

#if !NETFX_CORE && !SILVERLIGHT
		protected override void BuildSerializerCodeCore( ISerializerCodeGenerationContext context, Type concreteType, PolymorphismSchema itemSchema )
		{
			Contract.Requires( context != null );
		}
#endif

		protected override Func<SerializationContext, MessagePackSerializer<TObject>> CreateSerializerConstructor( TContext codeGenerationContext, PolymorphismSchema schema )
		{
			Contract.Requires( codeGenerationContext != null );
			Contract.Ensures( Contract.Result<Func<SerializationContext, MessagePackSerializer<TObject>>>() != null );
			return default( Func<SerializationContext, MessagePackSerializer<TObject>> );
		}

		protected override Func<SerializationContext, MessagePackSerializer<TObject>> CreateEnumSerializerConstructor( TContext codeGenerationContext )
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

		protected override void EmitMethodPrologue( TContext context, EnumSerializerMethod method )
		{
			Contract.Requires( context != null );
			Contract.Requires( Enum.IsDefined( typeof( EnumSerializerMethod ), method ) );
		}

		protected override void EmitMethodPrologue( TContext context, CollectionSerializerMethod method, MethodInfo declaration )
		{
			Contract.Requires( context != null );
			Contract.Requires( Enum.IsDefined( typeof( CollectionSerializerMethod ), method ) );
			Contract.Requires( declaration != null );
		}

		protected override void EmitMethodEpilogue( TContext context, SerializerMethod method, TConstruct construct )
		{
			Contract.Requires( context != null );
			Contract.Requires( Enum.IsDefined( typeof( SerializationMethod ), method ) );
			Contract.Requires( construct != null );
		}

		protected override void EmitMethodEpilogue( TContext context, EnumSerializerMethod method, TConstruct construct )
		{
			Contract.Requires( context != null );
			Contract.Requires( Enum.IsDefined( typeof( EnumSerializerMethod ), method ) );
			Contract.Requires( construct != null );
		}

		protected override void EmitMethodEpilogue( TContext context, CollectionSerializerMethod method, TConstruct construct )
		{
			Contract.Requires( context != null );
			Contract.Requires( Enum.IsDefined( typeof( CollectionSerializerMethod ), method ) );
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

		protected override TConstruct MakeNullLiteral( TContext context, Type contextType )
		{
			Contract.Requires( context != null );
			Contract.Requires( contextType != null );
			Contract.Ensures( Contract.Result<TConstruct>() != null );
			Contract.Ensures( Contract.Result<TConstruct>().ContextType == contextType );
			return default( TConstruct );
		}

		protected override TConstruct MakeByteLiteral( TContext context, byte constant )
		{
			Contract.Requires( context != null );
			Contract.Ensures( Contract.Result<TConstruct>() != null );
			Contract.Ensures( Contract.Result<TConstruct>().ContextType == typeof( byte ) );
			return default( TConstruct );
		}

		protected override TConstruct MakeSByteLiteral( TContext context, sbyte constant )
		{
			Contract.Requires( context != null );
			Contract.Ensures( Contract.Result<TConstruct>() != null );
			Contract.Ensures( Contract.Result<TConstruct>().ContextType == typeof( sbyte ) );
			return default( TConstruct );
		}

		protected override TConstruct MakeInt16Literal( TContext context, short constant )
		{
			Contract.Requires( context != null );
			Contract.Ensures( Contract.Result<TConstruct>() != null );
			Contract.Ensures( Contract.Result<TConstruct>().ContextType == typeof( short ) );
			return default( TConstruct );
		}

		protected override TConstruct MakeUInt16Literal( TContext context, ushort constant )
		{
			Contract.Requires( context != null );
			Contract.Ensures( Contract.Result<TConstruct>() != null );
			Contract.Ensures( Contract.Result<TConstruct>().ContextType == typeof( ushort ) );
			return default( TConstruct );
		}

		protected override TConstruct MakeInt32Literal( TContext context, int constant )
		{
			Contract.Requires( context != null );
			Contract.Ensures( Contract.Result<TConstruct>() != null );
			Contract.Ensures( Contract.Result<TConstruct>().ContextType == typeof( int ) );
			return default( TConstruct );
		}

		protected override TConstruct MakeUInt32Literal( TContext context, uint constant )
		{
			Contract.Requires( context != null );
			Contract.Ensures( Contract.Result<TConstruct>() != null );
			Contract.Ensures( Contract.Result<TConstruct>().ContextType == typeof( uint ) );
			return default( TConstruct );
		}

		protected override TConstruct MakeInt64Literal( TContext context, long constant )
		{
			Contract.Requires( context != null );
			Contract.Ensures( Contract.Result<TConstruct>() != null );
			Contract.Ensures( Contract.Result<TConstruct>().ContextType == typeof( long ) );
			return default( TConstruct );
		}

		protected override TConstruct MakeUInt64Literal( TContext context, ulong constant )
		{
			Contract.Requires( context != null );
			Contract.Ensures( Contract.Result<TConstruct>() != null );
			Contract.Ensures( Contract.Result<TConstruct>().ContextType == typeof( ulong ) );
			return default( TConstruct );
		}

		protected override TConstruct MakeReal32Literal( TContext context, float constant )
		{
			Contract.Requires( context != null );
			Contract.Ensures( Contract.Result<TConstruct>() != null );
			Contract.Ensures( Contract.Result<TConstruct>().ContextType == typeof( float ) );
			return default( TConstruct );
		}

		protected override TConstruct MakeReal64Literal( TContext context, double constant )
		{
			Contract.Requires( context != null );
			Contract.Ensures( Contract.Result<TConstruct>() != null );
			Contract.Ensures( Contract.Result<TConstruct>().ContextType == typeof( double ) );
			return default( TConstruct );
		}

		protected override TConstruct MakeBooleanLiteral( TContext context, bool constant )
		{
			Contract.Requires( context != null );
			Contract.Ensures( Contract.Result<TConstruct>() != null );
			Contract.Ensures( Contract.Result<TConstruct>().ContextType == typeof( bool ) );
			return default( TConstruct );
		}

		protected override TConstruct MakeCharLiteral( TContext context, char constant )
		{
			Contract.Requires( context != null );
			Contract.Ensures( Contract.Result<TConstruct>() != null );
			Contract.Ensures( Contract.Result<TConstruct>().ContextType == typeof( char ) );
			return default( TConstruct );
		}

		protected override TConstruct MakeStringLiteral( TContext context, string constant )
		{
			Contract.Requires( context != null );
			Contract.Ensures( Contract.Result<TConstruct>() != null );
			Contract.Ensures( Contract.Result<TConstruct>().ContextType == typeof( string ) );
			return default( TConstruct );
		}

		protected override TConstruct MakeEnumLiteral( TContext context, Type type, object constant )
		{
			Contract.Requires( context != null );
#if !NETFX_CORE
			Contract.Requires( type.IsEnum );
#endif
			Contract.Requires( constant != null );
			Contract.Ensures( Contract.Result<TConstruct>() != null );
			Contract.Ensures( Contract.Result<TConstruct>().ContextType == type );
			return default( TConstruct );
		}

		protected override TConstruct MakeDefaultLiteral( TContext context, Type type )
		{
			Contract.Requires( context != null );
#if !NETFX_CORE
			Contract.Requires( type.IsValueType );
#endif
			Contract.Ensures( Contract.Result<TConstruct>() != null );
			Contract.Ensures( Contract.Result<TConstruct>().ContextType == type );
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

		protected override TConstruct EmitUnboxAnyExpression( TContext context, Type targetType, TConstruct value )
		{
			Contract.Requires( context != null );
			Contract.Requires( targetType != null );
			Contract.Requires( value != null );
			Contract.Requires( !value.ContextType.GetIsValueType() );
			Contract.Ensures( Contract.Result<TConstruct>() != null );
			Contract.Ensures( Contract.Result<TConstruct>().ContextType == targetType );
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

		protected override TConstruct EmitIncrement( TContext context, TConstruct int32Value )
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

		protected override TConstruct EmitFieldOfExpression( TContext context, FieldInfo field )
		{
			Contract.Requires( context != null );
			Contract.Requires( field != null );
			Contract.Ensures( Contract.Result<TConstruct>() != null );
			Contract.Ensures( Contract.Result<TConstruct>().ContextType == typeof( Type ) );
			return default( TConstruct );
		}

		protected override TConstruct EmitMethodOfExpression( TContext context, MethodBase method )
		{
			Contract.Requires( context != null );
			Contract.Requires( method != null );
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
			Contract.Requires( ( variable != null ) == constructor.DeclaringType.GetIsValueType() );
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

		protected override TConstruct EmitCreateNewArrayExpression( TContext context, Type elementType, int length )
		{
			Contract.Requires( context != null );
			Contract.Requires( elementType != null );
			Contract.Requires( length >= 0 );
			Contract.Ensures( Contract.Result<TConstruct>() != null );
			Contract.Ensures( Contract.Result<TConstruct>().ContextType == elementType.MakeArrayType() );
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

		protected override TConstruct EmitSetArrayElementStatement( TContext context, TConstruct array, TConstruct index, TConstruct value )
		{
			Contract.Requires( context != null );
			Contract.Requires( array != null );
			Contract.Requires( array.ContextType.IsArray );
			Contract.Requires( index != null );
			Contract.Requires( index.ContextType == typeof( int ) );
			Contract.Requires( value != null );
			Contract.Requires( array.ContextType.GetElementType().IsAssignableFrom( value.ContextType ) );
			Contract.Ensures( Contract.Result<TConstruct>() != null );
			Contract.Ensures( Contract.Result<TConstruct>().ContextType == array.ContextType );
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

		protected override TConstruct EmitGetPropertyExpression( TContext context, TConstruct instance, PropertyInfo property )
		{
			Contract.Requires( context != null );
			Contract.Requires( property != null );
			Contract.Requires( property.GetGetMethod() != null );
			Contract.Requires( instance == null && property.GetGetMethod().IsStatic );
			Contract.Requires( instance != null && !property.GetGetMethod().IsStatic );
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

		protected override TConstruct EmitSetProperty( TContext context, TConstruct instance, PropertyInfo property, TConstruct value )
		{
			Contract.Requires( context != null );
			Contract.Requires( property != null );
			Contract.Requires( property.GetSetMethod() != null );
			Contract.Requires( instance == null && property.GetSetMethod().IsStatic );
			Contract.Requires( instance != null && !property.GetSetMethod().IsStatic );
			Contract.Requires( value != null );
			Contract.Requires(
				property.GetSetMethod().GetParameters()[ 0 ].ParameterType.IsAssignableFrom( value.ContextType )
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

		protected override TConstruct EmitTryFinally( TContext context, TConstruct tryStatement, TConstruct finallyStatement )
		{
			Contract.Requires( context != null );
			Contract.Requires( tryStatement != null );
			Contract.Requires( finallyStatement != null );
			Contract.Ensures( Contract.Result<TConstruct>() != null );
			Contract.Ensures( Contract.Result<TConstruct>().ContextType == tryStatement.ContextType );
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

		protected override TConstruct EmitStringSwitchStatement ( TContext context, TConstruct target, IDictionary<string, TConstruct> cases, TConstruct defaultCase ) {
			Contract.Requires(context != null);
			Contract.Requires(target != null);
			Contract.Requires(defaultCase != null);
			Contract.Requires(cases != null);
			Contract.Requires(cases.Count > 0);
			Contract.Ensures(Contract.Result<TConstruct>() != null);
			Contract.Ensures(Contract.Result<TConstruct>().ContextType == typeof(void));
			return default(TConstruct);
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

		protected override TConstruct ReferArgument( TContext context, Type type, string name, int index )
		{
			Contract.Requires( context != null );
			Contract.Requires( type != null );
			Contract.Requires( !String.IsNullOrEmpty( name ) );
			Contract.Requires( index >= 0 );
			Contract.Ensures( Contract.Result<TConstruct>() != null );
			return default( TConstruct );
		}

		protected override TConstruct EmitEnumFromUnderlyingCastExpression( TContext context, Type enumType, TConstruct underlyingValue )
		{
			Contract.Requires( context != null );
			Contract.Requires( enumType != null );
#if !NETFX_CORE
			Contract.Requires( enumType.IsEnum );
#endif
			Contract.Requires( underlyingValue != null );
#if !NETFX_CORE
			Contract.Requires( underlyingValue.ContextType.IsPrimitive );
#endif
			Contract.Ensures( Contract.Result<TConstruct>() != null );
			Contract.Ensures( Contract.Result<TConstruct>().ContextType == enumType );
			return default( TConstruct );
		}

		protected override TConstruct EmitEnumToUnderlyingCastExpression( TContext context, Type underlyingType, TConstruct enumValue )
		{
			Contract.Requires( context != null );
			Contract.Requires( underlyingType != null );
#if !NETFX_CORE
			Contract.Requires( underlyingType.IsPrimitive );
#endif
			Contract.Requires( enumValue != null );
#if !NETFX_CORE
			Contract.Requires( enumValue.ContextType.IsEnum );
#endif
			Contract.Ensures( Contract.Result<TConstruct>() != null );
			Contract.Ensures( Contract.Result<TConstruct>().ContextType == underlyingType );
			return default( TConstruct );
		}
	}
}
