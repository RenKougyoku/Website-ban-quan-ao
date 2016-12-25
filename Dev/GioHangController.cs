using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using webmaylanh.Models;

namespace webmaylanh.Controllers
{
    public class GioHangController : Controller
    {
        MayLanhDbContext db = new MayLanhDbContext();
        // GET: GioHang
        public List<ItemGioHang> LayGioHang()
        {
            // giỏ hàng đả tồn tại
            List<ItemGioHang> lstGioHang = Session["GioHang"] as List<ItemGioHang>;
            if (lstGioHang == null)
            {
                // nếu session giỏ hàng đã tồn tại
                lstGioHang = new List<ItemGioHang>();
                Session["GioHang"] = lstGioHang;

            }
            return lstGioHang;
        }
        // thêm giỏ hàng thông thường (Load lại trang)
        public ActionResult ThemGioHang(int SanPhamID, string strUrl)
        {
            // kiểm tra sản phẩm có tồn tại trong csdl ko
            SanPham sanpham = db.SanPhams.SingleOrDefault(n => n.SanPhamID == SanPhamID);
            if (sanpham == null)
            {
                // trang đường dẩn ko hợp lệ
                Response.StatusCode = 404;
                return null;
            }
            // lấy giỏ hàng 
            List<ItemGioHang> lstGioHang = LayGioHang();
            // trường hợp đả tồn tại một sản phẩm trên giỏ hàng
            ItemGioHang sanphamcheck = lstGioHang.SingleOrDefault(n => n.SanPhamID == SanPhamID);
            if (sanphamcheck != null)
            {
                sanphamcheck.SoLuong++;
                sanphamcheck.TongTien = sanphamcheck.SoLuong * sanphamcheck.DonGia * (1 - sanphamcheck.GiamGia);
                return Redirect(strUrl);
            }
            ItemGioHang itemGH = new ItemGioHang(SanPhamID);
            lstGioHang.Add(itemGH);
            db.SaveChanges();
            return Redirect(strUrl);
        }
        // xây dựng phương thức tính số lương
        public double TinhTongSoLuong()
        {
            List<ItemGioHang> lstGioHang = Session["GioHang"] as List<ItemGioHang>;
            if (lstGioHang == null)
            {
                return 0;
            }
            return lstGioHang.Sum(n => n.SoLuong);
        }
        // phương thức tính tiền
        public double? TongTien()
        {
            List<ItemGioHang> lstGioHang = Session["GioHang"] as List<ItemGioHang>;
            if (lstGioHang == null)
            {
                return 0;
            }
            return lstGioHang.Sum(n => n.TongTien);
        }
        // xây dựng giỏ hàng partial
        public ActionResult GioHangPartial()
        {
            if (TinhTongSoLuong() == 0)
            {
                ViewBag.TongSoLuong = 0;
                ViewBag.TongTien = 0;
                return PartialView();
            }
            ViewBag.TongSoLuong = TinhTongSoLuong();
            ViewBag.TongTien = TongTien();
            return PartialView();
        }

