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

namespace MsgPack.Serialization
{
	/// <summary>
	///		Represents result of serializer code generation for each input target types.
	/// </summary>
	public sealed class SerializerCodeGenerationResult
	{
		/// <summary>
		///		Gets the file path which contains generated serializer.
		/// </summary>
		/// <value>
		///		The file path which contains generated serializer.
		/// </value>
		/// <remarks>
		///		If the generation method generates source codes, this property indicates each generated source code file. 
		///		Else, if the genration method generates an assembly, this property indicates the assembly file.
		///		This property will not be <c>null</c>, and will be valid file path.
		/// </remarks>
		public string FilePath { get; private set; }

		/// <summary>
		///		Gets the target type of serializer generation.
		/// </summary>
		/// <value>
		///		The target type of serializer generation.
		///		This property will not be <c>null</c>.
		/// </value>
		public Type TargetType { get; private set; }

		/// <summary>
		///		Gets the namespace of the generated serializer.
		/// </summary>
		/// <value>
		///		The namespace of the generated serializer.
		///		This value will not be <c>null</c>, but might be empty which represents global namespace.
		/// </value>
		public string SerializerTypeNamespace { get; private set; }

		/// <summary>
		///		Gets the type name of the generated serializer.
		/// </summary>
		/// <value>
		///		The type name of the generated serializer.
		///		This property will not be <c>null</c> nor empty.
		/// </value>
		public string SerializerTypeName { get; private set; }

		/// <summary>
		///		Gets the full name of the generated serializer type.
		/// </summary>
		/// <value>
		///		The full name of the generated serializer type.
		///		This property will not be <c>null</c> nor empty.
		/// </value>
		public string SerializerTypeFullName { get; private set; }

		internal SerializerCodeGenerationResult(
			Type targetType,
			string filePath,
			string serializerTypeFullName,
			string serializerTypeNamespace,
			string serializerTypeName 
		)
		{
			this.TargetType = targetType;
			this.FilePath = filePath;
			this.SerializerTypeFullName = serializerTypeFullName;
			this.SerializerTypeNamespace = serializerTypeNamespace;
			this.SerializerTypeName = serializerTypeName;
		}
	}
}