using Newtonsoft.Json;
using ClassLibrary;

namespace CsvConverter
{

    public static class DemoDataRepository
    {
        public static List<Category> GetCategories()
        {
            var json = File.ReadAllText("categories.json");
            var categories = JsonConvert.DeserializeObject<List<Category>>(json);
            return categories!;
        }

        public static List<Subcategory> GetSubcategories()
        {
            var json = File.ReadAllText("subcategories.json");
            var subcategories = JsonConvert.DeserializeObject<List<Subcategory>>(json);
            return subcategories!;
        }
    }
}
