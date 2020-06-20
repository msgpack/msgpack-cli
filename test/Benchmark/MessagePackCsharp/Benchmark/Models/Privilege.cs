// Copyright (c) All contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.


using ProtoBuf;

namespace Benchmark.Models
{
    [ProtoContract, System.Serializable, System.Runtime.Serialization.DataContract, MessagePack.MessagePackObject]
    public class Privilege : IGenericEquality<Privilege>
    {
        [System.Runtime.Serialization.DataMember, ProtoMember(1), MessagePack.Key(1 - 1)]
        public string short_description { get; set; }

        [System.Runtime.Serialization.DataMember, ProtoMember(2), MessagePack.Key(2 - 1)]
        public string description { get; set; }

        [System.Runtime.Serialization.DataMember, ProtoMember(3), MessagePack.Key(3 - 1)]
        public int? reputation { get; set; }

        public bool Equals(Privilege obj)
        {
            return
                this.description.TrueEqualsString(obj.description) &&
                this.reputation.TrueEquals(obj.reputation) &&
                this.short_description.TrueEqualsString(obj.short_description);
        }

        public bool EqualsDynamic(dynamic obj)
        {
            return
                this.description.TrueEqualsString((string)obj.description) &&
                this.reputation.TrueEquals((int?)obj.reputation) &&
                this.short_description.TrueEqualsString((string)obj.short_description);
        }
    }
}
