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
using System.Reflection;
using System.Reflection.Emit;
using NLiblet.Reflection;

namespace MsgPack.Serialization
{
	/// <summary>
	///		Manages <see cref="SerializationMethodGenerator"/> which generates colletable serialization methods.
	/// </summary>
	internal sealed class CollectableSerializationMethodGeneratorManager : SerializationMethodGeneratorManager
	{
		/// <summary>
		///		Singleton instance.
		/// </summary>
		public static readonly CollectableSerializationMethodGeneratorManager Instance = new CollectableSerializationMethodGeneratorManager();

		private CollectableSerializationMethodGeneratorManager() { }

		/// <summary>
		///		Create the new <see cref="SerializationMethodGenerator"/> instance.
		/// </summary>
		/// <param name="operation">The name of the operation. This value may not be <c>null</c> nor empty.</param>
		/// <param name="targetType">The target type of serialization. This value may not be <c>null</c>.</param>
		/// <param name="targetMemberName">The name of the target member of serialization. This value may not be <c>null</c> nor empty.</param>
		/// <param name="returnType">The return type of the generating method.</param>
		/// <param name="parameterTypes">The parameter types of the generating method.</param>
		/// <returns>
		///		The new <see cref="SerializationMethodGenerator"/> instance.
		/// </returns>
		protected sealed override SerializationMethodGenerator CreateGeneratorCore( string operation, Type targetType, string targetMemberName, Type returnType, params Type[] parameterTypes )
		{
			return new CollectableSerializationMethodGenerator( operation, targetType, targetMemberName, returnType, parameterTypes );
		}

		/// <summary>
		///		Genereates serialization methods which is collectable by GC.
		/// </summary>
		private sealed class CollectableSerializationMethodGenerator : SerializationMethodGenerator
		{
			private readonly DynamicMethod _dynamicMethod;

			public CollectableSerializationMethodGenerator( string operation, Type targetType, string targetMemberName, Type returnType, Type[] parameterTypes )
			{
				this._dynamicMethod = new DynamicMethod( IdentifierUtility.BuildMethodName( operation, targetType, targetMemberName ), returnType, parameterTypes.Length == 0 ? null : parameterTypes );
			}

			public sealed override TracingILGenerator GetILGenerator()
			{
				if ( IsTraceEnabled )
				{
					this.Trace.WriteLine( "{0}::{1}", MethodBase.GetCurrentMethod(), this._dynamicMethod );
				}

				return new TracingILGenerator( this._dynamicMethod, this.Trace );
			}

			protected sealed override TDelegate CreateDelegateCore<TDelegate>()
			{
				return this._dynamicMethod.CreateDelegate( typeof( TDelegate ) ) as TDelegate;
			}
		}
	}
}
