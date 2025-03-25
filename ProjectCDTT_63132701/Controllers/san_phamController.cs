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
            var sanPhams = db.SanPhams.Where(s => s.MaLoaiSP == 1);

            if (minPrice.HasValue)
            {
                sanPhams = sanPhams.Where(s => s.DonGia >= minPrice.Value);
            }
            if (maxPrice.HasValue)
            {
                sanPhams = sanPhams.Where(s => s.DonGia <= maxPrice.Value);
            }

            if (!string.IsNullOrEmpty(loaiBac))
            {
                sanPhams = sanPhams.Where(s => s.LoaiBac == loaiBac);
            }

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
            var sanPhams = db.SanPhams.Where(s => s.MaLoaiSP == 2);

            if (minPrice.HasValue)
            {
                sanPhams = sanPhams.Where(s => s.DonGia >= minPrice.Value);
            }
            if (maxPrice.HasValue)
            {
                sanPhams = sanPhams.Where(s => s.DonGia <= maxPrice.Value);
            }

            if (!string.IsNullOrEmpty(loaiBac))
            {
                sanPhams = sanPhams.Where(s => s.LoaiBac == loaiBac);
            }

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
            var sanPhams = db.SanPhams.Where(s => s.MaLoaiSP == 3); 

            if (minPrice.HasValue)
            {
                sanPhams = sanPhams.Where(s => s.DonGia >= minPrice.Value);
            }
            if (maxPrice.HasValue)
            {
                sanPhams = sanPhams.Where(s => s.DonGia <= maxPrice.Value);
            }

            if (!string.IsNullOrEmpty(loaiBac))
            {
                sanPhams = sanPhams.Where(s => s.LoaiBac == loaiBac);
            }

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
            var sanPhams = db.SanPhams.Where(s => s.MaLoaiSP == 4); 

            if (minPrice.HasValue)
            {
                sanPhams = sanPhams.Where(s => s.DonGia >= minPrice.Value);
            }
            if (maxPrice.HasValue)
            {
                sanPhams = sanPhams.Where(s => s.DonGia <= maxPrice.Value);
            }

            if (!string.IsNullOrEmpty(loaiBac))
            {
                sanPhams = sanPhams.Where(s => s.LoaiBac == loaiBac);
            }

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
            if (Session["UserRole"] == null || Session["UserRole"].ToString().Trim() != "User")
            {
                return RedirectToAction("Login", "Tai_khoan");
            }
            var maKhachHang = (int)Session["UserId"];
            var gioHang = db.GioHangs.FirstOrDefault(g => g.MaKH == maKhachHang && g.TrangThai == "Pending");

            if (gioHang != null)
            {
                var chiTietGioHang = db.ChiTietGioHangs
                    .Where(c => c.MaGH == gioHang.MaGH)
                    .Include(c => c.SanPham)
                    .ToList();

                ViewBag.Cart = chiTietGioHang;
            }
            else
            {
                ViewBag.Cart = new List<ChiTietGioHang>();
            }
            return View();
        }

        [HttpPost]
        public ActionResult AddToCart(int MaSP, int SoLuong, string returnUrl)
        {
            if (Session["UserRole"] == null || Session["UserRole"].ToString().Trim() != "User")
            {
                return RedirectToAction("Login", "Tai_khoan");
            }

            var maKhachHang = (int)Session["UserId"];
            var gioHang = db.GioHangs.FirstOrDefault(g => g.MaKH == maKhachHang && g.TrangThai == "Pending");
            var sanPham = db.SanPhams.Find(MaSP);

            if (gioHang == null)
            {
                gioHang = new GioHang
                {
                    MaKH = maKhachHang,
                    TrangThai = "Pending",
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

        [HttpPost]
        public ActionResult RemoveFromCart(int MaSP)
        {
            if (Session["UserRole"] == null || Session["UserRole"].ToString().Trim() != "User")
            {
                TempData["Message"] = "Ban can dang nhap de thao tac tren gio hang";
                return RedirectToAction("Login", "Tai_khoan");
            }
            var maKhachHang = (int)Session["UserId"];
            var gioHang = db.GioHangs.FirstOrDefault(g => g.MaKH == maKhachHang && g.TrangThai == "Pending");

            if (gioHang != null)
            {
                var chiTiet = db.ChiTietGioHangs.FirstOrDefault(c => c.MaGH == gioHang.MaGH && c.MaSP == MaSP);
                if (chiTiet != null)
                {
                    db.ChiTietGioHangs.Remove(chiTiet);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("GioHang");
        }

        [HttpPost]
        public ActionResult DatHang()
        {
            int maKhachHang = (int)Session["UserId"];
            var gioHang = db.GioHangs.Include("ChiTietGioHangs.SanPham")
                                     .FirstOrDefault(g => g.MaKH == maKhachHang && g.TrangThai == "Pending");
            if (gioHang == null || !gioHang.ChiTietGioHangs.Any())
            {
                TempData["Message"] = "Giỏ hàng của bạn trống";
                return RedirectToAction("Index", "San_pham");
            }

            string GenerateTrackingNumber()
            {
                return "DH-" + DateTime.Now.ToString("yyyyMMdd") + "-" + new Random().Next(100000, 999999);
            }

            DonHang donHang = new DonHang
            {
                MaKH = maKhachHang,
                TongTien = gioHang.ChiTietGioHangs.Sum(ct => (ct.SoLuong ?? 0) * (ct.SanPham.DonGia)),
                NgayTao = DateTime.Now,
                TrangThai = "Pending",
                MaVanDon = GenerateTrackingNumber()
            };

            db.DonHangs.Add(donHang);
            db.SaveChanges(); 

            HoaDon hoaDon = new HoaDon
            {
                MaDH = donHang.MaDH, 
                MaKH = maKhachHang,
                NgayTao = DateTime.Now,
                TrangThai = "Pending",
                TongTien = donHang.TongTien
            };
            db.HoaDons.Add(hoaDon);
            db.SaveChanges(); 

            foreach (var item in gioHang.ChiTietGioHangs)
            {
                ChiTietHoaDon chiTiet = new ChiTietHoaDon
                {
                    MaDH = hoaDon.MaDH, 
                    MaSP = item.MaSP,
                    SoLuong = item.SoLuong ?? 0,
                    Gia = item.SanPham.DonGia
                };
                db.ChiTietHoaDons.Add(chiTiet);
            }

            db.SaveChanges();

            db.ChiTietGioHangs.RemoveRange(gioHang.ChiTietGioHangs);
            db.GioHangs.Remove(gioHang);
            db.SaveChanges();

            TempData["Message"] = "Đặt hàng thành công";
            return RedirectToAction("Index", "San_pham");
        }


        public ActionResult ChiTietHoaDon(int? id)
        {
            var hoaDon = db.HoaDons
                .Where(h => h.MaDH == id)
                .Include(h => h.KhachHang)
                .Include(h => h.ChiTietHoaDons)
                .Include(h => h.ChiTietHoaDons.Select(ct => ct.SanPham))
                .FirstOrDefault();
            if (hoaDon == null)
            {
                return HttpNotFound();
            }
            return View(hoaDon);
        }

        [HttpPost]
        public ActionResult DuyetDonHang(int MaDH)
        {
            var hoaDon = db.HoaDons.FirstOrDefault(h => h.MaDH == MaDH);
            hoaDon.TrangThai = "Đã duyệt";
            db.SaveChanges();
            return RedirectToAction("ChiTietHoaDon", new { id = MaDH });
        }


        public ActionResult YeuThich()
        {
            return View();
        }

        public ActionResult DichVu()
        {
            return View();
        }

        public ActionResult KhuyenMai()
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
