using Microsoft.AspNetCore.Mvc;
using MovieWeb.Common;
using MovieWeb.Models;

namespace MovieWeb.Controllers
{
    public class AccessController : Controller
    {
        private readonly MovieDbContext db;

        public AccessController(MovieDbContext context)
        {
            db = context;
        }

        [HttpGet]
        public IActionResult Login()
        {


            if (HttpContext.Session.GetString("UserName") != null)
            {
                return RedirectToAction("index", "home");
            }
            if (HttpContext.Session.GetString("Admin") != null)
            {
                return RedirectToAction("DanhSachPhim", "admin");
            }
            return View();
        }

        [HttpPost]
        public IActionResult Login(TaiKhoan user)
        {
            if (ModelState.IsValid)
            {
                if (HttpContext.Session.GetString("UserName") == null && HttpContext.Session.GetString("Admin") == null)
                {
                    var pass = MaHoa.GetMd5Hash16(user.PassWord);

                    var obj = db.TaiKhoans.Where(x => x.TenTaiKhoan == user.TenTaiKhoan &&
                    x.PassWord == pass).FirstOrDefault();
                    if (obj != null)
                    {
                        if (obj.PhanQuyen == 1)
                        {
                            HttpContext.Session.SetInt32("id", obj.MaTaiKhoan);
                            HttpContext.Session.SetString("UserName", obj.TenTaiKhoan.ToString());
                            HttpContext.Session.SetString("AnhDaiDien", obj.Anh != null ? obj.Anh.ToString() : "https://antimatter.vn/wp-content/uploads/2022/11/anh-avatar-trang-fb-mac-dinh.jpg");

                            return RedirectToAction("index", "home");
                        }
                        if (obj.PhanQuyen == 2)
                        {
                            HttpContext.Session.SetString("Admin", obj.TenTaiKhoan.ToString());
                            HttpContext.Session.SetString("AnhDaiDien", obj.Anh != null ? obj.Anh.ToString() : "https://antimatter.vn/wp-content/uploads/2022/11/anh-avatar-trang-fb-mac-dinh.jpg");
                            return RedirectToAction("Danh-Sach-Phim", "admin");
                        }

                    }
                }

            }
            return View();
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            HttpContext.Session.Remove("UserName");
            HttpContext.Session.Remove("Admin");
            return RedirectToAction("Index", "Home");

        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(TaiKhoan taiKhoan)
        {
            if (ModelState.IsValid)
            {
                var check = db.TaiKhoans.FirstOrDefault(x => x.TenTaiKhoan == taiKhoan.TenTaiKhoan);
                if (check == null)
                {
                    taiKhoan.PassWord = MaHoa.GetMd5Hash16(taiKhoan.PassWord);
                    taiKhoan.IsDeleted = 0;
                    taiKhoan.PhanQuyen = 1;
                    taiKhoan.Anh = "https://antimatter.vn/wp-content/uploads/2022/11/anh-avatar-trang-fb-mac-dinh.jpg";
                    db.TaiKhoans.Add(taiKhoan);
                    db.SaveChanges();
                    return RedirectToAction("LogIn");
                }
                else
                {
                    ViewBag.Error = "Tên tài khoản đã được sử dụng!Vui lòng nhập 1 tên khác";
                    return View();
                }
            }
            return View();
        }
    }
}
