using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QLMYCF_65130390.Models;
using QLMYCF_65130390.Filters;   // ⭐ THÊM

namespace QLMYCF_65130390.Controllers
{
    // ⭐ PHÂN QUYỀN: ADMIN + QUẢN LÝ
    [AuthorizeRole(Roles = "Quản lý, Admin")]
    public class TIN_TUC65130390Controller : Controller
    {
        private QLMYCF_65130390Entities db = new QLMYCF_65130390Entities();

        // ===== GIỮ NGUYÊN LOGIC CŨ =====
        private bool CheckAdmin()
        {
            return Session["Quyen"] != null && Session["Quyen"].ToString() != "Khách hàng";
        }

        // ========== INDEX ==========
        public ActionResult Index()
        {
            if (!CheckAdmin()) return RedirectToAction("Login", "Auth_65130390");

            var list = db.TIN_TUC.OrderByDescending(x => x.MaTT).ToList();
            return View(list);
        }

        // ========== DETAILS ==========
        public ActionResult Details(int id)
        {
            if (!CheckAdmin()) return RedirectToAction("Login", "Auth_65130390");

            var tt = db.TIN_TUC.Find(id);
            return View(tt);
        }

        // ========== CREATE ==========
        public ActionResult Create()
        {
            if (!CheckAdmin()) return RedirectToAction("Login", "Auth_65130390");
            return View();
        }

        [HttpPost]
        public ActionResult Create(TIN_TUC tt, HttpPostedFileBase AnhDaiDien)
        {
            if (!CheckAdmin()) return RedirectToAction("Login", "Auth_65130390");

            if (AnhDaiDien != null)
            {
                string file = System.IO.Path.GetFileName(AnhDaiDien.FileName);
                AnhDaiDien.SaveAs(Server.MapPath("~/Images/" + file));
                tt.AnhDaiDien = file;
            }

            tt.NgayDang = DateTime.Now;
            tt.MaNV = (int?)Session["MaNV"];

            db.TIN_TUC.Add(tt);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        // ========== EDIT ==========
        public ActionResult Edit(int id)
        {
            if (!CheckAdmin()) return RedirectToAction("Login", "Auth_65130390");

            var tt = db.TIN_TUC.Find(id);
            return View(tt);
        }

        [HttpPost]
        public ActionResult Edit(TIN_TUC tt, HttpPostedFileBase AnhDaiDien)
        {
            if (!CheckAdmin()) return RedirectToAction("Login", "Auth_65130390");

            var old = db.TIN_TUC.Find(tt.MaTT);

            old.TieuDe = tt.TieuDe;
            old.NoiDung = tt.NoiDung;
            old.TrangThai = tt.TrangThai;

            if (AnhDaiDien != null)
            {
                string file = System.IO.Path.GetFileName(AnhDaiDien.FileName);
                AnhDaiDien.SaveAs(Server.MapPath("~/Images/" + file));
                old.AnhDaiDien = file;
            }

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // ========== DELETE ==========
        public ActionResult Delete(int id)
        {
            var tt = db.TIN_TUC.Find(id);
            return View(tt);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var tt = db.TIN_TUC.Find(id);
            db.TIN_TUC.Remove(tt);
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
