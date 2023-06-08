using HotelOrderFinal.Models;
using System.ComponentModel;

namespace HotelOrderFinal.ViewModels
{
    public partial class ShowOderData
    {     

         [DisplayName("會員ID")]
        public string MemberId { get; set; }
        [DisplayName("訂單ID")]
        public string OrderId { get; set; }
        [DisplayName("訂單日期")]
        public DateTime? OrderDate { get; set; }
        [DisplayName("訂單金額")]
        public int OrderTotalPrice { get; set; }
        [DisplayName("入住人數")]
        public int? CheckInPeople { get; set; }
        [DisplayName("備註")]
        public string OrderRemark { get; set; }

        [DisplayName("活動ID")]
        public string ActivityID { get; set; }
        [DisplayName("折扣ID")]
        public string DiscountID { get; set; }

        [DisplayName("活動人數")]
        public int? ActivityPeople { get; set; }
        public string RoomClassPhoto1 { get; set; }
        public string RoomClassPhoto2 { get; set; }
        public string RoomClassPhoto3 { get; set; }




    }
}
