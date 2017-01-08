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
    public class AdminLoaiController : Controller
    {
        private ClothesEntities db = new ClothesEntities();
         [Authorize(Roles = "admin")]
        // GET: Admin/AdminLoai
        public ActionResult Index()
        {
            var loais = db.Loais.Include(l => l.ChungLoai);
            return View(loais.ToList());
        }

        // GET: Admin/AdminLoai/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Loai loai = db.Loais.Find(id);
            if (loai == null)
            {
                return HttpNotFound();
            }
            return View(loai);
        }

        // GET: Admin/AdminLoai/Create
        public ActionResult Create()
        {
            ViewBag.ChungLoaiID = new SelectList(db.ChungLoais, "ChungLoaiID", "Ten");
            return View();
        }

        // POST: Admin/AdminLoai/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LoaiID,Ten,ChungLoaiID,BiDanh")] Loai loai)
        {
            if (ModelState.IsValid)
            {
                db.Loais.Add(loai);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ChungLoaiID = new SelectList(db.ChungLoais, "ChungLoaiID", "Ten", loai.ChungLoaiID);
            return View(loai);
        }

        // GET: Admin/AdminLoai/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Loai loai = db.Loais.Find(id);
            if (loai == null)
            {
                return HttpNotFound();
            }
            ViewBag.ChungLoaiID = new SelectList(db.ChungLoais, "ChungLoaiID", "Ten", loai.ChungLoaiID);
            return View(loai);
        }

        // POST: Admin/AdminLoai/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "LoaiID,Ten,ChungLoaiID,BiDanh")] Loai loai)
        {
            if (ModelState.IsValid)
            {
                db.Entry(loai).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ChungLoaiID = new SelectList(db.ChungLoais, "ChungLoaiID", "Ten", loai.ChungLoaiID);
            return View(loai);
        }

        // GET: Admin/AdminLoai/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Loai loai = db.Loais.Find(id);
            if (loai == null)
            {
                return HttpNotFound();
            }
            return View(loai);
        }

        // POST: Admin/AdminLoai/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Loai loai = db.Loais.Find(id);
            db.Loais.Remove(loai);
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
