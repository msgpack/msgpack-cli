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

using System;
using System.Buffers;
using System.Runtime.CompilerServices;
#if FEATURE_TAP
using System.Threading;
using System.Threading.Tasks;
using MsgPack.Internal;
#endif // FEATURE_TAP

namespace MsgPack.Serialization.Polymorphic
{
	/// <summary>
	///		Defines internal protocol betwenn polymorphic serializers and collection serializers to accomplish polymorphism.
	/// </summary>
	internal interface IPolymorphicDeserializer
	{
		object? DeserializePolymorphic(ref DeserializationOperationContext context, ref SequenceReader<byte> reader);

#if FEATURE_TAP
		ValueTask<object?> DeserializePolymorphicAsync(AsyncDeserializationOperationContext context, ReadOnlyStreamSequence sequence, CancellationToken cancellationToken );
#endif // FEATURE_TAP
	}

#warning TODO: Compat
	//internal static class PolymorphicDeserializerCompatibilityExtensions
	//{
	//	public static object PolymorphicUnpackFrom(this IPolymorphicDeserializer deserializer, Unpacker unpacker)
	//		=> deserializer.DeserializePolymorphic(unpacker.Sequence);

	//	public static ValueTask<object> PolymorphicUnpackFromAsync(this IPolymorphicDeserializer deserializer, Unpacker unpacker)
	//		=> deserializer.PolymorphicDeserializeAsync(unpacker.Stream);
	//}
}
