using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using MovieWeb.Models;
using MovieWeb.Models.API;
using System.Data;

namespace MovieWeb.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoteController : ControllerBase
    {
        private readonly MovieDbContext db;

        public VoteController(MovieDbContext context)
        {
            db = context;
        }

        [HttpGet]
        public async Task<StringResult> GetVote([FromQuery] int maphim)
        {
            string connectionString = db.Database.GetConnectionString()!;

            using SqlConnection sql = new SqlConnection(connectionString);
            sql.Open();
            using SqlCommand sqlCommand = new SqlCommand("numVote", sql);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@maphim", maphim);
            var num = await sqlCommand.ExecuteReaderAsync();
            int cmt = 0;
            if (num.HasRows)
            {
                await num.ReadAsync();
                cmt = num.IsDBNull(0) ? 0 : num.GetInt32(0);
            }
            return new StringResult(cmt.ToString());
        }
        [HttpGet("user")]
        public async Task<StringResult> GetVoteUser([FromQuery] int maphim, [FromQuery] int user)
        {
            string connectionString = db.Database.GetConnectionString()!;

            using SqlConnection sql = new SqlConnection(connectionString);
            sql.Open();
            using SqlCommand sqlCommand = new SqlCommand("numVoteUser", sql);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@maphim", maphim);
            sqlCommand.Parameters.AddWithValue("@mauser", user);
            var num = await sqlCommand.ExecuteReaderAsync();
            int cmt = 0;
            if (num.HasRows)
            {
                num.Read();
                cmt = num.GetInt32(0);
            }
            return new StringResult(cmt.ToString());
        }
        [HttpPut]
        public async Task<StringResult> putVote([FromQuery] int maphim, [FromQuery] int user, [FromBody] StringResult data)
        {
            string connectionString = db.Database.GetConnectionString()!;

            using SqlConnection sql = new SqlConnection(connectionString);
            sql.Open();
            using SqlCommand sqlCommand = new SqlCommand("updateVoteUser", sql);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@maphim", maphim);
            sqlCommand.Parameters.AddWithValue("@mauser", user);
            sqlCommand.Parameters.AddWithValue("@data", Convert.ToInt32(data.Result));
            await sqlCommand.ExecuteNonQueryAsync();

            return data;
        }
    }
}
