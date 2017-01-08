using ShopOnline.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;

using System.IO;
using System.Data.Entity.Infrastructure;

namespace ShopOnline.Areas.Admin.Controllers
{
    public class AdminSanPhamController : Controller
    {

        private ClothesEntities db = new ClothesEntities();
         [Authorize(Roles = "admin")]
        // GET: AdminSanPham
        public ActionResult Index(int? page)
        {
            //var sanPhams = db.SanPhams.Include(s => s.Loai);
            //return View(sanPhams.ToList());
            int pageSize = 10;
            int pageNum = (page ?? 1);
            IEnumerable<SanPham> sanPhams = db.SanPhams.OrderBy(s => s.LoaiID);
            ViewBag.SanPhamMenuActive = "active";
            return View(sanPhams.ToPagedList(pageNum, pageSize));
        }

        // GET: AdminSanPham/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sanPham = db.SanPhams.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            return View(sanPham);
        }

        #region Tạo sản phẩm mới
        // GET: AdminSanPham/Create
        public ActionResult Create()
        {
            ViewBag.LoaiID = new SelectList(db.Loais, "LoaiID", "Ten");

            //Tạo nguồn dữ liệu cho DropDownBox chọn Trạng thái.
            var dsTrangThai = new[]{
                                     new {TrangThaiID = 1, Ten = "Nổi Bật"},
                                     new {TrangThaiID = 2, Ten = "Bình Thường"},
                                    };

            ViewBag.TrangThai = new SelectList(dsTrangThai, "TrangThaiID", "Ten");
            //Tạo nguồn dữ liệu cho checkBox chọn size. 
            ViewBag.Sizes = db.Sizes.ToList();
            return View();
        }

        // POST: AdminSanPham/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "SanPhamID,Ma,Ten,GiaBan,TrangThai,Mota,LoaiID,NgungBan,NgayCapNhat,BiDanh,NhaSanXuat,SoLuongBan,Hinh")] SanPham sanPham, int[] SizeIDs)
        {
            //Kiểm tra trùng tên.
            int d = db.SanPhams.Count(p => p.Ten == sanPham.Ten.Trim());
            if (d > 0) ModelState.AddModelError("Ten", "Tên sản phẩm không được trùng");
            //Kiểm tra trùng mã.
            int c = db.SanPhams.Count(p => p.Ma == sanPham.Ma.Trim());
            if (c > 0) ModelState.AddModelError("Ma", "Mã sản phẩm không được trùng");

            if (ModelState.IsValid)
            {
                try
                {
                    //1 - Thêm sản phẩm mới vào DbSet
                    sanPham.BiDanh = XuLyDuLieu.LoaiBoDauTiengViet(sanPham.Ten);
                    db.SanPhams.Add(sanPham);
                    //2 - Thêm các sản phẩm size mới vào DbSet
                    if (SizeIDs != null)
                    {
                        for (int i = 0; i < SizeIDs.Length; i++)
                        {
                            var sanPhamSizeMoi = new SanPhamSize();
                            sanPhamSizeMoi.SanPhamID = sanPham.SanPhamID;
                            sanPhamSizeMoi.SizeID = SizeIDs[i];
                            db.SanPhamSizes.Add(sanPhamSizeMoi);
                        }
                    }
                    //Lưu vào Database.
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    object cauBaoLoi = "Ghi không thành công.";
                    return View("ThongBao", cauBaoLoi);
                }
            }
            //Tạo nguồn dữ liệu cho DropDownBox chọn loại.
            ViewBag.LoaiID = new SelectList(db.Loais, "LoaiID", "Ten", sanPham.LoaiID);

            //Tạo nguồn dữ liệu cho DropDownBox chọn Trạng thái.
            var dsTrangThai = new[]{
                                     new {TrangThaiID = 1, Ten = "Nổi Bật"},
                                     new {TrangThaiID = 2, Ten = "Bình Thường"},
                                    };
            ViewBag.TrangThai = new SelectList(dsTrangThai, "TrangThaiID", "Ten", sanPham.TrangThai);
            //Tạo nguồn dữ liệu cho checkBox chọn size.
            ViewBag.Sizes = db.Sizes.ToList();

            //Tạo nguồn dữ liệu cho chọn size

            return View(sanPham);


        }
        #endregion

        // GET: AdminSanPham/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sanPham = db.SanPhams.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            ViewBag.LoaiID = new SelectList(db.Loais, "LoaiID", "Ten", sanPham.LoaiID);
            return View(sanPham);
        }

        // POST: AdminSanPham/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SanPhamID,Ma,Ten,GiaBan,TrangThai,Mota,LoaiID,NhaSanXuat,NgungBan,NgayCapNhat,BiDanh,Hinh")] SanPham sanPham)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sanPham).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LoaiID = new SelectList(db.Loais, "LoaiID", "Ten", sanPham.LoaiID);
            return View(sanPham);
        }

        // GET: AdminSanPham/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sanPham = db.SanPhams.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            return View(sanPham);
        }

        // POST: AdminSanPham/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SanPham sanPham = db.SanPhams.Find(id);
            db.SanPhams.Remove(sanPham);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}