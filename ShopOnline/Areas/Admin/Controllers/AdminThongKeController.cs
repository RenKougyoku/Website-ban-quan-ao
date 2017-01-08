using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShopOnline.Models;

namespace ShopOnline.Areas.Admin.Controllers
{
    public class AdminThongKeController : Controller
    {
         
        private ClothesEntities db = new ClothesEntities();
        // GET: AdminThongKe
        public ActionResult Index()
        {
            
            ViewBag.ThongKeDoanhThu = ThongKeDoanhThu();// thống kê doanh thu
            ViewBag.ThongKeDonHang = ThongKeDonHang(); // thống kê đơn hàng
            ViewBag.ThongKeSanPham = ThongKeSanPham(); // thống kê số lượng sản phẩm tồn
            
            return View();
        }
         public decimal ThongKeDoanhThu()
        {
            int sdh = db.DonHangs.Count();
            decimal TongDoanhThu = Convert.ToDecimal(db.DonHangChiTiets.Sum(n => n.DonGia * sdh)); 
            //decimal TongDoanhThu = Convert.ToDecimal(db.DonHangChiTiets.Sum(n => n.DonGia * n.SoLuong)); /* (1 - n.GiamGia)).Value)*/
            return TongDoanhThu; // thống kê tổng doanh thu
        }
        public double ThongKeDonHang()
        {
            // đếm đơn đặt hàng
            double slddh = db.DonHangs.Count();
            return slddh;

            int ddh = -1; 
            var lstDDH = db.DonHangs;
            if (lstDDH.Count() > -1)
            {
                ddh = lstDDH.Count();
            }
            return ddh;
        }
       
        public int ThongKeSanPham()
        {
            // đếm số lượng sản phẩm
            int sanpham = db.SanPhams.Sum(n=>n.SoLuongBan).Value;
            return sanpham;
        }


      


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (db != null)
                    db.Dispose();
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}
	
