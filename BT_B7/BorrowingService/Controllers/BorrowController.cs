using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using Dapper;
using BorrowingService.Models;

namespace BorrowingService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BorrowController : ControllerBase
    {
        private readonly string _connectionString;
        private readonly IHttpClientFactory _httpClientFactory;

        public BorrowController(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _httpClientFactory = httpClientFactory;
        }

        [HttpPost]
        public async Task<IActionResult> BorrowBook([FromQuery] int userId, [FromQuery] int bookId)
        {
            var client = _httpClientFactory.CreateClient();

            // 1. Kiểm tra User tồn tại và lấy Rank
            var userResponse = await client.GetAsync($"https://localhost:5001/api/users/{userId}");
            if (!userResponse.IsSuccessStatusCode)
                return BadRequest("Người dùng không tồn tại.");

            var user = await userResponse.Content.ReadFromJsonAsync<dynamic>();

            // 2. Kiểm tra sách tồn tại và còn stock không
            var bookResponse = await client.GetAsync($"https://localhost:5002/api/books/{bookId}");
            if (!bookResponse.IsSuccessStatusCode)
                return BadRequest("Sách không tồn tại.");

            var book = await bookResponse.Content.ReadFromJsonAsync<dynamic>();
            if (book.stock <= 0)
                return BadRequest("Sách đã hết.");

            // 3. Lưu vào BorrowingDB
            using var connection = new SqlConnection(_connectionString);
            var borrowRecord = new
            {
                UserId = userId,
                BookId = bookId,
                BorrowDate = DateTime.Now
            };

            await connection.ExecuteAsync(
                "INSERT INTO BorrowRecords (UserId, BookId, BorrowDate) VALUES (@UserId, @BookId, @BorrowDate)",
                borrowRecord);

            // 4. Giảm Stock sách
            await client.PutAsJsonAsync($"https://localhost:5002/api/books/{bookId}/stock", -1);

            // 5. Gửi thông báo
            await client.PostAsJsonAsync("https://localhost:5004/api/notifications",
                $"Người dùng {userId} đã mượn sách ID {bookId} thành công.");

            return Ok(new
            {
                Message = "Mượn sách thành công!",
                UserId = userId,
                BookId = bookId
            });
        }
    }
}