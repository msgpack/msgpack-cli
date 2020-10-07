// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using MsgPack.Internal;

namespace MsgPack.Serialization
{
	internal partial class ReflectionExtensions
	{
		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public static T InvokePreservingExceptionType<T>(this ConstructorInfo source, params object?[] parameters)
			=> (T)InvokePreservingExceptionType(source, parameters);

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public static object InvokePreservingExceptionType(this ConstructorInfo source, params object?[] parameters)
		{
#if FEATURE_DO_NOT_WRAP_EXCEPTIONS
			return source.Invoke(BindingFlags.DoNotWrapExceptions, binder: null, parameters, culture: null);
#else
			try
			{
				return source.Invoke(parameters);
			}
			catch (TargetInvocationException ex)
			{
				HandleTargetInvocationException(ex);
				throw; // failed to hoist-up, never reaches except .NET Framework 3.5
			}
#endif // FEATURE_DO_NOT_WRAP_EXCEPTIONS
		}

		public static object? InvokePreservingExceptionType(this MethodInfo source, object instance, params object[] parameters)
		{
#if FEATURE_DO_NOT_WRAP_EXCEPTIONS
			return source.Invoke(instance, BindingFlags.DoNotWrapExceptions, binder: null, parameters, culture: null);
#else
			try
			{
				return source.Invoke(instance, parameters);
			}
			catch (TargetInvocationException ex)
			{
				HandleTargetInvocationException(ex);
				throw; // failed to hoist-up, never reaches except .NET Framework 3.5
			}
#endif // FEATURE_DO_NOT_WRAP_EXCEPTIONS
		}

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		public static T CreateInstancePreservingExceptionType<T>(this Type instanceType, params object?[] constructorParameters)
			=> (T)CreateInstancePreservingExceptionType(instanceType, constructorParameters);

		public static object CreateInstancePreservingExceptionType(this Type type, params object?[] constructorParameters)
		{
#if FEATURE_DO_NOT_WRAP_EXCEPTIONS
			return Activator.CreateInstance(type, BindingFlags.DoNotWrapExceptions, binder: null, constructorParameters, culture: null)!;
#else
			try
			{
				return Activator.CreateInstance(type, constructorParameters)!;
			}
			catch (TargetInvocationException ex)
			{
				HandleTargetInvocationException(ex);
				throw; // failed to hoist-up, never reaches except .NET Framework 3.5
			}
#endif // FEATURE_DO_NOT_WRAP_EXCEPTIONS
		}

		[MethodImpl(MethodImplOptionsShim.AggressiveInlining)]
		private static void HandleTargetInvocationException(TargetInvocationException ex)
		{
#if FEATURE_EXCEPTION_DISPATCH_INFO
			ExceptionDispatchInfo.Capture(ex).Throw();
#else
			var rethrowing = HoistUpInnerException(ex);
			if (rethrowing == null)
			{
				// ctor.Invoke threw exception, so rethrow original TIE.
				return;
			}
			else
			{
				throw rethrowing;
			}
#endif // FEATURE_EXCEPTION_DISPATCH_INFO
		}

#if !FEATURE_EXCEPTION_DISPATCH_INFO
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "This method should swallow exception in restoring inner exception of TargetInvocationException.")]
		private static Exception? HoistUpInnerException(TargetInvocationException targetInvocationException)
		{
			if (targetInvocationException.InnerException == null)
			{
				return null;
			}

			var innerExceptionType = targetInvocationException.InnerException.GetType();
			var ctor = innerExceptionType.GetConstructor(ExceptionConstructorWithInnerParameterTypes);
			if (ctor == null)
			{
				Diagnostic.General.HoistUpInnerExceptionFailed(new { innerExceptionType });
				return null;
			}

			try
			{
				return (ctor.Invoke(new object[] { targetInvocationException.InnerException.Message, targetInvocationException }) as Exception);
			}
			catch (Exception ex)
			{
				Diagnostic.General.HoistUpInnerExceptionFailed(new { innerExceptionType, instantiationError = ex });
				return null;
			}
		}
#endif // FEATURE_EXCEPTION_DISPATCH_INFO

		public static ConstructorInfo GetRequiredConstructor(this Type type, params Type[] types)
		{
			var result = type.GetConstructor(types);
			Debug.Assert(result != null, $"public {type}({String.Join(", ", types as object[])}) not found");
			return result;
		}
	}
}
