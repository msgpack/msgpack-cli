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
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using MsgPack.Serialization.AbstractSerializers;
using MsgPack.Serialization.Reflection;

namespace MsgPack.Serialization.EmittingSerializers
{
	internal abstract class ILConstruct : ICodeConstruct
	{
		public static class Any
		{
			// nop
		}

		public static readonly ILConstruct[] NoArguments = new ILConstruct[ 0 ];

		private readonly Type _type;

		public Type ContextType
		{
			get { return this._type; }
		}

		public virtual bool IsTerminating
		{
			get { return false; }
		}

		protected ILConstruct( Type type )
		{
			this._type = type;
		}

		public virtual void Evaluate( TracingILGenerator il )
		{
			throw new InvalidOperationException(
				String.Format( CultureInfo.CurrentCulture, "'{0}' does not define stand alone instruction.", this )
				);
		}

		public virtual void LoadValue( TracingILGenerator il, bool shouldBeAddress )
		{
			throw new InvalidOperationException(
				String.Format( CultureInfo.CurrentCulture, "'{0}' does not define load value instruction.", this )
			);
		}

		public virtual void StoreValue( TracingILGenerator il )
		{
			throw new InvalidOperationException(
				String.Format( CultureInfo.CurrentCulture, "'{0}' does not define store value instruction.", this )
			);
		}

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

