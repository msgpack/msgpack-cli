#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2016 FUJIWARA, Yusuke
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
using System.IO;

namespace MsgPack.Serialization
{
	/// <summary>
	///		Represents code generation sink which is responsible to emitting target.
	/// </summary>
	public abstract class CodeGenerationSink
	{
		/// <summary>
		///		Initializes a new instance of the <see cref="CodeGenerationSink"/> class.
		/// </summary>
		protected CodeGenerationSink() { }

		/// <summary>
		///		Assigns the appropriate <see cref="TextWriter"/> to the specified <see cref="SerializerCodeInformation"/>
		///		based on the argument and the method of this object.
		/// </summary>
		/// <param name="codeInformation">
		///		The <see cref="SerializerCodeInformation"/> object which holds informations to determine output and output themselves.
		/// </param>
		/// <exception cref="ArgumentNullException"><paramref name="codeInformation"/> is <c>null</c>.</exception>
		public void AssignTextWriter( SerializerCodeInformation codeInformation )
		{
			if ( codeInformation == null )
			{
				throw new ArgumentNullException( "codeInformation" );
			}

			this.AssignTextWriterCore( codeInformation );
		}

		/// <summary>
		///		Assigns the appropriate <see cref="TextWriter"/> to the specified <see cref="SerializerCodeInformation"/>
		///		based on the argument and the method of this object.
		/// </summary>
		/// <param name="codeInformation">
		///		The <see cref="SerializerCodeInformation"/> object which holds informations to determine output and output themselves.
		///		The override implementation must set its <see cref="SerializerCodeInformation.TextWriter"/> property via <c>Set*</c> method.
		///		This value will not be <c>null</c>.
		/// </param>
		protected abstract void AssignTextWriterCore( SerializerCodeInformation codeInformation );

		/// <summary>
		///		Gets a pre-defined <see cref="CodeGenerationSink"/> object which assigns individual <see cref="TextWriter"/> for files toward each codes.
		/// </summary>
		/// <returns>The pre-defined <see cref="CodeGenerationSink"/>. This value will not be <c>null</c>.</returns>
		public static CodeGenerationSink ForIndividualFile()
		{
			return IndividualFileCodeGenerationSink.Instance;
		}

		/// <summary>
		///		Gets a pre-defined <see cref="CodeGenerationSink"/> object which assigns specified <see cref="TextWriter"/> toward all codes.
		/// </summary>
		/// <param name="writer">The <see cref="TextWriter"/> to be used for all codes.</param>
		/// <returns>The pre-defined <see cref="CodeGenerationSink"/>. This value will not be <c>null</c>.</returns>
		/// <exception cref="ArgumentNullException">The <paramref name="writer"/> is <c>null</c>.</exception>
		public static CodeGenerationSink ForSpecifiedTextWriter( TextWriter writer )
		{
			return new SingleTextWriterCodeGenerationSink( writer );
		}
	}
}