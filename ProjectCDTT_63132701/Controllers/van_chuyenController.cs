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
    public class van_chuyenController : Controller
    {
        private Project_63132701Entities1 db = new Project_63132701Entities1();

        // GET: van_chuyen
        public ActionResult Index()
        {
            var van_chuyen = db.van_chuyen.Include(v => v.don_hang);
            return View(van_chuyen.ToList());
        }

        // GET: van_chuyen/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            van_chuyen van_chuyen = db.van_chuyen.Find(id);
            if (van_chuyen == null)
            {
                return HttpNotFound();
            }
            return View(van_chuyen);
        }

        // GET: van_chuyen/Create
        public ActionResult Create()
        {
            ViewBag.id_don_hang = new SelectList(db.don_hang, "id", "trang_thai");
            return View();
        }

        // POST: van_chuyen/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,id_don_hang,don_vi_van_chuyen,trang_thai,ngay_tao,thoi_gian_giao,thoi_gian_nhan")] van_chuyen van_chuyen)
        {
            if (ModelState.IsValid)
            {
                db.van_chuyen.Add(van_chuyen);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_don_hang = new SelectList(db.don_hang, "id", "trang_thai", van_chuyen.id_don_hang);
            return View(van_chuyen);
        }

        // GET: van_chuyen/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            van_chuyen van_chuyen = db.van_chuyen.Find(id);
            if (van_chuyen == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_don_hang = new SelectList(db.don_hang, "id", "trang_thai", van_chuyen.id_don_hang);
            return View(van_chuyen);
        }

        // POST: van_chuyen/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,id_don_hang,don_vi_van_chuyen,trang_thai,ngay_tao,thoi_gian_giao,thoi_gian_nhan")] van_chuyen van_chuyen)
        {
            if (ModelState.IsValid)
            {
                db.Entry(van_chuyen).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_don_hang = new SelectList(db.don_hang, "id", "trang_thai", van_chuyen.id_don_hang);
            return View(van_chuyen);
        }

        // GET: van_chuyen/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            van_chuyen van_chuyen = db.van_chuyen.Find(id);
            if (van_chuyen == null)
            {
                return HttpNotFound();
            }
            return View(van_chuyen);
        }

        // POST: van_chuyen/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            van_chuyen van_chuyen = db.van_chuyen.Find(id);
            db.van_chuyen.Remove(van_chuyen);
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
