using Shoesws.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using webmaylanh.Models;

namespace webmaylanh.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        MayLanhDbContext db = new MayLanhDbContext();
        [HttpPost]
        public ActionResult AddToCart(int SanPhamID, int SoLuong = 1)
        {
            SanPham sp = db.SanPhams.SingleOrDefault(p => p.SanPhamID == SanPhamID);
            // Lưu thông tin vào giỏ hàng trong Sesion.
            CartModel cart = Session["Cart"] as CartModel;
            if (cart == null)
            {
                cart = new CartModel();
                Session["Cart"] = cart;
            }
            cart.Add(new CartItem(SanPhamID,  SoLuong));
            // Chuyển sang action Index --> Xem thông tin giỏ hàng.
            return RedirectToAction("Index","Cart");
        }
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
        //// phương thức tính tiền
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

        // GET: Cart
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
        public RedirectToRouteResult UpdateToCart(int SanPhamID, int SoLuong)
        {
            var cart = Session["Cart"] as CartModel;
            cart.Update(SanPhamID, SoLuong);
            return RedirectToAction("Index");

        }
        [HttpPost]
        public RedirectToRouteResult RemoveFromCart(int SanPhamID)
        {
            var cart = Session["Cart"] as CartModel;
            cart.Remove(SanPhamID);
            return RedirectToAction("Index");
        }

        public ActionResult Checkout()
        {
            CartModel cart = Session["Cart"] as CartModel;
            if (cart == null || cart.Items.Count() == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Title = "Checkout";
            ViewBag.Message = "Nhập thông tin đặt hàng!";
            return View();
        }
        [HttpPost]

        public ActionResult Checkout(DonHang donHang, string address, string mobile, string shipName, string email)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //Tham chiếu đến giỏ hàng lưu trong Session
                    var cart = Session["Cart"] as CartModel;
                    //Phát sinh Đơn hàng
                    donHang.NgayDatHang = DateTime.Now;
                    donHang.DaGiao = false;
                    db.DonHangs.Add(donHang);
                    double? total = 0;
                    //Phát sinh Đơn hàng chi tiết
                    foreach (var item in cart.Items)
                    {
                        DonHangCT ct = new DonHangCT();
                        ct.DonHangID = donHang.DonHangID;
                        ct.SoLuong = item.SoLuong;
                        ct.DonGia = item.SanPham.GiaBan;
                        ct.GiamGia = item.SanPham.GiamGia;
                        ct.ThanhTien = item.SoLuong * item.SanPham.GiaBan * (1 - item.SanPham.GiamGia);
                        db.DonHangCTs.Add(ct);

                        total += (item.SanPham.GiaBan.GetValueOrDefault(0) * item.SoLuong * (1 - item.SanPham.GiamGia));
                    }
                    db.SaveChanges();

                    //donHang.DiaChi = address;
                    //donHang.SoDienThoai = mobile;
                    //donHang.TenKhachHang = shipName;
                    //donHang.Email = email;
                    //string content = System.IO.File.ReadAllText(Server.MapPath("~/Content/CssHome/template/neworder.html"));

                    //content = content.Replace("{{TenKhachHang}}", shipName);
                    //content = content.Replace("{{DienThoai}}", mobile);
                    //content = content.Replace("{{Email}}", email);
                    //content = content.Replace("{{DiaChi}}", address);
                    //content = content.Replace("{{ThanhTien}}", String.Format("{0:0,0 VNĐ}", total).Replace(",", "."));
                    //var toEmail = ConfigurationManager.AppSettings["ToEmailAddress"].ToString();
                    //new MailHelper().SendMail(email, "Đơn hàng mới từ Shop Giày AK&B", content);
                    //new MailHelper().SendMail(toEmail, "Đơn hàng mới từ Shop Giày AK&B", content);

                    cart.Clear();

                    ViewBag.Message = "Hoàn tất - Thông tin đặt hàng!";
                    return View("Completed", donHang);
                }
                catch
                {
                    object tb = "Ghi thông tin đặt hàng thất bại";
                    return View("ThongBao", donHang);
                }
            }
            ViewBag.Message = "Nhập thông tin đặt hàng!";
            return View(donHang);
        }
    }
}