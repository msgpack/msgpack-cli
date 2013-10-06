#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2013 FUJIWARA, Yusuke
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
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MsgPack
{
	internal static class ReflectionAbstractions
	{
		public static readonly char TypeDelimiter = '.';
		public static readonly Type[] EmptyTypes = new Type[ 0 ];

		public static bool GetIsValueType( this Type source )
		{
#if NETFX_CORE
			return source.GetTypeInfo().IsValueType;
#else
			return source.IsValueType;
#endif
		}

		public static bool GetIsEnum( this Type source )
		{
#if NETFX_CORE
			return source.GetTypeInfo().IsEnum;
#else
			return source.IsEnum;
#endif
		}

		public static bool GetIsInterface( this Type source )
		{
#if NETFX_CORE
			return source.GetTypeInfo().IsInterface;
#else
			return source.IsInterface;
#endif
		}

		public static bool GetIsAbstract( this Type source )
		{
#if NETFX_CORE
			return source.GetTypeInfo().IsAbstract;
#else
			return source.IsAbstract;
#endif
		}

		public static bool GetIsGenericType( this Type source )
		{
#if NETFX_CORE
			return source.GetTypeInfo().IsGenericType;
#else
			return source.IsGenericType;
#endif
		}

		public static bool GetIsGenericTypeDefinition( this Type source )
		{
#if NETFX_CORE
			return source.GetTypeInfo().IsGenericTypeDefinition;
#else
			return source.IsGenericTypeDefinition;
#endif
		}

		public static bool GetContainsGenericParameters( this Type source )
		{
#if NETFX_CORE
			return source.GetTypeInfo().ContainsGenericParameters;
#else
			return source.ContainsGenericParameters;
#endif
		}

		public static Assembly GetAssembly( this Type source )
		{
#if NETFX_CORE
			return source.GetTypeInfo().Assembly;
#else
			return source.Assembly;
#endif
		}

		public static bool GetIsPublic( this Type source )
		{
#if NETFX_CORE
			return source.GetTypeInfo().IsPublic;
#else
			return source.IsPublic;
#endif
		}

		public static Type GetBaseType( this Type source )
		{
#if NETFX_CORE
			return source.GetTypeInfo().BaseType;
#else
			return source.BaseType;
#endif
		}

		public static Type[] GetGenericTypeParameters( this Type source )
		{
#if NETFX_CORE
			return source.GetTypeInfo().GenericTypeParameters;
#else
			return source.GetGenericArguments().Where( t => t.IsGenericParameter ).ToArray();
#endif
		}

#if NETFX_CORE
		public static MethodInfo GetMethod( this Type source, string name )
		{
			return source.GetRuntimeMethods().SingleOrDefault( m => m.Name == name );
		}

		public static MethodInfo GetMethod( this Type source, string name, Type[] parameters )
		{
			return source.GetRuntimeMethod( name, parameters );
		}

		public static IEnumerable<MethodInfo> GetMethods( this Type source )
		{
			return source.GetRuntimeMethods();
		}

		public static PropertyInfo GetProperty( this Type source, string name )
		{
			return source.GetRuntimeProperty( name );
		}

		public static ConstructorInfo GetConstructor( this Type source, Type[] parameteres )
		{
			return source.GetTypeInfo().DeclaredConstructors.SingleOrDefault( c => c.GetParameters().Select( p => p.ParameterType ).SequenceEqual( parameteres ) );
		}

		public static IEnumerable<ConstructorInfo> GetConstructors( this Type source )
		{
			return source.GetTypeInfo().DeclaredConstructors;
		}

		public static Type[] GetGenericArguments( this Type source )
		{
			return source.GenericTypeArguments;
		}

		public static bool IsAssignableFrom( this Type source, Type target )
		{
			return source.GetTypeInfo().IsAssignableFrom( target.GetTypeInfo() );
		}

		public static IEnumerable<Type> GetInterfaces( this Type source )
		{
			return source.GetTypeInfo().ImplementedInterfaces;
		}

		public static MethodInfo GetSetMethod( this PropertyInfo source )
		{
			return source.SetMethod;
		}

		public static IEnumerable<Type> FindInterfaces( this Type source, Func<Type, object, bool> filter, object filterCriteria )
		{
			return source.GetTypeInfo().ImplementedInterfaces.Where( t => filter( t, filterCriteria ) );
		}

		public static InterfaceMapping GetInterfaceMap( this Type source, Type interfaceType )
		{
			return source.GetTypeInfo().GetRuntimeInterfaceMap( interfaceType );
		}

		public static bool IsDefined( this Type source, Type attributeType )
		{
			return source.GetTypeInfo().IsDefined( attributeType );
		}
#else
		public static bool IsDefined( this MemberInfo source, Type attributeType )
		{
			return Attribute.IsDefined( source, attributeType );
		}

		public static T GetCustomAttribute<T>( this MemberInfo source )
			where T : Attribute
		{
			return Attribute.GetCustomAttribute( source, typeof( T ) ) as T;
		}		
#endif
	}
}
