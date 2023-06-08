using System.ComponentModel;

namespace HotelOrderFinal.ViewModels
{
    public class ShowOrderDetailData
    {
        [DisplayName("訂單ID")]
        public string orderID { get; set; }
        [DisplayName("入住日期")]
        public string CheckInDate { get; set; }
        [DisplayName("退房日期")]
        public string CheckOutDate { get; set; }
        [DisplayName("支付日期")]
        public string PaymentDate { get; set; }
        public string RoomClassName { get; set; }
        public int? PaymentID { get; set; }
        [DisplayName("支付金額")]
        public int PaymentPrice { get; set; }
        public int? OrderDetailStatusID { get; set; }
        public string RoomID { get; set; }
        [DisplayName("訂單明細備註")]
        public string OrderDetailRemark { get; set; }
        public string RoomClassPhoto1 { get; set; }
        public string RoomClassPhoto2 { get; set; }
        public string RoomClassPhoto3 { get; set; }
    }
}
