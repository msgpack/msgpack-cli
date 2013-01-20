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
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;

namespace MsgPack.Serialization.EmittingSerializers
{
	/// <summary>
	///		Manages serializer generators.
	/// </summary>
	internal sealed class DefaultSerializationMethodGeneratorManager : SerializationMethodGeneratorManager
	{
		private static readonly ConstructorInfo _debuggableAttributeCtor =
			typeof( DebuggableAttribute ).GetConstructor( new[] { typeof( bool ), typeof( bool ) } );
		private static readonly object[] _debuggableAttributeCtorArguments = new object[] { true, true };

#if !WINDOWS_PHONE
		private static int _assemblySequence = -1;
		private int _typeSequence = -1;
#endif

#if !SILVERLIGHT
		private static DefaultSerializationMethodGeneratorManager _canCollect = new DefaultSerializationMethodGeneratorManager( false, true, null );

		/// <summary>
		///		Get the singleton instance for can-collect mode.
		/// </summary>
		public static DefaultSerializationMethodGeneratorManager CanCollect
		{
			get { return DefaultSerializationMethodGeneratorManager._canCollect; }
		}

		private static DefaultSerializationMethodGeneratorManager _canDump = new DefaultSerializationMethodGeneratorManager( true, false, null );

		/// <summary>
		///		Get the singleton instance for can-dump mode.
		/// </summary>
		public static DefaultSerializationMethodGeneratorManager CanDump
		{
			get { return DefaultSerializationMethodGeneratorManager._canDump; }
		}
#endif

		private static DefaultSerializationMethodGeneratorManager _fast = new DefaultSerializationMethodGeneratorManager( false, false, null );

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
			_canCollect = new DefaultSerializationMethodGeneratorManager( false, true, null );
			_canDump = new DefaultSerializationMethodGeneratorManager( true, false, null );
#endif
			_fast = new DefaultSerializationMethodGeneratorManager( false, false, null );
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

#if !WINDOWS_PHONE
		private readonly AssemblyBuilder _assembly;
		private readonly ModuleBuilder _module;
		private readonly string _moduleFileName;
		private readonly bool _isDebuggable;
		private readonly bool _isExternalAssemblyBuilder;
#endif

#if WINDOWS_PHONE
		private DefaultSerializationMethodGeneratorManager( bool isDebuggable, bool isCollectable, object assemblyBuilder )
		{
		}
#else
		private DefaultSerializationMethodGeneratorManager( bool isDebuggable, bool isCollectable, AssemblyBuilder assemblyBuilder )
		{
			this._isDebuggable = isDebuggable;
			this._isExternalAssemblyBuilder = assemblyBuilder != null;

			string assemblyName;
			if ( assemblyBuilder != null )
			{
				assemblyName = assemblyBuilder.GetName( false ).Name;
				this._assembly = assemblyBuilder;
			}
			else
			{
				assemblyName = typeof( DefaultSerializationMethodGeneratorManager ).Namespace + ".GeneratedSerealizers" + Interlocked.Increment( ref _assemblySequence );
				var dedicatedAssemblyBuilder =
					AppDomain.CurrentDomain.DefineDynamicAssembly(
						new AssemblyName( assemblyName ),
#if !SILVERLIGHT
						isDebuggable
						? AssemblyBuilderAccess.RunAndSave
#if !NETFX_35
						: ( isCollectable ? AssemblyBuilderAccess.RunAndCollect : AssemblyBuilderAccess.Run )
#else
						: AssemblyBuilderAccess.Run
#endif
#else
						AssemblyBuilderAccess.Run 
#endif
					);

				SetUpAssemblyBuilderAttributes( dedicatedAssemblyBuilder, isDebuggable );
				this._assembly = dedicatedAssemblyBuilder;
			}

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
#endif // else SILVERLIGHT
		}
#endif // !WINDOWS_PHONE

#if !WINDOWS_PHONE
		internal static void SetUpAssemblyBuilderAttributes( AssemblyBuilder dedicatedAssemblyBuilder, bool isDebuggable )
		{
			if ( isDebuggable )
			{
				dedicatedAssemblyBuilder.SetCustomAttribute( new CustomAttributeBuilder( _debuggableAttributeCtor, _debuggableAttributeCtorArguments ) );
			}
			else
			{
				dedicatedAssemblyBuilder.SetCustomAttribute(
					new CustomAttributeBuilder(
						typeof( DebuggableAttribute ).GetConstructor( new[] { typeof( DebuggableAttribute.DebuggingModes ) } ),
						new object[] { DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints }
					)
				);
			}

			dedicatedAssemblyBuilder.SetCustomAttribute(
				new CustomAttributeBuilder(
					typeof( System.Runtime.CompilerServices.CompilationRelaxationsAttribute ).GetConstructor( new[] { typeof( int ) } ),
					new object[] { 8 }
				)
			);
#if !SILVERLIGHT && !NETFX_35
			dedicatedAssemblyBuilder.SetCustomAttribute(
				new CustomAttributeBuilder(
					typeof( System.Security.SecurityRulesAttribute ).GetConstructor( new[] { typeof( System.Security.SecurityRuleSet ) } ),
					new object[] { System.Security.SecurityRuleSet.Level2 },
					new[] { typeof( System.Security.SecurityRulesAttribute ).GetProperty( "SkipVerificationInFullTrust" ) },
					new object[] { true }
				)
			);
#endif // !SILVERLIGHT
		}
#endif // !WINDOWS_PHONE

#if !SILVERLIGHT
		/// <summary>
		///		Create a new dumpable <see cref="SerializationMethodGeneratorManager"/> with specified brandnew assembly builder.
		/// </summary>
		/// <param name="assemblyBuilder">An assembly builder which will store all generated types.</param>
		/// <returns>
		///		The appropriate <see cref="SerializationMethodGeneratorManager"/> to generate pre-cimplied serializers.
		///		This value will not be <c>null</c>.
		///	</returns>
		public static SerializationMethodGeneratorManager Create( AssemblyBuilder assemblyBuilder )
		{
			return new DefaultSerializationMethodGeneratorManager( true, false, assemblyBuilder );
		}

		private void DumpToCore()
		{
			this._assembly.Save( this._moduleFileName );
		}
#endif

		/// <summary>
		///		Creates new <see cref="SerializerEmitter"/> which corresponds to the specified <see cref="EmitterFlavor"/>.
		/// </summary>
		/// <param name="targetType">The type of the serialization target.</param>
		/// <param name="emitterFlavor"><see cref="EmitterFlavor"/>.</param>
		/// <returns>
		///		New <see cref="SerializerEmitter"/> which corresponds to the specified <see cref="EmitterFlavor"/>.
		/// </returns>
		protected sealed override SerializerEmitter CreateEmitterCore( Type targetType, EmitterFlavor emitterFlavor )
		{
#if !WINDOWS_PHONE
			switch ( emitterFlavor )
			{
				case EmitterFlavor.FieldBased:
				{
					return new FieldBasedSerializerEmitter( this._module, this._isExternalAssemblyBuilder ? default( int? ) : Interlocked.Increment( ref this._typeSequence ), targetType, this._isDebuggable );
				}
				case EmitterFlavor.ContextBased:
				default:
				{
					return new ContextBasedSerializerEmitter( targetType );
				}
			}
#else
			return new ContextBasedSerializerEmitter( targetType );
#endif
		}
	}
}