#region -- License Terms --
// 
// MessagePack for CLI
// 
// Copyright (C) 2015 FUJIWARA, Yusuke
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

namespace MsgPack.Serialization
{
	/// <summary>
	///		Wraps non-generic <see cref="IMessagePackSingleObjectSerializer"/> which implements <see cref="ICustomizableEnumSerializer"/> to avoid AOT issue.
	/// </summary>
	/// <typeparam name="T">The type to be serialized.</typeparam>
	internal sealed class EnumTypedMessagePackSerializerWrapper<T> : TypedMessagePackSerializerWrapper<T>, ICustomizableEnumSerializer
	{
		private readonly ICustomizableEnumSerializer _underlyingEnumSerializer;

		public EnumTypedMessagePackSerializerWrapper( SerializationContext context, IMessagePackSingleObjectSerializer underlying )
			: base( context, underlying )
		{
			this._underlyingEnumSerializer = underlying as ICustomizableEnumSerializer;
		}

		ICustomizableEnumSerializer ICustomizableEnumSerializer.GetCopyAs( EnumSerializationMethod method )
		{
			return 
				new EnumTypedMessagePackSerializerWrapper<T>(
					this.OwnerContext,
					this._underlyingEnumSerializer.GetCopyAs( method ) as IMessagePackSingleObjectSerializer
				);
		}
	}
}