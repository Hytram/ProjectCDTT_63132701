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
    public class San_phamController : Controller
    {
        private Project_63132701Entities2 db = new Project_63132701Entities2();

        public ActionResult QLSP()
        {
            var sanPhams = db.SanPhams.ToList();
            return View(sanPhams);
        }


        public ActionResult ThuongHieu()
        {
            return View();
        }

        public ActionResult Nhan(decimal? minPrice, decimal? maxPrice, string loaiBac, string sapXep)
        {
            var sanPhams = db.SanPhams.Where(s => s.MaLoaiSP == 1); // ID danh mục 'Nhẫn bạc'

            // 🔹 Lọc theo khoảng giá
            if (minPrice.HasValue)
            {
                sanPhams = sanPhams.Where(s => s.DonGia >= minPrice.Value);
            }
            if (maxPrice.HasValue)
            {
                sanPhams = sanPhams.Where(s => s.DonGia <= maxPrice.Value);
            }

            // 🔹 Lọc theo loại bạc
            if (!string.IsNullOrEmpty(loaiBac))
            {
                sanPhams = sanPhams.Where(s => s.LoaiBac == loaiBac);
            }

            // 🔹 Sắp xếp sản phẩm
            switch (sapXep)
            {
                case "moi-nhat":
                    sanPhams = sanPhams.OrderByDescending(s => s.NgayTao);
                    break;
                case "gia-cao":
                    sanPhams = sanPhams.OrderByDescending(s => s.DonGia);
                    break;
                case "gia-thap":
                    sanPhams = sanPhams.OrderBy(s => s.DonGia);
                    break;
            }

            return View(sanPhams.ToList());
        }
        public ActionResult Day_Chuyen(decimal? minPrice, decimal? maxPrice, string loaiBac, string sapXep)
        {
            var sanPhams = db.SanPhams.Where(s => s.MaLoaiSP == 2); // ID danh mục 'Nhẫn bạc'

            // 🔹 Lọc theo khoảng giá
            if (minPrice.HasValue)
            {
                sanPhams = sanPhams.Where(s => s.DonGia >= minPrice.Value);
            }
            if (maxPrice.HasValue)
            {
                sanPhams = sanPhams.Where(s => s.DonGia <= maxPrice.Value);
            }

            // 🔹 Lọc theo loại bạc
            if (!string.IsNullOrEmpty(loaiBac))
            {
                sanPhams = sanPhams.Where(s => s.LoaiBac == loaiBac);
            }

            // 🔹 Sắp xếp sản phẩm
            switch (sapXep)
            {
                case "moi-nhat":
                    sanPhams = sanPhams.OrderByDescending(s => s.NgayTao);
                    break;
                case "gia-cao":
                    sanPhams = sanPhams.OrderByDescending(s => s.DonGia);
                    break;
                case "gia-thap":
                    sanPhams = sanPhams.OrderBy(s => s.DonGia);
                    break;
            }

            return View(sanPhams.ToList());
        }
        public ActionResult Lac_Tay(decimal? minPrice, decimal? maxPrice, string loaiBac, string sapXep)
        {
            var sanPhams = db.SanPhams.Where(s => s.MaLoaiSP == 3); // ID danh mục 'Nhẫn bạc'

            // 🔹 Lọc theo khoảng giá
            if (minPrice.HasValue)
            {
                sanPhams = sanPhams.Where(s => s.DonGia >= minPrice.Value);
            }
            if (maxPrice.HasValue)
            {
                sanPhams = sanPhams.Where(s => s.DonGia <= maxPrice.Value);
            }

            // 🔹 Lọc theo loại bạc
            if (!string.IsNullOrEmpty(loaiBac))
            {
                sanPhams = sanPhams.Where(s => s.LoaiBac == loaiBac);
            }

            // 🔹 Sắp xếp sản phẩm
            switch (sapXep)
            {
                case "moi-nhat":
                    sanPhams = sanPhams.OrderByDescending(s => s.NgayTao);
                    break;
                case "gia-cao":
                    sanPhams = sanPhams.OrderByDescending(s => s.DonGia);
                    break;
                case "gia-thap":
                    sanPhams = sanPhams.OrderBy(s => s.DonGia);
                    break;
            }

            return View(sanPhams.ToList());
        }
        
        public ActionResult Bong_Tai(decimal? minPrice, decimal? maxPrice, string loaiBac, string sapXep)
        {
            var sanPhams = db.SanPhams.Where(s => s.MaLoaiSP == 4); // ID danh mục 'Nhẫn bạc'

            // 🔹 Lọc theo khoảng giá
            if (minPrice.HasValue)
            {
                sanPhams = sanPhams.Where(s => s.DonGia >= minPrice.Value);
            }
            if (maxPrice.HasValue)
            {
                sanPhams = sanPhams.Where(s => s.DonGia <= maxPrice.Value);
            }

            // 🔹 Lọc theo loại bạc
            if (!string.IsNullOrEmpty(loaiBac))
            {
                sanPhams = sanPhams.Where(s => s.LoaiBac == loaiBac);
            }

            // 🔹 Sắp xếp sản phẩm
            switch (sapXep)
            {
                case "moi-nhat":
                    sanPhams = sanPhams.OrderByDescending(s => s.NgayTao);
                    break;
                case "gia-cao":
                    sanPhams = sanPhams.OrderByDescending(s => s.DonGia);
                    break;
                case "gia-thap":
                    sanPhams = sanPhams.OrderBy(s => s.DonGia);
                    break;
            }

            return View(sanPhams.ToList());
        }



        public ActionResult ChiTietSanPham(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index"); 
            }

            var sanPham = db.SanPhams.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }

            return View(sanPham);
        }


        public ActionResult GioHang()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddToCart(int MaSP, int SoLuong, string returnUrl)
        {
            if (Session["UserRole"] == null || Session["UserRole"].ToString().Trim() != "Khách hàng")
            {
                return RedirectToAction("Login", "Tai_khoan");
            }

            var maKhachHang = (int)Session["UserId"];
            var gioHang = db.GioHangs.FirstOrDefault(g => g.MaKH == maKhachHang && g.TrangThai == "Chưa duyệt");
            var sanPham = db.SanPhams.Find(MaSP);

            if (gioHang == null)
            {
                gioHang = new GioHang
                {
                    MaKH = maKhachHang,
                    TrangThai = "Chưa duyệt",
                    NgayDatHang = DateTime.Now
                };
                db.GioHangs.Add(gioHang);
                db.SaveChanges();
            }

            var chiTiet = db.ChiTietGioHangs.FirstOrDefault(c => c.MaGH == gioHang.MaGH && c.MaSP == MaSP);
            if (chiTiet != null)
            {
                if (sanPham.SoLuong < chiTiet.SoLuong + SoLuong)
                {
                    TempData["ErrorMessage"] = "Số lượng sản phẩm trong kho không đủ!";
                    return Redirect(Request.UrlReferrer?.ToString());
                }
                chiTiet.SoLuong += SoLuong;
            }
            else
            {
                db.ChiTietGioHangs.Add(new ChiTietGioHang
                {
                    MaGH = gioHang.MaGH,
                    MaSP = MaSP,
                    SoLuong = SoLuong,
                    DonGia = sanPham.DonGia
                });
            }

            sanPham.SoLuong -= SoLuong;
            db.SaveChanges();

            if (!string.IsNullOrEmpty(returnUrl))
            {
                return Redirect(returnUrl);
            }

            int? cartCount = db.ChiTietGioHangs.Where(c => c.MaGH == gioHang.MaGH).Sum(c => c.SoLuong);
            Session["CartCount"] = cartCount;

            return RedirectToAction("San_Pham");
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
            return View();
        }

        // GET: san_pham/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham san_pham = db.SanPhams.Find(id);
            if (san_pham == null)
            {
                return HttpNotFound();
            }
            return View(san_pham);
        }

        // GET: san_pham/Create
        public ActionResult Create()
        {
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams, "id", "ten_danh_muc");
            return View();
        }

        // POST: san_pham/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,ten_san_pham,mo_ta,gia,so_luong,id_danh_muc,anh_san_pham,loai_bac,ngay_tao")] SanPham san_pham)
        {
            if (ModelState.IsValid)
            {
                db.SanPhams.Add(san_pham);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_danh_muc = new SelectList(db.LoaiSanPhams, "id", "ten_danh_muc", san_pham);
            return View(san_pham);
        }

        // GET: san_pham/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham san_pham = db.SanPhams.Find(id);
            if (san_pham == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_danh_muc = new SelectList(db.LoaiSanPhams, "id", "ten_danh_muc", san_pham.MaLoaiSP);
            return View(san_pham);
        }

        // POST: san_pham/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,ten_san_pham,mo_ta,gia,so_luong,id_danh_muc,anh_san_pham,loai_bac,ngay_tao")] SanPham san_pham)
        {
            if (ModelState.IsValid)
            {
                db.Entry(san_pham).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_danh_muc = new SelectList(db.LoaiSanPhams, "id", "ten_danh_muc", san_pham.MaLoaiSP);
            return View(san_pham);
        }

        // GET: san_pham/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham san_pham = db.SanPhams.Find(id);
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
            SanPham san_pham = db.SanPhams.Find(id);
            db.SanPhams.Remove(san_pham);
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
