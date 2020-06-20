// Copyright (c) All contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#pragma warning disable IDE1006
#pragma warning disable SA1516


using System;
using System.Collections.Generic;
using ProtoBuf;

namespace Benchmark.Models
{
    [ProtoContract, System.Serializable, System.Runtime.Serialization.DataContract, MessagePack.MessagePackObject]
    public class Answer : IGenericEquality<Answer>
    {
        [System.Runtime.Serialization.DataMember, ProtoMember(1), MessagePack.Key(1 - 1)]
        public int? question_id { get; set; }

        [System.Runtime.Serialization.DataMember, ProtoMember(2), MessagePack.Key(2 - 1)]
        public int? answer_id { get; set; }

        [System.Runtime.Serialization.DataMember, ProtoMember(3), MessagePack.Key(3 - 1)]
        public DateTime? locked_date { get; set; }

        [System.Runtime.Serialization.DataMember, ProtoMember(4), MessagePack.Key(4 - 1)]
        public DateTime? creation_date { get; set; }

        [System.Runtime.Serialization.DataMember, ProtoMember(5), MessagePack.Key(5 - 1)]
        public DateTime? last_edit_date { get; set; }

        [System.Runtime.Serialization.DataMember, ProtoMember(6), MessagePack.Key(6 - 1)]
        public DateTime? last_activity_date { get; set; }

        [System.Runtime.Serialization.DataMember, ProtoMember(7), MessagePack.Key(7 - 1)]
        public int? score { get; set; }

        [System.Runtime.Serialization.DataMember, ProtoMember(8), MessagePack.Key(8 - 1)]
        public DateTime? community_owned_date { get; set; }

        [System.Runtime.Serialization.DataMember, ProtoMember(9), MessagePack.Key(9 - 1)]
        public bool? is_accepted { get; set; }

        [System.Runtime.Serialization.DataMember, ProtoMember(10), MessagePack.Key(10 - 1)]
        public string body { get; set; }

        [System.Runtime.Serialization.DataMember, ProtoMember(11), MessagePack.Key(11 - 1)]
        public ShallowUser owner { get; set; }

        [System.Runtime.Serialization.DataMember, ProtoMember(12), MessagePack.Key(12 - 1)]
        public string title { get; set; }

        [System.Runtime.Serialization.DataMember, ProtoMember(13), MessagePack.Key(13 - 1)]
        public int? up_vote_count { get; set; }

        [System.Runtime.Serialization.DataMember, ProtoMember(14), MessagePack.Key(14 - 1)]
        public int? down_vote_count { get; set; }

        [System.Runtime.Serialization.DataMember, ProtoMember(15), MessagePack.Key(15 - 1)]
        public List<Comment> comments { get; set; }

        [System.Runtime.Serialization.DataMember, ProtoMember(16), MessagePack.Key(16 - 1)]
        public string link { get; set; }

        [System.Runtime.Serialization.DataMember, ProtoMember(17), MessagePack.Key(17 - 1)]
        public List<string> tags { get; set; }

        [System.Runtime.Serialization.DataMember, ProtoMember(18), MessagePack.Key(18 - 1)]
        public bool? upvoted { get; set; }

        [System.Runtime.Serialization.DataMember, ProtoMember(19), MessagePack.Key(19 - 1)]
        public bool? downvoted { get; set; }

        [System.Runtime.Serialization.DataMember, ProtoMember(20), MessagePack.Key(20 - 1)]
        public bool? accepted { get; set; }

        [System.Runtime.Serialization.DataMember, ProtoMember(21), MessagePack.Key(21 - 1)]
        public ShallowUser last_editor { get; set; }

        [System.Runtime.Serialization.DataMember, ProtoMember(22), MessagePack.Key(22 - 1)]
        public int? comment_count { get; set; }

        [System.Runtime.Serialization.DataMember, ProtoMember(23), MessagePack.Key(23 - 1)]
        public string body_markdown { get; set; }

        [System.Runtime.Serialization.DataMember, ProtoMember(24), MessagePack.Key(24 - 1)]
        public string share_link { get; set; }

