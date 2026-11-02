using Microsoft.AspNetCore.Mvc;
using MovieWeb.Models;

namespace MovieWeb.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class TheLoaiController : ControllerBase
    {
        private readonly MovieDbContext db;

        public TheLoaiController(MovieDbContext context)
        {
            db = context;
        }

        [HttpGet("GetAll")]
        public List<TheLoai> GetAll()
        {
            var lst = db.TheLoais.OrderBy(x => x.TenTheLoai).ToList();

            return lst;
        }
        [HttpGet]
        public List<TPhim> GetMovie([FromQuery] int matl)
        {
            var lst = db.TPhims.Where(x => x.MaTheLoai == matl).OrderBy(x => x.TenPhim).ToList();
            return lst;
        }
    }
}
