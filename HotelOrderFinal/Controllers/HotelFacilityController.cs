using HotelOrderFinal.Models;
using Microsoft.AspNetCore.Mvc;

namespace HotelOrderFinal.Controllers
{
    public class HotelFacilityController : Controller
    {
        private IWebHostEnvironment _enviro;
        public HotelFacilityController(IWebHostEnvironment p)
        {
            _enviro = p;
        }
        public IActionResult Index()
        {
            HotelOrderContext db = new HotelOrderContext();
            var datas = from c in db.HotelFacility
                        select c;
            return View(datas);
        }

        public IActionResult Edit(int? id)
        {
            HotelOrderContext db = new HotelOrderContext();
            HotelFacility facility = db.HotelFacility.FirstOrDefault(t => t.HotelFacilityId == id);
            if (facility == null)
                return RedirectToAction("Index");
            return View(facility);
        }
        [HttpPost]
        public IActionResult Edit(CHotelFacilityWrap p)
        {
            HotelOrderContext db = new HotelOrderContext();
            HotelFacility facility = db.HotelFacility.FirstOrDefault(t => t.HotelFacilityId == p.HotelFacilityId);
            if (facility != null)
            {
                if (p.photo != null)
                {
                    string photoName = Guid.NewGuid().ToString() + ".png";
                    string path = _enviro.WebRootPath + "/image/facility/" + photoName;
                    p.photo.CopyTo(new FileStream(path, FileMode.Create));
                    facility.HotelFacilityImage = photoName;
                }

                facility.HotelFacilityName = p.HotelFacilityName;

                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
