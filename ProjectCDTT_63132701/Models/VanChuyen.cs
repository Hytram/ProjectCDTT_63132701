//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ProjectCDTT_63132701.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class VanChuyen
    {
        public int MaVanChuyen { get; set; }
        public Nullable<int> MaDH { get; set; }
        public string DonViVanChuyen { get; set; }
        public string TrangThai { get; set; }
        public Nullable<System.DateTime> NgayTao { get; set; }
        public Nullable<System.DateTime> ThoiGianGiao { get; set; }
        public Nullable<System.DateTime> ThoiGianNhan { get; set; }
        public string DiaChiChiTiet { get; set; }
        public string PhuongXa { get; set; }
        public string QuanHuyen { get; set; }
        public string TinhThanh { get; set; }
        public decimal GiaVanChuyen { get; set; }
    
        public virtual DonHang DonHang { get; set; }
    }
}
