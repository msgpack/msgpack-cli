#region -- License Terms --
//
// NLiblet
//
// Copyright (C) 2011-2015 FUJIWARA, Yusuke
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
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;

namespace MsgPack.Serialization.Reflection
{
	/// <summary>
	///		<see cref="ILGenerator"/> like IL stream builder with tracing.
	/// </summary>
	internal sealed partial class TracingILGenerator : IDisposable
	{
		private readonly ILGenerator _underlying;
		private readonly TextWriter _realTrace;
		private readonly TextWriter _trace;
		private readonly StringBuilder _traceBuffer;
		private readonly Dictionary<LocalBuilder, string> _localDeclarations = new Dictionary<LocalBuilder, string>();
		private readonly Dictionary<Label, string> _labels = new Dictionary<Label, string>();

		private readonly Label _endOfMethod;

		/// <summary>
		///		Get <see cref="Label"/> for end of method.
		/// </summary>
		/// <value>
		///		<see cref="Label"/> for end of method.
		/// </value>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For assertion" )]
		public Label EndOfMethod
		{
			get { return this._endOfMethod; }
		}

		private readonly Stack<Label> _endOfExceptionBlocks = new Stack<Label>();

#if DEBUG
		/// <summary>
		///		Get <see cref="Label"/> for end of current exception blocks.
		/// </summary>
		/// <value>
		///		<see cref="Label"/> for end of current exception blocks.
		///		When there are no exception blocks, then null.
		/// </value>
		public Label? EndOfCurrentExceptionBlock
		{
			get
			{
				if ( this._endOfExceptionBlocks.Count == 0 )
				{
					return null;
				}
				else
				{
					return this._endOfExceptionBlocks.Peek();
				}
			}
		}

#endif // DEBUG
		/// <summary>
		///		Get whether there are any exception blocks in current positon.
		/// </summary>
		/// <value>
		///		If there are any exception blocks in current positon then <c>true</c>; otherwise, <c>false</c>.
		/// </value>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For assertion" )]
		public bool IsInExceptionBlock
		{
			get { return 0 < this._endOfExceptionBlocks.Count; }
		}

		private bool _isInDynamicMethod;
#if DEBUG
		/// <summary>
		///		Get the value whether this instance used for dynamic method.
		/// </summary>
		/// <value>If  this instance used for dynamic method then <c>true</c>; otherwise <c>false</c>.</value>
		/// <remarks>
		///		Dynamic method does not support debugging information like local variable name.
		/// </remarks>
		public bool IsInDynamicMethod
		{
			get { return this._isInDynamicMethod; }
		}
#endif // DEBUG

		private int _indentLevel;

#if DEBUG
		/// <summary>
		///		Get level of indentation.
		/// </summary>
		public int IndentLevel
		{
			get
			{
				Contract.Ensures( 0 <= Contract.Result<int>() );

				return this._indentLevel;
			}
		}
#endif // DEBUG

		private string _indentChars = "  ";

#if DEBUG
		/// <summary>
		///		Get or set indent characters.
		/// </summary>
		/// <value>
		///		<see cref="String"/> to be used to indent.
		///		To reset default, specify null.
		/// </value>
		public string IndentCharacters
		{
			get
			{
				Contract.Ensures( Contract.Result<string>() != null );

				return this._indentChars;
			}
			set { this._indentChars = value ?? "  "; }
		}
#endif // DEBUG

		private int _lineNumber;

#if DEBUG
		/// <summary>
		///		Get current line number.
		/// </summary>
		/// <value>Current line number.</value>
		public int LineNumber
		{
			get
			{
				Contract.Ensures( 0 <= Contract.Result<int>() );

				return this._lineNumber;
			}
		}
#endif // DEBUG

		private bool _isEnded = false;

		/// <summary>
		///		Get whether this IL stream is ended with 'ret'.
		/// </summary>
		/// <returns>
		///		When this IL stream is ended with 'ret' then <c>true</c>; otherwise, <c>false</c>.
		/// </returns>
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "For assertion" )]
		public bool IsEnded
		{
			get { return this._isEnded; }
		}

		// TODO: NLiblet
		private readonly bool _isDebuggable;

#if DEBUG
#if !WINDOWS_PHONE
		/// <summary>
		///		Initializes a new instance of the <see cref="TracingILGenerator"/> class.
		/// </summary>
		/// <param name="methodBuilder">The method builder.</param>
		/// <param name="traceWriter">The trace writer.</param>
		public TracingILGenerator( MethodBuilder methodBuilder, TextWriter traceWriter )
			: this( methodBuilder != null ? methodBuilder.GetILGenerator() : null, false, traceWriter, false )
		{
			Contract.Assert( methodBuilder != null );
		}
#endif
#endif // DEBUG

		/// <summary>
		/// Initializes a new instance of the <see cref="TracingILGenerator"/> class.
		/// </summary>
		/// <param name="dynamicMethod">The dynamic method.</param>
		/// <param name="traceWriter">The trace writer.</param>
		public TracingILGenerator( DynamicMethod dynamicMethod, TextWriter traceWriter )
			: this( dynamicMethod != null ? dynamicMethod.GetILGenerator() : null, true, traceWriter, false )
		{
			Contract.Assert( dynamicMethod != null );
		}

		// TODO: NLIblet
