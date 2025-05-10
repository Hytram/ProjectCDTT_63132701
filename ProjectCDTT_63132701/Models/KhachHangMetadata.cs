using System;
using System.ComponentModel.DataAnnotations;

namespace ProjectCDTT_63132701.Models
{
    [MetadataType(typeof(KhachHangMetadata))]
    public partial class KhachHang
    {
    }
    public class KhachHangMetadata
    {
        [Required(ErrorMessage = "Họ tên không được để trống!")]
        [RegularExpression(@"^([A-ZÀ-Ỵ][a-zà-ỹ]+)(\s[A-ZÀ-Ỵ][a-zà-ỹ]+)*$",
            ErrorMessage = "Họ tên phải viết hoa chữ cái đầu!")]
        public string HoTen { get; set; }

        [Required(ErrorMessage = "Email không được để trống!")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được để trống!")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\w\s]).{6,}$",
            ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự, gồm chữ hoa, chữ thường, số và ký tự đặc biệt")]
        public string MatKhau { get; set; }

        [Required(ErrorMessage = "Số điện thoại không được để trống!")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Số điện thoại phải là 10 chữ số")]
        public string SoDienThoai { get; set; }

        [Required(ErrorMessage = "Địa chỉ không được để trống!")]
        public string DiaChi { get; set; }
    }
}
