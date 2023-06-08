using HotelOrderFinal.Models;
using HotelOrderFinal.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NuGet.Frameworks;
using NuGet.Protocol;
using System.Globalization;
using System.Text.Json;
using static Microsoft.AspNetCore.Razor.Language.TagHelperMetadata;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace HotelOrderFinal.Controllers
{
    public class OrderController : Controller
    {
        public IHttpContextAccessor _contextAccessor;

        public OrderController(IHttpContextAccessor contextAccessor)
        {
            this._contextAccessor = contextAccessor;
        }

        HotelOrderContext db = new HotelOrderContext();
        List<CShopCartViewModel> _cart = new List<CShopCartViewModel>();

        public IActionResult List(string checkInDate, string checkOutDate, string hotelId)
        {         
            // 儲存當前頁面路徑到 Session
            HttpContext.Session.SetString("ReturnUrl", "/Order/List");

            //讀取與設定入退宿時間      
            DateTime checkIn;
            DateTime checkOut;

            ////從活動過來的日期
            //if (!string.IsNullOrEmpty(TempData["CheckInDate"] as string) && !string.IsNullOrEmpty(TempData["CheckOutDate"] as string))
            //{
            //    checkIn = DateTime.ParseExact(TempData["CheckInDate"] as string, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            //    checkOut = DateTime.ParseExact(TempData["CheckOutDate"] as string, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            //}
            ////從首頁過來的日期
            /*else*/ if (!string.IsNullOrEmpty(checkInDate) && !string.IsNullOrEmpty(checkOutDate))
            {
                checkIn = DateTime.ParseExact(checkInDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                checkOut = DateTime.ParseExact(checkOutDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            }
            else
            {
                checkIn = DateTime.Today;
                checkOut = DateTime.Today.AddDays(1);
            }

            HttpContext.Session.SetString("CHECKINDATE", checkIn.ToString("yyyy-MM-dd"));
            HttpContext.Session.SetString("CHECKOUTDATE", checkOut.ToString("yyyy-MM-dd"));

           //抓取飯店名稱列表
            CSearchRoomViewModel h = new CSearchRoomViewModel();
            h.hotels = db.HotelIndustry.ToList();

            //讀取與設定飯店ID
            int hotelid = 0;

            if (!string.IsNullOrEmpty(hotelId) && int.TryParse(hotelId, out int parsedHotelId))
            {
                hotelid = int.Parse(hotelId);
            }
            else
            {
                hotelid = 1;
            }

            var hotelname = db.HotelIndustry.Where(h => h.HotelId == hotelid).Select(h => h.HotelName).FirstOrDefault();
            HttpContext.Session.SetString("HOTELNAME", hotelname.ToString());
            //string hn = HttpContext.Session.GetString("HOTELNAME");

            //查詢空閒房間方法
            //查詢指定時間區間內已被預訂的房間
            var reservedRooms = from od in db.OrderDetail
                                where !(od.CheckOutDate <= checkIn || od.CheckInDate >= checkOut)
                                select od.RoomId;
            var reservedRoomList = reservedRooms.ToList();

            //所有房間扣掉已預訂房間 , 篩選飯店跟用房型分類
            var freeRooms = db.Room.Include(r => r.RoomClass).AsEnumerable()
                .Where(r => !reservedRoomList.Contains(r.RoomId) && r.HotelId == hotelid)
                .GroupBy(r => r.RoomClass.RoomClassName)
                .Select(g => new { g.Key, g }).ToList();

            List<CSearchRoomViewModel> vmList = new List<CSearchRoomViewModel>();

            foreach (var room in freeRooms)
            {

                var f = room.g.First();
                CSearchRoomViewModel vm = new CSearchRoomViewModel();
                vm.RoomClassId = f.RoomClassId;
                vm.RoomClassPhoto1 = f.RoomClass.RoomClassPhoto1;
                vm.RoomClassDetail = f.RoomClass.RoomClassDetail;
                vm.WeekdayPrice = (int)Math.Floor(f.RoomClass.WeekdayPrice.GetValueOrDefault());
                vm.HolidayPrice = (int)Math.Floor(f.RoomClass.HolidayPrice.GetValueOrDefault());
                vm.AddPrice = (int)Math.Floor(f.RoomClass.AddPrice.GetValueOrDefault());
                vm.RoomClassName = f.RoomClass.RoomClassName;
                vm.RoomClassPeople = f.RoomClass.RoomClassPeople;
                vm.RoomClassSize = f.RoomClass.RoomClassSize;
                vm.CheckInDate = checkIn;
                vm.CheckOutDate = checkOut;
                vm.HotelName = f.Hotel.HotelName;
                vmList.Add(vm);
            }

            ViewBag.CheckInDate = checkIn;
            ViewBag.CheckOutDate = checkOut;
            ViewBag.HotelId = hotelid;
            ViewBag.Hotels = h.hotels;

            //if (freeRooms == null)
            //{
            //    return View();
            //}
            //else
            //{
                return View(vmList);
            //}
        }

        //搜尋房間(入/退宿時間、飯店ID)
        [HttpPost]
        public IActionResult SearchRoom(string checkInDate, string checkOutDate, string hotelId)
        {
            //讀取與設定入退宿時間      
            DateTime checkIn;
            DateTime checkOut;

            if (!string.IsNullOrEmpty(checkInDate) && !string.IsNullOrEmpty(checkOutDate))
            {
                checkIn = DateTime.ParseExact(checkInDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                checkOut = DateTime.ParseExact(checkOutDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            }
            else
            {
                checkIn = DateTime.Today;
                checkOut = DateTime.Today.AddDays(1);
            }

            HttpContext.Session.SetString("CHECKINDATE", checkIn.ToString("yyyy-MM-dd"));
            HttpContext.Session.SetString("CHECKOUTDATE", checkOut.ToString("yyyy-MM-dd"));

            //抓取飯店名稱列表
            CSearchRoomViewModel h = new CSearchRoomViewModel();
            h.hotels = db.HotelIndustry.ToList();

            //讀取與設定飯店ID
            int hotelid = 0;

            if (!string.IsNullOrEmpty(hotelId) && int.TryParse(hotelId, out int parsedHotelId))
            {
                hotelid = int.Parse(hotelId);
            }
            else
            {
                hotelid = 1;
            }

            var hotelname = db.HotelIndustry.Where(h => h.HotelId == hotelid).Select(h => h.HotelName).FirstOrDefault();
            HttpContext.Session.SetString("HOTELNAME", hotelname.ToString());

            //查詢空閒房間方法
            //查詢指定時間區間內已被預訂的房間
            var reservedRooms = from od in db.OrderDetail
                                where !(od.CheckOutDate <= checkIn || od.CheckInDate >= checkOut)
                                select od.RoomId;
            var reservedRoomList = reservedRooms.ToList();

            //所有房間扣掉已預訂房間 , 篩選飯店跟用房型分類
            var freeRooms = db.Room.Include(r => r.RoomClass).AsEnumerable()
                .Where(r => !reservedRoomList.Contains(r.RoomId) && r.HotelId == hotelid)
                .GroupBy(r => r.RoomClass.RoomClassName)
                .Select(g => new { g.Key, g }).ToList();

            List<CSearchRoomViewModel> vmList = new List<CSearchRoomViewModel>();

            foreach (var room in freeRooms)
            {
                var f = room.g.First();

                CSearchRoomViewModel vm = new CSearchRoomViewModel();
                vm.RoomClassId = f.RoomClassId;
                vm.RoomClassPhoto1 = f.RoomClass.RoomClassPhoto1;
                vm.RoomClassDetail = f.RoomClass.RoomClassDetail;
                vm.WeekdayPrice = (int)Math.Floor(f.RoomClass.WeekdayPrice.GetValueOrDefault());
                vm.HolidayPrice = (int)Math.Floor(f.RoomClass.HolidayPrice.GetValueOrDefault());
                vm.AddPrice = (int)Math.Floor(f.RoomClass.AddPrice.GetValueOrDefault());
                vm.RoomClassName = f.RoomClass.RoomClassName;
                vm.RoomClassPeople = f.RoomClass.RoomClassPeople;
                vm.RoomClassSize = f.RoomClass.RoomClassSize;
                vm.CheckInDate = checkIn;
                vm.CheckOutDate = checkOut;
                vm.HotelName = f.Hotel.HotelName;
                vmList.Add(vm);
            }

            //資料轉為json格式
            string json = "";
            json = JsonSerializer.Serialize(vmList);

            if (vmList.Count == 0)
            {
                return Json(null);
            }
            else
            {
                return Json(json);
            }
        }

        //房間加入購物車
        public ActionResult AddShopCart(string RoomClassId)
        {
            HotelOrderContext db = new HotelOrderContext();
            var roomClass = db.RoomClass.Find(RoomClassId);

            string checkInDateStr = HttpContext.Session.GetString("CHECKINDATE");
            string checkOutDateStr = HttpContext.Session.GetString("CHECKOUTDATE");
            string hotelname = HttpContext.Session.GetString("HOTELNAME");

            if (roomClass != null)
            {
               
                DateTime checkInDate;
                DateTime checkOutDate;

                if (DateTime.TryParse(checkInDateStr, out checkInDate) && DateTime.TryParse(checkOutDateStr, out checkOutDate))
                {
                    string json = "";
                    List<CShopCartViewModel> cartList = null;
                    if (HttpContext.Session.Keys.Contains("CartData"))
                    { 
                        json = HttpContext.Session.GetString("CartData");
                        cartList = JsonSerializer.Deserialize<List<CShopCartViewModel>>(json);
                    }
                    else
                    {
                        cartList = new List<CShopCartViewModel>();
                    }

                    int fId = 0;

                    if (cartList.Count > 0)
                    {
                        fId = cartList.Max(item => item.FId) + 1;
                    }

                    CShopCartViewModel cartItem = new CShopCartViewModel();
                    {
                        cartItem.FId = fId;
                        cartItem.RoomClassName = roomClass.RoomClassName;
                        cartItem.RoomClassPhoto1 = roomClass.RoomClassPhoto1;
                        cartItem.RoomClassSize = roomClass.RoomClassSize;
                        cartItem.RoomClassId = roomClass.RoomClassId;
                        cartItem.RoomClassPeople = roomClass.RoomClassPeople;
                        cartItem.HolidayPrice = (int)Math.Floor(roomClass.HolidayPrice.GetValueOrDefault());
                        cartItem.WeekdayPrice = (int)Math.Floor(roomClass.WeekdayPrice.GetValueOrDefault());
                        cartItem.AddPrice = (int)Math.Floor(roomClass.AddPrice.GetValueOrDefault());
                        cartItem.CheckInDate = checkInDate;
                        cartItem.CheckOutDate = checkOutDate;
                        cartItem.HotelName = hotelname;
                        //var hotelID = db.Room.Where(r => r.RoomClassId == RoomClassId).Select(r => r.HotelId).FirstOrDefault();
                        //cartItem.HotelName = db.HotelIndustry.FirstOrDefault(h => h.HotelId == hotelID)?.HotelName;

                    };
                    cartList.Add(cartItem);

                    json = JsonSerializer.Serialize(cartList);
                    HttpContext.Session.SetString("CartData", json);

                    return Json(json);
                }
            }
            return Json(null);
        }

        //網頁更新時顯示購物車session內容
        public ActionResult GetShopCartFromSession()
        {
            string json = HttpContext.Session.GetString("CartData");
            List<CShopCartViewModel> cartList = null;

            if (!string.IsNullOrEmpty(json))
            {
                cartList = JsonSerializer.Deserialize<List<CShopCartViewModel>>(json);
            }

            return Json(cartList);
        }

        //進入訂單明細顯示顯示購物車session內容
        public ActionResult GetShopCartOrderDetailSession()
        {
            string json = HttpContext.Session.GetString("CartData");
            List<CShopCartViewModel> cartList = null;

            if (!string.IsNullOrEmpty(json))
            {
                cartList = JsonSerializer.Deserialize<List<CShopCartViewModel>>(json);
            }

            return Json(cartList);
        }

        //刪除購物車session
        [HttpPost]
        public void DeleteSessionData(int FId)
        {
            string json = HttpContext.Session.GetString("CartData");
            List<CShopCartViewModel> cartList = null;

            if (!string.IsNullOrEmpty(json))
            {
                cartList = JsonSerializer.Deserialize<List<CShopCartViewModel>>(json);

                cartList.RemoveAll(item => item.FId == FId);

                string updatedJson = JsonSerializer.Serialize(cartList);
                HttpContext.Session.SetString("CartData", updatedJson);
            }
        }

        //是否會員登錄判斷
        public IActionResult CheckLoginStatus()
        {
            if (HttpContext.Session.GetString("UserID") != null)
            {
                // 使用者已登入
                return Json(new { loggedIn = true });
            }
            else
            {
                // 使用者未登入
                return Json(new { loggedIn = false });
            }
        }

        //進入訂單明細讀取優惠卷ID
        public IActionResult GetDiscountId( )
        {
            var userId = _contextAccessor.HttpContext.Session.GetString("UserID");
            HotelOrderContext db = new HotelOrderContext();
            //IEnumerable<DiscountDetail> usesid = db.DiscountDetail.Where(x => x.MemberId == userId);

            //var discountid = db.DiscountDetail.Include(x => x.Discount).Where(x => x.MemberId == userId).Select(x => x.DiscountId);
            //var 可以使用的優惠卷ID = db.DiscountDetail.Where(x => x.DiscountUse == 0).Select(x => x.DiscountId).ToList();
            // var discountdiscountid = db.Discount.Where(x=>x.DiscountId == 可以使用的優惠卷ID)

            //var discountid = db.DiscountDetail.Include(x => x.Discount).Where(x => x.MemberId == userId).Where(x => x.DiscountUse == 0).Select(x => x.DiscountId);
            //List<Discount> ids = new List<Discount>();
            //foreach(var item in discountid)
            //{
            //    decimal discount=(decimal)db.Discount.Where(d => d.DiscountId == (int)item).Select(x => x.DiscountDiscount).FirstOrDefault();
            //    string name = db.Discount.Where(d => d.DiscountId == (int)item).Select(x => x.DiscountName).FirstOrDefault();

            //    ids.Add(discount);
            //    ids.Add(name);
            //}

            //return Json(ids);

            var discountIds = db.DiscountDetail
                .Include(x => x.Discount)
                .Where(x => x.MemberId == userId && x.DiscountUse == 0)
                .Select(x => x.Discount)
                .ToList();

            List<Discount> discountList = new List<Discount>();
            foreach (var discount in discountIds)
            {
                Discount disc = new Discount
                {
                    DiscountId = discount.DiscountId,
                    DiscountName = discount.DiscountName,
                    DiscountImage = discount.DiscountImage,
                    DiscountDirections = discount.DiscountDirections,
                    DiscountDiscount = discount.DiscountDiscount,
                    DiscountExist = discount.DiscountExist,
                    
                };

                discountList.Add(disc);
            }

            return Json(discountList);

        }

        //進入訂單明細讀取活動session
        public IActionResult GetActivitySession()
        {
            HotelOrderContext db = new HotelOrderContext();

            //string test = "1";
            //HttpContext.Session.SetString("ActivityId", test);

            string activityId = HttpContext.Session.GetString("ActivityId");

            if (activityId != null)
            {
                var 活動參加人數 = db.Order.Where(o => o.ActivityId == int.Parse(activityId)).Select(o => o.ActivityPeople).ToList();
                int 活動參加總人數 = 活動參加人數.Where(x => x.HasValue).Sum(x => x.Value);

                var activity = db.Activity.Find(int.Parse(activityId));
                //string json = "";
                COrderDetailViewModel activityItem = new COrderDetailViewModel();
                {
                    activityItem.ActivityName = activity.ActivityName;
                    activityItem.ActivityPeople = activity.ActivityPeople;
                    activityItem.ActivityCost=activity.ActivityCost;
                    DateTime? activityTime = activity.ActivityTime.GetValueOrDefault().Date;
                    activityItem.ActivityTime = activityTime;
                    activityItem.活動參加總人數 = 活動參加總人數;

                }

                //json = JsonSerializer.Serialize(activityItem);
                //return Json(json);
                return Json(activityItem);
            }
            else
            {
                return Json(null);
            }
        }

        //訂房明細頁面
        public IActionResult SaveDB(COrderDetailViewModel vm)
        {
            ////活動
            //string activityId = HttpContext.Session.GetString("ActivityId");
            //var activity = db.Activity.Where(a => a.ActivityId == int.Parse(activityId));
            ////活動人數待查

            ////優惠卷會員
            //var userId = _contextAccessor.HttpContext.Session.GetString("UserID");
            //string memberId = db.RoomMember.Where(m => m.MemberId == userId).Select(m => m.MemberId).FirstOrDefault();


            ////取得最大ID
            //string maxOID = db.Order.Max(oid => oid.OrderId);
            //string maxODID = db.OrderDetail.Max(odid => odid.OrderDetailId);


            ////取得ID的數字
            //int numOID = int.Parse(maxOID.Substring(2));
            //string newOrderID = string.Format("OD{0:D6}", numOID + 1);
            //int numODID = int.Parse(maxODID.Substring(3));
            //string newOrderDetailID = string.Format("ODD{0:D6}", numODID + 1);


            //Order o = new Order
            //{
            //    OrderId = newOrderID,
            //    MemberId = memberId,
            //    OrderDate = DateTime.Now,
            //    //OrderTotalPrice = ,  
            //    //CheckInPeople = , //網頁取值
            //    ActivityId = int.Parse(activityId)
            //    //ActivityPeople = , //網頁取值
            //};

            //OrderDetail od = new OrderDetail
            //{
            //    OrderDetailId = newOrderDetailID,
            //    OrderId = newOrderID,
            //    //RoomID =  ,  //房間list
            //    //CheckInDate =  , ///房間list
            //    //CheckOutDate = , //房間list
            //    //PaymentDate = DateTime.Now,
            //    //PaymentID = this.ODPaymentID,
            //    //PaymentPrice = ,  //房間list


            //};

            ////修改
            //DiscountDetail dd = new DiscountDetail()
            //{
            //    DiscountUse = 1,

            //};
            return View();
        }

        public IActionResult Detail()
        {

            return View();
        }

        //未用到
        public IActionResult Create()
        {
            var userId = _contextAccessor.HttpContext.Session.GetString("UserID");
            HotelOrderContext db = new HotelOrderContext();
            IEnumerable<RoomMember> usersid = db.RoomMember.Where(x => x.MemberId == userId);

            return View(usersid);
        }

        //訂單完成畫面
        public IActionResult ShowOrder()
        {
            return View();
        }

        public IActionResult ShowOrderDetail(string id)
        {
            ShowOrderDetailData model = new ShowOrderDetailData();

            using (var dbContext = new HotelOrderContext())
            {
                var query = (from a in dbContext.OrderDetail
                             join b in dbContext.Room
                             on new { a.RoomId } equals new { b.RoomId }
                             join c in dbContext.RoomClass
                             on new { b.RoomClassId } equals new { c.RoomClassId } into d
                             from c in d.DefaultIfEmpty()
                             where a.OrderId == id
                             select new { a.OrderId, a.CheckInDate, a.CheckOutDate, a.OrderDetailRemark, a.OrderDetailStatusId, a.PaymentDate, a.PaymentId, a.PaymentPrice, c.RoomClassPhoto1, c.RoomClassPhoto2, c.RoomClassPhoto3, c.RoomClassName }).FirstOrDefault();

                if (query != null)
                {
                    model.orderID = query.OrderId;
                    model.CheckInDate = query.CheckInDate.Value.ToString("yyyy/MM/dd");
                    model.CheckOutDate = query.CheckOutDate.Value.ToString("yyyy/MM/dd");
                    model.OrderDetailRemark = query.OrderDetailRemark;
                    model.OrderDetailStatusID = query.OrderDetailStatusId;
                    model.PaymentDate = query.PaymentDate.Value.ToString("yyyy/MM/dd");
                    model.PaymentID = query.PaymentId;
                    model.PaymentPrice = (int)(query.PaymentPrice.Value);
                    model.RoomClassPhoto1 = query.RoomClassPhoto1;
                    model.RoomClassPhoto2 = query.RoomClassPhoto2;
                    model.RoomClassPhoto3 = query.RoomClassPhoto3;
                    model.RoomClassName = query.RoomClassName;
                }
            }

            return View(model);
                       

            //HotelOrderContext db = new HotelOrderContext();
            //ShowOrderDetailData model = db.OrderDetail.Select(o => new ShowOrderDetailData
            //{
            //    orderID = o.OrderId,
            //    CheckInDate = o.CheckInDate.Value.ToString("yyyy/MM/dd"),
            //    CheckOutDate = o.CheckOutDate.Value.ToString("yyyy/MM/dd"),
            //    OrderDetailRemark = o.OrderDetailRemark,
            //    OrderDetailStatusID = o.OrderDetailStatusId,
            //    PaymentDate = o.PaymentDate.Value.ToString("yyyy/MM/dd"),
            //    PaymentID = o.PaymentId,
            //    PaymentPrice = (int)o.PaymentPrice.Value,
            //    RoomID = o.RoomId
            //}).Where(o => o.orderID == id).FirstOrDefault();

            //return View(model);
        }
    }
}
