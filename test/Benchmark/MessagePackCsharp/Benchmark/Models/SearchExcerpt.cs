// Copyright (c) All contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.


using System;
using System.Collections.Generic;
using ProtoBuf;

#pragma warning disable SA1300 // Element should begin with upper-case letter

namespace Benchmark.Models
{
    public enum SearchExcerptItemType : byte
    {
        question = 1,
        answer = 2,
    }

    [ProtoContract, System.Serializable, System.Runtime.Serialization.DataContract, MessagePack.MessagePackObject]
    public class SearchExcerpt : IGenericEquality<SearchExcerpt>
    {
        [System.Runtime.Serialization.DataMember, ProtoMember(1), MessagePack.Key(1 - 1)]
        public string title { get; set; }

        [System.Runtime.Serialization.DataMember, ProtoMember(2), MessagePack.Key(2 - 1)]
        public string excerpt { get; set; }

        [System.Runtime.Serialization.DataMember, ProtoMember(3), MessagePack.Key(3 - 1)]
        public DateTime? community_owned_date { get; set; }

        [System.Runtime.Serialization.DataMember, ProtoMember(4), MessagePack.Key(4 - 1)]
        public DateTime? locked_date { get; set; }

        [System.Runtime.Serialization.DataMember, ProtoMember(5), MessagePack.Key(5 - 1)]
        public DateTime? creation_date { get; set; }

        [System.Runtime.Serialization.DataMember, ProtoMember(6), MessagePack.Key(6 - 1)]
        public DateTime? last_activity_date { get; set; }

        [System.Runtime.Serialization.DataMember, ProtoMember(7), MessagePack.Key(7 - 1)]
        public ShallowUser owner { get; set; }

        [System.Runtime.Serialization.DataMember, ProtoMember(8), MessagePack.Key(8 - 1)]
        public ShallowUser last_activity_user { get; set; }

        [System.Runtime.Serialization.DataMember, ProtoMember(9), MessagePack.Key(9 - 1)]
        public int? score { get; set; }

        [System.Runtime.Serialization.DataMember, ProtoMember(10), MessagePack.Key(10 - 1)]
        public SearchExcerptItemType? item_type { get; set; }

        [System.Runtime.Serialization.DataMember, ProtoMember(11), MessagePack.Key(11 - 1)]
        public string body { get; set; }

        [System.Runtime.Serialization.DataMember, ProtoMember(12), MessagePack.Key(12 - 1)]
        public int? question_id { get; set; }

        [System.Runtime.Serialization.DataMember, ProtoMember(13), MessagePack.Key(13 - 1)]
        public bool? is_answered { get; set; }

        [System.Runtime.Serialization.DataMember, ProtoMember(14), MessagePack.Key(14 - 1)]
        public int? answer_count { get; set; }

        [System.Runtime.Serialization.DataMember, ProtoMember(15), MessagePack.Key(15 - 1)]
        public List<string> tags { get; set; }

        [System.Runtime.Serialization.DataMember, ProtoMember(16), MessagePack.Key(16 - 1)]
        public DateTime? closed_date { get; set; }

        [System.Runtime.Serialization.DataMember, ProtoMember(17), MessagePack.Key(17 - 1)]
        public int? answer_id { get; set; }

        [System.Runtime.Serialization.DataMember, ProtoMember(18), MessagePack.Key(18 - 1)]
        public bool? is_accepted { get; set; }

        public bool Equals(SearchExcerpt obj)
        {
            return
                this.answer_count.TrueEquals(obj.answer_count) &&
                this.answer_id.TrueEquals(obj.answer_id) &&
                this.body.TrueEqualsString(obj.body) &&
                this.closed_date.TrueEquals(obj.closed_date) &&
                this.community_owned_date.TrueEquals(obj.community_owned_date) &&
                this.creation_date.TrueEquals(obj.creation_date) &&
                this.excerpt.TrueEqualsString(obj.excerpt) &&
                this.is_accepted.TrueEquals(obj.is_accepted) &&
                this.is_answered.TrueEquals(obj.is_answered) &&
                this.item_type.TrueEquals(obj.item_type) &&
                this.last_activity_date.TrueEquals(obj.last_activity_date) &&
                this.last_activity_user.TrueEquals(obj.last_activity_user) &&
                this.locked_date.TrueEquals(obj.locked_date) &&
                this.owner.TrueEquals(obj.owner) &&
                this.question_id.TrueEquals(obj.question_id) &&
                this.score.TrueEquals(obj.score) &&
                this.tags.TrueEqualsString(obj.tags) &&
                this.title.TrueEqualsString(obj.title);
        }

        public bool EqualsDynamic(dynamic obj)
        {
            return
                this.answer_count.TrueEquals((int?)obj.answer_count) &&
                this.answer_id.TrueEquals((int?)obj.answer_id) &&
                this.body.TrueEqualsString((string)obj.body) &&
                this.closed_date.TrueEquals((DateTime?)obj.closed_date) &&
                this.community_owned_date.TrueEquals((DateTime?)obj.community_owned_date) &&
                this.creation_date.TrueEquals((DateTime?)obj.creation_date) &&
                this.excerpt.TrueEqualsString((string)obj.excerpt) &&
                this.is_accepted.TrueEquals((bool?)obj.is_accepted) &&
                this.is_answered.TrueEquals((bool?)obj.is_answered) &&
                this.item_type.TrueEquals((SearchExcerptItemType?)obj.item_type) &&
                this.last_activity_date.TrueEquals((DateTime?)obj.last_activity_date) &&
                ((this.last_activity_user == null && obj.last_activity_user == null) ||
                 this.last_activity_user.EqualsDynamic(obj.last_activity_user)) &&
                this.locked_date.TrueEquals((DateTime?)obj.locked_date) &&
                ((this.owner == null && obj.owner == null) || this.owner.EqualsDynamic(obj.owner)) &&
                this.question_id.TrueEquals((int?)obj.question_id) &&
                this.score.TrueEquals((int?)obj.score) &&
                this.tags.TrueEqualsString((IEnumerable<string>)obj.tags) &&
                this.title.TrueEqualsString((string)obj.title);
        }
    }
}
