using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProjectCDTT_63132701.Models;

namespace ProjectCDTT_63132701.Controllers
{
    public class Tai_khoanController : Controller
    {
        private Project_63132701Entities2 db = new Project_63132701Entities2();

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string email, string password)
        {
            var user = db.KhachHangs.FirstOrDefault(u => u.Email.ToLower() == email.ToLower() && u.MatKhau == password);
            if (user != null)
            {
                Session["UserRole"] = user.VaiTro.Trim();
                Session["UserEmail"] = user.Email;
                Session["UserId"] = user.MaKH;
                var gioHang = db.GioHangs.FirstOrDefault(g => g.MaKH == user.MaKH && g.TrangThai == "Pending");
                Session["CartCount"] = gioHang != null ? db.ChiTietGioHangs.Where(c => c.MaGH == gioHang.MaGH).Sum(c => c.SoLuong) : 0;

<<<<<<< HEAD
                if (user.VaiTro == "Admin")
=======
                if(user.VaiTro == "Admin")
>>>>>>> 3cc5d5c67337ef894980c7361f08aa74d14266b7
                {
                    return RedirectToAction("QLSP", "San_pham");
                }
                else
                {
                    return RedirectToAction("Index", "San_pham");
                }

            }

            ViewBag.Message = "Sai thông tin đăng nhập";
            return View();
        }

        [HttpGet]
        public ActionResult Logout()
        {
            Session.Clear(); 
            Session.Abandon(); 
            return RedirectToAction("Index", "San_pham");
        }

    }
}
