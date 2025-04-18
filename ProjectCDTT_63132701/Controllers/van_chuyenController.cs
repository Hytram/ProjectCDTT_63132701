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
            VanChuyen vanChuyen = db.VanChuyens.Find(id);
            if (vanChuyen == null)
            {
                return HttpNotFound();
            }
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
            ViewBag.MaDH = new SelectList(db.DonHangs, "MaDH", "TrangThai", vanChuyen.MaDH);
            return View(vanChuyen);
        }

        // POST: Van_chuyen/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaVanChuyen,MaDH,DonViVanChuyen,TrangThai,NgayTao,ThoiGianGiao,ThoiGianNhan")] VanChuyen vanChuyen)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vanChuyen).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("QLVC");
            }
            ViewBag.MaDH = new SelectList(db.DonHangs, "MaDH", "TrangThai", vanChuyen.MaDH);
            return View(vanChuyen);
        }

        // GET: Van_chuyen/Delete/5
        public ActionResult Delete(int? id)
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

            return View(vanChuyen);
        }

        // POST: VanChuyen/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            VanChuyen vanChuyen = db.VanChuyens.Find(id);

            if (vanChuyen != null)
            {
                db.VanChuyens.Remove(vanChuyen);
                db.SaveChanges();
            }

            return RedirectToAction("QLVC", "vanChuyen"); 
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
