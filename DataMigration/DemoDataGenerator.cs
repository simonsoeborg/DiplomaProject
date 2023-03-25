using ClassLibrary.Models;

namespace DataMigration
{
    public class DemoDataGenerator
    {
        private readonly GroenlundDbContext _context;
        public DemoDataGenerator(GroenlundDbContext context)
        {
            _context = context;
        }
        public void GenerateDemoData()
        {
            //GenerateCategories(_context);
            //GenerateSubCategories(_context);
            GenerateProducts();
            GenerateProductItems(_context);
        }
        private void GenerateCategories(GroenlundDbContext context)
        {
            List<Category> categories = DemoDataRepository.GetCategories();
            foreach (Category category in categories)
            {
                context.Categories.Add(category);
                Console.WriteLine("Added " + category.Name + "to database\n");
            }
            context.SaveChanges();

        }
        private void GenerateSubCategories(GroenlundDbContext context)
        {
            List<Subcategory> subcategories = DemoDataRepository.GetSubcategories();
            foreach (Subcategory subcategory in subcategories)
            {
                context.Subcategories.Add(subcategory);
                Console.WriteLine("Added " + subcategory.Name + "to database\n");
            }
            context.SaveChanges();
        }

        private void GenerateProducts()
        {

            List<Product> products = DemoDataRepository.GetProducts();
            foreach (Product product in products)
            {
                _context.Products.Add(product);
                Console.WriteLine("Added " + product.Name + "to database\n");
            }
            _context.SaveChanges();
        }

        private void GenerateProductItems(GroenlundDbContext context)
        {
            List<ProductItem> productItems = DemoDataRepository.GetProductItems();
            foreach (ProductItem productItem in productItems)
            {
                context.ProductItems.Add(productItem);
                Console.WriteLine("Added productItem: " + productItem.Id + "for " + productItem.Product.Name + " to database\n");
            }
            context.SaveChanges();
        }
    }
}
