using ClassLibrary.Models;

namespace DataMigration.Helpers
{
    class CategoryStrings
    {
        public string Name = "";
        public Category Category = new();
        public List<string> Keywords = new();
    }
    class SubcategoryStrings
    {
        public string Name = "";
        public Subcategory Subcategory = new Subcategory();
        public List<string> Keywords = new();
    }
    public static class CategoryHelper
    {
        public static List<Subcategory> ExtractSubcategories(Category category, List<Subcategory> subcategories, string input)
        {
            List<Subcategory> result = new();
            List<int> possibleSubcategoryIds = subcategories.FindAll(x => x.Category.Id == category.Id).Select(s => s.Id).ToList();
            List<SubcategoryStrings> filteredSubcategoryStrings = SubcategoryStringsList(subcategories).FindAll(s => possibleSubcategoryIds.Contains(s.Subcategory.Id));

            foreach (var subcategory in filteredSubcategoryStrings)
            {
                foreach (var keyword in subcategory.Keywords)
                {
                    if (input.ToLowerInvariant().Contains(keyword.ToLowerInvariant()))
                    {
                        result.Add(subcategory.Subcategory);
                    }
                }
            }

            return result;
        }
        public static Category? InferCategory(List<Category> categories, string input)
        {
            input = input.ToLowerInvariant();
            List<CategoryStrings> categoryStrings = CategoryStringsList(categories);

            foreach (var catStrings in categoryStrings)
            {
                //Console.WriteLine("Checking category " + catStrings.Name);
                foreach (string catString in catStrings.Keywords)
                {
                    if (input.ToLowerInvariant().Contains(catString.ToLowerInvariant()))
                    {
                        //Console.WriteLine("Found category {0} for input {1}", catStrings.Category.Name, input);
                        return catStrings.Category;
                    }
                }
            }
            return null;
        }

        private static List<CategoryStrings> CategoryStringsList(List<Category> categories)
        {
            List<CategoryStrings> catStrings = new();
            CategoryStrings figurer = new()
            {
                Name = "Figurer",
                Category = categories.FirstOrDefault(c => c.Id == 3)!,
                Keywords = new List<string>() {
                    "bjørn", "fugl", "hund", "kat", "hest", "menneske", "ørn", "ugle",
                    "mand", "kvinde", "pige", "dreng", "dahl jensen", "elefant",
                    "figur", "menneskelig figur", "figur B&G", "figur royal copenhagen",
                    "isbjørne / bjørne", "andre figur", "fugle", "musselmalet figur", "katte","løver / tiger",
                    "hunde" },
            };
            catStrings.Add(figurer);


            CategoryStrings steldele = new()
            {
                Name = "Steldele",
                Category = categories.FirstOrDefault(c => c.Id == 2)!,
                Keywords = new List<string>() { "tallerken", "kop", "krus", "skål", "kande", "fad", "kage", "krukker", "frokost", "middag", "dyb" },
            };
            catStrings.Add(steldele);



            CategoryStrings keramik = new()
            {
                Name = "Keramik",
                Category = categories.FirstOrDefault(c => c.Id == 4)!,
                Keywords = new List<string>() { "aluminia", "saxbo", "arne bang", "palshus", "kähler", "keramik", "stentøj" },
            };
            catStrings.Add(keramik);

            CategoryStrings five = new()
            {
                Name = "Guld & Sølv",
                Category = categories.FirstOrDefault(c => c.Id == 5)!,
                Keywords = new List<string>() { "broche", "halskæde", "smykke", "saltkar", "guld", "sølv" },
            };
            catStrings.Add(five);

            CategoryStrings six = new()
            {
                Name = "Bestik",
                Category = categories.FirstOrDefault(c => c.Id == 6)!,
                Keywords = new List<string>() { "gaffel", "gafler", "kniv", "ske", "bestik", "kage" },
            };
            catStrings.Add(six);

            CategoryStrings seven = new()
            {
                Name = "Platter",
                Category = categories.FirstOrDefault(c => c.Id == 7)!,
                Keywords = new List<string>() { "platte" },
            };
            catStrings.Add(seven);

            CategoryStrings eight = new()
            {
                Name = "Glas",
                Category = categories.FirstOrDefault(c => c.Id == 8)!,
                Keywords = new List<string>() { "vinglas", "ølglas", "dessertglas", "vase" },
            };
            CategoryStrings stel = new()
            {
                Name = "Stel",
                Category = categories.FirstOrDefault(c => c.Id == 1)!,
                Keywords = new List<string>() { "mega", "musselmalet", "halvblonde", "helblonde", "riflet", "flora", "konkylie", "purpur" },
            };
            catStrings.Add(stel);
            catStrings.Add(eight);

            return catStrings;
        }
        private static List<SubcategoryStrings> SubcategoryStringsList(List<Subcategory> subcategories)
        {
            List<SubcategoryStrings> subcatStrings = new();
            SubcategoryStrings bjørne = new()
            {
                Name = "Bjørne",
                Subcategory = subcategories.FirstOrDefault(c => c.Id == 15)!,
                Keywords = new List<string>() { "bjørn", "isbjørne / bjørne", "isbjørn" },
            };
            subcatStrings.Add(bjørne);

            SubcategoryStrings fugle = new()
            {
                Name = "Fugle",
                Subcategory = subcategories.FirstOrDefault(c => c.Id == 16)!,
                Keywords = new List<string>() { "fugl", "fugle", "ørn", "ugle" },
            };
            subcatStrings.Add(fugle);


            return subcatStrings;
        }
    }

}
