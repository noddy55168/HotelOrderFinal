using HotelOrderFinal.Models;
using HotelOrderFinal.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelOrderFinal.Controllers
{
    public class RoomController : Controller
    {
        private IWebHostEnvironment _enviro;
        public RoomController(IWebHostEnvironment p)
        {
            _enviro = p;
        }
        public IActionResult List()
        {
            HotelOrderContext db = new HotelOrderContext();
            var datas = from c in db.RoomClass
                        select c;
            return View(datas);
        }
        public IActionResult doubleRoom()
        {
            HotelOrderContext db = new HotelOrderContext();
            var datas = from c in db.RoomClass
                        where c.RoomClassSize == 13
                        select c;
            return View(datas);
        }
        public IActionResult quadrupleRoom()
        {
            HotelOrderContext db = new HotelOrderContext();
            var datas = from c in db.RoomClass
                        where c.RoomClassSize == 18
                        select c;
            return View(datas);
        }
        public IActionResult familyRoom()
        {
            HotelOrderContext db = new HotelOrderContext();
            var datas = from c in db.RoomClass
                        where c.RoomClassSize == 20
                        select c;
            return View(datas);
        }
        public IActionResult presidentialSuite()
        {
            HotelOrderContext db = new HotelOrderContext();
            var datas = from c in db.RoomClass
                        where c.RoomClassSize == 128
                        select c;
            return View(datas);
        }


        public IActionResult Detail(string id)
        {

            HotelOrderContext db = new HotelOrderContext();
            var room = db.RoomClass.FirstOrDefault(r => r.RoomClassId == id);
            if (room == null)
            {
                return View("List");
            }

            var facilities = db.MultipleRoomFacility
                .Where(mrf => mrf.RoomClassId == id)
                .Select(mrf => mrf.Facility)
                .ToList();
           
                var hotelId = db.Room.FirstOrDefault(h => h.RoomClassId == id)?.HotelId;
            var hotelName = db.HotelIndustry.FirstOrDefault(h => h.HotelId == hotelId)?.HotelName;
            var hotelIds = db.Room.Where(h => h.RoomClassId == id).Select(h => h.HotelId).ToList();
            var hotelIndustries = db.HotelIndustry.Where(h => hotelIds.Contains(h.HotelId)).ToList();


            var viewModel = new CRoomClassViewModel
            {
                RoomClass = room,
                Facility = facilities,
                HotelIndustry = hotelIndustries,
                HotelName = hotelName
            };

            return View(viewModel);
        }

        public IActionResult Edit(string id)
        {
            HotelOrderContext db = new HotelOrderContext();
            RoomClass room = db.RoomClass.FirstOrDefault(t => t.RoomClassId == id);
            if (room == null)
                return RedirectToAction("List");
            return View(room);
        }
        [HttpPost]
        public IActionResult Edit(CRoomClassWrap p)
        {
            HotelOrderContext db = new HotelOrderContext();
            RoomClass room = db.RoomClass.FirstOrDefault(t => t.RoomClassId == p.RoomClassId);
            if (room != null)
            {
                if (p.photo1 != null)
                {
                    string photoName = Guid.NewGuid().ToString() + ".jpg";
                    string path = _enviro.WebRootPath + "/image/room/" + photoName;
                    p.photo1.CopyTo(new FileStream(path, FileMode.Create));
                    room.RoomClassPhoto1 = photoName;
                }
                if (p.photo2 != null)
                {
                    string photoName = Guid.NewGuid().ToString() + ".jpg";
                    string path = _enviro.WebRootPath + "/image/room/" + photoName;
                    p.photo2.CopyTo(new FileStream(path, FileMode.Create));
                    room.RoomClassPhoto2 = photoName;
                }
                if (p.photo3 != null)
                {
                    string photoName = Guid.NewGuid().ToString() + ".jpg";
                    string path = _enviro.WebRootPath + "/image/room/" + photoName;
                    p.photo3.CopyTo(new FileStream(path, FileMode.Create));
                    room.RoomClassPhoto3 = photoName;
                }

                room.RoomClassName = p.RoomClassName;
                room.RoomClassDetail = p.RoomClassDetail;
                room.WeekdayPrice = p.WeekdayPrice;
                room.HolidayPrice = p.HolidayPrice;
                room.AddPrice = p.AddPrice;
                
                db.SaveChanges();
            }
            return RedirectToAction("AdminRoom");
        }

        public IActionResult SearchRooms()
        {
            HotelOrderContext db = new HotelOrderContext();
            var datas = from c in db.RoomClass
                        select c;
            return View(datas);

        }

        public IActionResult AdminRoom()
        {
            HotelOrderContext db = new HotelOrderContext();
            var datas = from c in db.RoomClass
                        select c;
            return View(datas);
        }

    }
}
