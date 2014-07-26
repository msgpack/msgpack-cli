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

namespace MsgPack.Serialization.ReflectionSerializers
{
	internal class ReflectionNilImplicationHandler : NilImplicationHandler<Action<object>, Func<object, bool>, ReflectionSerializerNilImplicationHandlerParameter, ReflectionSerializerNilImplicationHandlerOnUnpackedParameter>
	{
		public static readonly ReflectionNilImplicationHandler Instance = new ReflectionNilImplicationHandler();

		private ReflectionNilImplicationHandler() { }

		protected override Func<object, bool> OnPackingMessagePackObject( ReflectionSerializerNilImplicationHandlerParameter parameter )
		{
			return value => ( ( MessagePackObject )value ).IsNil;
		}

		protected override Func<object, bool> OnPackingReferenceTypeObject( ReflectionSerializerNilImplicationHandlerParameter parameter )
		{
			return value => value == null;
		}

		protected override Func<object, bool> OnPackingNullableValueTypeObject( ReflectionSerializerNilImplicationHandlerParameter parameter )
		{
			// Runtime boxes 'null' Nullable<T> as null reference.
			return value => value == null;
		}

		protected override Action<object> OnPackingCore( ReflectionSerializerNilImplicationHandlerParameter parameter, Func<object, bool> condition )
		{
			return
				value =>
				{
					if ( condition( value ) )
					{
						throw SerializationExceptions.NewNullIsProhibited( parameter.MemberName );
					}
				};
		}

		protected override Action<object> OnNopOnUnpacked( ReflectionSerializerNilImplicationHandlerOnUnpackedParameter parameter )
		{
			return _ => { };
		}

		protected override Action<object> OnThrowNullIsProhibitedExceptionOnUnpacked( ReflectionSerializerNilImplicationHandlerOnUnpackedParameter parameter )
		{
			return
				_ =>
				{
					throw SerializationExceptions.NewNullIsProhibited( parameter.MemberName );
				};
		}

		protected override Action<object> OnThrowValueTypeCannotBeNull3OnUnpacked( ReflectionSerializerNilImplicationHandlerOnUnpackedParameter parameter )
		{
			return
				_ =>
				{
					throw SerializationExceptions.NewValueTypeCannotBeNull(
						parameter.MemberName,
						parameter.ItemType,
						parameter.DeclaringType 
					);
				};
		}
	}
}
