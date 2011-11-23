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
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;
using NLiblet.Reflection;

namespace MsgPack.Serialization
{
	/// <summary>
	///		Manages <see cref="SerializationMethodGenerator"/> which generates dumpable serialization methods.
	/// </summary>
	internal sealed class DumpableSerializationMethodGeneratorManager : SerializationMethodGeneratorManager
	{
		private static readonly ConstructorInfo _debuggableAttributeCtor =
			typeof( DebuggableAttribute ).GetConstructor( new[] { typeof( bool ), typeof( bool ) } );
		private static readonly object[] _debuggableAttributeCtorArguments = new object[] { true, true };
		private static int _assemblySequence = -1;
		private int _typeSequence = -1;

		private static DumpableSerializationMethodGeneratorManager _canDump = new DumpableSerializationMethodGeneratorManager( true );

		/// <summary>
		///		Get the singleton instance for can-dump mode.
		/// </summary>
		public static DumpableSerializationMethodGeneratorManager CanDump
		{
			get { return DumpableSerializationMethodGeneratorManager._canDump; }
		}

		private static DumpableSerializationMethodGeneratorManager _fast = new DumpableSerializationMethodGeneratorManager( false );

		/// <summary>
		///		Get the singleton instance for fast mode.
		/// </summary>
		public static DumpableSerializationMethodGeneratorManager Fast
		{
			get { return DumpableSerializationMethodGeneratorManager._fast; }
		}

		internal static void Refresh()
		{
			_canDump = new DumpableSerializationMethodGeneratorManager( true );
			_fast = new DumpableSerializationMethodGeneratorManager( false );
		}

		/// <summary>
		///		Save ILs as modules to specified directory.
		/// </summary>
		/// <exception cref="PathTooLongException">
		///		The file path generated is too long on the current platform.
		/// </exception>
		/// <exception cref="UnauthorizedAccessException">
		///		Current user does not have required permission to save file on the current directory.
		/// </exception>
		/// <exception cref="IOException">
		///		The output device does not have enough free space.
		///		Or the target file already exists and is locked by other thread.
		///		Or the low level I/O error is occurred.
		/// </exception>
		public static void DumpTo()
		{
			_canDump.DumpToCore();
		}

		private readonly AssemblyBuilder _assembly;
		private readonly ModuleBuilder _module;
		private readonly string _moduleFileName;

		private DumpableSerializationMethodGeneratorManager( bool isDebuggable )
		{
			var assemblyName = typeof( DumpableSerializationMethodGenerator ).Namespace + ".GeneratedSerealizers" + Interlocked.Increment( ref _assemblySequence );
			this._assembly =
				AppDomain.CurrentDomain.DefineDynamicAssembly(
					new AssemblyName( assemblyName ),
					AssemblyBuilderAccess.RunAndSave
				);
			if ( isDebuggable )
			{
				this._assembly.SetCustomAttribute( new CustomAttributeBuilder( _debuggableAttributeCtor, _debuggableAttributeCtorArguments ) );
			}
			else
			{
				this._assembly.SetCustomAttribute(
					new CustomAttributeBuilder(
						typeof( DebuggableAttribute ).GetConstructor( new[] { typeof( DebuggableAttribute.DebuggingModes ) } ),
						new object[] { DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints }
					)
				);
			}

			this._assembly.SetCustomAttribute(
				new CustomAttributeBuilder(
					typeof( System.Runtime.CompilerServices.CompilationRelaxationsAttribute ).GetConstructor( new[] { typeof( int ) } ),
					new object[] { 8 }
				)
			);
			this._assembly.SetCustomAttribute(
				new CustomAttributeBuilder(
					typeof( System.Security.SecurityRulesAttribute ).GetConstructor( new[] { typeof( System.Security.SecurityRuleSet ) } ),
					new object[] { System.Security.SecurityRuleSet.Level2 },
					new[] { typeof( System.Security.SecurityRulesAttribute ).GetProperty( "SkipVerificationInFullTrust" ) },
					new object[] { true }
				)
			);
			this._moduleFileName = assemblyName + ".dll";
			this._module = this._assembly.DefineDynamicModule( assemblyName, this._moduleFileName, true );
		}

		protected sealed override SerializationMethodGenerator CreateGeneratorCore( string operation, Type targetType, string targetMemberName, Type returnType, params Type[] parameterTypes )
		{
			return new DumpableSerializationMethodGenerator( this._module, Interlocked.Increment( ref this._typeSequence ), operation, targetType, targetMemberName, returnType, parameterTypes );
		}

		private void DumpToCore()
		{
			this._assembly.Save( this._moduleFileName );
		}

		/// <summary>
		///		Genereates serialization methods which can be save to file.
		/// </summary>
		private sealed class DumpableSerializationMethodGenerator : SerializationMethodGenerator
		{
			private MethodInfo _runtimeMethodInfo;
			private readonly TypeBuilder _typeBuilder;
			private readonly MethodBuilder _methodBuilder;

			public DumpableSerializationMethodGenerator( ModuleBuilder host, int sequence, string operation, Type targetType, string targetMemberName, Type returnType, Type[] parameterTypes )
			{
				string methodName = Emittion.BuildMethodName( operation, targetType, targetMemberName );
				string typeName = String.Join( Type.Delimiter.ToString(), typeof( DumpableSerializationMethodGenerator ).Namespace, "Generated", methodName + "_Holder" + sequence );
				Tracer.Emit.TraceEvent( Tracer.EventType.DefineType, Tracer.EventId.DefineType, "Create {0}::{1}", methodName, typeName );
				this._typeBuilder =
					host.DefineType(
						typeName,
						TypeAttributes.Abstract | TypeAttributes.Sealed | TypeAttributes.Public | TypeAttributes.AutoClass | TypeAttributes.AutoLayout | TypeAttributes.BeforeFieldInit
					);

				this._methodBuilder =
					this._typeBuilder.DefineMethod(
						methodName,
						MethodAttributes.Public | MethodAttributes.Static,
						CallingConventions.Standard,
						returnType,
						parameterTypes
					);
			}

			private static TypeBuilder CreateTypeBuilder( ModuleBuilder host, string methodName )
			{
				return
					host.DefineType(
						String.Join( Type.Delimiter.ToString(), typeof( DumpableSerializationMethodGenerator ).Namespace, "Generated", methodName + "_Holder" ),
						TypeAttributes.Abstract | TypeAttributes.Sealed | TypeAttributes.Public | TypeAttributes.AutoClass | TypeAttributes.AutoLayout | TypeAttributes.BeforeFieldInit
					);
			}

			public sealed override TracingILGenerator GetILGenerator()
			{
				if ( IsTraceEnabled )
				{
					this.Trace.WriteLine( "{0}::{1}", MethodBase.GetCurrentMethod(), this._methodBuilder );
				}

				return new TracingILGenerator( this._methodBuilder, this.Trace );
			}

			protected sealed override TDelegate CreateDelegateCore<TDelegate>()
			{
				if ( this._runtimeMethodInfo == null )
				{
					this._runtimeMethodInfo = this._typeBuilder.CreateType().GetMethod( this._methodBuilder.Name );
				}


				return Delegate.CreateDelegate( typeof( TDelegate ), this._runtimeMethodInfo ) as TDelegate;
			}
		}
	}
}
