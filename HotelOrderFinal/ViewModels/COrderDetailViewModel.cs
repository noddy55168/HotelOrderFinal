namespace HotelOrderFinal.ViewModels
{
    public class COrderDetailViewModel
    {
        public string RoomId { get; set; }
        public string RoomClassId { get; set; }

        public string RoomClassName { get; set; }
        public int? RoomClassSize { get; set; }
        public int? RoomClassPeople { get; set; }
        public string RoomClassDetail { get; set; }
        public string RoomClassPhoto1 { get; set; }

        public decimal? WeekdayPrice { get; set; }
        public decimal? HolidayPrice { get; set; }
        public decimal? AddPrice { get; set; }

        public DateTime? CheckInDate { get; set; }
        public DateTime? CheckOutDate { get; set; }

        public int DiscountId { get; set; }
        public string DiscountName { get; set; }
        public byte[] DiscountImage { get; set; }
        public string DiscountDirections { get; set; }
        public decimal? DiscountDiscount { get; set; }
        public bool? DiscountExist { get; set; }


        public int? ActivityPeople { get; set; }//參加活動的人數

        //活動
        public int ActivityId { get; set; }
        public string ActivityName { get; set; }
        public string ActivityImage { get; set; }
        public string ActivityDirections { get; set; }
        public DateTime? ActivityTime { get; set; }
        public string ActivityPlace { get; set; }
        
        public string ActivityCost { get; set; }
        public int? 活動參加總人數 { get; set; }

    }
}
