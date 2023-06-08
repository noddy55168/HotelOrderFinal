using HotelOrderFinal.Models;

namespace HotelOrderFinal.ViewModels
{
    public class CRoomClassViewModel
    {
        public RoomClass RoomClass { get; set; }
        public List<RoomFacility> Facility { get; set; }
        public List<HotelIndustry> HotelIndustry { get; set; }
        public string HotelName { get; set; }

    }
}
