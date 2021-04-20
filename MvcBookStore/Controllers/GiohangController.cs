using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcBookStore.Models;

namespace MvcBookStore.Controllers
{
    public class GiohangController : Controller
    {
        dbShopGiayDataContextDataContext data = new dbShopGiayDataContextDataContext();
        // GET: Giohang
       
        public List<Giohang> Laygiohang()
        {
            List<Giohang> lstGiohang = Session["Giohang"] as List<Giohang>;
            if (lstGiohang==null)
            {

                lstGiohang = new List<Giohang>();
                Session["Giohang"] = lstGiohang;
            }
            return lstGiohang;
        }

        public ActionResult ThemGiohang(int iMaGiay, string strURL)
        {
            List<Giohang> lstGiohang = Laygiohang();

            Giohang sanpham = lstGiohang.Find(n => n.iMaGiay == iMaGiay);
                if (sanpham==null)
                {
                    sanpham = new Giohang(iMaGiay);
                    lstGiohang.Add(sanpham);
                    return Redirect(strURL);
                }
                else
                {
                    sanpham.iSoluong++;
                    return Redirect(strURL);
                }
        }

        private int TongSoLuong()
        {
            int iTongSoLuong = 0;
            List<Giohang> lstGiohang = Session["GioHang"] as List<Giohang>;
            if(lstGiohang!=null)
            {
                iTongSoLuong = lstGiohang.Sum(n => n.iSoluong);
            }
            return iTongSoLuong;
        }

        private double TongTien()
        {
            double iTongTien = 0;
            List<Giohang> lstGiohang = Session["GioHang"] as List<Giohang>;
            if (lstGiohang != null)
            {
                iTongTien = lstGiohang.Sum(n => n.dThanhtien);
            }
            return iTongTien;
        }

        public ActionResult GioHang()
        {        
            List<Giohang> lstGiohang = Laygiohang();
            if (lstGiohang.Count == 0)
            {
                return RedirectToAction("Index", "ShopGiay");
            }
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            return View(lstGiohang);
        }

        public ActionResult GiohangPartial()
        {
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            return PartialView();
        }
        //xoa gio hang
        public ActionResult XoaGiohang(int iMaSP)
        {
            //lay gio hang tu session
            List<Giohang> lstGiohang = Laygiohang();
            //kiem tra sach da co trong session["gio hang"]
            Giohang sanpham = lstGiohang.SingleOrDefault(n => n.iMaGiay == iMaSP);
            // neu ton tai thi cho sua so luong
            if (sanpham != null)
            {
                lstGiohang.RemoveAll(n => n.iMaGiay == iMaSP);
                return RedirectToAction("GioHang");
            }
            if (lstGiohang.Count == 0)
            {
                return RedirectToAction("Index", "ShopGiay");
            }
            return RedirectToAction("GioHang");
        }
        // cap nhat gio hang
        public ActionResult CapnhatGiohang (int iMaSP, FormCollection f)
        {
            //lay gio hang tu session
            List<Giohang> lstGiohang = Laygiohang();
            //kiem tra sach da co trong session["gio hang"]
            Giohang sanpham = lstGiohang.SingleOrDefault(n => n.iMaGiay == iMaSP);
            //neu ton tai thi cho sua so luong
            if (sanpham != null)
            {
                sanpham.iSoluong = int.Parse(f["txtSoluong"].ToString());
            }
            return RedirectToAction("Giohang");
        }
        public ActionResult XoaTatcaGiohang()
        {
            //lay gio hang tu session
            List<Giohang> lstGiohang = Laygiohang();
            lstGiohang.Clear();
            return RedirectToAction("Index", "ShopGiay");
        }
        // hien thi view dathang de cap nhat cac thong tin cho don hang
        [HttpGet]
        public ActionResult Dathang()
        {
            //kiem tra dang nhap
            if (Session["Taikhoan"] == null || Session["Taikhoan"].ToString() == "")
            {
                return RedirectToAction("Dangnhap", "Nguoidung");
            }
            if(Session["Giohang"] == null)
            {
                return RedirectToAction("Index", "ShopGiay");
            }

            // lay gio hang tu session
            List<Giohang> lstGiohang = Laygiohang();
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            return View(lstGiohang);
        }
       
        public ActionResult Dathang(FormCollection collection)
        {
            //them don hang
            DONDATHANG ddh = new DONDATHANG();
            KHACHHANG kh = (KHACHHANG)Session["Taikhoan"];
            List<Giohang> gh = Laygiohang();
            ddh.MaKH = kh.MaKH;
            ddh.Ngaydat = DateTime.Now;
            var ngaygiao = String.Format("{0:MM/dd/yyyy}", collection["Ngaygiao"]);
            ddh.Ngaygiao = DateTime.Parse(ngaygiao);
            ddh.Tinhtranggiaohang = false;
            ddh.Dathanhtoan = false;
            data.DONDATHANGs.InsertOnSubmit(ddh);
            data.SubmitChanges();
            // them chi tiet don hang
            foreach (var item in gh)
            {
                CHITIETDONTHANG ctdh = new CHITIETDONTHANG();
                ctdh.MaDonHang = ddh.MaDonHang;
                ctdh.MaGiay = item.iMaGiay;
                ctdh.Soluong = item.iSoluong;
                ctdh.Dongia = (decimal)item.dDonggia;
                data.CHITIETDONTHANGs.InsertOnSubmit(ctdh);
            }
            data.SubmitChanges();
            Session["Giohang"] = null;
            return RedirectToAction("Xacnhandonhang", "Giohang");

        }
        public ActionResult Xacnhandonhang()
        {
            return View();
        }
        
    }
}