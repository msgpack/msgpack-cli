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
using System.Diagnostics.Contracts;
using System.Linq.Expressions;

using MsgPack.Serialization.AbstractSerializers;

namespace MsgPack.Serialization.ExpressionSerializers
{
	/// <summary>
	///		Represents a code construct for <see cref="ExpressionTreeSerializerBuilder{TObject}"/>.
	/// </summary>
	internal sealed class ExpressionConstruct : ICodeConstruct
	{
		private readonly Expression _expression;

		/// <summary>
		///		Gets the <see cref="Expression"/> for this construct.
		/// </summary>
		/// <value>
		///		The <see cref="Expression"/> for this construct.
		///		This value will not be <c>null</c>.
		/// </value>
		public Expression Expression
		{
			get { return this._expression; }
		}

		private readonly bool _isSignificantReference;

		/// <summary>
		///		Gets a value indicating whether this instance is significant reference.
		/// </summary>
		/// <value>
		///		<c>true</c> if this instance is significant reference; otherwise, <c>false</c>.
		/// </value>
		/// <remarks>
		///		'Significant' reference should be appeared in block expression to represent single parameter/variable reference.
		/// </remarks>
		public bool IsSignificantReference
		{
			get { return this._isSignificantReference; }
		}

		/// <summary>
		///		Gets the context type of this construct.
		/// </summary>
		/// <value>
		/// The context type of this construct.
		/// This value will not be <c>null</c>, but might be <see cref="Void" />.
		/// </value>
		/// <remarks>
		///		This property wraps <see cref="System.Linq.Expressions.Expression.Type"/> property for <see cref="Expression"/> property.
		/// </remarks>
		public Type ContextType
		{
			get { return this._expression.Type; }
		}

		/// <summary>
		///		Initializes a new instance of the <see cref="ExpressionConstruct"/> class, not significant reference.
		/// </summary>
		/// <param name="expression">The <see cref="System.Linq.Expressions.Expression"/>.</param>
		public ExpressionConstruct( Expression expression )
			: this( expression, false )
		{
		}

		/// <summary>
		///		Initializes a new instance of the <see cref="ExpressionConstruct"/> class.
		/// </summary>
		/// <param name="expression">The <see cref="System.Linq.Expressions.Expression"/>.</param>
		/// <param name="isSignificantReference">
		///		<c>true</c> if this instance is significant reference; otherwise, <c>false</c>.
		/// </param>
		public ExpressionConstruct( Expression expression, bool isSignificantReference )
		{
#if DEBUG
			Contract.Assert( expression != null );
#endif
			this._expression = expression;
			this._isSignificantReference = isSignificantReference;
		}

		public static implicit operator ExpressionConstruct( Expression expression )
		{
			return expression == null ? null : new ExpressionConstruct( expression );
		}

		public static implicit operator Expression( ExpressionConstruct construct )
		{
			return construct == null ? null : construct.Expression;
		}

		internal void ToString( System.IO.TextWriter textWriter )
		{
			this.ToString( textWriter, 0 );
		}

		private void ToString( System.IO.TextWriter textWriter, int indentLevel )
		{
			new ExpressionDumper( textWriter, indentLevel ).Visit( this.Expression );
		}
	}
}