#if !WINDOWS_PHONE
		/// <summary>
		///		Initializes a new instance of the <see cref="TracingILGenerator"/> class.
		/// </summary>
		/// <param name="methodBuilder">The method builder.</param>
		/// <param name="traceWriter">The trace writer.</param>
		/// <param name="isDebuggable"><c>true</c> if the underlying builders are debuggable; othersie <c>false</c>.</param>
		public TracingILGenerator( MethodBuilder methodBuilder, TextWriter traceWriter, bool isDebuggable )
			: this( methodBuilder != null ? methodBuilder.GetILGenerator() : null, false, traceWriter, isDebuggable )
		{
			Contract.Assert( methodBuilder != null );
		}

		/// <summary>
		///		Initializes a new instance of the <see cref="TracingILGenerator"/> class.
		/// </summary>
		/// <param name="constructorBuilder">The constructor builder.</param>
		/// <param name="traceWriter">The trace writer.</param>
		/// <param name="isDebuggable"><c>true</c> if the underlying builders are debuggable; othersie <c>false</c>.</param>
		public TracingILGenerator( ConstructorBuilder constructorBuilder, TextWriter traceWriter, bool isDebuggable )
			: this( constructorBuilder != null ? constructorBuilder.GetILGenerator() : null, false, traceWriter, isDebuggable )
		{
			Contract.Assert( constructorBuilder != null );
		}

#endif

		// TODO: NLiblet
		private TracingILGenerator( ILGenerator underlying, bool isInDynamicMethod, TextWriter traceWriter, bool isDebuggable )
		{
			this._underlying = underlying;
			this._realTrace = traceWriter ?? TextWriter.Null;
			this._traceBuffer = traceWriter != null ? new StringBuilder() : null;
			this._trace = traceWriter != null ? new StringWriter( this._traceBuffer, CultureInfo.InvariantCulture ) : TextWriter.Null;
			this._isInDynamicMethod = isInDynamicMethod;
			this._endOfMethod = underlying == null ? default( Label ) : underlying.DefineLabel();
			this._isDebuggable = isDebuggable;
		}

		/// <summary>
		///		Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			this._trace.Dispose();
		}

		///	<summary>
		///		Emit 'ret' instruction with specified arguments.
		///	</summary>
		public void EmitRet()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceOpCode( OpCodes.Ret );
			this._underlying.Emit( OpCodes.Ret );
			this.FlushTrace();
			this._isEnded = true;
		}

		public void FlushTrace()
		{
			if ( this._traceBuffer != null && this._traceBuffer.Length > 0 )
			{
				this.TraceLocals();
				this._trace.Flush();
				this._realTrace.Write( this._traceBuffer );
				this._traceBuffer.Clear();
			}
		}

		#region -- Locals --

		/// <summary>
		///		Declare local without pinning and name for debugging.
		/// </summary>
		/// <param name="localType"><see cref="Type"/> of local variable.</param>
		/// <returns><see cref="LocalBuilder"/> to refer declared local variable.</returns>
		public LocalBuilder DeclareLocal( Type localType )
		{
			Contract.Assert( !this.IsEnded );
			Contract.Assert( localType != null );
			Contract.Ensures( Contract.Result<LocalBuilder>() != null );

			return this.DeclareLocalCore( localType, null );
		}

#if DEBUG
		/// <summary>
		///		Declare local without name for debugging.
		/// </summary>
		/// <param name="localType"><see cref="Type"/> of local variable.</param>
		/// <param name="pinned">If <c>true</c>, the local variable will be pinned.</param>
		/// <returns><see cref="LocalBuilder"/> to refer declared local variable.</returns>
		public LocalBuilder DeclareLocal( Type localType, bool pinned )
		{
			Contract.Assert( !this.IsEnded );
			Contract.Assert( localType != null );
			Contract.Ensures( Contract.Result<LocalBuilder>() != null );

			return this.DeclareLocalCore( localType, null, pinned );
		}
#endif // DEBUG

		/// <summary>
		///		Declare local with name for debugging and without pinning.
		///		Note that this method is not enabled for dynamic method.
		/// </summary>
		/// <param name="localType"><see cref="Type"/> of local variable.</param>
		/// <param name="name">Name of the local variable.</param>
		/// <returns><see cref="LocalBuilder"/> to refer declared local variable.</returns>
		public LocalBuilder DeclareLocal( Type localType, string name )
		{
			Contract.Assert( !this.IsEnded );
			Contract.Assert( localType != null );
			Contract.Assert( name != null );
			Contract.Ensures( Contract.Result<LocalBuilder>() != null );

			return this.DeclareLocalCore( localType, name );
		}

#if DEBUG
		/// <summary>
		///		Declare local with name for debugging.
		///		Note that this method is not enabled for dynamic method.
		/// </summary>
		/// <param name="localType"><see cref="Type"/> of local variable.</param>
		/// <param name="name">Name of the local variable.</param>
		/// <param name="pinned">If <c>true</c>, the local variable will be pinned.</param>
		/// <returns><see cref="LocalBuilder"/> to refer declared local variable.</returns>
		public LocalBuilder DeclareLocal( Type localType, string name, bool pinned )
		{
			Contract.Assert( !this.IsEnded );
			Contract.Assert( localType != null );
			Contract.Assert( name != null );
			Contract.Ensures( Contract.Result<LocalBuilder>() != null );

			return this.DeclareLocalCore( localType, name, pinned );
		}
#endif // DEBUG

		private LocalBuilder DeclareLocalCore( Type localType, string name )
		{
			var result = this._underlying.DeclareLocal( localType );
			this._localDeclarations.Add( result, name );
			// TODO: NLiblet
			if ( !this._isInDynamicMethod && this._isDebuggable )
			{
				try
				{
					result.SetLocalSymInfo( name );
				}
				catch ( NotSupportedException )
				{
					this._isInDynamicMethod = true;
				}
			}
			return result;
		}

