using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ShopOnline.Models;

namespace ShopOnline.Controllers
{
    public class SANPHAMSController : Controller
    {
        private ClothesEntities db = new ClothesEntities();

        // GET: /SANPHAMS/
        public ActionResult Index()
        {
            var sanphams = db.SanPhams.Include(s => s.Loai);
            return View(sanphams.ToList());
        }

        // GET: /SANPHAMS/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sanpham = db.SanPhams.Find(id);
            if (sanpham == null)
            {
                return HttpNotFound();
            }
            return View(sanpham);
        }

        // GET: /SANPHAMS/Create
        public ActionResult Create()
        {
            ViewBag.LoaiID = new SelectList(db.Loais, "LoaiID", "Ten");
            return View();
        }

        // POST: /SANPHAMS/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="SanPhamID,Ma,Ten,GiaBan,TrangThai,Mota,LoaiID,NhaSanXuat,NgungBan,NgayCapNhat,BiDanh")] SanPham sanpham)
        {
            if (ModelState.IsValid)
            {
                db.SanPhams.Add(sanpham);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.LoaiID = new SelectList(db.Loais, "LoaiID", "Ten", sanpham.LoaiID);
            return View(sanpham);
        }

        // GET: /SANPHAMS/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sanpham = db.SanPhams.Find(id);
            if (sanpham == null)
            {
                return HttpNotFound();
            }
            ViewBag.LoaiID = new SelectList(db.Loais, "LoaiID", "Ten", sanpham.LoaiID);
            return View(sanpham);
        }

        // POST: /SANPHAMS/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="SanPhamID,Ma,Ten,GiaBan,TrangThai,Mota,LoaiID,NhaSanXuat,NgungBan,NgayCapNhat,BiDanh")] SanPham sanpham)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sanpham).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LoaiID = new SelectList(db.Loais, "LoaiID", "Ten", sanpham.LoaiID);
            return View(sanpham);
        }

        // GET: /SANPHAMS/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sanpham = db.SanPhams.Find(id);
            if (sanpham == null)
            {
                return HttpNotFound();
            }
            return View(sanpham);
        }

        // POST: /SANPHAMS/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SanPham sanpham = db.SanPhams.Find(id);
            db.SanPhams.Remove(sanpham);
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