        // Xem giỏ hàng
        public ActionResult XemGioHang()
        {
            List<ItemGioHang> lstGioHang = LayGioHang();
            return View(lstGioHang);
        }
        [HttpGet]
        public ActionResult SuaGioHang(int SanPhamID)
        {
            // kiểm tra sản phẩm có tồn tại ko
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            // kiểm tra sản phẩm có tồn tại trong csdl ko
            SanPham sanpham = db.SanPhams.SingleOrDefault(n => n.SanPhamID == SanPhamID);
            if (sanpham == null)
            {
                // trang đường dẩn ko hợp lệ
                Response.StatusCode = 404;
                return null;
            }
            // lấy list giỏ hàng từ session
            List<ItemGioHang> lstGioHang = LayGioHang();
            // kiểm tra sản phẩm có tồn tại trong giỏ hàng chưa
            ItemGioHang sanphamcheck = lstGioHang.SingleOrDefault(n => n.SanPhamID == SanPhamID);
            if (sanphamcheck == null)
            {
                return RedirectToAction("Index", "Home");
            }
            // Lấy list giỏ hàng trên giao diện
            ViewBag.GioHang = lstGioHang;
            // nếu sản phẩm đả tồn tại
            return View(sanphamcheck);
        }
        // cập nhật giỏ hàng
        [HttpPost]
        public ActionResult CapNhatGioHang(ItemGioHang itemGH)
        {
            // kiểm tra số lượng tồn
            SanPham sanphamcheck = db.SanPhams.SingleOrDefault(n => n.SanPhamID == itemGH.SanPhamID);
            // cập nhật số lượng trong session giỏ hàng
            List<ItemGioHang> lstGH = LayGioHang();
            // lấy sản phẩm cập nhật từ  trong list<GioHang>
            ItemGioHang itemGHUpdate = lstGH.Find(n => n.SanPhamID == itemGH.SanPhamID);
            // cập nhật lại số lượng và tổng tiền
            itemGHUpdate.SoLuong = itemGH.SoLuong;
            itemGHUpdate.TongTien = itemGHUpdate.SoLuong * itemGHUpdate.DonGia * (1 - itemGHUpdate.GiamGia);
            return RedirectToAction("XemGioHang");
        }
        public ActionResult XoaGioHang(int SanPhamID)
        {
            // kiểm tra sản phẩm có tồn tại ko
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            // kiểm tra sản phẩm có tồn tại trong csdl ko
            SanPham sanpham = db.SanPhams.SingleOrDefault(n => n.SanPhamID == SanPhamID);
            if (sanpham == null)
            {
                // trang đường dẩn ko hợp lệ
                Response.StatusCode = 404;
                return null;
            }
            // lấy list giỏ hàng từ session
            List<ItemGioHang> lstGioHang = LayGioHang();
            // kiểm tra sản phẩm có tồn tại trong giỏ hàng chưa
            ItemGioHang sanphamcheck = lstGioHang.SingleOrDefault(n => n.SanPhamID == SanPhamID);
            if (sanphamcheck == null)
            {
                return RedirectToAction("Index", "Home");
            }
            // xóa sản phẩm 
            lstGioHang.Remove(sanphamcheck);
            return RedirectToAction("XemGioHang");
        }
        public ActionResult DatHang()
        {
            KhachHang kh = (KhachHang)Session["TaiKhoan"];

            if (kh == null)
            {
                return RedirectToAction("DangNhap", "KhachHang");
            }
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            List<ItemGioHang> lstGioHang = LayGioHang();
            ViewBag.Title = "Đặt Hàng";
            ViewBag.Message = "Nhập thông tin đặt hàng!";
            return View(lstGioHang);
        }
        [HttpPost]
        // xậy dựng action đặt hàng
        public ActionResult DatHang(FormCollection collection, DonHang dh)
        {
            if (ModelState.IsValid)
            {
                //Thêm đơn hàng.
                KhachHang kh = (KhachHang)Session["TaiKhoan"];
                List<ItemGioHang> lstGH = LayGioHang();
                dh.KhachHangID = kh.KhachHangID;
                var ghichu = collection["GhiChu"];
                var ngaygiao = String.Format("{0:MM/dd/yyyy}", collection["NgayGiao"]);
                dh.NgayDatHang = DateTime.Now;
                dh.DaGiao = false;
                db.DonHangs.Add(dh);
                db.SaveChanges();
                try
                {
                    // thêm chi tiết đơn đặt hàng
                    foreach (var item in lstGH)
                    {
                        DonHangCT ct = new DonHangCT();
                        ct.DonHangID = dh.DonHangID;
                        ct.SanPhamID = item.SanPhamID;
                        ct.SoLuong = item.SoLuong;
                        ct.DonGia = item.DonGia;
                        ct.GiamGia = item.GiamGia;
                        ct.ThanhTien = item.SoLuong * item.DonGia * (1 - item.GiamGia);
                        db.DonHangCTs.Add(ct);
                    }
                    db.SaveChanges();
                    ViewBag.Message = "Hoàn tất - Thông tin đặt hàng!";
                    return View("Completed", dh);
                }
                catch
                {
                    object tb = "Ghi thông tin đặt hàng thất bại";
                    return View("ThongBao", dh);
                }
            }
            ViewBag.Message = "Nhập thông tin đặt hàng!";
            return View(dh);
        }
        // thêm giỏ hàng ajax
        public ActionResult ThemGioHangAjax(int SanPhamID, string strUrl)
        {
            // kiểm tra sản phẩm có tồn tại trong csdl ko
            SanPham sanpham = db.SanPhams.SingleOrDefault(n => n.SanPhamID == SanPhamID);
            if (sanpham == null)
            {
                // trang đường dẩn ko hợp lệ
                Response.StatusCode = 404;
                return null;
            }
            // lấy giỏ hàng 

            List<ItemGioHang> lstGioHang = LayGioHang();
            // trường hợp đả tồn tại một sản phẩm trên giỏ hàng
            ItemGioHang sanphamcheck = lstGioHang.SingleOrDefault(n => n.SanPhamID == SanPhamID);
            {
                if (sanphamcheck != null)
                    sanphamcheck.SoLuong++;
                sanphamcheck.TongTien = sanphamcheck.SoLuong * sanphamcheck.DonGia * (1 - sanphamcheck.GiamGia);
                ViewBag.TongSoLuong = TinhTongSoLuong();
                ViewBag.TongTien = TongTien();
                return PartialView("GioHangPartial");
            }

            ItemGioHang itemGH = new ItemGioHang(SanPhamID);
            lstGioHang.Add(itemGH);
            ViewBag.TongSoLuong = TinhTongSoLuong();
            ViewBag.TongTien = TongTien();
            return PartialView("GioHangPartial");
        }
        private List<DonHang> Layspmoi(int count)
        {
            return db.DonHangs.OrderByDescending(a => a.DonHangID).Take(count).ToList();
        }
        public ActionResult thanhtoantructuyen(int? page)
        {

            var sp = Layspmoi(1);
            ViewBag.TongTien = TongTien();
            Session["GioHang"] = null;
            return View(sp);

        } 
    }
}