using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QLMYCF_65130390.Models;
using QLMYCF_65130390.Filters;   // ⭐ THÊM

namespace QLMYCF_65130390.Controllers
{
    // ⭐ PHÂN QUYỀN: ADMIN + QUẢN LÝ
    [AuthorizeRole(Roles = "Admin,Quản lý")]
    public class NHAN_VIEN65130390Controller : Controller
    {
        QLMYCF_65130390Entities db = new QLMYCF_65130390Entities();

        // ===== GIỮ NGUYÊN LOGIC CŨ =====
        private bool CheckAdmin()
        {
            if (Session["Quyen"] == null) return false;

            string role = Session["Quyen"].ToString();

            // Chỉ Admin và Quản lý được truy cập
            return role == "Admin"  ||  role == "Quản lý";
        }

        // =================== DANH SÁCH ===================
        public ActionResult Index()
        {
            if (!CheckAdmin()) return RedirectToAction("Login", "Auth_65130390");
            return View(db.NHAN_VIEN.ToList());
        }

        // =================== CHI TIẾT ===================
        public ActionResult Details(int id)
        {
            if (!CheckAdmin()) return RedirectToAction("Login", "Auth_65130390");
            return View(db.NHAN_VIEN.Find(id));
        }

        // =================== THÊM ===================
        public ActionResult Create()
        {
            if (!CheckAdmin()) return RedirectToAction("Login", "Auth_65130390");
            return View();
        }

        [HttpPost]
        public ActionResult Create(NHAN_VIEN nv)
        {
            if (ModelState.IsValid)
            {
                db.NHAN_VIEN.Add(nv);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(nv);
        }

        // =================== SỬA ===================
        public ActionResult Edit(int id)
        {
            if (!CheckAdmin()) return RedirectToAction("Login", "Auth_65130390");
            return View(db.NHAN_VIEN.Find(id));
        }

        [HttpPost]
        public ActionResult Edit(NHAN_VIEN nv)
        {
            if (ModelState.IsValid)
            {
                var old = db.NHAN_VIEN.Find(nv.MaNV);

                old.HoTenNV = nv.HoTenNV;
                old.Email = nv.Email;
                old.SoDienThoai = nv.SoDienThoai;
                old.DiaChi = nv.DiaChi;
                old.Quyen = nv.Quyen;
                old.NgayVaoLam = nv.NgayVaoLam;
                old.MatKhau = nv.MatKhau;

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(nv);
        }

        // =================== XÓA ===================
        public ActionResult Delete(int id)
        {
            if (!CheckAdmin()) return RedirectToAction("Login", "Auth_65130390");
            return View(db.NHAN_VIEN.Find(id));
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var nv = db.NHAN_VIEN.Find(id);
            db.NHAN_VIEN.Remove(nv);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
