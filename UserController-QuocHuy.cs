using ShopOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ShopOnline.Controllers
{
    public class UserAccountController : Controller
    {
        ClothesEntities db = new ClothesEntities();
       public ActionResult UserRegister()
        {
            return View();
        }

       //Hàm lưu thông tin đăng ký
       [HttpPost]
       public ActionResult SaveUser([Bind(Include = "Email, UserName, FirstName, LastName, Address,Password")]User UserAccount)
       {
           //Kiểm tra hợp lệ dữ liệu
           if (ModelState.IsValid)
           {
               //kiểm tra tên đăng nhập, email có bị đã tồn tại hay chưa?
               var checkUserName = db.Users.Any(s => s.UserName == UserAccount.UserName);
               var checkEmail = db.Users.Any(s => s.Email == UserAccount.Email);

               //Các lỗi nếu có trong quá trình đăng ký tài khoản
               if (checkUserName)
               {
                   ModelState.AddModelError("", "Tên đăng nhập đã tồn tại!");
               }
               if (checkEmail)
               {
                   ModelState.AddModelError("", "Email đã có người sử dụng!");
               }
               if (checkUserName == true || checkEmail == true)
               {
                   //trả về view đăng ký và thông báo các lỗi ở trên
                   return View("UserRegister",UserAccount);
               }
               else
               {
                   //Lưu thông tin tài khoản vào CSDL
                   db.Users.Add(UserAccount);
                   db.SaveChanges();
                   //xác thực tài khoản trong ứng dụng
                   FormsAuthentication.SetAuthCookie(UserAccount.UserName, false);
                   //trả về trang chủ đăng ký thành công
                   return Redirect("/");

               }
           }
           else
           {
               //Trang báo lỗi nhập liệu không hợp lệ!
               return View("Error");
           }
       }

       //Đăng xuất khỏi ứng dụng
       public ActionResult UserSignOut()
       {
           FormsAuthentication.SignOut();
           //Về trang chủ
           return Redirect("/");
       }

        //Đăng nhập
       public ActionResult UserLogin()
       {
           if(User.Identity.IsAuthenticated)
           {
               //user đã đăng nhập thì chuyển về trang chủ
               return Redirect("/");
           }
           return View();
       }

       [HttpPost]
       public ActionResult CheckLogin(UserLoginViewModel account)
       {
           if (ModelState.IsValid)
           {
               //cấu trúc linq kiểm tra xem có tài khoản nào khớp hay không
               var checkLogin = db.Users.Any(s => s.UserName == account.LoginName && s.Password == account.Password);
               //nếu có checklogin = true
               if (checkLogin)
               {
                   FormsAuthentication.SetAuthCookie(account.LoginName, false);
                   return RedirectToAction("Index", "Home");
                   //Dẫn đến Action QuanLyNguoiDung Controller Admin trong Areas Admin
               }
               else
               {
                   ModelState.AddModelError("", "Sai tên đăng nhập hoặc mật khẩu, kiểm tra lại nhé!");
                   //trở lại trang đăng nhập, báo lỗi
                   return View("UserLogin", account);
               }
           }
           //Đăng nhập thành công, trở về trang chủ
           return Redirect("/");
       }
    }
}