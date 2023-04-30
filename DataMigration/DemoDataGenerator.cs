using ClassLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace DataMigration
{
    public class DemoDataGenerator
    {
        private readonly GroenlundDbContext _context;
        private readonly DataMigrater dm;
        public DemoDataGenerator()
        {
            _context = new();
            dm = new();
        }
        public void PopulateDatabase(bool? msSQL)
        {
            /* Roles table */
            ClearTableAndResetSeed(_context.Roles, "Roles", _context, msSQL);
            InsertEntityInDatabase(_context.Roles, "Roles", DemoDataRepository.Roles(), msSQL);

            /* Categories table */
            ClearTableAndResetSeed(_context.Categories, "Categories", _context, msSQL);
            InsertEntityInDatabase(_context.Categories, "Categories", DemoDataRepository.Categories(), msSQL);

            /* Subcategories table */
            ClearTableAndResetSeed(_context.Subcategories, "Subcategories", _context, msSQL);
            InsertEntityInDatabase(_context.Subcategories, "Subcategories", DemoDataRepository.Subcategories(), msSQL);

            var (Products, ProductItems, Images) = dm.ExtractProducts();

            /* Products table */
            ClearTableAndResetSeed(_context.Products, "Products", _context, msSQL);
            InsertProductsInDatabase("Products", Products, msSQL);

            /* ProductItems table */
            ClearTableAndResetSeed(_context.ProductItems, "ProductItems", _context, msSQL);
            InsertEntityInDatabase(_context.ProductItems, "ProductItems", ProductItems, msSQL);

            /* Images table */
            ClearTableAndResetSeed(_context.Images, "Images", _context, msSQL);
            InsertEntityInDatabase(_context.Images, "Images", Images, msSQL);


            /* Customers table */
            ClearTableAndResetSeed(_context.Customers, "Customers", _context, msSQL);
            InsertEntityInDatabase(_context.Customers, "Customers", DemoDataRepository.Customers(), msSQL);

            /* DiscountCodes table */
            ClearTableAndResetSeed(_context.DiscountCodes, "DiscountCodes", _context, msSQL);
            InsertEntityInDatabase(_context.DiscountCodes, "DiscountCodes", DemoDataRepository.DiscountCodes(), msSQL);

            /* Users table */
            ClearTableAndResetSeed(_context.Users, "Users", _context, msSQL);
            InsertEntityInDatabase(_context.Users, "Users", DemoDataRepository.Users(), msSQL);

            var (Orders, Payments, OrderElements) = GenerateOrders(_context);

            /* Payments table */
            ClearTableAndResetSeed(_context.Payments, "Payments", _context, msSQL);
            InsertEntityInDatabase(_context.Payments, "Payments", Payments, msSQL);

            /* Orders table */
            ClearTableAndResetSeed(_context.Orders, "Orders", _context, msSQL);
            InsertEntityInDatabase(_context.Orders, "Orders", Orders, msSQL);

            /* OrderElements table */
            ClearTableAndResetSeed(_context.OrderElements, "OrderElements", _context, msSQL);
            InsertEntityInDatabase(_context.OrderElements, "OrderElements", OrderElements, msSQL);
        }

        private void InsertEntityInDatabase<T>(DbSet<T> tableEntity, string tableName, List<T> entities, bool? msSQL) where T : class
        {
            Console.WriteLine("Creating " + tableName);
            int numberOfEntities = entities.Count;
            int i = 0;

            foreach (var entity in entities)
            {
                using var transaction = _context.Database.BeginTransaction();
                try
                {
                    if (msSQL != null && msSQL == true)
                    {
                        _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT GroenlundDB.dbo." + tableName + " ON");
                    }
                    tableEntity.Add(entity);
                    _context.SaveChanges();
                    if (msSQL != null && msSQL == true)
                    {
                        _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT GroenlundDB.dbo." + tableName + " OFF");
                    }
                    transaction.Commit();
                    i++;
                    Console.Write($"{tableName} created: {i}/{numberOfEntities}\r");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine("Failed creating " + tableName + ex.Message + "\n");
                    Console.WriteLine("StackTrace:" + ex.StackTrace + "\n");
                    Console.WriteLine("InnerException:" + ex.InnerException + "\n");
                }

            }

            Console.WriteLine("Successfully created " + numberOfEntities + " " + tableName + "\n");
        }

        private void InsertProductsInDatabase(string tableName, List<Product> products, bool? msSQL)
        {
            List<Subcategory> subcategories = _context.Subcategories.Include(s => s.Category).ToList();
            int numberOfProducts = products.Count;
            int i = 0;
            Console.WriteLine("Creating " + tableName);

            foreach (var product in products)
            {
                // This has to be done in order for EF-Core to ensure tracking of subcategory entities is correct
                MapSubcategoryEntityToProduct(subcategories, product);

                using var transaction = _context.Database.BeginTransaction();
                try
                {
                    if (msSQL != null && msSQL == true)
                    {
                        _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT GroenlundDB.dbo." + tableName + " ON");
                    }
                    _context.Products.Add(product);
                    _context.SaveChanges();
                    if (msSQL != null && msSQL == true)
                    {
                        _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT GroenlundDB.dbo." + tableName + " OFF");
                    }
                    transaction.Commit();
                    i++;
                    Console.Write($"{tableName} created: {i}/{numberOfProducts}\r");

                }
                catch (Exception ex)
                {
                    //transaction.Rollback();
                    Console.WriteLine("Failed creating " + tableName + ex.Message + "\n");
                    Console.WriteLine("StackTrace:" + ex.StackTrace + "\n");
                    Console.WriteLine("InnerException:" + ex.InnerException + "\n");
                }

            }

            Console.WriteLine("Successfully created " + numberOfProducts + " " + tableName);
        }

        private static void MapSubcategoryEntityToProduct(List<Subcategory> subcategoryEntities, Product product)
        {
            List<Subcategory> productSubcategories = product.Subcategories.ToList();
            List<Subcategory> newProductSubcategories = new();

            foreach (var productSubcat in productSubcategories)
            {
                newProductSubcategories.Add(subcategoryEntities.Find(s => s.Id == productSubcat.Id)!);
            }

            product.Subcategories = newProductSubcategories;
        }

        private static void ClearTableAndResetSeed<T>(DbSet<T> dbTable, string tableName, GroenlundDbContext context, bool? msSQL) where T : class
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
            if (msSQL != null && msSQL == true)
            {
                context.Database.ExecuteSqlRaw("DBCC CHECKIDENT('" + tableName + "', RESEED, 0)");
                Console.WriteLine("Resetted seed for " + tableName);
            }
        }

        private static (List<Order> Orders, List<Payment> Payments, List<OrderElements>) GenerateOrders(GroenlundDbContext context)
        {
            var orders = new List<Order>();
            var payments = new List<Payment>();
            var orderElements = new List<OrderElements>();
            var customers = context.Customers.ToList();
            var discountCodes = context.DiscountCodes.ToList();
            var productItems = context.ProductItems.ToList();
            List<int> usedProductItemIds = new();
            List<string> deliveryStatuses = new() { "Delivered", "Pending", "Shipped" };
            List<string> paymentMethods = new() { "Credit Card", "PayPal", "Bank Transfer", "MobilePay", "Stripe" };
            int orderIdCounter = 1;
            int orderElementsCounter = 1;

            foreach (var customer in customers)
            {
                int numberOfOrders = new Random().Next(1, 10);


                for (int i = 0; i < numberOfOrders; i++)
                {
                    int randomNumberOfProductItems = new Random().Next(1, 5);
                    List<ProductItem> productItems1 = new();
                    for (int x = 0; x <= randomNumberOfProductItems; x++)
                    {
                        int randomProductItemId = new Random().Next(1, productItems.Count);

                        while (usedProductItemIds.Contains(randomProductItemId))
                        {
                            randomProductItemId = new Random().Next(1, productItems.Count);
                        }
                        productItems1.Add(productItems[randomProductItemId]);
                    }




                    var order = new Order
                    {
                        Id = orderIdCounter,
                        CustomerId = customer.Id,
                        Active = numberOfOrders % 2 == 0,
                        DeliveryStatus = deliveryStatuses[new Random().Next(1, 3)],
                        DiscountCode = discountCodes[new Random().Next(1, discountCodes.Count)].Code,
                    };

                    List<OrderElements> specificOrderOrderElements = new();
                    foreach (var po in productItems1)
                    {
                        var oe = new OrderElements()
                        {
                            Id = orderElementsCounter,
                            Order = order,
                            OrderId = order.Id,
                            ProductItem = po,
                            ProductItemId = po.Id,
                        };
                        specificOrderOrderElements.Add(oe);
                        orderElements.Add(oe);
                        orderElementsCounter++;
                    }

                    //order.OrderElements = specificOrderOrderElements;

                    decimal paymentAmount = 0;
                    foreach (var orderElement in order.OrderElements)
                    {
                        paymentAmount += orderElement.ProductItem.CurrentPrice;
                    }

                    var discountCode = discountCodes.Find(d => d.Code == order.DiscountCode)!;
                    paymentAmount *= (100 - discountCode.DiscountPercentage);

                    var payment = new Payment
                    {
                        Id = orderIdCounter,
                        Amount = (double)paymentAmount,
                        DatePaid = RandomDay(),
                        Method = paymentMethods[new Random().Next(1, paymentMethods.Count)],
                        Approved = Convert.ToSByte(orderIdCounter % 2)
                    };

                    order.PaymentId = payment.Id;
                    order.PaymentStatus = payment.Approved == 1 ? "Approved" : "Pending";

                    orders.Add(order);
                    payments.Add(payment);

                    orderIdCounter++;

                }

            }

            return (orders, payments, orderElements);
        }
        private static DateTime RandomDay()
        {
            DateTime start = new(2019, 1, 1);
            int range = (DateTime.Today - start).Days;
            return start.AddDays(new Random().Next(range));
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