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
    public class Khach_hangController : Controller
    {
        private Project_63132701Entities2 db = new Project_63132701Entities2();

        public ActionResult QLKH()
        {
            var khachHangs = db.KhachHangs.ToList();
            return View(khachHangs);
        }



        public ActionResult EditProfile()
        {
            // Kiểm tra nếu người dùng chưa đăng nhập
            if (Session["UserRole"] == null || Session["UserRole"].ToString().Trim() != "User")
            {
                return RedirectToAction("Login", "Tai_khoan"); // Chuyển hướng đến trang đăng nhập
            }

            int maKhachHang = (int)Session["UserId"]; // Lấy ID khách hàng từ Session

            // Lấy thông tin khách hàng từ database
            var khachHang = db.KhachHangs.SingleOrDefault(kh => kh.MaKH == maKhachHang);

            if (khachHang == null)
            {
                return HttpNotFound(); // Nếu không tìm thấy khách hàng
            }

            return View(khachHang); // Trả về View với thông tin khách hàng
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProfile(KhachHang khachHang)
        {
            if (ModelState.IsValid)
            {
                int maKhachHang = (int)Session["UserId"]; // đảm bảo lấy đúng khách hàng đang đăng nhập

                var existingKhachHang = db.KhachHangs.SingleOrDefault(kh => kh.MaKH == maKhachHang);

                if (existingKhachHang != null)
                {
                    existingKhachHang.HoTen = khachHang.HoTen;
                    existingKhachHang.SoDienThoai = khachHang.SoDienThoai;
                    existingKhachHang.DiaChi = khachHang.DiaChi;

                    try
                    {
                        db.SaveChanges();
                        return RedirectToAction("Index", "San_pham");
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", "Lỗi khi lưu thay đổi: " + ex.Message);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Không tìm thấy khách hàng.");
                }
            }

            return View(khachHang);
        }













        // GET: Khach_hang
        public ActionResult Index()
        {
            return View(db.KhachHangs.ToList());
        }

        // GET: Khach_hang/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KhachHang khachHang = db.KhachHangs.Find(id);
            if (khachHang == null)
            {
                return HttpNotFound();
            }
            return View(khachHang);
        }

        // GET: Khach_hang/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Khach_hang/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaKH,HoTen,Email,MatKhau,SoDienThoai,DiaChi,VaiTro")] KhachHang khachHang)
        {
            if (ModelState.IsValid)
            {
                db.KhachHangs.Add(khachHang);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(khachHang);
        }

        // GET: Khach_hang/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KhachHang khachHang = db.KhachHangs.Find(id);
            if (khachHang == null)
            {
                return HttpNotFound();
            }
            return View(khachHang);
        }

        // POST: Khach_hang/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaKH,HoTen,Email,MatKhau,SoDienThoai,DiaChi,VaiTro")] KhachHang khachHang)
        {
            if (ModelState.IsValid)
            {
                db.Entry(khachHang).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("QLKH");
            }
            return View(khachHang);
        }

        // GET: Khach_hang/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KhachHang khachHang = db.KhachHangs.Find(id);
            if (khachHang == null)
            {
                return HttpNotFound();
            }
            return View(khachHang);
        }

        // POST: Khach_hang/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            KhachHang khachHang = db.KhachHangs.Find(id);
            db.KhachHangs.Remove(khachHang);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
