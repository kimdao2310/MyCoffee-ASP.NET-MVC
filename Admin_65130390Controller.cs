using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QLMYCF_65130390.Filters;   // ⭐ GIỮ NGUYÊN

namespace QLMYCF_65130390.Controllers
{
    // ⭐ CHỈ SỬA DÒNG NÀY: cho tất cả nhân sự vào Admin Panel
    [AuthorizeRole(Roles = "Admin,Quản lý,Nhân viên")]
    public class Admin_65130390Controller : Controller
    {
        // GET: Admin_65130390
        public ActionResult Index()
        {
            // Nếu chưa đăng nhập → quay lại Login
            if (Session["Quyen"] == null)
            {
                return RedirectToAction("Login", "Auth_65130390");
            }

            // Nếu là KHÁCH HÀNG → không được vào admin
            if (Session["Quyen"].ToString() == "Khách hàng")
            {
                return RedirectToAction("Login", "Auth_65130390");
            }

            // Nếu là Admin / Quản Lý / Nhân Viên → cho vào admin panel
            return View();
        }
    }
}
