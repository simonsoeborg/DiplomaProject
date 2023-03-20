using ClassLibrary;
using Microsoft.EntityFrameworkCore;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Serilog;
using WebScraper.Controllers;

var builder = WebApplication.CreateBuilder(args);

/* Add services to the container */
builder.Services.AddControllers();

/* Setup DBContext and add configuration options */
builder.Services.AddDbContext<GroenlundDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

    options.UseSqlServer(connectionString);
});

builder.Services.AddSingleton<IWebDriver>(provider =>
{
    var chromeOptions = new ChromeOptions();
    chromeOptions.AddArgument("--headless");
    chromeOptions.AddArgument("--no-sandbox");
    chromeOptions.AddArgument("--disable-dev-shm-usage");
    var driver = new ChromeDriver(chromeOptions);
    return driver;
});

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("C:\\Logs\\GroenlundAPI.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Logging.AddSerilog(Log.Logger);

builder.Services.AddSingleton<IWebDriverService, WebDriverService>();

builder.Services.AddTransient<SniperHandler>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
    });
});
var app = builder.Build();

app.UseCors("AllowAll");
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
