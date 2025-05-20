using Microsoft.Extensions.FileProviders;
using TomatisCRM_API.Controllers;
using TomatisCRM_API.Models;
using TomatisCRM_API.Models.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<TestAppTomatisCrmContext>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.WithOrigins("http://localhost:3000","http://192.168.2.72:3000","http://192.168.2.72","http://192.168.2.55:3000")
                .AllowAnyMethod()  // Allows any method (GET, POST, etc.)
                .AllowAnyHeader()
                .AllowCredentials(); // Allows any header
        });
});

builder.Services.AddControllers();

var app = builder.Build();

if(!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "Uploads")))
{
    Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "Uploads"));
}

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Uploads")),
    RequestPath = "/media",
    ServeUnknownFileTypes = true,
    DefaultContentType = "application/octet-stream"
});

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseRouting();
app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
