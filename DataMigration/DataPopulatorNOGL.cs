using ClassLibrary.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Text.Json;

namespace DataMigration
{
    public class DataPopulatorNOGL
    {

        private readonly GroenlundDbContext _context;

        public DataPopulatorNOGL(GroenlundDbContext context)
        {
            _context = context;
        }

        public void PopulateSomeDataToJSON()
        {
            var categories = CreateCategoriesList();
            var subCategories = CreateSubcategories(categories);

            //string folderPath = Path.Combine(AppContext.BaseDirectory, "DemoData", "SøborgData");
            //Directory.CreateDirectory(folderPath);

            // Serialize the categories list to JSON
            //string categoriesJson = JsonSerializer.Serialize(categories);
            //string subCategoriesJson = JsonSerializer.Serialize(subCategories);

            //// Write the JSON to a file in the SøborgData folder
            //string categoriesPath = Path.Combine(folderPath, "categoriesSøborg.json");
            //string subcategoriesPath = Path.Combine(folderPath, "subcategoriesSøborg.json");
            //File.WriteAllText(categoriesPath, categoriesJson);
            //File.WriteAllText(subcategoriesPath, subCategoriesJson);
            //var json = File.ReadAllText(subcategoriesPath);
            //List<Subcategory> test = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Subcategory>>(json)!;

            //foreach (var item in test)
            //{
            //    Console.WriteLine(item.Name);
            //}

            //CreateCategoriesInDatabase();
            //CreateSubcategoriesInDatabase();
        }

