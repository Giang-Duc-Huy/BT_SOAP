using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using IdentityService.Models;
using Dapper;

namespace IdentityService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly string _connectionString;

        public UsersController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // Lấy thông tin User theo Id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            using var connection = new SqlConnection(_connectionString);

            var user = await connection.QueryFirstOrDefaultAsync<User>(
                "SELECT * FROM Users WHERE Id = @Id",
                new { Id = id });

            if (user == null)
                return NotFound(new { Message = "Không tìm thấy người dùng" });

            return Ok(user);
        }

        // Lấy Rank của User (dùng cho BorrowingService)
        [HttpGet("{id}/rank")]
        public async Task<IActionResult> GetUserRank(int id)
        {
            using var connection = new SqlConnection(_connectionString);

            var rank = await connection.QueryFirstOrDefaultAsync<string>(
                "SELECT Rank FROM Users WHERE Id = @Id",
                new { Id = id });

            if (string.IsNullOrEmpty(rank))
                return NotFound();

            return Ok(new { Rank = rank });
        }
    }
}