#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2014 FUJIWARA, Yusuke
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

using MsgPack.Serialization.AbstractSerializers;
using MsgPack.Serialization.Reflection;

namespace MsgPack.Serialization.EmittingSerializers
{
	/// <summary>
	///		Genereates enum serialization methods which can be save to file.
	/// </summary>
	[ContractClass( typeof( EnumSerializerEmitterContract ) )]
	internal abstract class EnumSerializerEmitter : IDisposable
	{
		protected static readonly Type[] UnpackFromUnderlyingValueParameterTypes = { typeof( MessagePackObject ) };

		/// <summary>
		/// Initializes a new instance of the <see cref="SerializerEmitter"/> class.
		/// </summary>
		protected EnumSerializerEmitter() { }

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
			// nop
		}

		/// <summary>
		///		Gets the IL generator to implement <see cref="EnumMessagePackSerializer{TEnum}.PackUnderlyingValueTo"/> overrides.
		/// </summary>
		/// <returns>
		///		The IL generator to implement <see cref="EnumMessagePackSerializer{TEnum}.PackUnderlyingValueTo"/> overrides.
		///		This value will not be <c>null</c>.
		/// </returns>
		public abstract TracingILGenerator GetPackUnderyingValueToMethodILGenerator();

		/// <summary>
		///		Gets the IL generator to implement <see cref="EnumMessagePackSerializer{TEnum}.UnpackFromUnderlyingValue"/> overrides.
		/// </summary>
		/// <returns>
		///		The IL generator to implement <see cref="EnumMessagePackSerializer{TEnum}.UnpackFromUnderlyingValue"/> overrides.
		///		This value will not be <c>null</c>.
		/// </returns>
		public abstract TracingILGenerator GetUnpackFromUnderlyingValueMethodILGenerator();

		/// <summary>
		///		Creates the serializer type built now and returns its new instance.
		/// </summary>
		/// <typeparam name="T">Target type to be serialized/deserialized.</typeparam>
		/// <param name="context">The <see cref="SerializationContext"/> to holds serializers.</param>
		/// <param name="serializationMethod">The <see cref="EnumSerializationMethod"/> which determines serialization form of the enums.</param>
		/// <returns>
		///		Newly built <see cref="MessagePackSerializer{T}"/> instance.
		///		This value will not be <c>null</c>.
		///	</returns>
		public MessagePackSerializer<T> CreateInstance<T>( SerializationContext context, EnumSerializationMethod serializationMethod )
		{
			return this.CreateConstructor<T>()( context, serializationMethod );
		}

		/// <summary>
		///		Creates instance constructor delegates.
		/// </summary>
		/// <typeparam name="T">Target type to be serialized/deserialized.</typeparam>
		/// <returns>A delegate for serializer constructor.</returns>
		public abstract Func<SerializationContext, EnumSerializationMethod, MessagePackSerializer<T>> CreateConstructor<T>();
	}

	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Contract class" )]
	[ContractClassFor( typeof( EnumSerializerEmitter ) )]
	internal sealed class EnumSerializerEmitterContract : EnumSerializerEmitter
	{
		public override TracingILGenerator GetPackUnderyingValueToMethodILGenerator()
		{
			Contract.Ensures( Contract.Result<TracingILGenerator>() != null );
			return null;
		}

		public override TracingILGenerator GetUnpackFromUnderlyingValueMethodILGenerator()
		{
			Contract.Ensures( Contract.Result<TracingILGenerator>() != null );
			return null;
		}

		public override Func<SerializationContext, EnumSerializationMethod, MessagePackSerializer<T>> CreateConstructor<T>()
		{
			Contract.Ensures( Contract.Result<Func<PackerCompatibilityOptions, EnumSerializerMethod, MessagePackSerializer<T>>>() != null );
			return null;
		}
	}
}