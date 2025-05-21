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
    public class Van_chuyenController : Controller
    {
        private Project_63132701Entities2 db = new Project_63132701Entities2();


        public ActionResult QLVC()
        {
            var donHangVanChuyens = db.VanChuyens.Include(d => d.DonHang.KhachHang).ToList();
            return View(donHangVanChuyens);
        }

        // GET: Van_chuyen
        public ActionResult Index()
        {
            var vanChuyens = db.VanChuyens.Include(v => v.DonHang);
            return View(vanChuyens.ToList());
        }

        // GET: Van_chuyen/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VanChuyen vanChuyen = db.VanChuyens
                    .Include(vc => vc.DonHang)
                    .Include(vc => vc.DonHang.ChiTietDonHangs.Select(ct => ct.SanPham))
                    .Include(vc => vc.DonHang.KhachHang)
                    .FirstOrDefault(vc => vc.MaVanChuyen == id);
            if (vanChuyen == null)
            {
                return HttpNotFound();
            }

            var hoaDon = db.HoaDons.FirstOrDefault(hd => hd.MaDH == vanChuyen.MaDH);
            ViewBag.TongTienHoaDon = hoaDon.TongTien;
            return View(vanChuyen);
        }

        // GET: Van_chuyen/Create
        public ActionResult Create()
        {
            ViewBag.MaDHList = new SelectList(
                db.DonHangs.Where(dh => !db.VanChuyens.Any(vc => vc.MaDH == dh.MaDH)),
                "MaDH", "MaDH"
            );

            ViewBag.DonViVanChuyenList = new SelectList(new[]
            {
                new { Value = "J&TExpress", Text = "J&T Express" },
                new { Value = "GiaoHangNhanh", Text = "Giao Hàng Nhanh" }
            }, "Value", "Text");

            ViewBag.TinhThanhList = new SelectList(
                db.GiaVanChuyens.Select(g => g.TinhThanh).Distinct(),
                "TinhThanh"
            );

            return View();
        }


        // POST: Van_chuyen/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(VanChuyen vc)
        {
            if (ModelState.IsValid)
            {
                db.VanChuyens.Add(vc);
                db.SaveChanges();
                return RedirectToAction("QLVC", "Van_chuyen");
            }

            ViewBag.DonViVanChuyenList = new SelectList(new[]
            {
                new { Value = "J&TExpress", Text = "J&T Express" },
                new { Value = "GiaoHangNhanh", Text = "Giao Hàng Nhanh" },
            }, "Value", "Text");
            return View(vc);
        }

       // GET: Van_chuyen/Edit/5
public ActionResult Edit(int? id)
{
    if (id == null)
    {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
    }
    VanChuyen vanChuyen = db.VanChuyens.Find(id);
    if (vanChuyen == null)
    {
        return HttpNotFound();
    }

    ViewBag.MaDH = new SelectList(db.DonHangs, "MaDH", "TrangThai", vanChuyen.MaDH); // Đảm bảo giá trị MaDH được truyền lên.
    return View(vanChuyen);
}
        // POST: Van_chuyen/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(VanChuyen vanChuyen)
        {
            if (ModelState.IsValid)
            {
                var existing = db.VanChuyens.Find(vanChuyen.MaVanChuyen);
                if (existing != null)
                {
                    // Chỉ cập nhật trường được phép thay đổi
                    existing.DonViVanChuyen = vanChuyen.DonViVanChuyen;
                    existing.ThoiGianGiao = vanChuyen.ThoiGianGiao;
                    existing.ThoiGianNhan = vanChuyen.ThoiGianNhan;

                    db.SaveChanges();
                    return RedirectToAction("QLVC");
                }
                return HttpNotFound();
            }

            return View(vanChuyen);
        }






        // GET: Van_chuyen/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var vanChuyen = db.VanChuyens.
                Include(vc => vc.DonHang).
                FirstOrDefault(vc => vc.MaVanChuyen == id);

            if (vanChuyen == null)
            {
                return HttpNotFound();
            }

            return View(vanChuyen);
        }

        // POST: VanChuyen/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var vanChuyen = db.VanChuyens.Find(id);
            var donHang = db.DonHangs.Find(vanChuyen.MaDH);
            var hoaDon = db.HoaDons.Find(vanChuyen.MaDH);

            if (vanChuyen == null)
            {
                return HttpNotFound(); // Nếu không tìm thấy đơn vị vận chuyển
            }

            try
            {
                db.DonHangs.Remove(donHang);
                db.HoaDons.Remove(hoaDon);
                db.VanChuyens.Remove(vanChuyen);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                // Log lỗi hoặc thông báo cho người dùng
                ModelState.AddModelError("", "Không thể xóa đơn vị vận chuyển: " + ex.Message);
                return View(vanChuyen); // Quay lại view với thông báo lỗi
            }

            return RedirectToAction("QLVC", "Van_chuyen");
        }
    }
    }
