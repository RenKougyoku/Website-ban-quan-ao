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
    
    public partial class PhieuNhap
    {
        public PhieuNhap()
        {
            this.PhieuNhapChiTiets = new HashSet<PhieuNhapChiTiet>();
        }
    
        public int PhieuNhapID { get; set; }
        public Nullable<System.DateTime> NgayNhap { get; set; }
    
        public virtual ICollection<PhieuNhapChiTiet> PhieuNhapChiTiets { get; set; }
    }
}
