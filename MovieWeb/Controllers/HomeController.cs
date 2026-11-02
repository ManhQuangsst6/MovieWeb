using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using MovieWeb.Models;
using MovieWeb.ViewModels;
using System.Diagnostics;

namespace MovieWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly MovieDbContext db;

        public HomeController(ILogger<HomeController> logger, MovieDbContext context)
        {
            _logger = logger;
            db = context;
        }

        public IActionResult Index()
        {
            string connectionString = db.Database.GetConnectionString()!;

            using SqlConnection sql = new SqlConnection(connectionString);
            sql.Open();
            using SqlCommand sqlCommand = new SqlCommand("select * from moviewnew", sql);
            var lst = sqlCommand.ExecuteReader();
            List<Film> filmNew = new List<Film>();
            if (lst.HasRows)
            {
                while (lst.Read())
                {
                    filmNew.Add(new Film(lst.GetInt32(0), lst.GetString(1), lst.GetString(2),
                        lst.GetString(3), lst.GetInt32(4)));
                }
            }
            FilmHome filmHome = new FilmHome()
            {
                filmNew = filmNew
            };
            sql.Close();
            sql.Open();
            sqlCommand.CommandText = "select * from Top5MovieByView";
            var top = sqlCommand.ExecuteReader();
            List<Film> topFilm = new List<Film>();
            if (top.HasRows)
            {
                while (top.Read())
                {
                    topFilm.Add(new Film(top.GetInt32(0), top.GetString(1), top.GetString(2),
                        top.GetString(3), top.GetInt32(4)));
                }
            }
            filmHome.topFilm = topFilm;

            return View(filmHome);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}