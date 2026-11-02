using Microsoft.AspNetCore.Mvc;
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
        public async Task<List<TPhim>> Get(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return new List<TPhim>();
            }

            return await db.TPhims
                .AsNoTracking()
                .Where(movie => movie.TenPhim != null && movie.TenPhim.Contains(name))
                .OrderBy(movie => movie.TenPhim)
                .ToListAsync();
        }
    }
}
