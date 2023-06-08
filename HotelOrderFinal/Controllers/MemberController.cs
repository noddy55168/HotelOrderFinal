using HotelOrderFinal.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Win32;
using System.Diagnostics.Metrics;
using System.Xml.Linq;
using System.Net.Mail;
using System.Web;
using HotelOrderFinal.ViewModels;
using System.Net.Mail;
using System.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;


namespace HotelOrderFinal.Controllers
{
    public class MemberController : Controller
    {
        private HotelOrderContext db = new HotelOrderContext();
   
        private IWebHostEnvironment _enviro;


            // GET: MemberController
            public IActionResult Index(string id)
        {
            //驗證會員登入是否又錯誤，找不到該會員會回到首頁

            if ( HttpContext.Session.GetString ( "UserID" ) == null )
            {
                return RedirectToAction ( "Index" , "Home" );
            }
            id = HttpContext.Session.GetString ( "UserID" );
            db = new HotelOrderContext ( );
            RoomMember cust = db.RoomMember.FirstOrDefault ( t => t.MemberId == id );
            if ( cust == null )
                return RedirectToAction ( "Index" , "Home" );
            return View ( cust );
        }
        public IActionResult MyAccount ( )
        {
            return PartialView ( "_MyAccount" );
        }
        //【登入】==========================================================================================
        public IActionResult Login ( )
        {
            return View ( );
        }

