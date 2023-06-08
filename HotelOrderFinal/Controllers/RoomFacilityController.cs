using HotelOrderFinal.Models;
using Microsoft.AspNetCore.Mvc;

namespace HotelOrderFinal.Controllers
{
    public class RoomFacilityController : Controller
    {
        private IWebHostEnvironment _enviro;
        public RoomFacilityController(IWebHostEnvironment p)
        {
            _enviro = p;
        }
        public IActionResult Index()
        {
            HotelOrderContext db = new HotelOrderContext();
            var datas = from c in db.RoomFacility
                        select c;
            return View(datas);            
        }

        public IActionResult Edit(int? id)
        {
            HotelOrderContext db = new HotelOrderContext();
            RoomFacility facility = db.RoomFacility.FirstOrDefault(t => t.FacilityId == id);
            if (facility == null)
                return RedirectToAction("Index");
            return View(facility);
        }
        [HttpPost]
        public IActionResult Edit(CFacilityWrap p)
        {
            HotelOrderContext db = new HotelOrderContext();
            RoomFacility facility = db.RoomFacility.FirstOrDefault(t => t.FacilityId == p.facilityId);
            if (facility != null)
            {
                if (p.photo != null)
                {
                    string photoName = Guid.NewGuid().ToString() + ".png";
                    string path = _enviro.WebRootPath + "/image/facility/" + photoName;                    
                    p.photo.CopyTo(new FileStream(path, FileMode.Create));
                    facility.FacilityImage = photoName;
                }

                facility.FacilityName = p.FacilityName;
              
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
