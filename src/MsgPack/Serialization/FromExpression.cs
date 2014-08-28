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
	// TODO: NLiblet
	internal static partial class FromExpression
	{
		public static PropertyInfo ToProperty<TSource, T>( Expression<Func<TSource, T>> source )
		{
			return ToPropertyCore( source );
		}

		public static PropertyInfo ToProperty<T>( Expression<Func<T>> source )
		{
			return ToPropertyCore( source );
		}

		private static PropertyInfo ToPropertyCore<T>( Expression<T> source )
		{
			if ( source == null )
			{
				throw new ArgumentNullException( "source" );
			}

			var memberExpression = source.Body as MemberExpression;
			if ( memberExpression == null )
			{
				ThrowNotValidExpressionTypeException( source );
			}

			var property = memberExpression.Member as PropertyInfo;
			if ( property == null )
			{
				throw new ArgumentException( String.Format( CultureInfo.CurrentCulture, "Member '{0}' is not property.", memberExpression.Member ), "source" );
			}

			return property;
		}

		private static MethodInfo ToMethodCore<T>( Expression<T> source )
		{
			if ( source == null )
			{
				throw new ArgumentNullException( "source" );
			}

			var methodCallExpression = source.Body as MethodCallExpression;
			if ( methodCallExpression == null )
			{
				ThrowNotValidExpressionTypeException( source );
			}

			return methodCallExpression.Method;
		}

		public static MethodInfo ToOperator<T1, T2, TResult>( Expression<Func<T1, T2, TResult>> source )
		{
			if ( source == null )
			{
				throw new ArgumentNullException( "source" );
			}

			var binaryExpression = source.Body as BinaryExpression;
			if ( binaryExpression == null )
			{
				ThrowNotValidExpressionTypeException( source );
			}

			return binaryExpression.Method;
		}
		
		private static void ThrowNotValidExpressionTypeException( Expression source )
		{
			throw new NotSupportedException(
				String.Format(
					CultureInfo.CurrentCulture,
					"Specified expression '{0}' is too complex. Simple member reference expression is only supported. ",
					source
				)
			);
		}
	}
}
