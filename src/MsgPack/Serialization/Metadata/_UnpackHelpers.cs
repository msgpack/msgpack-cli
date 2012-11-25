#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2012 FUJIWARA, Yusuke
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
using System.Collections;
using System.Linq;
using System.Reflection;

namespace MsgPack.Serialization.Metadata
{
	internal static class _UnpackHelpers
	{
		public static readonly MethodInfo UnpackArrayTo_1 =
			typeof( UnpackHelpers ).GetMethod( "UnpackArrayTo" );
		public static readonly MethodInfo UnpackCollectionTo_1 =
			typeof( UnpackHelpers ).GetMethods().Single( item => item.Name == "UnpackCollectionTo" && item.GetParameters().Length == 4 && item.GetGenericArguments().Length == 1 );
		public static readonly MethodInfo UnpackCollectionTo_2 =
			typeof( UnpackHelpers ).GetMethods().Single( item => item.Name == "UnpackCollectionTo" && item.GetParameters().Length == 4 && item.GetGenericArguments().Length == 2 );
		public static readonly MethodInfo UnpackMapTo_2 =
			typeof( UnpackHelpers ).GetMethods().Single( item => item.Name == "UnpackMapTo" && item.GetGenericArguments().Length == 2 );
		public static readonly MethodInfo UnpackNonGenericCollectionTo =
			FromExpression.ToMethod( ( Unpacker unpacker, IList collection, Action<object> addition ) => UnpackHelpers.UnpackCollectionTo( unpacker, collection, addition ) );
		public static readonly MethodInfo UnpackNonGenericCollectionTo_1 =
			typeof( UnpackHelpers ).GetMethods().Single( item => item.Name == "UnpackCollectionTo" && item.GetParameters().Length == 3 && item.GetGenericArguments().Length == 1 );
		public static readonly MethodInfo UnpackNonGenericMapTo =
			FromExpression.ToMethod( ( Unpacker unpacker, IDictionary dictionary ) => UnpackHelpers.UnpackMapTo( unpacker, dictionary ) );
		public static readonly MethodInfo ConvertWithEnsuringNotNull_1Method =
			typeof( UnpackHelpers ).GetMethod( "ConvertWithEnsuringNotNull" );
		public static readonly MethodInfo InvokeUnpackFrom_1Method =
			typeof( UnpackHelpers ).GetMethod( "InvokeUnpackFrom" );
	}
}
