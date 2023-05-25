﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace HotelOrderFinal.Models
{
    public partial class Comment
    {
        public int CommentId { get; set; }
        [DisplayName("評點日期")]
        public DateTime? CommentDate { get; set; }
        [DisplayName("會員編號")]
        public string MemberId { get; set; }
        [DisplayName("分類：1.整體 2.房間 3.服務")]
        public int? CommentClass { get; set; }
        [DisplayName("評點：1.非常失望..5.非常滿意")]
        public int? CommentPoint { get; set; }
        [DisplayName("評價摘要")]
        public string CommentDetail { get; set; }

        public virtual RoomMember Member { get; set; }
    }
}