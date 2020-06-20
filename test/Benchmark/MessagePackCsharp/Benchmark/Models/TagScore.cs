// Copyright (c) All contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.


using ProtoBuf;

namespace Benchmark.Models
{
    [ProtoContract, System.Serializable, System.Runtime.Serialization.DataContract, MessagePack.MessagePackObject]
    public class TagScore : IGenericEquality<TagScore>
    {
        [System.Runtime.Serialization.DataMember, ProtoMember(1), MessagePack.Key(1 - 1)]
        public ShallowUser user { get; set; }

        [System.Runtime.Serialization.DataMember, ProtoMember(2), MessagePack.Key(2 - 1)]
        public int? score { get; set; }

        [System.Runtime.Serialization.DataMember, ProtoMember(3), MessagePack.Key(3 - 1)]
        public int? post_count { get; set; }

        public bool Equals(TagScore obj)
        {
            return
                this.post_count.TrueEquals(obj.post_count) &&
                this.score.TrueEquals(obj.score) &&
                this.user.TrueEquals(obj.user);
        }

        public bool EqualsDynamic(dynamic obj)
        {
            return
                this.post_count.TrueEquals((int?)obj.post_count) &&
                this.score.TrueEquals((int?)obj.score) &&
                ((this.user == null && obj.user == null) || this.user.EqualsDynamic(obj.user));
        }
    }
}
