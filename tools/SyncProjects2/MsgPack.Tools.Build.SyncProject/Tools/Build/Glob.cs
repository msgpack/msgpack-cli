#region -- License Terms --
//  MessagePack for CLI
// 
//  Copyright (C) 2015 FUJIWARA, Yusuke
// 
//     Licensed under the Apache License, Version 2.0 (the "License");
//     you may not use this file except in compliance with the License.
//     You may obtain a copy of the License at
// 
//         http://www.apache.org/licenses/LICENSE-2.0
// 
//     Unless required by applicable law or agreed to in writing, software
//     distributed under the License is distributed on an "AS IS" BASIS,
//     WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//     See the License for the specific language governing permissions and
//     limitations under the License.
#endregion

using System;
using Newtonsoft.Json;

namespace MsgPack.Tools.Build
{
	/// <summary>
	///		Represents glob information for project synchronizer.
	/// </summary>
	[JsonConverter( typeof( GlobJsonConverter ) )]
	public struct Glob : IEquatable<Glob>
	{
		/// <summary>
		///		Gets the path.
		/// </summary>
		/// <value>
		///		The path glob pattern.
		/// </value>
		public string Path { get; }

		/// <summary>
		///		Gets the type.
		/// </summary>
		/// <value>
		///		The type of this entry.
		/// </value>
		public GlobType Type { get; }

		/// <summary>
		///		Initializes a new instance of the <see cref="Glob"/> struct.
		/// </summary>
		/// <param name="path">The path glob pattern.</param>
		/// <param name="type">The type of this entry.</param>
		public Glob( string path, GlobType type )
		{
			this.Path = path;
			this.Type = type;
		}

		/// <summary>
		///		Determines whether the other object which is same type is equal to this instance or not.
		/// </summary>
		/// <param name="other">The object to be compared to this object.</param>
		/// <returns>
		///		<c>true</c>, if the <paramref name="other"/> is equal to this instance; otherwise, <c>false</c>.
		/// </returns>
		public bool Equals( Glob other )
			=> this.Path == other.Path && this.Type == other.Type;

		/// <summary>
		///		Determines whether the specified <see cref="System.Object" />, is equal to this instance.
		/// </summary>
		/// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
		/// <returns>
		///		<c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
		/// </returns>
		public override bool Equals( object obj )
			=> ( obj is Glob other ) ? this.Equals( other ) : false;

		/// <summary>
		///		Returns a hash code for this instance.
		/// </summary>
		/// <returns>
		///		A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
		/// </returns>
		public override int GetHashCode()
			=> ( this.Path?.GetHashCode() ).GetValueOrDefault() ^ this.Type.GetHashCode();

		/// <summary>
		///		Determines whether objects are equal to each other.
		/// </summary>
		/// <param name="left">A <see cref="Glob"/>.</param>
		/// <param name="right">A <see cref="Glob"/>.</param>
		/// <returns>
		///		<c>true</c>, if the specified object are equal; otherwise, <c>false</c>.
		/// </returns>
		public static bool operator ==( Glob left, Glob right )
			=> left.Equals( right );

		/// <summary>
		///		Determines whether objects are not equal to each other.
		/// </summary>
		/// <param name="left">A <see cref="Glob"/>.</param>
		/// <param name="right">A <see cref="Glob"/>.</param>
		/// <returns>
		///		<c>true</c>, if the specified object are not equal; otherwise, <c>false</c>.
		/// </returns>
		public static bool operator !=( Glob left, Glob right )
			=> !left.Equals( right );
	}
}
