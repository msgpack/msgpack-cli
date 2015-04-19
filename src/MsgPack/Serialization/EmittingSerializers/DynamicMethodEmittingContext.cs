#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2015 FUJIWARA, Yusuke
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

namespace MsgPack.Serialization.EmittingSerializers
{
	/// <summary>
	///		Implements <see cref="MsgPack.Serialization.AbstractSerializers.SerializerGenerationContext{TConstruct}"/> for <see cref="DynamicMethodSerializerBuilder{TObject}"/>.
	/// </summary>
	internal class DynamicMethodEmittingContext : ILEmittingContext
	{
		private readonly ILConstruct _context;

		/// <summary>
		///		Gets the code construct which represents 'context' parameter of generated methods.
		/// </summary>
		/// <value>
		///		The code construct which represents 'context' parameter of generated methods.
		///		Its type is <see cref="SerializationContext"/>, and it holds dependent serializers.
		///		This value will not be <c>null</c>.
		/// </value>
		public override ILConstruct Context { get { return this._context; } }

		/// <summary>
		///		Initializes a new instance of the <see cref="DynamicMethodEmittingContext"/> class.
		/// </summary>
		/// <param name="context">The serialization context.</param>
		/// <param name="targetType">Type of the serialization target.</param>
		/// <param name="emitterFactory">
		///		The factory for <see cref="SerializerEmitter"/> to be used.
		/// </param>
		/// <param name="enumEmitterFactory">
		///		The factory for <see cref="EnumSerializerEmitter"/> to be used.
		/// </param>
		public DynamicMethodEmittingContext( SerializationContext context, Type targetType,
			Func<SerializerEmitter> emitterFactory, Func<EnumSerializerEmitter> enumEmitterFactory )
			: base( context, emitterFactory, enumEmitterFactory )
		{
			this._context = ILConstruct.Argument( 0, typeof( SerializationContext ), "context" );
			this.Reset( targetType, null );
		}
	}
}