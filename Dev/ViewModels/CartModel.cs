using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopOnline.ViewModels
{
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
            CartItem cartItem = _items.Find(p => p.SanPhamSizeID == item.SanPhamSizeID);
            if (cartItem == null)
                _items.Add(item);
            else
                cartItem.SoLuong += item.SoLuong;
        }

        public void Update(int SanPhamSizeID, int Soluong)
        {
            var item = _items.Find(p => p.SanPhamSizeID == SanPhamSizeID);
            item.SoLuong = Soluong;
        }

        public void Remove(int SanPhamSizeID)
        {
            var item = _items.Find(p => p.SanPhamSizeID == SanPhamSizeID);
            _items.Remove(item);
        }

        public double? Total()
        {
            var kq = _items.Sum(p => (p.SoLuong * p.SanPham.GiaBan ));
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