#if DEBUG
		private LocalBuilder DeclareLocalCore( Type localType, string name, bool pinned )
		{
			var result = this._underlying.DeclareLocal( localType, pinned );
			this._localDeclarations.Add( result, name );
			// TODO: NLiblet
			if ( !this._isInDynamicMethod && this._isDebuggable )
			{
				try
				{
					result.SetLocalSymInfo( name );
				}
				catch ( NotSupportedException )
				{
					this._isInDynamicMethod = true;
				}
			}
			return result;
		}
#endif // DEBUG

		private void TraceLocals()
		{
			Contract.Assert( this._realTrace != null );
			// TOOD: without init?
			this._realTrace.WriteLine( ".locals init (" );

			foreach ( var local in this._localDeclarations )
			{
				this.WriteIndent( this._realTrace, 1 );

				this._realTrace.Write( "[" );
				this._realTrace.Write( local.Key.LocalIndex );
				this._realTrace.Write( "] " );

				WriteType( this._realTrace, local.Key.LocalType );

				if ( local.Key.IsPinned )
				{
					this._realTrace.Write( "(pinned)" );
				}

				if ( local.Value != null )
				{
					this._realTrace.Write( " " );
					this._realTrace.Write( local.Value );
				}

				this._realTrace.WriteLine();
			}

			this._realTrace.WriteLine( ")" );
		}

		#endregion

		#region -- Exceptions --

		// Note: Leave always leave not leave.s.
		// FIXME: Integration check.

#if DEBUG
		/// <summary>
		///		Emit exception block with catch blocks.
		/// </summary>
		/// <param name="tryBlockEmitter">
		///		<see cref="Action{T1,T2}"/> which emits exception block (try in C#) body.
		///		A 1st argument is this <see cref="TracingILGenerator"/>,
		///		and a 2nd argument is <see cref="Label"/> will to be end of emitting exception block.
		///		The delegate do not have to emit leave or leave.s instrauction at tail of the body.
		/// </param>
		/// <param name="firstCatchBlock">
		///		<see cref="Tuple{T1,T2}"/> for catch block body emittion.
		///		A 1st item of the tuple is <see cref="Type"/> which indicates catching exception type.
		///		A 2nd item of the tuple is <see cref="Action{T1,T2}"/> which emits catch block body.
		///		A 1st argument of the delegate is this <see cref="TracingILGenerator"/>,
		///		a 2nd argument of the delegate is <see cref="Label"/> will to be end of emitting exception block,
		///		and 3rd argument of the delegate is the 1st item of the tuple.
		///		The delegate do not have to emit leave or leave.s instrauction at tail of the body.
		/// </param>
		/// <param name="remainingCatchBlockEmitters">
		///		<see cref="Tuple{T1,T2}"/> for catch block body emittion.
		///		A 1st item of the tuple is <see cref="Type"/> which indicates catching exception type.
		///		A 2nd item of the tuple is <see cref="Action{T1,T2}"/> which emits catch block body.
		///		A 1st argument of the delegate is this <see cref="TracingILGenerator"/>,
		///		a 2nd argument of the delegate is <see cref="Label"/> will to be end of emitting exception block,
		///		and 3rd argument of the delegate is the 1st item of the tuple.
		///		The delegate do not have to emit leave or leave.s instrauction at tail of the body.
		/// </param>
		[SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Tuple" )]
		public void EmitExceptionBlock(
			Action<TracingILGenerator, Label> tryBlockEmitter,
			Tuple<Type, Action<TracingILGenerator, Label, Type>> firstCatchBlock,
			params Tuple<Type, Action<TracingILGenerator, Label, Type>>[] remainingCatchBlockEmitters
		)
		{
			Contract.Assert( !this.IsEnded );
			Contract.Assert( tryBlockEmitter != null );
			Contract.Assert( firstCatchBlock != null );
			Contract.Assert( firstCatchBlock.Item1 != null && firstCatchBlock.Item2 != null );
			Contract.Assert( remainingCatchBlockEmitters != null );
			Contract.Assert( Contract.ForAll( remainingCatchBlockEmitters, item => item != null && item.Item1 != null && item.Item2 != null ) );

			this.EmitExceptionBlockCore( tryBlockEmitter, firstCatchBlock, remainingCatchBlockEmitters, null );
		}

		/// <summary>
		///		Emit exception block with catch blocks and a finally block.
		/// </summary>
		/// <param name="tryBlockEmitter">
		///		<see cref="Action{T1,T2}"/> which emits exception block (try in C#) body.
		///		A 1st argument is this <see cref="TracingILGenerator"/>,
		///		and a 2nd argument is <see cref="Label"/> will to be end of emitting exception block.
		///		The delegate do not have to emit leave or leave.s instrauction at tail of the body.
		/// </param>
		/// <param name="finallyBlockEmitter">
		///		<see cref="Action{T1,T2}"/> which emits finally block  body.
		///		A 1st argument is this <see cref="TracingILGenerator"/>,
		///		and a 2nd argument is <see cref="Label"/> will to be end of emitting exception block.
		///		The delegate do not have to emit endfinally instrauction at tail of the body.
		/// </param>
		/// <param name="catchBlockEmitters">
		///		<see cref="Tuple{T1,T2}"/> for catch block body emittion.
		///		A 1st item of the tuple is <see cref="Type"/> which indicates catching exception type.
		///		A 2nd item of the tuple is <see cref="Action{T1,T2}"/> which emits catch block body.
		///		A 1st argument of the delegate is this <see cref="TracingILGenerator"/>,
		///		a 2nd argument of the delegate is <see cref="Label"/> will to be end of emitting exception block,
		///		and 3rd argument of the delegate is the 1st item of the tuple.
		///		The delegate do not have to emit leave or leave.s instrauction at tail of the body.
		/// </param>
		public void EmitExceptionBlock(
			Action<TracingILGenerator, Label> tryBlockEmitter,
			Action<TracingILGenerator, Label> finallyBlockEmitter,
			params Tuple<Type, Action<TracingILGenerator, Label, Type>>[] catchBlockEmitters
		)
		{
			Contract.Assert( !this.IsEnded );
			Contract.Assert( tryBlockEmitter != null );
			Contract.Assert( finallyBlockEmitter != null );
			Contract.Assert( catchBlockEmitters != null );
			Contract.Assert( Contract.ForAll( catchBlockEmitters, item => item != null && item.Item1 != null && item.Item2 != null ) );

			this.EmitExceptionBlockCore( tryBlockEmitter, null, catchBlockEmitters, finallyBlockEmitter );
		}

		private void EmitExceptionBlockCore( Action<TracingILGenerator, Label> tryBlockEmitter, Tuple<Type, Action<TracingILGenerator, Label, Type>> firstCatchBlockEmitter, Tuple<Type, Action<TracingILGenerator, Label, Type>>[] remainingCatchBlockEmitters, Action<TracingILGenerator, Label> finallyBlockEmitter )
		{
			var endOfExceptionBlock = this.BeginExceptionBlock();
			tryBlockEmitter( this, endOfExceptionBlock );
			if ( firstCatchBlockEmitter != null )
			{
				this.BeginCatchBlock( firstCatchBlockEmitter.Item1 );
				firstCatchBlockEmitter.Item2( this, endOfExceptionBlock, firstCatchBlockEmitter.Item1 );
			}

			foreach ( var catchBlockEmitter in remainingCatchBlockEmitters )
			{
				this.BeginCatchBlock( catchBlockEmitter.Item1 );
				firstCatchBlockEmitter.Item2( this, endOfExceptionBlock, catchBlockEmitter.Item1 );
			}

			if ( finallyBlockEmitter != null )
			{
				this.BeginFinallyBlock();
				finallyBlockEmitter( this, endOfExceptionBlock );
			}

			this.EndExceptionBlock();
		}
#endif // DEBUG

		/// <summary>
		///		Begin exception block (try in C#) here.
		///		Note that you do not have to emit leave or laeve.s instrauction at tail of the body.
		/// </summary>
		/// <returns><see cref="Label"/> will to be end of begun exception block.</returns>
		public Label BeginExceptionBlock()
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceWriteLine( ".try" );
			this.Indent();

			var result = this._underlying.BeginExceptionBlock();
			this._endOfExceptionBlocks.Push( result );
			this._labels[ result ] = "END_TRY_" + this._labels.Count.ToString( CultureInfo.InvariantCulture );
			return result;
		}

