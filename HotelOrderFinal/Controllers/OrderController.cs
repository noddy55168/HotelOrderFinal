﻿using HotelOrderFinal.Models;
using HotelOrderFinal.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NuGet.Frameworks;
using NuGet.Protocol;
using System.Globalization;
using System.Linq;

namespace HotelOrderFinal.Controllers
{
    public class OrderController : Controller
    {

        

        List<CShopCartViewModel> _cart = new List<CShopCartViewModel>();

        public IActionResult List()
        {
            //讀取與設定入住退房日
            string checkInDateStr = HttpContext.Session.GetString("CHECKINDATE");
            string checkOutDateStr = HttpContext.Session.GetString("CHECKOUTDATE");

            DateTime checkIn;
            DateTime checkOut;

            if (!string.IsNullOrEmpty(checkInDateStr) && !string.IsNullOrEmpty(checkOutDateStr))
            {
                checkIn = DateTime.ParseExact(checkInDateStr, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                checkOut = DateTime.ParseExact(checkOutDateStr, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            }
            else
            {
                checkIn = DateTime.Today;
                checkOut = DateTime.Today.AddDays(1);
                HttpContext.Session.SetString("CHECKINDATE", checkIn.ToString("yyyy-MM-dd"));
                HttpContext.Session.SetString("CHECKOUTDATE", checkOut.ToString("yyyy-MM-dd"));
            }

            ViewBag.CheckInDate = checkIn;
            ViewBag.CheckOutDate = checkOut;

            HotelOrderContext db = new HotelOrderContext();

            //查詢空閒房間方法
            //查詢指定時間區間內已被預訂的房間
            var reservedRooms = from od in db.OrderDetail
                                where !(od.CheckOutDate <= checkIn || od.CheckInDate >= checkOut)
                                select od.RoomId;
            var reservedRoomList = reservedRooms.ToList();

            //所有房間扣掉已預訂房間

            //var freeRooms = (from r in db.Room.Include(r => r.RoomClass)
            //                 where !reservedRooms.Contains(r.RoomId)
            //                 select r).Distinct();

            //var freeRooms = (from r in db.Room.Include(r => r.RoomClass)
            //                 where !reservedRooms.Contains(r.RoomId)
            //                 select r)
            //                 .Select(rs => new {rs.RoomClass.RoomClassId, rs.RoomClass.RoomClassName }).Distinct();

            //var freeRooms = from r in db.Room.Include(r => r.RoomClass).AsEnumerable()
            //                where !reservedRooms.Contains(r.RoomId)
            //                group r by r.RoomClass.RoomClassName into g
            //                select new { g.Key, g };

            var freeRooms = db.Room.Include(r => r.RoomClass).AsEnumerable()
                .Where(r => !reservedRoomList.Contains(r.RoomId))
                .GroupBy(r => r.RoomClass.RoomClassName)
                .Select(g => new { g.Key, g }).ToList();

            List<CSearchRoomViewModel> vmList = new List<CSearchRoomViewModel>();

            foreach (var room in freeRooms)
            {
                var f = room.g.First();

                CSearchRoomViewModel vm = new CSearchRoomViewModel();
                vm.RoomClassId = f.RoomClass.RoomClassId;
                vm.RoomClassPhoto1 = f.RoomClass.RoomClassPhoto1;
                vm.RoomClassDetail = f.RoomClass.RoomClassDetail;
                vm.WeekdayPrice = f.RoomClass.WeekdayPrice;
                vm.HolidayPrice = f.RoomClass.HolidayPrice;
                vm.AddPrice = f.RoomClass.AddPrice;
                vm.RoomClassName = f.RoomClass.RoomClassName;
                vm.RoomClassPeople = f.RoomClass.RoomClassPeople;
                vm.RoomClassSize = f.RoomClass.RoomClassSize;
                vmList.Add(vm);
            }

            if (freeRooms == null)
            {
                return View();
            }
            else
            {
                return View(vmList);
            }
        }

        

        [HttpPost]
        public IActionResult SearchRoom(string checkInDate, string checkOutDate)
        {

            DateTime checkIn = DateTime.ParseExact(checkInDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            DateTime checkOut = DateTime.ParseExact(checkOutDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);

            HttpContext.Session.SetString("CHECKINDATE", checkIn.ToString("yyyy-MM-dd"));
            HttpContext.Session.SetString("CHECKOUTDATE", checkOut.ToString("yyyy-MM-dd"));

            HotelOrderContext db = new HotelOrderContext();

            //查詢空閒房間方法
            //查詢指定時間區間內已被預訂的房間
            var reservedRooms = from od in db.OrderDetail
                                where !(od.CheckOutDate <= checkIn || od.CheckInDate >= checkOut)
                                select od.RoomId;
            var reservedRoomList = reservedRooms.ToList();

            //所有房間扣掉已預訂房間

            var freeRooms = db.Room.Include(r => r.RoomClass).AsEnumerable()
                .Where(r => !reservedRoomList.Contains(r.RoomId))
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
                vm.WeekdayPrice = f.RoomClass.WeekdayPrice;
                vm.HolidayPrice = f.RoomClass.HolidayPrice;
                vm.AddPrice = f.RoomClass.AddPrice;
                vm.RoomClassName = f.RoomClass.RoomClassName;
                vm.RoomClassPeople = f.RoomClass.RoomClassPeople;
                vm.RoomClassSize = f.RoomClass.RoomClassSize;
                vmList.Add(vm);
            }

            if (freeRooms != null)
            {
                return View();
            }
            else
            {
                return View(vmList);
            }

        }



        public ActionResult AddShopCart(string RoomClassId)
        {
            HotelOrderContext db = new HotelOrderContext();
            var roomClass = db.RoomClass.Find(RoomClassId);

            string checkInDateStr = HttpContext.Session.GetString("CHECKINDATE");
            string checkOutDateStr = HttpContext.Session.GetString("CHECKOUTDATE");


            string json = "";

            if (roomClass != null)
            {
                DateTime checkInDate;
                DateTime checkOutDate;
                if (DateTime.TryParse(checkInDateStr, out checkInDate) && DateTime.TryParse(checkOutDateStr, out checkOutDate))
                {
                    CShopCartViewModel cartItem = new CShopCartViewModel();
                    {
                        cartItem.RoomClassName = roomClass.RoomClassName;
                        cartItem.RoomClassPhoto1 = roomClass.RoomClassPhoto1;
                        cartItem.RoomClassSize = roomClass.RoomClassSize;
                        cartItem.RoomClassId = roomClass.RoomClassId;
                        cartItem.RoomClassPeople = roomClass.RoomClassPeople;
                        cartItem.HolidayPrice = roomClass.HolidayPrice;
                        cartItem.WeekdayPrice = roomClass.WeekdayPrice;
                        cartItem.AddPrice = roomClass.AddPrice;
                        cartItem.CheckInDate = checkInDate;
                        cartItem.CheckOutDate = checkOutDate;
                    };
                    _cart.Add(cartItem);
                }
                }
                json = JsonConvert.SerializeObject(_cart);

            return Json(json);
        }



        public IActionResult Detail()
        {
            if (HttpContext.Session.GetString("UserID") == null)
            {
                return RedirectToAction("Login", "Member");
            }
            return View();
        }

        public IActionResult Create()
        {

            return View();
        }

        public IActionResult ShowOrder()
        {
            return View();
        }
    }
}
