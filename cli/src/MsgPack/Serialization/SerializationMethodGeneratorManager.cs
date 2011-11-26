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
	///		Defines common features and interfaces for <see cref="SerializationMethodGeneratorManager"/>.
	/// </summary>
	internal abstract class SerializationMethodGeneratorManager
	{
		// TODO: Configurable
		internal static SerializationMethodGeneratorOption DefaultSerializationMethodGeneratorOption = SerializationMethodGeneratorOption.Fast;

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
			switch ( option )
			{
				case SerializationMethodGeneratorOption.CanDump:
				{
					return DefaultSerializationMethodGeneratorManager.CanDump;
				}
				case SerializationMethodGeneratorOption.CanCollect:
				{
					return DefaultSerializationMethodGeneratorManager.CanCollect;
				}
				default:
				{
					return DefaultSerializationMethodGeneratorManager.Fast;
				}
			}
		}

		public SerializerEmitter CreateEmitter( Type targetType )
		{
			if ( targetType == null )
			{
				throw new ArgumentNullException( "targetType" );
			}

			return this.CreateEmitterCore( targetType );
		}

		protected abstract SerializerEmitter CreateEmitterCore( Type targetType );
	}
}
