#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2014 FUJIWARA, Yusuke
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
using System.Reflection;
using System.Reflection.Emit;
using System.Security;
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
		private static readonly object[] _debuggableAttributeCtorArguments = { true, true };

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
			get { return _canCollect; }
		}

		private static DefaultSerializationMethodGeneratorManager _canDump = new DefaultSerializationMethodGeneratorManager( true, false, null );

		/// <summary>
		///		Get the singleton instance for can-dump mode.
		/// </summary>
		public static DefaultSerializationMethodGeneratorManager CanDump
		{
			get { return _canDump; }
		}
#endif

		private static DefaultSerializationMethodGeneratorManager _fast = new DefaultSerializationMethodGeneratorManager( false, false, null );

		/// <summary>
		///		Get the singleton instance for fast mode.
		/// </summary>
		public static DefaultSerializationMethodGeneratorManager Fast
		{
			get { return _fast; }
		}

		internal static void Refresh()
		{
#if !SILVERLIGHT
			_canCollect = new DefaultSerializationMethodGeneratorManager( false, true, null );
			_canDump = new DefaultSerializationMethodGeneratorManager( true, false, null );
#endif
			_fast = new DefaultSerializationMethodGeneratorManager( false, false, null );
		}

#if !WINDOWS_PHONE
		// ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
		private readonly AssemblyBuilder _assembly;
		private readonly ModuleBuilder _module;
		private readonly bool _isDebuggable;
		private readonly bool _isExternalAssemblyBuilder;
#endif

#if WINDOWS_PHONE
		private DefaultSerializationMethodGeneratorManager( bool isDebuggable, bool isCollectable, object assemblyBuilder )
		{
		}
#else
#if SILVERLIGHT || NETFX_35
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "isCollectable", Justification = "Used in other platforms" )]
#endif // SILVERLIGHT
#if !NETFX_35
		[SecuritySafeCritical]
#endif // !NETFX_35
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

#if SILVERLIGHT
			this._module = this._assembly.DefineDynamicModule( assemblyName, true );
#else
			if ( isDebuggable )
			{
				this._module = this._assembly.DefineDynamicModule( assemblyName, assemblyName + ".dll", true );
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
						// ReSharper disable once AssignNullToNotNullAttribute
						typeof( DebuggableAttribute ).GetConstructor( new[] { typeof( DebuggableAttribute.DebuggingModes ) } ),
						new object[] { DebuggableAttribute.DebuggingModes.IgnoreSymbolStoreSequencePoints }
					)
				);
			}

			dedicatedAssemblyBuilder.SetCustomAttribute(
				new CustomAttributeBuilder(
					// ReSharper disable once AssignNullToNotNullAttribute
					typeof( System.Runtime.CompilerServices.CompilationRelaxationsAttribute ).GetConstructor( new[] { typeof( int ) } ),
					new object[] { 8 }
				)
			);
#if !SILVERLIGHT && !NETFX_35
			dedicatedAssemblyBuilder.SetCustomAttribute(
				new CustomAttributeBuilder(
					// ReSharper disable once AssignNullToNotNullAttribute
					typeof( SecurityRulesAttribute ).GetConstructor( new[] { typeof( SecurityRuleSet ) } ),
					new object[] { SecurityRuleSet.Level2 },
					new[] { typeof( SecurityRulesAttribute ).GetProperty( "SkipVerificationInFullTrust" ) },
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
#endif

		/// <summary>
		///		Creates new <see cref="SerializerEmitter"/> which corresponds to the specified <see cref="EmitterFlavor"/>.
		/// </summary>
		/// <param name="targetType">The type of the serialization target.</param>
		/// <param name="emitterFlavor"><see cref="EmitterFlavor"/>.</param>
		/// <returns>
		///		New <see cref="SerializerEmitter"/> which corresponds to the specified <see cref="EmitterFlavor"/>.
		/// </returns>
		protected override SerializerEmitter CreateEmitterCore( Type targetType, EmitterFlavor emitterFlavor )
		{
#if !WINDOWS_PHONE
			switch ( emitterFlavor )
			{
				case EmitterFlavor.FieldBased:
				{
					return new FieldBasedSerializerEmitter( this._module, this._isExternalAssemblyBuilder ? default( int? ) : Interlocked.Increment( ref this._typeSequence ), targetType, this._isDebuggable );
				}
				default:
				{
					return new ContextBasedSerializerEmitter( targetType );
				}
			}
#else
			return new ContextBasedSerializerEmitter( targetType );
#endif
		}

		protected override EnumSerializerEmitter CreateEnumEmitterCore( Type targetType, EmitterFlavor emitterFlavor )
		{
#if !WINDOWS_PHONE
			switch ( emitterFlavor )
			{
				case EmitterFlavor.FieldBased:
				{
					return new FieldBasedEnumSerializerEmitter( this._module, this._isExternalAssemblyBuilder ? default( int? ) : Interlocked.Increment( ref this._typeSequence ), targetType, this._isDebuggable );
				}
				default:
				{
					return new ContextBasedEnumSerializerEmitter( targetType );
				}
			}
#else
			return new ContextBasedEnumSerializerEmitter( targetType );
#endif
		}
	}
}