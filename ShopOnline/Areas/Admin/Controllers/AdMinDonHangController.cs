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
    public class AdMinDonHangController : Controller
    {
        private ClothesEntities db = new ClothesEntities();
         [Authorize(Roles = "admin")]
        // GET: Admin/AdMinDonHang
        // GET: AdMinDonHang
        public ActionResult Index()
        {
            return View(db.DonHangs.ToList());
        }

        // GET: AdMinDonHang/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonHang donHang = db.DonHangs.Find(id);
            if (donHang == null)
            {
                return HttpNotFound();
            }
            return View(donHang);
        }


        // GET: AdMinDonHang/Edit/5
        [HttpGet]
        public ActionResult Edit(int? Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonHang model = db.DonHangs.SingleOrDefault(n => n.DonHangID == Id);
            if (model == null)
            {
                return HttpNotFound();
            }
            //lấy danh sách chi tiết
            var lstChiTietDH = db.DonHangChiTiets.Where(n => n.DonHangID == Id);
            ViewBag.ListChiTietDH = lstChiTietDH;

            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(DonHang ddh, DonHangChiTiet ctdh)
        {
            //truy vấn cơ sở dử liệu
            DonHang ddhUpdate = db.DonHangs.SingleOrDefault(n => n.DonHangID == ddh.DonHangID);
            ddhUpdate.DaGiaoHang = ddh.DaGiaoHang;
            ddhUpdate.UserID = ddh.UserID;
            db.SaveChanges();
            //lấy danh sách chi tiết đơn đăt hàng để hiển thị cho ng dùng
            var lstChiTietDH = db.DonHangChiTiets.Where(n => n.DonHangID == ddh.DonHangID);
            ViewBag.ListChiTietDH = lstChiTietDH;
            return RedirectToAction("Index");
        }

        // GET: AdMinDonHang/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var _Order = db.DonHangs.Find(id);
            _Order.DonHangChiTiets.ToList().ForEach(m => db.DonHangChiTiets.Remove(m));
            db.DonHangs.Remove(_Order);
            db.SaveChanges();
            return new EmptyResult();

        }





        //// GET: /SANPHAMS/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    SanPham sanpham = db.SanPhams.Find(id);
        //    if (sanpham == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(sanpham);
        //}

        //// POST: /SANPHAMS/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SanPham sanpham = db.SanPhams.Find(id);
            db.SanPhams.Remove(sanpham);
            db.SaveChanges();
            return RedirectToAction("Index");
        }







        public ActionResult ChuaGiao()
        {
            // lấy danh sách đơn hàng chưa giao
            var lstChuaGiao = db.DonHangs.Where(n => n.DaGiaoHang == false /*&& n.Deleted == false*/).OrderBy(n => n.NgayDatHang);
            return View(lstChuaGiao);
        }
        public ActionResult DaGiao()
        {
            var lstDaGiao = db.DonHangs.Where(n => n.DaGiaoHang == true /*&& n.Deleted == false*/);
            return View(lstDaGiao);
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
