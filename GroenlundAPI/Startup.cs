using GroenlundDBContext.Context;
using System.Configuration;

namespace GroenlundAPI
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        private readonly IHostApplicationLifetime _hostApplicationLifeTime;

        public Startup(IConfiguration configuration, IHostApplicationLifetime hostApplicationLifetime)
        {
            Configuration = configuration;
            _hostApplicationLifeTime = hostApplicationLifetime;
        }

        public void OnApplicationStarted()
        {
            ProductContext PC = new();
            PC.AddProductsFromCsv("C:\\Users\\simon_k33m7ql\\Downloads\\catalog_products.csv");
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            _hostApplicationLifeTime.ApplicationStarted.Register(OnApplicationStarted);
        }
    }
}
