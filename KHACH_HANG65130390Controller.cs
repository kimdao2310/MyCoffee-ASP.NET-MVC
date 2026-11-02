using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QLMYCF_65130390.Models;
using QLMYCF_65130390.Filters;

namespace QLMYCF_65130390.Controllers
{
    // ⭐ ADMIN + QUẢN LÝ + NHÂN VIÊN (NV chỉ xem Details)
    [AuthorizeRole(Roles = "Admin,Quản lý,Nhân viên")]
    public class KHACH_HANG65130390Controller : Controller
    {
        private QLMYCF_65130390Entities db = new QLMYCF_65130390Entities();

        // ⭐ CHỈ ADMIN + QUẢN LÝ được quản lý
        private bool CheckAdmin()
        {
            if (Session["Quyen"] == null) return false;

            string role = Session["Quyen"].ToString();
            return role == "Admin" || role == "Quản lý";
        }

        // ================== DANH SÁCH ==================
        public ActionResult Index()
        {
            if (!CheckAdmin()) return RedirectToAction("NoPermission", "Auth_65130390");

            var kh = db.KHACH_HANG.ToList();
            return View(kh);
        }

        // ================== CHI TIẾT ==================
        public ActionResult Details(int id)
        {
            if (Session["Quyen"] == null)
                return RedirectToAction("Login", "Auth_65130390");

            var kh = db.KHACH_HANG.Find(id);
            return View(kh);
        }

        // ================== SỬA ==================
        public ActionResult Edit(int id)
        {
            if (!CheckAdmin()) return RedirectToAction("NoPermission", "Auth_65130390");

            var kh = db.KHACH_HANG.Find(id);
            return View(kh);
        }

        [HttpPost]
        public ActionResult Edit(KHACH_HANG kh)
        {
            if (!CheckAdmin()) return RedirectToAction("NoPermission", "Auth_65130390");

            if (ModelState.IsValid)
            {
                var old = db.KHACH_HANG.Find(kh.MaKH);

                old.HoTen = kh.HoTen;
                old.Email = kh.Email;
                old.SoDienThoai = kh.SoDienThoai;
                old.DiaChi = kh.DiaChi;

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(kh);
        }

        // ================== XÓA ==================
        public ActionResult Delete(int id)
        {
            if (!CheckAdmin()) return RedirectToAction("NoPermission", "Auth_65130390");

            var kh = db.KHACH_HANG.Find(id);
            return View(kh);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!CheckAdmin()) return RedirectToAction("NoPermission", "Auth_65130390");

            var kh = db.KHACH_HANG.Find(id);
            db.KHACH_HANG.Remove(kh);
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
