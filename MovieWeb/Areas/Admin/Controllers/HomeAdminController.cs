using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MovieWeb.Areas.Admin.ViewModel;
using MovieWeb.Models;
using MovieWeb.Models.Authentication;
using X.PagedList;

namespace MovieWeb.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin")]
    [Authentication("Admin")]
    public class HomeAdminController : Controller
    {
        private readonly MovieDbContext db;

        public HomeAdminController(MovieDbContext context)
        {
            db = context;
        }

        public void anh()
        {
            ViewBag.Anh = HttpContext.Session.GetString("AnhDaiDien");
            ViewBag.Ten = HttpContext.Session.GetString("Admin");
        }

        [Route("danh-sach-phim")]
        public IActionResult DanhSachPhim(int? page)
        {
            anh();
            int pageSize = 5;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;
            var lstphim = db.TPhims.AsNoTracking().OrderBy(x => x.TenPhim);
            PagedList<TPhim> lst = new PagedList<TPhim>(lstphim, pageNumber, pageSize);
            return View(lst);
        }
        [Route("DanhSachTheLoai")]
        public IActionResult DanhSachTheLoai(int matheloai, int? page)
        {
            anh();
            int pageSize = 12;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;
            List<TPhim> lstphim = db.TPhims.Where(x => x.MaTheLoai == matheloai)
                .OrderBy(x => x.TenPhim).ToList();
            PagedList<TPhim> lst = new PagedList<TPhim>(lstphim, pageNumber, pageSize);
            ViewBag.matheloai = matheloai;
            return View(lst);
        }
        [Route("ThemPhimMoi")]
        [HttpGet]
        public IActionResult ThemPhimMoi()
        {
            anh();
            ViewBag.MaHinhThuc = new SelectList(db.HinhThucs.ToList(), "MaHinhThuc", "TenHinhThuc");
            ViewBag.MaTrangThai = new SelectList(db.TrangThais.ToList(), "MaTrangThai", "TenTrangThai");
            ViewBag.MaTheLoai = new SelectList(db.TheLoais.ToList(), "MaTheLoai", "TenTheLoai");
            return View();
        }
        [Route("ThemPhimMoi")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ThemPhimMoi(TPhim phim)
        {

            if (ModelState.IsValid)
            {
                db.TPhims.Add(phim);
                db.SaveChanges();
                return RedirectToAction("DanhSachPhim");
            }
            ViewBag.MaHinhThuc = new SelectList(db.HinhThucs.ToList(), "MaHinhThuc", "TenHinhThuc");
            ViewBag.MaTrangThai = new SelectList(db.TrangThais.ToList(), "MaTrangThai", "TenTrangThai");
            ViewBag.MaTheLoai = new SelectList(db.TheLoais.ToList(), "MaTheLoai", "TenTheLoai");
            anh();
            return View(phim);
        }
        [Route("SuaPhim")]
        [HttpGet]
        public IActionResult SuaPhim(int maPhim)
        {
            anh();
            ViewBag.MaHinhThuc = new SelectList(db.HinhThucs.ToList(), "MaHinhThuc", "TenHinhThuc");
            ViewBag.MaTrangThai = new SelectList(db.TrangThais.ToList(), "MaTrangThai", "TenTrangThai");
            ViewBag.MaTheLoai = new SelectList(db.TheLoais.ToList(), "MaTheLoai", "TenTheLoai");
            var phim = db.TPhims.Find(maPhim);
            return View(phim);
        }
        [Route("SuaPhim")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SuaPhim(TPhim phim)
        {
            if (ModelState.IsValid)
            {
                db.Entry(phim).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("DanhSachPhim", "HomeAdmin");
            }
            ViewBag.MaHinhThuc = new SelectList(db.HinhThucs.ToList(), "MaHinhThuc", "TenHinhThuc");
            ViewBag.MaTrangThai = new SelectList(db.TrangThais.ToList(), "MaTrangThai", "TenTrangThai");
            ViewBag.MaTheLoai = new SelectList(db.TheLoais.ToList(), "MaTheLoai", "TenTheLoai");
            anh();
            return View(phim);
        }

        [Route("XoaPhim")]
        [HttpGet]
        public IActionResult XoaPhim(int maPhim)
        {
            TempData["Message"] = "";
            var episodes = db.Episodes.Where(x => x.MaPhim == maPhim);
            int matapphim = db.Episodes.Where(x => x.MaPhim == maPhim).FirstOrDefault()?.MaTapPhim ?? 0;
            var thoiluongdaxem = db.ThoiLuongDaXems.Where(x => x.MaTapPhim == matapphim);
            if (episodes.Any() && matapphim != 0)
            {
                db.Episodes.RemoveRange(episodes);
                db.ThoiLuongDaXems.RemoveRange(thoiluongdaxem);
            }
            else if (episodes.Any())
            {
                db.Episodes.RemoveRange(episodes);
            }
            var comments = db.Comments.Where(x => x.MaPhim == maPhim);
            if (comments.Any())
            {
                db.Comments.RemoveRange(comments);
            }
            var danhgiachungs = db.DanhGiaChungs.Where(x => x.MaPhim == maPhim);
            if (danhgiachungs.Any())
            {
                db.DanhGiaChungs.RemoveRange(danhgiachungs);
            }
            db.TPhims.Remove(db.TPhims.Find(maPhim));
            db.SaveChanges();
            TempData["Message"] = "Phim đã được xóa";
            return RedirectToAction("DanhSachPhim", "HomeAdmin");

        }

        [Route("danhsachtapphim")]
        public IActionResult DanhSachTapPhim(int? page, int maphim)
        {
            anh();
            int pageSize = 10;
            ViewBag.MaPhim = maphim;
            string tenphim = db.TPhims.Find(maphim).TenPhim;
            ViewBag.TenPhim = tenphim;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;
            var lstTapPhim = db.Episodes.Where(x => x.MaPhim == maphim).OrderBy(x => x.SoTapPhim);
            PagedList<Episode> lst = new PagedList<Episode>(lstTapPhim, pageNumber, pageSize);
            return View(lst);
        }

        [Route("themtapphimmoi")]
        [HttpGet]
        public IActionResult ThemTapPhimMoi(int maphim)
        {
            anh();
            ViewBag.MaPhim = maphim;
            return View();
        }
        [Route("themtapphimmoi")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ThemTapPhimMoi(Episode episode)
        {
            if (ModelState.IsValid)
            {
                db.Episodes.Add(episode);
                db.SaveChanges();
                return RedirectToAction("DanhSachTapPhim", new { maphim = episode.MaPhim });
            }
            anh();
            return View(episode);
        }

        [Route("suatapphim")]
        [HttpGet]
        public IActionResult SuaTapPhim(int maTapPhim)
        {
            anh();
            var episode = db.Episodes.Find(maTapPhim);
            ViewBag.MaPhim = episode.MaPhim;
            return View(episode);
        }
        [Route("suatapphim")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SuaTapPhim(Episode episode)
        {

            if (ModelState.IsValid)
            {
                db.Update(episode);
                db.SaveChanges();
                return RedirectToAction("DanhSachTapPhim", "HomeAdmin", new { maphim = episode.MaPhim });
            }
            anh();
            return View(episode);
        }

        [Route("xoatapphim")]
        [HttpGet]
        public IActionResult XoaTapPhim(int maTapPhim)
        {
            TempData["Message"] = "";
            var episode = db.Episodes.Find(maTapPhim);
            var thoiLuongXemPhim = db.ThoiLuongDaXems.Where(x => x.MaTapPhim == maTapPhim).ToList();
            if (thoiLuongXemPhim != null)
            {
                db.ThoiLuongDaXems.RemoveRange(thoiLuongXemPhim);
            }
            db.Remove(episode);
            db.SaveChanges();
            return RedirectToAction("DanhSachTapPhim", "HomeAdmin", new { maphim = episode.MaPhim });
        }
        [Route("Comment")]
        public IActionResult DanhSachCmt(int maPhim)
        {
            anh();
            string tenphim = db.TPhims.Find(maPhim).TenPhim;
            ViewBag.TenPhim = tenphim;
            var lstComment = db.Comments.Where(x => x.MaPhim == maPhim).ToList();
            List<Comment> tichCuc = new List<Comment>();
            List<Comment> tieuCuc = new List<Comment>();
            foreach (var comment in lstComment)
            {
                var camXuc = SentimentAnalyzer.Sentiments.Predict(comment.NoiDung).Score;
                if (camXuc > 0) tichCuc.Add(comment);
                else tieuCuc.Add(comment);
            }
            ViewComment viewComment = new ViewComment
            {
                tichCuc = tichCuc,
                tieuCuc = tieuCuc
            };
            return View(viewComment);
        }
        [Route("XoaComment")]
        [HttpGet]
        public IActionResult XoaComment(int maComment)
        {
            db.Remove(db.Comments.Find(maComment));
            db.SaveChanges();
            return RedirectToAction("Comment", "HomeAdmin");
        }
    }
}
