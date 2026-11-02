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
    [AuthorizeRole(Roles = "Quản lý,Admin")]
    public class QUANG_CAO65130390Controller : Controller
    {
        private QLMYCF_65130390Entities db = new QLMYCF_65130390Entities();

        // ===== GIỮ NGUYÊN LOGIC CŨ =====
        private bool CheckAdmin()
        {
            return Session["Quyen"] != null && Session["Quyen"].ToString() != "Khách hàng";
        }

        // ================= INDEX =================
        public ActionResult Index()
        {
            if (!CheckAdmin()) return RedirectToAction("Login", "Auth_65130390");

            var qc = db.QUANG_CAO.OrderByDescending(x => x.MaQC).ToList();
            return View(qc);
        }

        // ================= DETAILS =================
        public ActionResult Details(int id)
        {
            if (!CheckAdmin()) return RedirectToAction("Login", "Auth_65130390");

            var qc = db.QUANG_CAO.Find(id);
            return View(qc);
        }

        // ================= CREATE =================
        [HttpGet]
        public ActionResult Create()
        {
            if (!CheckAdmin()) return RedirectToAction("Login", "Auth_65130390");
            return View();
        }

        [HttpPost]
        public ActionResult Create(QUANG_CAO qc, HttpPostedFileBase HinhAnh)
        {
            if (HinhAnh != null)
            {
                string file = System.IO.Path.GetFileName(HinhAnh.FileName);
                HinhAnh.SaveAs(Server.MapPath("~/Images/" + file));
                qc.HinhAnh = file;
            }

            qc.MaNV = (int?)Session["MaNV"];
            db.QUANG_CAO.Add(qc);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        // ================= EDIT =================
        [HttpGet]
        public ActionResult Edit(int id)
        {
            if (!CheckAdmin()) return RedirectToAction("Login", "Auth_65130390");

            var qc = db.QUANG_CAO.Find(id);
            return View(qc);
        }

        [HttpPost]
        public ActionResult Edit(QUANG_CAO qc, HttpPostedFileBase HinhAnh)
        {
            var old = db.QUANG_CAO.Find(qc.MaQC);

            old.TieuDe = qc.TieuDe;
            old.NoiDung = qc.NoiDung;
            old.Link = qc.Link;
            old.NgayBatDau = qc.NgayBatDau;
            old.NgayKetThuc = qc.NgayKetThuc;
            old.TrangThai = qc.TrangThai;

            if (HinhAnh != null)
            {
                string file = System.IO.Path.GetFileName(HinhAnh.FileName);
                HinhAnh.SaveAs(Server.MapPath("~/Images/" + file));
                old.HinhAnh = file;
            }

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // ================= DELETE =================
        public ActionResult Delete(int id)
        {
            var qc = db.QUANG_CAO.Find(id);
            return View(qc);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var qc = db.QUANG_CAO.Find(id);
            db.QUANG_CAO.Remove(qc);
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
