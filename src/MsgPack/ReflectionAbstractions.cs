#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010-2015 FUJIWARA, Yusuke
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
using System.Collections.Generic;
using System.Diagnostics;
#if !UNITY
#if XAMIOS || XAMDROID
using Contract = MsgPack.MPContract;
#else
using System.Diagnostics.Contracts;
#endif // XAMIOS || XAMDROID
#endif // !UNITY
using System.Linq;
using System.Reflection;

namespace MsgPack
{
	internal static class ReflectionAbstractions
	{
		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1802:UseLiteralsWhereAppropriate", Justification = "Same as FCL" )]
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

#if DEBUG
		public static bool GetContainsGenericParameters( this Type source )
		{
#if NETFX_CORE
			return source.GetTypeInfo().ContainsGenericParameters;
#else
			return source.ContainsGenericParameters;
#endif
		}
#endif // DEBUG

		public static Assembly GetAssembly( this Type source )
		{
#if NETFX_CORE
			return source.GetTypeInfo().Assembly;
#else
			return source.Assembly;
#endif
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Wrong detection" )]
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

#if DEBUG
		public static IEnumerable<PropertyInfo> GetProperties( this Type source )
		{
			return source.GetRuntimeProperties();
		}

		public static FieldInfo GetField( this Type source, string name )
		{
			return source.GetRuntimeField( name );
		}
#endif

		public static ConstructorInfo GetConstructor( this Type source, Type[] parameteres )
		{
			return source.GetTypeInfo().DeclaredConstructors.SingleOrDefault( c => c.GetParameters().Select( p => p.ParameterType ).SequenceEqual( parameteres ) );
		}

		public static IEnumerable<ConstructorInfo> GetConstructors( this Type source )
		{
			return source.GetTypeInfo().DeclaredConstructors.Where( c => c.IsPublic );
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

		public static MethodInfo GetGetMethod( this PropertyInfo source )
		{
			return GetGetMethod( source, false );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "containsNonPublic", Justification = "For API compabitility" )]
		public static MethodInfo GetGetMethod( this PropertyInfo source, bool containsNonPublic )
		{
			var getter = source.GetMethod;
			return ( containsNonPublic || getter.IsPublic ) ? getter : null;
		}

		public static MethodInfo GetSetMethod( this PropertyInfo source )
		{
			return GetSetMethod( source, false );
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "containsNonPublic", Justification = "For API compabitility" )]
		public static MethodInfo GetSetMethod( this PropertyInfo source, bool containsNonPublic )
		{
			var setter = source.SetMethod;
			return ( containsNonPublic || setter.IsPublic ) ? setter : null;
		}

		public static IEnumerable<Type> FindInterfaces( this Type source, Func<Type, object, bool> filter, object filterCriteria )
		{
			return source.GetTypeInfo().ImplementedInterfaces.Where( t => filter( t, filterCriteria ) );
		}

		public static InterfaceMapping GetInterfaceMap( this Type source, Type interfaceType )
		{
			return source.GetTypeInfo().GetRuntimeInterfaceMap( interfaceType );
		}

		public static IEnumerable<CustomAttributeData> GetCustomAttributesData( this Type source )
		{
			return source.GetTypeInfo().CustomAttributes;
		}

		public static IEnumerable<CustomAttributeData> GetCustomAttributesData( this MemberInfo source )
		{
			return source.CustomAttributes;
		}

		public static Type GetAttributeType( this CustomAttributeData source )
		{
			return source.AttributeType;
		}

		public static string GetMemberName( this CustomAttributeNamedArgument source )
		{
			return source.MemberName;
		}
#else
		public static T GetCustomAttribute<T>( this MemberInfo source )
			where T : Attribute
		{
			return Attribute.GetCustomAttribute( source, typeof( T ) ) as T;
		}

#if NETFX_35 || NETFX_40 || SILVERLIGHT || UNITY
		public static bool IsDefined( this MemberInfo source, Type attributeType )
		{
			return Attribute.IsDefined( source, attributeType );
		}
#endif // NETFX_35 || NETFX_40 || SILVERLIGHT || UNITY

#if !SILVERLIGHT
		public static Type GetAttributeType( this CustomAttributeData source )
		{
			return source.Constructor.DeclaringType;
		}

