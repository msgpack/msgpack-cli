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

namespace MsgPack.Serialization
{
	/// <summary>
	///		Implements <see cref="MessagePackSerializerProvider"/> for enums.
	///		This class accepts <see cref="EnumSerializationMethod"/> as a provider parameter.
	/// </summary>
	internal sealed class EnumMessagePackSerializerProvider : MessagePackSerializerProvider
	{
		private readonly object _serializerForName;
		private readonly object _serializerForIntegral;

		/// <summary>
		/// Initializes a new instance of the <see cref="EnumMessagePackSerializerProvider"/> class.
		/// </summary>
		/// <param name="serializer">The serializer implements <see cref="ICustomizableEnumSerializer"/>.</param>
		public EnumMessagePackSerializerProvider( ICustomizableEnumSerializer serializer )
		{
			this._serializerForName = serializer.GetCopyAs( EnumSerializationMethod.ByName ); ;
			this._serializerForIntegral = serializer.GetCopyAs( EnumSerializationMethod.ByUnderlyingValue );
		}

		/// <summary>
		/// Gets a serializer instance for specified parameter.
		/// </summary>
		/// <param name="providerParameter">A provider specific parameter.</param>
		/// <returns>	
		/// A serializer object for specified parameter.
		/// </returns>
		public override object Get( object providerParameter )
		{
			if ( ( providerParameter is EnumSerializationMethod ) &&
				 ( ( EnumSerializationMethod )providerParameter ) == EnumSerializationMethod.ByUnderlyingValue )
			{
				return this._serializerForIntegral;
			}
			else
			{
				return this._serializerForName;
			}
		}
	}
}