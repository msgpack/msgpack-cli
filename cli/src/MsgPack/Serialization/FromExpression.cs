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
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;

namespace MsgPack.Serialization
{
	internal static partial class FromExpression
	{
		// TODO: NLiblet
		public static PropertyInfo ToProperty<TSource, T>( Expression<Func<TSource, T>> source )
		{
			return ToPropertyCore( source );
		}

		// TODO: NLiblet
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

		// TODO: NLiblet
		private static MethodInfo ToInstanceMethodCore<T>( Expression<T> source )
		{
			return ToMethodCore( source );
		}


		// TODO: NLiblet
		private static MethodInfo ToStaticMethodCore<T>( Expression<T> source )
		{
			return ToMethodCore( source );
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
