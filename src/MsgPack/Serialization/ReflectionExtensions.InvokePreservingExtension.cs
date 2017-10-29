#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2016 FUJIWARA, Yusuke
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

#if UNITY_5 || UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_WII || UNITY_IPHONE || UNITY_ANDROID || UNITY_PS3 || UNITY_XBOX360 || UNITY_FLASH || UNITY_BKACKBERRY || UNITY_WINRT
#define UNITY
#endif

using System;
using System.Diagnostics;
using System.Reflection;

namespace MsgPack.Serialization
{
	partial class ReflectionExtensions
	{
		public static object InvokePreservingExceptionType( this ConstructorInfo source, params object[] parameters )
		{
			try
			{
				return source.Invoke( parameters );
			}
			catch ( TargetInvocationException ex )
			{
				var rethrowing = HoistUpInnerException( ex );
				if ( rethrowing == null )
				{
					// ctor.Invoke threw exception, so rethrow original TIE.
					throw;
				}
				else
				{
					throw rethrowing;
				}
			}
		}

		public static object InvokePreservingExceptionType( this MethodInfo source, object instance, params object[] parameters )
		{
			try
			{
				return source.Invoke( instance, parameters );
			}
			catch ( TargetInvocationException ex )
			{
				var rethrowing = HoistUpInnerException( ex );
				if ( rethrowing == null )
				{
					// ctor.Invoke threw exception, so rethrow original TIE.
					throw;
				}
				else
				{
					throw rethrowing;
				}
			}
		}

		public static T CreateInstancePreservingExceptionType<T>( Type instanceType, params object[] constructorParameters )
		{
			return ( T ) CreateInstancePreservingExceptionType( instanceType, constructorParameters );
		}

		public static object CreateInstancePreservingExceptionType( Type type, params object[] constructorParameters )
		{
			try
			{
				return Activator.CreateInstance( type, constructorParameters );
			}
			catch ( TargetInvocationException ex )
			{
				var rethrowing = HoistUpInnerException( ex );
				if ( rethrowing == null )
				{
					// ctor.Invoke threw exception, so rethrow original TIE.
					throw;
				}
				else
				{
					throw rethrowing;
				}
			}
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "This method should swallow exception in restoring inner exception of TargetInvocationException." )]
		private static Exception HoistUpInnerException( TargetInvocationException targetInvocationException )
		{
			if ( targetInvocationException.InnerException == null )
			{
				return null;
			}

			var ctor = targetInvocationException.InnerException.GetType().GetConstructor( ExceptionConstructorWithInnerParameterTypes );
			if ( ctor == null )
			{
				return null;
			}

			try
			{
				return ctor.Invoke( new object[] { targetInvocationException.InnerException.Message, targetInvocationException } ) as Exception;
			}
#if !UNITY || MSGPACK_UNITY_FULL
			catch ( Exception ex )
#else
			catch ( Exception )
#endif // !UNITY || MSGPACK_UNITY_FULL
			{
#if !UNITY || MSGPACK_UNITY_FULL
				Debug.WriteLine( "HoistUpInnerException:" + ex );
#endif // !UNITY || MSGPACK_UNITY_FULL
				return null;
			}
		}
	}
}
