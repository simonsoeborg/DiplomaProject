using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using WebScraper.Controllers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IWebDriver>(provider =>
{
    var chromeOptions = new ChromeOptions();
    //chromeOptions.AddArgument("--headless");
    //chromeOptions.AddArgument("--no-sandbox");
    //chromeOptions.AddArgument("--disable-dev-shm-usage");
    var driver = new ChromeDriver(chromeOptions);
    return driver;
});

builder.Services.AddSingleton<IWebDriverService, WebDriverService>();
builder.Services.AddControllers();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();