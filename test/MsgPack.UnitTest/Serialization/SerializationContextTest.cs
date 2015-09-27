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

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
#if !MSTEST
using NUnit.Framework;
#else
using TestFixtureAttribute = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestClassAttribute;
using TestAttribute = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestMethodAttribute;
using TimeoutAttribute = NUnit.Framework.TimeoutAttribute;
using Assert = NUnit.Framework.Assert;
using Is = NUnit.Framework.Is;
#endif

namespace MsgPack.Serialization
{
	[TestFixture]
	[Timeout( 15000 )]
	public class SerializationContextTest
	{
		[Test]
		public void TestGetSerializer()
		{
			var target = new SerializationContext();
			Assert.That( target.GetSerializer<int>(), Is.Not.Null );
			Assert.That( target.Serializers.Get<ComplexType>( target ), Is.Null );
			Assert.That( target.GetSerializer<int>(), Is.Not.Null );
		}

		[Test]
		public void TestGetSerializer_Null()
		{
			var target = new SerializationContext();
			Assert.Throws<ArgumentNullException>( () => target.GetSerializer( null ) );
		}

#if !XAMIOS && !XAMDROID && !UNITY
		[Test]
		public void TestGetSerializer_Concurrent()
		{
			var target = new SerializationContext();
			new ConcurrentHelper<MessagePackSerializer<ComplexType>>(
				target.GetSerializer<ComplexType>
			).Test();
		}
#endif // !XAMIOS && !XAMDROID && !UNITY

		[Test]
		public void TestGetSerializer_Type()
		{
			var target = new SerializationContext();
			Assert.That( target.GetSerializer( typeof( int ) ), Is.Not.Null );
#if !XAMIOS && !XAMDROID && !UNITY
			Assert.That( target.GetSerializer( typeof( ComplexType ) ), Is.Not.Null );
#endif // !XAMIOS && !XAMDROID && !UNITY
		}

#if !XAMIOS && !XAMDROID && !UNITY
		[Test]
		public void TestGetSerializer_TypeConcurrent()
		{
			var target = new SerializationContext();
			new ConcurrentHelper<MessagePackSerializer<ComplexType>>(
				() => ( MessagePackSerializer<ComplexType> )target.GetSerializer( typeof( ComplexType ) )
			).Test();
		}
#endif // !XAMIOS && !XAMDROID && !UNITY

		[Test]
		public void TestDefaultCollectionTypes_Default_Check()
		{
			var context = new SerializationContext();
			Assert.That( context.DefaultCollectionTypes.Get( typeof( IList<> ) ), Is.EqualTo( typeof( List<> ) ) );
#if !NETFX_35
			Assert.That( context.DefaultCollectionTypes.Get( typeof( ISet<> ) ), Is.EqualTo( typeof( HashSet<> ) ) );
#endif
			Assert.That( context.DefaultCollectionTypes.Get( typeof( ICollection<> ) ), Is.EqualTo( typeof( List<> ) ) );
#if !NETFX_35 && !UNITY && !NETFX_40
		    Assert.That( context.DefaultCollectionTypes.Get( typeof( IReadOnlyCollection<> ) ), Is.EqualTo( typeof( List<> ) ) );
#endif // !NETFX_35 && !UNITY && !NETFX_40
			Assert.That( context.DefaultCollectionTypes.Get( typeof( IEnumerable<> ) ), Is.EqualTo( typeof( List<> ) ) );
		    Assert.That( context.DefaultCollectionTypes.Get( typeof( IDictionary<,> ) ), Is.EqualTo( typeof( Dictionary<,> ) ) );
#if !NETFX_35 && !UNITY && !NETFX_40
		    Assert.That( context.DefaultCollectionTypes.Get( typeof( IReadOnlyDictionary<,> ) ), Is.EqualTo( typeof( Dictionary<,> ) ) );
#endif // !NETFX_35 && !UNITY && !NETFX_40
		    Assert.That( context.DefaultCollectionTypes.Get( typeof( IList ) ), Is.EqualTo( typeof( List<MessagePackObject> ) ) );
#if !NETFX_35 && !UNITY && !NETFX_40
		    Assert.That( context.DefaultCollectionTypes.Get( typeof( IReadOnlyList<> ) ), Is.EqualTo( typeof( List<> ) ) );
#endif // !NETFX_35 && !UNITY && !NETFX_40
			Assert.That( context.DefaultCollectionTypes.Get( typeof( ICollection ) ), Is.EqualTo( typeof( List<MessagePackObject> ) ) );
			Assert.That( context.DefaultCollectionTypes.Get( typeof( IEnumerable ) ), Is.EqualTo( typeof( List<MessagePackObject> ) ) );
			Assert.That( context.DefaultCollectionTypes.Get( typeof( IDictionary ) ), Is.EqualTo( typeof( MessagePackObjectDictionary ) ) );
		}

