using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QLMYCF_65130390.Models;

namespace QLMYCF_65130390.Controllers
{
    public class LOAI_SAN_PHAM65130390Controller : Controller
    {
        private QLMYCF_65130390Entities db = new QLMYCF_65130390Entities();

        // GET: LOAI_SAN_PHAM65130390
        private bool CheckAdmin()
        {
            return Session["Quyen"] != null && Session["Quyen"].ToString() != "Khách hàng";
        }

        // ========== INDEX ==========
        public ActionResult Index()
        {
            if (!CheckAdmin()) return RedirectToAction("Login", "Auth_65130390");

            var loai = db.LOAI_SAN_PHAM.ToList();
            return View(loai);
        }

        // GET: LOAI_SAN_PHAM65130390/Details/5
        public ActionResult Details(string id)
        {
            if (!CheckAdmin()) return RedirectToAction("Login", "Auth_65130390");

            var lsp = db.LOAI_SAN_PHAM.Find(id);
            return View(lsp);
        }

        // GET: LOAI_SAN_PHAM65130390/Create
        public ActionResult Create()
        {
            if (!CheckAdmin()) return RedirectToAction("Login", "Auth_65130390");
            return View();
        }

        // POST: LOAI_SAN_PHAM65130390/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaLSP,TenLSP")] LOAI_SAN_PHAM lOAI_SAN_PHAM)
        {
            if (ModelState.IsValid)
            {
                db.LOAI_SAN_PHAM.Add(lOAI_SAN_PHAM);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(lOAI_SAN_PHAM);
        }

        // GET: LOAI_SAN_PHAM65130390/Edit/5
        public ActionResult Edit(string id)
        {
            if (!CheckAdmin()) return RedirectToAction("Login", "Auth_65130390");

            var lsp = db.LOAI_SAN_PHAM.Find(id);
            return View(lsp);
        }

        // POST: LOAI_SAN_PHAM65130390/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaLSP,TenLSP")] LOAI_SAN_PHAM lOAI_SAN_PHAM)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lOAI_SAN_PHAM).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(lOAI_SAN_PHAM);
        }

        // GET: LOAI_SAN_PHAM65130390/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LOAI_SAN_PHAM lOAI_SAN_PHAM = db.LOAI_SAN_PHAM.Find(id);
            if (lOAI_SAN_PHAM == null)
            {
                return HttpNotFound();
            }
            return View(lOAI_SAN_PHAM);
        }

        // POST: LOAI_SAN_PHAM65130390/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            LOAI_SAN_PHAM lOAI_SAN_PHAM = db.LOAI_SAN_PHAM.Find(id);
            db.LOAI_SAN_PHAM.Remove(lOAI_SAN_PHAM);
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
