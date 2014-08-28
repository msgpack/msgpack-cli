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
using System.Diagnostics.Contracts;
using System.Reflection.Emit;

namespace MsgPack.Serialization.EmittingSerializers
{
	/// <summary>
	///		Defines common features and interfaces for <see cref="SerializationMethodGeneratorManager"/>.
	/// </summary>
	internal abstract class SerializationMethodGeneratorManager
	{
		/// <summary>
		///		Get the appropriate <see cref="SerializationMethodGeneratorManager"/> for the current configuration.
		/// </summary>
		/// <returns>
		///		The appropriate <see cref="SerializationMethodGeneratorManager"/> for the current configuration.
		///		This value will not be <c>null</c>.
		///	</returns>
		public static SerializationMethodGeneratorManager Get()
		{
#if SILVERLIGHT
			return Get( SerializationMethodGeneratorOption.Fast );
#else
			return Get( SerializerDebugging.DumpEnabled ? SerializationMethodGeneratorOption.CanDump : SerializationMethodGeneratorOption.Fast );
#endif
		}

		/// <summary>
		///		Get the appropriate <see cref="SerializationMethodGeneratorManager"/> for specified options.
		/// </summary>
		/// <param name="option"><see cref="SerializationMethodGeneratorOption"/>.</param>
		/// <returns>
		///		The appropriate <see cref="SerializationMethodGeneratorManager"/> for specified options. 
		///		This value will not be <c>null</c>.
		///	</returns>
#if SILVERLIGHT
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "option", Justification = "Used in other platform" )]
#endif // SILVERLIGHT
		public static SerializationMethodGeneratorManager Get( SerializationMethodGeneratorOption option )
		{
#if SILVERLIGHT
			return DefaultSerializationMethodGeneratorManager.Fast;
#else
			switch ( option )
			{
				case SerializationMethodGeneratorOption.CanDump:
				{
					return DefaultSerializationMethodGeneratorManager.CanDump;
				}
				case SerializationMethodGeneratorOption.CanCollect:
				{
					return DefaultSerializationMethodGeneratorManager.CanCollect;
				}
				default:
				{
					return DefaultSerializationMethodGeneratorManager.Fast;
				}
			}
#endif
		}

#if !SILVERLIGHT
		/// <summary>
		///		Get the dumpable <see cref="SerializationMethodGeneratorManager"/> with specified brandnew assembly builder.
		/// </summary>
		/// <param name="assemblyBuilder">An assembly builder which will store all generated types.</param>
		/// <returns>
		///		The appropriate <see cref="SerializationMethodGeneratorManager"/> to generate pre-cimplied serializers.
		///		This value will not be <c>null</c>.
		///	</returns>
		public static SerializationMethodGeneratorManager Get( AssemblyBuilder assemblyBuilder )
		{
			return DefaultSerializationMethodGeneratorManager.Create( assemblyBuilder );
		}
#endif

		/// <summary>
		///		Creates new <see cref="SerializerEmitter"/> which corresponds to the specified <see cref="EmitterFlavor"/>.
		/// </summary>
		/// <param name="targetType">The type of the serialization target.</param>
		/// <param name="emitterFlavor"><see cref="EmitterFlavor"/>.</param>
		/// <returns>New <see cref="SerializerEmitter"/> which corresponds to the specified <see cref="EmitterFlavor"/>.</returns>
		public SerializerEmitter CreateEmitter( Type targetType, EmitterFlavor emitterFlavor )
		{
			Contract.Requires( targetType != null );
			Contract.Ensures( Contract.Result<SerializerEmitter>() != null );

			return this.CreateEmitterCore( targetType, emitterFlavor );
		}

		/// <summary>
		///		Creates new <see cref="SerializerEmitter"/> which corresponds to the specified <see cref="EmitterFlavor"/>.
		/// </summary>
		/// <param name="targetType">The type of the serialization target.</param>
		/// <param name="emitterFlavor"><see cref="EmitterFlavor"/>.</param>
		/// <returns>New <see cref="SerializerEmitter"/> which corresponds to the specified <see cref="EmitterFlavor"/>.</returns>
		protected abstract SerializerEmitter CreateEmitterCore( Type targetType, EmitterFlavor emitterFlavor );


		/// <summary>
		///		Creates new <see cref="EnumSerializerEmitter"/> which corresponds to the specified <see cref="EmitterFlavor"/>.
		/// </summary>
		/// <param name="targetType">The type of the serialization target.</param>
		/// <param name="emitterFlavor"><see cref="EmitterFlavor"/>.</param>
		/// <returns>New <see cref="EnumSerializerEmitter"/> which corresponds to the specified <see cref="EmitterFlavor"/>.</returns>
		public EnumSerializerEmitter CreateEnumEmitter( Type targetType, EmitterFlavor emitterFlavor )
		{
			Contract.Requires( targetType != null );
			Contract.Ensures( Contract.Result<EnumSerializerEmitter>() != null );

			return this.CreateEnumEmitterCore( targetType, emitterFlavor );
		}

		/// <summary>
		///		Creates new <see cref="EnumSerializerEmitter"/> which corresponds to the specified <see cref="EmitterFlavor"/>.
		/// </summary>
		/// <param name="targetType">The type of the serialization target.</param>
		/// <param name="emitterFlavor"><see cref="EmitterFlavor"/>.</param>
		/// <returns>New <see cref="SerializerEmitter"/> which corresponds to the specified <see cref="EmitterFlavor"/>.</returns>
		protected abstract EnumSerializerEmitter CreateEnumEmitterCore( Type targetType, EmitterFlavor emitterFlavor );
	}
}
