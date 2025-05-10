using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
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

            return RedirectToAction("GioHang", "San_pham");
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
                    var sanPham = db.SanPhams.FirstOrDefault(p => p.MaSP == MaSP);
                    if (sanPham != null)
                    {
                        sanPham.SoLuong += chiTiet.SoLuong;
                    }
                    db.ChiTietGioHangs.Remove(chiTiet);
                    db.SaveChanges();
                    int cartCount = db.ChiTietGioHangs.Where(c => c.MaGH == gioHang.MaGH).Sum(c => (int?)c.SoLuong) ?? 0;
                    Session["CartCount"] = cartCount;
                }
            }

            return RedirectToAction("GioHang");
        }

        [HttpPost]
        public ActionResult DatHang(string tinhThanh, string quanHuyen, string phuongXa, string diaChiChiTiet, string donViVanChuyen, int maGiaVanChuyen, decimal giaVanChuyen)
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
                TongTien = giaVanChuyen,
                NgayTao = DateTime.Now,
                TrangThai = "Pending",
                MaVanDon = GenerateTrackingNumber()
            };

            db.DonHangs.Add(donHang);
            db.SaveChanges();

            VanChuyen vanChuyen = new VanChuyen
            {
                MaDH = donHang.MaDH,
                MaGiaVanChuyen = maGiaVanChuyen,
                TinhThanh = tinhThanh,
                QuanHuyen = quanHuyen,
                PhuongXa = phuongXa,
                GiaVanChuyen = giaVanChuyen,
                DiaChiChiTiet = diaChiChiTiet,
                DonViVanChuyen = donViVanChuyen.Trim(),
                NgayTao = DateTime.Now,
                TrangThai = "Pending"
            };
            db.VanChuyens.Add(vanChuyen);
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
                    SoLuong = item.SoLuong,
                    Gia = item.SanPham.DonGia
                };
                db.ChiTietHoaDons.Add(chiTiet);

                ChiTietDonHang chiTietDonHang = new ChiTietDonHang
                {
                    MaDH = donHang.MaDH,
                    MaSP = item.MaSP,
                    SoLuong = item.SoLuong,
                    DonGia = item.SanPham.DonGia
                };
                db.ChiTietDonHangs.Add(chiTietDonHang);
            }

            db.SaveChanges();

            db.ChiTietGioHangs.RemoveRange(gioHang.ChiTietGioHangs);
            db.GioHangs.Remove(gioHang);
            db.SaveChanges();
            Session["CartCount"] = 0;
            TempData["Message"] = "Đặt hàng thành công!";
            return RedirectToAction("GioHang", "San_pham");
        }

        public ActionResult HoaDon()
        {
            if (Session["UserRole"] == null || Session["UserRole"].ToString().Trim() != "User")
                return RedirectToAction("Login", "Tai_khoan");

            int maKhachHang = (int)Session["UserId"];

            var hoaDons = db.HoaDons
                            .Where(hd => hd.MaKH == maKhachHang)
                            .OrderByDescending(hd => hd.NgayTao)
                            .ToList();

            return View(hoaDons);
        }

        public ActionResult ChiTietHoaDon(int id)
        {
            if (Session["UserRole"] == null || Session["UserRole"].ToString().Trim() != "User")
            {
                return RedirectToAction("Login", "Tai_khoan");
            }
            var maKhachHang = (int)Session["UserId"];
            var hoaDon = db.HoaDons
                .Include(h => h.KhachHang)
                .Include(h => h.ChiTietHoaDons.Select(ct => ct.SanPham)) 
                .FirstOrDefault(g => g.MaKH == maKhachHang && g.MaDH == id);

            if (hoaDon == null)
            {
                TempData["ErrorMessage"] = "Hiện quý khách không có đơn hàng nào!";
            }
            return View(hoaDon);
        }

        [HttpPost]
        public ActionResult HuyDonHang(int maDH)
        {
            var hoaDon = db.HoaDons.FirstOrDefault(h => h.MaDH == maDH);
            if (hoaDon == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy đơn hàng hoặc không thể hủy.";
                return RedirectToAction("HoaDon");
            }
            var vanChuyen = db.VanChuyens.FirstOrDefault(v => v.MaDH == maDH);
            var chiTietDonHang = db.ChiTietDonHangs.Where(c => c.MaDH == maDH);
            var chiTietHoaDon = db.ChiTietHoaDons.Where(c => c.MaDH == maDH);
            var donHang = db.DonHangs.FirstOrDefault(h => h.MaDH == maDH);

            db.ChiTietDonHangs.RemoveRange(chiTietDonHang);
            db.ChiTietHoaDons.RemoveRange(chiTietHoaDon);
            db.VanChuyens.Remove(vanChuyen);
            db.HoaDons.Remove(hoaDon);
            db.DonHangs.Remove(donHang);
            db.SaveChanges();

            TempData["SuccessMessage"] = "Hủy đơn hàng thành công!";
            return RedirectToAction("HoaDon");
        }

        [HttpPost]
        public ActionResult DuyetDonHang(int MaDH)
        {
            var hoaDon = db.HoaDons.FirstOrDefault(h => h.MaDH == MaDH);
            hoaDon.TrangThai = "Đã duyệt";
            db.SaveChanges();
            return RedirectToAction("ChiTietHoaDon", new { id = MaDH });
        }



        [HttpPost]
        public ActionResult AddToWishlist(int MaSP)
        {
            if (Session["UserRole"] == null || Session["UserRole"].ToString().Trim() != "User")
            {
                return Json(new { success = false, message = "Bạn cần đăng nhập để thêm vào wishlist." });
            }

            var maKhachHang = Session["UserId"] as int?;
            if (!maKhachHang.HasValue)
            {
                return Json(new { success = false, message = "Bạn cần đăng nhập để thêm vào wishlist." });
            }

            var exist = db.YeuThiches.FirstOrDefault(y => y.MaKH == maKhachHang.Value && y.MaSP == MaSP);
            if (exist == null)
            {
                var yeuThich = new YeuThich
                {
                    MaKH = maKhachHang.Value,
                    MaSP = MaSP,
                    NgayThem = DateTime.Now
                };
                db.YeuThiches.Add(yeuThich);
                db.SaveChanges();
            }

            int wishlistCount = db.YeuThiches.Count(y => y.MaKH == maKhachHang.Value);
            Session["FavoriteCount"] = wishlistCount;
            return Json(new { success = true, count = wishlistCount , message="Thêm vào wishlist thành công!"});
        }


        public ActionResult YeuThich()
        {
            if (Session["UserRole"] == null || Session["UserRole"].ToString().Trim() != "User")
            {
                return RedirectToAction("Login", "Tai_khoan");
            }

            int maKhachHang = (int)Session["UserId"];

            var yeuThichList = db.YeuThiches
                .Where(y => y.MaKH == maKhachHang)
                .Include(y => y.SanPham)
                .ToList();

            return View(yeuThichList);
        }

        [HttpPost]
        public ActionResult RemoveFromWishlist(int MaSP)
        {
            if (Session["UserRole"] == null || Session["UserRole"].ToString().Trim() != "User")
            {
                return RedirectToAction("Login", "Tai_khoan");
            }

            var maKhachHang = Session["UserId"] as int?;
            if (!maKhachHang.HasValue)
            {
                return RedirectToAction("Login", "Tai_khoan");
            }

            var yeuThich = db.YeuThiches.FirstOrDefault(y => y.MaKH == maKhachHang.Value && y.MaSP == MaSP);
            if (yeuThich != null)
            {
                db.YeuThiches.Remove(yeuThich);
                db.SaveChanges();
            }

            int wishlistCount = db.YeuThiches.Count(y => y.MaKH == maKhachHang.Value);
            Session["FavoriteCount"] = wishlistCount;

            return Json(new { success = true, count = wishlistCount, message = "Xóa khỏi wishlist thành công!" });
        }

        public JsonResult GetWishlistInfo()
        {
            var maKhachHang = Session["UserId"] as int?;

            if (!maKhachHang.HasValue)
            {
                return Json(new { success = false, wishlist = new List<int>(), count = 0 }, JsonRequestBehavior.AllowGet);
            }

            var wishlist = db.YeuThiches.Where(y => y.MaKH == maKhachHang.Value)
                                        .Select(y => y.MaSP)
                                        .ToList();

            return Json(new { success = true, wishlist, count = wishlist.Count }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult BaoHanh()
        {
            return View();
        }

        public ActionResult TTVanChuyen()
        {
            return View();
        }

        public ActionResult LoaiBac()
        {
            return View();
        }

        public ActionResult KhuyenMai()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetShippingCost(string tinhThanh, string donViVanChuyen)
        {
            var shippingCost = db.GiaVanChuyens
                .FirstOrDefault(v => v.TinhThanh.Trim() == tinhThanh.Trim() &&
                                     v.DonViVanChuyen.Trim() == donViVanChuyen.Trim());

            if (shippingCost != null)
            {
                return Json(new { success = true, cost = shippingCost.GiaVanChuyen1, maGiaVanChuyen = shippingCost.MaGiaVanChuyen}, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false, message = "Không tìm thấy giá vận chuyển cho khu vực này!" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult SearchProduct(string keyword)
        {
            var matchedProducts = db.SanPhams
                .Where(sp => sp.TenSP.Contains(keyword))
                .Select(sp => new {
                    sp.MaSP,
                    sp.TenSP,
                    sp.AnhSanPham
                }).ToList();

            return Json(matchedProducts, JsonRequestBehavior.AllowGet);
        }



        // GET: san_pham
        public ActionResult Index()
        {
            var newestProducts = db.SanPhams
                .OrderByDescending(sp => sp.NgayTao)
                .Take(4)
                .ToList();

            var oldestProducts = db.SanPhams
                .OrderBy(sp => sp.NgayTao)
                .Take(4)
                .ToList();

            var viewModel = new HomeIndexViewModel
            {
                NewestProducts = newestProducts,
                OldestProducts = oldestProducts
            };

            return View(viewModel);
        }


        // GET: san_pham/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham san_pham = db.SanPhams.Include(s => s.LoaiSanPham).FirstOrDefault(s => s.MaSP == id);

            if (san_pham == null)
            {
                return HttpNotFound();
            }

            return View(san_pham);
        }


        // GET: san_pham/Create
        public ActionResult Create()
        {
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams, "MaLoaiSP", "TenLoaiSP");
            return View();
        }


        // POST: san_pham/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(
    [Bind(Include = "TenSP,MoTa,DonGia,SoLuong,MaLoaiSP,LoaiBac,NgayTao")] SanPham san_pham,
    HttpPostedFileBase AnhSanPhamUpload)
        {
            if (ModelState.IsValid)
            {
                if (AnhSanPhamUpload != null && AnhSanPhamUpload.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(AnhSanPhamUpload.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/images"), fileName);

                    AnhSanPhamUpload.SaveAs(path);

                    san_pham.AnhSanPham = fileName;
                }

                db.SanPhams.Add(san_pham);
                db.SaveChanges();
                return RedirectToAction("QLSP", "san_pham");
            }

            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams, "MaLoaiSP", "TenLoaiSP", san_pham.MaLoaiSP);
            return View(san_pham);
        }



        // GET: san_pham/Edit/5
        // GET: san_pham/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham sanPham = db.SanPhams.Find(id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams, "MaLoaiSP", "TenLoaiSP", sanPham.MaLoaiSP);
            return View(sanPham);
        }

        // POST: san_pham/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaSP,TenSP,MoTa,LoaiBac,DonGia,SoLuong,MaLoaiSP")] SanPham sanPham, HttpPostedFileBase AnhSanPhamUpload)
        {
            if (ModelState.IsValid)
            {
                // Xử lý ảnh sản phẩm
                if (AnhSanPhamUpload != null && AnhSanPhamUpload.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(AnhSanPhamUpload.FileName);
                    string path = Path.Combine(Server.MapPath("~/images"), fileName);
                    AnhSanPhamUpload.SaveAs(path);
                    sanPham.AnhSanPham = fileName;
                }
                else
                {
                    // Giữ nguyên ảnh cũ nếu không chọn ảnh mới
                    var oldData = db.SanPhams.AsNoTracking().FirstOrDefault(sp => sp.MaSP == sanPham.MaSP);
                    if (oldData != null)
                    {
                        sanPham.AnhSanPham = oldData.AnhSanPham;
                    }
                }

                db.Entry(sanPham).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("QLSP");
            }

            // Nếu ModelState không hợp lệ, load lại danh sách dropdown
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams, "MaLoaiSP", "TenLoaiSP", sanPham.MaLoaiSP);
            return View(sanPham);
        }


        // GET: san_pham/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham san_pham = db.SanPhams.Include(s => s.LoaiSanPham).FirstOrDefault(s => s.MaSP == id);

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

            if (san_pham == null)
            {
                return HttpNotFound();
            }

            db.SanPhams.Remove(san_pham);
            db.SaveChanges();
            return RedirectToAction("QLSP", "san_pham");
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
