using HotelOrderFinal.Models;
using Microsoft.AspNetCore.Mvc;

namespace HotelOrderFinal.Controllers
{
    public class AdminController : Controller
    {
        private HotelOrderContext db;
        private IWebHostEnvironment _enviro;
        public IActionResult List ( string id )
        {
            if ( HttpContext.Session.GetString ( "AdminId" ) == null )
            {
                return RedirectToAction ( "Index" , "Home" );
            }
            id = HttpContext.Session.GetString ( "AdminId" );
            db = new HotelOrderContext ( );
            Admin cust = db.Admin.FirstOrDefault ( t => t.AdminId == id );
            if ( cust == null )
                return RedirectToAction ( "Index" , "Home" );
            //cust的話因為model傳入型別不可使用IEnumerable
            var data = from d in db.Admin
                       select d;
            return View ( data );
        }
        public IActionResult Login ( )
        {
            return View ( );
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login ( Admin model )
        {
            db = new HotelOrderContext ( );
            //HttpContext.Session.SetString("LayoutMessage", "_LayoutMember");
            var admin = db.Admin.Where ( o => o.AdminAccount == model.AdminAccount && o.AdminPassword == model.AdminPassword ).FirstOrDefault ( );
            if ( admin == null )
            {
                //ViewBag.Message = "帳密錯誤，登入失敗";
                TempData ["AdminErrorMessage"] = "管理者帳密錯誤，登入失敗";
                return RedirectToAction ( "Index" , "Home" );
            }
            ViewBag.Message = model.AdminId + "，登入成功";
            TempData ["SuccessMessage"] = model.AdminId + "，登入成功";
            // 將使用者名字存入 ViewBag 或 ViewData 中
            //ViewBag.UserName = model.AdminId;
            ViewData ["AdminId"] = admin.AdminId;

            HttpContext.Session.SetString ( "AdminId" , admin.AdminId );
            HttpContext.Session.SetString ( "AdminPassword" , admin.AdminPassword );
            HttpContext.Session.SetString ( "LayoutMessage" , "_LayoutAdmin" );

            return RedirectToAction ( "Index" , "Home" );
        }
    }
}
