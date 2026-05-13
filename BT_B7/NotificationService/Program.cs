var builder = WebApplication.CreateBuilder(args);

// Thêm services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// ================== SWAGGER ==================
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Identity Service API",   // ← THAY TÊN THEO TỪNG SERVICE
        Version = "v1",
        Description = "API cho hệ thống Quản lý Thư viện Số"
    });
});

// ================== CORS ==================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Identity Service API V1"); // ← THAY TÊN
        c.RoutePrefix = string.Empty; // Swagger là trang mặc định
    });
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();

app.MapControllers();

app.Run();