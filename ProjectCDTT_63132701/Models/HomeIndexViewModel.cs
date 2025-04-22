using ProjectCDTT_63132701.Models;
using System.Collections.Generic;
namespace ProjectCDTT_63132701.Models
{
    public class HomeIndexViewModel
    {
        public List<SanPham> NewestProducts { get; set; }
        public List<SanPham> OldestProducts { get; set; }
    }
}