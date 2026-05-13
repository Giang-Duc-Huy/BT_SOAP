using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using Dapper;
using BookService.Models;

namespace BookService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly string _connectionString;

        public BooksController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // Lấy thông tin sách
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBook(int id)
        {
            using var connection = new SqlConnection(_connectionString);

            var book = await connection.QueryFirstOrDefaultAsync<Book>(
                "SELECT * FROM Books WHERE Id = @Id",
                new { Id = id });

            if (book == null)
                return NotFound(new { Message = "Không tìm thấy sách" });

            return Ok(book);
        }

        // Cập nhật Stock (giảm khi mượn sách)
        [HttpPut("{id}/stock")]
        public async Task<IActionResult> UpdateStock(int id, [FromBody] int quantity)
        {
            using var connection = new SqlConnection(_connectionString);

            var affected = await connection.ExecuteAsync(
                "UPDATE Books SET Stock = Stock + @Quantity WHERE Id = @Id",
                new { Id = id, Quantity = quantity });

            if (affected > 0)
                return Ok(new { Success = true, Message = "Cập nhật Stock thành công" });

            return BadRequest("Không tìm thấy sách hoặc cập nhật thất bại");
        }
    }
}