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

namespace MsgPack.Serialization
{
	/// <summary>
	///		Manages serializer generators.
	/// </summary>
	internal sealed class DefaultSerializationMethodGeneratorManager : SerializationMethodGeneratorManager
	{
		private static readonly ConstructorInfo _debuggableAttributeCtor =
			typeof( DebuggableAttribute ).GetConstructor( new[] { typeof( bool ), typeof( bool ) } );
		private static readonly object[] _debuggableAttributeCtorArguments = new object[] { true, true };
		private static int _assemblySequence = -1;
		private int _typeSequence = -1;

#if !SILVERLIGHT
		private static DefaultSerializationMethodGeneratorManager _canCollect = new DefaultSerializationMethodGeneratorManager( false, true );

		/// <summary>
		///		Get the singleton instance for can-collect mode.
		/// </summary>
		public static DefaultSerializationMethodGeneratorManager CanCollect
		{
			get { return DefaultSerializationMethodGeneratorManager._canCollect; }
		}

		private static DefaultSerializationMethodGeneratorManager _canDump = new DefaultSerializationMethodGeneratorManager( true, false );

		/// <summary>
		///		Get the singleton instance for can-dump mode.
		/// </summary>
		public static DefaultSerializationMethodGeneratorManager CanDump
		{
			get { return DefaultSerializationMethodGeneratorManager._canDump; }
		}
#endif

		private static DefaultSerializationMethodGeneratorManager _fast = new DefaultSerializationMethodGeneratorManager( false, false );

		/// <summary>
		///		Get the singleton instance for fast mode.
		/// </summary>
		public static DefaultSerializationMethodGeneratorManager Fast
		{
			get { return DefaultSerializationMethodGeneratorManager._fast; }
		}

		internal static void Refresh()
		{
#if !SILVERLIGHT
			_canCollect = new DefaultSerializationMethodGeneratorManager( false, true );
			_canDump = new DefaultSerializationMethodGeneratorManager( true, false );
#endif
			_fast = new DefaultSerializationMethodGeneratorManager( false, false );
		}

#if !SILVERLIGHT
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
#endif

		private readonly AssemblyBuilder _assembly;
		private readonly ModuleBuilder _module;
		private readonly string _moduleFileName;
		private readonly bool _isDebuggable;

		private DefaultSerializationMethodGeneratorManager( bool isDebuggable, bool isCollectable )
		{
			this._isDebuggable = isDebuggable;

			var assemblyName = typeof( DefaultSerializationMethodGeneratorManager ).Namespace + ".GeneratedSerealizers" + Interlocked.Increment( ref _assemblySequence );
			this._assembly =
				AppDomain.CurrentDomain.DefineDynamicAssembly(
					new AssemblyName( assemblyName ),
#if !SILVERLIGHT
					isDebuggable ? AssemblyBuilderAccess.RunAndSave : ( isCollectable ? AssemblyBuilderAccess.RunAndCollect : AssemblyBuilderAccess.Run )
#else
					AssemblyBuilderAccess.Run 
#endif
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
#if !SILVERLIGHT
			this._assembly.SetCustomAttribute(
				new CustomAttributeBuilder(
					typeof( System.Security.SecurityRulesAttribute ).GetConstructor( new[] { typeof( System.Security.SecurityRuleSet ) } ),
					new object[] { System.Security.SecurityRuleSet.Level2 },
					new[] { typeof( System.Security.SecurityRulesAttribute ).GetProperty( "SkipVerificationInFullTrust" ) },
					new object[] { true }
				)
			);
#endif

			this._moduleFileName = assemblyName + ".dll";

#if SILVERLIGHT
			this._module = this._assembly.DefineDynamicModule( assemblyName, true );
#else
			if ( isDebuggable )
			{
				this._module = this._assembly.DefineDynamicModule( assemblyName, this._moduleFileName, true );
			}
			else
			{
				this._module = this._assembly.DefineDynamicModule( assemblyName, true );
			}
#endif
		}

#if !SILVERLIGHT
		private void DumpToCore()
		{
			this._assembly.Save( this._moduleFileName );
		}
#endif

		protected sealed override SerializerEmitter CreateEmitterCore( Type targetType )
		{
			return new SerializerEmitter( this._module, Interlocked.Increment( ref this._typeSequence ), targetType, this._isDebuggable );
		}
	}
}