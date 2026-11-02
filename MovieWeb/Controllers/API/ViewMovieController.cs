using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using MovieWeb.Models.API;
using MovieWeb.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace MovieWeb.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ViewMovieController : ControllerBase
    {
        private readonly MovieDbContext db;

        public ViewMovieController(MovieDbContext context)
        {
            db = context;
        }

        [HttpGet]
        public async Task<StringResult> GetView([FromQuery] int maphim)
        {
            string connectionString = db.Database.GetConnectionString()!;

            using SqlConnection sql = new SqlConnection(connectionString);
            sql.Open();
            using SqlCommand sqlCommand = new SqlCommand("ViewMovie", sql);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@maphim", maphim);
            var num = await sqlCommand.ExecuteReaderAsync();
            int cmt = 0;
            if (num.HasRows)
            {
                num.Read();
                cmt = num.GetInt32(0);
            }
            return new StringResult(cmt.ToString());
        }
        public async Task<StringResult> Put([FromQuery] int matapphim)
        {
            string connectionString = db.Database.GetConnectionString()!;

            using SqlConnection sql = new SqlConnection(connectionString);
            sql.Open();
            using SqlCommand sqlCommand = new SqlCommand("UpView", sql);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@matapphim", matapphim);
            await sqlCommand.ExecuteNonQueryAsync();

            return new StringResult(matapphim.ToString());
        }
    }
}
