//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ShopOnline.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class DonHangChiTiet
    {
        public int DonHangID { get; set; }
        public int SanPhamSizeID { get; set; }
        public int SoLuong { get; set; }
        public int DonGia { get; set; }
        public int ThanhTien { get; set; }
    
        public virtual DonHang DonHang { get; set; }
        public virtual SanPhamSize SanPhamSize { get; set; }
    }
}
