using Microsoft.AspNetCore.Mvc;
using MovieWeb.Models;
using MovieWeb.Models.Authentication;

namespace MovieWeb.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin")]
    [Authentication("Admin")]
    public class UserController : Controller
    {
        private readonly MovieDbContext db;

        public UserController(MovieDbContext context)
        {
            db = context;
        }

        public void anh()
        {
            ViewBag.Anh = HttpContext.Session.GetString("AnhDaiDien");
            ViewBag.Ten = HttpContext.Session.GetString("Admin");
        }
        [Route("list-User")]

        public IActionResult DanhSachUser()
        {
            anh();
            var lstUser = db.TaiKhoans.Where(x => x.IsDeleted == 0 &&
            x.PhanQuyen == 1).OrderBy(x => x.TenTaiKhoan).ToList();
            return View(lstUser);
        }
        [HttpGet("Khoa")]
        public IActionResult KhoaTK(int userCode)
        {
            TaiKhoan user = db.TaiKhoans.Find(userCode);
            user.IsDeleted = 1;
            DateTime update = DateTime.Now;
            user.DateUpdate = update;
            db.Update(user);
            db.SaveChanges();
            return RedirectToAction("DanhSachUser");
        }
    }
}
