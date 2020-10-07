// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace MsgPack.Serialization
{
	internal static partial class ReflectionExtensions
	{
		private const string NullableAttributeTypeName = "System.CompilerServices.NullableAttribute";
		private const string NullableContextAttributeTypeName = "System.CompilerServices.NullableContextAttribute";

		private static readonly Type[] ExceptionConstructorWithInnerParameterTypes = { typeof(string), typeof(Exception) };
		private static readonly Type[] ObjectAddParameterTypes = { typeof(object) };

		public static bool IsNullableType(this Type type)
			=> !type.GetIsValueType() || Nullable.GetUnderlyingType(type) != null;

		public static bool IsNullable(this MemberInfo member)
		{
			// see https://github.com/dotnet/roslyn/blob/master/docs/features/nullable-metadata.md
			var nullabileArguments = member.GetCustomAttributesData().SingleOrDefault(a => a.AttributeType.FullName == NullableAttributeTypeName)?.GetConstructorArguments();
			if (nullabileArguments?[0].ArgumentType == typeof(byte))
			{
				var nullability = (byte)nullabileArguments[0].Value!;
				if (nullability!=0)
				{
					return nullability == 2;
				}
			}
			else if (nullabileArguments?[0].ArgumentType == typeof(byte[]))
			{
				var nullability = ((byte[])nullabileArguments[0].Value!)[0];
				if (nullability != 0)
				{
					return nullability == 2;
				}
			}

			// Note: oblivious (0) will be treated as nullable in this context.
			return 
				(byte)member.DeclaringType!
					.GetCustomAttributesData()
					.SingleOrDefault(a => a.AttributeType.FullName == NullableContextAttributeTypeName)?
					.GetConstructorArguments()[0].Value!
				!= 1;
		}

		public static bool IsCodecPrimitive(this Type type)
			=> type.GetIsPrimitive()
				&& (Type.GetTypeCode(type) switch
				{
					TypeCode.Char | TypeCode.Object => false,
					_ => true
				});

		public static bool IsAssignableTo(this Type type, Type t)
			=> t.IsAssignableFrom(type);

		public static TDelegate CreateDelegate<TDelegate>(this MethodInfo method)
			where TDelegate : Delegate
			=> (TDelegate)method.CreateDelegate(typeof(TDelegate));

		public static Type[] GetParameterTypes(this MethodBase source)
		{
			var parameters = source.GetParameters();
			Type[] parameterTypes = new Type[parameters.Length];
			for (var i = 0; i < parameters.Length; i++)
			{
				parameterTypes[i] = parameters[i].ParameterType;
			}

			return parameterTypes;
		}

		public static Type GetMemberValueType(this MemberInfo source)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}

#if !NETFX_CORE && !NETSTANDARD1_1 && !NETSTANDARD1_3
			var asType = source as Type;
			if (asType != null)
			{
				// Nested type.
				return asType;
			}
#else
			Debug.Assert(typeof(MemberInfo).IsAssignableFrom(typeof(Type)), "Type is assginable to MemberInfo on this platform, so should not step in this line.");
			Debug.Assert(typeof(Type).IsAssignableFrom(typeof(TypeInfo)), "TypeInfo is assginable to Type on this platform, so should not step in this line.");

			var asTypeInfo = source as TypeInfo;
			if (asTypeInfo != null)
			{
				// Nested type.
				return asTypeInfo.AsType();
			}
#endif // !NETFX_CORE

			var asProperty = source as PropertyInfo;
			var asField = source as FieldInfo;

			if (asProperty == null && asField == null)
			{
				throw new InvalidOperationException($"'{source}'({source.GetType()}) is not field nor property.");
			}

			return asProperty != null ? asProperty.PropertyType : asField!.FieldType;
		}
	}
}
