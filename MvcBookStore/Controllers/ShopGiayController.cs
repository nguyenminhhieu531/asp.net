using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcBookStore.Models;

using PagedList;
using PagedList.Mvc;
namespace MvcBookStore.Controllers
{
    public class ShopGiayController : Controller
    {
        dbShopGiayDataContextDataContext data = new dbShopGiayDataContextDataContext();
        //dbShopGiayContext data = new dbShopGiayContext();
        // GET: BookStore

        private List<GIAY> LayGIAYmoi(int count)
        {
            return data.GIAYs.OrderByDescending(a => a.Ngaycapnhat).Take(count).ToList();
        }
        public ActionResult Index(int? page)
        {
            int pageSize = 6;
            int pageNum = (page ?? 1);
            var GIAYmoi = LayGIAYmoi(21);
            return View(GIAYmoi.ToPagedList(pageNum, pageSize));
        }
        public ActionResult NHASANXUAT()
        {
            var nsx = from cd in data.NHASANXUATs select cd;
            return PartialView(nsx);
        }
        public ActionResult LoaiGiay()
        {
            var LoaiGiay = from cd in data.LOAIGIAYs select cd;
            return PartialView(LoaiGiay);
        }
        public ActionResult SPTheoLoaiGiay(int? id)
        {
            var GIAY = from s in data.GIAYs where s.MaLoai == id select s;
            return View(GIAY);
        }
        public ActionResult SPTheoNSX(int? id)
        {
            var GIAY = from cd in data.GIAYs where cd.MaNSX == id select cd;
            return View(GIAY);
        }
        public ActionResult Details(int? id)
        {
            var GIAY = from s in data.GIAYs
                       where s.MaGiay == id
                       select s;
            return View(GIAY.Single());
        }
        public ActionResult Lienhe()
        {
 
            return View();
        }
        public ActionResult Tintuc()
        {
            return View();
        }
        public ActionResult Gioithieu()
        {
            return View();
        }
    }
}