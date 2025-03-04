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
    public class don_hangController : Controller
    {
        private Project_63132701Entities1 db = new Project_63132701Entities1();

        // GET: don_hang
        public ActionResult Index()
        {
            var don_hang = db.don_hang.Include(d => d.khach_hang);
            return View(don_hang.ToList());
        }

        // GET: don_hang/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            don_hang don_hang = db.don_hang.Find(id);
            if (don_hang == null)
            {
                return HttpNotFound();
            }
            return View(don_hang);
        }

        // GET: don_hang/Create
        public ActionResult Create()
        {
            ViewBag.id_khach_hang = new SelectList(db.khach_hang, "id", "ho_ten");
            return View();
        }

        // POST: don_hang/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,id_khach_hang,tong_tien,trang_thai,ngay_tao,ma_van_don")] don_hang don_hang)
        {
            if (ModelState.IsValid)
            {
                db.don_hang.Add(don_hang);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_khach_hang = new SelectList(db.khach_hang, "id", "ho_ten", don_hang.id_khach_hang);
            return View(don_hang);
        }

        // GET: don_hang/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            don_hang don_hang = db.don_hang.Find(id);
            if (don_hang == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_khach_hang = new SelectList(db.khach_hang, "id", "ho_ten", don_hang.id_khach_hang);
            return View(don_hang);
        }

        // POST: don_hang/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,id_khach_hang,tong_tien,trang_thai,ngay_tao,ma_van_don")] don_hang don_hang)
        {
            if (ModelState.IsValid)
            {
                db.Entry(don_hang).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_khach_hang = new SelectList(db.khach_hang, "id", "ho_ten", don_hang.id_khach_hang);
            return View(don_hang);
        }

        // GET: don_hang/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            don_hang don_hang = db.don_hang.Find(id);
            if (don_hang == null)
            {
                return HttpNotFound();
            }
            return View(don_hang);
        }

        // POST: don_hang/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            don_hang don_hang = db.don_hang.Find(id);
            db.don_hang.Remove(don_hang);
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
