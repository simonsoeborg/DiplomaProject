using ClassLibrary.Models;
using DataMigration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using IHost host = Host.CreateDefaultBuilder(args).Build();



IConfigurationRoot root = new ConfigurationBuilder().AddJsonFile("appsettings.json", false, true).Build();

// Create a new database context using MySql TestDB database
GroenlundDbContext context = new(options: new DbContextOptionsBuilder<GroenlundDbContext>().UseMySql(root["TestDB"], ServerVersion.AutoDetect(root["TestDB"])).Options);

// Create a new database context using MySql GroenlundDb database
//GroenlundDbContext context = new(options: new DbContextOptionsBuilder<GroenlundDbContext>().UseMySql(root["GroenlundDb"], ServerVersion.AutoDetect(root["GroenlundDb"])).Options);

// Create a new database context using MSSql database
//GroenlundDbContext context = new(options: new DbContextOptionsBuilder<GroenlundDbContext>().UseSqlServer(root["MSSQL"]).Options);

DataMigrater dataMigrater = new(context);
DemoDataGenerator demoDataGenerator = new(context);

dataMigrater.CreateDataInDatabase();
