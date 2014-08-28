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
using System.Globalization;
using System.Reflection;
using System.Reflection.Emit;
using MsgPack.Serialization.AbstractSerializers;
using MsgPack.Serialization.Reflection;

namespace MsgPack.Serialization.EmittingSerializers
{
	/// <summary>
	///		Represents code construct for <see cref="ILEmittingSerializerBuilder{TContext,TObject}"/>s.
	/// </summary>
	internal abstract class ILConstruct : ICodeConstruct
	{
		public static readonly ILConstruct[] NoArguments = new ILConstruct[ 0 ];

		private readonly Type _contextType;

		/// <summary>
		///		Gets the context type of this construct.
		/// </summary>
		/// <value>
		///		The context type of this construct.
		///		This value will not be <c>null</c>, but might be <see cref="Void" />.
		/// </value>
		/// <remarks>
		///		A context type represents the type of the evaluation context.
		/// </remarks>
		public Type ContextType
		{
			get { return this._contextType; }
		}

		/// <summary>
		///		Gets a value indicating whether this instance is terminating.
		/// </summary>
		/// <value>
		///		<c>true</c> if this instruction terminates method; otherwise, <c>false</c>.
		/// </value>
		public virtual bool IsTerminating
		{
			get { return false; }
		}

		/// <summary>
		///		Initializes a new instance of the <see cref="ILConstruct"/> class.
		/// </summary>
		/// <param name="contextType">The type.</param>
		protected ILConstruct( Type contextType )
		{
			this._contextType = contextType;
		}

		/// <summary>
		///		Evaluates this construct that is executing this construct as instruction.
		/// </summary>
		/// <param name="il">The <see cref="TracingILGenerator"/>.</param>
		/// <exception cref="InvalidOperationException">
		///		This construct does not have eval semantics.
		/// </exception>
		public virtual void Evaluate( TracingILGenerator il )
		{
			throw new InvalidOperationException(
				String.Format( CultureInfo.CurrentCulture, "'{0}' does not define stand alone instruction.", this )
			);
		}

		/// <summary>
		///		Loads value from the storage represented by this construct.
		/// </summary>
		/// <param name="il">The <see cref="TracingILGenerator"/>.</param>
		/// <param name="shouldBeAddress">
		///		<c>true</c>, if value type value should be pushed its address instead of bits; otherwise, <c>false</c>.
		/// </param>
		/// <exception cref="InvalidOperationException">
		///		This construct does not have load value semantics.
		/// </exception>
		public virtual void LoadValue( TracingILGenerator il, bool shouldBeAddress )
		{
			throw new InvalidOperationException(
				String.Format( CultureInfo.CurrentCulture, "'{0}' does not define load value instruction.", this )
			);
		}

		/// <summary>
		///		Stores value to the storage represented by this construct.
		/// </summary>
		/// <param name="il">The <see cref="TracingILGenerator"/>.</param>
		/// <exception cref="InvalidOperationException">
		///		This construct does not have store value semantics.
		/// </exception>
		public virtual void StoreValue( TracingILGenerator il )
		{
			throw new InvalidOperationException(
				String.Format( CultureInfo.CurrentCulture, "'{0}' does not define store value instruction.", this )
			);
		}

		/// <summary>
		///		Evaluates this construct as branch instruction.
		/// </summary>
		/// <param name="il">The <see cref="TracingILGenerator"/>.</param>
		/// <param name="else">The <see cref="Label"/> which points the head of 'else' instructions.</param>
		/// <exception cref="InvalidOperationException">
		///		This construct does not have branch semantics.
		/// </exception>
		public virtual void Branch( TracingILGenerator il, Label @else )
		{
			throw new InvalidOperationException(
				String.Format( CultureInfo.CurrentCulture, "'{0}' does not define branch instruction.", this )
			);
		}

		public static ILConstruct LoadField( ILConstruct instance, FieldInfo field )
		{
			return new LoadFieldILConstruct( instance, field );
		}

		public static ILConstruct StoreField( ILConstruct instance, FieldInfo field, ILConstruct value )
		{
			return new StoreFieldILConstruct( instance, field, value );
		}

