﻿using HotelOrderFinal.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace HotelOrderFinal.Controllers
{
    public class ActivityController : Controller
    {
        private HotelOrderContext db = new HotelOrderContext();
        private IWebHostEnvironment _enviro;
        
        
        public ActivityController(IWebHostEnvironment p)
        {
            _enviro = p;
        }
        public ActionResult ActivityByDetails(int? id)
        {
             
            Activity cust = db.Activity.FirstOrDefault(t => t.ActivityId == id);
            if (cust == null)
                return RedirectToAction("List");
            return View(cust);
        }
        [HttpPost]
        public ActionResult setSessionByActivity(int id)
        {          
            var a = db.Activity.FirstOrDefault(t => t.ActivityId == id);
            if (a== null)
            {
                return RedirectToAction("ActivityByDetails");
            }
            HttpContext.Session.SetString("SelectedActivityId", a.ActivityId.ToString());
            HttpContext.Session.SetString("ActivityTime", a.ActivityTime.ToString());
            return RedirectToAction("List", "Order");
        }
        public IActionResult List()
        {            

            
            var datas = from c in db.Activity
                        select c;
            return View(datas);
        }
        public IActionResult Edit(int? id)
        {
            HotelOrderContext db = new HotelOrderContext();
            Activity cust = db.Activity.FirstOrDefault(t => t.ActivityId == id);
            if (cust == null)
                return RedirectToAction("List");
            return View(cust);
        }
        [HttpPost]
        public IActionResult Edit(CActivityWrap p)
        {
            
            Activity cust = db.Activity.FirstOrDefault(t => t.ActivityId == p.ActivityId);
            if (cust != null)
            {
                if (p.photo != null)
                {
                    string photoName = Guid.NewGuid().ToString() + ".jpg";
                    string path = _enviro.WebRootPath + "/image/" + photoName;
                    p.photo.CopyTo(new FileStream(path, FileMode.Create));
                    cust.ActivityImage = photoName;
                }

                cust.ActivityName = p.ActivityName;            
                cust.ActivityDirections = p.ActivityDirections;
                cust.ActivityTime = p.ActivityTime;
                cust.ActivityPlace = p.ActivityPlace;
                cust.ActivityPeople = p.ActivityPeople;
                cust.ActivityCost = p.ActivityCost;
                db.SaveChanges();
            }
            return RedirectToAction("List");
        }

    }
}
