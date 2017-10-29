#region -- License Terms --
// 
// MessagePack for CLI
// 
// Copyright (C) 2016 FUJIWARA, Yusuke
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

#if DEBUG
#define ASSERT
#endif // DEBUG

using System;
#if ASSERT
#if FEATURE_MPCONTRACT
using Contract = MsgPack.MPContract;
#else
using System.Diagnostics.Contracts;
#endif // FEATURE_MPCONTRACT
#endif // ASSERT
using System.Reflection;

namespace MsgPack.Serialization
{
	/// <summary>
	///		Represents and encapsulates context informatino to verify actual type for runtime type polymorphism.
	/// </summary>
	public struct PolymorphicTypeVerificationContext : IEquatable<PolymorphicTypeVerificationContext>
	{
		// TODO: Use ref for internal struct on stack.

		private readonly string _loadingTypeFullName;

		/// <summary>
		///		Gets the full type name including its namespace to be loaded.
		/// </summary>
		/// <value>
		///		The full type name including its namespace to be loaded. This value will not be <c>null</c>.
		/// </value>
		public string LoadingTypeFullName { get { return this._loadingTypeFullName; } }

		private readonly string _loadingAssemblyFullName;

		/// <summary>
		///		Gets the full name of the loading assembly.
		/// </summary>
		/// <value>
		///		The full name of the loading assembly.
		/// </value>
		public string LoadingAssemblyFullName { get { return this._loadingAssemblyFullName; } }

		private readonly AssemblyName _loadingAssemblyName;

		/// <summary>
		///		Gets the name of the loading assembly.
		/// </summary>
		/// <value>
		///		The name of the loading assembly.
		/// </value>
		public AssemblyName LoadingAssemblyName { get { return this._loadingAssemblyName; } }

		internal PolymorphicTypeVerificationContext( string loadingTypeFullName, AssemblyName loadingAssemblyName, string loadingAssemblyFullName )
		{
#if ASSERT
			Contract.Assert( loadingTypeFullName != null );
			Contract.Assert( loadingAssemblyName != null );
#endif // ASSERT
			this._loadingTypeFullName = loadingTypeFullName;
			this._loadingAssemblyName = loadingAssemblyName;
			this._loadingAssemblyFullName = loadingAssemblyFullName;
		}

		/// <summary>
		///		Returns a <see cref="System.String" /> that represents this instance.
		/// </summary>
		/// <returns>
		///		A <see cref="System.String" /> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			if ( this._loadingTypeFullName == null )
			{
				return String.Empty;
			}

			return this._loadingTypeFullName + ", " + this._loadingAssemblyFullName;
		}

		/// <summary>
		///		Determines whether the specified <see cref="System.Object" /> is equal to this instance.
		/// </summary>
		/// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
		/// <returns>
		///		<c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
		/// </returns>
		public override bool Equals( object obj )
		{
			if ( !( obj is PolymorphicTypeVerificationContext ) )
			{
				return false;
			}

			return this.Equals( ( PolymorphicTypeVerificationContext ) obj );
		}

		/// <summary>
		///		Determines whether the specified <see cref="PolymorphicTypeVerificationContext" /> is equal to this instance.
		/// </summary>
		/// <param name="other">The <see cref="PolymorphicTypeVerificationContext" /> to compare with this instance.</param>
		/// <returns>
		///		<c>true</c> if the specified <see cref="PolymorphicTypeVerificationContext" /> is equal to this instance; otherwise, <c>false</c>.
		/// </returns>
		public bool Equals( PolymorphicTypeVerificationContext other )
		{
			return this._loadingTypeFullName == other._loadingTypeFullName && this._loadingAssemblyFullName == other._loadingAssemblyFullName;
		}

		/// <summary>
		///		Returns a hash code for this instance.
		/// </summary>
		/// <returns>
		///		A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
		/// </returns>
		public override int GetHashCode()
		{
			if ( this._loadingTypeFullName == null )
			{
				return 0;
			}

			return this._loadingTypeFullName.GetHashCode() ^ this._loadingAssemblyFullName.GetHashCode();
		}

		/// <summary>
		///		Determines whether the specified <see cref="PolymorphicTypeVerificationContext" />s are equal.
		/// </summary>
		/// <param name="left">The <see cref="PolymorphicTypeVerificationContext" />.</param>
		/// <param name="right">The <see cref="PolymorphicTypeVerificationContext" />.</param>
		/// <returns>
		///		<c>true</c> if the specified <see cref="PolymorphicTypeVerificationContext" />s are equal to each other; otherwise, <c>false</c>.
		/// </returns>
		public static bool operator ==( PolymorphicTypeVerificationContext left, PolymorphicTypeVerificationContext right )
		{
			return left.Equals( right );
		}

		/// <summary>
		///		Determines whether the specified <see cref="PolymorphicTypeVerificationContext" />s are not equal.
		/// </summary>
		/// <param name="left">The <see cref="PolymorphicTypeVerificationContext" />.</param>
		/// <param name="right">The <see cref="PolymorphicTypeVerificationContext" />.</param>
		/// <returns>
		///		<c>true</c> if the specified <see cref="PolymorphicTypeVerificationContext" />s are not equal; otherwise, <c>false</c>.
		/// </returns>
		public static bool operator !=( PolymorphicTypeVerificationContext left, PolymorphicTypeVerificationContext right )
		{
			return !left.Equals( right );
		}
	}
}