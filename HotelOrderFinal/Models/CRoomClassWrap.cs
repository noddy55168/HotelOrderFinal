namespace HotelOrderFinal.Models
{
    public class CRoomClassWrap
    {
        private RoomClass _roomClass;

        public RoomClass roomClass
        {
            get { return _roomClass; }
            set { _roomClass = value; }
        }

        public CRoomClassWrap()
        {
            roomClass = new RoomClass();
        }
        
        public string RoomClassId 
        {
            get { return _roomClass.RoomClassId; }
            set { _roomClass.RoomClassId = value; }
        }
public string RoomClassName
        {
            get { return _roomClass.RoomClassName; }
            set { _roomClass.RoomClassName = value; }
        }
        public string RoomClassDetail
        {
            get { return _roomClass.RoomClassDetail; }
            set { _roomClass.RoomClassDetail = value; }
        }
        public string RoomClassPhoto1
        {
            get { return _roomClass.RoomClassPhoto1; }
            set { _roomClass.RoomClassPhoto1 = value; }
        }
        public string RoomClassPhoto2
        {
            get { return _roomClass.RoomClassPhoto2; }
            set { _roomClass.RoomClassPhoto2 = value; }
        }
        public string RoomClassPhoto3
        {
            get { return _roomClass.RoomClassPhoto3; }
            set { _roomClass.RoomClassPhoto3 = value; }
        }
        public decimal? WeekdayPrice
        {
            get { return _roomClass.WeekdayPrice; }
            set { _roomClass.WeekdayPrice = value; }
        }
        public decimal? HolidayPrice
        {
            get { return _roomClass.HolidayPrice; }
            set { _roomClass.HolidayPrice = value; }
        }
        public decimal? AddPrice
        {
            get { return _roomClass.AddPrice; }
            set { _roomClass.AddPrice = value; }
        }

        public IFormFile photo1 { get; set; }
        public IFormFile photo2 { get; set; }
        public IFormFile photo3 { get; set; }
    }
}
