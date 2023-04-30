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

            CategoryStrings guldOgSoelv = new()
            {
                Name = "Guld & Sølv",
                Category = categories.FirstOrDefault(c => c.Id == 5)!,
                Keywords = new List<string>() { "broche", "halskæde", "smykke", "saltkar", "guld", "sølv" },
            };
            catStrings.Add(guldOgSoelv);

            CategoryStrings bestik = new()
            {
                Name = "Bestik",
                Category = categories.FirstOrDefault(c => c.Id == 6)!,
                Keywords = new List<string>() { "gaffel", "gafler", "kniv", "ske", "bestik", "kage" },
            };
            catStrings.Add(bestik);

            CategoryStrings platter = new()
            {
                Name = "Platter",
                Category = categories.FirstOrDefault(c => c.Id == 7)!,
                Keywords = new List<string>() { "platte" },
            };
            catStrings.Add(platter);

            CategoryStrings glas = new()
            {
                Name = "Glas",
                Category = categories.FirstOrDefault(c => c.Id == 8)!,
                Keywords = new List<string>() { "vinglas", "ølglas", "dessertglas", "vase" },
            };
            catStrings.Add(glas);
            CategoryStrings stel = new()
            {
                Name = "Stel",
                Category = categories.FirstOrDefault(c => c.Id == 1)!,
                Keywords = new List<string>() { "mega", "musselmalet", "halvblonde", "helblonde", "riflet", "flora", "konkylie", "purpur" },
            };
            catStrings.Add(stel);

            return catStrings;
        }
        private static List<SubcategoryStrings> SubcategoryStringsList(List<Subcategory> subcategories)
        {
            List<SubcategoryStrings> subcatStrings = new();

            // Kategori : Stel
            SubcategoryStrings mega = new()
            {
                Name = "Mega",
                Subcategory = subcategories.FirstOrDefault(c => c.Id == 1)!,
                Keywords = new List<string>() { "mega" }
            };
            subcatStrings.Add(mega);

            SubcategoryStrings musselmalet = new()
            {
                Name = "Musselmalet",
                Subcategory = subcategories.FirstOrDefault(c => c.Id == 2)!,
                Keywords = new List<string>() { "musselmalet" }
            };
            subcatStrings.Add(musselmalet);

            SubcategoryStrings halvblonde = new()
            {
                Name = "Halvblonde",
                Subcategory = subcategories.FirstOrDefault(c => c.Id == 3)!,
                Keywords = new List<string>() { "halvblonde" }
            };
            subcatStrings.Add(halvblonde);

            SubcategoryStrings helblonde = new()
            {
                Name = "Helblonde",
                Subcategory = subcategories.FirstOrDefault(c => c.Id == 4)!,
                Keywords = new List<string>() { "helblonde" }
            };
            subcatStrings.Add(helblonde);

            SubcategoryStrings riflet = new()
            {
                Name = "Riflet",
                Subcategory = subcategories.FirstOrDefault(c => c.Id == 5)!,
                Keywords = new List<string>() { "riflet" }
            };
            subcatStrings.Add(riflet);

            SubcategoryStrings flora = new()
            {
                Name = "Flora",
                Subcategory = subcategories.FirstOrDefault(c => c.Id == 6)!,
                Keywords = new List<string>() { "flora" }
            };
            subcatStrings.Add(flora);

            SubcategoryStrings konkylie = new()
            {
                Name = "Konkylie",
                Subcategory = subcategories.FirstOrDefault(c => c.Id == 7)!,
                Keywords = new List<string>() { "konkylie" }
            };
            subcatStrings.Add(konkylie);

            SubcategoryStrings purpur = new()
            {
                Name = "Purpur",
                Subcategory = subcategories.FirstOrDefault(c => c.Id == 8)!,
                Keywords = new List<string>() { "purpur" }
            };
            subcatStrings.Add(purpur);

            // Kategory: steldele
            SubcategoryStrings tallerkener = new()
            {
                Name = "Tallerkener",
                Subcategory = subcategories.FirstOrDefault(c => c.Id == 9)!,
                Keywords = new List<string>() { "tallerken" }
            };
            subcatStrings.Add(tallerkener);

            SubcategoryStrings kopperOgKrus = new()
            {
                Name = "Kopper og Krus",
                Subcategory = subcategories.FirstOrDefault(c => c.Id == 10)!,
                Keywords = new List<string>() { "kop", "krus" }
            };
            subcatStrings.Add(kopperOgKrus);

            SubcategoryStrings skåle = new()
            {
                Name = "Skåle",
                Subcategory = subcategories.FirstOrDefault(c => c.Id == 11)!,
                Keywords = new List<string>() { "skål" }
            };
            subcatStrings.Add(skåle);

            SubcategoryStrings kander = new()
            {
                Name = "Kander",
                Subcategory = subcategories.FirstOrDefault(c => c.Id == 12)!,
                Keywords = new List<string>() { "kande" }
            };
            subcatStrings.Add(kander);

            SubcategoryStrings serveringsdele = new()
            {
                Name = "Serveringsdele",
                Subcategory = subcategories.FirstOrDefault(c => c.Id == 13)!,
                Keywords = new List<string>() { "fad", "opsats", "bægre", "bræd", "servering", "terrin", "asiet" }
            };
            subcatStrings.Add(serveringsdele);

            SubcategoryStrings andetIPorcelaen = new()
            {
                Name = "Serveringsdele",
                Subcategory = subcategories.FirstOrDefault(c => c.Id == 14)!,
                Keywords = new List<string>() { "ske", "krukke" }
            };
            subcatStrings.Add(andetIPorcelaen);

            // Kategori: Figurer
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

            SubcategoryStrings hunde = new()
            {
                Name = "Hunde",
                Subcategory = subcategories.FirstOrDefault(c => c.Id == 17)!,
                Keywords = new List<string>() { "hund" },
            };
            subcatStrings.Add(hunde);

            SubcategoryStrings katte = new()
            {
                Name = "Katte",
                Subcategory = subcategories.FirstOrDefault(c => c.Id == 18)!,
                Keywords = new List<string>() { "kat" },
            };
            subcatStrings.Add(katte);

            SubcategoryStrings andreDyr = new()
            {
                Name = "Andre dyr",
                Subcategory = subcategories.FirstOrDefault(c => c.Id == 19)!,
                Keywords = new List<string>() { "elefant" },
            };
            subcatStrings.Add(andreDyr);

            SubcategoryStrings mennesker = new()
            {
                Name = "Mennesker",
                Subcategory = subcategories.FirstOrDefault(c => c.Id == 20)!,
                Keywords = new List<string>() { "menneske" },
            };
            subcatStrings.Add(mennesker);

            // Kategori: Keramik
            SubcategoryStrings rc = new()
            {
                Name = "Royal Copenhagen",
                Subcategory = subcategories.FirstOrDefault(c => c.Id == 21)!,
                Keywords = new List<string>() { "royal copenhagen" },
            };
            subcatStrings.Add(rc);

            SubcategoryStrings bg = new()
            {
                Name = "Royal Copenhagen",
                Subcategory = subcategories.FirstOrDefault(c => c.Id == 22)!,
                Keywords = new List<string>() { "bing", "grøndahl" },
            };
            subcatStrings.Add(bg);

            SubcategoryStrings aluminia = new()
            {
                Name = "Aluminia",
                Subcategory = subcategories.FirstOrDefault(c => c.Id == 23)!,
                Keywords = new List<string>() { "aluminia", },
            };
            subcatStrings.Add(aluminia);

            SubcategoryStrings saxbo = new()
            {
                Name = "Saxbo",
                Subcategory = subcategories.FirstOrDefault(c => c.Id == 24)!,
                Keywords = new List<string>() { "saxbo", },
            };
            subcatStrings.Add(saxbo);

            SubcategoryStrings arneBang = new()
            {
                Name = "Arne bang",
                Subcategory = subcategories.FirstOrDefault(c => c.Id == 25)!,
                Keywords = new List<string>() { "arne bang", },
            };
            subcatStrings.Add(arneBang);

            SubcategoryStrings palshus = new()
            {
                Name = "Palshus",
                Subcategory = subcategories.FirstOrDefault(c => c.Id == 26)!,
                Keywords = new List<string>() { "palshus", },
            };
            subcatStrings.Add(palshus);

            SubcategoryStrings oevrigKeramik = new()
            {
                Name = "Øvrig Keramik",
                Subcategory = subcategories.FirstOrDefault(c => c.Id == 27)!,
                Keywords = new List<string>() { "keramik", },
            };
            subcatStrings.Add(oevrigKeramik);

            // Kategori: Guld & Sølv
            SubcategoryStrings brocher = new()
            {
                Name = "Brocher",
                Subcategory = subcategories.FirstOrDefault(c => c.Id == 28)!,
                Keywords = new List<string>() { "broche", },
            };
            subcatStrings.Add(brocher);

            SubcategoryStrings halskaeder = new()
            {
                Name = "Halskæder",
                Subcategory = subcategories.FirstOrDefault(c => c.Id == 29)!,
                Keywords = new List<string>() { "halskæde", },
            };
            subcatStrings.Add(halskaeder);

            SubcategoryStrings smykker = new()
            {
                Name = "Smykker",
                Subcategory = subcategories.FirstOrDefault(c => c.Id == 30)!,
                Keywords = new List<string>() { "smykke", },
            };
            subcatStrings.Add(smykker);

            SubcategoryStrings oevrigGuldOgSoelv = new()
            {
                Name = "Øvrige",
                Subcategory = subcategories.FirstOrDefault(c => c.Id == 31)!,
                Keywords = new List<string>() { "guld", "sølv", "bøsse" },
            };
            subcatStrings.Add(oevrigGuldOgSoelv);

            // Kategori: Bestik
            SubcategoryStrings kageBestik = new()
            {
                Name = "Kagebestik",
                Subcategory = subcategories.FirstOrDefault(c => c.Id == 32)!,
                Keywords = new List<string>() { "kagegaffel", "kage" },
            };
            subcatStrings.Add(kageBestik);

            SubcategoryStrings knive = new()
            {
                Name = "Knive",
                Subcategory = subcategories.FirstOrDefault(c => c.Id == 33)!,
                Keywords = new List<string>() { "kniv" },
            };
            subcatStrings.Add(knive);

            SubcategoryStrings gafler = new()
            {
                Name = "Gafler",
                Subcategory = subcategories.FirstOrDefault(c => c.Id == 34)!,
                Keywords = new List<string>() { "gaffel", "gafler" },
            };
            subcatStrings.Add(gafler);

            SubcategoryStrings skeer = new()
            {
                Name = "Skeer",
                Subcategory = subcategories.FirstOrDefault(c => c.Id == 35)!,
                Keywords = new List<string>() { "ske" },
            };
            subcatStrings.Add(skeer);

            SubcategoryStrings fiskeBestik = new()
            {
                Name = "Fiskebestik",
                Subcategory = subcategories.FirstOrDefault(c => c.Id == 36)!,
                Keywords = new List<string>() { "fisk", "hummer" },
            };
            subcatStrings.Add(fiskeBestik);

            // Kategori: platter
            SubcategoryStrings aluminiaPlatter = new()
            {
                Name = "Aluminia platter",
                Subcategory = subcategories.FirstOrDefault(c => c.Id == 37)!,
                Keywords = new List<string>() { "aluminia" },
            };
            subcatStrings.Add(aluminiaPlatter);

            SubcategoryStrings bjoernWinblaadPlatter = new()
            {
                Name = "Bjørn Wiinblad platter",
                Subcategory = subcategories.FirstOrDefault(c => c.Id == 38)!,
                Keywords = new List<string>() { "bjørn", "wiinblaad" },
            };
            subcatStrings.Add(bjoernWinblaadPlatter);

            SubcategoryStrings bgPlatter = new()
            {
                Name = "Bing & Grøndahl platter",
                Subcategory = subcategories.FirstOrDefault(c => c.Id == 39)!,
                Keywords = new List<string>() { "bing", "grøndahl" },
            };
            subcatStrings.Add(bgPlatter);

            SubcategoryStrings rcPlatter = new()
            {
                Name = "Royal Copenhagen platter",
                Subcategory = subcategories.FirstOrDefault(c => c.Id == 40)!,
                Keywords = new List<string>() { "royal", "copenhagen" },
            };
            subcatStrings.Add(rcPlatter);

            // Kategori: Glas

            SubcategoryStrings vinglas = new()
            {
                Name = "Vinglas",
                Subcategory = subcategories.FirstOrDefault(c => c.Id == 41)!,
                Keywords = new List<string>() { "vin" },
            };
            subcatStrings.Add(vinglas);

            SubcategoryStrings dessertglas = new()
            {
                Name = "Dessertglas",
                Subcategory = subcategories.FirstOrDefault(c => c.Id == 42)!,
                Keywords = new List<string>() { "dessert" },
            };
            subcatStrings.Add(dessertglas);

            SubcategoryStrings oelglas = new()
            {
                Name = "Ølglas",
                Subcategory = subcategories.FirstOrDefault(c => c.Id == 43)!,
                Keywords = new List<string>() { "øl" },
            };
            subcatStrings.Add(oelglas);

            SubcategoryStrings vaser = new()
            {
                Name = "Vaser",
                Subcategory = subcategories.FirstOrDefault(c => c.Id == 44)!,
                Keywords = new List<string>() { "vase" },
            };
            subcatStrings.Add(vaser);

            return subcatStrings;
        }
    }

}
