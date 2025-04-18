﻿using System;
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
                Session["CartCount"] = gioHang != null ? db.ChiTietGioHangs.Where(c => c.MaGH == gioHang.MaGH).Sum(c => (int?)c.SoLuong) ?? 0 : 0;
                int wishlistCount = db.YeuThiches.Count(y => y.MaKH == user.MaKH);
                Session["FavoriteCount"] = wishlistCount;

                if (user.VaiTro == "Admin")
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
