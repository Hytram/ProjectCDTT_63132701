using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProjectCDTT_63132701.Models;
using EntityState = System.Data.Entity.EntityState;

namespace ProjectCDTN_63132701.Controllers
{
    public class san_phamController : Controller
    {
        private Project_63132701Entities db = new Project_63132701Entities();

        public ActionResult QLSP()
        {
            var sanPhams = db.san_pham.ToList();
            return View(sanPhams);
        }
        public ActionResult QLKH()
        {
            var khachHangs = db.khach_hang.ToList();
            return View(khachHangs);
        }
        public ActionResult QLDH()
        {
            var donHangs = db.don_hang.Include(h => h.khach_hang).ToList();
            return View(donHangs);
        }
        public ActionResult QLVC()
        {
            var donHangVanChuyens = db.van_chuyen.Include(d => d.don_hang).ToList();
            return View(donHangVanChuyens);
        }








        public ActionResult ThuongHieu()
        {
            return View();
        }


        public ActionResult Nhan(decimal? minPrice, decimal? maxPrice, string loaiBac, string sapXep)
        {
            var sanPhams = db.san_pham.Where(s => s.id_danh_muc == 1); // ID danh mục 'Nhẫn bạc'

            // 🔹 Lọc theo khoảng giá
            if (minPrice.HasValue)
            {
                sanPhams = sanPhams.Where(s => s.gia >= minPrice.Value);
            }
            if (maxPrice.HasValue)
            {
                sanPhams = sanPhams.Where(s => s.gia <= maxPrice.Value);
            }

            // 🔹 Lọc theo loại bạc
            if (!string.IsNullOrEmpty(loaiBac))
            {
                sanPhams = sanPhams.Where(s => s.loai_bac == loaiBac);
            }

            // 🔹 Sắp xếp sản phẩm
            switch (sapXep)
            {
                case "moi-nhat":
                    sanPhams = sanPhams.OrderByDescending(s => s.ngay_tao);
                    break;
                case "gia-cao":
                    sanPhams = sanPhams.OrderByDescending(s => s.gia);
                    break;
                case "gia-thap":
                    sanPhams = sanPhams.OrderBy(s => s.gia);
                    break;
            }

            return View(sanPhams.ToList());
        }





        public ActionResult GioHang()
        {
            return View();
        }

        public ActionResult YeuThich()
        {
            return View();
        }

        public ActionResult DichVu()
        {
            return View();
        }







        // GET: san_pham
        public ActionResult Index()
        {
            var san_pham = db.san_pham.Include(s => s.danh_muc);
            return View(san_pham.ToList());
        }

        // GET: san_pham/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            san_pham san_pham = db.san_pham.Find(id);
            if (san_pham == null)
            {
                return HttpNotFound();
            }
            return View(san_pham);
        }

        // GET: san_pham/Create
        public ActionResult Create()
        {
            ViewBag.id_danh_muc = new SelectList(db.danh_muc, "id", "ten_danh_muc");
            return View();
        }

        // POST: san_pham/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,ten_san_pham,mo_ta,gia,so_luong,id_danh_muc,anh_san_pham,loai_bac,ngay_tao")] san_pham san_pham)
        {
            if (ModelState.IsValid)
            {
                db.san_pham.Add(san_pham);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_danh_muc = new SelectList(db.danh_muc, "id", "ten_danh_muc", san_pham.id_danh_muc);
            return View(san_pham);
        }

        // GET: san_pham/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            san_pham san_pham = db.san_pham.Find(id);
            if (san_pham == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_danh_muc = new SelectList(db.danh_muc, "id", "ten_danh_muc", san_pham.id_danh_muc);
            return View(san_pham);
        }

        // POST: san_pham/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,ten_san_pham,mo_ta,gia,so_luong,id_danh_muc,anh_san_pham,loai_bac,ngay_tao")] san_pham san_pham)
        {
            if (ModelState.IsValid)
            {
                db.Entry(san_pham).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_danh_muc = new SelectList(db.danh_muc, "id", "ten_danh_muc", san_pham.id_danh_muc);
            return View(san_pham);
        }

        // GET: san_pham/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            san_pham san_pham = db.san_pham.Find(id);
            if (san_pham == null)
            {
                return HttpNotFound();
            }
            return View(san_pham);
        }

        // POST: san_pham/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            san_pham san_pham = db.san_pham.Find(id);
            db.san_pham.Remove(san_pham);
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
