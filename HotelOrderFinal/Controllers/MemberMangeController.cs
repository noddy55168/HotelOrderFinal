using HotelOrderFinal.Models;
using HotelOrderFinal.ViewModels;
using Microsoft.AspNetCore.Mvc;


namespace HotelOrderFinal.Controllers
{
    public class MemberMangeController : Controller
    {
        public IActionResult List(CAdminKeywordViewModel vm)
        {          
            HotelOrderContext db = new HotelOrderContext();
            IEnumerable<RoomMember> datas = null;
            if (string.IsNullOrEmpty(vm.txtKeyword))
            {
                datas = from c in db.RoomMember
                            select c;
            }
            else
            {
                datas = db.RoomMember.Where(p =>
                p.MemberId.Contains(vm.txtKeyword)||
                p.MemberName.Contains(vm.txtKeyword) ||
                p.MemberPhone.Contains(vm.txtKeyword) ||
                p.MemberEmail.Contains(vm.txtKeyword));
            }
            if (!datas.Any()) // 檢查搜尋結果是否為空
            {
                ViewData["Message"] = "查無任何資料！";
            }
            return View(datas);
        }
        

        //}
        public IActionResult Edit(string? id)
        {
            HotelOrderContext db = new HotelOrderContext();
            RoomMember member = db.RoomMember.FirstOrDefault(t => t.MemberId == id);
            if (member == null)
                return RedirectToAction("List");
            return View(member);
        }
        [HttpPost]
        public IActionResult Edit(RoomMember p)
        {
            HotelOrderContext db = new HotelOrderContext();
            RoomMember member = db.RoomMember.FirstOrDefault(t => t.MemberId == p.MemberId);
            if (member != null)
            {
                member.MemberName = p.MemberName;
                member.MemberBirthday = p.MemberBirthday;
                member.MemberGender = p.MemberGender;
               member.MemberPhone = p.MemberPhone;

                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        public IActionResult Delete(string? id)
        {
            HotelOrderContext db = new HotelOrderContext();
            RoomMember member = db.RoomMember.FirstOrDefault(a => a.MemberId == id);
            if (member != null)
            {
                db.RoomMember.Remove(member);
                db.SaveChanges();
            }
            return RedirectToAction("List");
        }

    }   
}
