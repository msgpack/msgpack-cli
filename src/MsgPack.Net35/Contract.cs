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
using System.Collections.Generic;
using System.Threading;

namespace System.Diagnostics.Contracts
{
	/// <summary>
	///		Compatibility Mock.
	/// </summary>
	internal static class Contract
	{
		private static readonly object _contractFailedLock = new object();
		private static EventHandler<ContractFailedEventArgs> _contractFailed;

		public static event EventHandler<ContractFailedEventArgs> ContractFailed
		{
			add
			{
				lock ( _contractFailedLock )
				{
					_contractFailed += value;
				}
			}
			remove
			{
				lock ( _contractFailedLock )
				{
					_contractFailed -= value;
				}
			}
		}

		private static void AssertCore( bool condition, string message )
		{
			if ( !condition )
			{
				var handler = Interlocked.CompareExchange( ref _contractFailed, null, null );
				if ( handler != null )
				{
					var e = new ContractFailedEventArgs();
					handler( null, e );

					if ( e.IsUnwined )
					{
						throw new Exception( message );
					}
				}

				Debug.Assert( condition, message );
			}
		}

		[Conditional( "DEBUG" )]
		public static void Assert( bool condition )
		{
			AssertCore( condition, "Assert failed." );
		}

		[Conditional( "DEBUG" )]
		public static void Assert( bool condition, string message )
		{
			AssertCore( condition, message );
		}

		[Conditional( "DEBUG" )]
		public static void Assume( bool condition )
		{
			AssertCore( condition, "Assume failed." );
		}

		[Conditional( "DEBUG" )]
		public static void Assume( bool condition, string message )
		{
			AssertCore( condition, message );
		}

		[Conditional( "DEBUG" )]
		public static void Requires( bool condition )
		{
			AssertCore( condition, "Precondition failed." );
		}

		[Conditional( "NEVER_COMPILED" )]
		public static void Ensures( bool condition ) { }

		public static T Result<T>()
		{
			return default( T );
		}

		public static T ValueAtReturn<T>( out T value )
		{
			value = default( T );
			return default( T );
		}

		public static bool ForAll<T>( IEnumerable<T> collection, Predicate<T> predicate )
		{
			return true;
		}

		[Conditional( "NEVER_COMPILED" )]
		public static void EndContractBlock() { }
	}

	/// <summary>
	///		Compatibility Mock.
	/// </summary>
	[Conditional( "NEVER_COMPILED" )]
	[AttributeUsage(
		AttributeTargets.Class | AttributeTargets.Delegate
		| AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Event
		| AttributeTargets.Parameter,
		Inherited = true
	)]
	internal sealed class PureAttribute : Attribute { }

	/// <summary>
	///		Compatibility Mock.
	/// </summary>
	[Conditional( "NEVER_COMPILED" )]
	[AttributeUsage( AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Delegate )]
	internal sealed class ContractClassAttribute : Attribute
	{
		public ContractClassAttribute( Type typeContainingContracts ) { }
	}

	/// <summary>
	///		Compatibility Mock.
	/// </summary>
	[Conditional( "NEVER_COMPILED" )]
	[AttributeUsage( AttributeTargets.Class )]
	internal sealed class ContractClassForAttribute : Attribute
	{
		public ContractClassForAttribute( Type typeContractsAreFor ) { }
	}

	internal sealed class ContractFailedEventArgs : EventArgs
	{
		internal bool IsUnwined { get; private set; }

		public void SetUnwind()
		{
			this.IsUnwined = true;
		}
	}
}
