namespace HotelOrderFinal.Models
{
    public class CHotelFacilityWrap
    {
        private HotelFacility _facility;

        public HotelFacility facility
        {
            get { return _facility; }
            set { _facility = value; }
        }

        public CHotelFacilityWrap()
        {
            facility = new HotelFacility();
        }

        public int HotelFacilityId
        {
            get { return _facility.HotelFacilityId; }
            set { _facility.HotelFacilityId = value; }
        }
        public string HotelFacilityName
        {
            get { return _facility.HotelFacilityName; }
            set { _facility.HotelFacilityName = value; }
        }
        public string? HotelFacilityImage
        {
            get { return _facility.HotelFacilityImage; }
            set { _facility.HotelFacilityImage = value; }
        }

        public IFormFile photo { get; set; }
    }
}
