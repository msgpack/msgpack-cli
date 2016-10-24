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
using System.Text;

namespace MsgPack.Serialization
{
	/// <summary>
	///		Represents generating serializer code information.
	/// </summary>
	public sealed class SerializerCodeInformation
	{
		/// <summary>
		///		Gets the full name of the generating type.
		/// </summary>
		/// <value>
		///		The full name of the generating type.
		/// </value>
		public string TypeFullName { get; private set; }

		/// <summary>
		///		Gets the directory where the code file to be generated.
		/// </summary>
		/// <value>
		///		The directory where the code file to be generated.
		/// </value>
		public string Directory { get; private set; }

		/// <summary>
		///		Gets the file extension of the code including leading dot.
		/// </summary>
		/// <value>
		///		The file extension of the code including leading dot.
		/// </value>
		public string FileExtension { get; private set; }

		/// <summary>
		///		Gets or sets the <see cref="T:TextWriter"/> which the code text to be written.
		/// </summary>
		/// <value>
		///		The text writer the <see cref="T:TextWriter"/> which the code text to be written.
		///		The initial value is <c>null</c>.
		///		If this property is not set, the code generator will fail to emit the code.
		/// </value>
		/// <remarks>
		///		You should not set this property directly, use <see cref="SetFileWriter"/> or <see cref="SetNonFileWriter"/> instead.
		/// </remarks>
		public TextWriter TextWriter { get; set; }

		/// <summary>
		///		Gets or sets the file path of the generating code.
		/// </summary>
		/// <value>
		///		The file path of the generating code.
		///		The initial value is <c>null</c>.
		///		This property will be used to report code generation result.
		/// </value>
		/// <remarks>
		///		You should not set this property directly, use <see cref="SetFileWriter"/> or <see cref="SetNonFileWriter"/> instead.
		/// </remarks>
		public string FilePath { get; set; }

		internal SerializerCodeInformation( string typeFullName, string directory, string fileExtension )
		{
			this.TypeFullName = typeFullName;
			this.Directory = directory;
			this.FileExtension = fileExtension;
		}

		/// <summary>
		///		Sets up this object to use <see cref="T:TextWriter"/> for specified file path.
		/// </summary>
		/// <param name="path">The file path.</param>
		/// <exception cref="ArgumentNullException">The <paramref name="path"/> is <c>null</c>.</exception>
		/// <exception cref="ArgumentException">The <paramref name="path"/> is empty or invalid character.</exception>
		/// <exception cref="NotSupportedException">The <paramref name="path"/> is unsupported form.</exception>
		/// <exception cref="PathTooLongException">The <paramref name="path"/> is too long.</exception>
		/// <exception cref="DirectoryNotFoundException">The <paramref name="path"/> point the directory which does not exist.</exception>
		/// <exception cref="UnauthorizedAccessException">The <paramref name="path"/> points the location which is not writable from this process.</exception>
		/// <remarks>
		///		Use this method to emit the code to the file.
		///		This method sets <see cref="P:TextWriter"/> and <see cref="FilePath"/> property.
		/// </remarks>
		public void SetFileWriter( string path )
		{
			this.TextWriter = new StreamWriter( path, false, Encoding.UTF8 );
			this.FilePath = Path.GetFullPath( path );
		}

		/// <summary>
		///		Sets up this object to use specified <see cref="T:TextWriter"/>.
		/// </summary>
		/// <param name="writer">The writer.</param>
		/// <exception cref="ArgumentNullException">The <paramref name="writer"/> is <c>null</c>.</exception>
		/// <remarks>
		///		Use this method to emit the code to specified <see cref="T:TextWriter"/> instead of the file.
		///		Use <see cref="SetFileWriter"/> if you want to emit to the file.
		///		This method sets the <see cref="P:TextWriter"/> property with the argument, and sets <c>null</c> for <see cref="FilePath"/>.
		/// </remarks>
		public void SetNonFileWriter( TextWriter writer )
		{
			if ( writer == null )
			{
				throw new ArgumentNullException( "writer" );
			}

			this.TextWriter = writer;
			this.FilePath = null;
		}
	}
}