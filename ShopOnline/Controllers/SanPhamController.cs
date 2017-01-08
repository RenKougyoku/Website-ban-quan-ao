using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ShopOnline.Models;
using PagedList;
using PagedList.Mvc;
using System.Configuration;
using System.Xml;
namespace ShopOnline.Controllers
{
    public class SanPhamController : Controller
    {
        ClothesEntities db = new ClothesEntities();
        //public ActionResult AllSanPham()
        //{
        //    IEnumerable<SanPham> sanPhams = db.SanPhams.OrderBy(s => s.LoaiID);
        //    return View(sanPhams);
        //}
       
       //  GET: /SanPham/
       private List<SanPham>laysanpham(int count)
       {
           return db.SanPhams.OrderByDescending(a => a.NgayCapNhat).Take(count).ToList();
       }
        public ActionResult AllSanPham(int ? page)
        {
            int pageSize = 12;
            int pageNum = (page ?? 1);
            IEnumerable<SanPham> sanPhams = db.SanPhams.OrderBy(s => s.LoaiID);
            ViewBag.SanPhamMenuActive = "active";
            return View(sanPhams.ToPagedList(pageNum, pageSize));
            
        }


        public ActionResult ChiTietSanPham(int? Id)
        {
            // kiểm tra tham số truyền vào có rổng hay không
            if (Id == null)
            {
                return HttpNotFound();
            }
            var sanpham = db.SanPhams.SingleOrDefault(n => n.SanPhamID == Id);
            
            // kiểm tra id sản phẩm truyền  vào
            if (sanpham == null)
            {
                // thông báo ko tìm thấy
                return HttpNotFound();
            }
            return View(sanpham);
        }
        public ActionResult SanPhamNam( int? page)
        {
           
            //var lstSanPham = db.SanPhams.Where(n => n.Loai.ChungLoaiID==3);
            IEnumerable<SanPham> sanphamnam = db.SanPhams.Where(s => (s.Loai.ChungLoai.ChungLoaiID == 3)).OrderBy(s=>s.Ten);
           
            
            int pageSize = 12;
            int pageNum = (page ?? 1);
            ViewBag.SanPhamMenuActive = "active";
            return View(sanphamnam.ToPagedList(pageNum, pageSize));
            //return View(sanphamnam);

        }
        public ActionResult SanPhamNu(int? page)
        {

            //var lstSanPham = db.SanPhams.Where(n => n.Loai.ChungLoaiID==3);
            IEnumerable<SanPham> sanphamnam = db.SanPhams.Where(s => (s.Loai.ChungLoai.ChungLoaiID == 10)).OrderBy(s => s.Ten);


            int pageSize = 12;
            int pageNum = (page ?? 1);
            ViewBag.SanPhamMenuActive = "active";
            return View(sanphamnam.ToPagedList(pageNum, pageSize));
            //return View(sanphamnam);

        }

        public ActionResult SanPhamTreEm(int? page)
        {

            //var lstSanPham = db.SanPhams.Where(n => n.Loai.ChungLoaiID==3);
            IEnumerable<SanPham> sanphamnam = db.SanPhams.Where(s => (s.Loai.ChungLoai.ChungLoaiID == 15)).OrderBy(s => s.Ten);


            int pageSize = 12;
            int pageNum = (page ?? 1);
            ViewBag.SanPhamMenuActive = "active";
            return View(sanphamnam.ToPagedList(pageNum, pageSize));
            //return View(sanphamnam);

        }

	}
}