#if DEBUG
		/// <summary>
		///		Begin catch block with specified exception.
		///		Note that you do not have to emit leave or laeve.s instrauction at tail of the body.
		/// </summary>
		/// <param name="exceptionType"><see cref="Type"/> for catch.</param>
		public void BeginCatchBlock( Type exceptionType )
		{
			Contract.Assert( !this.IsEnded );
			Contract.Assert( exceptionType != null );
			Contract.Assert( this.IsInExceptionBlock );

			this.Unindent();
			this.TraceStart();
			this.TraceWrite( ".catch" );
			this.TraceType( exceptionType );
			this.TraceWriteLine();
			this.Indent();
			this._underlying.BeginCatchBlock( exceptionType );
		}

		/// <summary>
		///		Begin filter block.
		///		Note that you do not have to emit leave or laeve.s instrauction at tail of the body.
		/// </summary>
		public void BeginExceptFilterBlock()
		{
			Contract.Assert( !this.IsEnded );
			Contract.Assert( this.IsInExceptionBlock );

			this.Unindent();
			this.TraceStart();
			this.TraceWriteLine( ".filter" );
			this.Indent();
			this._underlying.BeginExceptFilterBlock();
		}

		/// <summary>
		///		Begin fault block.
		///		Note that you do not have to emit endfinally instrauction at tail of the body.
		/// </summary>
		public void BeginFaultBlock()
		{
			Contract.Assert( !this.IsEnded );
			Contract.Assert( this.IsInExceptionBlock );

			this.Unindent();
			this.TraceStart();
			this.TraceWriteLine( ".fault" );
			this.Indent();
			this._underlying.BeginFaultBlock();
		}
#endif // DEBUG

		/// <summary>
		///		Begin finally block.
		///		Note that you do not have to emit endfinally instrauction at tail of the body.
		/// </summary>
		public void BeginFinallyBlock()
		{
			Contract.Assert( !this.IsEnded );
			Contract.Assert( this.IsInExceptionBlock );

			this.Unindent();
			this.TraceStart();
			this.TraceWriteLine( ".finally" );
			this.Indent();
			this._underlying.BeginFinallyBlock();
		}

		/// <summary>
		///		End current exception block and its last clause.
		/// </summary>
		public void EndExceptionBlock()
		{
			Contract.Assert( !this.IsEnded );
			Contract.Assert( this.IsInExceptionBlock );

			this.Unindent();
			this._underlying.EndExceptionBlock();
			this._endOfExceptionBlocks.Pop();
		}

		#endregion

		#region -- Labels --

#if DEBUG
		/// <summary>
		///		Define new <see cref="Label"/> without name for tracing.
		/// </summary>
		/// <returns><see cref="Label"/> which will be target of branch instructions.</returns>
		public Label DefineLabel()
		{
			Contract.Assert( !this.IsEnded );

			return this.DefineLabel( "LABEL_" + this._labels.Count.ToString( CultureInfo.InvariantCulture ) );
		}
