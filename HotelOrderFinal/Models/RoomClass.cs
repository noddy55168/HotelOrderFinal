﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace HotelOrderFinal.Models
{
    public partial class RoomClass
    {
        public RoomClass()
        {
            Comment = new HashSet<Comment>();
            Room = new HashSet<Room>();
        }

        public string RoomClassId { get; set; }
        public string RoomClassName { get; set; }
        public string RoomClassDetail { get; set; }
        public decimal? WeekdayPrice { get; set; }
        public decimal? HolidayPrice { get; set; }
        public decimal? AddPrice { get; set; }

        public virtual ICollection<Comment> Comment { get; set; }
        public virtual ICollection<Room> Room { get; set; }
    }
}