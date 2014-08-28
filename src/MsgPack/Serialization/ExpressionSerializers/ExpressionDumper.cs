#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2012 FUJIWARA, Yusuke
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
using System.Globalization;
using System.IO;
using System.Linq.Expressions;
#if NETFX_CORE
using  System.Reflection;
#endif

namespace MsgPack.Serialization.ExpressionSerializers
{
#if !NETFX_35
	/// <summary>
	///		Takes text dump for expression tree supporting block expression etc.
	/// </summary>
	internal class ExpressionDumper : ExpressionVisitor
	{
		private int _indentLevel;
		private readonly TextWriter _writer;

		public ExpressionDumper( TextWriter writer, int indentLevel )
		{
			this._writer = writer;
			this._indentLevel = indentLevel;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "Switch for many enum members" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
		protected override Expression VisitBinary( BinaryExpression node )
		{
			bool isChecked = false;

			switch ( node.NodeType )
			{
				case ExpressionType.AddAssignChecked:
				case ExpressionType.AddChecked:
				case ExpressionType.ConvertChecked:
				case ExpressionType.MultiplyAssignChecked:
				case ExpressionType.MultiplyChecked:
				case ExpressionType.SubtractAssignChecked:
				case ExpressionType.SubtractChecked:
				{
					isChecked = true;
					break;
				}
			}

			if ( isChecked )
			{
				this._writer.Write( "checked( " );
			}

			this.Visit( node.Left );

			switch ( node.NodeType )
			{
				case ExpressionType.Add:
				case ExpressionType.AddChecked:
				{
					this._writer.Write( " + " );
					break;
				}
				case ExpressionType.AddAssign:
				case ExpressionType.AddAssignChecked:
				{
					this._writer.Write( " += " );
					break;
				}
				case ExpressionType.And:
				{
					this._writer.Write( " & " );
					break;
				}
				case ExpressionType.AndAlso:
				{
					this._writer.Write( " && " );
					break;
				}
				case ExpressionType.AndAssign:
				{
					this._writer.Write( " &= " );
					break;
				}
				case ExpressionType.Assign:
				{
					this._writer.Write( " = " );
					break;
				}
				case ExpressionType.ArrayIndex:
				{
					this._writer.Write( "[ " );
					break;
				}
				case ExpressionType.Coalesce:
				{
					this._writer.Write( " ?? " );
					break;
				}
				case ExpressionType.Divide:
				{
					this._writer.Write( " / " );
					break;
				}
				case ExpressionType.DivideAssign:
				{
					this._writer.Write( " /= " );
					break;
				}
				case ExpressionType.Equal:
				{
					this._writer.Write( " == " );
					break;
				}
				case ExpressionType.ExclusiveOr:
				{
					this._writer.Write( " ^ " );
					break;
				}
				case ExpressionType.ExclusiveOrAssign:
				{
					this._writer.Write( " ^= " );
					break;
				}
				case ExpressionType.GreaterThan:
				{
					this._writer.Write( " > " );
					break;
				}
				case ExpressionType.GreaterThanOrEqual:
				{
					this._writer.Write( " >= " );
					break;
				}
				case ExpressionType.LeftShift:
				{
					this._writer.Write( " << " );
					break;
				}
				case ExpressionType.LeftShiftAssign:
				{
					this._writer.Write( " <<= " );
					break;
				}
				case ExpressionType.LessThan:
				{
					this._writer.Write( " < " );
					break;
				}
				case ExpressionType.LessThanOrEqual:
				{
					this._writer.Write( " <= " );
					break;
				}
				case ExpressionType.Modulo:
				{
					this._writer.Write( " % " );
					break;
				}
				case ExpressionType.ModuloAssign:
				{
					this._writer.Write( " %= " );
					break;
				}
				case ExpressionType.Multiply:
				case ExpressionType.MultiplyChecked:
				{
					this._writer.Write( " * " );
					break;
				}
				case ExpressionType.MultiplyAssign:
				case ExpressionType.MultiplyAssignChecked:
				{
					this._writer.Write( " *= " );
					break;
				}
				case ExpressionType.NotEqual:
				{
					this._writer.Write( " != " );
					break;
				}
				case ExpressionType.Or:
				{
					this._writer.Write( " | " );
					break;
				}
				case ExpressionType.OrAssign:
				{
					this._writer.Write( " |= " );
					break;
				}
				case ExpressionType.OrElse:
				{
					this._writer.Write( " || " );
					break;
				}
				case ExpressionType.Power:
				{
					this._writer.Write( " `pow` " );
					break;
				}
				case ExpressionType.PowerAssign:
				{
					this._writer.Write( " `pow`= " );
					break;
				}
				case ExpressionType.RightShift:
				{
					this._writer.Write( " >> " );
					break;
				}
				case ExpressionType.RightShiftAssign:
				{
					this._writer.Write( " >>= " );
					break;
				}
				case ExpressionType.Subtract:
				case ExpressionType.SubtractChecked:
				{
					this._writer.Write( " - " );
					break;
				}
				case ExpressionType.SubtractAssign:
				case ExpressionType.SubtractAssignChecked:
				{
					this._writer.Write( " -= " );
					break;
				}
				default:
				{
					throw new NotSupportedException( String.Format( CultureInfo.CurrentCulture, "Binary operation {0}(NodeType:{1}) is not supported. Expression tree:{2}", node.GetType().Name, node.NodeType, node ) );
				}
			}

			this.Visit( node.Right );

			if ( isChecked )
			{
				this._writer.Write( " )" );
			}
			else if ( node.NodeType == ExpressionType.ArrayIndex )
			{
				this._writer.Write( " ]" );
			}

			return node;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
		protected override Expression VisitBlock( BlockExpression node )
		{
			this.WriteIndent();
			this._writer.WriteLine( '{' );

			this._indentLevel++;
			try
			{
				if ( node.Variables.Count > 0 )
				{
					this.WriteIndent();
					this._writer.Write( "var : [" );

					this._indentLevel++;
					try
					{
						for ( int i = 0; i < node.Variables.Count; i++ )
						{
							if ( i == 0 )
							{
								this._writer.WriteLine();
							}
							else
							{
								this._writer.WriteLine( ',' );
							}

							this.WriteIndent();
							this._writer.Write( node.Variables[ i ].Name );
							this._writer.Write( " : " );
							this._writer.Write( node.Variables[ i ].Type );
						}
					}
					finally
					{
						this._indentLevel--;
					}

					this._writer.WriteLine();
					this.WriteIndent();
					this._writer.Write( " ]" );
					this._writer.WriteLine();
				}

				for ( int i = 0; i < node.Expressions.Count; i++ )
				{
					if ( i > 0 )
					{
						this._writer.WriteLine( ';' );
					}

					this.WriteIndent();
					this.Visit( node.Expressions[ i ] );

					if ( i == node.Expressions.Count - 1 )
					{
						this._writer.WriteLine();
					}
				}
			}
			finally
			{
				this._indentLevel--;
			}

			this.WriteIndent();
			this._writer.Write( "} -> " );
			this._writer.WriteLine( node.Type );
			this.WriteIndent();

			return node;
		}

		protected override CatchBlock VisitCatchBlock( CatchBlock node )
		{
			this.ThrowUnsupportedNodeException( node );
			return base.VisitCatchBlock( node );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
		protected override Expression VisitConditional( ConditionalExpression node )
		{
			if ( node.Type != typeof( void ) )
			{
				this.VisitConditionalExpression( node );
			}
			else if ( node.IfFalse is DefaultExpression )
			{
				this.VisitIfThenStatement( node );
			}
			else
			{
				this.VisitIfThenElseStatement( node );
			}

			return node;
		}

		private void VisitIfThenStatement( ConditionalExpression node )
		{
			this._writer.Write( "if (" );
			this.Visit( node.Test );
			this._writer.WriteLine( ") {" );

			this._indentLevel++;
			try
			{
				this.WriteIndent();
				this.Visit( node.IfTrue );
			}
			finally
			{
				this._indentLevel--;
			}

			this._writer.WriteLine();
			this.WriteIndent();
			this._writer.Write( "}" );
		}

		private void VisitIfThenElseStatement( ConditionalExpression node )
		{
			this._writer.Write( "if (" );
			this.Visit( node.Test );
			this._writer.WriteLine( ") {" );

			this._indentLevel++;
			try
			{
				this.WriteIndent();
				this.Visit( node.IfTrue );
			}
			finally
			{
				this._indentLevel--;
			}

			this._writer.WriteLine();
			this.WriteIndent();
			this._writer.WriteLine( "}" );
			this.WriteIndent();
			this._writer.WriteLine( "else" );
			this.WriteIndent();
			this._writer.WriteLine( "{" );

			this._indentLevel++;
			try
			{
				this.WriteIndent();
				this.Visit( node.IfFalse );
			}
			finally
			{
				this._indentLevel--;
			}

			this._writer.WriteLine();
			this.WriteIndent();
			this._writer.Write( "}" );
		}

		private void VisitConditionalExpression( ConditionalExpression node )
		{
			this._writer.WriteLine( '(' );

			this._indentLevel++;
			try
			{
				this.WriteIndent();
				this.Visit( node.Test );
			}
			finally
			{
				this._indentLevel--;
			}

			this._writer.WriteLine();
			this.WriteIndent();
			this._writer.WriteLine( ") ? (" );

			this._indentLevel++;
			try
			{
				this.WriteIndent();
				this.Visit( node.IfTrue );
			}
			finally
			{
				this._indentLevel--;
			}

			this._writer.WriteLine();
			this.WriteIndent();
			this._writer.WriteLine( ") : (" );

			this._indentLevel++;
			try
			{
				this.WriteIndent();
				this.Visit( node.IfFalse );
			}
			finally
			{
				this._indentLevel--;
			}

			this._writer.WriteLine();
			this.WriteIndent();
			this._writer.Write( " -> " );
			this._writer.Write( node.Type );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
		protected override Expression VisitConstant( ConstantExpression node )
		{
			this._writer.Write( node.ToString() );
			return node;
		}

		protected override Expression VisitDebugInfo( DebugInfoExpression node )
		{
			this.ThrowUnsupportedNodeException( node );

			return base.VisitDebugInfo( node );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
		protected override Expression VisitDefault( DefaultExpression node )
		{
			this._writer.Write( node.ToString() );
			return node;
		}

#if !NETFX_CORE && !WINDOWS_PHONE
		protected override Expression VisitDynamic( DynamicExpression node )
		{
			this.ThrowUnsupportedNodeException( node );
			return base.VisitDynamic( node );
		}
#endif

		protected override ElementInit VisitElementInit( ElementInit node )
		{
			this.ThrowUnsupportedNodeException( node );
			return base.VisitElementInit( node );
		}

		protected override Expression VisitExtension( Expression node )
		{
			this.ThrowUnsupportedNodeException( node );
			return base.VisitExtension( node );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
		protected override Expression VisitGoto( GotoExpression node )
		{
			this._writer.Write( node.ToString() );
			return node;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
		protected override Expression VisitIndex( IndexExpression node )
		{
			this.Visit( node.Object );
			this._writer.Write( "[ " );

			this._indentLevel++;
			try
			{
				for ( int i = 0; i < node.Arguments.Count; i++ )
				{
					if ( i > 0 )
					{
						this._writer.Write( ", " );
					}

					this.Visit( node.Arguments[ i ] );
				}
			}
			finally
			{
				this._indentLevel--;
			}

			this._writer.Write( " ]" );

			return node;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
		protected override Expression VisitInvocation( InvocationExpression node )
		{
			this.Visit( node.Expression );

			this._writer.Write( '(' );

			if ( node.Arguments.Count > 0 )
			{
				this._writer.WriteLine();

				this._indentLevel++;
				try
				{
					for ( int i = 0; i < node.Arguments.Count; i++ )
					{
						if ( i > 0 )
						{
							this._writer.WriteLine( ',' );
						}

						this.WriteIndent();
						this.Visit( node.Arguments[ i ] );
					}
				}
				finally
				{
					this._indentLevel--;
				}

				this._writer.WriteLine();
				this.WriteIndent();
			}

			this._writer.Write( ") -> " );
			this._writer.Write( node.Type );

			return node;
		}

		protected override Expression VisitLabel( LabelExpression node )
		{
			this._writer.WriteLine();
			this._writer.WriteLine( node );
			this.WriteIndent();
			return node;
		}

		protected override LabelTarget VisitLabelTarget( LabelTarget node )
		{
			this.ThrowUnsupportedNodeException( node );
			return base.VisitLabelTarget( node );
		}

		protected override Expression VisitListInit( ListInitExpression node )
		{
			this.ThrowUnsupportedNodeException( node );
			return base.VisitListInit( node );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
		protected override Expression VisitLambda<T>( Expression<T> node )
		{
			this._writer.Write( '(' );

			if ( node.Parameters.Count > 0 )
			{
				this._writer.WriteLine();

				this._indentLevel++;
				try
				{

					for ( int i = 0; i < node.Parameters.Count; i++ )
					{
						if ( i > 0 )
						{
							this._writer.WriteLine( ',' );
						}

						this.WriteIndent();
						this._writer.Write( node.Parameters[ i ].Name );
						this._writer.Write( " : " );
						this._writer.Write( node.Parameters[ i ].Type );
					}
				}
				finally
				{
					this._indentLevel--;
				}

				this._writer.WriteLine();
				this.WriteIndent();
			}

			this._writer.Write( ") => (" );

			this._indentLevel++;
			try
			{
				this.Visit( node.Body );
			}
			finally
			{
				this._indentLevel--;
			}

			this._writer.Write( ") -> " );
			this._writer.Write( node.ReturnType );

			return node;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
		protected override Expression VisitLoop( LoopExpression node )
		{
			this._writer.WriteLine();
			this.WriteIndent();
			this._writer.Write( "loop {" );
			// TODO: continue/break...

			this._indentLevel++;
			try
			{
				this.WriteIndent();
				this.Visit( node.Body );
			}
			finally
			{
				this._indentLevel--;
			}

			this._writer.Write( "} -> " );
			this._writer.WriteLine( node.Type );

			return node;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
		protected override Expression VisitMember( MemberExpression node )
		{
			this.Visit( node.Expression );
			this._writer.Write( '.' );
			this._writer.Write( node.Member.Name );

			return node;
		}

		protected override MemberAssignment VisitMemberAssignment( MemberAssignment node )
		{
			this.ThrowUnsupportedNodeException( node );
			return base.VisitMemberAssignment( node );
		}

		protected override MemberBinding VisitMemberBinding( MemberBinding node )
		{
			this.ThrowUnsupportedNodeException( node );
			return base.VisitMemberBinding( node );
		}

		protected override Expression VisitMemberInit( MemberInitExpression node )
		{
			this.ThrowUnsupportedNodeException( node );
			return base.VisitMemberInit( node );
		}

		protected override MemberListBinding VisitMemberListBinding( MemberListBinding node )
		{
			this.ThrowUnsupportedNodeException( node );
			return base.VisitMemberListBinding( node );
		}

		protected override MemberMemberBinding VisitMemberMemberBinding( MemberMemberBinding node )
		{
			this.ThrowUnsupportedNodeException( node );
			return base.VisitMemberMemberBinding( node );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
		protected override Expression VisitMethodCall( MethodCallExpression node )
		{
			if ( node.Object == null )
			{
				if ( node.Method.DeclaringType != null )
				{
					this._writer.Write( node.Method.DeclaringType.Name );
				}
			}
			else
			{
				this.Visit( node.Object );
			}

			this._writer.Write( '.' );
			this._writer.Write( node.Method.Name );
			this._writer.Write( '(' );

			if ( node.Arguments.Count > 0 )
			{
				this._writer.WriteLine();

				this._indentLevel++;
				try
				{
					for ( int i = 0; i < node.Arguments.Count; i++ )
					{
						if ( i > 0 )
						{
							this._writer.WriteLine( ',' );
						}

						this.WriteIndent();
						this.Visit( node.Arguments[ i ] );
					}
				}
				finally
				{
					this._indentLevel--;
				}

				this._writer.WriteLine();
				this.WriteIndent();
			}

			this._writer.Write( ") -> " );
			this._writer.Write( node.Type );

			return node;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
		protected override Expression VisitNew( NewExpression node )
		{
			this._writer.Write( "new " );
			this._writer.Write( node.Type );
			this._writer.Write( "( " );
			if ( node.Arguments.Count > 0 )
			{
				this._writer.WriteLine();
				this._indentLevel++;
				for ( int i = 0; i < node.Arguments.Count; i++ )
				{
					if ( i > 0 )
					{
						this._writer.WriteLine( ',' );
					}

					this.WriteIndent();
					this.Visit( node.Arguments[ i ] );
				}
				this._indentLevel--;
			}

			this._writer.Write( ") " );

			return node;
		}

		protected override Expression VisitNewArray( NewArrayExpression node )
		{
			this._writer.Write( node );
			return node;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
		protected override Expression VisitParameter( ParameterExpression node )
		{
			this._writer.Write( node.ToString() );
			return node;
		}

		protected override Expression VisitRuntimeVariables( RuntimeVariablesExpression node )
		{
			this.ThrowUnsupportedNodeException( node );
			return base.VisitRuntimeVariables( node );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
		protected override Expression VisitSwitch( SwitchExpression node )
		{
			this._writer.Write( "switch ( " );
			this._writer.Write( node.Comparison );
			this._writer.WriteLine( " ) {" );
			foreach ( var @case in node.Cases )
			{
				this.VisitSwitchCase( @case );
			}

			if ( node.DefaultBody != null )
			{
				this._writer.WriteLine( "default: " );
				this._writer.WriteLine( " ):" );
				this._indentLevel++;
				this.Visit( node.DefaultBody );
				this._indentLevel--;
			}

			this._writer.WriteLine( "}" );
			return base.VisitSwitch( node );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
		protected override SwitchCase VisitSwitchCase( SwitchCase node )
		{
			this._writer.Write( "case ( " );
			this.Visit( node.TestValues );
			this._writer.WriteLine( " ):" );
			this._indentLevel++;
			this.Visit( node.Body );
			this._indentLevel--;

			return node;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
		protected override Expression VisitTry( TryExpression node )
		{
			this._writer.WriteLine( "try (" );

			if ( node.Body != null )
			{
				this._indentLevel++;
				try
				{
					this.WriteIndent();
					this.Visit( node.Body );
				}
				finally
				{
					this._indentLevel--;
				}

				this._writer.WriteLine();
				this.WriteIndent();
			}

			this._writer.Write( ')' );
			foreach ( var handler in node.Handlers )
			{
				this.WriteIndent();
				this._writer.Write( " catch[ " );
				this._writer.Write( handler.Variable.Name );
				this._writer.Write( " : " );
				this._writer.Write( handler.Test );
				this._writer.Write( " ] " );

				if ( handler.Filter != null )
				{
					this._writer.WriteLine( "when (" );
					this._indentLevel++;
					try
					{
						this.WriteIndent();
						this.Visit( handler.Filter );
						this._writer.WriteLine();
					}
					finally
					{
						this._indentLevel--;
					}

					this.WriteIndent();
					this._writer.Write( ") handler (" );
				}

				this._writer.WriteLine( '(' );

				this._indentLevel++;
				try
				{
					this.WriteIndent();
					this.Visit( handler.Body );
					this._writer.WriteLine();
				}
				finally
				{
					this._indentLevel--;
				}

				this.WriteIndent();
				this._writer.Write( ')' );
			}

			if ( node.Finally != null )
			{
				this._writer.WriteLine( " finally (" );
				this._indentLevel++;
				try
				{
					this.WriteIndent();
					this.Visit( node.Finally );
					this._writer.WriteLine();
				}
				finally
				{
					this._indentLevel--;
				}

				this.WriteIndent();
				this._writer.Write( ')' );
			}

			if ( node.Fault != null )
			{
				this._writer.WriteLine( " fault (" );
				this._indentLevel++;
				try
				{
					this.WriteIndent();
					this.Visit( node.Fault );
					this._writer.WriteLine();
				}
				finally
				{
					this._indentLevel--;
				}

				this.WriteIndent();
				this._writer.Write( ')' );
			}

			return node;
		}

		protected override Expression VisitTypeBinary( TypeBinaryExpression node )
		{
			this.ThrowUnsupportedNodeException( node );
			return base.VisitTypeBinary( node );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "Switch for many enum members" )]
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "0", Justification = "By design" )]
		protected override Expression VisitUnary( UnaryExpression node )
		{
			switch ( node.NodeType )
			{
				case ExpressionType.Convert:
				case ExpressionType.Unbox:
				{
					this._writer.Write( "( " );
					this._writer.Write( node.Type );
					this._writer.Write( " )" );
					break;
				}
				case ExpressionType.ConvertChecked:
				{
					this._writer.Write( "checked( ( " );
					this._writer.Write( node.Type );
					this._writer.Write( " )" );
					break;
				}
				case ExpressionType.Decrement:
				{
					this._writer.Write( "`decr`( " );
					break;
				}
				case ExpressionType.Increment:
				{
					this._writer.Write( "`incr`( " );
					break;
				}
				case ExpressionType.IsFalse:
				{
					this._writer.Write( "`false`( " );
					break;
				}
				case ExpressionType.IsTrue:
				{
					this._writer.Write( "`true`( " );
					break;
				}
				case ExpressionType.Negate:
				{
					this._writer.Write( "( -( " );
					break;
				}
				case ExpressionType.NegateChecked:
				{
					this._writer.Write( "checked( -( " );
					break;
				}
				case ExpressionType.OnesComplement:
				{
					this._writer.Write( "~(" );
					break;
				}
				case ExpressionType.PreDecrementAssign:
				{
					this._writer.Write( "--(" );
					break;
				}
				case ExpressionType.PreIncrementAssign:
				{
					this._writer.Write( "++(" );
					break;
				}
				case ExpressionType.Throw:
				{
					this._writer.Write( "throw ( " );
					break;
				}
				case ExpressionType.UnaryPlus:
				{
					this._writer.Write( "( +( " );
					break;
				}
				case ExpressionType.Not:
				{
					this._writer.Write( "!( " );
					break;
				}
				case ExpressionType.ArrayLength:
				case ExpressionType.PostDecrementAssign:
				case ExpressionType.PostIncrementAssign:
				case ExpressionType.TypeAs:
				case ExpressionType.TypeIs:
				{
					this._writer.Write( "( " );
					break;
				}
				default:
				{
					throw new NotSupportedException( String.Format( CultureInfo.CurrentCulture, "Unary operation {0}(NodeType:{1}) is not supported. Expression tree:{2}", node.GetType().Name, node.NodeType, node ) );
				}
			}

			this.Visit( node.Operand );

			switch ( node.NodeType )
			{
				case ExpressionType.ArrayLength:
				{
					this._writer.Write( " ).Length" );
					break;
				}
				case ExpressionType.ConvertChecked:
				case ExpressionType.Decrement:
				case ExpressionType.Increment:
				case ExpressionType.IsFalse:
				case ExpressionType.IsTrue:
				case ExpressionType.Not:
				case ExpressionType.OnesComplement:
				case ExpressionType.PreDecrementAssign:
				case ExpressionType.PreIncrementAssign:
				case ExpressionType.Throw:
				{
					this._writer.Write( " )" );
					break;
				}
				case ExpressionType.Negate:
				case ExpressionType.NegateChecked:
				case ExpressionType.UnaryPlus:
				{
					this._writer.Write( " ) )" );
					break;
				}
				case ExpressionType.PostDecrementAssign:
				{
					this._writer.Write( " )--" );
					break;
				}
				case ExpressionType.PostIncrementAssign:
				{
					this._writer.Write( " )++" );
					break;
				}
				case ExpressionType.TypeAs:
				{
					this._writer.Write( ") as " );
					this._writer.Write( node.Type );
					break;
				}
				case ExpressionType.TypeIs:
				{
					this._writer.Write( ") is " );
					this._writer.Write( node.Type );
					break;
				}
			}

			return node;
		}

		private void ThrowUnsupportedNodeException( Expression node )
		{
#if DEBUG
#if !NETFX_CORE
			Type expressionType = node.GetType();
			while ( expressionType != null && expressionType.IsNotPublic )
			{
				expressionType = expressionType.BaseType;
			}
#else
			TypeInfo expressionType = node.GetType().GetTypeInfo();
			while ( expressionType.IsNotPublic )
			{
				expressionType = expressionType.BaseType.GetTypeInfo();
			}
#endif

			throw new NotImplementedException( String.Format( CultureInfo.CurrentCulture, "{0}(NodeType:{1})", expressionType, node.NodeType ) );
#else
			this._writer.Write( node );
#endif
		}

		private void ThrowUnsupportedNodeException<T>( T node )
		{
#if DEBUG
			throw new NotImplementedException( String.Format( CultureInfo.CurrentCulture, "{0}({1}) is not supported yet.", typeof( T ).Name, node.GetType() ) );
#else
			this._writer.Write( node );
#endif
		}

		private void WriteIndent()
		{
			WriteIndent( this._writer, this._indentLevel );
		}

		internal static void WriteIndent( TextWriter writer, int indentLevel )
		{
			for ( int i = 0; i < indentLevel; i++ )
			{
				writer.Write( ' ' );
				writer.Write( ' ' );
			}
		}
	}
#endif
}
