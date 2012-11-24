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
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.IO;
using MsgPack.Serialization.Reflection;

namespace MsgPack.Serialization.EmittingSerializers
{
	/// <summary>
	///		Genereates serialization methods which can be save to file.
	/// </summary>
	[ContractClass( typeof( SerializerEmitterContract ) )]
	internal abstract class SerializerEmitter : IDisposable
	{
		protected static readonly Type[] UnpackFromCoreParameterTypes = new[] { typeof( Unpacker ) };

		/// <summary>
		///		 Gets a value indicating whether this instance is trace enabled.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if the trace enabled; otherwise, <c>false</c>.
		/// </value>
		protected static bool IsTraceEnabled
		{
			get { return ( Tracer.Emit.Switch.Level & SourceLevels.Verbose ) == SourceLevels.Verbose; }
		}

		private readonly TextWriter _trace = IsTraceEnabled ? new StringWriter( CultureInfo.InvariantCulture ) : TextWriter.Null;

		/// <summary>
		///		Gets the <see cref="TextWriter"/> for tracing.
		/// </summary>
		/// <value>
		///		The <see cref="TextWriter"/> for tracing.
		///		This value will not be <c>null</c>.
		/// </value>
		protected TextWriter Trace { get { return this._trace; } }

		/// <summary>
		///		Flushes the trace.
		/// </summary>
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

		/// <summary>
		/// Initializes a new instance of the <see cref="SerializerEmitter"/> class.
		/// </summary>
		protected SerializerEmitter()
		{
		}

		/// <summary>
		///		Releases all managed resources.
		/// </summary>
		public void Dispose()
		{
			this.Dispose( true );
			GC.SuppressFinalize( this );
		}

		/// <summary>
		///		Releases unmanaged and optionally managed resources.
		/// </summary>
		/// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
		protected virtual void Dispose( bool disposing )
		{
			if ( disposing )
			{
				this._trace.Dispose();
			}
		}

		/// <summary>
		///		Gets the IL generator to implement <see cref="M:MessagePackSerializer{T}.PackToCore"/> overrides.
		/// </summary>
		/// <returns>
		///		The IL generator to implement <see cref="M:MessagePackSerializer{T}.PackToCore"/> overrides.
		///		This value will not be <c>null</c>.
		/// </returns>
		public abstract TracingILGenerator GetPackToMethodILGenerator();

		/// <summary>
		///		Gets the IL generator to implement <see cref="M:MessagePackSerializer{T}.UnpackFromCore"/> overrides.
		/// </summary>
		/// <returns>
		///		The IL generator to implement <see cref="M:MessagePackSerializer{T}.UnpackFromCore"/> overrides.
		///		This value will not be <c>null</c>.
		/// </returns>
		public abstract TracingILGenerator GetUnpackFromMethodILGenerator();

		/// <summary>
		///		Gets the IL generator to implement <see cref="M:MessagePackSerializer{T}.UnpackToCore"/> overrides.
		/// </summary>
		/// <returns>
		///		The IL generator to implement <see cref="M:MessagePackSerializer{T}.UnpackToCore"/> overrides.
		/// </returns>
		/// <remarks>
		///		When this method is called, <see cref="M:MessagePackSerializer{T}.UnpackToCore"/> will be overridden.
		///		This value will not be <c>null</c>.
		/// </remarks>
		public abstract TracingILGenerator GetUnpackToMethodILGenerator();

		/// <summary>
		///		Creates the serializer type built now and returns its new instance.
		/// </summary>
		/// <typeparam name="T">Target type to be serialized/deserialized.</typeparam>
		/// <param name="context">The <see cref="SerializationContext"/> to holds serializers.</param>
		/// <returns>
		///		Newly built <see cref="MessagePackSerializer{T}"/> instance.
		///		This value will not be <c>null</c>.
		///	</returns>
		public abstract MessagePackSerializer<T> CreateInstance<T>( SerializationContext context );

		/// <summary>
		///		Regisgter using <see cref="MessagePackSerializer{T}"/> target type to the current emitting session.
		/// </summary>
		/// <param name="targetType">Type to be serialized/deserialized.</param>
		/// <returns>
		///		<see cref=" Action{T1,T2}"/> to emit serializer retrieval instructions.
		///		The 1st argument should be <see cref="TracingILGenerator"/> to emit instructions.
		///		The 2nd argument should be argument index of the serializer holder, normally 0 (this pointer).
		///		This value will not be <c>null</c>.
		/// </returns>
		public abstract Action<TracingILGenerator, int> RegisterSerializer( Type targetType );
	}

	[ContractClassFor( typeof( SerializerEmitter ) )]
	internal abstract class SerializerEmitterContract : SerializerEmitter
	{
		public override TracingILGenerator GetPackToMethodILGenerator()
		{
			Contract.Ensures( Contract.Result<TracingILGenerator>() != null );
			return null;
		}

		public override TracingILGenerator GetUnpackFromMethodILGenerator()
		{
			Contract.Ensures( Contract.Result<TracingILGenerator>() != null );
			return null;
		}

		public override TracingILGenerator GetUnpackToMethodILGenerator()
		{
			Contract.Ensures( Contract.Result<TracingILGenerator>() != null );
			return null;
		}

		public override MessagePackSerializer<T> CreateInstance<T>( SerializationContext context )
		{
			Contract.Requires( context != null );
			Contract.Ensures( Contract.Result<MessagePackSerializer<T>>() != null );
			return null;
		}

		public override Action<TracingILGenerator, int> RegisterSerializer( Type targetType )
		{
			Contract.Requires( targetType != null );
			Contract.Ensures( Contract.Result<Action<TracingILGenerator, int>>() != null );
			throw new NotImplementedException();
		}
	}

}
