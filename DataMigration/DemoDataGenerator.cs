using ClassLibrary.Models;
using Microsoft.EntityFrameworkCore;
using System.Text;

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
            Console.Clear();
            InsertRolesCategoriesSubcategories(msSQL);
            InsertProductsProductItemsImages(msSQL);
            InsertCustomersDiscountCodesUsers(msSQL);
            InsertOrdersPayments(msSQL);
        }
        private void InsertRolesCategoriesSubcategories(bool? msSQL)
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
        }
        private void InsertProductsProductItemsImages(bool? msSQL)
        {
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
        }
        private void InsertCustomersDiscountCodesUsers(bool? msSQL)
        {
            /* Customers table */
            ClearTableAndResetSeed(_context.Customers, "Customers", _context, msSQL);
            InsertEntityInDatabase(_context.Customers, "Customers", DemoDataRepository.Customers(), msSQL);

            /* DiscountCodes table */
            ClearTableAndResetSeed(_context.DiscountCodes, "DiscountCodes", _context, msSQL);
            InsertEntityInDatabase(_context.DiscountCodes, "DiscountCodes", DemoDataRepository.DiscountCodes(), msSQL);

            /* Users table */
            ClearTableAndResetSeed(_context.Users, "Users", _context, msSQL);
            InsertEntityInDatabase(_context.Users, "Users", DemoDataRepository.Users(), msSQL);
        }
        private void InsertOrdersPayments(bool? msSQL)
        {
            var (Orders, Payments) = GenerateOrders(_context);

            /* Payments table */
            ClearTableAndResetSeed(_context.Payments, "Payments", _context, msSQL);
            InsertEntityInDatabase(_context.Payments, "Payments", Payments, msSQL);

            /* OrderElements & Orders table */
            //ClearTableAndResetSeed(_context.OrderElements, "OrderElements", _context, msSQL);
            ClearTableAndResetSeed(_context.Orders, "Orders", _context, msSQL);
            InsertEntityInDatabase(_context.Orders, "Orders", Orders, msSQL);
            //InsertEntityInDatabase(_context.OrderElements, "OrderElements", OrderElements, msSQL);

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

            Console.WriteLine("Successfully created " + numberOfProducts + " " + tableName + "\n");
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
        public static (List<Order> Orders, List<Payment> Payments) GenerateOrders(GroenlundDbContext context)
        {
            var orders = new List<Order>();
            var payments = new List<Payment>();
            var customers = context.Customers.ToList();
            var discountCodes = context.DiscountCodes.ToList();
            var productItems = context.ProductItems.Where(p => p.Sold == 1).ToList();
            List<int> usedProductItemIds = new();
            List<string> deliveryStatuses = new() { "Delivered", "Pending", "Shipped" };
            List<string> paymentMethods = new() { "Credit Card", "PayPal", "Bank Transfer", "MobilePay", "Stripe" };
            int orderIdCounter = 1;
            //int orderElementsCounter = 1;
            int customerProductItems = productItems.Count / customers.Count;
            int remainingProductItems = productItems.Count;

            foreach (var customer in customers)
            {
                int customerRemainingProductItems = customerProductItems;
                while (customerRemainingProductItems > 0)
                {
                    int randomNumberOfProductItems = new Random().Next(1, 5);
                    if (randomNumberOfProductItems > remainingProductItems)
                    {
                        randomNumberOfProductItems = remainingProductItems;
                    }
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

                    var createdDate = RandomDay();
                    var discountCode = discountCodes[new Random().Next(1, discountCodes.Count)];

                    var order = new Order
                    {
                        Id = orderIdCounter,
                        CustomerId = customer.Id,
                        Customer = customer,
                        Active = false,
                        DeliveryStatus = deliveryStatuses[new Random().Next(1, 3)],
                        DiscountCodeId = discountCode.Id,
                        DiscountCode = discountCode,
                        CreatedDate = createdDate,
                        ProductItems = productItems1,
                        OrderStatus = "Completed",
                        DeliveryMethod = "Afhentning"

                    };



                    decimal paymentAmount = 0;
                    foreach (var productItem in order.ProductItems)
                    {
                        paymentAmount += productItem.CurrentPrice;
                    }

                    paymentAmount *= (100 - order.DiscountCode.DiscountPercentage);
                    order.TotalPrice = (double)paymentAmount;

                    var payment = new Payment
                    {
                        Id = orderIdCounter,
                        Amount = (double)paymentAmount,
                        DatePaid = createdDate,
                        Method = paymentMethods[new Random().Next(1, paymentMethods.Count)],
                        Status = "Approved",
                        TransactionId = GenerateTransactionID(8)
                    };

                    order.PaymentId = payment.Id;

                    orders.Add(order);
                    payments.Add(payment);

                    orderIdCounter++;
                    customerRemainingProductItems -= randomNumberOfProductItems;

                }
            }

            return (orders, payments);
        }
        private static DateTime RandomDay()
        {
            DateTime start = new(2019, 1, 1);
            int range = (DateTime.Today - start).Days;
            return start.AddDays(new Random().Next(range));
        }
        private static string GenerateTransactionID(int length)
        {
            const string Characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            Random random = new Random();
            StringBuilder sb = new StringBuilder(length);

            for (int i = 0; i < length; i++)
            {
                int randomIndex = random.Next(Characters.Length);
                sb.Append(Characters[randomIndex]);
            }

            return sb.ToString();
        }
    }
}