		public static ILConstruct StoreLocal( ILConstruct variable, ILConstruct value )
		{
			return new StoreVariableILConstruct( variable, value );
		}

		public static ILConstruct Instruction( string description, Type contextType, bool isTerminating, Action<TracingILGenerator> instructions )
		{
			return new SinglelStepILConstruct( contextType, description, isTerminating, instructions );
		}

		public static ILConstruct Argument( int index, Type type, string name )
		{
			return new VariableILConstruct( name, type, index );
		}

		public static ILConstruct IfThenElse( ILConstruct conditionExpression, ILConstruct thenExpression, ILConstruct elseExpression )
		{
			return new ConditionalILConstruct( conditionExpression, thenExpression, elseExpression );
		}

		public static ILConstruct AndCondition( IList<ILConstruct> conditionExpressions )
		{
			return new AndConditionILConstruct( conditionExpressions );
		}

		public static ILConstruct UnaryOperator( string @operator, ILConstruct input, Action<TracingILGenerator, ILConstruct> operation )
		{
			return new UnaryOperatorILConstruct( @operator, input, operation );
		}

		public static ILConstruct UnaryOperator( string @operator, ILConstruct input, Action<TracingILGenerator, ILConstruct> operation, Action<TracingILGenerator, ILConstruct, Label> branchOperation )
		{
			return new UnaryOperatorILConstruct( @operator, input, operation, branchOperation );
		}

		public static ILConstruct BinaryOperator( string @operator, Type resultType, ILConstruct left, ILConstruct right, Action<TracingILGenerator, ILConstruct, ILConstruct> operation, Action<TracingILGenerator, ILConstruct, ILConstruct, Label> branchOperation )
		{
			return new BinaryOperatorILConstruct( @operator, resultType, left, right, operation, branchOperation );
		}

		public static ILConstruct Invoke( ILConstruct target, MethodInfo method, IEnumerable<ILConstruct> arguments )
		{
			return new InvocationILConsruct( method, target, arguments );
		}

		internal static ILConstruct NewObject( ILConstruct variable, ConstructorInfo constructor, IEnumerable<ILConstruct> arguments )
		{
			return new InvocationILConsruct( constructor, variable, arguments );
		}

		public static ILConstruct Sequence( Type contextType, IEnumerable<ILConstruct> statements )
		{
			return new SequenceILConstruct( contextType, statements );
		}

		public static ILConstruct Composite( ILConstruct before, ILConstruct context )
		{
			return new StatementExpressionILConstruct( before, context );
		}

		public static ILConstruct Literal<T>( Type type, T literalValue, Action<TracingILGenerator> instruction )
		{
			// ReSharper disable CompareNonConstrainedGenericWithNull
			return new SinglelStepILConstruct( type, "literal " + ( literalValue == null ? "(null)" : literalValue.ToString() ), false, instruction );
			// ReSharper restore CompareNonConstrainedGenericWithNull
		}

		public static ILConstruct Variable( Type type, string name )
		{
			return new VariableILConstruct( name, type );
		}

		protected static void ValidateContextTypeMatch( ILConstruct left, ILConstruct right )
		{
			//if ( left.ContextType == typeof( Any ) || right.ContextType == typeof( Any ) )
			//{
			//	return;
			//}

			if ( GetNormalizedType( left.ContextType ) != GetNormalizedType( right.ContextType ) )
			{
				throw new ArgumentException(
					String.Format(
						CultureInfo.CurrentCulture,
						"Right type '{1}' does not equal to left type '{0}'.",
						left.ContextType,
						right.ContextType
						),
					"right"
					);
			}
		}

		private static Type GetNormalizedType( Type type )
		{
			if ( !type.IsPrimitive )
			{
				return type;
			}

			if ( type == typeof( sbyte ) || type == typeof( short ) || type == typeof( int ) ||
				type == typeof( byte ) || type == typeof( ushort ) || type == typeof( uint ) )
			{
				return typeof( long );
			}

			if ( type == typeof( float ) )
			{
				return typeof( double );
			}

			return type;
		}
	}
}