		[Test]
		public void TestDefaultCollectionTypes_Register_Collection_New_Ok()
		{
			var context = new SerializationContext();
			context.DefaultCollectionTypes.Register( typeof( NewAbstractCollection<> ), typeof( NewConcreteCollection<> ) );
			Assert.That( context.DefaultCollectionTypes.Get( typeof( NewAbstractCollection<> ) ), Is.EqualTo( typeof( NewConcreteCollection<> ) ) );
		}

		[Test]
		public void TestDefaultCollectionTypes_Register_Collection_Overwrite_Ok()
		{
			var context = new SerializationContext();
			context.DefaultCollectionTypes.Register( typeof( IList<> ), typeof( Collection<> ) );
			Assert.That( context.DefaultCollectionTypes.Get( typeof( IList<> ) ), Is.EqualTo( typeof( Collection<> ) ) );
		}

		[Test]
		public void TestDefaultCollectionTypes_Register_Collection_Closed_Ok()
		{
			var context = new SerializationContext();
			context.DefaultCollectionTypes.Register( typeof( IList<string> ), typeof( Collection<string> ) );
			Assert.That( context.DefaultCollectionTypes.Get( typeof( IList<> ) ), Is.EqualTo( typeof( List<> ) ) );
			Assert.That( context.DefaultCollectionTypes.Get( typeof( IList<string> ) ), Is.EqualTo( typeof( Collection<string> ) ) );
		}

		[Test]
		public void TestDefaultCollectionTypes_Register_NonCollection_Fail()
		{
			var context = new SerializationContext();
			Assert.Throws<ArgumentException>( () => context.DefaultCollectionTypes.Register( typeof( Stream ), typeof( MemoryStream ) ) );
		}

		[Test]
		public void TestDefaultCollectionTypes_Register_ArityIsTooMany_Fail()
		{
			var context = new SerializationContext();
			Assert.Throws<ArgumentException>( () => context.DefaultCollectionTypes.Register( typeof( IEnumerable<> ), typeof( Dictionary<,> ) ) );
		}

		[Test]
		public void TestDefaultCollectionTypes_Register_ArityIsTooFew_Fail()
		{
			var context = new SerializationContext();
			Assert.Throws<ArgumentException>( () => context.DefaultCollectionTypes.Register( typeof( IDictionary<,> ), typeof( StringKeyDictionary<> ) ) );
		}

#if !NETFX_CORE && !SILVERLIGHT
		[Test]
		public void TestDefaultCollectionTypes_Register_Incompatible_Fail()
		{
			var context = new SerializationContext();
			Assert.Throws<ArgumentException>( () => context.DefaultCollectionTypes.Register( typeof( IList<> ), typeof( ArrayList ) ) );
		}
#endif // !NETFX_CORE && !SILVERLIGHT

		[Test]
		public void TestDefaultCollectionTypes_Register_NonGenericGenericMpoOk()
		{
			var context = new SerializationContext();
			context.DefaultCollectionTypes.Register( typeof( IList ), typeof( Collection<MessagePackObject> ) );
			Assert.That( context.DefaultCollectionTypes.Get( typeof( IList ) ), Is.EqualTo( typeof( Collection<MessagePackObject> ) ) );
		}

