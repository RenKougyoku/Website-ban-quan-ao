using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopOnline.Areas.Admin.Controllers
{
    public class LVQManageController : Controller
    {
        [Authorize(Roles = "admin")]
        // GET: Admin/LVQManage
        public ActionResult Index()
        {
            return View();
        }
    }
}