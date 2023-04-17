using ClassLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace DataMigration
{
    public class DemoDataGenerator
    {
        private readonly GroenlundDbContext _context;
        public DemoDataGenerator()
        {
            _context = new();
        }
        public void PopulateDatabase()
        {
            /* Roles table */
            ClearTableAndResetSeed(_context.Roles, "Roles", _context);
            InsertEntityInDatabase(_context.Roles, "Roles", DemoDataRepository.Roles());

            /* Categories table */
            ClearTableAndResetSeed(_context.Categories, "Categories", _context);
            InsertEntityInDatabase(_context.Categories, "Categories", DemoDataRepository.Categories());

            /* Subcategories table */
            ClearTableAndResetSeed(_context.Subcategories, "Subcategories", _context);
            InsertEntityInDatabase(_context.Subcategories, "Subcategories", DemoDataRepository.Subcategories());

            DataMigrater dg = new();
            var (Products, ProductItems, Images) = dg.ExtractProducts();

            /* Products table */
            ClearTableAndResetSeed(_context.Products, "Products", _context);
            InsertProductsInDatabase("Products", Products);

            /* ProductItems table */
            ClearTableAndResetSeed(_context.ProductItems, "ProductItems", _context);
            InsertEntityInDatabase(_context.ProductItems, "ProductItems", ProductItems);

            /* Images table */
            ClearTableAndResetSeed(_context.Images, "Images", _context);
            InsertEntityInDatabase(_context.Images, "Images", Images);

            /* Orders table */
            ClearTableAndResetSeed(_context.Orders, "Orders", _context);
            InsertEntityInDatabase(_context.Orders, "Orders", DemoDataRepository.Orders());

            /* Customers, Payments, DiscountCodes tables */

            //TODO
            //ClearTableAndResetSeed(_context.Customers, "Customers", _context);
            //ClearTableAndResetSeed(_context.Payments, "Payments", _context);
            //ClearTableAndResetSeed(_context.DiscountCodes, "DiscountCodes", _context);

            /* Users table */
            ClearTableAndResetSeed(_context.Users, "Users", _context);
            InsertEntityInDatabase(_context.Users, "Users", DemoDataRepository.Users());
        }

        private void InsertEntityInDatabase<T>(DbSet<T> tableEntity, string tableName, List<T> entities) where T : class
        {
            Console.WriteLine("Creating " + tableName);

            foreach (var entity in entities)
            {
                using var transaction = _context.Database.BeginTransaction();
                try
                {
                    _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT GroenlundDB.dbo." + tableName + " ON");
                    tableEntity.Add(entity);
                    _context.SaveChanges();
                    _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT GroenlundDB.dbo." + tableName + " OFF");
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine("Failed creating " + tableName + ex.Message + "\n");
                    Console.WriteLine("StackTrace:" + ex.StackTrace + "\n");
                    Console.WriteLine("InnerException:" + ex.InnerException + "\n");
                }
            }

            Console.WriteLine("Successfully created " + tableName);
        }

        private void InsertProductsInDatabase(string tableName, List<Product> products)
        {
            List<Subcategory> subcategories = _context.Subcategories.Include(s => s.Category).ToList();

            Console.WriteLine("Creating " + tableName);

            foreach (var product in products)
            {
                // This has to be done in order for EF-Core to ensure tracking of subcategory entities is correct
                MapSubcategoryEntityToProduct(subcategories, product);

                using var transaction = _context.Database.BeginTransaction();
                try
                {
                    _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT GroenlundDB.dbo.Products ON");

                    _context.Products.Add(product);
                    _context.SaveChanges();
                    _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT GroenlundDB.dbo." + tableName + " OFF");
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    //transaction.Rollback();
                    Console.WriteLine("Failed creating " + tableName + ex.Message + "\n");
                    Console.WriteLine("StackTrace:" + ex.StackTrace + "\n");
                    Console.WriteLine("InnerException:" + ex.InnerException + "\n");
                }
            }

            Console.WriteLine("Successfully created " + tableName);
        }

        private void MapSubcategoryEntityToProduct(List<Subcategory> subcategoryEntities, Product product)
        {
            List<Subcategory> productSubcategories = product.Subcategories.ToList();
            List<Subcategory> newProductSubcategories = new();

            foreach (var productSubcat in productSubcategories)
            {
                newProductSubcategories.Add(subcategoryEntities.Find(s => s.Id == productSubcat.Id)!);
            }

            product.Subcategories = newProductSubcategories;
        }

        private static void ClearTableAndResetSeed<T>(DbSet<T> dbTable, string tableName, GroenlundDbContext context) where T : class
        {
            if (dbTable.Any())
            {
                dbTable.ExecuteDelete();
                context.SaveChanges();
                Console.WriteLine("Database contains " + tableName);
                Console.WriteLine("Deleting all " + tableName);

                // Detach deleted entities from the context
                foreach (var entity in context.ChangeTracker.Entries())
                {
                    if (entity.State == EntityState.Deleted)
                    {
                        entity.State = EntityState.Detached;
                    }
                }
            }

            // Reset the seed
            context.Database.ExecuteSqlRaw("DBCC CHECKIDENT('" + tableName + "', RESEED, 0)");
            Console.WriteLine("Resetted seed for " + tableName);
        }
    }
}


/*
 * 
 * 
 * 
 * 
 *  private void InsertProductItemsInDatabase(DbSet<ProductItem> productItemTable, string tableName, List<ProductItem> productItems)
        {
            Console.WriteLine("Creating " + tableName);

            foreach (var productItem in productItems)
            {
                using var transaction = _context.Database.BeginTransaction();
                try
                {
                    _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT GroenlundDB.dbo." + tableName + " ON");
                    //_context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT GroenlundDB.dbo.Images ON");
                    productItemTable.Add(productItem);
                    _context.SaveChanges();
                    _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT GroenlundDB.dbo." + tableName + " OFF");
                    //_context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT GroenlundDB.dbo.Images OFF");
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine("Failed creating " + tableName + ex.Message + "\n");
                    Console.WriteLine("StackTrace:" + ex.StackTrace + "\n");
                    Console.WriteLine("InnerException:" + ex.InnerException + "\n");
                }
            }

            Console.WriteLine("Successfully created " + tableName);
        }
 * 
    private void CreateProductsInDatabase()
    {
        var existingProducts = _context.Products;
        var existingProductItems = _context.ProductItems;
        if (existingProducts.Any())
        {
            try
            {
                _context.Products.ExecuteDelete();
                Console.WriteLine("Deleted all products in database\n");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        if (existingProductItems.Any())
        {
            try
            {
                _context.ProductItems.ExecuteDelete();
                Console.WriteLine("Deleted all productItems in database\n");

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        var getData = ExtractProducts();
        List<Product> Products = getData.Products;
        List<ProductItem> ProductItems = getData.ProductItems;

        foreach (Product prod in Products)
        {
            _context.Products.Add(prod);
            try
            {
                _context.SaveChanges();
                Console.WriteLine(prod.Name + " added");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed creating product" + ex.Message);
            }
        }


        _context.SaveChanges();

        foreach (ProductItem productItem in ProductItems)
        {
            _context.ProductItems.Add(productItem);
            try
            {
                _context.SaveChanges();
                Console.WriteLine("ProductItem for " + productItem.Product.Name + " added");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed creating productItem" + ex.Message + "\n");
                Console.WriteLine("StackTrace:" + ex.StackTrace + "\n");
                Console.WriteLine("InnerException:" + ex.InnerException + "\n");
            }
        }
    }
*/