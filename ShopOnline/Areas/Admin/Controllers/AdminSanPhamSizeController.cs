using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ShopOnline.Models;

namespace ShopOnline.Areas.Admin.Controllers
{
    public class AdminSanPhamSizeController : Controller
    {
        private ClothesEntities db = new ClothesEntities();
         [Authorize(Roles = "admin")]
        // GET: Admin/AdminSanPhamSize
        public ActionResult Index()
        {
            var sanPhamSizes = db.SanPhamSizes.Include(s => s.SanPham).Include(s => s.Size);
            return View(sanPhamSizes.ToList());
        }

        // GET: Admin/AdminSanPhamSize/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPhamSize sanPhamSize = db.SanPhamSizes.Find(id);
            if (sanPhamSize == null)
            {
                return HttpNotFound();
            }
            return View(sanPhamSize);
        }

        // GET: Admin/AdminSanPhamSize/Create
        public ActionResult Create()
        {
            ViewBag.SanPhamID = new SelectList(db.SanPhams, "SanPhamID", "Ma");
            ViewBag.SizeID = new SelectList(db.Sizes, "SizeID", "MaSize");
            return View();
        }

        // POST: Admin/AdminSanPhamSize/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SanPhamSizeID,SanPhamID,SizeID")] SanPhamSize sanPhamSize)
        {
            if (ModelState.IsValid)
            {
                db.SanPhamSizes.Add(sanPhamSize);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.SanPhamID = new SelectList(db.SanPhams, "SanPhamID", "Ma", sanPhamSize.SanPhamID);
            ViewBag.SizeID = new SelectList(db.Sizes, "SizeID", "MaSize", sanPhamSize.SizeID);
            return View(sanPhamSize);
        }

        // GET: Admin/AdminSanPhamSize/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPhamSize sanPhamSize = db.SanPhamSizes.Find(id);
            if (sanPhamSize == null)
            {
                return HttpNotFound();
            }
            ViewBag.SanPhamID = new SelectList(db.SanPhams, "SanPhamID", "Ma", sanPhamSize.SanPhamID);
            ViewBag.SizeID = new SelectList(db.Sizes, "SizeID", "MaSize", sanPhamSize.SizeID);
            return View(sanPhamSize);
        }

        // POST: Admin/AdminSanPhamSize/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SanPhamSizeID,SanPhamID,SizeID")] SanPhamSize sanPhamSize)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sanPhamSize).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SanPhamID = new SelectList(db.SanPhams, "SanPhamID", "Ma", sanPhamSize.SanPhamID);
            ViewBag.SizeID = new SelectList(db.Sizes, "SizeID", "MaSize", sanPhamSize.SizeID);
            return View(sanPhamSize);
        }

        // GET: Admin/AdminSanPhamSize/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPhamSize sanPhamSize = db.SanPhamSizes.Find(id);
            if (sanPhamSize == null)
            {
                return HttpNotFound();
            }
            return View(sanPhamSize);
        }

        // POST: Admin/AdminSanPhamSize/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SanPhamSize sanPhamSize = db.SanPhamSizes.Find(id);
            db.SanPhamSizes.Remove(sanPhamSize);
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
