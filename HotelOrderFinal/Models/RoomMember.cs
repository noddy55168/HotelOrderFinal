﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace HotelOrderFinal.Models
{
    public partial class RoomMember
    {
        public RoomMember()
        {
            Comment = new HashSet<Comment>();
            Discount = new HashSet<Discount>();
            Order = new HashSet<Order>();
        }

        public string AdminId { get; set; }
        public string MemberId { get; set; }
        public string MemberName { get; set; }
        public DateTime? MemberBirthday { get; set; }
        public string MemberGender { get; set; }
        public string MemberIdentity { get; set; }
        public string MemberPhone { get; set; }
        public string MemberEmail { get; set; }
        public string MemberPhoto { get; set; }
        public string MemberPassword { get; set; }
        public string MemberLavel { get; set; }
        public string MemberOrderTotal { get; set; }

        public virtual RoomAdmin Admin { get; set; }
        public virtual ICollection<Comment> Comment { get; set; }
        public virtual ICollection<Discount> Discount { get; set; }
        public virtual ICollection<Order> Order { get; set; }
    }
}