#endif // DEBUG

		/// <summary>
		///		Define new <see cref="Label"/> with name for tracing.
		/// </summary>
		/// <param name="name">Name of label. Note that debugging information will not have this name.</param>
		/// <returns><see cref="Label"/> which will be target of branch instructions.</returns>
		public Label DefineLabel( string name )
		{
			Contract.Assert( !this.IsEnded );
			Contract.Assert( name != null );

			var result = this._underlying.DefineLabel();
			this._labels.Add( result, name );
			return result;
		}

		/// <summary>
		///		Mark current position using specifieid <see cref="Label"/>.
		/// </summary>
		/// <param name="label"><see cref="Label"/>.</param>
		public void MarkLabel( Label label )
		{
			this.TraceStart();
			this.TraceWriteLine( this._labels[ label ] );
			this._underlying.MarkLabel( label );
		}

		#endregion

#if !SILVERLIGHT
		#region -- Calli --

#if DEBUG
		/// <summary>
		///		Emit 'calli' instruction for indirect unmanaged function call.
		/// </summary>
		/// <param name="unmanagedCallingConvention"><see cref="CallingConvention"/> of unmanaged function.</param>
		/// <param name="returnType">Return <see cref="Type"/> of the function.</param>
		/// <param name="parameterTypes">Parameter <see cref="Type"/>s of the function.</param>
		public void EmitCalli( CallingConvention unmanagedCallingConvention, Type returnType, Type[] parameterTypes )
		{
			Contract.Assert( !this.IsEnded );
			Contract.Assert( returnType != null );
			Contract.Assert( parameterTypes != null );
			Contract.Assert( Contract.ForAll( parameterTypes, item => item != null ) );

			this.TraceStart();
			this.TraceWrite( "calli " );
			this.TraceSignature( null, unmanagedCallingConvention, returnType, parameterTypes, Type.EmptyTypes );
			this._underlying.EmitCalli( OpCodes.Calli, unmanagedCallingConvention, returnType, parameterTypes );
		}

		/// <summary>
		///		Emit 'calli' instruction for indirect managed method call.
		/// </summary>
		/// <param name="managedCallingConventions"><see cref="CallingConventions"/> of managed method.</param>
		/// <param name="returnType">Return <see cref="Type"/> of the method.</param>
		/// <param name="requiredParameterTypes">Required parameter <see cref="Type"/>s of the method.</param>
		/// <param name="optionalParameterTypes">Optional varargs parameter <see cref="Type"/>s of the method.</param>
		public void EmitCalli( CallingConventions managedCallingConventions, Type returnType, Type[] requiredParameterTypes, Type[] optionalParameterTypes )
		{
			Contract.Assert( !this.IsEnded );
			Contract.Assert( returnType != null );
			Contract.Assert( requiredParameterTypes != null );
			Contract.Assert( Contract.ForAll( requiredParameterTypes, item => item != null ) );
			Contract.Assert( optionalParameterTypes != null );
			Contract.Assert(
				optionalParameterTypes == null
				|| optionalParameterTypes.Length == 0
				|| ( managedCallingConventions & CallingConventions.VarArgs ) == CallingConventions.VarArgs
			);
			Contract.Assert( optionalParameterTypes == null || Contract.ForAll( optionalParameterTypes, item => item != null ) );

			this.TraceStart();
			this.TraceWrite( "calli " );
			this.TraceSignature( managedCallingConventions, null, returnType, requiredParameterTypes, optionalParameterTypes );
			this._underlying.EmitCalli( OpCodes.Calli, managedCallingConventions, returnType, requiredParameterTypes, optionalParameterTypes );
		}

#endif // DEBUG
		#endregion
#endif // !SILVERLIGHT

		#region -- Constrained. --

#if DEBUG
		/// <summary>
		///		Emit constrained 'callvirt' instruction.
		/// </summary>
		/// <param name="constrainedTo"><see cref="Type"/> to be constrained to.</param>
		/// <param name="target">Target <see cref="MethodInfo"/> which must be virtual method.</param>
		public void EmitConstrainedCallvirt( Type constrainedTo, MethodInfo target )
		{
			Contract.Assert( !this.IsEnded );
			Contract.Assert( constrainedTo != null );
			Contract.Assert( target != null );

			this.TraceStart();
			this.TraceWrite( "constraind." );
			this.TraceType( constrainedTo );
			this.TraceWriteLine();
			this._underlying.Emit( OpCodes.Constrained, constrainedTo );

			this.EmitCallvirt( target );
		}
#endif // DEBUG

		#endregion

		#region -- Readonly. --

#if DEBUG
		/// <summary>
		///		Emit readonly 'ldelema' instruction.
		/// </summary>
		/// <param name="elementType"><see cref="Type"/> of array element.</param>
		public void EmitReadOnlyLdelema( Type elementType )
		{
			Contract.Assert( !this.IsEnded );
			Contract.Assert( elementType != null );

			this.TraceStart();
			this.TraceWriteLine( "readonly." );
			this._underlying.Emit( OpCodes.Readonly );

			this.EmitLdelema( elementType );
		}
#endif // DEBUG

		#endregion

		#region -- Tail. --

#if DEBUG
		///	<summary>
		///		Emit 'call' instruction with specified arguments as tail call.
		///	</summary>
		///	<param name="target"><see cref="System.Reflection.MethodInfo"/> as target.</param>
		///	<remarks>
		///		Subsequent 'ret' instruction will be emitted together.
		///	</remarks>
		public void EmitTailCall( MethodInfo target )
		{
			Contract.Assert( !this.IsEnded );
			Contract.Assert( target != null );

			this.TraceStart();
			this.TraceWriteLine( "tail." );

			this._underlying.Emit( OpCodes.Tailcall );

			this.EmitCall( target );
			this.EmitRet();
		}

		///	<summary>
		///		Emit 'callvirt' instruction with specified arguments as tail call.
		///	</summary>
		///	<param name="target"><see cref="System.Reflection.MethodInfo"/> as target.</param>
		///	<remarks>
		///		Subsequent 'ret' instruction will be emitted together.
		///	</remarks>
		public void EmitTailCallVirt( MethodInfo target )
		{
			Contract.Assert( !this.IsEnded );
			Contract.Assert( target != null );

			this.TraceStart();
			this.TraceWriteLine( "tail." );

			this._underlying.Emit( OpCodes.Tailcall );

			this.EmitCallvirt( target );
			this.EmitRet();
		}

