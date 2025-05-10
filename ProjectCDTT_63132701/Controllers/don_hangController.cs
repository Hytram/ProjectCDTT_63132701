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
    public class Don_hangController : Controller
    {
        private Project_63132701Entities2 db = new Project_63132701Entities2();

        public ActionResult QLDH()
        {
            var donHangs = db.DonHangs.Include(h => h.KhachHang).ToList();
            return View(donHangs);
        }







        // GET: Don_hang
        public ActionResult Index(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var hoaDon = db.HoaDons.Include("KhachHang")
                                  .FirstOrDefault(h => h.MaDH == id);

            if (hoaDon == null)
            {
                return HttpNotFound();
            }

            var chiTietHoaDon = db.ChiTietHoaDons
                                   .Where(ct => ct.MaDH == id)
                                   .Include("SanPham")
                                   .ToList();

            ViewBag.ChiTietSanPham = chiTietHoaDon;

            return View(hoaDon);
        }

       
        // GET: Don_hang/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var hoaDon = db.HoaDons
                .Include("KhachHang")
                .Include("ChiTietHoaDons.SanPham")
                .FirstOrDefault(h => h.MaDH == id);

            if (hoaDon == null)
            {
                return HttpNotFound();
            }

            return View(hoaDon);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DuyetDonHang(int id)
        {
            var donHang = db.DonHangs.FirstOrDefault(dh => dh.MaDH == id);
            var hoaDon = db.HoaDons.FirstOrDefault(hd => hd.MaDH == id);
            var vanChuyen = db.VanChuyens.FirstOrDefault(vc => vc.MaDH == id);

            if (donHang != null)
            {
                if (donHang.TrangThai == "Confirmed")
                {
                    donHang.TrangThai = "Pending";

                    if (hoaDon != null)
                    {
                        hoaDon.TrangThai = "Pending";
                    }

                    if (vanChuyen != null)
                    {
                        vanChuyen.TrangThai = "Pending";
                    }

                    TempData["SuccessMessage"] = "Đã hủy duyệt đơn hàng.";
                }
                else
                {
                    donHang.TrangThai = "Confirmed";

                    if (hoaDon != null)
                    {
                        hoaDon.TrangThai = "Shipping";
                    }

                    if (vanChuyen != null)
                    {
                        vanChuyen.TrangThai = "Shipping";
                    }

                    TempData["SuccessMessage"] = "Đã duyệt đơn hàng thành công.";
                }

                db.SaveChanges();
                return RedirectToAction("QLDH");
            }

            TempData["ErrorMessage"] = "Không tìm thấy đơn hàng.";
            return RedirectToAction("QLDH");
        }


        // GET: Don_hang/Create
        public ActionResult Create()
        {
            ViewBag.MaKH = new SelectList(db.KhachHangs, "MaKH", "HoTen");
            ViewBag.MaDH = new SelectList(db.HoaDons, "MaDH", "TrangThai");
            return View();
        }

        // POST: Don_hang/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaDH,MaKH,TongTien,TrangThai,NgayTao,MaVanDon")] DonHang donHang)
        {
            if (ModelState.IsValid)
            {
                db.DonHangs.Add(donHang);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaKH = new SelectList(db.KhachHangs, "MaKH", "HoTen", donHang.MaKH);
            ViewBag.MaDH = new SelectList(db.HoaDons, "MaDH", "TrangThai", donHang.MaDH);
            return View(donHang);
        }

        // GET: Don_hang/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var hoaDon = db.HoaDons
                .Include(h => h.KhachHang)
                .Include(h => h.DonHang)
                .Include(h => h.ChiTietHoaDons.Select(c => c.SanPham))
                .FirstOrDefault(h => h.MaDH == id);

            if (hoaDon == null)
            {
                return HttpNotFound();
            }

            return View(hoaDon);
        }

        // POST: HoaDon/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(HoaDon hoaDon)
        {
            if (ModelState.IsValid)
            {
                var existingHoaDon = db.HoaDons.Include(h => h.DonHang).FirstOrDefault(h => h.MaDH == hoaDon.MaDH);
                if (existingHoaDon != null)
                {
                    existingHoaDon.DonHang.TrangThai = hoaDon.DonHang.TrangThai;
                    db.SaveChanges();
                    ViewBag.Success = true;
                    return RedirectToAction("QLDH", new { id = hoaDon.MaDH });
                }
                return HttpNotFound();
            }
            return View(hoaDon);
        }


        // GET: Don_hang/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonHang donHang = db.DonHangs.Find(id);
            if (donHang == null)
            {
                return HttpNotFound();
            }
            return View(donHang);
        }

        // POST: Don_hang/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DonHang donHang = db.DonHangs.Find(id);
            db.DonHangs.Remove(donHang);
            db.SaveChanges();
            return RedirectToAction("QLDH");
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
