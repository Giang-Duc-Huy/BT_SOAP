using BT_4.Models;
using BT_4.Models;      // ← Quan trọng
using BT_4.Services;    // ← Quan trọng

var builder = WebApplication.CreateBuilder(args);

// === CẤU HÌNH MONGO DB ===
builder.Services.Configure<BookStoreDatabaseSettings>(
    builder.Configuration.GetSection("BookStoreDatabase"));

builder.Services.AddSingleton<BookService>();

// === Controllers + Swagger ===
builder.Services.AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.PropertyNamingPolicy = null);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();           // ← Giờ sẽ không lỗi nữa

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();