using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using Dapper;
using NotificationService.Models;

namespace NotificationService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationsController : ControllerBase
    {
        private readonly string _connectionString;

        public NotificationsController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        [HttpPost]
        public async Task<IActionResult> SendNotification([FromBody] string message)
        {
            using var connection = new SqlConnection(_connectionString);

            var log = new NotificationLog
            {
                Message = message,
                SentDate = DateTime.Now
            };

            await connection.ExecuteAsync(
                "INSERT INTO Logs (Message, SentDate) VALUES (@Message, @SentDate)",
                log);

            return Ok(new
            {
                Success = true,
                Message = "Đã ghi log thông báo thành công",
                Content = message
            });
        }
    }
}