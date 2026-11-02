using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using QLMYCF_65130390.Models;

namespace QLMYCF_65130390.Controllers
{
    public class HomeController : Controller
    {
        private QLMYCF_65130390Entities db = new QLMYCF_65130390Entities();

        public ActionResult Index()
        {
            // Banner đang hoạt động
            var banners = db.QUANG_CAO
                .Where(x => x.TrangThai == 1 &&
                            (x.NgayBatDau == null || x.NgayBatDau <= System.DateTime.Now) &&
                            (x.NgayKetThuc == null || x.NgayKetThuc >= System.DateTime.Now))
                .OrderByDescending(x => x.MaQC)
                .ToList();

           
            var sanPhamMoi = db.SAN_PHAM
                                .OrderByDescending(x => x.MaSP)
                                .Take(8)
                                .ToList();

            // Tin tức mới nhất
            var tinTucMoi = db.TIN_TUC
                               .Where(x => x.TrangThai == 1)
                               .OrderByDescending(x => x.NgayDang)
                               .Take(4)
                               .ToList();

            // Gửi sang ViewModel
            var vm = new HomeViewModel
            {
                Banners = banners,
                SanPhamMoi = sanPhamMoi,
                TinTucMoi = tinTucMoi
            };

            return View(vm);

        }
        public ActionResult About(string search, string loai, string sort)
        {
            var sp = db.SAN_PHAM.AsQueryable();

            if (!string.IsNullOrEmpty(search))
                sp = sp.Where(x => x.TenSP.Contains(search));

            if (!string.IsNullOrEmpty(loai))
                sp = sp.Where(x => x.MaLSP == loai);

            switch (sort)
            {
                case "price_asc": sp = sp.OrderBy(x => x.DonGia); break;
                case "price_desc": sp = sp.OrderByDescending(x => x.DonGia); break;
                default: sp = sp.OrderByDescending(x => x.MaSP); break;
            }

            ViewBag.LoaiSP = db.LOAI_SAN_PHAM.ToList();

            return View(sp.ToList());
        }


        public ActionResult ChiTietSanPham(string id)
        {
            if (id == null)
                return HttpNotFound();

            var sp = db.SAN_PHAM.Find(id);

            if (sp == null)
                return HttpNotFound();

            return View(sp);
        }

        public ActionResult Search(string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
                return View(new List<SAN_PHAM>());

            var result = db.SAN_PHAM
                           .Where(x => x.TenSP.Contains(keyword) || x.MoTaSP.Contains(keyword))
                           .ToList();

            ViewBag.Keyword = keyword;

            return View(result);
        }

        public ActionResult Contact()
        {
            var tin = db.TIN_TUC
                        .Where(x => x.TrangThai == 1)
                        .OrderByDescending(x => x.NgayDang)
                        .ToList();

            return View(tin);
        }



        public ActionResult ChiTietTinTuc(int id)
        {
            var tt = db.TIN_TUC.FirstOrDefault(x => x.MaTT == id);

            if (tt == null)
                return HttpNotFound();

            return View(tt);
        }
        // GET: /Home/LienHe
        public ActionResult LienHe()
        {
            return View();
        }


        public ActionResult GuiLienHe(string HoTen, string Email, string NoiDung)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("dao.ltk.65cntt@ntu.edu.vn");
                mail.To.Add("dao.ltk.65cntt@ntu.edu.vn");
                mail.Subject = "📩 LIÊN HỆ TỪ KHÁCH HÀNG";

                mail.Body =
                     "Họ và tên: " + HoTen + "\n" +
                     "Email: " + Email + "\n\n" +
                     "NỘI DUNG:\n" + NoiDung;

                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                smtp.Credentials = new NetworkCredential(
                    "dao.ltk.65cntt@ntu.edu.vn",
                    "kdzt fcju ahdt zzhy"
                );
                smtp.EnableSsl = true;

                smtp.Send(mail);

                TempData["msg"] = "Gửi liên hệ thành công cảm ơn bạn đã gửi!";
            }
            catch
            {
                TempData["msg"] = "Gửi liên hệ thất bại!";
            }

            return RedirectToAction("LienHe");
        }
        public ActionResult LichSuDonHang()
        {
            if (Session["MaKH"] == null)
                return RedirectToAction("Login", "Auth_65130390");

            int maKH = (int)Session["MaKH"];

            var list = db.GIO_HANG
                         .Where(x => x.MaKH == maKH && x.TrangThai >= 0) 
                         .OrderByDescending(x => x.MaHD)
                         .ToList();

            return View(list);
        }
        public ActionResult ChiTietDonHang(int id)
        {
            if (Session["MaKH"] == null)
                return RedirectToAction("Login", "Auth_65130390");

            var hd = db.GIO_HANG
                       .Include("CHI_TIET_GH")
                       .Include("CHI_TIET_GH.SAN_PHAM")
                       .FirstOrDefault(x => x.MaHD == id);

            if (hd == null) return HttpNotFound();

            // Không cho xem đơn của người khác
            if (hd.MaKH != (int)Session["MaKH"])
                return RedirectToAction("LichSuDonHang");

            return View(hd);
        }


    }



}