        public bool Equals(Answer obj)
        {
            return
                this.accepted.TrueEquals(obj.accepted) &&
                this.answer_id.TrueEquals(obj.answer_id) &&
                this.body.TrueEqualsString(obj.body) &&
                this.body_markdown.TrueEqualsString(obj.body_markdown) &&
                this.comment_count.TrueEquals(obj.comment_count) &&
                this.comments.TrueEqualsList(obj.comments) &&
                this.community_owned_date.TrueEquals(obj.community_owned_date) &&
                this.creation_date.TrueEquals(obj.creation_date) &&
                this.down_vote_count.TrueEquals(obj.down_vote_count) &&
                this.downvoted.TrueEquals(obj.downvoted) &&
                this.is_accepted.TrueEquals(obj.is_accepted) &&
                this.last_activity_date.TrueEquals(obj.last_activity_date) &&
                this.last_edit_date.TrueEquals(obj.last_edit_date) &&
                this.last_editor.TrueEquals(obj.last_editor) &&
                this.link.TrueEqualsString(obj.link) &&
                this.locked_date.TrueEquals(obj.locked_date) &&
                this.owner.TrueEquals(obj.owner) &&
                this.question_id.TrueEquals(obj.question_id) &&
                this.score.TrueEquals(obj.score) &&
                this.share_link.TrueEqualsString(obj.share_link) &&
                this.tags.TrueEqualsString(obj.tags) &&
                this.title.TrueEqualsString(obj.title) &&
                this.up_vote_count.TrueEquals(obj.up_vote_count) &&
                this.upvoted.TrueEquals(obj.upvoted);
        }

        public bool EqualsDynamic(dynamic obj)
        {
            return
                this.accepted.TrueEquals((bool?)obj.accepted) &&
                this.answer_id.TrueEquals((int?)obj.answer_id) &&
                this.body.TrueEqualsString((string)obj.body) &&
                this.body_markdown.TrueEqualsString((string)obj.body_markdown) &&
                this.comment_count.TrueEquals((int?)obj.comment_count) &&
                this.comments.TrueEqualsListDynamic((IEnumerable<dynamic>)obj.comments) &&
                this.community_owned_date.TrueEquals((DateTime?)obj.community_owned_date) &&
                this.creation_date.TrueEquals((DateTime?)obj.creation_date) &&
                this.down_vote_count.TrueEquals((int?)obj.down_vote_count) &&
                this.downvoted.TrueEquals((bool?)obj.downvoted) &&
                this.is_accepted.TrueEquals((bool?)obj.is_accepted) &&
                this.last_activity_date.TrueEquals((DateTime?)obj.last_activity_date) &&
                this.last_edit_date.TrueEquals((DateTime?)obj.last_edit_date) &&
                ((this.last_editor == null && obj.last_editor == null) || this.last_editor.EqualsDynamic(obj.last_editor)) &&
                this.link.TrueEqualsString((string)obj.link) &&
                this.locked_date.TrueEquals((DateTime?)obj.locked_date) &&
                ((this.owner == null && obj.owner == null) || this.owner.EqualsDynamic(obj.owner)) &&
                this.question_id.TrueEquals((int?)obj.question_id) &&
                this.score.TrueEquals((int?)obj.score) &&
                this.share_link.TrueEqualsString((string)obj.share_link) &&
                this.tags.TrueEqualsString((IEnumerable<string>)obj.tags) &&
                this.title.TrueEqualsString((string)obj.title) &&
                this.up_vote_count.TrueEquals((int?)obj.up_vote_count) &&
                this.upvoted.TrueEquals((bool?)obj.upvoted);
        }
    }

    [MessagePack.MessagePackObject(true)]
    public class Answer2 : IGenericEquality<Answer2>
    {
        public int? question_id { get; set; }
        public int? answer_id { get; set; }
        public DateTime? locked_date { get; set; }
        public DateTime? creation_date { get; set; }
        public DateTime? last_edit_date { get; set; }
        public DateTime? last_activity_date { get; set; }
        public int? score { get; set; }
        public DateTime? community_owned_date { get; set; }
        public bool? is_accepted { get; set; }
        public string body { get; set; }
        public ShallowUser2 owner { get; set; }
        public string title { get; set; }
        public int? up_vote_count { get; set; }
        public int? down_vote_count { get; set; }
        public List<Comment2> comments { get; set; }
        public string link { get; set; }
        public List<string> tags { get; set; }
        public bool? upvoted { get; set; }
        public bool? downvoted { get; set; }
        public bool? accepted { get; set; }
        public ShallowUser2 last_editor { get; set; }
        public int? comment_count { get; set; }
        public string body_markdown { get; set; }
        public string share_link { get; set; }

