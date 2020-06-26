// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace MsgPack.Serialization.Reflection
{
	internal sealed class ILGenerationContext
	{
		private struct TypeEntry
		{
			public TypeBuilder TypeBuilder { get; }
			public IReadOnlyDictionary<string, FieldBuilder> Fields { get; }
			public IReadOnlyDictionary<MethodSignature, (ConstructorInfo Original, ConstructorBuilder Builder)> Constructors { get; }
			public IReadOnlyDictionary<MethodSignature, (MethodInfo Original, MethodBuilder Builder)> Methods { get; }

			public TypeEntry(
				TypeBuilder typeBuilder,
				Dictionary<string, FieldBuilder> fields,
				Dictionary<MethodSignature, (ConstructorInfo Original, ConstructorBuilder Builder)> constructors,
				Dictionary<MethodSignature, (MethodInfo Original, MethodBuilder Builder)> methods
			)
			{
				this.TypeBuilder = typeBuilder;
				this.Fields = fields;
				this.Constructors = constructors;
				this.Methods = methods;
			}
		}

		private readonly AssemblyBuilder _assemblyBuilder;
		private readonly ModuleBuilder _moduleBuilder;
		private readonly Dictionary<string, TypeEntry> _typeBuilders = new Dictionary<string, TypeEntry>();
		private readonly Dictionary<MethodSignature, (MethodInfo Original, MethodBuilder Builder)> _globalMethods = new Dictionary<MethodSignature, (MethodInfo Original, MethodBuilder Builder)>();
		private readonly Dictionary<string, FieldBuilder> _globalFields = new Dictionary<string, FieldBuilder>();

		public ILGenerationContext(string assemblyName, AssemblyBuilderAccess access)
		{
			this._assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName(assemblyName), access);
			this._moduleBuilder = this._assemblyBuilder.DefineDynamicModule($"{assemblyName}.dll");
		}

#warning TODO: ILGeneration

		public IEnumerable<Type> MakeTypes()
		{
			foreach (var typeBuilder in this._typeBuilders)
			{
				if (typeBuilder.Value.TypeBuilder.IsCreated())
				{
					continue;
				}

				var type = typeBuilder.Value.TypeBuilder.CreateType();
				// Type may be null if typeB
				if (type != null)
				{
					yield return type;
				}
			}
		}

		private static CustomAttributeBuilder ToAttributeBuilder(CustomAttributeData attributeData)
		{
			if (attributeData.NamedArguments.Count == 0)
			{
				return new CustomAttributeBuilder(attributeData.Constructor, attributeData.ConstructorArguments.Select(a => a.Value).ToArray());
			}
			else
			{
				var fields = new List<FieldInfo>();
				var fieldValues = new List<object>();
				var properties = new List<PropertyInfo>();
				var propertyValues = new List<object>();

				foreach (var argument in attributeData.NamedArguments)
				{
					if (argument.TypedValue.Value is null)
					{
						continue;
					}

					if (argument.IsField)
					{
						fields.Add((argument.MemberInfo as FieldInfo)!);
						fieldValues.Add(argument.TypedValue.Value);
					}
					else
					{
						properties.Add((argument.MemberInfo as PropertyInfo)!);
						propertyValues.Add(argument.TypedValue.Value);
					}
				}

				return
					new CustomAttributeBuilder(
						attributeData.Constructor,
						attributeData.ConstructorArguments.Select(a => a.Value).ToArray(),
						properties.ToArray(),
						propertyValues.ToArray(),
						fields.ToArray(),
						fieldValues.ToArray()
					);
			}
		}

		private TypeEntry GetSharedType(Type type)
		{
			const BindingFlags AllFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly;

			if (this._typeBuilders.TryGetValue(type.FullName!, out var typeEntry))
			{
				return typeEntry;
			}

			var fields = new Dictionary<string, FieldBuilder>();
			var constructors = new Dictionary<MethodSignature, (ConstructorInfo Original, ConstructorBuilder Builder)>();
			var methods = new Dictionary<MethodSignature, (MethodInfo Original, MethodBuilder Builder)>();

			var typeBuilder =
				this._moduleBuilder.DefineType(
					type.FullName!,
					type.Attributes,
					type.BaseType,
					type.GetInterfaces()
				);

			foreach (var attribute in type.GetCustomAttributesData())
			{
				typeBuilder.SetCustomAttribute(ToAttributeBuilder(attribute));
			}

			foreach (var field in type.GetFields(AllFlags))
			{
				if ((field.Attributes & FieldAttributes.HasFieldRVA) == 0)
				{
					// normal field
					var builder = typeBuilder.DefineField(field.Name, field.FieldType, field.GetRequiredCustomModifiers(), field.GetOptionalCustomModifiers(), field.Attributes);
					var constant = field.GetRawConstantValue();
					if (constant != null)
					{
						builder.SetConstant(constant);
					}

					foreach (var attribute in field.GetCustomAttributesData())
					{
						builder.SetCustomAttribute(ToAttributeBuilder(attribute));
					}

					fields[field.Name] = builder;
				}
				else
				{
					// RVA field -- for example, back-end of ReadOnlySpan<byte> S => new byte[] { 1, 2, 3 } to avoid newarr.
					var value = field.GetValue(null);
					if (value == null)
					{
						throw new NotSupportedException($"RVA field must have value type object.");
					}

					// Value is non public struct which has size that is equal to .sdata section bytes.
					// So, reinterpret it as byte span and get data as byte array thorugh the span.

					// First, pin boxed value object in GC heap.
					var valueHandle = GCHandle.Alloc(value, GCHandleType.Pinned);
					try
					{
						ReadOnlySpan<byte> data;
						unsafe
						{
							// Reinterpret pinned object reference as pointer.
							void* pValue = valueHandle.AddrOfPinnedObject().ToPointer();
							// Reinterpret boxed object reference as byte span for unmanaged memory.
							// Note that value type's size can be gotten from Marshal.SizeOf(object).
							data = MemoryMarshal.CreateReadOnlySpan(ref Unsafe.AsRef<byte>(pValue), Marshal.SizeOf(value));
						}

						var builder = typeBuilder.DefineInitializedData(field.Name, data.ToArray(), field.Attributes);
						foreach (var attribute in field.GetCustomAttributesData())
						{
							builder.SetCustomAttribute(ToAttributeBuilder(attribute));
						}

						fields[field.Name] = builder;
					}
					finally
					{
						valueHandle.Free();
					}
				}
			}

			foreach (var constructor in type.GetConstructors(AllFlags))
			{
				var builder =
					typeBuilder.DefineConstructor(
						constructor.Attributes,
						constructor.CallingConvention,
						constructor.GetParameters().Select(p => p.ParameterType).ToArray()
					);
				builder.SetImplementationFlags(builder.GetMethodImplementationFlags());

				foreach (var attribute in builder.GetCustomAttributesData())
				{
					builder.SetCustomAttribute(ToAttributeBuilder(attribute));
				}

				constructors[new MethodSignature(constructor)] = (constructor, builder);
			}

			foreach (var method in type.GetMethods(AllFlags))
			{
				var parameters = method.GetParameters();
				var builder =
					typeBuilder.DefineMethod(
						method.Name,
						method.Attributes,
						method.CallingConvention,
						method.ReturnParameter.ParameterType,
						method.ReturnParameter.GetRequiredCustomModifiers(),
						method.ReturnParameter.GetOptionalCustomModifiers(),
						parameters.Select(p => p.ParameterType).ToArray(),
						parameters.Select(p => p.GetRequiredCustomModifiers()).ToArray(),
						parameters.Select(p => p.GetOptionalCustomModifiers()).ToArray()
					);
				builder.SetImplementationFlags(builder.GetMethodImplementationFlags());

				foreach (var attribute in builder.GetCustomAttributesData())
				{
					builder.SetCustomAttribute(ToAttributeBuilder(attribute));
				}

				methods[new MethodSignature(method)] = (method, builder);
			}

			foreach (var property in type.GetProperties(AllFlags))
			{
				var parameters = property.GetIndexParameters();
				var builder =
					typeBuilder.DefineProperty(
						property.Name,
						property.Attributes,
						property.PropertyType,
						property.GetRequiredCustomModifiers(),
						property.GetOptionalCustomModifiers(),
						parameters.Select(p => p.ParameterType).ToArray(),
						parameters.Select(p => p.GetRequiredCustomModifiers()).ToArray(),
						parameters.Select(p => p.GetOptionalCustomModifiers()).ToArray()
					);

				var constant = property.GetRawConstantValue();
				if (constant != null)
				{
					builder.SetConstant(constant);
				}

				foreach (var attribute in property.GetCustomAttributesData())
				{
					builder.SetCustomAttribute(ToAttributeBuilder(attribute));
				}

				var getter = property.GetGetMethod(nonPublic: true);
				if (getter != null)
				{
					builder.SetGetMethod(methods[new MethodSignature(getter)].Builder);
				}

				var setter = property.GetSetMethod(nonPublic: true);
				if (setter != null)
				{
					builder.SetGetMethod(methods[new MethodSignature(setter)].Builder);
				}
			}


			foreach (var @event in type.GetEvents(AllFlags))
			{
				var builder =
					typeBuilder.DefineEvent(
						@event.Name,
						@event.Attributes,
						@event.EventHandlerType!
					);

				foreach (var attribute in @event.GetCustomAttributesData())
				{
					builder.SetCustomAttribute(ToAttributeBuilder(attribute));
				}

				var getter = @event.GetAddMethod(nonPublic: true);
				if (getter != null)
				{
					builder.SetAddOnMethod(methods[new MethodSignature(getter)].Builder);
				}

				var setter = @event.GetRemoveMethod(nonPublic: true);
				if (setter != null)
				{
					builder.SetRemoveOnMethod(methods[new MethodSignature(setter)].Builder);
				}

				var raise = @event.GetRaiseMethod(nonPublic: true);
				if (raise != null)
				{
					builder.SetRaiseMethod(methods[new MethodSignature(raise)].Builder);
				}

				foreach (var other in @event.GetOtherMethods(nonPublic: true))
				{
					builder.AddOtherMethod(methods[new MethodSignature(other)].Builder);
				}
			}

			typeEntry = new TypeEntry(typeBuilder, fields, constructors, methods);
			this._typeBuilders[type.FullName!] = typeEntry;

			return typeEntry;
		}

		public MethodInfo ResolveMethod(MethodBase declaringMethod, int token)
		{
			var typeArguments = declaringMethod.DeclaringType?.GetGenericArguments();
			var methodArguments = declaringMethod.GetGenericArguments();
			var result = declaringMethod.Module.ResolveMethod(token, (typeArguments ?? Array.Empty<Type>()).Length == 0 ? null : typeArguments, methodArguments.Length == 0 ? null : methodArguments);
			if (result == null)
			{
				// result may be null when the token indicates unresolvable DynamicMethod
				throw new NotSupportedException($"Cannot resolve dynamic method token 0x{token:X8} in module '{declaringMethod.Module}'.");
			}

			if (!result.IsPublic && !result.IsFamily)
			{
				throw new InvalidOperationException($"{result.DeclaringType}.{result} is not public.");
			}

			if (!(result is MethodInfo method))
			{
				throw new InvalidOperationException($"{result.DeclaringType}.{result} is not a method.");
			}

			var declaringType = result.DeclaringType;

			if (declaringType == null)
			{
				throw new NotImplementedException("Global function is not implemented.");
			}

			if (declaringType.IsPublic)
			{
				return method;
			}

			return this.GetSharedType(declaringType).Methods[new MethodSignature(method)].Builder;
		}

		public ConstructorInfo ResolveConstructor(MethodBase declaringMethod, int token)
		{
			var typeArguments = declaringMethod.DeclaringType?.GetGenericArguments();
			var methodArguments = declaringMethod.GetGenericArguments();
			var result = declaringMethod.Module.ResolveMethod(token, (typeArguments ?? Array.Empty<Type>()).Length == 0 ? null : typeArguments, methodArguments.Length == 0 ? null : methodArguments);
			if (!(result is ConstructorInfo constructor))
			{
				throw new InvalidOperationException($"Method token 0x{token:X8} in module '{declaringMethod.Module}' may be dynamic method, it is not a constructor.");
			}

			if (!result.IsPublic && !result.IsFamily)
			{
				throw new InvalidOperationException($"{result.DeclaringType}.{result} is not public.");
			}

			var declaringType = constructor.DeclaringType!;

			if (declaringType.IsPublic)
			{
				return constructor;
			}

			return this.GetSharedType(declaringType).Constructors[new MethodSignature(constructor)].Builder;
		}

		public FieldInfo ResolveField(MethodBase declaringMethod, int token)
		{
			var typeArguments = declaringMethod.DeclaringType?.GetGenericArguments();
			var methodArguments = declaringMethod.GetGenericArguments();
			var result = declaringMethod.Module.ResolveField(token, (typeArguments ?? Array.Empty<Type>()).Length == 0 ? null : typeArguments, methodArguments.Length == 0 ? null : methodArguments);

			// FieldInfo should not be null here.
			if (!result!.IsPublic && !result.IsFamily)
			{
				throw new InvalidOperationException($"{result.DeclaringType}.{result} is not public.");
			}

			var declaringType = result.DeclaringType;

			if (declaringType == null)
			{
				throw new NotImplementedException("Global field is not implemented.");
			}

			if (declaringType.IsPublic)
			{
				return result;
			}

			return this.GetSharedType(declaringType).Fields[result.Name];
		}

		public Type ResolveType(MethodBase declaringMethod, int token)
		{
			var typeArguments = declaringMethod.DeclaringType?.GetGenericArguments();
			var methodArguments = declaringMethod.GetGenericArguments();
			var result = declaringMethod.Module.ResolveType(token, (typeArguments ?? Array.Empty<Type>()).Length == 0 ? null : typeArguments, methodArguments.Length == 0 ? null : methodArguments);

			if (result.IsNested)
			{
				throw new NotImplementedException("Not implemented yet.");
			}

			if (result.IsPublic)
			{
				return result;
			}

			return this.GetSharedType(result).TypeBuilder;
		}
	}
}