#if !SILVERLIGHT
		/// <summary>
		///		Emit 'calli' instruction for indirect unmanaged function call as tail call.
		/// </summary>
		/// <param name="unmanagedCallingConvention"><see cref="CallingConvention"/> of unmanaged function.</param>
		/// <param name="returnType">Return <see cref="Type"/> of the function.</param>
		/// <param name="parameterTypes">Parameter <see cref="Type"/>s of the function.</param>
		///	<remarks>
		///		Subsequent 'ret' instruction will be emitted together.
		///	</remarks>
		public void EmitTailCalli( CallingConvention unmanagedCallingConvention, Type returnType, Type[] parameterTypes )
		{
			Contract.Assert( !this.IsEnded );
			Contract.Assert( returnType != null );
			Contract.Assert( parameterTypes != null );
			Contract.Assert( Contract.ForAll( parameterTypes, item => item != null ) );

			this.TraceStart();
			this.TraceWriteLine( "tail." );

			this._underlying.Emit( OpCodes.Tailcall );

			this.EmitCalli( unmanagedCallingConvention, returnType, parameterTypes );
			this.EmitRet();
		}

		/// <summary>
		///		Emit 'calli' instruction for indirect managed method call as tail call.
		/// </summary>
		/// <param name="managedCallingConventions"><see cref="CallingConventions"/> of managed method.</param>
		/// <param name="returnType">Return <see cref="Type"/> of the method.</param>
		/// <param name="requiredParameterTypes">Required parameter <see cref="Type"/>s of the method.</param>
		/// <param name="optionalParameterTypes">Optional varargs parameter <see cref="Type"/>s of the method.</param>
		///	<remarks>
		///		Subsequent 'ret' instruction will be emitted together.
		///	</remarks>
		public void EmitTailCalli( CallingConventions managedCallingConventions, Type returnType, Type[] requiredParameterTypes, Type[] optionalParameterTypes )
		{
			Contract.Assert( !this.IsEnded );
			Contract.Assert( returnType != null );
			Contract.Assert( requiredParameterTypes != null );
			Contract.Assert( Contract.ForAll( requiredParameterTypes, item => item != null ) );
			Contract.Assert( optionalParameterTypes != null );
			Contract.Assert(
				optionalParameterTypes == null
				|| optionalParameterTypes.Length == 0
				|| ( managedCallingConventions & CallingConventions.VarArgs ) == CallingConventions.VarArgs
			);
			Contract.Assert( optionalParameterTypes == null || Contract.ForAll( optionalParameterTypes, item => item != null ) );

			this.TraceStart();
			this.TraceWriteLine( "tail." );

			this._underlying.Emit( OpCodes.Tailcall );

			this.EmitCalli( managedCallingConventions, returnType, requiredParameterTypes, optionalParameterTypes );
			this.EmitRet();
		}
#endif // SILVERLIGHT
#endif // DEBUG

		#endregion

		#region -- Unaligned. --

#if DEBUG
		/// <summary>
		///		Emit 'unaligned.' prefix.
		/// </summary>
		/// <param name="alignment">Alignment.</param>
		public void EmitUnaligned( byte alignment )
		{
			Contract.Assert( !this.IsEnded );

			this.TraceStart();
			this.TraceWrite( "unaligned." );
			this.TraceOperand( alignment );
			this.TraceWriteLine();

			this._underlying.Emit( OpCodes.Unaligned, alignment );
		}
#endif // DEBUG

		#endregion

		#region -- Tracing --

		/// <summary>
		///		Write trace message.
		/// </summary>
		/// <param name="value">The string.</param>
		public void TraceWrite( string value )
		{
			Contract.Assert( !this.IsEnded );
			Contract.Assert( this._trace != null );

			this._trace.Write( value );
		}

#if DEBUG
		/// <summary>
		///		Write trace message.
		/// </summary>
		/// <param name="format">The format string.</param>
		/// <param name="arg0">Format argument.</param>
		public void TraceWrite( string format, object arg0 )
		{
			Contract.Assert( !this.IsEnded );
			Contract.Assert( this._trace != null );

			this._trace.Write( format, arg0 );
		}

		/// <summary>
		///		Write trace message.
		/// </summary>
		/// <param name="format">The format string.</param>
		/// <param name="args">Format arguments.</param>
		public void TraceWrite( string format, params object[] args )
		{
			Contract.Assert( !this.IsEnded );
			Contract.Assert( this._trace != null );

			this._trace.Write( format, args );
		}
#endif // DEBUG

		/// <summary>
		///		Write trace line break.
		/// </summary>
		public void TraceWriteLine()
		{
			Contract.Assert( !this.IsEnded );
			Contract.Assert( this._trace != null );

			this._trace.WriteLine();
		}

		/// <summary>
		///		Write trace message followed by line break.
		/// </summary>
		/// <param name="value">The string.</param>
		public void TraceWriteLine( string value )
		{
			Contract.Assert( !this.IsEnded );
			Contract.Assert( this._trace != null );

			this._trace.WriteLine( value );
		}

		/// <summary>
		///		Write trace message followed by line break.
		/// </summary>
		/// <param name="format">The format string.</param>
		/// <param name="arg0">Format argument.</param>
		public void TraceWriteLine( string format, object arg0 )
		{
			Contract.Assert( !this.IsEnded );
			Contract.Assert( this._trace != null );

			this._trace.WriteLine( format, arg0 );
		}

