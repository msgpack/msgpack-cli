#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010 FUJIWARA, Yusuke
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
using System.Diagnostics;
using System.Globalization;
using System.IO;
using NLiblet.Reflection;

namespace MsgPack.Serialization
{
	/// <summary>
	///		Defines IL generation features for serialization methods.
	/// </summary>
	internal abstract class SerializationMethodGenerator
	{
		/// <summary>
		///		 Gets a value indicating whether this instance is trace enabled.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if the trace enabled; otherwise, <c>false</c>.
		/// </value>
		internal static bool IsTraceEnabled
		{
			get { return ( Tracer.Emit.Switch.Level & SourceLevels.Verbose ) == SourceLevels.Verbose; }
		}

		private readonly TextWriter _trace = IsTraceEnabled ? new StringWriter() : TextWriter.Null;

		/// <summary>
		///		Gets the <see cref="TextWriter"/> for tracing.
		/// </summary>
		/// <value>
		///		The <see cref="TextWriter"/> for tracing.
		///		This value will not be <c>null</c>.
		/// </value>
		protected TextWriter Trace { get { return this._trace; } }

		/// <summary>
		///		Initializes a new instance of the <see cref="SerializationMethodGenerator"/> class.
		/// </summary>
		protected SerializationMethodGenerator() { }

		/// <summary>
		///		Gets the IL generator to emit IL stream.
		/// </summary>
		/// <returns>
		///		<see cref="TracingILGenerator"/>. This value will not be <c>null</c>.
		/// </returns>
		public abstract TracingILGenerator GetILGenerator();

		/// <summary>
		///		Creates the delegate to invoke generated method.
		/// </summary>
		/// <typeparam name="TDelegate">The type of the delegate.</typeparam>
		/// <returns>
		///		Delegate to invoke generated method.
		///		This value will not be <c>null</c>.
		///		Note that the delegate may throw <see cref="VerificationException"/>,
		///		<see cref="InvalidProgramException"/>, or <see cref="ExecutionEngineException"/>
		///		when the emitted IL is not correct.
		/// </returns>
		/// <exception cref="InvalidOperationException">
		///		<typeparamref name="TDelegate"/> is not delegate, namely is not derived from <see cref="MulticastDelegate"/>.
		///		Or, the signature of <typeparamref name="TDelegate"/> is not compatible for previous <see cref="SerializationMethodGenerator.CreateGenerator"/> invocation parameters.
		/// </exception>
		public TDelegate CreateDelegate<TDelegate>()
			where TDelegate : class
		{
			if ( !typeof( MulticastDelegate ).IsAssignableFrom( typeof( TDelegate ) ) )
			{
				throw new InvalidOperationException( String.Format( CultureInfo.CurrentCulture, "'{0}' is not delegate.", typeof( TDelegate ) ) );
			}

			var result = this.CreateDelegateCore<TDelegate>();
			this.FlushTrace();
			return result;
		}

		/// <summary>
		///		Creates the delegate to invoke generated method.
		/// </summary>
		/// <typeparam name="TDelegate">The type of the delegate.</typeparam>
		/// <returns>
		///		Delegate to invoke generated method.
		///		This value will not be <c>null</c>.
		///		Note that the delegate may throw <see cref="VerificationException"/>,
		///		<see cref="InvalidProgramException"/>, or <see cref="ExecutionEngineException"/>
		///		when the emitted IL is not correct.
		/// </returns>
		/// <exception cref="InvalidOperationException">
		///		<typeparamref name="TDelegate"/> is not delegate, namely is not derived from <see cref="MulticastDelegate"/>.
		///		Or, the signature of <typeparamref name="TDelegate"/> is not compatible for previous <see cref="SerializationMethodGenerator.CreateGenerator"/> invocation parameters.
		/// </exception>
		protected abstract TDelegate CreateDelegateCore<TDelegate>()
			where TDelegate : class;

		public void FlushTrace()
		{
			StringWriter writer = this._trace as StringWriter;
			if ( writer != null )
			{
				var buffer = writer.GetStringBuilder();
				var source = Tracer.Emit;
				if ( source != null && 0 < buffer.Length )
				{
					source.TraceData( Tracer.EventType.ILTrace, Tracer.EventId.ILTrace, buffer );
				}

				buffer.Clear();
			}
		}
	}
}
