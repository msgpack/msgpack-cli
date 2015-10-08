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
using System.Globalization;
using System.IO;
using System.Reflection;

namespace MsgPack.Serialization
{
	/// <summary>
	///		Represents configuration for pre-generated serializer assembly generation.
	/// </summary>
	public sealed class SerializerAssemblyGenerationConfiguration : ISerializerGeneratorConfiguration
	{
		private string _outputDirectory;

		/// <summary>
		///		Gets or sets the output directory for generated source codes.
		/// </summary>
		/// <value>
		///		The output directory for generated source codes.
		///		The default is current directory.
		/// </value>
		/// <exception cref="ArgumentNullException">The specified value is <c>null</c>.</exception>
		/// <exception cref="ArgumentException">The specified value is empty or too long.</exception>
		/// <exception cref="NotSupportedException">The specified path format is not supported.</exception>
		public string OutputDirectory
		{
			get { return this._outputDirectory; }
			set { this._outputDirectory = Path.GetFullPath( value ?? "." ); }
		}
		private SerializationMethod _serializationMethod;

		/// <summary>
		/// Gets or sets the serialization method to pack object.
		/// </summary>
		/// <value>
		/// A value of <see cref="SerializationMethod" />.
		/// </value>
		/// <exception cref="ArgumentOutOfRangeException">Specified value is not valid  <see cref="SerializationMethod"/>.</exception>
		public SerializationMethod SerializationMethod
		{
			get { return this._serializationMethod; }
			set
			{
				switch ( value )
				{
					case SerializationMethod.Array:
					case SerializationMethod.Map:
					{
						this._serializationMethod = value;
						break;
					}
					default:
					{
						throw new ArgumentOutOfRangeException( "value" );
					}
				}
			}
		}

		private EnumSerializationMethod _enumSerializationMethod;

		/// <summary>
		///		Gets or sets the default enum serialization method for generating enum type serializers.
		/// </summary>
		/// <value>
		///		A value of <see cref="EnumSerializationMethod"/>.
		/// </value>
		/// <exception cref="ArgumentOutOfRangeException">Specified value is not valid  <see cref="EnumSerializationMethod"/>.</exception>
		public EnumSerializationMethod EnumSerializationMethod
		{
			get { return this._enumSerializationMethod; }
			set
			{
				switch ( value )
				{
					case EnumSerializationMethod.ByName:
					case EnumSerializationMethod.ByUnderlyingValue:
					{
						this._enumSerializationMethod = value;
						break;
					}
					default:
					{
						throw new ArgumentOutOfRangeException( "value" );
					}
				}
			}
		}

		/// <summary>
		///		Gets or sets the name of the assembly.
		///		This property is required.
		/// </summary>
		/// <value>
		///		The name of the assembly.
		/// </value>
		public AssemblyName AssemblyName { get; set; }

		/// <summary>
		///		Gets or sets a value indicating whether recursively generates dependent types which do not have built-in serializer or not.
		/// </summary>
		/// <value>
		/// <c>true</c> if recursively generates dependent types which do not have built-in serializer; otherwise, <c>false</c>.
		/// </value>
		public bool IsRecursive { get; set; }

		/// <summary>
		///		Gets or sets a value indicating whether prefer reflection based collection serializers instead of dyhnamic generated serializers.
		/// </summary>
		/// <value>
		/// <c>true</c> if prefer reflection based collection serializers instead of dyhnamic generated serializers; otherwise, <c>false</c>.
		/// </value>
		public bool PreferReflectionBasedSerializer { get; set; }

		/// <summary>
		///		Gets or sets a value indicating whether creating Nullable of T serializers for value type serializers.
		/// </summary>
		/// <value>
		/// <c>true</c> if creates Nullable of T serializers for value type serializers; otherwise, <c>false</c>.
		/// </value>
		public bool WithNullableSerializers { get; set; }

		/// <summary>
		///		Initializes a new instance of the <see cref="SerializerAssemblyGenerationConfiguration"/> class.
		/// </summary>
		public SerializerAssemblyGenerationConfiguration()
		{
			this.OutputDirectory = null;
			this._serializationMethod = SerializationMethod.Array;
		}

		void ISerializerGeneratorConfiguration.Validate()
		{
			try
			{
				// ReSharper disable ReturnValueOfPureMethodIsNotUsed
				Path.GetFullPath( "." + Path.DirectorySeparatorChar + this.AssemblyName.Name );
				// ReSharper restore ReturnValueOfPureMethodIsNotUsed
			}
			catch ( ArgumentException ex )
			{
				throw CreateValidationError( ex );
			}
			catch ( NotSupportedException ex )
			{
				throw CreateValidationError( ex );
			}
		}

		private static Exception CreateValidationError( Exception innerException )
		{
			return
				new InvalidOperationException(
					String.Format(
						CultureInfo.CurrentCulture, "AssemblyName property is not set correctly. Detail: {0}", innerException.Message
						),
					innerException
					);
		}
	}
}