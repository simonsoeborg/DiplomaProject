using ClassLibrary.Models;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using WebScraper.Controllers;

var builder = WebApplication.CreateBuilder(args);

/* Add services to the container */
builder.Services.AddControllers().AddNewtonsoftJson();

//builder.Services.AddDbContext<GroenlundDbContext>(
//options =>
//{
//    //System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
//});

builder.Services.AddDbContext<GroenlundDbContext>();


builder.Services.AddSingleton<IWebDriver>(provider =>
{
    var chromeOptions = new ChromeOptions();
    chromeOptions.AddArgument("--headless");
    chromeOptions.AddArgument("--no-sandbox");
    chromeOptions.AddArgument("--disable-dev-shm-usage");
    var driver = new ChromeDriver(chromeOptions);
    return driver;
});

builder.Services.AddSingleton<IWebDriverService, WebDriverService>();
builder.Services.AddTransient<SniperHandler>();
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen(options =>
//{
//    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
//    {
//        In = ParameterLocation.Header,
//        Name = "Authorization",
//        Type = SecuritySchemeType.ApiKey
//    });

//    options.OperationFilter<SecurityRequirementsOperationFilter>();
//});

//builder.Services.AddAuthentication().AddJwtBearer();

builder.Services.AddSwaggerGen();
builder.Services.AddCors();

var app = builder.Build();


app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
