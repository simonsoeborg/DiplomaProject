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
            InsertEntityInDatabase(_context.Roles, "Roles");

            /* Categories table */
            ClearTableAndResetSeed(_context.Categories, "Categories", _context);
            InsertEntityInDatabase(_context.Categories, "Categories");

            /* Subcategories table */
            ClearTableAndResetSeed(_context.Subcategories, "Subcategories", _context);
            InsertEntityInDatabase(_context.Subcategories, "Subcategories");

            ///* Products table */
            //ClearTableAndResetSeed(_context.Products, "Products", _context);
            //InsertEntityInDatabase(_context.Products, "products");

            ///* ProductItems table */
            //ClearTableAndResetSeed(_context.ProductItems, "ProductItems", _context);
            //InsertEntityInDatabase(_context.ProductItems, "productitems");

        }

        private void InsertEntityInDatabase<T>(DbSet<T> tableEntity, string tableName) where T : class
        {
            Console.WriteLine("Creating " + tableName);
            var jsondata = GetData(tableEntity);
            //var rolesJson = DemoDataRepository.GetRoles();

            foreach (var dataEntry in jsondata)
            {
                using var transaction = _context.Database.BeginTransaction();
                try
                {
                    _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT GroenlundDB.dbo." + tableName + " ON");

                    //_context.Set<Role>().Add(role);
                    tableEntity.Add(dataEntry);
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


        private void ClearTableAndResetSeed<T>(DbSet<T> tableEntity, string tableName, GroenlundDbContext context) where T : class
        {
            if (tableEntity.Any())
            {
                tableEntity.ExecuteDelete();
                _context.SaveChanges();
                Console.WriteLine("Database contains " + tableName);
                Console.WriteLine("Deleting all roles " + tableName);

                // Detach deleted entities from the context
                foreach (var entity in _context.ChangeTracker.Entries())
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

        private static List<T> GetData<T>(DbSet<T> tableEntity) where T : class
        {
            var entityType = tableEntity.EntityType.ClrType;

            if (entityType == typeof(Role))
            {
                return DemoDataRepository.Roles().Cast<T>().ToList();
            }
            if (entityType == typeof(Category))
            {
                return DemoDataRepository.SøborgCategories().Cast<T>().ToList();
            }
            if (entityType == typeof(Subcategory))
            {
                return DemoDataRepository.SøborgSubcategories().Cast<T>().ToList();
            }
            if (entityType == typeof(Product))
            {
                return DemoDataRepository.Products().Cast<T>().ToList();
            }
            if (entityType == typeof(ProductItem))
            {
                return DemoDataRepository.ProductItems().Cast<T>().ToList();
            }
            else
            {
                Console.Error.WriteLine("Unable to match entitytype with a list of json objects");
            }

            return new List<T>();
        }
    }
}


/*
 * 
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