		public static ILConstruct Nop( Type contextType )
		{
			return new NopILConstruct( contextType );
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

		public static ILConstruct BinaryOperator( string @operator, Type resultType, ILConstruct left, ILConstruct right, Action<TracingILGenerator, ILConstruct, ILConstruct> operation )
		{
			return new BinaryOperatorILConstruct( @operator, resultType, left, right, operation );
		}

		public static ILConstruct BinaryOperator( string @operator, Type resultType, ILConstruct left, ILConstruct right, Action<TracingILGenerator, ILConstruct, ILConstruct> operation, Action<TracingILGenerator, ILConstruct, ILConstruct, Label> branchOperation )
		{
			return new BinaryOperatorILConstruct( @operator, resultType, left, right, operation, branchOperation );
		}

		public static ILConstruct Invoke( ILConstruct target, MethodInfo method, IEnumerable<ILConstruct> arguments )
		{
			return new InvocationILConsruct( method, target, arguments );
		}

		internal static ILConstruct NewObject( ConstructorInfo constructor, IEnumerable<ILConstruct> arguments )
		{
			return new InvocationILConsruct( constructor, null, arguments );
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
			return new SinglelStepILConstruct( type, "literal " + ( literalValue == null ? "(null)" : literalValue.ToString() ), false, instruction );
		}

		public static ILConstruct Variable( ILEmittingContext context, Type type, string name, Action<TracingILGenerator, ILConstruct> initialization )
		{
			return new VariableILConstruct( name, type, initialization );
		}

		private static void ValidateContextTypeMatch( ILConstruct left, ILConstruct right )
		{
			if ( left.ContextType == typeof( Any ) || right.ContextType == typeof( Any ) )
			{
				return;
			}

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

		private class SinglelStepILConstruct : ILConstruct
		{
			private readonly string _description;
			private readonly Action<TracingILGenerator> _instruction;
			private readonly bool _isTerminating;

			public override bool IsTerminating
			{
				get { return this._isTerminating; }
			}

			public SinglelStepILConstruct( Type contextType, string description, bool isTerminating, Action<TracingILGenerator> instruction )
				: base( contextType )
			{
				this._description = description;
				this._instruction = instruction;
				this._isTerminating = isTerminating;
			}

			public override void Evaluate( TracingILGenerator il )
			{
				il.TraceWriteLine( "// Eval->: {0}", this );
				this._instruction( il );
				il.TraceWriteLine( "// ->Eval: {0}", this );
			}

			public override void LoadValue( TracingILGenerator il, bool shouldBeAddress )
			{
				il.TraceWriteLine( "// Load->: {0}", this );
				this._instruction( il );
				il.TraceWriteLine( "// ->Load: {0}", this );
			}

			public override void StoreValue( TracingILGenerator il )
			{
				il.TraceWriteLine( "// Stor->: {0}", this );
				this._instruction( il );
				il.TraceWriteLine( "// ->Stor: {0}", this );
			}

			public override string ToString()
			{
				return String.Format( CultureInfo.InvariantCulture, "Instruction[{0}]: {1}", this.ContextType, this._description );
			}
		}

		private abstract class ContextfulILConstruct : ILConstruct
		{
			protected ContextfulILConstruct( Type contextType )
				: base( contextType )
			{
			}

			public sealed override void Branch( TracingILGenerator il, Label @else )
			{
				il.TraceWriteLine( "// Brnc->: {0}", this );
				if ( this.ContextType != typeof( bool ) )
				{
					throw new InvalidOperationException(
						String.Format( CultureInfo.CurrentCulture, "Cannot branch with non boolean type '{0}'.", this.ContextType )
						);
				}

				this.BranchCore( il, @else );
				il.TraceWriteLine( "// ->Brnc: {0}", this );
			}

			protected virtual void BranchCore( TracingILGenerator il, Label @else )
			{
				this.LoadValue( il, false );
				il.EmitBrfalse( @else );
			}
		}

		private class VariableILConstruct : ContextfulILConstruct
		{
			private readonly bool _isLocal;
			private int _index;
			private readonly string _name;
			private readonly Action<TracingILGenerator, ILConstruct> _initializer;

			public VariableILConstruct( string name, Type valueType, Action<TracingILGenerator, ILConstruct> initializer )
				: base( valueType )
			{
				Contract.Assert( name != null );
				this._isLocal = true;
				this._name = name;
				this._index = -1;
				this._initializer = initializer;
			}

			public VariableILConstruct( string name, Type valueType, int parameterIndex )
				: base( valueType )
			{
				Contract.Assert( name != null );
				this._isLocal = false;
				this._name = name;
				this._index = parameterIndex;
				this._initializer = null;
			}

			public override void Evaluate( TracingILGenerator il )
			{
				if ( this._isLocal && this._index < 0 )
				{
					il.TraceWriteLine( "// Eval->: {0}", this );

					this._index = il.DeclareLocal( this.ContextType, this._name ).LocalIndex;

					if ( this._initializer != null )
					{
						this._initializer( il, this );
					}
					il.TraceWriteLine( "// ->Eval: {0}", this );
				}
			}

			public override void LoadValue( TracingILGenerator il, bool shouldBeAddress )
			{
				this.Evaluate( il );
				il.TraceWriteLine( "// Load->: {0}", this );
				if ( this.ContextType.GetIsValueType() && shouldBeAddress )
				{
					if ( this._isLocal )
					{
						il.EmitAnyLdloca( this._index );
					}
					else
					{
						il.EmitAnyLdarga( this._index );
					}
				}
				else
				{
					if ( this._isLocal )
					{
						il.EmitAnyLdloc( this._index );
					}
					else
					{
						il.EmitAnyLdarg( this._index );
					}
				}
				il.TraceWriteLine( "// ->Load: {0}", this );
			}

			public override void StoreValue( TracingILGenerator il )
			{
				il.TraceWriteLine( "// Stor->: {0}", this );
				if ( !this._isLocal )
				{
					throw new InvalidOperationException( "Cannot overwrite argument." );
				}

				il.EmitAnyStloc( this._index );
				il.TraceWriteLine( "// ->Stor: {0}", this );
			}

			public override string ToString()
			{
				return String.Format( CultureInfo.InvariantCulture, "Variable[{0}]: [{3}{4}]{1}({2})", this.ContextType, this._name, this._type, this._isLocal ? "local" : "arg", this._index );
			}
		}

		private class StoreVariableILConstruct : ILConstruct
		{
			private readonly ILConstruct _variable;
			private readonly ILConstruct _value;

			public StoreVariableILConstruct( ILConstruct variable, ILConstruct value )
				: base( typeof( void ) )
			{
				this._variable = variable;
				this._value = value;
			}

			public override void Evaluate( TracingILGenerator il )
			{
				if ( this._value != null )
				{
					this._value.LoadValue( il, false );
				}

				this._variable.StoreValue( il );
			}

			public override string ToString()
			{
				return String.Format( CultureInfo.InvariantCulture, "StoreVariable: {0} = (context)", this._variable );
			}
		}

		private class SequenceILConstruct : ILConstruct
		{
			private readonly ILConstruct[] _statements;

			public SequenceILConstruct( Type contextType, IEnumerable<ILConstruct> statements )
				: base( contextType )
			{
				this._statements = statements.ToArray();
			}

			public override void Evaluate( TracingILGenerator il )
			{
				il.TraceWriteLine( "// Eval->: {0}", this );

				foreach ( var statement in this._statements )
				{
					statement.Evaluate( il );
				}

				il.TraceWriteLine( "// ->Eval: {0}", this );
			}

			public override string ToString()
			{
				return String.Format( CultureInfo.InvariantCulture, "Sequence[{0}]", this._statements.Length );
			}
		}

		private class StatementExpressionILConstruct : ContextfulILConstruct
		{
			private bool _isBound;
			private readonly ILConstruct _binding;
			private readonly ILConstruct _expression;

			public StatementExpressionILConstruct( ILConstruct binding, ILConstruct expression )
				: base( expression.ContextType )
			{
				this._binding = binding;
				this._expression = expression;
			}

			public override void Evaluate( TracingILGenerator il )
			{
				il.TraceWriteLine( "// Eval->: {0}", this );
				this.Evaluate( il, false );
				il.TraceWriteLine( "// ->Eval: {0}", this );
			}

			public override void LoadValue( TracingILGenerator il, bool shouldBeAddress )
			{
				il.TraceWriteLine( "// Load->: {0}", this );
				this.Evaluate( il, shouldBeAddress );
				il.TraceWriteLine( "// ->Load: {0}", this );
			}

			private void Evaluate( TracingILGenerator il, bool shouldBeAddress )
			{
				if ( !this._isBound )
				{
					this._binding.Evaluate( il );
					this._isBound = true;
				}

				this._expression.LoadValue( il, shouldBeAddress );
			}

			public override string ToString()
			{
				return String.Format( CultureInfo.InvariantCulture, "Bind[{0}]: {1} context: {2}", this.ContextType, this._binding, this._expression );
			}
		}

		private class UnaryOperatorILConstruct : ContextfulILConstruct
		{
			private readonly string _operator;
			private readonly ILConstruct _input;
			private readonly Action<TracingILGenerator, ILConstruct> _operation;
			private readonly Action<TracingILGenerator, ILConstruct, Label> _branchOperation;

			public UnaryOperatorILConstruct( string @operator, ILConstruct input, Action<TracingILGenerator, ILConstruct> operation )
				: this( @operator, input, operation, ( il, i, @else ) => BranchWithOperationResult( i, operation, il, @else ) )
			{
			}

			public UnaryOperatorILConstruct( string @operator, ILConstruct input, Action<TracingILGenerator, ILConstruct> operation, Action<TracingILGenerator, ILConstruct, Label> branchOperation )
				: base( input.ContextType )
			{
				this._operator = @operator;
				this._input = input;
				this._operation = operation;
				this._branchOperation = branchOperation;
			}

			private static void BranchWithOperationResult( ILConstruct input, Action<TracingILGenerator, ILConstruct> operation, TracingILGenerator il, Label @else )
			{
				operation( il, input );
				il.EmitBrfalse( @else );
			}

			public override void Evaluate( TracingILGenerator il )
			{
				il.TraceWriteLine( "// Eval->: {0}", this );
				this._operation( il, this._input );
				il.TraceWriteLine( "// ->Eval: {0}", this );
			}

			public override void LoadValue( TracingILGenerator il, bool shouldBeAddress )
			{
				il.TraceWriteLine( "// Stor->: {0}", this );
				this._operation( il, this._input );
				il.TraceWriteLine( "// ->Stor: {0}", this );
			}

			protected override void BranchCore( TracingILGenerator il, Label @else )
			{
				this._branchOperation( il, this._input, @else );
			}

			public override string ToString()
			{
				return String.Format( CultureInfo.InvariantCulture, "UnaryOperator[{0}]: ({1} {2})", this.ContextType, this._operator, this._input );
			}
		}

		private class BinaryOperatorILConstruct : ContextfulILConstruct
		{
			private readonly string _operator;
			private readonly ILConstruct _left;
			private readonly ILConstruct _right;
			private readonly Action<TracingILGenerator, ILConstruct, ILConstruct> _operation;
			private readonly Action<TracingILGenerator, ILConstruct, ILConstruct, Label> _branchOperation;

			public BinaryOperatorILConstruct( string @operator, Type resultType, ILConstruct left, ILConstruct right, Action<TracingILGenerator, ILConstruct, ILConstruct> operation )
				: this( @operator, resultType, left, right, operation, ( il, l, r, @else ) => BranchWithOperationResult( l, r, operation, il, @else ) )
			{
			}

			public BinaryOperatorILConstruct( string @operator, Type resultType, ILConstruct left, ILConstruct right, Action<TracingILGenerator, ILConstruct, ILConstruct> operation, Action<TracingILGenerator, ILConstruct, ILConstruct, Label> branchOperation )
				: base( resultType )
			{
				ValidateContextTypeMatch( left, right );
				this._operator = @operator;
				this._left = left;
				this._right = right;
				this._operation = operation;
				this._branchOperation = branchOperation;
			}

			public override void Evaluate( TracingILGenerator il )
			{
				il.TraceWriteLine( "// Eval->: {0}", this );
				this._operation( il, this._left, this._right );
				il.TraceWriteLine( "// ->Eval: {0}", this );
			}

			public override void LoadValue( TracingILGenerator il, bool shouldBeAddress )
			{
				il.TraceWriteLine( "// Load->: {0}", this );
				this._operation( il, this._left, this._right );
				il.TraceWriteLine( "// ->Load: {0}", this );
			}

			private static void BranchWithOperationResult( ILConstruct left, ILConstruct right, Action<TracingILGenerator, ILConstruct, ILConstruct> operation, TracingILGenerator il, Label @else )
			{
				operation( il, left, right );
				il.EmitBrfalse( @else );
			}

			protected override void BranchCore( TracingILGenerator il, Label @else )
			{
				this._branchOperation( il, this._left, this._right, @else );
			}

			public override string ToString()
			{
				return String.Format( CultureInfo.InvariantCulture, "BinaryOperator[{0}]: ({2} {1} {3})", this.ContextType, this._operator, this._left, this._right );
			}
		}

		private class InvocationILConsruct : ContextfulILConstruct
		{
			private readonly ILConstruct _target;
			private readonly MethodBase _method;
			private readonly IEnumerable<ILConstruct> _arguments;

			public InvocationILConsruct( MethodInfo method, ILConstruct target, IEnumerable<ILConstruct> arguments )
				: base( method.ReturnType )
			{
				if ( method.IsStatic )
				{
					if ( target != null )
					{
						throw new ArgumentException(
							String.Format( CultureInfo.CurrentCulture, "target must be null for static method '{0}'", method )
							);
					}
				}
				else
				{
					if ( target == null )
					{
						throw new ArgumentException(
							String.Format( CultureInfo.CurrentCulture, "target must not be null for instance method '{0}'", method )
							);
					}
				}

				this._method = method;
				this._target = target;
				this._arguments = arguments;
			}

			public InvocationILConsruct( ConstructorInfo ctor, ILConstruct target, IEnumerable<ILConstruct> arguments )
				: base( ctor.DeclaringType )
			{
				if ( ctor.DeclaringType.GetIsValueType() )
				{
					if ( target == null )
					{
						throw new ArgumentException(
							String.Format( CultureInfo.CurrentCulture, "target must not be null for expression type constructor '{0}'", ctor )
							);
					}
				}
				else
				{
					if ( target != null )
					{
						throw new ArgumentException(
							String.Format( CultureInfo.CurrentCulture, "target must be null for reference type constructor method '{0}'", ctor )
							);
					}
				}

				this._method = ctor;
				this._target = target;
				this._arguments = arguments;
			}

			public override void Evaluate( TracingILGenerator il )
			{
				il.TraceWriteLine( "// Eval->: {0}", this );
				this.Invoke( il );
				il.TraceWriteLine( "// ->Eval: {0}", this );
			}

			public override void LoadValue( TracingILGenerator il, bool shouldBeAddress )
			{
				il.TraceWriteLine( "// Load->: {0}", this );
				this.Invoke( il );
				il.TraceWriteLine( "// ->Load: {0}", this );
			}

			public override void StoreValue( TracingILGenerator il )
			{
				il.TraceWriteLine( "// Stor->: {0}", this );
				this.Invoke( il );
				il.TraceWriteLine( "// ->Stor: {0}", this );
			}

			private void Invoke( TracingILGenerator il )
			{
				ConstructorInfo asConsctructor;
				if ( ( asConsctructor = this._method as ConstructorInfo ) != null )
				{
					if ( asConsctructor.DeclaringType.GetIsValueType() )
					{
						this._target.LoadValue( il, true );
						foreach ( var argument in this._arguments )
						{
							argument.LoadValue( il, false );
						}

						il.EmitCallConstructor( asConsctructor );
					}
					else
					{
						foreach ( var argument in this._arguments )
						{
							argument.LoadValue( il, false );
						}

						il.EmitNewobj( asConsctructor );
					}
				}
				else
				{
					// method
					if ( !this._method.IsStatic )
					{
						this._target.LoadValue( il, this._target.ContextType.GetIsValueType() );
					}

					foreach ( var argument in this._arguments )
					{
						argument.LoadValue( il, false );
					}

					if ( this._method.IsStatic || this._target.ContextType.GetIsValueType() )
					{
						il.EmitCall( this._method as MethodInfo );
					}
					else
					{
						il.EmitCallvirt( this._method as MethodInfo );
					}
				}
			}

			public override string ToString()
			{
				return String.Format( CultureInfo.InvariantCulture, "Invoke[{0}]: {1}", this.ContextType, this._method );
			}
		}

		private class LoadFieldILConstruct : ContextfulILConstruct
		{
			private readonly ILConstruct _instance;
			private readonly FieldInfo _field;

			public LoadFieldILConstruct( ILConstruct instance, FieldInfo field )
				: base( field.FieldType )
			{
				this._instance = instance;
				this._field = field;
			}

			public override void LoadValue( TracingILGenerator il, bool shouldBeAddress )
			{
				il.TraceWriteLine( "// Load->: {0}", this );
				if ( this._instance != null )
				{
					this._instance.LoadValue( il, this._instance.ContextType.GetIsValueType() );
				}

				if ( shouldBeAddress )
				{
					il.EmitLdflda( this._field );
				}
				else
				{
					il.EmitLdfld( this._field );
				}
				il.TraceWriteLine( "// ->Load: {0}", this );
			}

			public override string ToString()
			{
				return String.Format( CultureInfo.InvariantCulture, "LoadField[{0}]: {1}", this.ContextType, this._field );
			}
		}

		private class StoreFieldILConstruct : ContextfulILConstruct
		{
			private readonly ILConstruct _instance;
			private readonly ILConstruct _value;
			private readonly FieldInfo _field;

			public StoreFieldILConstruct( ILConstruct instance, FieldInfo field, ILConstruct value )
				: base( typeof( void ) )
			{
				this._instance = instance;
				this._field = field;
				this._value = value;
			}

			public override void Evaluate( TracingILGenerator il )
			{
				this.StoreValue( il );
			}

			public override void StoreValue( TracingILGenerator il )
			{
				il.TraceWriteLine( "// Stor->: {0}", this );
				if ( this._instance != null )
				{
					this._instance.LoadValue( il, this._instance.ContextType.GetIsValueType() );
				}

				this._value.LoadValue( il, false );

				il.EmitStfld( this._field );
				il.TraceWriteLine( "// ->Stor: {0}", this );
			}

			public override string ToString()
			{
				return String.Format( CultureInfo.InvariantCulture, "StoreField[void]: {0}", this._field );
			}
		}

		private sealed class NopILConstruct : ILConstruct
		{
			public NopILConstruct( Type contextType )
				: base( contextType ) { }

			public override void Evaluate( TracingILGenerator il )
			{
				// nop
			}

			public override void LoadValue( TracingILGenerator il, bool shouldBeAddress )
			{
				// nop
			}

			public override void StoreValue( TracingILGenerator il )
			{
				// nop
			}

			public override string ToString()
			{
				return "(nop)";
			}
		}

		private sealed class ConditionalILConstruct : ILConstruct
		{
			private readonly ILConstruct _condition;
			private readonly ILConstruct _thenExpression;
			private readonly ILConstruct _elseExpression;

			public ConditionalILConstruct( ILConstruct condition, ILConstruct thenExpression, ILConstruct elseExpression )
				: base( thenExpression.ContextType )
			{
				if ( condition.ContextType != typeof( bool ) )
				{
					throw new ArgumentException( String.Format( CultureInfo.CurrentCulture, "condition must be boolean: {0}", condition ), "condition" );
				}

				if ( elseExpression != null && elseExpression.ContextType != thenExpression.ContextType )
				{
					throw new ArgumentException( String.Format( CultureInfo.CurrentCulture, "elseExpression type must be '{0}' but '{1}':{2}", thenExpression.ContextType, elseExpression.ContextType, elseExpression ), "elseExpression" );
				}

				this._condition = condition;
				this._thenExpression = thenExpression;
				this._elseExpression = elseExpression;
			}

			public override void Evaluate( TracingILGenerator il )
			{
				il.TraceWriteLine( "// Eval->: {0}", this );
				this.DoConditionalInstruction(
					il,
					() => this._thenExpression.Evaluate( il ),
					() => this._elseExpression.Evaluate( il )
				);
				il.TraceWriteLine( "// ->Eval: {0}", this );
			}

			public override void LoadValue( TracingILGenerator il, bool shouldBeAddress )
			{
				il.TraceWriteLine( "// Load->: {0}", this );
				this.DoConditionalInstruction(
					il,
					() => this._thenExpression.LoadValue( il, shouldBeAddress ),
					() => this._elseExpression.LoadValue( il, shouldBeAddress )
				);
				il.TraceWriteLine( "// ->Load: {0}", this );
			}

			public override void StoreValue( TracingILGenerator il )
			{
				il.TraceWriteLine( "// Stor->: {0}", this );
				this.DoConditionalInstruction(
					il,
					() => this._thenExpression.StoreValue( il ),
					() => this._elseExpression.StoreValue( il )
				);
				il.TraceWriteLine( "// ->Stor: {0}", this );
			}

			private void DoConditionalInstruction(
				TracingILGenerator il, Action onThen, Action onElse
			)
			{
				if ( this._elseExpression != null )
				{
					var @else = il.DefineLabel( "ELSE" );
					var endIf = il.DefineLabel( "END_IF" );
					this._condition.Branch( il, @else );
					onThen();
					if ( !this._thenExpression.IsTerminating )
					{
						il.EmitBr( endIf );
					}

					il.MarkLabel( @else );
					onElse();
					il.MarkLabel( endIf );
				}
				else
				{
					var endIf = il.DefineLabel( "END_IF" );
					this._condition.Branch( il, endIf );
					onThen();
					il.MarkLabel( endIf );
				}
			}

			public override string ToString()
			{
				return
					String.Format(
						CultureInfo.InvariantCulture,
						"Condition[{0}]: ({1}) ? ({2}) : ({3})",
						this.ContextType,
						this._condition,
						this._thenExpression,
						this._elseExpression
					);
			}
		}

		private sealed class AndConditionILConstruct : ILConstruct
		{
			private readonly IList<ILConstruct> _expressions;

			public AndConditionILConstruct( IList<ILConstruct> expressions )
				: base( typeof( bool ) )
			{
				if ( expressions.Count == 0 )
				{
					throw new ArgumentException( "Empty expressions.", "expressions" );
				}

				if ( expressions.Any( c => c.ContextType != typeof( bool ) ) )
				{
					throw new ArgumentException( "An argument expressions cannot contains non boolean expression.", "expressions" );
				}

				this._expressions = expressions;
			}

			public override void Evaluate( TracingILGenerator il )
			{
				il.TraceWriteLine( "// Eval->: {0}", this );
				this.EvaluateCore( il );
				il.TraceWriteLine( "// ->Eval: {0}", this );
			}

			public override void LoadValue( TracingILGenerator il, bool shouldBeAddress )
			{
				il.TraceWriteLine( "// Load->: {0}", this );
				this.EvaluateCore( il );
				il.TraceWriteLine( "// ->Load: {0}", this );
			}

			private void EvaluateCore( TracingILGenerator il )
			{
				for ( int i = 0; i < this._expressions.Count; i++ )
				{
					this._expressions[ i ].LoadValue( il, false );

					if ( i > 0 )
					{
						il.EmitAnd();
					}
				}
			}

			public override void Branch( TracingILGenerator il, Label @else )
			{
				il.TraceWriteLine( "// Brnc->: {0}", this );
				foreach ( var expression in this._expressions )
				{
					expression.LoadValue( il, false );
					il.EmitBrfalse( @else );
				}

				il.TraceWriteLine( "// ->Brnc: {0}", this );
			}

			public override string ToString()
			{
				return
					String.Format(
					// ReSharper disable CoVariantArrayConversion
						CultureInfo.InvariantCulture, "And[{0}]: ({1})", this.ContextType, String.Join( ", ", this._expressions.ToArray() as object[] )
					// ReSharper restore CoVariantArrayConversion
					);
			}
		}
	}
}