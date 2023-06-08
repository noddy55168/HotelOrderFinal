using HotelOrderFinal.Models;

namespace HotelOrderFinal.ViewModels
{
    public class CShopCartViewModel
    {
        public string RoomId { get; set; }
        public string RoomClassId { get; set; }
        public int? HotelId { get; set; }
        public string HotelName { get; set; }

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

        public int FId { get; set; }
        public List<HotelIndustry> hotels { get; set; }
    }
}
