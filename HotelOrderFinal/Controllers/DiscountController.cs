using HotelOrderFinal.Models;
using HotelOrderFinal.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace HotelOrderFinal.Controllers
{
    public class DiscountController : Controller
    {
        private HotelOrderContext db = new HotelOrderContext();
        public IHttpContextAccessor _contextAccessor;
        public IWebHostEnvironment _enviro;

        public DiscountController(IHttpContextAccessor contextAccessor , IWebHostEnvironment enviro)
        {
            this._contextAccessor = contextAccessor;
            _enviro = enviro;
            
        }

        public IActionResult List(CKeywordViewModel vm)
        {
            IEnumerable<Discount> datas = null;
            if (string.IsNullOrEmpty(vm.txtKeyword))
                datas = from c in db.Discount
                        select c;
            else
                return RedirectToAction("Index", "Home");
            return View(datas);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Discount p, CDiscountWrap x)
        {
            try
            {
                // 取得優惠是否存在
                p.DiscountExist = true;
                db.Discount.Add(p);
                db.SaveChanges();

                // 取得當時的會員數量
                int memberCount = db.RoomMember.Count();
                Discount cust = db.Discount.FirstOrDefault(t => t.DiscountId == p.DiscountId);

                if (cust != null)
                {
                    if (x.photo != null)
                    {
                        string photoName = Guid.NewGuid().ToString() + ".jpg";
                        string path = _enviro.WebRootPath + "/image/" + photoName;
                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            x.photo.CopyTo(stream);
                        }
                        cust.DiscountImage = photoName;
                    }

                    cust.DiscountName = x.DiscountName;
                    cust.DiscountDirections = x.DiscountDirections;
                    cust.DiscountDiscount = x.DiscountDiscount;
                    cust.DiscountExist = x.DiscountExist;
                    db.SaveChanges();
                }

                if (memberCount > 0)
                {
                    // 所有的會員ID
                    var members = db.RoomMember.Select(x => x.MemberId);

                    int lastID = db.Discount.OrderByDescending(x => x.DiscountId).FirstOrDefault().DiscountId;

                    foreach (var itme in members)
                    {
                        DiscountDetail detail = new DiscountDetail();
                        detail.DiscountId = lastID;
                        detail.DiscountStart = DateTime.Now;
                        detail.DiscountEnd = DateTime.Now.AddMonths(3);
                        detail.DiscountUse = 0;
                        detail.MemberId = itme;
                        db.DiscountDetail.Add(detail);
                    }

                    db.SaveChanges();
                }

                return RedirectToAction("List");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return View();
            }
        }

        //public IActionResult Create(Discount p , CDiscountWrap x)
        //{
        //    try
        //    {
        //        // 取得優惠是否存在
        //        p.DiscountExist = true;
        //        db.Discount.Add(p);
        //        db.SaveChanges();


        //        // 取得當時的會員數量
        //        int memberCount = db.RoomMember.Count();
        //        Discount cust = db.Discount.FirstOrDefault(t => t.DiscountId == p.DiscountId);
        //        if (cust != null)
        //        {
        //            if (x.photo != null)
        //            {
        //                string photoName = Guid.NewGuid().ToString() + ".jpg";
        //                string path = _enviro.WebRootPath + "/image/" + photoName;
        //                x.photo.CopyTo(new FileStream(path, FileMode.Create));
        //                cust.DiscountImage = photoName;
        //            }

        //            cust.DiscountName = x.DiscountName;
        //            cust.DiscountDirections = x.DiscountDirections;
        //            cust.DiscountDiscount = x.DiscountDiscount;
        //            cust.DiscountExist = x.DiscountExist;
        //            db.SaveChanges();
        //        }
        //        if (p.DiscountExist == true && memberCount > 0)
        //        {
        //            //所有的會員ID
        //            var members = db.RoomMember.Select(x => x.MemberId);

        //            int lastID = db.Discount.OrderByDescending(x => x.DiscountId).FirstOrDefault().DiscountId;
        //            foreach (var itme in members)
        //            {
        //                DiscountDetail detail = new DiscountDetail();
        //                detail.DiscountId = lastID;
        //                detail.DiscountStart = DateTime.Now;
        //                detail.DiscountEnd = DateTime.Now.AddMonths(3);
        //                detail.DiscountUse = 0;
        //                detail.MemberId = itme;
        //                db.DiscountDetail.Add(detail);
        //            }
        //            db.SaveChanges();
        //            return RedirectToAction("List");
        //        }                
        //        return RedirectToAction("List");
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.ToString());
        //        return View();
        //    }
        //}

        public IActionResult Delete(int? id)
        {
            
            Discount cust = db.Discount.FirstOrDefault(t => t.DiscountId == id);
            if (cust != null)
            {
                db.Discount.Remove(cust);
                db.SaveChanges();
            }
            return RedirectToAction("List");
        }
        public IActionResult Edit(int? id)
        {           
            Discount cust = db.Discount.FirstOrDefault(t => t.DiscountId == id);
            if (cust == null)
                return RedirectToAction("List");
            return View(cust);
        }
        [HttpPost]
        public IActionResult Edit(CDiscountWrap p)
        {

            Discount cust = db.Discount.FirstOrDefault(t => t.DiscountId == p.DiscountId);
            if (cust != null)
            {
                if (p.photo != null)
                {
                    string photoName = Guid.NewGuid().ToString() + ".jpg";
                    string path = _enviro.WebRootPath + "/image/" + photoName;
                    p.photo.CopyTo(new FileStream(path, FileMode.Create));
                    cust.DiscountImage = photoName;
                }

                cust.DiscountName = p.DiscountName;
                cust.DiscountDirections = p.DiscountDirections;
                cust.DiscountDiscount = p.DiscountDiscount;
                cust.DiscountExist = p.DiscountExist;           
                db.SaveChanges();
            }
            return RedirectToAction("List");
        }
        //public IActionResult Edit(Discount p)
        //{           
        //    Discount cust = db.Discount.FirstOrDefault(t => t.DiscountId == p.DiscountId);
        //    if (cust != null)
        //    {
        //        cust.DiscountName = p.DiscountName;
        //        cust.DiscountImage = p.DiscountImage;
        //        cust.DiscountDirections = p.DiscountDirections;
        //        cust.DiscountDiscount = p.DiscountDiscount;
        //        db.SaveChanges();
        //    }
        //    return RedirectToAction("List");
        //}

        public IActionResult DiscountByMember()
        {
            var userId = _contextAccessor.HttpContext.Session.GetString("UserID");

            //IEnumerable<DiscountDetail> usesid = db.DiscountDetail.Where(x => x.MemberId == userId);
            //var 這個會員的優惠=db.DiscountDetail.Where(d => d.MemberId == userId);
            //List<string> discountNames = new List<string>();
            //foreach(var item in 這個會員的優惠)
            //{
            //    var 優惠類型=db.Discount.FirstOrDefault(d=>d.DiscountId==(int)item.DiscountId);
            //    if (優惠類型 != null)
            //        discountNames.Add(優惠類型.DiscountName);
            //}
            //CTingViewModel vm = new CTingViewModel();
            //vm.names = discountNames;
            //vm.xx = usesid;

            List<CDiscountViewModel> list = new List<CDiscountViewModel>();
            var discountDetails=db.DiscountDetail.Where(x => x.MemberId == userId).ToList();
            foreach(var item in discountDetails)
            {
                CDiscountViewModel vm = new CDiscountViewModel();
                vm.discountDetail = item;
                var 取得優惠資料=db.Discount.FirstOrDefault(x => x.DiscountId == item.DiscountId);
                vm.names = 取得優惠資料.DiscountName;
                list.Add(vm);
            }
            //IEnumerable<Discount> usesid = db.Discount.Where(x => x.MemberId == userId);
            //return View(usesid);
            return View(list);
        }
    }
}
