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
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using NLiblet.Reflection;

namespace MsgPack.Serialization
{
	/// <summary>
	///		Genereates serialization methods which can be save to file.
	/// </summary>
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
		///		The 2nd argument should be argument index of the serializer holder.
		///		This value will not be <c>null</c>.
		/// </returns>
		public abstract Action<TracingILGenerator, int> RegisterSerializer( Type targetType );
	}

	internal sealed class FieldBasedSerializerEmitter : SerializerEmitter
	{
		private static readonly Type[] _constructorParameterTypes = new[] { typeof( SerializationContext ) };

		private readonly Dictionary<RuntimeTypeHandle, FieldBuilder> _serializers;
		private readonly ConstructorBuilder _constructorBuilder;
		private readonly TypeBuilder _typeBuilder;
		private readonly MethodBuilder _packMethodBuilder;
		private readonly MethodBuilder _unpackFromMethodBuilder;
		private MethodBuilder _unpackToMethodBuilder;
		private readonly bool _isDebuggable;

		/// <summary>
		/// Initializes a new instance of the <see cref="FieldBasedSerializerEmitter"/> class.
		/// </summary>
		/// <param name="host">The host <see cref="ModuleBuilder"/>.</param>
		/// <param name="sequence">The sequence number to name new type..</param>
		/// <param name="targetType">Type of the serialization target.</param>
		/// <param name="isDebuggable">Set to <c>true</c> when <paramref name="host"/> is debuggable.</param>
		public FieldBasedSerializerEmitter( ModuleBuilder host, int sequence, Type targetType, bool isDebuggable )
			: base()
		{
			string typeName =
				String.Join(
				Type.Delimiter.ToString(),
				typeof( SerializerEmitter ).Namespace,
				"Generated",
				IdentifierUtility.EscapeTypeName( targetType ) + "Serializer" + sequence
			);
			Tracer.Emit.TraceEvent( Tracer.EventType.DefineType, Tracer.EventId.DefineType, "Create {0}", typeName );
			this._typeBuilder =
				host.DefineType(
					typeName,
					TypeAttributes.Sealed | TypeAttributes.Public | TypeAttributes.AutoClass | TypeAttributes.AutoLayout | TypeAttributes.BeforeFieldInit,
					typeof( MessagePackSerializer<> ).MakeGenericType( targetType )
				);

			this._constructorBuilder = this._typeBuilder.DefineConstructor( MethodAttributes.Public, CallingConventions.Standard, _constructorParameterTypes );

			this._packMethodBuilder =
				this._typeBuilder.DefineMethod(
					"PackToCore",
					MethodAttributes.Family | MethodAttributes.Virtual | MethodAttributes.Final,
					CallingConventions.HasThis,
					typeof( void ),
					new Type[] { typeof( Packer ), targetType }
				);

			this._unpackFromMethodBuilder =
				this._typeBuilder.DefineMethod(
					"UnpackFromCore",
					MethodAttributes.Family | MethodAttributes.Virtual | MethodAttributes.Final,
					CallingConventions.HasThis,
					targetType,
					UnpackFromCoreParameterTypes
				);

			this._typeBuilder.DefineMethodOverride( this._packMethodBuilder, this._typeBuilder.BaseType.GetMethod( this._packMethodBuilder.Name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic ) );
			this._typeBuilder.DefineMethodOverride( this._unpackFromMethodBuilder, this._typeBuilder.BaseType.GetMethod( this._unpackFromMethodBuilder.Name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic ) );
			this._serializers = new Dictionary<RuntimeTypeHandle, FieldBuilder>();
			this._isDebuggable = isDebuggable;
		}

		public sealed override TracingILGenerator GetPackToMethodILGenerator()
		{
			if ( IsTraceEnabled )
			{
				this.Trace.WriteLine( "{0}::{1}", MethodBase.GetCurrentMethod(), this._packMethodBuilder );
			}

			return new TracingILGenerator( this._packMethodBuilder, this.Trace, this._isDebuggable );
		}

		public sealed override TracingILGenerator GetUnpackFromMethodILGenerator()
		{
			if ( IsTraceEnabled )
			{
				this.Trace.WriteLine( "{0}::{1}", MethodBase.GetCurrentMethod(), this._unpackFromMethodBuilder );
			}

			return new TracingILGenerator( this._unpackFromMethodBuilder, this.Trace, this._isDebuggable );
		}

		public sealed override TracingILGenerator GetUnpackToMethodILGenerator()
		{
			if ( IsTraceEnabled )
			{
				this.Trace.WriteLine( "{0}::{1}", MethodBase.GetCurrentMethod(), this._unpackToMethodBuilder );
			}

			if ( this._unpackToMethodBuilder == null )
			{
				this._unpackToMethodBuilder =
					this._typeBuilder.DefineMethod(
						"UnpackToCore",
						MethodAttributes.Family | MethodAttributes.Virtual | MethodAttributes.Final,
						CallingConventions.HasThis,
						null,
						new Type[] { typeof( Unpacker ), this._unpackFromMethodBuilder.ReturnType }
					);

				this._typeBuilder.DefineMethodOverride( this._unpackToMethodBuilder, this._typeBuilder.BaseType.GetMethod( this._unpackToMethodBuilder.Name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic ) );
			}

			return new TracingILGenerator( this._unpackToMethodBuilder, this.Trace, this._isDebuggable );
		}

		public sealed override MessagePackSerializer<T> CreateInstance<T>( SerializationContext context )
		{
			var contextParameter = Expression.Parameter( typeof( SerializationContext ) );
			return
				Expression.Lambda<Func<SerializationContext, MessagePackSerializer<T>>>(
					Expression.New(
						this.Create(),
						contextParameter
					),
					contextParameter
				).Compile()( context );
		}


		/// <summary>
		///		Creates the serializer type built now and returns its constructor.
		/// </summary>
		/// <returns>
		///		Newly built <see cref="MessagePackSerializer{T}"/> type constructor.
		///		This value will not be <c>null</c>.
		///	</returns>
		private ConstructorInfo Create()
		{
			if ( !this._typeBuilder.IsCreated() )
			{
				/*
				 *	.ctor( SerializationContext ) : base()
				 *	{
				 *		this._serializer0 = context.GetSerializer<T0>();
				 *		this._serializer1 = context.GetSerializer<T1>();
				 *		this._serializer2 = context.GetSerializer<T2>();
				 *			:
				 *	}
				 */
				var il = this._constructorBuilder.GetILGenerator();
				// : base()
				il.Emit( OpCodes.Ldarg_0 );
				il.Emit( OpCodes.Call, this._typeBuilder.BaseType.GetConstructor( BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Type.EmptyTypes, null ) );

				// this._serializerN = context.GetSerializer<T>();
				foreach ( var entry in this._serializers )
				{
					var targetType = Type.GetTypeFromHandle( entry.Key );
					var getMethod = Metadata._SerializationContext.GetSerializer1_Method.MakeGenericMethod( targetType );
					il.Emit( OpCodes.Ldarg_0 );
					il.Emit( OpCodes.Ldarg_1 );
					il.Emit( OpCodes.Callvirt, getMethod );
					il.Emit( OpCodes.Stfld, entry.Value );
				}

				il.Emit( OpCodes.Ret );
			}

			return this._typeBuilder.CreateType().GetConstructor( _constructorParameterTypes );
		}

		public sealed override Action<TracingILGenerator, int> RegisterSerializer( Type targetType )
		{
			if ( this._typeBuilder.IsCreated() )
			{
				throw new InvalidOperationException( "Type is already built." );
			}

			FieldBuilder result;
			if ( !this._serializers.TryGetValue( targetType.TypeHandle, out result ) )
			{
				result = this._typeBuilder.DefineField( "_serializer" + this._serializers.Count, typeof( MessagePackSerializer<> ).MakeGenericType( targetType ), FieldAttributes.Private | FieldAttributes.InitOnly );
				this._serializers.Add( targetType.TypeHandle, result );
			}

			return
				( il, thisIndex ) =>
				{
					il.EmitAnyLdarg( thisIndex );
					il.EmitLdfld( result );
				};
		}
	}

	internal sealed class CallbackMessagePackSerializer<T> : MessagePackSerializer<T>
	{
		private readonly SerializationContext _context;
		private readonly Action<SerializationContext, Packer, T> _packToCore;
		private readonly Func<SerializationContext, Unpacker, T> _unpackFromCore;
		private readonly Action<SerializationContext, Unpacker, T> _unpackToCore;

		public CallbackMessagePackSerializer(
			SerializationContext context,
			Action<SerializationContext, Packer, T> packToCore,
			Func<SerializationContext, Unpacker, T> unpackFromCore,
			Action<SerializationContext, Unpacker, T> unpackToCore
		)
		{
			this._context = context;
			this._packToCore = packToCore;
			this._unpackFromCore = unpackFromCore;
			this._unpackToCore = unpackToCore;
		}

		protected internal override void PackToCore( Packer packer, T objectTree )
		{
			this._packToCore( this._context, packer, objectTree );
		}

		protected internal override T UnpackFromCore( Unpacker unpacker )
		{
			return this._unpackFromCore( this._context, unpacker );
		}

		protected internal sealed override void UnpackToCore( Unpacker unpacker, T collection )
		{
			if ( this._unpackToCore != null )
			{
				this._unpackToCore( this._context, unpacker, collection );
			}
			else
			{
				base.UnpackToCore( unpacker, collection );
			}
		}
	}

	internal sealed class ContextBasedSerializerEmitter : SerializerEmitter
	{
		private readonly Type _targetType;
		private readonly DynamicMethod _packToMethod;
		private readonly DynamicMethod _unpackFromMethod;
		private DynamicMethod _unpackToMethod;

		public ContextBasedSerializerEmitter( Type targetType )
		{
			this._targetType = targetType;

			this._packToMethod = new DynamicMethod( "PackToCore", null, new[] { typeof( SerializationContext ), typeof( Packer ), targetType } );
			this._unpackFromMethod = new DynamicMethod( "UnpackFromCore", targetType, new[] { typeof( SerializationContext ), typeof( Unpacker ) } );
		}

		public override TracingILGenerator GetPackToMethodILGenerator()
		{
			if ( IsTraceEnabled )
			{
				this.Trace.WriteLine( "{0}::{1}", MethodBase.GetCurrentMethod(), this._unpackFromMethod );
			}

			return new TracingILGenerator( this._packToMethod, this.Trace );
		}

		public override TracingILGenerator GetUnpackFromMethodILGenerator()
		{
			if ( IsTraceEnabled )
			{
				this.Trace.WriteLine( "{0}::{1}", MethodBase.GetCurrentMethod(), this._unpackFromMethod );
			}

			return new TracingILGenerator( this._unpackFromMethod, this.Trace );
		}

		public override TracingILGenerator GetUnpackToMethodILGenerator()
		{
			if ( this._unpackToMethod == null )
			{
				this._unpackToMethod = new DynamicMethod( "UnpackToCore", null, new[] { typeof( SerializationContext ), typeof( Unpacker ), this._targetType } );
			}

			if ( IsTraceEnabled )
			{
				this.Trace.WriteLine( "{0}::{1}", MethodBase.GetCurrentMethod(), this._unpackToMethod );
			}

			return new TracingILGenerator( this._unpackToMethod, this.Trace );
		}

		public override MessagePackSerializer<T> CreateInstance<T>( SerializationContext context )
		{
			var packTo = this._packToMethod.CreateDelegate( typeof( Action<SerializationContext, Packer, T> ) ) as Action<SerializationContext, Packer, T>;
			var unpackFrom = this._unpackFromMethod.CreateDelegate( typeof( Func<SerializationContext, Unpacker, T> ) ) as Func<SerializationContext, Unpacker, T>;
			var unpackTo = default( Action<SerializationContext, Unpacker, T> );
			if ( this._unpackToMethod != null )
			{
				unpackTo = this._unpackToMethod.CreateDelegate( typeof( Action<SerializationContext, Unpacker, T> ) ) as Action<SerializationContext, Unpacker, T>;
			}

			return new CallbackMessagePackSerializer<T>( context, packTo, unpackFrom, unpackTo );
		}

		public override Action<TracingILGenerator, int> RegisterSerializer( Type targetType )
		{
			return
				( il, contextIndex ) =>
				{
					il.EmitAnyLdarg( contextIndex );
					il.EmitAnyCall( Metadata._SerializationContext.GetSerializer1_Method.MakeGenericMethod( targetType ) );
				};
		}
	}

}
