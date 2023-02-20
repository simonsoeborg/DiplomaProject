using GroenlundEntityFramework.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

/* Add services to the container */
builder.Services.AddControllers();

/* Setup DBContext and add configuration options */
builder.Services.AddDbContext<GroenlundDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

    options.UseMySql(
        connectionString,
        ServerVersion.AutoDetect(connectionString));
});


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.WebHost.UseUrls("http://*:5000");
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
}
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