#if DEBUG
		/// <summary>
		///		Write trace message followed by line break.
		/// </summary>
		/// <param name="format">The format string.</param>
		/// <param name="args">Format arguments.</param>
		public void TraceWriteLine( string format, params object[] args )
		{
			Contract.Assert( !this.IsEnded );
			Contract.Assert( this._trace != null );

			this._trace.WriteLine( format, args );
		}
#endif // DEBUG

		private void TraceType( Type type )
		{
			WriteType( this._trace, type );
		}

		private static void WriteType( TextWriter writer, Type type )
		{
			Contract.Assert( writer != null );

			if ( type == null || type == typeof( void ) )
			{
				writer.Write( "void" );
			}
			else if ( type.IsGenericParameter )
			{
				writer.Write( "{0}{1}", type.DeclaringMethod == null ? "!" : "!!", type.Name );
			}
			else
			{
#if SILVERLIGHT
				var endOfAssemblySimpleName = type.Assembly.FullName.IndexOf( ',' );
				writer.Write( "[{0}]{1}", endOfAssemblySimpleName < 0 ? type.Assembly.FullName : type.Assembly.FullName.Remove( endOfAssemblySimpleName ), type.FullName );
#else
				writer.Write( "[{0}]{1}", type.Assembly.GetName().Name, type.FullName );
#endif
			}
		}

		private void TraceField( FieldInfo field )
		{
			// <instr_field> <type> <typeSpec> :: <id>
			Contract.Assert( field != null );

			WriteType( this._trace, field.FieldType );
			this._trace.Write( " " );

			// TODO: NLiblet
#if !SILVERLIGHT
			var asFieldBuilder = field as FieldBuilder;
			if ( asFieldBuilder == null )
			{
				var modreqs = field.GetRequiredCustomModifiers();
				if ( 0 < modreqs.Length )
				{
					this._trace.Write( "modreq(" );

					foreach ( var modreq in modreqs )
					{
						WriteType( this._trace, modreq );
					}

					this._trace.Write( ") " );
				}

				var modopts = field.GetOptionalCustomModifiers();
				if ( 0 < modopts.Length )
				{
					this._trace.Write( "modopt(" );

					foreach ( var modopt in modopts )
					{
						WriteType( this._trace, modopt );
					}

					this._trace.Write( ") " );
				}
			}
#endif

#if !SILVERLIGHT
			if ( this._isInDynamicMethod || asFieldBuilder == null ) // declaring type of the field should be omitted for same type.
#endif
			{
				WriteType( this._trace, field.DeclaringType );
			}
			this._trace.Write( "::" );
			this._trace.Write( field.Name );
		}

#if DEBUG
		private void TraceSignature( CallingConventions? managedCallingConventions, CallingConvention? unmanagedCallingConvention, Type returnType, Type[] requiredParameterTypes, Type[] optionalParameterTypes )
		{
			//  <instr_sig> <callConv> <type> ( <parameters> )  
			WriteCallingConventions( this._trace, managedCallingConventions, unmanagedCallingConvention );
			WriteType( this._trace, returnType );
			this._trace.Write( " (" );
			for ( int i = 0; i < requiredParameterTypes.Length; i++ )
			{
				if ( 0 < i )
				{
					this._trace.Write( ", " );
				}
				else
				{
					this._trace.Write( " " );
				}

				WriteType( this._trace, requiredParameterTypes[ i ] );
			}

			if ( 0 < optionalParameterTypes.Length )
			{
				if ( 0 < requiredParameterTypes.Length )
				{
					this.TraceWrite( "," );
				}

				this.TraceWrite( " [" );

				for ( int i = 0; i < optionalParameterTypes.Length; i++ )
				{
					if ( 0 < i )
					{
						this._trace.Write( ", " );
					}
					else
					{
						this._trace.Write( " " );
					}

					WriteType( this._trace, optionalParameterTypes[ i ] );
				}

				this.TraceWrite( " ]" );
			}

			if ( 0 < ( requiredParameterTypes.Length + optionalParameterTypes.Length ) )
			{
				this._trace.Write( " " );
			}

			this._trace.WriteLine( ")" );
		}
