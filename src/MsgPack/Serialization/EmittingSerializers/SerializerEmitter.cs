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
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Reflection;

using MsgPack.Serialization.Reflection;

namespace MsgPack.Serialization.EmittingSerializers
{
	/// <summary>
	///		Genereates serialization methods which can be save to file.
	/// </summary>
	[ContractClass( typeof( SerializerEmitterContract ) )]
	internal abstract class SerializerEmitter : IDisposable
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SerializerEmitter"/> class.
		/// </summary>
		protected SerializerEmitter() { }

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
		///		Gets the IL generator to implement <see cref="MessagePackSerializer{T}.PackToCore"/> overrides.
		/// </summary>
		/// <returns>
		///		The IL generator to implement <see cref="MessagePackSerializer{T}.PackToCore"/> overrides.
		///		This value will not be <c>null</c>.
		/// </returns>
		public abstract TracingILGenerator GetPackToMethodILGenerator();

		/// <summary>
		///		Gets the IL generator to implement <see cref="MessagePackSerializer{T}.UnpackFromCore"/> overrides.
		/// </summary>
		/// <returns>
		///		The IL generator to implement <see cref="MessagePackSerializer{T}.UnpackFromCore"/> overrides.
		///		This value will not be <c>null</c>.
		/// </returns>
		public abstract TracingILGenerator GetUnpackFromMethodILGenerator();

		/// <summary>
		///		Gets the IL generator to implement <see cref="MessagePackSerializer{T}.UnpackToCore"/> overrides.
		/// </summary>
		/// <returns>
		///		The IL generator to implement <see cref="MessagePackSerializer{T}.UnpackToCore"/> overrides.
		/// </returns>
		/// <remarks>
		///		When this method is called, <see cref="MessagePackSerializer{T}.UnpackToCore"/> will be overridden.
		///		This value will not be <c>null</c>.
		/// </remarks>
		public abstract TracingILGenerator GetUnpackToMethodILGenerator();

		/// <summary>
		///		Gets the IL generator to implement AddItem(TCollection, TItem) or AddItem(TCollection, object) overrides.
		/// </summary>
		/// <param name="declaration">The virtual method declaration to be overriden.</param>
		/// <returns>
		///		The IL generator to implement AddItem(TCollection, TItem) or AddItem(TCollection, object) overrides.
		///		This value will not be <c>null</c>.
		/// </returns>
		public abstract TracingILGenerator GetAddItemMethodILGenerator( MethodInfo declaration );

		/// <summary>
		///		Gets the IL generator to implement CreateInstance(int) overrides.
		/// </summary>
		/// <param name="declaration">The virtual method declaration to be overriden.</param>
		/// <returns>
		///		The IL generator to implement CreateInstance(int) overrides.
		///		This value will not be <c>null</c>.
		/// </returns>
		public abstract TracingILGenerator GetCreateInstanceMethodILGenerator( MethodInfo declaration );
		
		/// <summary>
		///		Gets the IL generator to implement private static RestoreSchema() method.
		/// </summary>
		/// <returns>
		///		The IL generator to implement RestoreSchema() static method.
		///		This value will not be <c>null</c>.
		/// </returns>
		public abstract TracingILGenerator GetRestoreSchemaMethodILGenerator();

		/// <summary>
		///		Creates the serializer type built now and returns its new instance.
		/// </summary>
		/// <typeparam name="T">Target type to be serialized/deserialized.</typeparam>
		/// <param name="context">The <see cref="SerializationContext"/> to holds serializers.</param>
		/// <param name="schema">The <see cref="PolymorphismSchema"/> for this instance.</param>
		/// <returns>
		///		Newly built <see cref="MessagePackSerializer{T}"/> instance.
		///		This value will not be <c>null</c>.
		///	</returns>
		public MessagePackSerializer<T> CreateInstance<T>( SerializationContext context, PolymorphismSchema schema )
		{
			return this.CreateConstructor<T>()( context, schema );
		}

		/// <summary>
		///		Creates instance constructor delegates.
		/// </summary>
		/// <typeparam name="T">Target type to be serialized/deserialized.</typeparam>
		/// <returns>A delegate for serializer constructor.</returns>
		public abstract Func<SerializationContext, PolymorphismSchema, MessagePackSerializer<T>> CreateConstructor<T>();

