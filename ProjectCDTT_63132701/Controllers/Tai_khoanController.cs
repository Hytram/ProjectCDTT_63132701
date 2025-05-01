using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
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
                Session["FavoriteCount"] = db.YeuThiches.Count(y => y.MaKH == user.MaKH);
                Session["ConfirmedOrderCount"] = db.HoaDons.Count(h => h.MaKH == user.MaKH && h.TrangThai == "Shipping");

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

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(string hoTen, string email, string soDienThoai, string diaChi, string matKhau)
        {
            try
            {
                if (db.KhachHangs.Any(k => k.Email.ToLower() == email.ToLower()))
                {
                    ViewBag.Message = "Email đã được sử dụng.";
                    return View();
                }

                var newKhachHang = new KhachHang
                {
                    HoTen = hoTen,
                    Email = email,
                    SoDienThoai = soDienThoai,
                    DiaChi = diaChi,
                    MatKhau = matKhau,
                    VaiTro = "User"
                };

                db.KhachHangs.Add(newKhachHang);
                db.SaveChanges();

                ViewBag.Message = "Đăng ký thành công! Vui lòng đăng nhập.";
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Đã xảy ra lỗi: " + ex.Message;
                return View();
            }
        }





        // GET: Hiển thị form nhập email
        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        // POST: Gửi email với link reset
        [HttpPost]
        public ActionResult ForgotPassword(string email)
        {
            var user = db.KhachHangs.SingleOrDefault(u => u.Email == email);
            if (user != null)
            {
                // Tạo token
                string token = Guid.NewGuid().ToString();
                user.Token = token;
                user.TokenHetHan = DateTime.UtcNow.AddHours(1);  // Token hết hạn sau 1 giờ
                db.SaveChanges();

                // Tạo link reset
                string resetLink = Url.Action("ResetPassword", "tai_khoan", new { email = user.Email, token = token }, Request.Url.Scheme);
                Console.WriteLine("Link gửi đi: " + resetLink);
                // Gửi email
                EmailService.Send(email, "Đặt lại mật khẩu", $"Nhấn vào đây để đặt lại mật khẩu: <a href='{resetLink}'>Reset mật khẩu</a>");
            }

            ViewBag.Message = "Nếu email tồn tại, bạn sẽ nhận được hướng dẫn qua email.";
            return View();
        }


        public static class EmailService
        {
            public static void Send(string toEmail, string subject, string body)
            {
                var fromEmail = "caohuyentramdk@gmail.com";
                var fromPassword = "owxq irnl rway gudj"; // Dùng App Password nếu là Gmail

                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromEmail, fromPassword)
                };

                var message = new MailMessage(fromEmail, toEmail, subject, body);
                message.IsBodyHtml = true;

                smtp.Send(message);
            }
        }






        // GET: Hiển thị form đặt lại mật khẩu qua email
        public ActionResult ResetPassword(string email, string token)
        {
            var khachHang = db.KhachHangs.SingleOrDefault(k => k.Email == email && k.Token == token);

            // Kiểm tra token hợp lệ và chưa hết hạn
            if (khachHang == null || khachHang.TokenHetHan < DateTime.UtcNow)
            {
                // Token không hợp lệ hoặc đã hết hạn, chuyển hướng đến trang TokenInvalid
                return RedirectToAction("TokenInvalid");
            }

            ViewBag.Email = email;
            ViewBag.Token = token;
            return View();
        }


        [HttpPost]
        public ActionResult ResetPassword(string email, string token, string matKhauMoi, string xacNhanMatKhau)
        {
            var khachHang = db.KhachHangs.SingleOrDefault(k => k.Email == email && k.Token == token);

            // Kiểm tra token hợp lệ và chưa hết hạn
            if (khachHang == null || khachHang.TokenHetHan < DateTime.UtcNow)
            {
                return RedirectToAction("TokenInvalid");
            }

            if (matKhauMoi != xacNhanMatKhau)
            {
                ModelState.AddModelError("", "Mật khẩu mới và xác nhận không khớp.");
                ViewBag.Email = email;
                ViewBag.Token = token;
                return View();
            }

            try
            {
                khachHang.MatKhau = matKhauMoi;
                khachHang.Token = null;
                db.SaveChanges();

                ViewBag.ThongBao = "Đặt lại mật khẩu thành công. Bạn có thể đăng nhập lại.";
                return View("SuccessPassword");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Lỗi lưu mật khẩu: " + ex.Message);
                ViewBag.Email = email;
                ViewBag.Token = token;
                return View();
            }

        }

        public ActionResult TokenInvalid()
        {
            return View();
        }

        public ActionResult SuccessPassword(string matKhauMoi, string xacNhanMatKhau)
        {
            // Kiểm tra và thực hiện logic reset mật khẩu

            if (matKhauMoi == xacNhanMatKhau)
            {
                // Cập nhật mật khẩu vào cơ sở dữ liệu
                // ...

                return View("SuccessPassword");  // Gọi view SuccessPassword
            }
            else
            {
                // Nếu mật khẩu không khớp
                ViewBag.Message = "Mật khẩu và xác nhận mật khẩu không khớp!";
                return View();
            }
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
