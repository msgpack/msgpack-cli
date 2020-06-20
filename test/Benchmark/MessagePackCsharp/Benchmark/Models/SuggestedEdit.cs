// Copyright (c) All contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.


using System;
using System.Collections.Generic;
using ProtoBuf;

namespace Benchmark.Models
{
    [ProtoContract, System.Serializable, System.Runtime.Serialization.DataContract, MessagePack.MessagePackObject]
    public class SuggestedEdit : IGenericEquality<SuggestedEdit>
    {
        [System.Runtime.Serialization.DataMember, ProtoMember(1), MessagePack.Key(1 - 1)]
        public int? suggested_edit_id { get; set; }

        [System.Runtime.Serialization.DataMember, ProtoMember(2), MessagePack.Key(2 - 1)]
        public int? post_id { get; set; }

        [System.Runtime.Serialization.DataMember, ProtoMember(3), MessagePack.Key(3 - 1)]
        public PostType? post_type { get; set; }

        [System.Runtime.Serialization.DataMember, ProtoMember(4), MessagePack.Key(4 - 1)]
        public string body { get; set; }

        [System.Runtime.Serialization.DataMember, ProtoMember(5), MessagePack.Key(5 - 1)]
        public string title { get; set; }

        [System.Runtime.Serialization.DataMember, ProtoMember(6), MessagePack.Key(6 - 1)]
        public List<string> tags { get; set; }

        [System.Runtime.Serialization.DataMember, ProtoMember(7), MessagePack.Key(7 - 1)]
        public string comment { get; set; }

        [System.Runtime.Serialization.DataMember, ProtoMember(8), MessagePack.Key(8 - 1)]
        public DateTime? creation_date { get; set; }

        [System.Runtime.Serialization.DataMember, ProtoMember(9), MessagePack.Key(9 - 1)]
        public DateTime? approval_date { get; set; }

        [System.Runtime.Serialization.DataMember, ProtoMember(10), MessagePack.Key(10 - 1)]
        public DateTime? rejection_date { get; set; }

        [System.Runtime.Serialization.DataMember, ProtoMember(11), MessagePack.Key(11 - 1)]
        public ShallowUser proposing_user { get; set; }

        public bool Equals(SuggestedEdit obj)
        {
            return
                this.approval_date.TrueEquals(obj.approval_date) &&
                this.body.TrueEqualsString(obj.body) &&
                this.comment.TrueEqualsString(obj.comment) &&
                this.creation_date.TrueEquals(obj.creation_date) &&
                this.post_id.TrueEquals(obj.post_id) &&
                this.post_type.TrueEquals(obj.post_type) &&
                this.proposing_user.TrueEquals(obj.proposing_user) &&
                this.rejection_date.TrueEquals(obj.rejection_date) &&
                this.suggested_edit_id.TrueEquals(obj.suggested_edit_id) &&
                this.tags.TrueEqualsString(obj.tags) &&
                this.title.TrueEqualsString(obj.title);
        }

        public bool EqualsDynamic(dynamic obj)
        {
            return
                this.approval_date.TrueEquals((DateTime?)obj.approval_date) &&
                this.body.TrueEqualsString((string)obj.body) &&
                this.comment.TrueEqualsString((string)obj.comment) &&
                this.creation_date.TrueEquals((DateTime?)obj.creation_date) &&
                this.post_id.TrueEquals((int?)obj.post_id) &&
                this.post_type.TrueEquals((PostType?)obj.post_type) &&
                ((this.proposing_user == null && obj.proposing_user == null) ||
                 this.proposing_user.EqualsDynamic(obj.proposing_user)) &&
                this.rejection_date.TrueEquals((DateTime?)obj.rejection_date) &&
                this.suggested_edit_id.TrueEquals((int?)obj.suggested_edit_id) &&
                this.tags.TrueEqualsString((IEnumerable<string>)obj.tags) &&
                this.title.TrueEqualsString((string)obj.title);
        }
    }
}
