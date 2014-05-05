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
using System.Collections.Generic;
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
		protected static readonly Type[] ParseParameterTypes = { typeof( String ) };

		/// <summary>
		///		Flushes the trace.
		/// </summary>
		[Obsolete]
		public void FlushTrace()
		{
			// nop
		}

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
		///		Gets the IL generator to implement <see cref="EnumMessagePackSerializer{TEnum}.GetUnderlyingValueString"/> overrides.
		/// </summary>
		/// <returns>
		///		The IL generator to implement <see cref="EnumMessagePackSerializer{TEnum}.GetUnderlyingValueString"/> overrides.
		///		This value will not be <c>null</c>.
		/// </returns>
		public abstract TracingILGenerator GetGetUnderlyingValueStringMethodILGenerator();

		/// <summary>
		///		Gets the IL generator to implement <see cref="EnumMessagePackSerializer{TEnum}.UnpackFromUnderlyingValue"/> overrides.
		/// </summary>
		/// <returns>
		///		The IL generator to implement <see cref="EnumMessagePackSerializer{TEnum}.UnpackFromUnderlyingValue"/> overrides.
		///		This value will not be <c>null</c>.
		/// </returns>
		public abstract TracingILGenerator GetUnpackFromUnderlyingValueMethodILGenerator();

		/// <summary>
		///		Gets the IL generator to implement <see cref="EnumMessagePackSerializer{TEnum}.Parse"/> overrides.
		/// </summary>
		/// <returns>
		///		The IL generator to implement <see cref="EnumMessagePackSerializer{TEnum}.Parse"/> overrides.
		///		This value will not be <c>null</c>.
		/// </returns>
		public abstract TracingILGenerator GetParseMethodILGenerator();

		/// <summary>
		///		Creates the serializer type built now and returns its new instance.
		/// </summary>
		/// <typeparam name="T">Target type to be serialized/deserialized.</typeparam>
		/// <param name="context">The <see cref="SerializationContext"/> to holds serializers.</param>
		/// <param name="serializationMethod">The <see cref="EnumSerializationMethod"/> which determines serialization form of the enums.</param>
		/// <param name="enumNames">The names of enum members. The elements are corresponds to <paramref name="enumValues"/>.</param>
		/// <param name="enumValues">The names of enum values. The elements are corresponds to <paramref name="enumNames"/>.</param>
		/// <returns>
		///		Newly built <see cref="MessagePackSerializer{T}"/> instance.
		///		This value will not be <c>null</c>.
		///	</returns>
		public EnumMessagePackSerializer<T> CreateInstance<T>( SerializationContext context, EnumSerializationMethod serializationMethod, IList<string> enumNames, IList<T> enumValues )
		{
			return this.CreateConstructor<T>()( context, serializationMethod, enumNames, enumValues );
		}

		/// <summary>
		///		Creates instance constructor delegates.
		/// </summary>
		/// <typeparam name="T">Target type to be serialized/deserialized.</typeparam>
		/// <returns>A delegate for serializer constructor.</returns>
		public abstract Func<SerializationContext, EnumSerializationMethod, IList<String>, IList<T>, EnumMessagePackSerializer<T>> CreateConstructor<T>();
	}

	[ContractClassFor( typeof( EnumSerializerEmitter ) )]
	internal sealed class EnumSerializerEmitterContract : EnumSerializerEmitter
	{
		public override TracingILGenerator GetPackUnderyingValueToMethodILGenerator()
		{
			Contract.Ensures( Contract.Result<TracingILGenerator>() != null );
			return null;
		}

		public override TracingILGenerator GetGetUnderlyingValueStringMethodILGenerator()
		{
			Contract.Ensures( Contract.Result<TracingILGenerator>() != null );
			return null;
		}

		public override TracingILGenerator GetUnpackFromUnderlyingValueMethodILGenerator()
		{
			Contract.Ensures( Contract.Result<TracingILGenerator>() != null );
			return null;
		}

		public override TracingILGenerator GetParseMethodILGenerator()
		{
			Contract.Ensures( Contract.Result<TracingILGenerator>() != null );
			return null;
		}

		public override Func<SerializationContext, EnumSerializationMethod, IList<string>, IList<T>, EnumMessagePackSerializer<T>> CreateConstructor<T>()
		{
			Contract.Ensures( Contract.Result<Func<PackerCompatibilityOptions, EnumSerializerMethod, IList<string>, IList<T>, MessagePackSerializer<T>>>() != null );
			return null;
		}
	}
}