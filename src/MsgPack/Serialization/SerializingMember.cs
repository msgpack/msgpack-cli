// Copyright (c) FUJIWARA, Yusuke and all contributors.
// This file is licensed under Apache2 license.
// See the LICENSE in the project root for more information.

using System;
using System.Reflection;

namespace MsgPack.Serialization
{
	/// <summary>
	///		Represents serializing member information.
	/// </summary>
	internal readonly struct SerializingMember
	{
		public MemberInfo Member { get; }
		public DataMemberContract Contract { get; }
		public string MemberName { get; }
		public Utf8String Utf8MemberName { get; }
		public CollectionTraits CollectionTraits { get; }
		public MemberAccessStrategy GetterAccessStrategy { get; }
		public MemberAccessStrategy SetterAccessStrategy { get; }

		public SerializingMember(MemberInfo member, DataMemberContract contract, CollectionTraits traits, MemberAccessStrategy getterAccessStrategy, MemberAccessStrategy setterAccessStrategy)
		{
			this.Member = member;
			this.Contract = contract;
			// Use contract name for aliased map serialization.
			this.MemberName = contract.Name;
			this.Utf8MemberName = new Utf8String(contract.Name);
			this.CollectionTraits = traits;
			this.GetterAccessStrategy = getterAccessStrategy;
			this.SetterAccessStrategy = setterAccessStrategy;
		}

		// for tuple
		public SerializingMember(MemberInfo member, string name)
		{
			this.Member = member;
			this.Contract = default;
			// Use contract name for aliased map serialization.
			this.MemberName = name;
			this.Utf8MemberName = new Utf8String(name);
			// Tuple is not collection.
			this.CollectionTraits = CollectionTraits.NotCollection;
			// Tuples always have public getters.
			this.GetterAccessStrategy = MemberAccessStrategy.Direct;
			this.SetterAccessStrategy = MemberAccessStrategy.ViaConstrutor;
		}

		public EnumMemberSerializationMethod GetEnumMemberSerializationMethod()
			=> this.Member.GetCustomAttribute<MessagePackEnumMemberAttribute>(inherit: true)?.SerializationMethod ?? EnumMemberSerializationMethod.Default;

		public DateTimeMemberConversionMethod GetDateTimeMemberConversionMethod()
			=> this.Member.GetCustomAttribute<MessagePackDateTimeMemberAttribute>(inherit: true)?.DateTimeConversionMethod ?? DateTimeMemberConversionMethod.Default;

		public override string ToString()
		{
			if (this.MemberName == null)
			{
				// default instance.
				return String.Empty;
			}

			return $"{{\"Name\": \"{this.MemberName}\", \"Id\": {this.Contract.Id}, \"Member\": \"{this.Member}\", \"NilImplication\": \"{this.Contract.NilImplication}\" }}";
		}
	}
}
