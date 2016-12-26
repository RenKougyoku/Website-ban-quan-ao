using ShopOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopOnline.ViewModels
{
    public class CartItem
    {
        //Properties
        public int SanPhamSizeID { get; set; }
        public SanPham SanPham { get; set; }
        public Size Size { get; set; }
        public int SoLuong { get; set; }
        public double? GiaBan { get; set; }
        public double? TongTien
        {
            get { return SoLuong * GiaBan ; }
        }
       
        // Constructors
        public CartItem(int SanPhamSizeID, SanPham SanPham, Size Size, int SoLuong)
        {
            this.SanPhamSizeID = SanPhamSizeID;
            this.SanPham = SanPham;
            this.Size = Size;
            this.SoLuong = SoLuong;
        }
    }
}