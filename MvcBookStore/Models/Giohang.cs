using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcBookStore.Models
{
    public class Giohang
    {
        dbShopGiayDataContextDataContext data = new dbShopGiayDataContextDataContext();

        public int iMaGiay { set; get; }

        public string sTenGiay { set; get; }

        public string sAnhbia { set; get; }

        public Double dDonggia { set; get; }

        public int iSoluong { set; get; }

        public Double dThanhtien
        {
            get { return iSoluong * dDonggia; }
        }

        public Giohang(int MaGiay)
        {
            iMaGiay = MaGiay;
            GIAY sach = data.GIAYs.Single(n => n.MaGiay == iMaGiay);
            sTenGiay = sach.TenGiay;
            sAnhbia = sach.Anhbia;
            dDonggia = double.Parse(sach.Giaban.ToString());
            iSoluong = 1;
        }
    }
}