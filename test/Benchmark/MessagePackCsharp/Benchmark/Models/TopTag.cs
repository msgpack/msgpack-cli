// Copyright (c) All contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.


using ProtoBuf;

namespace Benchmark.Models
{
    [ProtoContract, System.Serializable, System.Runtime.Serialization.DataContract, MessagePack.MessagePackObject]
    public class TopTag : IGenericEquality<TopTag>
    {
        [System.Runtime.Serialization.DataMember, ProtoMember(1), MessagePack.Key(1 - 1)]
        public string tag_name { get; set; }

        [System.Runtime.Serialization.DataMember, ProtoMember(2), MessagePack.Key(2 - 1)]
        public int? question_score { get; set; }

        [System.Runtime.Serialization.DataMember, ProtoMember(3), MessagePack.Key(3 - 1)]
        public int? question_count { get; set; }

        [System.Runtime.Serialization.DataMember, ProtoMember(4), MessagePack.Key(4 - 1)]
        public int? answer_score { get; set; }

        [System.Runtime.Serialization.DataMember, ProtoMember(5), MessagePack.Key(5 - 1)]
        public int? answer_count { get; set; }

        [System.Runtime.Serialization.DataMember, ProtoMember(6), MessagePack.Key(6 - 1)]
        public int? user_id { get; set; }

        public bool Equals(TopTag obj)
        {
            return
                this.answer_count.TrueEquals(obj.answer_count) &&
                this.answer_score.TrueEquals(obj.answer_score) &&
                this.question_count.TrueEquals(obj.question_count) &&
                this.question_score.TrueEquals(obj.question_score) &&
                this.tag_name.TrueEqualsString(obj.tag_name) &&
                this.user_id.TrueEquals(obj.user_id);
        }

        public bool EqualsDynamic(dynamic obj)
        {
            return
                this.answer_count.TrueEquals((int?)obj.answer_count) &&
                this.answer_score.TrueEquals((int?)obj.answer_score) &&
                this.question_count.TrueEquals((int?)obj.question_count) &&
                this.question_score.TrueEquals((int?)obj.question_score) &&
                this.tag_name.TrueEqualsString((string)obj.tag_name) &&
                this.user_id.TrueEquals((int?)obj.user_id);
        }
    }
}
