// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Reflection;
using System.Reflection.Emit;

using MsgPack.Serialization.Reflection;

namespace MsgPack.Serialization
{
	internal partial class ReflectionExtensions
	{
		public static TDelegate CreateConstructorDelegate<TDelegate>(this ConstructorInfo constructor)
		{
			return (TDelegate)CreateDelegate(typeof(TDelegate), constructor.DeclaringType, constructor, constructor.GetParameterTypes());
		}

		private static object CreateDelegate(Type delegateType, Type targetType, ConstructorInfo constructor, Type[] parameterTypes)
		{
			var dynamicMethod =
#if !SILVERLIGHT
				new DynamicMethod("Create" + targetType.Name, targetType, parameterTypes, restrictedSkipVisibility: true);
#else
				new DynamicMethod( "Create" + targetType.Name, targetType, parameterTypes );
#endif // !SILVERLIGHT
			var il = new TracingILGenerator(dynamicMethod, NullTextWriter.Instance, isDebuggable: false);
			if (constructor == null)
			{
				// Value type's init.
				il.DeclareLocal(targetType);
				il.EmitAnyLdloca(0);
				il.EmitInitobj(targetType);
				il.EmitAnyLdloc(0);
			}
			else
			{
				for (var i = 0; i < parameterTypes.Length; i++)
				{
					il.EmitAnyLdarg(i);
				}

				il.EmitNewobj(constructor);
			}

			il.EmitRet();
			return dynamicMethod.CreateDelegate(delegateType);
		}
	}
}
