// Copyright (c) All contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.


using System;
using ProtoBuf;

namespace Benchmark.Models
{
    [ProtoContract, System.Serializable, System.Runtime.Serialization.DataContract, MessagePack.MessagePackObject]
    public class NetworkUser : IGenericEquality<NetworkUser>
    {
        [System.Runtime.Serialization.DataMember, ProtoMember(1), MessagePack.Key(1 - 1)]
        public string site_name { get; set; }

        [System.Runtime.Serialization.DataMember, ProtoMember(2), MessagePack.Key(2 - 1)]
        public string site_url { get; set; }

        [System.Runtime.Serialization.DataMember, ProtoMember(3), MessagePack.Key(3 - 1)]
        public int? user_id { get; set; }

        [System.Runtime.Serialization.DataMember, ProtoMember(4), MessagePack.Key(4 - 1)]
        public int? reputation { get; set; }

        [System.Runtime.Serialization.DataMember, ProtoMember(5), MessagePack.Key(5 - 1)]
        public int? account_id { get; set; }

        [System.Runtime.Serialization.DataMember, ProtoMember(6), MessagePack.Key(6 - 1)]
        public DateTime? creation_date { get; set; }

        [System.Runtime.Serialization.DataMember, ProtoMember(7), MessagePack.Key(7 - 1)]
        public UserType? user_type { get; set; }

        [System.Runtime.Serialization.DataMember, ProtoMember(8), MessagePack.Key(8 - 1)]
        public User.BadgeCount badge_counts { get; set; }

        [System.Runtime.Serialization.DataMember, ProtoMember(9), MessagePack.Key(9 - 1)]
        public DateTime? last_access_date { get; set; }

        [System.Runtime.Serialization.DataMember, ProtoMember(10), MessagePack.Key(10 - 1)]
        public int? answer_count { get; set; }

        [System.Runtime.Serialization.DataMember, ProtoMember(11), MessagePack.Key(11 - 1)]
        public int? question_count { get; set; }

        public bool Equals(NetworkUser obj)
        {
            return
                this.account_id.TrueEquals(obj.account_id) &&
                this.answer_count.TrueEquals(obj.answer_count) &&
                this.badge_counts.TrueEquals(obj.badge_counts) &&
                this.creation_date.TrueEquals(obj.creation_date) &&
                this.last_access_date.TrueEquals(obj.last_access_date) &&
                this.question_count.TrueEquals(obj.question_count) &&
                this.reputation.TrueEquals(obj.reputation) &&
                this.site_name.TrueEqualsString(obj.site_name) &&
                this.site_url.TrueEqualsString(obj.site_url) &&
                this.user_id.TrueEquals(obj.user_id) &&
                this.user_type.TrueEquals(obj.user_type);
        }

        public bool EqualsDynamic(dynamic obj)
        {
            return
                this.account_id.TrueEquals((int?)obj.account_id) &&
                this.answer_count.TrueEquals((int?)obj.answer_count) &&
                ((this.badge_counts == null && obj.badge_counts == null) || this.badge_counts.EqualsDynamic(obj.badge_counts)) &&
                this.creation_date.TrueEquals((DateTime?)obj.creation_date) &&
                this.last_access_date.TrueEquals((DateTime?)obj.last_access_date) &&
                this.question_count.TrueEquals((int?)obj.question_count) &&
                this.reputation.TrueEquals((int?)obj.reputation) &&
                this.site_name.TrueEqualsString((string)obj.site_name) &&
                this.site_url.TrueEqualsString((string)obj.site_url) &&
                this.user_id.TrueEquals((int?)obj.user_id) &&
                this.user_type.TrueEquals((UserType?)obj.user_type);
        }
    }
}
