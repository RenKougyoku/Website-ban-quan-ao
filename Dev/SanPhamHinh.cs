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
    
    public partial class SanPhamHinh
    {
        public int SanPhamHinhID { get; set; }
        public string TenHinh { get; set; }
        public int SanPhamID { get; set; }
        public int ThuTuHienThi { get; set; }
    
        public virtual SanPham SanPham { get; set; }
    }
}