		public static string GetMemberName( this CustomAttributeNamedArgument source )
		{
			return source.MemberInfo.Name;
		}

#else
		public static Type GetAttributeType( this Attribute source )
		{
			return source.GetType();
		}
#endif // !SILVERLIGHT
#endif // NETFX_CORE

		public static string GetCultureName( this AssemblyName source )
		{
#if NETFX_35 || NETFX_40 || SILVERLIGHT || UNITY
			return source.CultureInfo.Name;
#else
			return source.CultureName;
#endif
		}

#if NETFX_35 || UNITY
		public static IEnumerable<CustomAttributeData> GetCustomAttributesData( this MemberInfo source )
		{
			return CustomAttributeData.GetCustomAttributes( source );
		}

		public static IEnumerable<CustomAttributeData> GetCustomAttributesData( this ParameterInfo source )
		{
			return CustomAttributeData.GetCustomAttributes( source );
		}
#endif // NETFX_35 || UNITY

#if NETFX_CORE
		public static IEnumerable<CustomAttributeData> GetCustomAttributesData( this ParameterInfo source )
		{
			return source.CustomAttributes;
		}
#endif // NETFX_CORE

#if SILVERLIGHT
		public static IEnumerable<Attribute> GetCustomAttributesData( this MemberInfo source )
		{
			return source.GetCustomAttributes( false ).OfType<Attribute>();
		}

		public static IEnumerable<NamedArgument> GetNamedArguments( this Attribute attribute )
		{
			return
				attribute.GetType()
					.GetMembers( BindingFlags.Public | BindingFlags.Instance )
					.Where( m => m.MemberType == MemberTypes.Field || m.MemberType == MemberTypes.Property )
					.Select( m => new NamedArgument( attribute, m ) );
		}
#else
		public static IEnumerable<CustomAttributeNamedArgument> GetNamedArguments( this CustomAttributeData source )
		{
			return source.NamedArguments;
		}

		public static CustomAttributeTypedArgument GetTypedValue( this CustomAttributeNamedArgument source )
		{
			return source.TypedValue;
		}
#endif // SILVERLIGHT

#if SILVERLIGHT
		public struct NamedArgument
		{
			private object _instance;
			private MemberInfo _memberInfo;

			public NamedArgument( object instance, MemberInfo memberInfo )
			{
				this._instance = instance;
				this._memberInfo = memberInfo;
			}

			public string GetMemberName()
			{
				return this._memberInfo.Name;
			}

			public KeyValuePair<Type, object> GetTypedValue()
			{
				Type type;
				object value;
				PropertyInfo asProperty;
				if ( ( asProperty = this._memberInfo as PropertyInfo ) != null )
				{
					type = asProperty.PropertyType;
					value = asProperty.GetValue( this._instance, null );
				}
				else
				{
					var asField = this._memberInfo as FieldInfo;
#if DEBUG
					Contract.Assert( asField != null );
#endif
					type = asField.FieldType;
					value = asField.GetValue( this._instance );
				}

				return new KeyValuePair<Type, object>( type, value );
			}
		}
#endif // SILVERLIGHT

#if NETFX_35 || NETFX_40 || UNITY
		public static Delegate CreateDelegate( this MethodInfo source, Type delegateType )
		{
			return Delegate.CreateDelegate( delegateType, source );
		}
#endif // NETFX_35 || NETFX_40 || UNITY

		public static bool GetHasDefaultValue( this ParameterInfo source )
		{
#if NETFX_35 || NETFX_40 || SILVERLIGHT || UNITY
			return source.DefaultValue != DBNull.Value;
#else
			return source.HasDefaultValue;
#endif
		}
	}
}