        private void CreateCategoriesInDatabase()
        {
            var categories = _context.Categories;
            if (categories.Any())
            {
                // Deletes all rows/data in the table
                _context.Categories.ExecuteDelete();
                _context.SaveChanges();
                Console.WriteLine("Database contains categories");
                Console.WriteLine("Deleting all categories");
            }

            Console.WriteLine("Creating categories");

            // var categoriesJson = DemoDataRepository.GetCategories();
            foreach (var category in CreateCategoriesList())
            {
                try
                {
                    _context.Categories.Add(category);
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }

            Console.WriteLine("Successfully created categories");
        }

        private void CreateSubcategoriesInDatabase()
        {
            var subcategories = _context.Subcategories;
            if (subcategories.Any())
            {
                // Deletes all rows/data in the table
                _context.Subcategories.ExecuteDelete();
                _context.SaveChanges();
                Console.WriteLine("Database contains subcategories");
                Console.WriteLine("Deleting all subcategories");
            }

            Console.WriteLine("Creating subcategories");

            // var subcategoriesJson = DemoDataRepository.GetSubcategories();
            foreach (var subcategory in CreateSubcategories(CreateCategoriesList()))
            {
                try
                {
                    _context.Subcategories.Add(subcategory);
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }

            Console.WriteLine("Successfully created subcategories\n");
        }

        public List<Category> CreateCategoriesList()
        {
            return new List<Category>
                {
                    new Category { Id = 0, Name = "Stel|Chineese Export", Order = 1, ImageUrl = "https://via.placeholder.com/600x400", Description = "Spisestel & Kaffestel | For Dining & Coffee" },
                    new Category { Id = 1, Name = "Figurer|Figurines", Order = 2, ImageUrl = "https://via.placeholder.com/600x400", Description = "Figurer & Skulpturer | Figurines & Sculptures" },
                    new Category { Id = 2, Name = "Smykker|Jewerly", Order = 3, ImageUrl = "https://via.placeholder.com/600x400", Description = "Smykker | Jewelry" },
                    new Category { Id = 3, Name = "Sølv|Silver", Order = 4, ImageUrl = "https://via.placeholder.com/600x400", Description = "Sølvtøj & Sølvsmykker | Silverware and Silver trinkets" },
                    new Category { Id = 4, Name = "Guld|Gold", Order = 5, ImageUrl = "https://via.placeholder.com/600x400", Description = "Guld og Guldsmykker | Gold and Gold trinkets" },
                    new Category { Id = 5, Name = "Bestik|Cutlery", Order = 6, ImageUrl = "https://via.placeholder.com/600x400", Description = "Bistik | Cutlery" },
                    new Category { Id = 6, Name = "Keramik|Ceramics", Order = 7, ImageUrl = "https://via.placeholder.com/600x400", Description = "Keramik og Stentøj | Ceramics and Stonework" },
                    new Category { Id = 7, Name = "Platter|Plaques", Order = 8, ImageUrl = "https://via.placeholder.com/600x400", Description = "Platter | Plaques and Plates" },
                    new Category { Id = 8, Name = "Malerier|Paintings", Order = 9, ImageUrl = "https://via.placeholder.com/600x400", Description = "Malerier | Paintings" },
                    new Category { Id = 9, Name = "Lamper|Lamps", Order = 10, ImageUrl = "https://via.placeholder.com/600x400", Description = "Lamper | Lamps" },
                    new Category { Id = 10, Name = "Glas|Glass", Order = 11, ImageUrl = "https://via.placeholder.com/600x400", Description = "Glas | Glass" }
                };
        }

        public List<Subcategory> CreateSubcategories(List<Category> categories)
        {
            return new List<Subcategory>
                {
                    // Categories 1-11 are for Stel
                    new Subcategory { Id = 0, CategoryId = categories[0].Id, Name = "Blåmalet", Order = 1, ImageUrl = "https://via.placeholder.com/600x400", Description = "Blåmalet Stel" },
                    new Subcategory { Id = 1, CategoryId = categories[0].Id, Name = "Empire", Order = 2, ImageUrl = "https://via.placeholder.com/600x400", Description = "Empire Stel" },
                    new Subcategory { Id = 2, CategoryId = categories[0].Id, Name = "Fiskestel", Order = 3, ImageUrl = "https://via.placeholder.com/600x400", Description = "Fiskestel" },
                    new Subcategory { Id = 3, CategoryId = categories[0].Id, Name = "Flora Danica", Order = 4, ImageUrl = "https://via.placeholder.com/600x400", Description = "Flora Danica Stel" },
                    new Subcategory { Id = 4, CategoryId = categories[0].Id, Name = "Grøn Blomst Svejfet", Order = 5, ImageUrl = "https://via.placeholder.com/600x400", Description = "Grøn Blomst Svejfet Stel" },
                    new Subcategory { Id = 5, CategoryId = categories[0].Id, Name = "Guld Vifte", Order = 6, ImageUrl = "https://via.placeholder.com/600x400", Description = "Guld Vifte Stel" },
                    new Subcategory { Id = 6, CategoryId = categories[0].Id, Name = "Henriette", Order = 7, ImageUrl = "https://via.placeholder.com/600x400", Description = "Henriette Stel" },
                    new Subcategory { Id = 7, CategoryId = categories[0].Id, Name = "Konkylie Hvid", Order = 8, ImageUrl = "https://via.placeholder.com/600x400", Description = "Konkylie Hvid Stel" },
                    new Subcategory { Id = 8, CategoryId = categories[0].Id, Name = "Konkylie Rosa", Order = 9, ImageUrl = "https://via.placeholder.com/600x400", Description = "Konkylie Rosa Stel" },
                    new Subcategory { Id = 9, CategoryId = categories[0].Id, Name = "Mega Blå", Order = 10, ImageUrl = "https://via.placeholder.com/600x400", Description = "Mega Blå Stel" },
                    new Subcategory { Id = 10, CategoryId = categories[0].Id, Name = "Musselmalet", Order = 11, ImageUrl = "https://via.placeholder.com/600x400", Description = "Musselmalet Stel" },
                    // Categories 12-22 are for Figurer
                    new Subcategory { Id = 11, CategoryId = categories[1].Id, Name = "Bjørne", Order = 1, ImageUrl = "https://via.placeholder.com/600x400", Description = "Bjørne Figurer" },
                    new Subcategory { Id = 12, CategoryId = categories[1].Id, Name = "Fugle", Order = 2, ImageUrl = "https://via.placeholder.com/600x400", Description = "Fugle Figurer" },
                    new Subcategory { Id = 13, CategoryId = categories[1].Id, Name = "Hunde", Order = 3, ImageUrl = "https://via.placeholder.com/600x400", Description = "Hunde Figurer" },
                    new Subcategory { Id = 14, CategoryId = categories[1].Id, Name = "Kat", Order = 4, ImageUrl = "https://via.placeholder.com/600x400", Description = "Kat Figurer" },
                    new Subcategory { Id = 15, CategoryId = categories[1].Id, Name = "Mænd og kvinder", Order = 5, ImageUrl = "https://via.placeholder.com/600x400", Description = "Mænd og kvinder Figurer" },
                    new Subcategory { Id = 16, CategoryId = categories[1].Id, Name = "Mus", Order = 6, ImageUrl = "https://via.placeholder.com/600x400", Description = "Mus Figurer" },
                    new Subcategory { Id = 17, CategoryId = categories[1].Id, Name = "Nisser", Order = 7, ImageUrl = "https://via.placeholder.com/600x400", Description = "Nisser Figurer" },
                    new Subcategory { Id = 18, CategoryId = categories[1].Id, Name = "Papegøjer", Order = 8, ImageUrl = "https://via.placeholder.com/600x400", Description = "Papegøjer Figurer" },
                    new Subcategory { Id = 19, CategoryId = categories[1].Id, Name = "Pattedyr", Order = 9, ImageUrl = "https://via.placeholder.com/600x400", Description = "Pattedyr Figurer" },
                    new Subcategory { Id = 20, CategoryId = categories[1].Id, Name = "Piger", Order = 10, ImageUrl = "https://via.placeholder.com/600x400", Description = "Piger Figurer" },
                    new Subcategory { Id = 21, CategoryId = categories[1].Id, Name = "Skildpadder", Order = 11, ImageUrl = "https://via.placeholder.com/600x400", Description = "Skildpadder Figurer" },
                    // Categories 23-33 are for Vaser
                    new Subcategory { Id = 22, CategoryId = categories[2].Id, Name = "Aluminia", Order = 1, ImageUrl = "https://via.placeholder.com/600x400", Description = "Aluminia Vaser" },
                    new Subcategory { Id = 23, CategoryId = categories[2].Id, Name = "Baca", Order = 2, ImageUrl = "https://via.placeholder.com/600x400", Description = "Baca Vaser" },
                    new Subcategory { Id = 24, CategoryId = categories[2].Id, Name = "Cylinder", Order = 3, ImageUrl = "https://via.placeholder.com/600x400", Description = "Cylinder Vaser" },
                    new Subcategory { Id = 25, CategoryId = categories[2].Id, Name = "Eva", Order = 4, ImageUrl = "https://via.placeholder.com/600x400", Description = "Eva Vaser" },
                    new Subcategory { Id = 26, CategoryId = categories[2].Id, Name = "Jens Harald Quistgaard", Order = 5, ImageUrl = "https://via.placeholder.com/600x400", Description = "Jens Harald Quistgaard Vaser" },
                    new Subcategory { Id = 27, CategoryId = categories[2].Id, Name = "Kunstfajance", Order = 6, ImageUrl = "https://via.placeholder.com/600x400", Description = "Kunstfajance Vaser" },
                    // Categories 23-27 are for Smykker
                    new Subcategory { Id = 28, CategoryId = categories[3].Id, Name = "Armbånd", Order = 1, ImageUrl = "https://via.placeholder.com/600x400", Description = "Armbånd Smykker" },
                    new Subcategory { Id = 29, CategoryId = categories[3].Id, Name = "Halskæder", Order = 2, ImageUrl = "https://via.placeholder.com/600x400", Description = "Halskæder Smykker" },
                    new Subcategory { Id = 30, CategoryId = categories[3].Id, Name = "Øreringe", Order = 3, ImageUrl = "https://via.placeholder.com/600x400", Description = "Øreringe Smykker" },
                    new Subcategory { Id = 31, CategoryId = categories[3].Id, Name = "Ringe", Order = 4, ImageUrl = "https://via.placeholder.com/600x400", Description = "Ringe Smykker" },
                    new Subcategory { Id = 32, CategoryId = categories[3].Id, Name = "Brocher", Order = 5, ImageUrl = "https://via.placeholder.com/600x400", Description = "Brocher Smykker" },
                    // Categories 28-30 are for Bestik
                    new Subcategory { Id = 42, CategoryId = categories[5].Id, Name = "bestik", Order = 1, ImageUrl = "https://via.placeholder.com/600x400", Description = "bestik Smykker" },
                    new Subcategory { Id = 43, CategoryId = categories[5].Id, Name = "Julebestik", Order = 2, ImageUrl = "https://via.placeholder.com/600x400", Description = "Julebestik Smykker" },
                    new Subcategory { Id = 44, CategoryId = categories[5].Id, Name = "Mindebestik", Order = 3, ImageUrl = "https://via.placeholder.com/600x400", Description = "Mindebestik Smykker" },
                    new Subcategory { Id = 45, CategoryId = categories[5].Id, Name = "Andre bestik", Order = 4, ImageUrl = "https://via.placeholder.com/600x400", Description = "Andre bestik Smykker" },
                    // Categories 31-34 are for Keramik
                    new Subcategory { Id = 46, CategoryId = categories[6].Id, Name = "Bjørn Wiinblad", Order = 1, ImageUrl = "https://via.placeholder.com/600x400", Description = "Bjørn Wiinblad Keramik" },
                    new Subcategory { Id = 47, CategoryId = categories[6].Id, Name = "Ipsen", Order = 2, ImageUrl = "https://via.placeholder.com/600x400", Description = "Ipsen Keramik" },
                    new Subcategory { Id = 48, CategoryId = categories[6].Id, Name = "Kähler", Order = 3, ImageUrl = "https://via.placeholder.com/600x400", Description = "Kähler Keramik" },
                    new Subcategory { Id = 49, CategoryId = categories[6].Id, Name = "Saxbo", Order = 4, ImageUrl = "https://via.placeholder.com/600x400", Description = "Saxbo Keramik" },
                    new Subcategory { Id = 50, CategoryId = categories[6].Id, Name = "Arne bang", Order = 5, ImageUrl = "https://via.placeholder.com/600x400", Description = "Arne bang Keramik" },
                    new Subcategory { Id = 51, CategoryId = categories[6].Id, Name = "Palshus", Order = 6, ImageUrl = "https://via.placeholder.com/600x400", Description = "Palshus Keramik" },
                    // Categories 35-36 are for Platter
                    new Subcategory { Id = 52, CategoryId = categories[7].Id, Name = "Platter", Order = 1, ImageUrl = "https://via.placeholder.com/600x400", Description = "Platter " + categories[7].Name },
                    new Subcategory { Id = 53, CategoryId = categories[7].Id, Name = "Årskrus", Order = 2, ImageUrl = "https://via.placeholder.com/600x400", Description = "Årskrus " + categories[7].Name },
                    // Categories 37-38 are for Malerier
                    new Subcategory { Id = 54, CategoryId = categories[8].Id, Name = "Moderne malerier", Order = 1, ImageUrl = "https://via.placeholder.com/600x400", Description = "Moderne malerier " + categories[8].Name },
                    new Subcategory { Id = 55, CategoryId = categories[8].Id, Name = "Ældre malerier", Order = 2, ImageUrl = "https://via.placeholder.com/600x400", Description = "Ældre malerier " + categories[8].Name },
                    // Categories 39-40 are for Lamper
                    new Subcategory { Id = 56, CategoryId = categories[9].Id, Name = "Loftslamper", Order = 1, ImageUrl = "https://via.placeholder.com/600x400", Description = "Loftslamper Antik og Kunst" },
                    new Subcategory { Id = 57, CategoryId = categories[9].Id, Name = "Bordlamper", Order = 2, ImageUrl = "https://via.placeholder.com/600x400", Description = "Bordlamper Antik og Kunst" },
                    new Subcategory { Id = 58, CategoryId = categories[9].Id, Name = "Væglamper", Order = 3, ImageUrl = "https://via.placeholder.com/600x400", Description = "Væglamper Antik og Kunst" },
                    new Subcategory { Id = 59, CategoryId = categories[9].Id, Name = "Standere", Order = 4, ImageUrl = "https://via.placeholder.com/600x400", Description = "Standere Antik og Kunst" },
                    new Subcategory { Id = 60, CategoryId = categories[9].Id, Name = "Andet", Order = 5, ImageUrl = "https://via.placeholder.com/600x400", Description = "Andet Antik og Kunst" },
                    // Categories 41-42 are for Glas
                    new Subcategory { Id = 61, CategoryId = categories[10].Id, Name = "Vaser", Order = 1, ImageUrl = "https://via.placeholder.com/600x400", Description = "Vaser Antik og Kunst" },
                    new Subcategory { Id = 62, CategoryId = categories[10].Id, Name = "Skåle", Order = 2, ImageUrl = "https://via.placeholder.com/600x400", Description = "Skåle Antik og Kunst" },
                    new Subcategory { Id = 63, CategoryId = categories[10].Id, Name = "Kander", Order = 3, ImageUrl = "https://via.placeholder.com/600x400", Description = "Kander Antik og Kunst" },
                    new Subcategory { Id = 64, CategoryId = categories[10].Id, Name = "Glasfigurer", Order = 4, ImageUrl = "https://via.placeholder.com/600x400", Description = "Glasfigurer Antik og Kunst" },
                    // Categories 43-44 are for Sølv
                    new Subcategory { Id = 65, CategoryId = categories[3].Id, Name = "Bestik", Order = 1, ImageUrl = "https://via.placeholder.com/600x400", Description = "Bestik forhandlet af " + categories[3].Name },
                    new Subcategory { Id = 66, CategoryId = categories[3].Id, Name = "Lysestager", Order = 2, ImageUrl = "https://via.placeholder.com/600x400", Description = "Lysestager forhandlet af " + categories[3].Name },
                    new Subcategory { Id = 67, CategoryId = categories[3].Id, Name = "Fade", Order = 3, ImageUrl = "https://via.placeholder.com/600x400", Description = "Fade forhandlet af " + categories[3].Name },
                    new Subcategory { Id = 68, CategoryId = categories[3].Id, Name = "Kander", Order = 4, ImageUrl = "https://via.placeholder.com/600x400", Description = "Kander forhandlet af " + categories[3].Name },
                    new Subcategory { Id = 69, CategoryId = categories[3].Id, Name = "Diverse", Order = 5, ImageUrl = "https://via.placeholder.com/600x400", Description = "Diverse sølvting forhandlet af " + categories[3].Name },
                    // Categories 45-46 are for Guld
                    new Subcategory { Id = 70, CategoryId = categories[4].Id, Name = "Ringe", Order = 2, ImageUrl = "https://via.placeholder.com/600x400", Description = "Ringe Smykker" },
                    new Subcategory { Id = 71, CategoryId = categories[4].Id, Name = "Øreringe", Order = 2, ImageUrl = "https://via.placeholder.com/600x400", Description = "Øreringe Smykker" },
                    new Subcategory { Id = 72, CategoryId = categories[4].Id, Name = "Armbånd", Order = 3, ImageUrl = "https://via.placeholder.com/600x400", Description = "Armbånd Smykker" },
                    new Subcategory { Id = 73, CategoryId = categories[4].Id, Name = "Halskæder", Order = 4, ImageUrl = "https://via.placeholder.com/600x400", Description = "Halskæder Smykker" },
                    new Subcategory { Id = 74, CategoryId = categories[4].Id, Name = "Brocher", Order = 5, ImageUrl = "https://via.placeholder.com/600x400", Description = "Brocher Smykker" }
                };
        }
    }
}
