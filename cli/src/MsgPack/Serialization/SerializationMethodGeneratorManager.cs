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

namespace MsgPack.Serialization
{
	/// <summary>
	///		Defines common features and interfaces for <see cref="SerializationMethodGeneratorManager"/>
	///		which manages individual <see cref="SerializationMethodGenerator"/>.
	/// </summary>
	internal abstract class SerializationMethodGeneratorManager
	{
		// TODO: Configurable
		internal static SerializationMethodGeneratorOption DefaultSerializationMethodGeneratorOption = SerializationMethodGeneratorOption.CanCollect;

		/// <summary>
		///		Get the appropriate <see cref="SerializationMethodGeneratorManager"/> for the current configuration.
		/// </summary>
		/// <returns>
		///		The appropriate <see cref="SerializationMethodGeneratorManager"/> for the current configuration.
		///		This value will not be <c>null</c>.
		///	</returns>
		public static SerializationMethodGeneratorManager Get()
		{
			return Get( DefaultSerializationMethodGeneratorOption );
		}

		/// <summary>
		///		Get the appropriate <see cref="SerializationMethodGeneratorManager"/> for specified options.
		/// </summary>
		/// <param name="option"><see cref="SerializationMethodGeneratorOption"/>.</param>
		/// <returns>
		///		The appropriate <see cref="SerializationMethodGeneratorManager"/> for specified options. 
		///		This value will not be <c>null</c>.
		///	</returns>
		public static SerializationMethodGeneratorManager Get( SerializationMethodGeneratorOption option )
		{
			if ( option == SerializationMethodGeneratorOption.CanDump )
			{
				return DumpableSerializationMethodGeneratorManager.Instance;
			}
			else
			{
				return CollectableSerializationMethodGeneratorManager.Instance;
			}
		}

		/// <summary>
		///		Create the new <see cref="SerializationMethodGenerator"/> instance.
		/// </summary>
		/// <param name="operation">The name of the operation.</param>
		/// <param name="targetType">The target type of serialization.</param>
		/// <param name="targetMemberName">The name of the target member of serialization.</param>
		/// <param name="returnType">The return type of the generating method.</param>
		/// <param name="parameterTypes">The parameter types of the generating method.</param>
		/// <returns>
		///		The new <see cref="SerializationMethodGenerator"/> instance.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="operation"/> is <c>null</c>.
		///		Or, <paramref name="targetType"/> is <c>null</c>.
		///		Or, <paramref name="targetMemberName"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="operation"/> is empty.
		///		Or, <paramref name="targetMemberName"/> is empty.
		/// </exception>
		public SerializationMethodGenerator CreateGenerator( string operation, Type targetType, string targetMemberName, Type returnType, params Type[] parameterTypes )
		{
			Validation.ValidateIsNotNullNorEmpty( operation, "operation" );
			if ( targetType == null )
			{
				throw new ArgumentNullException( "targetType" );
			}
			Validation.ValidateIsNotNullNorEmpty( targetMemberName, "targetMemberName" );

			return this.CreateGeneratorCore( operation, targetType, targetMemberName, returnType == typeof( void ) ? null : returnType, parameterTypes ?? Type.EmptyTypes );
		}

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
		protected abstract SerializationMethodGenerator CreateGeneratorCore( string operation, Type targetType, string targetMemberName, Type returnType, params Type[] parameterTypes );
	}
}
