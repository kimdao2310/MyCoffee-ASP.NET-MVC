using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using QLMYCF_65130390.Models;

namespace QLMYCF_65130390.Controllers
{
    public class Auth_65130390Controller : Controller
    {
        QLMYCF_65130390Entities db = new QLMYCF_65130390Entities();

        //------------------------------------
        // ĐĂNG NHẬP
        //------------------------------------
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            // --- 1) NHÂN VIÊN ---
            var nv = db.NHAN_VIEN.FirstOrDefault(x => x.Email == email && x.MatKhau == password);
            if (nv != null)
            {
                Session["MaNV"] = nv.MaNV;
                Session["TenNguoiDung"] = nv.HoTenNV;
                Session["Email"] = nv.Email;
                Session["Quyen"] = nv.Quyen;

                return RedirectToAction("Index", "Admin_65130390");
            }

            // --- 2) KHÁCH HÀNG ---
            var kh = db.KHACH_HANG.FirstOrDefault(x => x.Email == email && x.MatKhau == password);
            if (kh != null)
            {
                Session["TenNguoiDung"] = kh.HoTen;
                Session["Email"] = kh.Email;
                Session["Quyen"] = "Khách hàng";
                Session["KH"] = kh;
                Session["MaKH"] = kh.MaKH;   // ⭐ FIX GIỎ HÀNG BẮT BUỘC

                // ⭐ Nếu user đang thêm giỏ hàng → trả về đúng trang đó
                if (Session["ReturnUrl"] != null)
                {
                    string url = Session["ReturnUrl"].ToString();
                    Session["ReturnUrl"] = null;
                    return Redirect(url);
                }

                return RedirectToAction("Index", "Home");
            }

            // Sai tài khoản hoặc mật khẩu
            ViewBag.Error = "Email hoặc mật khẩu không đúng!";
            return View();   // ⭐ BẮT BUỘC PHẢI RETURN VIEW
        }

        
        // ĐĂNG KÝ KHÁCH HÀNG
        
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        
        private void SendRegisterMailToAdmin(KHACH_HANG kh)
        {
            var mail = new MailMessage
            {
                From = new MailAddress("ledao231004@gmail.com"),
                Subject = "Khách hàng mới đăng ký",
                IsBodyHtml = true,
                Body = $@"
            <h3>THÔNG TIN KHÁCH HÀNG</h3>
            <p>Họ tên: {kh.HoTen}</p>
            <p>Email: {kh.Email}</p>
            <p>SĐT: {kh.SoDienThoai}</p>
            <p>Địa chỉ: {kh.DiaChi}</p>
            <p>Thời gian: {DateTime.Now}</p>"
            };
            
            mail.To.Add("ledao231004@gmail.com");

            using (var smtp = new SmtpClient("smtp.gmail.com", 587))
            {
                smtp.EnableSsl = true;
                smtp.Credentials = new System.Net.NetworkCredential(
                    "ledao231004@gmail.com",
                    "cvrcnxqphdrrysea" // APP PASSWORD
                );
                smtp.Send(mail);
            }

        }
        [HttpPost]
        public ActionResult Register(KHACH_HANG kh)
        {
            if (ModelState.IsValid)
            {
                // 1. KIỂM TRA TRÙNG EMAIL
                // Dùng Trim() để xóa khoảng trắng thừa nếu có
                var checkEmail = db.KHACH_HANG.FirstOrDefault(x => x.Email == kh.Email.Trim());

                if (checkEmail != null)
                {
                    // Nếu tìm thấy Email -> Báo lỗi
                    ViewBag.Error = "Email đã tồn tại, vui lòng sử dụng Email khác!";
                    return View(kh); // Trả lại View kèm model để không mất dữ liệu đã nhập
                }

                // 2. KHÔNG KIỂM TRA TÊN -> TRÙNG TÊN VẪN LƯU BÌNH THƯỜNG
                try
                {
                    // Lưu vào DB
                    db.KHACH_HANG.Add(kh);
                    db.SaveChanges();

                    // Gửi mail (bỏ vào try catch để lỡ lỗi mail thì vẫn đăng ký được)
                    try
                    {
                        SendRegisterMailToAdmin(kh);
                    }
                    catch { /* Có thể log lỗi mail ở đây nếu cần */ }

                    // Tự đăng nhập sau khi đăng ký thành công
                    Session["KH"] = kh;
                    Session["TenNguoiDung"] = kh.HoTen;
                    Session["Quyen"] = "Khách hàng";
                    Session["Email"] = kh.Email;
                    Session["MaKH"] = kh.MaKH;

                    return RedirectToAction("Index", "Home");
                }
                catch (Exception ex)
                {
                    // Đây là nơi bắt lỗi nếu Database cấu hình sai (VD: set Unique cho cột tên)
                    ViewBag.Error = "Lỗi khi lưu dữ liệu: " + ex.Message;
                    return View(kh);
                }
            }

            return View(kh);
        }


        // ĐĂNG XUẤT

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login", "Auth_65130390");
        }

        
        public ActionResult NoPermission()
        {
            return View();
        }
    }
}
