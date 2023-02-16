using GroenlundDBContext.Models;
using Microsoft.EntityFrameworkCore;

namespace GroenlundDBContext.Context
{
    public class ProductContext : DbContext
    {
        public DbSet<Product> Products { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            optionsBuilder.UseSqlServer(config.GetConnectionString("DefaultConnectionString"));
        }

        public void AddProductsFromCsv(string csvFilePath)
        {
            var products = File.ReadLines(csvFilePath)
                .Skip(1)
                .Select(line => line.Split(','))
                .Select(fields => new Product
                {
                    HandleId = int.Parse(fields[0]),
                    FieldType = fields[1],
                    Name = fields[2],
                    Description = fields[3],
                    ProductImageUrl = fields[4],
                    Collection = fields[5],
                    Sku = fields[6],
                    Ribbon = fields[7],
                    Price = decimal.Parse(fields[8]),
                    Surcharge = decimal.Parse(fields[9]),
                    Visible = bool.Parse(fields[10]),
                    DiscountMode = fields[11],
                    DiscountValue = decimal.Parse(fields[12]),
                    Inventory = int.Parse(fields[13]),
                    Weight = double.Parse(fields[14]),
                    Cost = decimal.Parse(fields[15])
                }).ToList();

            Products.AddRange(products);

            SaveChanges();
        }
    }
}
