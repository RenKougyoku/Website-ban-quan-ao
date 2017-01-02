using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


using webmaylanh.Models;

namespace Shoesws.ViewModels
{
    public class CartItem
    {
        //Properties
        public SanPham SanPham { get; set; }
        public int SanPhamID { get; set; }
        public int SoLuong { get; set; }

         //Constructors
        public CartItem(int SoLuong, int SanPhamID)
        {
            //this.SanPham = SanPham;
            this.SanPhamID = SanPhamID;
            this.SoLuong = SoLuong;
        }
    }
}