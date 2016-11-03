#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2016 FUJIWARA, Yusuke and contributors
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

#if !AOT
using System;
using System.Collections.Generic;
#if NETSTANDARD1_1
using Contract = MsgPack.MPContract;
using System.IO;
using System.Reflection;
#endif // NETSTANDARD1_1

using MsgPack.Serialization.AbstractSerializers;
#if !NETSTANDARD1_1
using MsgPack.Serialization.CodeDomSerializers;
using MsgPack.Serialization.EmittingSerializers;
#endif // !NETSTANDARD1_1

namespace MsgPack.Serialization
{
	partial class MessagePackSerializer
	{
		private static readonly IDictionary<EmitterFlavor, Func<Type, CollectionTraits, ISerializerBuilder>> _builderFactories =
			InitializeBuilderFactories();

		private static IDictionary<EmitterFlavor, Func<Type, CollectionTraits, ISerializerBuilder>> InitializeBuilderFactories()
		{
			// Static (non-customizable) naive plugin loader.

			var result = new Dictionary<EmitterFlavor, Func<Type, CollectionTraits, ISerializerBuilder>>(2);
#if !NETSTANDARD1_1
			result.Add( EmitterFlavor.FieldBased, ( t, c ) => new AssemblyBuilderSerializerBuilder( t, c ) );
			result.Add( EmitterFlavor.CodeDomBased, ( t, c ) => new CodeDomSerializerBuilder( t, c ) );
#else
			result.Add( EmitterFlavor.FieldBased, CreateFactory( LoadBuilderType( "MsgPack.RuntimeGeneration", "MsgPack.Sereialization.EmittingSerializers.AssemblyBuilderSerializerBuilder" ) ) );
			result.Add( EmitterFlavor.CodeDomBased, CreateFactory( LoadBuilderType( "MsgPack.CodeGeneration", "MsgPack.Sereialization.CodeDomSerializers.CodeDomSerializerBuilder" ) ) );
#endif // NETSTANDARD1_1
			return result;
		}

#if NETSTANDARD1_1
		private static readonly AssemblyName BaseAssemblyName = typeof(MessagePackSerializer).GetAssembly().GetName();

		private static Type LoadBuilderType( string name, string typeName )
		{
			var assemblyName =
				new AssemblyName( BaseAssemblyName.FullName )
				{
					Name = name
				};

			try
			{
				return Assembly.Load( assemblyName ).GetType( typeName );
			}
			catch ( IOException )
			{
				return null;
			}
			catch ( BadImageFormatException )
			{
				return null;
			}
		}

		private static readonly Type[] SerializerBuilderConstractorParameterTypes =
			new[] { typeof( Type ), typeof( CollectionTraits ) };

		private static Func<Type, CollectionTraits, ISerializerBuilder> CreateFactory( Type builderType )
		{
			if ( builderType == null )
			{
				return null;
			}

			var ctor = builderType.GetRuntimeConstructor( SerializerBuilderConstractorParameterTypes );
			Contract.Assert( ctor != null );
			return ctor.CreateConstructorDelegate<Func<Type, CollectionTraits, ISerializerBuilder>>();
		}

#endif // NETSTANDARD1_1

		private static ISerializerBuilder GetSerializerBuilder( Type targetType, SerializationContext context, CollectionTraits collectionTraits )
		{
			Func<Type, CollectionTraits, ISerializerBuilder> builderFactory;
			if ( !_builderFactories.TryGetValue( context.SerializerOptions.EmitterFlavor, out builderFactory ) || builderFactory == null )
			{
				return null;
			}

			return builderFactory( targetType, collectionTraits );
		}
	}
}
#endif // !AOT