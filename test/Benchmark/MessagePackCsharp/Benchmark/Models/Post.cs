// Copyright (c) All contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.


using System;
using System.Collections.Generic;
using ProtoBuf;

namespace Benchmark.Models
{
    [ProtoContract, System.Serializable, System.Runtime.Serialization.DataContract, MessagePack.MessagePackObject]
    public class Post : IGenericEquality<Post>
    {
        [System.Runtime.Serialization.DataMember, ProtoMember(1), MessagePack.Key(1 - 1)]
        public int? post_id { get; set; }

        [System.Runtime.Serialization.DataMember, ProtoMember(2), MessagePack.Key(2 - 1)]
        public PostType? post_type { get; set; }

        [System.Runtime.Serialization.DataMember, ProtoMember(3), MessagePack.Key(3 - 1)]
        public string body { get; set; }

        [System.Runtime.Serialization.DataMember, ProtoMember(4), MessagePack.Key(4 - 1)]
        public ShallowUser owner { get; set; }

        [System.Runtime.Serialization.DataMember, ProtoMember(5), MessagePack.Key(5 - 1)]
        public DateTime? creation_date { get; set; }

        [System.Runtime.Serialization.DataMember, ProtoMember(6), MessagePack.Key(6 - 1)]
        public DateTime? last_activity_date { get; set; }

        [System.Runtime.Serialization.DataMember, ProtoMember(7), MessagePack.Key(7 - 1)]
        public DateTime? last_edit_date { get; set; }

        [System.Runtime.Serialization.DataMember, ProtoMember(8), MessagePack.Key(8 - 1)]
        public int? score { get; set; }

        [System.Runtime.Serialization.DataMember, ProtoMember(9), MessagePack.Key(9 - 1)]
        public int? up_vote_count { get; set; }

        [System.Runtime.Serialization.DataMember, ProtoMember(10), MessagePack.Key(10 - 1)]
        public int? down_vote_count { get; set; }

        [System.Runtime.Serialization.DataMember, ProtoMember(11), MessagePack.Key(11 - 1)]
        public List<Comment> comments { get; set; }

        [System.Runtime.Serialization.DataMember, ProtoMember(12), MessagePack.Key(12 - 1)]
        public string link { get; set; }

        [System.Runtime.Serialization.DataMember, ProtoMember(13), MessagePack.Key(13 - 1)]
        public bool? upvoted { get; set; }

        [System.Runtime.Serialization.DataMember, ProtoMember(14), MessagePack.Key(14 - 1)]
        public bool? downvoted { get; set; }

        [System.Runtime.Serialization.DataMember, ProtoMember(15), MessagePack.Key(15 - 1)]
        public ShallowUser last_editor { get; set; }

        [System.Runtime.Serialization.DataMember, ProtoMember(16), MessagePack.Key(16 - 1)]
        public int? comment_count { get; set; }

        [System.Runtime.Serialization.DataMember, ProtoMember(17), MessagePack.Key(17 - 1)]
        public string body_markdown { get; set; }

        [System.Runtime.Serialization.DataMember, ProtoMember(18), MessagePack.Key(18 - 1)]
        public string share_link { get; set; }

        public bool Equals(Post obj)
        {
            return
                this.body.TrueEqualsString(obj.body) &&
                this.body_markdown.TrueEqualsString(obj.body_markdown) &&
                this.comment_count.TrueEquals(obj.comment_count) &&
                this.comments.TrueEqualsList(obj.comments) &&
                this.creation_date.TrueEquals(obj.creation_date) &&
                this.down_vote_count.TrueEquals(obj.down_vote_count) &&
                this.downvoted.TrueEquals(obj.downvoted) &&
                this.last_activity_date.TrueEquals(obj.last_activity_date) &&
                this.last_edit_date.TrueEquals(obj.last_edit_date) &&
                this.last_editor.TrueEquals(obj.last_editor) &&
                this.link.TrueEqualsString(obj.link) &&
                this.owner.TrueEquals(obj.owner) &&
                this.post_id.TrueEquals(obj.post_id) &&
                this.post_type.TrueEquals(obj.post_type) &&
                this.score.TrueEquals(obj.score) &&
                this.share_link.TrueEqualsString(obj.share_link) &&
                this.up_vote_count.TrueEquals(obj.up_vote_count) &&
                this.upvoted.TrueEquals(obj.upvoted);
        }

        public bool EqualsDynamic(dynamic obj)
        {
            return
                this.body.TrueEqualsString((string)obj.body) &&
                this.body_markdown.TrueEqualsString((string)obj.body_markdown) &&
                this.comment_count.TrueEquals((int?)obj.comment_count) &&
                this.comments.TrueEqualsListDynamic((IEnumerable<dynamic>)obj.comments) &&
                this.creation_date.TrueEquals((DateTime?)obj.creation_date) &&
                this.down_vote_count.TrueEquals((int?)obj.down_vote_count) &&
                this.downvoted.TrueEquals((bool?)obj.downvoted) &&
                this.last_activity_date.TrueEquals((DateTime?)obj.last_activity_date) &&
                this.last_edit_date.TrueEquals((DateTime?)obj.last_edit_date) &&
                ((this.last_editor == null && obj.last_editor == null) || this.last_editor.EqualsDynamic(obj.last_editor)) &&
                this.link.TrueEqualsString((string)obj.link) &&
                ((this.owner == null && obj.owner == null) || this.owner.EqualsDynamic(obj.owner)) &&
                this.post_id.TrueEquals((int?)obj.post_id) &&
                this.post_type.TrueEquals((PostType?)obj.post_type) &&
                this.score.TrueEquals((int?)obj.score) &&
                this.share_link.TrueEqualsString((string)obj.share_link) &&
                this.up_vote_count.TrueEquals((int?)obj.up_vote_count) &&
                this.upvoted.TrueEquals((bool?)obj.upvoted);
        }
    }
}