		[Test]
		public void TestDefaultCollectionTypes_Register_AbstractType_Fail()
		{
			var context = new SerializationContext();
			Assert.Throws<ArgumentException>( () => context.DefaultCollectionTypes.Register( typeof( IList ), typeof( IList<MessagePackObject> ) ) );
		}

		[Test]
		public void TestDefaultCollectionTypes_Register_OpenClose_Fail()
		{
			var context = new SerializationContext();
			Assert.Throws<ArgumentException>( () => context.DefaultCollectionTypes.Register( typeof( IList<> ), typeof( List<string> ) ) );
		}

		[Test]
		public void TestDefaultCollectionTypes_Register_CloseOpen_Fail()
		{
			var context = new SerializationContext();
			Assert.Throws<ArgumentException>( () => context.DefaultCollectionTypes.Register( typeof( IList<string> ), typeof( List<> ) ) );
		}

		[Test]
		public void TestDefault_SafeAndEasySettings()
		{
			Assert.That( SerializationContext.Default, Is.Not.Null );
			Assert.That( SerializationContext.Default.DefaultDateTimeConversionMethod, Is.EqualTo( DateTimeConversionMethod.Native ) );
			Assert.That( SerializationContext.Default.EnumSerializationMethod, Is.EqualTo( EnumSerializationMethod.ByName ) );
#if !XAMIOS && !UNITY
			Assert.That( SerializationContext.Default.GeneratorOption, Is.EqualTo( SerializationMethodGeneratorOption.Fast ) );
#endif // !XAMIOS && !UNITY
			Assert.That( SerializationContext.Default.SerializationMethod, Is.EqualTo( SerializationMethod.Array ) );
		}

		[Test]
		public void TestCreateClassicContext_Version0_5_Compatible()
		{
			var context = SerializationContext.CreateClassicContext();
			Assert.That( context, Is.Not.Null );
			Assert.That( context.DefaultDateTimeConversionMethod, Is.EqualTo( DateTimeConversionMethod.UnixEpoc ) );
			Assert.That( context.EnumSerializationMethod, Is.EqualTo( EnumSerializationMethod.ByName ) );
#if !XAMIOS && !UNITY
			Assert.That( context.GeneratorOption, Is.EqualTo( SerializationMethodGeneratorOption.Fast ) );
#endif // !XAMIOS && !UNITY
			Assert.That( context.SerializationMethod, Is.EqualTo( SerializationMethod.Array ) );
		}

		[Test]
		public void TestConfigureClassic_DefaultIsReplaced()
		{
			var previous = SerializationContext.Default;
			try
			{
				var result = SerializationContext.ConfigureClassic();

				// result is old.
				Assert.That( result, Is.SameAs( previous ) );
				// result is default settings.
				Assert.That( result, Is.Not.Null );
				Assert.That( result.DefaultDateTimeConversionMethod, Is.EqualTo( DateTimeConversionMethod.Native ) );
				Assert.That( result.EnumSerializationMethod, Is.EqualTo( EnumSerializationMethod.ByName ) );
#if !XAMIOS && !UNITY
				Assert.That( result.GeneratorOption, Is.EqualTo( SerializationMethodGeneratorOption.Fast ) );
#endif // !XAMIOS && !UNITY
				Assert.That( result.SerializationMethod, Is.EqualTo( SerializationMethod.Array ) );

				// default is now classic
				Assert.That( SerializationContext.Default, Is.Not.Null );
				Assert.That( SerializationContext.Default.DefaultDateTimeConversionMethod, Is.EqualTo( DateTimeConversionMethod.UnixEpoc ) );
				Assert.That( SerializationContext.Default.EnumSerializationMethod, Is.EqualTo( EnumSerializationMethod.ByName ) );
#if !XAMIOS && !UNITY
				Assert.That( SerializationContext.Default.GeneratorOption, Is.EqualTo( SerializationMethodGeneratorOption.Fast ) );
#endif // !XAMIOS && !UNITY
				Assert.That( SerializationContext.Default.SerializationMethod, Is.EqualTo( SerializationMethod.Array ) );
			}
			finally
			{
				SerializationContext.Default = previous;
			}

			// Verify restore
			Assert.That( SerializationContext.Default.DefaultDateTimeConversionMethod, Is.EqualTo( DateTimeConversionMethod.Native ) );
			Assert.That( SerializationContext.Default.EnumSerializationMethod, Is.EqualTo( EnumSerializationMethod.ByName ) );
#if !XAMIOS && !UNITY
			Assert.That( SerializationContext.Default.GeneratorOption, Is.EqualTo( SerializationMethodGeneratorOption.Fast ) );
#endif // !XAMIOS && !UNITY
			Assert.That( SerializationContext.Default.SerializationMethod, Is.EqualTo( SerializationMethod.Array ) );
		}

