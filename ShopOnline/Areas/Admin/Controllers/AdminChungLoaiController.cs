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
    public class AdminChungLoaiController : Controller
    {
        private ClothesEntities db = new ClothesEntities();
         [Authorize(Roles = "admin")]
        // GET: Admin/AdminChungLoai
        public ActionResult Index()
        {
            return View(db.ChungLoais.ToList());
        }

        // GET: Admin/AdminChungLoai/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChungLoai chungLoai = db.ChungLoais.Find(id);
            if (chungLoai == null)
            {
                return HttpNotFound();
            }
            return View(chungLoai);
        }

        // GET: Admin/AdminChungLoai/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/AdminChungLoai/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ChungLoaiID,Ten,BiDanh")] ChungLoai chungLoai)
        {
            if (ModelState.IsValid)
            {
                db.ChungLoais.Add(chungLoai);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(chungLoai);
        }

        // GET: Admin/AdminChungLoai/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChungLoai chungLoai = db.ChungLoais.Find(id);
            if (chungLoai == null)
            {
                return HttpNotFound();
            }
            return View(chungLoai);
        }

        // POST: Admin/AdminChungLoai/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ChungLoaiID,Ten,BiDanh")] ChungLoai chungLoai)
        {
            if (ModelState.IsValid)
            {
                db.Entry(chungLoai).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(chungLoai);
        }

        // GET: Admin/AdminChungLoai/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChungLoai chungLoai = db.ChungLoais.Find(id);
            if (chungLoai == null)
            {
                return HttpNotFound();
            }
            return View(chungLoai);
        }

        // POST: Admin/AdminChungLoai/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ChungLoai chungLoai = db.ChungLoais.Find(id);
            db.ChungLoais.Remove(chungLoai);
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