		/// <summary>
		///		Regisgters <see cref="MessagePackSerializer{T}"/> of target type usage to the current emitting session.
		/// </summary>
		/// <param name="targetType">The type of the member to be serialized/deserialized.</param>
		/// <param name="enumMemberSerializationMethod">The enum serialization method of the member to be serialized/deserialized.</param>
		/// <param name="dateTimeConversionMethod">The date time conversion method of the member to be serialized/deserialized.</param>
		/// <param name="polymorphismSchema">The schema for polymorphism support.</param>
		/// <param name="schemaRegenerationCodeProvider">The delegate to provide constructs to emit schema regeneration codes.</param>
		/// <returns>
		///		<see cref=" Action{T1,T2}"/> to emit serializer retrieval instructions.
		///		The 1st argument should be <see cref="TracingILGenerator"/> to emit instructions.
		///		The 2nd argument should be argument index of the serializer holder, normally 0 (this pointer).
		///		This value will not be <c>null</c>.
		/// </returns>
		public abstract Action<TracingILGenerator, int> RegisterSerializer(
			Type targetType,
			EnumMemberSerializationMethod enumMemberSerializationMethod,
			DateTimeMemberConversionMethod dateTimeConversionMethod,
			PolymorphismSchema polymorphismSchema,
			Func<IEnumerable<ILConstruct>> schemaRegenerationCodeProvider
		);

		/// <summary>
		///		Regisgters <see cref="FieldInfo"/> usage to the current emitting session.
		/// </summary>
		/// <param name="field">The <see cref="FieldInfo"/> to be registered.</param>
		/// <returns>
		///		<see cref=" Action{T1,T2}"/> to emit serializer retrieval instructions.
		///		The 1st argument should be <see cref="TracingILGenerator"/> to emit instructions.
		///		The 2nd argument should be argument index of the serializer holder, normally 0 (this pointer).
		///		This value will not be <c>null</c>.
		/// </returns>
		public virtual Action<TracingILGenerator, int> RegisterField( FieldInfo field )
		{
			throw new NotSupportedException();
		}

		/// <summary>
		///		Regisgters <see cref="MethodBase"/> usage to the current emitting session.
		/// </summary>
		/// <param name="method">The <see cref="MethodBase"/> to be registered.</param>
		/// <returns>
		///		<see cref=" Action{T1,T2}"/> to emit serializer retrieval instructions.
		///		The 1st argument should be <see cref="TracingILGenerator"/> to emit instructions.
		///		The 2nd argument should be argument index of the serializer holder, normally 0 (this pointer).
		///		This value will not be <c>null</c>.
		/// </returns>
		public virtual Action<TracingILGenerator, int> RegisterMethod( MethodBase method )
		{
			throw new NotSupportedException();
		}
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

		public override TracingILGenerator GetAddItemMethodILGenerator( MethodInfo declaration )
		{
			Contract.Requires( declaration != null );
			Contract.Ensures( Contract.Result<TracingILGenerator>() != null );
			return null;
		}

		public override TracingILGenerator GetCreateInstanceMethodILGenerator( MethodInfo declaration )
		{
			Contract.Requires( declaration != null );
			Contract.Ensures( Contract.Result<TracingILGenerator>() != null );
			return null;
		}

		public override TracingILGenerator GetRestoreSchemaMethodILGenerator()
		{
			Contract.Ensures( Contract.Result<TracingILGenerator>() != null );
			return null;
		}

		public override Func<SerializationContext, PolymorphismSchema, MessagePackSerializer<T>> CreateConstructor<T>()
		{
			Contract.Ensures( Contract.Result<Func<SerializationContext, PolymorphismSchema, MessagePackSerializer<T>>>() != null );
			return null;
		}

		public override Action<TracingILGenerator, int> RegisterSerializer(
			Type targetType,
			EnumMemberSerializationMethod enumMemberSerializationMethod,
			DateTimeMemberConversionMethod dateTimeConversionMethod,
			PolymorphismSchema polymorphismSchema,
			Func<IEnumerable<ILConstruct>> schemaRegenerationCodeProvider
		)
		{
			Contract.Requires( targetType != null );
			Contract.Requires( Enum.IsDefined( typeof( EnumMemberSerializationMethod ), enumMemberSerializationMethod ) );
			Contract.Requires( Enum.IsDefined( typeof( DateTimeMemberConversionMethod ), dateTimeConversionMethod ) );
			Contract.Ensures( Contract.Result<Action<TracingILGenerator, int>>() != null );
			return null;
		}
	}
}
