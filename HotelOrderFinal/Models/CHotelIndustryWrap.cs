namespace HotelOrderFinal.Models
{
    public class CHotelIndustryWrap
    {
        private HotelIndustry _HotelIndustry;
        public HotelIndustry HotelIndustry
        {
            get { return _HotelIndustry; }
            set { _HotelIndustry = value; }
        }
        public CHotelIndustryWrap()
        {
            HotelIndustry = new HotelIndustry();
        }
        public int HotelId
        {
            get { return _HotelIndustry.HotelId; }
            set { _HotelIndustry.HotelId = value; }
        }
        public string HotelName
        {
            get { return _HotelIndustry.HotelName; }
            set { _HotelIndustry.HotelName = value; }
        }
        public string HotelPhone
        {
            get { return _HotelIndustry.HotelPhone; }
            set { _HotelIndustry.HotelPhone = value; }
        }
        public int? HotelRegionId
        {
            get { return _HotelIndustry.HotelRegionId; }
            set { _HotelIndustry.HotelRegionId = value; }
        }
        public string HotelAddress
        {
            get { return _HotelIndustry.HotelAddress; }
            set { _HotelIndustry.HotelAddress = value; }
        }
        public string HotelImage
        {
            get { return _HotelIndustry.HotelImage; }
            set { _HotelIndustry.HotelImage = value; }
        }
        public string HotelImageDiscription
        {
            get { return _HotelIndustry.HotelImageDiscription; }
            set { _HotelIndustry.HotelImageDiscription = value; }
        }
        public string Lat
        {
            get { return _HotelIndustry.Lat; }
            set { _HotelIndustry.Lat = value; }
        }
        public string Lng
        {
            get { return _HotelIndustry.Lng; }
            set { _HotelIndustry.Lng = value; }
        }
        public IFormFile photo { get; set; }
    }
}
