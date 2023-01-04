using System;

namespace AppUtils.Data
{
    public abstract class Person
    {
        public string HoTen { get; set; } // Họ tên

        public string DiaChi { get; set; } // Địa chỉ

        public bool GioiTinh { get; set; }// Giới tính

        public DateTime DOB { get; set; }// Ngày tháng năm sinh

        public string NumberPhone { get; set; }// Số điện thoại

        public string Email { get; set; }// Email liên hệ

        public string Avatar { get; set; }// Tên hình ảnh
        public string MatKhau { get; set; } // passwrod tài khoản
    }
}
