using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shoesws.ViewModels
{
    //
    // Ứng dụng LinqToObject trong cài đặt code

    public class CartModel
    {
        // Field
        private List<CartItem> _items = new List<CartItem>();

        // Property

        public List<CartItem> Items
        {
            get { return _items; }
        }

        // Methods

        public void Add(CartItem item)
        {
            CartItem cartItem = _items.Find(p => p.SanPhamID == item.SanPhamID);
            if (cartItem == null)
                _items.Add(item);
            else
                cartItem.SoLuong += item.SoLuong;
        }

        public void Update(int SanPhamID, int Soluong)
        {
            var item = _items.Find(p => p.SanPham.SanPhamID == SanPhamID);
            item.SoLuong = Soluong;
        }

        public void Remove(int SanPhamID)
        {
            var item = _items.Find(p => p.SanPham.SanPhamID == SanPhamID);
            _items.Remove(item);
        }

        public double? Total()
        {
            var kq = _items.Sum(p => (p.SoLuong * p.SanPham.GiaBan * (1 - p.SanPham.GiamGia)));
            return kq;
        }
        public int Quantity()
        {
            var kq = _items.Sum(p => p.SoLuong);
            return kq;
        }
        public void Clear()
        {
            _items.Clear();
        }
    }
}