        [HttpPost]
        public IActionResult Login ( RoomMember model )
        {
            db = new HotelOrderContext ( );
            //HttpContext.Session.SetString("LayoutMessage", "_LayoutMember");
            var member = db.RoomMember.Where(o => o.MemberPhone == model.MemberPhone && o.MemberPassword == model.MemberPassword).FirstOrDefault();
            var returnUrl = HttpContext.Session.GetString("ReturnUrl");
            if (member == null)
            {
                //ViewBag.Message = "帳密錯誤，登入失敗";
                TempData["MemberErrorMessage"] = "帳密錯誤，登入失敗";
               
                if (!string.IsNullOrEmpty(returnUrl))
                {
                    // 清除 Session 中的頁面路徑
                    HttpContext.Session.Remove("ReturnUrl");
                    // 導向先前的頁面路徑
                    return Redirect(returnUrl);
                }
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Message = model.MemberName + "，歡迎光臨";
            TempData ["SuccessMessage"] = model.MemberName + "，歡迎光臨";
            // 將使用者名字存入 ViewBag 或 ViewData 中
            ViewBag.UserName = model.MemberName;
            ViewData ["UserID"] = member.MemberId;

            HttpContext.Session.SetString("UserName", member.MemberName);
            HttpContext.Session.SetString("UserID", member.MemberId);
            HttpContext.Session.SetString("UserPhone", member.MemberPhone);
            HttpContext.Session.SetString("UserPassword", member.MemberPassword);
            HttpContext.Session.SetString("LayoutMessage", "_LayoutMember");
            
            if (!string.IsNullOrEmpty(returnUrl))
            { // 清除 Session 中的頁面路徑
                HttpContext.Session.Remove("ReturnUrl");
                // 導向先前的頁面路徑
                return Redirect(returnUrl);
            }
                return RedirectToAction("Index", "Home");
        }


        public IActionResult terms()
        {
            return View();
        }

        //【忘記密碼】==========================================================================================

        public IActionResult ForgotPassword ( )
        {
            // 處理密碼重製
            return View ( );
        }
        //[HttpPost]  //用ajax方法請求回傳值跟form/submit是無相關的兩條路, 使用時要分清楚
        [Route("Member/find")]
        public IActionResult Find_password(string target) //忘記密碼方法
        { 
            bool isEmailExist = db.RoomMember.Any(x => x.MemberEmail == target);
            if (isEmailExist)
            {
                RoomMember member = db.RoomMember.FirstOrDefault(x => x.MemberEmail == target)!;
                string sVerify = member.MemberId + "|" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                sVerify = HttpUtility.UrlEncode(sVerify);
                int portNumber = HttpContext.Connection.LocalPort;
                string webPath = "https://localhost:" + portNumber + "/";
                string receivePage = "Member/ResetPwd";
                string mailContent = "請點擊以下連結，返回網站重新設定密碼，逾期 5 分鐘後，此連結將會失效。<br><br>";
                mailContent = mailContent + "<a href='" + webPath + receivePage + "?verify=" + sVerify + "'  target='_blank'>點此連結</a>";
                string mailSubject = "[訂房系統] 重設密碼驗證連結";
                string SmtpServer = "smtp.gmail.com";
                string GoogleMailUserID = "imapple1991@gmail.com"; //Google 發信帳號
                string GoogleMailUserPwd = "lqcacnvacukpnzpf"; //應用程式密碼
                //string GoogleMailUserID = _config["GoogleMailUserID"];
                //string GoogleMailUserPwd = _config["GoogleMailUserPwd"];
                int port = 587;
                MailMessage mms = new MailMessage();
                mms.From = new MailAddress(GoogleMailUserID);
                mms.Subject = mailSubject;
                mms.Body = mailContent;
                mms.IsBodyHtml = true;
                mms.SubjectEncoding = Encoding.UTF8;
                mms.To.Add(new MailAddress(target));
                using (SmtpClient client = new SmtpClient(SmtpServer, port))
                {
                    client.EnableSsl = true;
                    client.Credentials = new NetworkCredential(GoogleMailUserID, GoogleMailUserPwd);
                    client.Send(mms);
                }
            }
            return Content(isEmailExist.ToString());
            //return RedirectToAction("Index", "Home");
        }

        public IActionResult ResetPwd(string verify)  //重設密碼頁面
        {
            string script = "<script>alert('驗證碼錯誤或逾期失效');window.close();</script>";

            if (verify == "")
            {
                return Content(script, "text/html", System.Text.Encoding.UTF8);
            }
            string UserID = verify.Split('|')[0];
            string ResetTime = verify.Split('|')[1];
            DateTime dResetTime = Convert.ToDateTime(ResetTime);
            TimeSpan TS = new TimeSpan(DateTime.Now.Ticks - dResetTime.Ticks);
            double diff = Convert.ToDouble(TS.TotalMinutes);
            if (diff > 5)
            {
                return Content(script, "text/html", System.Text.Encoding.UTF8);
            }          
            RoomMember member = db.RoomMember.FirstOrDefault(x => x.MemberId == UserID)!;
            return View(member);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult doResetPwd(RoomMember target)  //修改密碼方法
        {
            if (db.RoomMember.Any(x => x.MemberId == target.MemberId))
            {
                RoomMember member = db.RoomMember.FirstOrDefault(x => x.MemberId == target.MemberId)!;
                member.MemberPassword = target.MemberPassword;
                db.SaveChanges();
                return Json(new
                {
                    success = "true",
                    message = "密碼重新設定成功"
                });
            }
            else
            {
                return Json(new
                {
                    success = "false",
                    message = "密碼重新設定失敗"
                });
            }
        }

        //【新增】==========================================================================================
        // GET: MemberController/Create
        public IActionResult Create ( )
        {
            return PartialView ( );
        }

        // POST: MemberController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create ( RoomMember model )
        {
            if ( !ModelState.IsValid )
            {
                return PartialView ( model );
            }

            string memberID_ = string.Empty;
            int maxMemberID = 0;
            model.AdminId = "AD00010";
            //性別處理
            string gender = model.MemberGender;
            string genderText = ( gender == "male" ) ? "男" : "女";
            //生日處理
            DateTime birthday = model.MemberBirthday.Value;

            try
            {
                db = new HotelOrderContext ( );

                //自動產生MemberID
                var q = db.RoomMember.Select ( x => x.MemberId ).ToList ( );
                int count = q.Count ( );
                maxMemberID = ( count > 0 ) ? Convert.ToInt32 ( q.Max ( ).Substring ( 2 ) ) : 0;
                memberID_ = ( count == 0 ) ? "MB00000" : "MB" + ( maxMemberID + 1 ).ToString ( ).PadLeft ( 5 , '0' );
                model.MemberId = memberID_;
                // 設置性別屬性
                model.MemberGender = genderText;
                model.MemberBirthday = birthday;
                
                //判定帳號是否重複註冊

                var account = db.RoomMember.Where(x=>x.MemberPhone==model.MemberPhone).ToList();
                if (account.Count > 0) 
                {
                    TempData["ErrorMessageSameAccount"] = "帳密錯誤，登入失敗";
                    return RedirectToAction("Index", "Home"); 
                }


                DiscountDetail discountDetail = new DiscountDetail ( );
                discountDetail.MemberId = memberID_;
                discountDetail.DiscountId = 1;
                discountDetail.DiscountStart = DateTime.Now;
                discountDetail.DiscountEnd = DateTime.Now.AddYears ( 1 );
                discountDetail.DiscountUse = 0;

                db.DiscountDetail.Add ( discountDetail );

                db.Add(model);
                db.SaveChanges();

                TempData["SuccessMessage"] = "註冊成功！";
                return RedirectToAction("RegistrationSuccess");
            }
            catch ( Exception ex )
            {
                Console.WriteLine ( ex.ToString ( ) );
                return PartialView ( );
                //return Json(new
                //{
                //    success = false,
                //    message = ex.Message
                //});
            }
        }
        public IActionResult RegistrationSuccess()
        {
            // 從TempData中獲取註冊成功的訊息
            string successMessage = TempData["SuccessMessage"] as string;
            ViewBag.SuccessMessage = successMessage;
            return View();
        }

        //【修改】==========================================================================================
        // GET: MemberController/Edit/5
        public IActionResult Edit ( string id )
        {
            if ( HttpContext.Session.GetString ( "UserID" ) == null )
            {
                return RedirectToAction ( "Index" , "Home" );
            }

            id = HttpContext.Session.GetString ( "UserID" );

            db = new HotelOrderContext ( );
            RoomMember cust = db.RoomMember.FirstOrDefault ( t => t.MemberId == id );
            if ( cust == null )
                return RedirectToAction ( "Index" , "Home" );
            return View ( cust );
        }

        // POST: MemberController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit ( RoomMember model )
        {
            try
            {
                db = new HotelOrderContext ( );
                RoomMember cust = db.RoomMember.FirstOrDefault ( t => t.MemberId == model.MemberId );
                if ( cust != null )
                {
                    cust.MemberName = model.MemberName;
                    cust.MemberEmail = model.MemberEmail;
                    cust.MemberBirthday = model.MemberBirthday;
                    cust.MemberGender = model.MemberGender;
                    db.SaveChanges ( );
                }
                TempData["SuccessMessage"] = "修改成功！";
                return RedirectToAction("EditSuccess", "Member");
            }
            catch
            {
                return View ( );
            }
        }

        public IActionResult EditSuccess()
        {
            // 從TempData中獲取註冊成功的訊息
            string successMessage = TempData["SuccessMessage"] as string;
            ViewBag.SuccessMessage = successMessage;
            return View();
        }

        //【修改密碼】==========================================================================================


        public IActionResult EditPassword ( string UserID )
        {

            if ( HttpContext.Session.GetString ( "UserID" ) == null )
            {
                return RedirectToAction ( "Index" , "Home" );
            }

            UserID = HttpContext.Session.GetString ( "UserID" );

            db = new HotelOrderContext ( );
            RoomMember cust = db.RoomMember.Where ( t => t.MemberId == UserID ).FirstOrDefault ( );

            if ( cust == null )
                return RedirectToAction ( "Index" , "Home" );
            return View ( cust );
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditPassword ( ChangePasswordViewModel change )
        {
            db = new HotelOrderContext();
               RoomMember cust = db.RoomMember.Where(o => o.MemberId == change.UserId).FirstOrDefault();
            if (!ModelState.IsValid)
            {
                return PartialView(change);
            }
            if (cust.MemberPassword != change.OldPassword)
            {
                ModelState.AddModelError("OldPassword", "舊密碼不正確");
                TempData["ErrorMessageOldPassword"] = "舊密碼不正確";
                return View("EditPassword", "model");
            }
            if ( change.NewPassword != change.ReNewPassword )
            {
                ModelState.AddModelError("ReNewPassword", "新密碼兩次輸入不一致");
                TempData["ErrorMessageReNewPassword"] = "新密碼兩次輸入不一致";
                return View("EditPassword", "model");
            }

            cust.MemberPassword = change.NewPassword;             
                db.SaveChanges();
           
            TempData["PasswordSuccessMessage"] = "修改成功！";
            return RedirectToAction("EditPasswordSuccess", "Member");
             // 在ViewBag中设置密码重置成功的消息
             //ViewBag.ResetSuccessMessage = "您的密碼已重設。";
            //    // 重定向到显示成功消息的页面
            //    return RedirectToAction("Index", "Member");

        }

        public IActionResult EditPasswordSuccess()
        {
            // 從TempData中獲取註冊成功的訊息
            string passwordsuccessMessage = TempData["PasswordSuccessMessage"] as string;
            ViewBag.PasswordsuccessMessage = passwordsuccessMessage;
            return View();
        }


        //【登出】==========================================================================================
        public IActionResult Logout ( )
        {
            // 登出

            HttpContext.Session.Clear ( );
            HttpContext.Session.SetString ( "LayoutMessage" , "_Layout" );

            // 重新導向至首頁
            return RedirectToAction ( "Index" , "Home" );
        }

        //【刪除】==========================================================================================
        // GET: MemberController/Delete/5
        public IActionResult Delete ( int id )
        {
            return View ( );
        }

        // POST: MemberController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete ( int id , IFormCollection collection )
        {
            try
            {
                return RedirectToAction ( nameof ( Index ) );
            }
            catch
            {
                return View ( );
            }
        }


        //【歷史訂單】==========================================================================================

        public IActionResult ShoppingCar ( int id )
        {
            //沒有資料庫的時候測試用
            //List<Order> orders = new List<Order>();
            //for (var i = 1; i <= 10; i++)
            //{
            //    Order order = new Order();
            //    order.OrderId = i.ToString();
            //    order.MemberId = "Member_" + i.ToString();
            //    order.OrderDate = DateTime.Now;
            //    order.OrderTotalPrice = 100 + i;
            //    order.CheckInPeople = i + 6;
            //    order.ActivityId = i+10;
            //    order.OrderRemark = string.Empty;
            //    orders.Add(order);
            //}
            db = new HotelOrderContext ( );

            List<Order> orders = db.Order.Select ( o => new Order
            {
                MemberId = o.MemberId ,
                OrderId = o.OrderId ,
                OrderDate = o.OrderDate ,
                OrderTotalPrice = o.OrderTotalPrice ,
                CheckInPeople = o.CheckInPeople ,
                OrderRemark = o.OrderRemark
            } ).Where ( t => t.MemberId == HttpContext.Session.GetString ( "UserID" ) ).ToList ( );

            List<ShowOderData> ShowOrders = orders.Select ( o => new ShowOderData
            {
                MemberId = o.MemberId ,
                OrderId = o.OrderId ,
                OrderDate = o.OrderDate,
                CheckInPeople = o.CheckInPeople ,
                OrderTotalPrice = ( int ) o.OrderTotalPrice ,
                OrderRemark = o.OrderRemark
            } ).ToList ( );

            return View ( ShowOrders );
        }


        public IActionResult MemberTerms ( int id )
        {
            return View ( );
        }

        public IActionResult MemberCard ( int id )
        {
            return View ( );
        }

    }
}
