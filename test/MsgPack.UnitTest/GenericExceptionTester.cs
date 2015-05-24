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
using System.IO;
#if NETFX_CORE
using System.Linq;
#endif
using System.Linq.Expressions;
#if NETFX_CORE
using System.Reflection;
#endif
#if !NETFX_CORE && !SILVERLIGHT
using System.Runtime.Serialization.Formatters.Binary;
#else
using System.Runtime.Serialization;
#endif // !NETFX_CORE && !SILVERLIGHT
using System.Security;
#if  !NETFX_CORE && !SILVERLIGHT
using System.Security.Permissions;
using System.Security.Policy;
#endif //  !NETFX_CORE && !SILVERLIGHT
#if !MSTEST
using NUnit.Framework;
#else
using TestFixtureAttribute = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestClassAttribute;
using TestAttribute = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestMethodAttribute;
using TimeoutAttribute = NUnit.Framework.TimeoutAttribute;
using Assert = NUnit.Framework.Assert;
using Is = NUnit.Framework.Is;
#endif

#if UNITY
using MsgPack.Serialization;
#endif // UNITY

namespace MsgPack
{
	/// <summary>
	///		Test suite for standard exception constructors, properties, and serialization.
	/// </summary>
	/// <typeparam name="T">Target exception type.</typeparam>
	public sealed class GenericExceptionTester<T>
#if  !NETFX_CORE && !SILVERLIGHT
		: MarshalByRefObject
#endif //  !NETFX_CORE && !SILVERLIGHT
		where T : Exception
	{
		private static readonly Type[] _emptyTypes = new Type[ 0 ];
		private static readonly Type[] _messageConstructorParameterTypes = new[] { typeof( string ) };
		private static readonly Type[] _innerExceptionConstructorParameterTypes = new[] { typeof( string ), typeof( Exception ) };

		private readonly Func<T> _defaultConstructor;
		private readonly Func<string, T> _messageConstructor;
		private readonly Func<string, Exception, T> _innerExceptionConstructor;
		private readonly Exception _constructorException;

		public T CreateTargetInstance( string message, Exception inner )
		{
			return this._innerExceptionConstructor( message, inner );
		}

		/// <summary>
		///		Initializes a new instance of the <see cref="GenericExceptionTester&lt;T&gt;"/> class.
		/// </summary>
		public GenericExceptionTester()
		{
			try
			{
#if UNITY
				this._defaultConstructor = () => ReflectionExtensions.CreateInstancePreservingExceptionType<T>( typeof( T ) );
#elif !NETFX_CORE
				this._defaultConstructor = Expression.Lambda<Func<T>>( Expression.New( typeof( T ).GetConstructor( _emptyTypes ) ) ).Compile();
#else
				this._defaultConstructor = Expression.Lambda<Func<T>>( Expression.New( typeof( T ).GetTypeInfo().DeclaredConstructors.Single( c => c.GetParameters().Length == 0 ) ) ).Compile();
#endif // if UNITY elif !NETFX_CORE
				var message = Expression.Parameter( typeof( string ), "message" );
				var innerException = Expression.Parameter( typeof( Exception ), "innerException" );

#if !UNITY
				this._messageConstructor =
					Expression.Lambda<Func<string, T>>(
#if !NETFX_CORE
						Expression.New( typeof( T ).GetConstructor( _messageConstructorParameterTypes ), message ),
#else
						Expression.New( typeof( T ).GetTypeInfo().DeclaredConstructors.Single( c => c.GetParameters().Select( p => p.ParameterType ).SequenceEqual( _messageConstructorParameterTypes ) ), message ),
#endif // !NETFX_CORE
						message
					).Compile();
#else
				this._messageConstructor = msg => ReflectionExtensions.CreateInstancePreservingExceptionType<T>( typeof( T ), msg );
#endif // !UNITY

#if !UNITY
				this._innerExceptionConstructor =
					Expression.Lambda<Func<string, Exception, T>>(
#if !NETFX_CORE
						Expression.New( typeof( T ).GetConstructor( _innerExceptionConstructorParameterTypes ), message, innerException ),
#else
						Expression.New( typeof( T ).GetTypeInfo().DeclaredConstructors.Single( c => c.GetParameters().Select( p => p.ParameterType ).SequenceEqual( _innerExceptionConstructorParameterTypes ) ), message, innerException ),
#endif // !NETFX_CORE
						message,
						innerException
					).Compile();
#else
				this._innerExceptionConstructor = ( msg, inner ) => ReflectionExtensions.CreateInstancePreservingExceptionType<T>( typeof( T ), msg, inner );
#endif // !UNITY
			}
			catch ( Exception ex )
			{
				this._constructorException = ex;
			}
		}

		/// <summary>
		///		Tests the exception.
		/// </summary>
		public void TestException()
		{
			if ( this._constructorException != null )
			{
				Assert.Fail( this._constructorException.ToString() );
			}

			this.TestDefaultConstructor();
			this.TestMessageConstructor_WithMessage_SetToMessage();
			this.TestMessageConstructor_WithNull_SetToDefaultMessage();
			this.TestInnerExceptionConstructor_WithMessageAndInnerException_SetToMessageAndInnerException();
			this.TestInnerExceptionConstructor_Null_SetToDefaultMessageAndNullInnerException();
#if !NETFX_CORE && !WINDOWS_PHONE && !XAMIOS && !XAMDROID && !UNITY
			this.TestSerialization();
#endif // if !NETFX_CORE && !WINDOWS_PHONE && !XAMIOS && !XAMDROID && !UNITY
		}

		private void TestDefaultConstructor()
		{
			var target = this._defaultConstructor();
			Assert.That( target.Message, Is.Not.Null.And.Not.Empty, "Message should not be null nor empty." );
			TestToString( target, typeof( T ).Name + "()" );
		}

		private void TestMessageConstructor_WithMessage_SetToMessage()
		{
			var message = Guid.NewGuid().ToString();
			var target = this._messageConstructor( message );
			Assert.That( target.Message, Is.Not.Null.And.StringContaining( message ), "Message should contain message argument." );
			TestToString( target, typeof( T ).Name + "()" );
		}

		private void TestMessageConstructor_WithNull_SetToDefaultMessage()
		{
			var defaultInstance = this._defaultConstructor();
			var target = this._messageConstructor( null );
			Assert.That( target.Message, Is.Not.Null.And.StringContaining( defaultInstance.Message ), "Message should equal to default message." );
			TestToString( target, typeof( T ).Name + "()" );
		}

		private void TestInnerExceptionConstructor_WithMessageAndInnerException_SetToMessageAndInnerException()
		{
			var message = Guid.NewGuid().ToString();
			var innerException = new Exception();
			var target = this._innerExceptionConstructor( message, innerException );
			Assert.That( target.Message, Is.Not.Null.And.StringContaining( message ), "Message should contain message argument." );
			Assert.That( target.InnerException, Is.SameAs( innerException ), "InnerException should contain innerException argument." );
			TestToString( target, typeof( T ).Name + "()" );
		}

		private void TestInnerExceptionConstructor_Null_SetToDefaultMessageAndNullInnerException()
		{
			var defaultInstance = this._defaultConstructor();
			var target = this._innerExceptionConstructor( null, null );
			Assert.That( target.Message, Is.Not.Null.And.StringContaining( defaultInstance.Message ), "Message should equal to default message." );
			Assert.That( target.InnerException, Is.Null, "InnerException should be null." );
			TestToString( target, typeof( T ).Name + "()" );
		}

		private static void TestToString( T target, string ctor )
		{
			TestToStringCore( MakeStackTrace( target ), ctor );
		}

		private static T MakeStackTrace( T target )
		{
			try
			{
				throw target;
			}
			catch ( T ex )
			{
				return ex;
			}
		}

		private static void TestToStringCore( T target, string ctor )
		{
			Assert.That( target.ToString(), Is.Not.Null.And.Not.Empty.And.StringContaining( target.Message ).And.StringContaining( target.GetType().FullName ).And.StringContaining( target.StackTrace ), "ToString() should contain Message, Type.FullName, and StackTrace ({0}).", ctor );
			if ( target.InnerException != null )
			{
				Assert.That( target.ToString(), Is.Not.Null.And.StringContaining( target.InnerException.Message ), "ToString() should contain InnerException ({0}).", ctor );
			}
		}

#if !NETFX_CORE && !WINDOWS_PHONE && !XAMIOS && !XAMDROID && !UNITY
		private void TestSerialization()
		{
			Assert.That( typeof( T ), Is.BinarySerializable );
			var innerMessage = Guid.NewGuid().ToString();
			var message = Guid.NewGuid().ToString();
			var target = this._innerExceptionConstructor( message, new Exception( innerMessage ) );
			using ( var buffer = new MemoryStream() )
			{
				var serializer = new BinaryFormatter();
				serializer.Serialize( buffer, target );
				buffer.Position = 0;
				var deserialized = serializer.Deserialize( buffer ) as T;
				Assert.That( deserialized, Is.Not.Null );
				Assert.That( deserialized.Message, Is.EqualTo( target.Message ) );
				Assert.That( deserialized.InnerException, Is.Not.Null.And.TypeOf( typeof( Exception ) ) );
				Assert.That( deserialized.InnerException.Message, Is.EqualTo( target.InnerException.Message ) );
			}
		}

		private void TestSerializationOnPartialTrust()
		{
			var appDomainSetUp = new AppDomainSetup() { ApplicationBase = AppDomain.CurrentDomain.SetupInformation.ApplicationBase };
			var evidence = new Evidence();
#if MONO || NETFX_35
#pragma warning disable 0612
			// TODO: patching
			// currently, Mono does not declare AddHostEvidence
			evidence.AddHost( new Zone( SecurityZone.Internet ) );
#pragma warning restore 0612
			var permisions = GetDefaultInternetZoneSandbox();
#else
			evidence.AddHostEvidence( new Zone( SecurityZone.Internet ) );
			var permisions = SecurityManager.GetStandardSandbox( evidence );
#endif
			AppDomain workerDomain = AppDomain.CreateDomain( "PartialTrust", evidence, appDomainSetUp, permisions, GetStrongName( this.GetType() ) );
			try
			{
				var innerMessage = Guid.NewGuid().ToString();
				var message = Guid.NewGuid().ToString();
				workerDomain.SetData( "MsgPack.GenericExceptionTester.InnerMessage", innerMessage );
				workerDomain.SetData( "MsgPack.GenericExceptionTester.Message", message );
				workerDomain.SetData( "MsgPack.GenericExceptionTester.Proxy", this );
				workerDomain.DoCallBack( TestSerializationOnPartialTrustCore );

				var target = workerDomain.GetData( "MsgPack.GenericExceptionTester.Target" ) as T;
				Assert.That( target, Is.Not.Null );
				Assert.That( target.Message, Is.EqualTo( target.Message ) );
				Assert.That( target.InnerException, Is.Not.Null.And.TypeOf( typeof( Exception ) ) );
				Assert.That( target.InnerException.Message, Is.EqualTo( target.InnerException.Message ) );
			}
			finally
			{
				AppDomain.Unload( workerDomain );
			}
		}

#if MONO || NETFX_35
		private static PermissionSet GetDefaultInternetZoneSandbox()
		{
			var permissions = new PermissionSet( PermissionState.None );
			permissions.AddPermission(
				new FileDialogPermission(
					FileDialogPermissionAccess.Open
				)
			);
			permissions.AddPermission(
				new IsolatedStorageFilePermission( PermissionState.None )
				{
					UsageAllowed = IsolatedStorageContainment.ApplicationIsolationByUser,
					UserQuota = 1024000
				}			
			);
			permissions.AddPermission(
				new SecurityPermission(
					SecurityPermissionFlag.Execution
				)
			);
			permissions.AddPermission(
				new UIPermission(
					UIPermissionWindow.SafeTopLevelWindows,
					UIPermissionClipboard.OwnClipboard
				)
			);
			
			return permissions;
		}
#endif // if MONO || NETFX_35

		public static void TestSerializationOnPartialTrustCore()
		{
			var innerMessage = AppDomain.CurrentDomain.GetData( "MsgPack.GenericExceptionTester.InnerMessage" ) as string;
			var message = AppDomain.CurrentDomain.GetData( "MsgPack.GenericExceptionTester.Message" ) as string;
			var instance = AppDomain.CurrentDomain.GetData( "MsgPack.GenericExceptionTester.Proxy" ) as GenericExceptionTester<T>;
			var target = instance.CreateTargetInstance( message, new Exception( innerMessage ) );
			Assert.That( target, Is.Not.Null );
			Assert.That( target.Message, Is.EqualTo( target.Message ) );
			Assert.That( target.InnerException, Is.Not.Null.And.TypeOf( typeof( Exception ) ) );
			Assert.That( target.InnerException.Message, Is.EqualTo( target.InnerException.Message ) );
			AppDomain.CurrentDomain.SetData( "MsgPack.GenericExceptionTester.Target", target );
		}

		private static StrongName GetStrongName( Type type )
		{
			var assemblyName = type.Assembly.GetName();
			return new StrongName( new StrongNamePublicKeyBlob( assemblyName.GetPublicKey() ), assemblyName.Name, assemblyName.Version );
		}
#endif // if !NETFX_CORE && !WINDOWS_PHONE && !XAMIOS && !XAMDROID && !UNITY
	}
}
