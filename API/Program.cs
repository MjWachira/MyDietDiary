using API.Services.DietService;
using API.Services.ExcelStorageService;
using API.Services.IServices;



var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register our services
builder.Services.AddSingleton<IExcelStorageService>(sp =>
{
    var filePath = Path.Combine(AppContext.BaseDirectory, "mydiet.xlsx");
    return new ExcelStorageService(filePath);
});

builder.Services.AddSingleton<IDietService, DietService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "MyDietDiary API V1");
});

app.UseRouting();
app.UseAuthorization();

app.MapControllers();

app.Run();
