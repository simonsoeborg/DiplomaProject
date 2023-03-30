using ClassLibrary.Models;
using Newtonsoft.Json;

namespace DataMigration
{

    public static class DemoDataRepository
    {
        public static List<Category> GetCategories()
        {
            var json = File.ReadAllText("DemoData/categories.json");
            return JsonConvert.DeserializeObject<List<Category>>(json)!;
        }

        public static List<Subcategory> GetSubcategories()
        {
            var json = File.ReadAllText("DemoData/subcategories.json");
            return JsonConvert.DeserializeObject<List<Subcategory>>(json)!;
        }

        public static List<Product> GetProducts()
        {
            var json = File.ReadAllText("DemoData/products.json");
            return JsonConvert.DeserializeObject<List<Product>>(json)!;
        }

        public static List<ProductItem> GetProductItems()
        {
            var json = File.ReadAllText("DemoData/productitems.json");
            return JsonConvert.DeserializeObject<List<ProductItem>>(json)!;
        }

        public static List<Role> GetRoles()
        {
            var json = File.ReadAllText("DemoData/roles.json");
            return JsonConvert.DeserializeObject<List<Role>>(json)!;
        }
    }
}
