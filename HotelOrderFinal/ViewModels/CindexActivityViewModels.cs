using HotelOrderFinal.Models;

namespace HotelOrderFinal.ViewModels
{
    public class CindexActivityViewModels
    {
        public List<int> ActivityID;
        public List<string> ActivityImage;

        public List<HotelIndustry> hotelIndustry { get; set; }
    }
}