        public bool Equals(Answer2 obj)
        {
            return
                this.accepted.TrueEquals(obj.accepted) &&
                this.answer_id.TrueEquals(obj.answer_id) &&
                this.body.TrueEqualsString(obj.body) &&
                this.body_markdown.TrueEqualsString(obj.body_markdown) &&
                this.comment_count.TrueEquals(obj.comment_count) &&
                this.comments.TrueEqualsList(obj.comments) &&
                this.community_owned_date.TrueEquals(obj.community_owned_date) &&
                this.creation_date.TrueEquals(obj.creation_date) &&
                this.down_vote_count.TrueEquals(obj.down_vote_count) &&
                this.downvoted.TrueEquals(obj.downvoted) &&
                this.is_accepted.TrueEquals(obj.is_accepted) &&
                this.last_activity_date.TrueEquals(obj.last_activity_date) &&
                this.last_edit_date.TrueEquals(obj.last_edit_date) &&
                this.last_editor.TrueEquals(obj.last_editor) &&
                this.link.TrueEqualsString(obj.link) &&
                this.locked_date.TrueEquals(obj.locked_date) &&
                this.owner.TrueEquals(obj.owner) &&
                this.question_id.TrueEquals(obj.question_id) &&
                this.score.TrueEquals(obj.score) &&
                this.share_link.TrueEqualsString(obj.share_link) &&
                this.tags.TrueEqualsString(obj.tags) &&
                this.title.TrueEqualsString(obj.title) &&
                this.up_vote_count.TrueEquals(obj.up_vote_count) &&
                this.upvoted.TrueEquals(obj.upvoted);
        }

        public bool EqualsDynamic(dynamic obj)
        {
            return
                this.accepted.TrueEquals((bool?)obj.accepted) &&
                this.answer_id.TrueEquals((int?)obj.answer_id) &&
                this.body.TrueEqualsString((string)obj.body) &&
                this.body_markdown.TrueEqualsString((string)obj.body_markdown) &&
                this.comment_count.TrueEquals((int?)obj.comment_count) &&
                this.comments.TrueEqualsListDynamic((IEnumerable<dynamic>)obj.comments) &&
                this.community_owned_date.TrueEquals((DateTime?)obj.community_owned_date) &&
                this.creation_date.TrueEquals((DateTime?)obj.creation_date) &&
                this.down_vote_count.TrueEquals((int?)obj.down_vote_count) &&
                this.downvoted.TrueEquals((bool?)obj.downvoted) &&
                this.is_accepted.TrueEquals((bool?)obj.is_accepted) &&
                this.last_activity_date.TrueEquals((DateTime?)obj.last_activity_date) &&
                this.last_edit_date.TrueEquals((DateTime?)obj.last_edit_date) &&
                ((this.last_editor == null && obj.last_editor == null) || this.last_editor.EqualsDynamic(obj.last_editor)) &&
                this.link.TrueEqualsString((string)obj.link) &&
                this.locked_date.TrueEquals((DateTime?)obj.locked_date) &&
                ((this.owner == null && obj.owner == null) || this.owner.EqualsDynamic(obj.owner)) &&
                this.question_id.TrueEquals((int?)obj.question_id) &&
                this.score.TrueEquals((int?)obj.score) &&
                this.share_link.TrueEqualsString((string)obj.share_link) &&
                this.tags.TrueEqualsString((IEnumerable<string>)obj.tags) &&
                this.title.TrueEqualsString((string)obj.title) &&
                this.up_vote_count.TrueEquals((int?)obj.up_vote_count) &&
                this.upvoted.TrueEquals((bool?)obj.upvoted);
        }
    }
}
