namespace HotelOrderFinal.Models
{
    public class CActivityWrap
    {
        private Activity _activity;

        public Activity activity
        {
            get { return _activity; }
            set { _activity = value; }
        }

        public CActivityWrap()
        {
            activity = new Activity();
        }

        public int ActivityId
        {
            get { return _activity.ActivityId; }
            set { _activity.ActivityId = value; }
        }
        public string ActivityName
        {
            get { return _activity.ActivityName; }
            set { _activity.ActivityName = value; }
        }
        public string? ActivityImage
        {
            get { return _activity.ActivityImage; }
            set { _activity.ActivityImage = value; }
        }
        public string? ActivityDirections
        {
            get { return _activity.ActivityDirections; }
            set { _activity.ActivityDirections = value; }
        }
        public DateTime? ActivityTime
        {
            get { return _activity.ActivityTime; }
            set { _activity.ActivityTime = value; }
        }
        public string? ActivityPlace
        {
            get { return _activity.ActivityPlace; }
            set { _activity.ActivityPlace = value; }
        }
        public int? ActivityPeople
        {
            get { return _activity.ActivityPeople; }
            set { _activity.ActivityPeople = value; }
        }
        public string? ActivityCost
        {
            get { return _activity.ActivityCost; }
            set { _activity.ActivityCost = value; }
        }
        public IFormFile photo { get; set; }
        
    }
}