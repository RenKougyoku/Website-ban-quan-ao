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
    
    public partial class DonHang
    {
        public DonHang()
        {
            this.DonHangChiTiets = new HashSet<DonHangChiTiet>();
        }
    
        public int DonHangID { get; set; }
        public System.DateTime NgayDatHang { get; set; }
        public string TenKhachHang { get; set; }
        public string DiaChi { get; set; }
        public string SoDienThoai { get; set; }
        public string Email { get; set; }
        public string GhiChu { get; set; }
        public bool DaGiaoHang { get; set; }
        public string UserID { get; set; }
    
        public virtual ICollection<DonHangChiTiet> DonHangChiTiets { get; set; }
    }
}
