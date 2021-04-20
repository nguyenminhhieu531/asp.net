using MvcBookStore.Models;
using System;
using System.IO;
using System.Linq;
using System.Web;
using PagedList;
using PagedList.Mvc;
using System.Web.Mvc;

namespace MvcBookStore.Controllers
{
    public class AdminController : Controller
    {
        dbShopGiayDataContextDataContext db = new dbShopGiayDataContextDataContext();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Giay(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 7;
            //return View(db.GIAYs.ToList());
            return View(db.GIAYs.ToList().OrderBy(n => n.MaGiay).ToPagedList(pageNumber, pageSize));
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            // Gán các giá trị người dùng nhập liệu cho các biến 
            var tendn = collection["username"];
            var matkhau = collection["password"];
            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Phải nhập tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Phải nhập mật khẩu";
            }
            else
            {
                //Gán giá trị cho đối tượng được tạo mới (ad)        

                ADMIN ad = db.ADMINs.SingleOrDefault(n => n.tenacc == tendn && n.pwd == matkhau);
                if (ad != null)
                {
                    // ViewBag.Thongbao = "Chúc mừng đăng nhập thành công";
                    Session["Taikhoanadmin"] = ad;
                    return RedirectToAction("Index", "Admin");
                }
                else
                    ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng";
            }
            return View();
        }

        [HttpGet]
        public ActionResult ThemmoiGiay()
        {
            //Dua du lieu vao dropdownList
            //Lay ds tu tabke chu de, sắp xep tang dan trheo ten chu de, chon lay gia tri Ma CD, hien thi thi Tenchude
            ViewBag.MaLoai = new SelectList(db.LOAIGIAYs.ToList().OrderBy(n => n.TenLoai), "MaLoai", "TenLoai");
            ViewBag.MaNSX = new SelectList(db.NHASANXUATs.ToList().OrderBy(n => n.TenNSX), "MaNSX", "TenNSX");
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemmoiGiay(GIAY giay, HttpPostedFileBase fileUpload)
        {
            //Dua du lieu vao dropdownload
            ViewBag.MaLoai = new SelectList(db.LOAIGIAYs.ToList().OrderBy(n => n.TenLoai), "MaLoai", "TenLoai");
            ViewBag.MaNSX = new SelectList(db.NHASANXUATs.ToList().OrderBy(n => n.TenNSX), "MaNSX", "TenNSX");
            //Kiem tra duong dan file
            if (fileUpload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";
                return View();
            }
            //Them vao CSDL
            else
            {
                if (ModelState.IsValid)
                {
                    //Luu ten fie, luu y bo sung thu vien using System.IO;
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    //Luu duong dan cua file
                    var path = Path.Combine(Server.MapPath("~/Hinhsanpham"), fileName);
                    //Kiem tra hình anh ton tai chua?
                    if (System.IO.File.Exists(path))
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    else
                    {
                        //Luu hinh anh vao duong dan
                        fileUpload.SaveAs(path);
                    }
                    giay.Anhbia = fileName;
                    //Luu vao CSDL
                    db.GIAYs.InsertOnSubmit(giay);
                    db.SubmitChanges();
                }
                return RedirectToAction("giay");
            }
        }

        //Hiển thị sản phẩm
        public ActionResult Details(int id)
        {
            //Lay ra doi tuong sach theo ma
            GIAY giay = db.GIAYs.SingleOrDefault(n => n.MaGiay == id);
            ViewBag.MaGiay = giay.MaGiay;
            if (giay == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(giay);
        }

        //Xóa sản phẩm
        [HttpGet]
        public ActionResult Xoagiay(int id)
        {
            //Lay ra doi tuong sach can xoa theo ma
            GIAY giay = db.GIAYs.SingleOrDefault(n => n.MaGiay == id);
            ViewBag.MaGiay = giay.MaGiay;
            if (giay == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(giay);
        }

        [HttpPost, ActionName("Xoagiay")]
        public ActionResult Xacnhanxoa(int id)
        {
            //Lay ra doi tuong sach can xoa theo ma
            GIAY giay = db.GIAYs.SingleOrDefault(n => n.MaGiay == id);
            ViewBag.MaGiay = giay.MaGiay;
            if (giay == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.GIAYs.DeleteOnSubmit(giay);
            db.SubmitChanges();
            return RedirectToAction("giay");
        }
        //Chinh sửa sản phẩm
        [HttpGet]
        public ActionResult Suagiay(int id)
        {
            //Lay ra doi tuong sach theo ma
            GIAY giay = db.GIAYs.SingleOrDefault(n => n.MaGiay == id);
            ViewBag.MaGiay = giay.MaGiay;
            if (giay == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //Dua du lieu vao dropdownList
            //Lay ds tu tabke chu de, sắp xep tang dan trheo ten chu de, chon lay gia tri Ma CD, hien thi thi Tenchude
            ViewBag.MaLoai = new SelectList(db.LOAIGIAYs.ToList().OrderBy(n => n.TenLoai), "MaLoai", "TenLoai", giay.MaLoai);
            ViewBag.MaNSX = new SelectList(db.NHASANXUATs.ToList().OrderBy(n => n.TenNSX), "MaNSX", "TenNSX", giay.MaNSX);
            return View(giay);
        }
        public ActionResult SPTheoLoaiGiay(int id)
        {
            var GIAY = from s in db.GIAYs where s.MaLoai == id select s;
            return View(GIAY);
        }
        public ActionResult SPTheoNSX(int id)
        {
            var GIAY = from cd in db.GIAYs where cd.MaNSX == id select cd;
            return View(GIAY);
        }
        public ActionResult DS_ThuongHieu()
        {

            return View(db.NHASANXUATs.ToList());
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Suagiay(GIAY giay, HttpPostedFileBase Upload)
        {
            //Dua du lieu vao dropdownload
            ViewBag.MaLoai = new SelectList(db.LOAIGIAYs.ToList().OrderBy(n => n.TenLoai), "Maloai", "TenLoai");
            ViewBag.MaNSX = new SelectList(db.NHASANXUATs.ToList().OrderBy(n => n.TenNSX), "MaNSX", "TenNSX");
            //Kiem tra duong dan file
            if (Upload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";
                return View();
            }
            //Them vao CSDL
            else
            {
                if (ModelState.IsValid)
                {
                    //Luu ten fie, luu y bo sung thu vien using System.IO;
                    var fileName = Path.GetFileName(Upload.FileName);
                    //Luu duong dan cua file
                    var path = Path.Combine(Server.MapPath("~/Hinhsanpham"), fileName);
                    //Kiem tra hình anh ton tai chua?
                    if (System.IO.File.Exists(path))
                        ViewBag.Thongbao = "File đã tồn tại";
                    else
                    {
                        //Luu hinh anh vao duong dan
                        Upload.SaveAs(path);
                    }
                    giay.Anhbia = fileName;
                    //Luu vao CSDL   
                    UpdateModel(giay);
                    GIAY g1 = new GIAY();
                    g1 = db.GIAYs.SingleOrDefault(m => m.MaGiay == giay.MaGiay);
                    db.GIAYs.DeleteOnSubmit(g1);
                    db.GIAYs.InsertOnSubmit(giay);
                    db.SubmitChanges();

                }
                return RedirectToAction("giay");
            }
        }

    }
}