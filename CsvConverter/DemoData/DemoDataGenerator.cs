using ClassLibrary;

namespace CsvConverter
{
    public static class DemoDataGenerator
    {
        public static void GenerateDemoData(GroenlundDbContext context)
        {
            GenerateCategories(context);
            GenerateSubCategories(context);
        }
        private static void GenerateCategories(GroenlundDbContext context)
        {
            List<Category> categories = DemoDataRepository.GetCategories();
            foreach (Category category in categories)
            {
                context.Categories.Add(category);
                Console.WriteLine("Added " + category.Name + "to database\n");
            }
            context.SaveChanges();

        }
        private static void GenerateSubCategories(GroenlundDbContext context)
        {
            List<Subcategory> subcategories = DemoDataRepository.GetSubcategories();
            foreach (Subcategory subcategory in subcategories)
            {
                context.Subcategories.Add(subcategory);
                Console.WriteLine("Added " + subcategory.Name + "to database\n");
            }
            context.SaveChanges();
        }
    }
}