		[Test]
		public void TestIssue24()
		{
			var context = new SerializationContext();
			context.Serializers.RegisterOverride( new NetDateTimeSerializer() );
			using ( var buffer = new MemoryStream() )
			{
				var serializer = context.GetSerializer<DateTime>();
				var dt = new DateTime( 999999999999999999L, DateTimeKind.Utc );
				serializer.Pack( buffer, dt );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );
				Assert.That( result, Is.EqualTo( dt ) );
			}
		}

		[Test]
		public void TestIssue27_Dictionary()
		{
			var context = new SerializationContext();
			using ( var buffer = new MemoryStream() )
			{
				var serializer = context.GetSerializer<IDictionary<String, String>>();
				var dic = new Dictionary<string, string> { { "A", "A" } };
				serializer.Pack( buffer, dic );
				buffer.Position = 0;

				var result = serializer.Unpack( buffer );

				Assert.That( result.Count, Is.EqualTo( 1 ) );
				Assert.That( result[ "A" ], Is.EqualTo( "A" ) );
			}
		}

		[Test]
		public void TestIssue27_List()
		{
			var context = new SerializationContext();
			using ( var buffer = new MemoryStream() )
			{
				var serializer = context.GetSerializer<IList<String>>();
				var list = new List<string> { "A" };
				serializer.Pack( buffer, list );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result.Count, Is.EqualTo( 1 ) );
				Assert.That( result[ 0 ], Is.EqualTo( "A" ) );
			}
		}


		[Test]
		public void TestIssue27_Collection()
		{
			var context = new SerializationContext();
			using ( var buffer = new MemoryStream() )
			{
				var serializer = context.GetSerializer<ICollection<string>>();
				var list = new List<string> { "A" };
				serializer.Pack( buffer, list );
				buffer.Position = 0;
				var result = serializer.Unpack( buffer );

				Assert.That( result.Count, Is.EqualTo( 1 ) );
				Assert.That( result.First(), Is.EqualTo( "A" ) );
			}
		}

		[Test]
		public void TestDefaultTableCapacity()
		{
			var table = SerializerRepository.InitializeDefaultTable( new SerializationContext() );
			Assert.That( table.Count, Is.LessThanOrEqualTo( SerializerRepository.DefaultTableCapacity ) );
		}

		[Test]
		public void TestResolveSerializer_NotRegistered_Raised()
		{
			var context = new SerializationContext();
			var raised = 0;
			// ReSharper disable once AccessToModifiedClosure
			EventHandler<ResolveSerializerEventArgs> handler = ( sender, e ) => { raised++; };
			context.ResolveSerializer += handler;
			context.GetSerializer<Image>();
			Assert.That( raised, Is.EqualTo( 1 ) );
		}

		[Test]
		public void TestResolveSerializer_Registered_NotRaised()
		{
			var context = new SerializationContext();
			var raised = 0;
			// ReSharper disable once AccessToModifiedClosure
			EventHandler<ResolveSerializerEventArgs> handler = ( sender, e ) => { raised++; };
			context.ResolveSerializer += handler;
			context.GetSerializer<Image>();
			// Re-get will not raise event because already registered.
			context.GetSerializer<Image>();
			Assert.That( raised, Is.EqualTo( 1 ) );
		}

		[Test]
		public void TestResolveSerializer_AddRemove_Removed()
		{
			var context = new SerializationContext();
			var raised = 0;
			// ReSharper disable once AccessToModifiedClosure
			EventHandler<ResolveSerializerEventArgs> handler = ( sender, e ) => { raised++; };
			context.ResolveSerializer += handler;
			context.GetSerializer<Image>();
			Assert.That( raised, Is.EqualTo( 1 ) );
			
			// Verify removed with new context
			context = new SerializationContext();
			raised = 0;
			context.ResolveSerializer += handler;
			context.ResolveSerializer -= handler;
			context.GetSerializer<Image>();
			Assert.That( raised, Is.EqualTo( 0 ) );
		}

		[Test]
		public void TestResolveSerializer_BuiltIns_NotRaised()
		{
			var context = new SerializationContext();
			var raised = 0;
			// ReSharper disable once AccessToModifiedClosure
			context.ResolveSerializer += ( sender, e ) => { raised++; };
			context.GetSerializer<int[]>();
			context.GetSerializer<int?>();
			context.GetSerializer<List<int>>();
			context.GetSerializer<DateTime>();
			Assert.That( raised, Is.EqualTo( 0 ) );
		}

		[Test]
		public void TestResolveSerializer_SetFound_CanCustomizeSerializer()
		{
			var context = new SerializationContext();
			var raised = 0;
			// ReSharper disable once AccessToModifiedClosure
			EventHandler<ResolveSerializerEventArgs> handler =
				( sender, e ) =>
				{
					raised++;
					e.SetSerializer( new CustomImageSerializer( e.Context ) );
				};
			context.ResolveSerializer += handler;
			context.GetSerializer<Image>();
			Assert.That( raised, Is.EqualTo( 1 ) );
		}

		[Test]
		public void TestResolveSerializer_SetNull_Generated()
		{
			var context = new SerializationContext();
			var raised = 0;
			// ReSharper disable once AccessToModifiedClosure
			EventHandler<ResolveSerializerEventArgs> handler = 
				( sender, e ) =>
				{
					raised++;
					e.SetSerializer<Image>( null );
				};
			context.ResolveSerializer += handler;
			var result = context.GetSerializer<Image>();
			Assert.That( raised, Is.EqualTo( 1 ) );
			Assert.That( result, Is.Not.Null );
#if !NETFX_35 && !UNITY && !NETFX_CORE && !SILVERLIGHT && !XAMIOS && !XAMDROID
			Assert.That( result.GetType().GetAssembly().IsDynamic );
#endif // !NETFX_35 && !UNITY && !NETFX_CORE && !SILVERLIGHT && !XAMIOS && !XAMDROID
		}


		[Test]
		public void TestResolveSerializer_WrongSerializer_Fail()
		{
			var context = new SerializationContext();
			var raised = 0;
			var registered = false;
			// ReSharper disable once AccessToModifiedClosure
			EventHandler<ResolveSerializerEventArgs> handler =
				( sender, e ) =>
				{
					raised++;
					e.SetSerializer( new NetDateTimeSerializer() );
					registered = true;
				};
			context.ResolveSerializer += handler;
			Assert.Throws<InvalidOperationException>(
				() => context.GetSerializer<Image>()
			);
			Assert.That( raised, Is.EqualTo( 1 ) );
			Assert.That( registered, Is.False );
		}