#endif // DEBUG

		private void TraceMethod( MethodBase method )
		{
			Contract.Assert( method != null );

#if !WINDOWS_PHONE
			bool isMethodBuilder = method is MethodBuilder || method is ConstructorBuilder;
#endif

			/*
			 *	<instr_method> <callConv> <type> [ <typeSpec> :: ] <methodName> ( <parameters> ) 
			 */

			// <callConv>
			if ( !method.IsStatic )
			{
				this._trace.Write( "instance " );
			}

			var unamanagedCallingConvention = default( CallingConvention? );
			// TODO: Back to NLiblet
#if !WINDOWS_PHONE
			if ( !isMethodBuilder )
#endif
			{
				// TODO: C++/CLI etc...
				var dllImport = Attribute.GetCustomAttribute( method, typeof( DllImportAttribute ) ) as DllImportAttribute;
				if ( dllImport != null )
				{
					unamanagedCallingConvention = dllImport.CallingConvention;
				}
			}

			WriteCallingConventions( this._trace, method.CallingConvention, unamanagedCallingConvention );

			var asMethodInfo = method as MethodInfo;
			if ( asMethodInfo != null )
			{
				WriteType( this._trace, asMethodInfo.ReturnType );
				this._trace.Write( " " );
			}

			if ( method.DeclaringType == null )
			{
				// '[' '.module' name1 ']' 
				this._trace.Write( "[.module" );
				this._trace.Write( asMethodInfo.Module.Name );
				this._trace.Write( "]::" );
			}
#if !WINDOWS_PHONE
			else if ( this._isInDynamicMethod || !isMethodBuilder ) // declaring type of the method should be omitted for same type.
#else
			else
#endif
			{
				WriteType( this._trace, method.DeclaringType );
				this._trace.Write( "::" );
			}

			this._trace.Write( method.Name );
			this._trace.Write( "(" );
#if !WINDOWS_PHONE
			if ( !isMethodBuilder )
#endif
			{
				var parameters = method.GetParameters();
				for ( int i = 0; i < parameters.Length; i++ )
				{
					if ( i == 0 )
					{
						this._trace.Write( " " );
					}
					else
					{
						this._trace.Write( ", " );
					}

					if ( parameters[ i ].IsOut )
					{
						this._trace.Write( "out " );
					}
					else if ( parameters[ i ].ParameterType.IsByRef )
					{
						this._trace.Write( "ref " );
					}

					WriteType( this._trace, parameters[ i ].ParameterType.IsByRef ? parameters[ i ].ParameterType.GetElementType() : parameters[ i ].ParameterType );
					this._trace.Write( " " );
					this._trace.Write( parameters[ i ].Name );
				}

				if ( 0 < parameters.Length )
				{
					this._trace.Write( " " );
				}
			}

			this._trace.Write( ")" );
		}

		[SuppressMessage( "Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase", Justification = "This is not normalization." )]
		private static void WriteCallingConventions( TextWriter writer, CallingConventions? managedCallingConverntions, CallingConvention? unamangedCallingConvention )
		{
			Contract.Assert( writer != null );

			bool needsSpace = false;
			if ( managedCallingConverntions != null )
			{
				if ( ( managedCallingConverntions & CallingConventions.ExplicitThis ) == CallingConventions.ExplicitThis )
				{
					writer.Write( "explicit" );
					needsSpace = true;
				}
			}

			if ( unamangedCallingConvention == null )
			{
				if ( managedCallingConverntions != null )
				{
					if ( ( managedCallingConverntions & CallingConventions.VarArgs ) == CallingConventions.VarArgs )
					{
						if ( needsSpace )
						{
							writer.Write( " " );
						}

						writer.Write( "varargs" );
					}
				}
			}
			else
			{
				switch ( unamangedCallingConvention.Value )
				{
					case CallingConvention.Winapi:
					case CallingConvention.StdCall:
					{
						if ( needsSpace )
						{
							writer.Write( " " );
						}

						writer.Write( "unmanaged stdcall" );
						break;
					}
					default:
					{
						if ( needsSpace )
						{
							writer.Write( " " );
						}

						writer.Write( "unmanaged " );
						writer.Write( unamangedCallingConvention.Value.ToString().ToLowerInvariant() );
						break;
					}
				}
			}
		}

		private void TraceOpCode( OpCode opCode )
		{
			this._trace.Write( opCode.Name );
		}

		private void TraceOperand( int value )
		{
			this._trace.Write( value );
		}

		private void TraceOperand( long value )
		{
			this._trace.Write( value );
		}

		private void TraceOperand( double value )
		{
			this._trace.Write( value );
		}

		private void TraceOperand( string value )
		{
			// QSTRING
			this._trace.Write( String.Format( CultureInfo.InvariantCulture, "\"{0:L}\"", value ) );
		}

		private void TraceOperand( Label value )
		{
			this._trace.Write( this._labels[ value ] );
		}

#if DEBUG
		private void TraceOperand( Label[] values )
		{
			for ( int i = 0; i < values.Length; i++ )
			{
				if ( 0 < i )
				{
					this._trace.Write( ", " );
				}

				this.TraceOperand( values[ i ] );
			}
		}
#endif // DEBUG

		private void TraceOperand( Type value )
		{
			this.TraceType( value );
		}

		private void TraceOperand( FieldInfo value )
		{
			this.TraceField( value );
		}

		private void TraceOperand( MethodBase value )
		{
			this.TraceMethod( value );
		}

		private void TraceOperandToken( Type target )
		{
			this.TraceType( target );
			this.TraceOperandTokenValue( target.MetadataToken );
		}

		private void TraceOperandToken( FieldInfo target )
		{
			this._trace.Write( "field " );
			this.TraceField( target );
			this.TraceOperandTokenValue( target.MetadataToken );
		}

		private void TraceOperandToken( MethodBase target )
		{
			this._trace.Write( "method " );
			this.TraceMethod( target );
			this.TraceOperandTokenValue( target.MetadataToken );
		}

		private void TraceOperandTokenValue( int value )
		{
			this._trace.Write( "<" );
			this._trace.Write( value.ToString( "x8", CultureInfo.InvariantCulture ) );
			this._trace.Write( ">" );
		}

		private void TraceStart()
		{
			this.WriteLineNumber();
			this.WriteIndent();
		}

		private void WriteLineNumber()
		{
			Contract.Assert( this._trace != null );

			this._trace.Write( "L_{0:d4}\t", this._lineNumber );
			this._lineNumber++;
		}

		private void WriteIndent()
		{
			this.WriteIndent( this._trace, this._indentLevel );
		}

		private void WriteIndent( TextWriter writer, int indentLevel )
		{
			Contract.Assert( writer != null );
			Contract.Assert( 0 <= indentLevel );

			for ( int i = 0; i < indentLevel; i++ )
			{
				writer.Write( this._indentChars );
			}
		}

		private void Indent()
		{
			this._indentLevel++;
		}

		private void Unindent()
		{
			Contract.Assert( 0 < this._indentLevel );

			this._indentLevel--;
		}

		#endregion
	}
}
