using HotelOrderFinal.Models;
using HotelOrderFinal.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Globalization;

namespace HotelOrderFinal.Controllers
{
    public class HomeController : Controller
    {
        private HotelOrderContext db = new HotelOrderContext();
        private readonly ILogger<HomeController> _logger;
        
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(string message = null)
        {/*string message = null*/
            //
            CindexActivityViewModels cavm = new CindexActivityViewModels();
            cavm.ActivityImage = db.Activity.Select(x=>x.ActivityImage).ToList();
          

            cavm.hotelIndustry = db.HotelIndustry.ToList();

            cavm.ActivityID = db.Activity.Select(x => x.ActivityId).ToList();
            if (!string.IsNullOrEmpty(message))
            {
                TempData["Message"] = message;
            }


            //ViewBag.Message = message;
            return View(cavm);
        }

        //[HttpPost]
        //public IActionResult setSession(string checkInDate, string checkOutDate)
        //{

        //    DateTime checkIn = DateTime.ParseExact(checkInDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
        //    DateTime checkOut = DateTime.ParseExact(checkOutDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);

        //    if (HttpContext.Session.GetString("CHECKINDATE") == null)
        //        HttpContext.Session.SetString("CHECKINDATE", checkIn.ToString("yyyy-MM-dd"));
        //    else
        //        HttpContext.Session.SetString("CHECKINDATE", checkIn.ToString("yyyy-MM-dd"));

        //    if (HttpContext.Session.GetString("CHECKOUTDATE") == null)
        //        HttpContext.Session.SetString("CHECKOUTDATE", checkOut.ToString("yyyy-MM-dd"));
        //    else
        //        HttpContext.Session.SetString("CHECKOUTDATE", checkOut.ToString("yyyy-MM-dd"));

        //    return RedirectToAction("List", "Order");
        //}

       

        public IActionResult About()
        {
            return View();
        }

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    //return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}
    
}
}