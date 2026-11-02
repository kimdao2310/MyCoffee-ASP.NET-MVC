using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QLMYCF_65130390.Models;

namespace QLMYCF_65130390.Controllers
{
    public class CART65130390Controller : Controller
    {
        private QLMYCF_65130390Entities db = new QLMYCF_65130390Entities();

        // ==========================
        // THÊM VÀO GIỎ HÀNG
        // ==========================
        public ActionResult AddToCart(string id)
        {
            if (Session["MaKH"] == null)
            {
                Session["ReturnUrl"] = Url.Action("AddToCart", new { id = id });
                return RedirectToAction("Login", "Auth_65130390");
            }

            int maKH = (int)Session["MaKH"];

            // Lấy hóa đơn tạm
            var hd = db.GIO_HANG.FirstOrDefault(x => x.MaKH == maKH && x.TrangThai == 0);

            if (hd == null)
            {
                hd = new GIO_HANG
                {
                    MaKH = maKH,
                    NgayDatHang = DateTime.Now,
                    TrangThai = 0,
                    TongTien = 0
                };
                db.GIO_HANG.Add(hd);
                db.SaveChanges();
            }

            // Lấy chi tiết giỏ
            var ct = db.CHI_TIET_GH.FirstOrDefault(x => x.MaHD == hd.MaHD && x.MaSP == id);

            if (ct == null)
            {
                ct = new CHI_TIET_GH
                {
                    MaHD = hd.MaHD,
                    MaSP = id,
                    SoLuong = 1,
                    DonGia = db.SAN_PHAM.Find(id).DonGia
                };
                db.CHI_TIET_GH.Add(ct);
            }
            else
            {
                ct.SoLuong++;
            }

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // ==========================
        // HIỂN THỊ GIỎ HÀNG
        // ==========================
        public ActionResult Index()
        {
            if (Session["MaKH"] == null)
                return RedirectToAction("Login", "Auth_65130390");

            int maKH = (int)Session["MaKH"];

            var hd = db.GIO_HANG.FirstOrDefault(x => x.MaKH == maKH && x.TrangThai == 0);

            if (hd == null)
                return View(new List<CHI_TIET_GH>());

            var list = db.CHI_TIET_GH.Where(x => x.MaHD == hd.MaHD).ToList();
            return View(list);
        }

        // ==========================
        // CẬP NHẬT
        // ==========================
        [HttpPost]
        public ActionResult Update(string id, int qty)
        {
            if (Session["MaKH"] == null)
                return RedirectToAction("Login", "Auth_65130390");

            int maKH = (int)Session["MaKH"];

            var hd = db.GIO_HANG.FirstOrDefault(x => x.MaKH == maKH && x.TrangThai == 0);
            if (hd == null) return RedirectToAction("Index");

            var item = db.CHI_TIET_GH.FirstOrDefault(x => x.MaHD == hd.MaHD && x.MaSP == id);

            if (item != null)
            {
                if (qty <= 0) db.CHI_TIET_GH.Remove(item);
                else item.SoLuong = qty;

                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        // ==========================
        // XÓA
        // ==========================
        public ActionResult Remove(string id)
        {
            if (Session["MaKH"] == null)
                return RedirectToAction("Login", "Auth_65130390");

            int maKH = (int)Session["MaKH"];

            var hd = db.GIO_HANG.FirstOrDefault(x => x.MaKH == maKH && x.TrangThai == 0);
            if (hd == null) return RedirectToAction("Index");

            var item = db.CHI_TIET_GH.FirstOrDefault(x => x.MaHD == hd.MaHD && x.MaSP == id);

            if (item != null)
            {
                db.CHI_TIET_GH.Remove(item);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        // ==========================
        // THANH TOÁN
        // ==========================
        public ActionResult Checkout()
        {
            if (Session["MaKH"] == null)
                return RedirectToAction("Login", "Auth_65130390");

            int maKH = (int)Session["MaKH"];

            var hd = db.GIO_HANG.FirstOrDefault(x => x.MaKH == maKH && x.TrangThai == 0);
            if (hd == null) return RedirectToAction("Index");

            var list = db.CHI_TIET_GH.Where(x => x.MaHD == hd.MaHD).ToList();
            if (list.Count == 0) return RedirectToAction("Index");

            hd.NgayDatHang = DateTime.Now;
            hd.TongTien = list.Sum(x => x.SoLuong * x.DonGia);
            hd.TrangThai = 0; // Chờ duyệt
            hd.MaNV_DatHang = maKH;

            db.SaveChanges();

            TempData["Success"] = "Đặt hàng thành công!";
            return RedirectToAction("CheckoutSuccess", "CART65130390");


        }
        public ActionResult CheckoutSuccess()
        {
            return View();
        }

        public ActionResult BuyNow(string id)
        {
            if (Session["MaKH"] == null)
            {
                Session["ReturnUrl"] = Url.Action("BuyNow", new { id = id });
                return RedirectToAction("Login", "Auth_65130390");
            }

            int maKH = (int)Session["MaKH"];

            // tìm hoặc tạo hóa đơn tạm
            var hd = db.GIO_HANG.FirstOrDefault(x => x.MaKH == maKH && x.TrangThai == 0);
            if (hd == null)
            {
                hd = new GIO_HANG
                {
                    MaKH = maKH,
                    NgayDatHang = DateTime.Now,
                    TrangThai = 0,
                    TongTien = 0
                };
                db.GIO_HANG.Add(hd);
                db.SaveChanges();
            }

            // thêm sản phẩm hoặc +1
            var ct = db.CHI_TIET_GH.FirstOrDefault(x => x.MaHD == hd.MaHD && x.MaSP == id);
            if (ct == null)
            {
                ct = new CHI_TIET_GH
                {
                    MaHD = hd.MaHD,
                    MaSP = id,
                    SoLuong = 1,
                    DonGia = db.SAN_PHAM.Find(id).DonGia
                };
                db.CHI_TIET_GH.Add(ct);
            }
            else
            {
                ct.SoLuong++;
            }

            db.SaveChanges();

            // ⭐ ĐI THẲNG ĐẾN TRANG THANH TOÁN
            return RedirectToAction("CheckoutPage");

        }
        // ==========================
        // TRANG THANH TOÁN
        // ==========================
        public ActionResult CheckoutPage()
        {
            if (Session["MaKH"] == null)
                return RedirectToAction("Login", "Auth_65130390");

            int maKH = (int)Session["MaKH"];

            var hd = db.GIO_HANG.FirstOrDefault(x => x.MaKH == maKH && x.TrangThai == 0);
            if (hd == null)
                return RedirectToAction("Index");

            var list = db.CHI_TIET_GH.Where(x => x.MaHD == hd.MaHD).ToList();

            ViewBag.TongTien = list.Sum(x => x.SoLuong * x.DonGia);

            return View(list);
        }


        // ==========================
        // XÁC NHẬN ĐẶT HÀNG
        // ==========================
        [HttpPost]
        public ActionResult ConfirmCheckout(string HoTen, string SDT, string DiaChi, string GhiChu)
        {
            if (Session["MaKH"] == null)
                return RedirectToAction("Login", "Auth_65130390");

            int maKH = (int)Session["MaKH"];

            // 1️⃣ Lấy giỏ hàng đang mở
            var hd = db.GIO_HANG.FirstOrDefault(x => x.MaKH == maKH && x.TrangThai == 0);
            if (hd == null)
                return RedirectToAction("Index");

            // 2️⃣ Lấy chi tiết giỏ hàng
            var list = db.CHI_TIET_GH.Where(x => x.MaHD == hd.MaHD).ToList();
            if (!list.Any())
            {
                TempData["Error"] = "Giỏ hàng trống!";
                return RedirectToAction("Index");
            }

            // 3️⃣ Tính tổng tiền
            hd.TongTien = list.Sum(x => x.SoLuong * x.DonGia);

            // 4️⃣ Cập nhật trạng thái đơn
            hd.NgayDatHang = DateTime.Now;
            hd.TrangThai = 1;        // ✅ ĐÃ ĐẶT – CHỜ DUYỆT
            hd.MaNV_DatHang = null;  // ✅ CHƯA CÓ NV XỬ LÝ

            db.SaveChanges();

            // 5️⃣ Cập nhật thông tin KH
            var kh = db.KHACH_HANG.FirstOrDefault(x => x.MaKH == maKH);
            if (kh != null)
            {
                kh.HoTen = HoTen;
                kh.SoDienThoai = SDT;
                kh.DiaChi = DiaChi;
                db.SaveChanges();
            }

            TempData["Success"] = "Đặt hàng thành công!";
            return RedirectToAction("CheckoutSuccess", "CART65130390");
        }



    }
}
