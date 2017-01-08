using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using System.Xml;
using System.Configuration;

using ShopOnline.Models;
namespace ShopOnline.Controllers
{
    public class HomeController : Controller
    {
        ClothesEntities db = new ClothesEntities();
        
        //
        // GET: /Home/
        public ActionResult Index()
        {
            return View();
        }






        [HttpGet]
        public ActionResult Search(string sTuKhoa, int? page)
        {
            // tìm kiếm theo tên sản phẩm

            // thực hiện phân trang
            // tạo biến số sản phẩm trên trang
            //if (Request.HttpMethod != "GET")
            //{
            //    page = 1;
            //}
            int Pagesize = 12;
            // tạo biến số trang hiện tại
            int PageNumber = (page ?? 1);
            var lstSanPham = db.SanPhams.Where(n => n.Ten.Contains(sTuKhoa));
            ViewBag.TuKhoa = sTuKhoa;
            return View(lstSanPham.OrderBy(n => n.Ten).ToPagedList(PageNumber, Pagesize));

        }
        [HttpPost]
        public ActionResult Search(string sTuKhoa)
        {
            return RedirectToAction("Search", new { @sTuKhoa = sTuKhoa });

        }

        public ActionResult GioiThieu()
        {
            return View();
        }
        public ActionResult TinTucRSS()
        {
            List<RSSItem> rssItemList = new List<RSSItem>();
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(ConfigurationManager.AppSettings["RssUrl"]);
            XmlElement channel = xmlDoc["rss"]["channel"];
            XmlNodeList itemList = channel.GetElementsByTagName("item");
            foreach (XmlNode item in itemList)
            {
                var rssItem = new RSSItem();
                rssItem.Title = item["title"].InnerText;
                rssItem.Description = item["description"].InnerText;
                rssItem.Link = item["link"].InnerText;
                rssItem.PubDate = Convert.ToDateTime(item["pubDate"].InnerText);
                rssItemList.Add(rssItem);
            }
            return PartialView(rssItemList.OrderByDescending(d => d.PubDate).Take(10));
        }
        private List<SanPham> SanPhamMoi(int count)
        {
            return db.SanPhams.OrderByDescending(s => s.NgayCapNhat).Take(count).ToList();
        }
        public ActionResult LaySanPhamMoi()
        {
            var spmoi = SanPhamMoi(8);
            return View(spmoi);
        }
	}
}