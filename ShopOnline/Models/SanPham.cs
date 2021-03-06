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
    
    public partial class SanPham
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SanPham()
        {
            this.PhieuNhapChiTiets = new HashSet<PhieuNhapChiTiet>();
            this.SanPhamHinhs = new HashSet<SanPhamHinh>();
            this.SanPhamSizes = new HashSet<SanPhamSize>();
        }
    
        public int SanPhamID { get; set; }
        public string Ma { get; set; }
        public string Ten { get; set; }
        public int GiaBan { get; set; }
        public Nullable<int> TrangThai { get; set; }
        public string Mota { get; set; }
        public int LoaiID { get; set; }
        public string NhaSanXuat { get; set; }
        public bool NgungBan { get; set; }
        public System.DateTime NgayCapNhat { get; set; }
        public string BiDanh { get; set; }
        public string Hinh { get; set; }
        public Nullable<int> SoLuongBan { get; set; }
    
        public virtual Loai Loai { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PhieuNhapChiTiet> PhieuNhapChiTiets { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SanPhamHinh> SanPhamHinhs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SanPhamSize> SanPhamSizes { get; set; }
    }
}
