using HotelOrderFinal.Models;
using HotelOrderFinal.ViewModels;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Plugins;
using System.Text.Json;

namespace HotelOrderFinal.Controllers
{
    public class HotelIndustryController : Controller
    {
        IWebHostEnvironment _enviro;
        public HotelIndustryController(IWebHostEnvironment p)
        {
            _enviro = p;
        }

        public IActionResult ListView()
        {
            HotelOrderContext db = new HotelOrderContext();
            var datas = from c in db.HotelIndustry
                        select c;
            return View(datas);
        }

        public IActionResult List()
        {
            return View();
        }

        public IActionResult Edit(int? id)
        {
            HotelOrderContext db = new HotelOrderContext();
            HotelIndustry Hl = db.HotelIndustry.FirstOrDefault(t => t.HotelId == id);
            if (Hl == null)
                return RedirectToAction("ListView");
            return View(Hl);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CHotelIndustryWrap p)
        {
            HotelOrderContext db = new HotelOrderContext();
            HotelIndustry cust = db.HotelIndustry.FirstOrDefault(t => t.HotelId == p.HotelId);
            if (cust != null)
            {
                if (p.photo != null)
                {
                    string photoName = Guid.NewGuid().ToString() + ".jpg";
                    string path = _enviro.WebRootPath + "/image/" + photoName;
                    p.photo.CopyTo(new FileStream(path, FileMode.Create));
                    cust.HotelImage = photoName;
                }

                cust.HotelName = p.HotelName;
                cust.HotelPhone = p.HotelPhone;
                cust.HotelRegionId = p.HotelRegionId;
                cust.HotelAddress = p.HotelAddress;
                cust.HotelImageDiscription = p.HotelImageDiscription;

                db.SaveChanges();
            }

            return RedirectToAction("ListView");
        }
        public IActionResult Create() //創建資料
        {
            return View();
        }
        [HttpPost]

        public IActionResult Create(HotelIndustry p, CHotelIndustryWrap x)
        {
            HotelOrderContext db = new HotelOrderContext();
            //先創建一個空資料好讓圖片跟內容做導向
            db.HotelIndustry.Add(p);
            db.SaveChanges();
            //
            HotelIndustry cust = db.HotelIndustry.FirstOrDefault(t => t.HotelId == p.HotelId);
            if (x.photo != null)
            {
                string photoName = Guid.NewGuid().ToString() + ".jpg";
                string path = _enviro.WebRootPath + "/image/" + photoName;
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    x.photo.CopyTo(stream);
                }
                cust.HotelImage = photoName;
            }

            cust.HotelName = x.HotelName;
            cust.HotelPhone = x.HotelPhone;
            cust.HotelAddress = x.HotelAddress;
            cust.HotelRegionId = x.HotelRegionId;
            cust.Lat = x.Lat;
            cust.Lng= x.Lng;
            cust.HotelImageDiscription= x.HotelImageDiscription;
            db.SaveChanges();
            return RedirectToAction("ListView");
        }

        //public IActionResult Create(HotelIndustry p)
        //{
        //    HotelOrderContext db = new HotelOrderContext();
        //    db.HotelIndustry.Add(p);
        //    db.SaveChanges();
        //    return RedirectToAction("List");
        //}


        public IActionResult Delete(int? id)
        {
            HotelOrderContext db = new HotelOrderContext();
            HotelIndustry cust = db.HotelIndustry.FirstOrDefault(t => t.HotelId == id);
            if (id != null)
            {
                db.HotelIndustry.Remove(cust);
                db.SaveChanges();
            }
            return RedirectToAction("ListView");
        }
    }
}
