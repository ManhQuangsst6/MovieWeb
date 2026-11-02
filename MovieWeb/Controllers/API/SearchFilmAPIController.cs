using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MovieWeb.Models;

namespace MovieWeb.Controllers.APIController
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchFilmController : ControllerBase
    {
        private readonly MovieDbContext db;

        public SearchFilmController(MovieDbContext context)
        {
            db = context;
        }

        [HttpGet()]
        public async Task<List<TPhim>> get(string name)
        {
            var val = new SqlParameter("@name", System.Data.SqlDbType.NVarChar);
            val.Value = name;
            var list = await db.TPhims.FromSqlRaw("exec searchMovie @name", val).ToListAsync();

            return list;
        }
    }
}
