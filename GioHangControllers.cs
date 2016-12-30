using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopOnline.Models;
using ShopOnline.ViewModels;

using Microsoft.AspNet.Identity;
using System.Configuration;

namespace ShopOnline.Controllers
{
    public class GioHangController : Controller
    {
        ClothesEntities db = new ClothesEntities();
        [HttpPost]
        public ActionResult AddToCart(int SanPhamSizeID, int SoLuong = 1)
        {
            if (Request.IsAuthenticated == false)
            {
                return RedirectToAction("UserLogin", "UserAccount");
            }

            SanPhamSize spSize = db.SanPhamSizes
                                   .Include("SanPham")
                                   .Include("Size")
                                   .SingleOrDefault(p => p.SanPhamSizeID == SanPhamSizeID);
            // Lưu thông tin vào giỏ hàng trong Sesion.
            CartModel cart = Session["Cart"] as CartModel;
            if (cart == null)
            {
                cart = new CartModel();
                Session["Cart"] = cart;
            }
            cart.Add(new CartItem(SanPhamSizeID, spSize.SanPham, spSize.Size, SoLuong));
            // Chuyển sang action Index --> Xem thông tin giỏ hàng.
            return RedirectToAction("Index");
        }

        //tong so luong sp trong gio hang

        public int TongSoLuong()
        {
            int iTongSoLuong = 0;
            CartModel cart = Session["Cart"] as CartModel;
            if (cart != null)
            {
                iTongSoLuong = cart.Quantity();
            }
            return iTongSoLuong;
        }

        //tong tien
        public double? TongTien()
        {
            double? iTongTien = 0;
            CartModel cart = Session["Cart"] as CartModel;
            if (cart != null)
            {
                iTongTien = cart.Total();
            }
            return iTongTien;
        }

        // xây dựng giỏ hàng partial
        public ActionResult GioHangPartial()
        {
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            return PartialView();
        }

        //Cart

        public ActionResult Index()
        {
            // Lấy thông tin từ giỏ hàng trong Sesion.
            CartModel cart = Session["Cart"] as CartModel;
            if (cart == null || cart.Items.Count == 0)
            {
                // Giỏ hàng trống.
                return RedirectToAction("Index", "Home");
            }
            return View(cart);
        }
        [HttpPost]
        public RedirectToRouteResult UpdateToCart(int SanPhamSizeID, int SoLuong)
        {
            var cart = Session["Cart"] as CartModel;
            cart.Update(SanPhamSizeID, SoLuong);
            return RedirectToAction("Index");

        }
        [HttpPost]
        public RedirectToRouteResult RemoveFromCart(int SanPhamSizeID)
        {
            var cart = Session["Cart"] as CartModel;
            cart.Remove(SanPhamSizeID);
            return RedirectToAction("Index");
        }


        public ActionResult Checkout()
        {
            CartModel cart = Session["Cart"] as CartModel;
            if (cart == null || cart.Items.Count() == 0)
            {
                return RedirectToAction("AllSanPham", "SanPham");
            }
            ViewBag.Title = "Checkout";
            ViewBag.Message = "Nhập thông tin đặt hàng!";
            return View();
        }
        [HttpPost]

        public ActionResult Checkout(DonHang donHang, string shipName, string mobile, string address, string email)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //Tham chiếu đến giỏ hàng lưu trong Session
                    var cart = Session["Cart"] as CartModel;

                    //Phát sinh Đơn hàng
                    donHang.NgayDatHang = DateTime.Now;
                    if (User.Identity.IsAuthenticated)
                    {
                        var userName = User.Identity.GetUserName();
                        var donHangs = db.DonHangs.Where(x => x.UserID == userName).ToList();
                    }
                    else
                    {
                        donHang.UserID = null;
                    }
                    donHang.DaGiaoHang = false;
                    db.DonHangs.Add(donHang);
                    double? total = 0;
                    //Phát sinh Đơn hàng chi tiết
                    foreach (var item in cart.Items)
                    {
                        DonHangChiTiet ct = new DonHangChiTiet();
                        ct.DonHangID = donHang.DonHangID;
                        ct.SanPhamSizeID = item.SanPhamSizeID;
                        ct.SoLuong = item.SoLuong;
                        ct.DonGia = item.SanPham.GiaBan;
                        ct.ThanhTien = item.SoLuong * item.SanPham.GiaBan;
                        db.DonHangChiTiets.Add(ct);
                        total += ct.ThanhTien;
                    }
                   // Session["Cart"] = null;
                    #region

                    donHang.DiaChi = address;
                    donHang.SoDienThoai = mobile;
                    donHang.TenKhachHang = shipName;
                    donHang.Email = email;
                    string content = System.IO.File.ReadAllText(Server.MapPath("~/Content/template/neworder.html"));

                    content = content.Replace("{{TenKhachHang}}", shipName);
                    content = content.Replace("{{DienThoai}}", mobile);
                    content = content.Replace("{{Email}}", email);
                    content = content.Replace("{{DiaChi}}", address);
                    content = content.Replace("{{ThanhTien}}", String.Format("{0:0,0 VNĐ}", total).Replace(",", "."));
                    var toEmail = ConfigurationManager.AppSettings["ToEmailAddress"].ToString();
                    new MailHelper().SendMail(email, "Đơn hàng mới từ Shop quần áo LVQ", content);
                    new MailHelper().SendMail(toEmail, "Đơn hàng mới từ Shop quần áo LVQ", content);
                    #endregion
                    db.SaveChanges();
                    cart.Clear();                  
                    ViewBag.Message = "Hoàn tất - Thông tin đặt hàng!";
                    return View("HoanTatDH", donHang);
                   
                    
                }
                catch
                {
                    //object tb = "Ghi thông tin đặt hàng thất bại";
                    ViewBag.Message1 = "Ghi thông tin đặt hàng thất bại";
                    return View("ThongBao", donHang);
                }
            }
            ViewBag.Message = "Nhập thông tin đặt hàng!";
            return View(donHang);
        }

        public ActionResult HoanTatDH()
        { return View(); }
        public ActionResult ThongBao()
        { return View(); }

    }
}