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
    // ⭐ PHÂN QUYỀN: ADMIN + QUẢN LÝ (KHÔNG CHO KHÁCH HÀNG)
    [AuthorizeRole(Roles = "Admin,Quản lý,Nhân viên")]

    public class GIO_HANG65130390Controller : Controller
    {
        private QLMYCF_65130390Entities db = new QLMYCF_65130390Entities();

        // ===== GIỮ NGUYÊN LOGIC CŨ =====
        private bool CheckAdmin()
        {
            return Session["Quyen"] != null && Session["Quyen"].ToString() != "Khách hàng";
        }

        // =================== DANH SÁCH ĐƠN HÀNG ===================
        public ActionResult Index()
        {
            if (!CheckAdmin()) return RedirectToAction("Login", "Auth_65130390");

            var list = db.GIO_HANG.OrderByDescending(h => h.MaHD).ToList();
            return View(list);
        }

        // =================== CHI TIẾT ĐƠN HÀNG ===================
        public ActionResult Details(int id)
        {
            var hd = db.GIO_HANG
                .Include("KHACH_HANG")
                .Include("NHAN_VIEN")
                .Include("NHAN_VIEN1")
                .Include("NHAN_VIEN2")
                .Include("CHI_TIET_GH")
                .Include("CHI_TIET_GH.SAN_PHAM")
                .FirstOrDefault(x => x.MaHD == id);

            if (hd == null)
                return HttpNotFound();

            return View(hd);
        }

        // =================== DUYỆT ĐƠN ===================
        public ActionResult DuyetDon(int id)
        {
            var hd = db.GIO_HANG.Find(id);
            if (hd != null)
            {
                hd.TrangThai = 1; // Đã duyệt
                hd.MaNV_Duyet = (int)Session["MaNV"];
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        // =================== GIAO HÀNG ===================
        public ActionResult GiaoHang(int id)
        {
            var hd = db.GIO_HANG.Find(id);
            if (hd != null)
            {
                hd.TrangThai = 2; // Đang giao
                hd.MaNV_GiaoHang = (int)Session["MaNV"];
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        // =================== HOÀN TẤT ===================
        public ActionResult HoanTat(int id)
        {
            var hd = db.GIO_HANG.Find(id);
            if (hd != null)
            {
                hd.TrangThai = 3; // Hoàn tất
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        // =================== XÓA ĐƠN ===================
        // =================== XÓA ĐƠN ===================
        public ActionResult Delete(int id)
        {
            // ❌ NHÂN VIÊN KHÔNG ĐƯỢC XÓA
            if (Session["Quyen"]?.ToString() == "Nhân viên")
            {
                return RedirectToAction("NoPermission", "Auth_65130390");
            }

            var hd = db.GIO_HANG.Find(id);

            if (hd == null)
                return HttpNotFound();

            var chitiet = db.CHI_TIET_GH.Where(x => x.MaHD == id).ToList();
            if (chitiet.Any())
            {
                db.CHI_TIET_GH.RemoveRange(chitiet);
            }

            db.GIO_HANG.Remove(hd);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

    }
}