#if !UNITY
		[Test]
		public void TestIssue116()
		{
			var context = new SerializationContext { DefaultDateTimeConversionMethod = DateTimeConversionMethod.UnixEpoc };
			context.Serializers.Register(
				new NetDateTimeSerializer(),
				SerializerRegistrationOptions.AllowOverride | SerializerRegistrationOptions.WithNullable 
			);
			var dt = DateTime.UtcNow;
			using ( var buffer = new MemoryStream() )
			{
				var serializerNullable = context.GetSerializer<DateTime?>();
				var serializerNotNullable = context.GetSerializer<DateTime>();
				serializerNotNullable.Pack( buffer, dt );
				buffer.Position = 0;
				var result = serializerNullable.Unpack( buffer );
				Assert.That( result.GetValueOrDefault(), Is.EqualTo( dt ) );
			}
		}
#endif // !UNITY

		private sealed class NetDateTimeSerializer : MessagePackSerializer<DateTime>
		{
			public NetDateTimeSerializer()
				: base( SerializationContext.Default ) {}

			protected internal override void PackToCore( Packer packer, DateTime objectTree )
			{
				packer.Pack( objectTree.ToUniversalTime().Ticks );
			}

			protected internal override DateTime UnpackFromCore( Unpacker unpacker )
			{
				return new DateTime( unpacker.LastReadData.AsInt64(), DateTimeKind.Utc );
			}
		}

