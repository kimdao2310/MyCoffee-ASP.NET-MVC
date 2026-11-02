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
    // ⭐ ADMIN + QUẢN LÝ + NHÂN VIÊN (để NV xem Index, Details)
    [AuthorizeRole(Roles = "Admin,Quản lý,Nhân viên")]
    public class SAN_PHAM65130390Controller : Controller
    {
        QLMYCF_65130390Entities db = new QLMYCF_65130390Entities();

        // ⭐ CHỈ ADMIN + QUẢN LÝ mới được CRUD
        private bool CheckAdmin()
        {
            if (Session["Quyen"] == null) return false;

            string role = Session["Quyen"].ToString();
            return role == "Admin" || role == "Quản lý";
        }

        // ================== 1) DANH SÁCH ==================
        public ActionResult Index()
        {
            var sp = db.SAN_PHAM.Include(s => s.LOAI_SAN_PHAM).ToList();
            return View(sp);
        }

        public ActionResult Details(string id)
        {
            if (string.IsNullOrEmpty(id))
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var sp = db.SAN_PHAM.Find(id);
            if (sp == null)
                return HttpNotFound();

            return View(sp);
        }

        // ================== 2) THÊM ==================
        [HttpGet]
        public ActionResult Create()
        {
            if (!CheckAdmin()) return RedirectToAction("NoPermission", "Auth_65130390");

            ViewBag.MaLSP = new SelectList(db.LOAI_SAN_PHAM, "MaLSP", "TenLSP");
            return View();
        }

        [HttpPost]
        public ActionResult Create(SAN_PHAM sp, HttpPostedFileBase AnhSanPham)
        {
            if (!CheckAdmin()) return RedirectToAction("NoPermission", "Auth_65130390");

            if (AnhSanPham != null)
            {
                string fileName = System.IO.Path.GetFileName(AnhSanPham.FileName);
                string path = "/Images/" + fileName;
                AnhSanPham.SaveAs(Server.MapPath(path));
                sp.AnhSanPham = fileName;
            }

            db.SAN_PHAM.Add(sp);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        // ================== 3) SỬA ==================
        [HttpGet]
        public ActionResult Edit(string id)
        {
            if (!CheckAdmin()) return RedirectToAction("NoPermission", "Auth_65130390");

            var sp = db.SAN_PHAM.Find(id);
            ViewBag.MaLSP = new SelectList(db.LOAI_SAN_PHAM, "MaLSP", "TenLSP", sp.MaLSP);
            return View(sp);
        }

        [HttpPost]
        public ActionResult Edit(SAN_PHAM sp, HttpPostedFileBase AnhSanPham)
        {
            if (!CheckAdmin()) return RedirectToAction("NoPermission", "Auth_65130390");

            var spCu = db.SAN_PHAM.Find(sp.MaSP);

            spCu.TenSP = sp.TenSP;
            spCu.MoTaSP = sp.MoTaSP;
            spCu.DonGia = sp.DonGia;
            spCu.DonViTinh = sp.DonViTinh;
            spCu.MaLSP = sp.MaLSP;

            if (AnhSanPham != null)
            {
                string fileName = System.IO.Path.GetFileName(AnhSanPham.FileName);
                string path = "/Images/" + fileName;
                AnhSanPham.SaveAs(Server.MapPath(path));
                spCu.AnhSanPham = fileName;
            }

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // ================== 4) XÓA ==================
        public ActionResult Delete(string id)
        {
            if (!CheckAdmin()) return RedirectToAction("NoPermission", "Auth_65130390");

            var sp = db.SAN_PHAM.Find(id);
            db.SAN_PHAM.Remove(sp);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
