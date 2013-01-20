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
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;

namespace MsgPack.Serialization
{
	// This file generated from FromExpression.tt T4Template.
	// Do not modify this file. Edit FromExpression.tt instead.

	partial class FromExpression 
	{
		public static ConstructorInfo ToConstructor<TResult>( Expression< System.Func<TResult> > source )
		{
			return ToConstructorCore( source );
		}
		public static ConstructorInfo ToConstructor<T, TResult>( Expression< System.Func<T, TResult> > source )
		{
			return ToConstructorCore( source );
		}
		public static ConstructorInfo ToConstructor<T1, T2, TResult>( Expression< System.Func<T1, T2, TResult> > source )
		{
			return ToConstructorCore( source );
		}
		public static ConstructorInfo ToConstructor<T1, T2, T3, TResult>( Expression< System.Func<T1, T2, T3, TResult> > source )
		{
			return ToConstructorCore( source );
		}
		public static ConstructorInfo ToConstructor<T1, T2, T3, T4, TResult>( Expression< System.Func<T1, T2, T3, T4, TResult> > source )
		{
			return ToConstructorCore( source );
		}
#if !WINDOWS_PHONE && !NETFX_35
		public static ConstructorInfo ToConstructor<T1, T2, T3, T4, T5, TResult>( Expression< System.Func<T1, T2, T3, T4, T5, TResult> > source )
		{
			return ToConstructorCore( source );
		}
#endif
#if !WINDOWS_PHONE && !NETFX_35
		public static ConstructorInfo ToConstructor<T1, T2, T3, T4, T5, T6, TResult>( Expression< System.Func<T1, T2, T3, T4, T5, T6, TResult> > source )
		{
			return ToConstructorCore( source );
		}
#endif
#if !WINDOWS_PHONE && !NETFX_35
		public static ConstructorInfo ToConstructor<T1, T2, T3, T4, T5, T6, T7, TResult>( Expression< System.Func<T1, T2, T3, T4, T5, T6, T7, TResult> > source )
		{
			return ToConstructorCore( source );
		}
#endif
#if !WINDOWS_PHONE && !NETFX_35
		public static ConstructorInfo ToConstructor<T1, T2, T3, T4, T5, T6, T7, T8, TResult>( Expression< System.Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> > source )
		{
			return ToConstructorCore( source );
		}
#endif
#if !WINDOWS_PHONE && !NETFX_35
		public static ConstructorInfo ToConstructor<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>( Expression< System.Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> > source )
		{
			return ToConstructorCore( source );
		}
#endif
#if !WINDOWS_PHONE && !NETFX_35
		public static ConstructorInfo ToConstructor<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>( Expression< System.Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> > source )
		{
			return ToConstructorCore( source );
		}
#endif
#if !WINDOWS_PHONE && !NETFX_35
		public static ConstructorInfo ToConstructor<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>( Expression< System.Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> > source )
		{
			return ToConstructorCore( source );
		}
#endif
#if !WINDOWS_PHONE && !NETFX_35
		public static ConstructorInfo ToConstructor<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>( Expression< System.Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> > source )
		{
			return ToConstructorCore( source );
		}
#endif
#if !WINDOWS_PHONE && !NETFX_35
		public static ConstructorInfo ToConstructor<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>( Expression< System.Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult> > source )
		{
			return ToConstructorCore( source );
		}
#endif
#if !WINDOWS_PHONE && !NETFX_35
		public static ConstructorInfo ToConstructor<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>( Expression< System.Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult> > source )
		{
			return ToConstructorCore( source );
		}
#endif
#if !WINDOWS_PHONE && !NETFX_35
		public static ConstructorInfo ToConstructor<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>( Expression< System.Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult> > source )
		{
			return ToConstructorCore( source );
		}
#endif
#if !WINDOWS_PHONE && !NETFX_35
		public static ConstructorInfo ToConstructor<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>( Expression< System.Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult> > source )
		{
			return ToConstructorCore( source );
		}
#endif
	}
}

