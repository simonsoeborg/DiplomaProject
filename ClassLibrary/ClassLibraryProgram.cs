//using ClassLibrary.Models;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Hosting;
//using IHost host = Host.CreateDefaultBuilder(args).Build();

//#pragma warning disable CS7022 // The entry point of the program is global code; ignoring entry point
//static void Main()
//{
//    IConfigurationRoot root = new ConfigurationBuilder().AddJsonFile("appsettings.json", false, true).Build();
//    string connString = root["TestDB"]!;
//    GroenlundDbContext context = new(
//        options: new DbContextOptionsBuilder<GroenlundDbContext>()
//            .UseMySql(connString, ServerVersion.AutoDetect(connString))
//                .Options
//    );
//}
//#pragma warning restore CS7022 // The entry point of the program is global code; ignoring entry point

//Main();



