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

using System;
using System.Collections.Generic;
#if !UNITY || MSGPACK_UNITY_FULL
using System.ComponentModel;
#endif //!UNITY || MSGPACK_UNITY_FULL
#if FEATURE_TAP
using System.Threading;
using System.Threading.Tasks;
#endif // FEATURE_TAP

namespace MsgPack.Serialization
{
	// This file was generated from PackHelperParameters.tt
	// DO NET modify this file directly.

	/// <summary>
	///		Represents parameters of <see cref="PackHelpers.PackToArray{TObject}(ref PackToArrayParameters{TObject})"/> method.
	/// </summary>
#if !UNITY || MSGPACK_UNITY_FULL
	[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
	public struct PackToArrayParameters<T>
	{
		/// <summary>
		///		The packer.
		/// </summary>
		public Packer Packer;

		/// <summary>
		///		The object to be packed.
		/// </summary>
		public T Target;

		/// <summary>
		///		Delegates each ones unpack single member in order.
		///		The 1st argument will be <see cref="Packer"/> and 2nd argument will be <see cref="Target"/>.
		/// </summary>
		public IList<Action<Packer, T>> Operations;
	}

#if FEATURE_TAP

	/// <summary>
	///		Represents parameters of <see cref="PackHelpers.PackToArrayAsync{TObject}(ref PackToArrayAsyncParameters{TObject})"/> method.
	/// </summary>
#if !UNITY || MSGPACK_UNITY_FULL
	[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
	public struct PackToArrayAsyncParameters<T>
	{
		/// <summary>
		///		The packer.
		/// </summary>
		public Packer Packer;

		/// <summary>
		///		The object to be packed.
		/// </summary>
		public T Target;

		/// <summary>
		///		Delegates each ones unpack single member in order.
		///		The 1st argument will be <see cref="Packer"/>, 2nd argument will be <see cref="Target"/>,
		///		3rd argument will be <see cref="CancellationToken"/> and returns <see cref="Task"/> represents async operation.
		/// </summary>
		public IList<Func<Packer, T, CancellationToken, Task>> Operations;

		/// <summary>
		///		The token to monitor for cancellation requests. The default value is <see cref="P:CancellationToken.None" />.
		/// </summary>
		public CancellationToken CancellationToken;
	}

#endif // FEATURE_TAP

	/// <summary>
	///		Represents parameters of <see cref="PackHelpers.PackToMap{TObject}(ref PackToMapParameters{TObject})"/> method.
	/// </summary>
#if !UNITY || MSGPACK_UNITY_FULL
	[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
	public struct PackToMapParameters<T>
	{
		/// <summary>
		///		The packer.
		/// </summary>
		public Packer Packer;

		/// <summary>
		///		The object to be packed.
		/// </summary>
		public T Target;

		/// <summary>
		///		Delegates table each ones unpack single member and their keys correspond to unpacking membmer names.
		///		The 1st argument will be <see cref="Packer"/>, 2nd argument will be <see cref="Target"/>, 3rd argument will be 
		/// </summary>
		public IDictionary<string, Action<Packer, T>> Operations;
	}

#if FEATURE_TAP

	/// <summary>
	///		Represents parameters of <see cref="PackHelpers.PackToMapAsync{TObject}(ref PackToMapAsyncParameters{TObject})"/> method.
	/// </summary>
#if !UNITY || MSGPACK_UNITY_FULL
	[EditorBrowsable( EditorBrowsableState.Never )]
#endif // !UNITY || MSGPACK_UNITY_FULL
	public struct PackToMapAsyncParameters<T>
	{
		/// <summary>
		///		The packer.
		/// </summary>
		public Packer Packer;

		/// <summary>
		///		The object to be packed.
		/// </summary>
		public T Target;

		/// <summary>
		///		Delegates table each ones unpack single member and their keys correspond to unpacking membmer names.
		///		The 1st argument will be <see cref="Packer"/>, 2nd argument will be <see cref="Target"/>,
		///		3rd argument will be <see cref="CancellationToken"/> and returns <see cref="Task"/> represents async operation.
		/// </summary>
		public IDictionary<string, Func<Packer, T, CancellationToken, Task>> Operations;

		/// <summary>
		///		The token to monitor for cancellation requests. The default value is <see cref="P:CancellationToken.None" />.
		/// </summary>
		public CancellationToken CancellationToken;
	}

#endif // FEATURE_TAP

}
