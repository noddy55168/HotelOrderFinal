namespace HotelOrderFinal.Models
{
    public class CFacilityWrap
    {
        private RoomFacility _facility;

        public RoomFacility facility
        {
            get { return _facility; }
            set { _facility = value; }
        }

        public CFacilityWrap()
        {
            facility = new RoomFacility();
        }

        public int facilityId
        {
            get { return _facility.FacilityId; }
            set { _facility.FacilityId = value; }
        }
        public string FacilityName
        {
            get { return _facility.FacilityName; }
            set { _facility.FacilityName = value; }
        }
        public string? FacilityImage
        {
            get { return _facility.FacilityImage; }
            set { _facility.FacilityImage = value; }
        }
       
        public IFormFile photo { get; set; }

    }
}