#if !XAMIOS && !XAMDROID && !UNITY
		private sealed class ConcurrentHelper<T> : IDisposable
			where T : class
		{
			private T _gotten;
			private readonly Func<T> _getter;
			private readonly Barrier _barrier;

			public ConcurrentHelper( Func<T> getter )
			{
				this._getter = getter;
				this._barrier = new Barrier( 1 );
			}

			public void Dispose()
			{
				this._barrier.Dispose();
			}

			public void Test()
			{
				var tasks = new List<Task>();
				for ( int i = 0; i < Environment.ProcessorCount * 2; i++ )
				{
					this._barrier.AddParticipant();
					tasks.Add(
						Task.Factory.StartNew(
							// ReSharper disable once PossibleNullReferenceException
							@this => ( @this as ConcurrentHelper<T> ).TestCore(),
							this
						)
					);
				}

				this._barrier.SignalAndWait();
				// Wait join
				Task.WaitAll( tasks.ToArray() );
			}

			private void TestCore()
			{
				var iteration = 50;

				this._barrier.SignalAndWait();

				for ( int i = 0; i < iteration; i++ )
				{
					this.TestValue();
				}
			}

			private void TestValue()
			{
				var value = this._getter();
				if ( value != null )
				{
					var old = Interlocked.Exchange( ref this._gotten, value );
#if !NETFX_CORE
					Assert.That( old, Is.Null.Or.SameAs( value ).Or.InstanceOf( typeof( LazyDelegatingMessagePackSerializer<> ).MakeGenericType( typeof( T ).GetGenericArguments()[ 0 ] ) ) );
#else
					Assert.That( old, Is.Null.Or.SameAs( value ).Or.InstanceOf( typeof( LazyDelegatingMessagePackSerializer<> ).MakeGenericType( typeof( T ).GenericTypeArguments[ 0 ] ) ) );
#endif
				}
			}
		}
#endif // !XAMIOS && !XAMDROID && !UNITY

		public abstract class NewAbstractCollection<T> : Collection<T>
		{

		}

		public sealed class NewConcreteCollection<T> : NewAbstractCollection<T>
		{
		}

		private sealed class StringKeyDictionary<T> : Dictionary<String, T>
		{
		}

		private sealed class CustomImageSerializer : MessagePackSerializer<Image>
		{
			public CustomImageSerializer( SerializationContext context )
				: base( context ) {}

			protected internal override void PackToCore( Packer packer, Image objectTree )
			{
				// Intensionally not implemented
				throw new NotImplementedException();
			}

			protected internal override Image UnpackFromCore( Unpacker unpacker )
			{
				// Intensionally not implemented
				throw new NotImplementedException();
			}
		}

	}
}
