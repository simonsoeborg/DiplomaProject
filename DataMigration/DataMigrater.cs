using ClassLibrary.Models;
using DataMigration.Helpers;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace DataMigration
{

    class DataMigrater
    {
        public RegexHelper RegexHelper;
        private readonly GroenlundDbContext _context;

        public DataMigrater()
        {
            RegexHelper = new();
            _context = new();
        }

        public void PrintProducts()
        {
            int counter = 0;
            var products = _context.Products.Include(p => p.Subcategories).ThenInclude(s => s.Category).ToList();
            foreach (var product in products)
            {
                counter++;
                Console.WriteLine(product + "\n");
                foreach (var subcategory in product.Subcategories)
                {
                    Console.WriteLine("Subcategory's category: " + subcategory.Category.Name);
                }
                Console.WriteLine("\n\n\n");
            }
            Console.WriteLine("Counted {0} Products", counter);
        }

        public void CreateJsonFiles()
        {
            var (Products, ProductItems, Images) = ExtractProducts();

            var jsonString = JsonConvert.SerializeObject(Products, Formatting.Indented);
            var fileName = "products.json";
            File.WriteAllText(fileName, jsonString);
            Console.WriteLine("JSON file saved to " + fileName);

            jsonString = JsonConvert.SerializeObject(ProductItems, Formatting.Indented);
            fileName = "productitems.json";
            File.WriteAllText(fileName, jsonString);
            Console.WriteLine("JSON file saved to " + fileName);

            jsonString = JsonConvert.SerializeObject(Images, Formatting.Indented);
            fileName = "images.json";
            File.WriteAllText(fileName, jsonString);
            Console.WriteLine("JSON file saved to " + fileName);

        }


        public (List<Product> Products, List<ProductItem> ProductItems, List<Image> Images) ExtractProducts()
        {
            List<string[]> data = GetCsvEntries();
            List<string[]> failedMatches = new();
            List<Product> Products = new();
            List<ProductItem> ProductItems = new();
            List<Image> Images = new();
            List<Category> categories = _context.Categories.ToList();
            List<Subcategory> subcategories = _context.Subcategories.ToList();
            int productIdCounter = 1;
            int productItemIdCounter = 1;
            int imageIdCounter = 1;

            for (int i = 1; i < data.Count; i++)
            {
                var dataItem = data[i];
                var (name, modelNumber) = RegexHelper.RecognizeModelnumberPattern(dataItem[2]);
                string categoryStringInput = dataItem[2] + dataItem[3] + dataItem[5];
                var productCategory = CategoryHelper.InferCategory(categories, categoryStringInput);
                if (productCategory == null)
                {
                    failedMatches.Add(dataItem);
                    continue;
                }

                var productSubcategories = CategoryHelper.ExtractSubcategories(productCategory, subcategories, categoryStringInput);
                var imageUrls = ExtractImages(dataItem[4]);

                if (string.IsNullOrEmpty(name) || productSubcategories.Count == 0 || imageUrls.Count == 0)
                {
                    // No point in having this data in the project, but saving it for future reference to PO.
                    failedMatches.Add(dataItem);
                    continue;
                }

                Product product = new()
                {
                    Id = productIdCounter,
                    Name = name,
                    ModelNumber = modelNumber,
                    Material = ExtractMaterialType(dataItem[3]),
                    Manufacturer = ExtractManufacturer(dataItem[3]),
                    Design = ExtractDesign(dataItem[3]),
                    Subcategories = productSubcategories,
                    Dimension = ExtractDimension(dataItem[3]),
                };

                // How many productItems for this product
                var productItemCount = ExtractProductItemCount(dataItem[13]);

                for (int x = 0; x < productItemCount; x++)
                {

                    // Create productItem for the product
                    decimal currentPrice = ExtractPrice(dataItem[8]);
                    decimal purchasePrice = currentPrice * (decimal)0.3;
                    decimal? weight = ExtractWeight(dataItem[14]);
                    weight ??= (decimal)new Random().NextDouble() * 15;

                    ProductItem productItem = new()
                    {
                        Id = productItemIdCounter,
                        Product = product,
                        ProductId = product.Id,
                        Condition = ExtractCondition(dataItem[3]),
                        Quality = ExtractQuality(dataItem[3]),
                        Sold = 0,
                        Weight = weight,
                        CurrentPrice = currentPrice,
                        PurchasePrice = purchasePrice,
                        CreatedDate = RandomDay(),
                        CustomText = "",
                        Images = new List<Image>()
                    };

                    bool? sold = ExtractSold(dataItem[10]);
                    if (sold != null && sold == true)
                    {
                        productItem.Sold = 1;
                        int randomDaysOnSale = new Random().Next(1, 150);
                        productItem.SoldDate = productItem.CreatedDate + TimeSpan.FromDays(randomDaysOnSale);
                    }

                    List<Image> productItemImages = new();
                    foreach (var img in imageUrls)
                    {
                        if (!string.IsNullOrEmpty(img))
                        {
                            var imgId = img.Split('.')[0];
                            string imgReducedSize = "https://static.wixstatic.com/media/" + img + "/v1/fill/w_630,h_840,al_c,q_85,usm_0.66_1.00_0.01/" + imgId + ".webp";
                            //Console.WriteLine(imgId);
                            //Console.WriteLine(imgReducedSize);

                            Image image = new()
                            {
                                ProductItemId = productItem.Id,
                                Id = imageIdCounter,
                                Url = imgReducedSize,
                            };
                            productItemImages.Add(image);
                            imageIdCounter++;
                        }
                    }


                    ProductItems.Add(productItem);
                    Images.AddRange(productItemImages);
                    productItemIdCounter++;
                }
                Products.Add(product);
                productIdCounter++;
            }

            Console.WriteLine("Added data to {0} products generated from {1} dataItems", Products.Count, data.Count);
            return (Products, ProductItems, Images);
        }
        public void TestRegExFilter()
        {
            List<string[]> data = ReadCsv("./products.csv");
            List<string> names = new();

            for (int i = 1; i < 5; i++)
            {
                var dataItem = data[i];
                names.Add(dataItem[2]);
            }

            RegexHelper.TestRegexFilter(names.ToArray(), RegexHelper.RegexMap());
        }

        public decimal? ExtractWeight(string input)
        {
            bool success = double.TryParse(input, out double sum);
            if (success)
            {
                return (decimal)sum;
            }
            return null;
        }

        public int ExtractProductItemCount(string input)
        {
            var parseSuccessful = int.TryParse(input, out int parseResult);
            if (parseSuccessful)
            {
                if (parseResult != 0)
                {
                    return parseResult;
                }
            }
            return 1;
        }


        public DateTime RandomDay()
        {
            DateTime start = new(2019, 1, 1);
            int range = (DateTime.Today - start).Days;
            return start.AddDays(new Random().Next(range));
        }

        // TODO - This should be finished if time allows for it.
        public string ExtractDimension(string input)
        {
            string result = "";
            //int startIndex = 0;
            //// Dimension can be Height / Højde - 
            //if (input.ToLowerInvariant().Contains("height"))
            //{
            //    startIndex = input.ToLowerInvariant().IndexOf("height") + "height".Length;

            //}
            //else if (input.ToLowerInvariant().Contains("højde"))
            //{
            //    startIndex = input.ToLowerInvariant().IndexOf("height") + "height".Length;
            //}
            //if (startIndex != 0)
            //{
            //    string substring = input.ToLowerInvariant().Substring(startIndex);
            //    result = substring;
            //}

            return result;
        }

        public List<string> ExtractImages(string input)
        {
            List<string> images = new();
            if (input.Contains("jpg") || input.Contains("wepb"))
            {
                string[] result = input.Split(';');
                foreach (string s in result)
                {
                    images.Add(s.Trim('\"'));
                }
            }

            return images;
        }
        struct Manufacturer
        {
            public string name;
            public string[] keywords;
        }

        public string ExtractManufacturer(string input)
        {
            Manufacturer[] manufacturers = new Manufacturer[]
            {
                new Manufacturer { name = "Royal Copenhagen", keywords = new string[] { "Royal", "Copenhagen" } },
                new Manufacturer { name = "Bing og Grøndahl", keywords = new string[] { "Bing", "Grøndahl", "Bing og Grøndahl" } },
                new Manufacturer { name = "Axel Brüel", keywords = new string[] { "Axel", "Brüel" } },
                new Manufacturer { name = "Dahl Jensen", keywords = new string[] { "Dahl Jensen" } },
                new Manufacturer { name = "Kähler", keywords = new string[] { "Kähler" } },
                new Manufacturer { name = "Holmegaard", keywords = new string[] { "Holmegaard" } },
                new Manufacturer { name = "Georg Jensen", keywords = new string[] { "Georg Jensen" } },
                new Manufacturer { name = "Orrefors", keywords = new string[] { "Orrefors" } },
                new Manufacturer { name = "Hjorth", keywords = new string[] { "Hjorth" } },
                new Manufacturer { name = "Knabstrup", keywords = new string[] { "Knabstrup" } },
                new Manufacturer { name = "Lyngby Porcelæn", keywords = new string[] { "Lyngby Porcelæn" } },
                new Manufacturer { name = "Aluminia", keywords = new string[] { "Aluminia" } },
                new Manufacturer { name = "Nymølle Keramik", keywords = new string[] { "Nymølle Keramik" } },
                new Manufacturer { name = "Saxbo Keramik", keywords = new string[] { "Saxbo Keramik" } },
                new Manufacturer { name = "Dansk Glasværk", keywords = new string[] { "Dansk Glasværk" } },
                new Manufacturer { name = "Palshus Keramik", keywords = new string[] { "Palshus Keramik" } },
                new Manufacturer { name = "Arne Bang", keywords = new string[] { "Arne Bang" } },
                new Manufacturer { name = "Søholm", keywords = new string[] { "Søholm" } },
                new Manufacturer { name = "Ipsen & Co.", keywords = new string[] { "Ipsen & Co" } },
                new Manufacturer { name = "A. Michelsen", keywords = new string[] { "A. Michelsen", "Anton Michelsen" } },
                new Manufacturer { name = "Cohr", keywords = new string[] { "Cohr" } },
                new Manufacturer { name = "Evald Nielsen", keywords = new string[] { "Evald Nielsen" } },
                new Manufacturer { name = "Hans Hansen", keywords = new string[] { "Hans Hansen" } },
                new Manufacturer { name = "Kay Bojesen", keywords = new string[] { "Kay Bojesen" } },
                new Manufacturer { name = "Carl Hansen & Søn", keywords = new string[] { "Carl Hansen & Søn" } },
                new Manufacturer { name = "Lyngby Glasværk", keywords = new string[] { "Lyngby Glasværk"}},
                new Manufacturer { name = "Grønlund & Lefort", keywords = new string[]  {"Grønlund", "Lefort"}},
                new Manufacturer { name = "Gun Von Wittrock", keywords = new string[] { "GUN VON WITTROCK" } },
                new Manufacturer { name = "Michael Andersen", keywords = new string[] {"Michael Andersen"} },

            };

            string result = "";

            foreach (var manufacturer in manufacturers)
            {
                foreach (var keyword in manufacturer.keywords)
                {
                    if (input.ToLowerInvariant().Contains(keyword.ToLowerInvariant()))
                    {
                        result = manufacturer.name;
                    }

                }
            }

            return result;
        }
        public string ExtractDesign(string input)
        {
            string result = "";
            int startIndex = input.IndexOf("Design");

            if (startIndex != -1)
            {
                startIndex += "Design".Length;
                string subString = input.Substring(startIndex, 40)
                    .Trim('&', ' ', ';', ':', '"')
                    .Split('<')[0]
                    .Trim()
                    .Replace("&nbsp", "")
                    .Replace("&#160;", "")
                    .Replace("&amp;nbsp;", "");

                result = subString;
            }

            return result;
        }

        public bool? ExtractSold(string input)
        {
            input = input.ToLowerInvariant();
            if (input.Contains("true"))
            {
                return false;
            }
            if (input.Contains("false"))
            {
                return true;
            }
            return null;
        }

        public QualityType ExtractQuality(string input)
        {
            string pattern = @"(1|2|3)\.\s*(?i)(quality|sortering)";

            Match match = Regex.Match(input, pattern);
            if (match.Success)
            {
                //string phrase = match.Groups[2].Value;
                switch (match.Groups[1].Value)
                {
                    case "1":
                        return QualityType.FirstQuality;
                    case "2":
                        return QualityType.SecondQuality;
                    case "3":
                        return QualityType.ThirdQuality;
                }
            }
            return QualityType.Undefined;
        }

        public ConditionType ExtractCondition(string input)
        {
            input = input.ToLowerInvariant();

            if (input.Contains("skår") || input.Contains("shards"))
            {
                if (input.Contains("mange") || input.Contains("many"))
                {
                    return ConditionType.ManyShards;
                }
                if (input.Contains("få") || input.Contains("few"))
                {
                    return ConditionType.FewShards;
                }
                if (input.Contains("ingen") || input.Contains("no"))
                {
                    return ConditionType.NoShards;
                }
            }
            return ConditionType.Undefined;
        }

        public decimal ExtractPrice(string input)
        {
            double.TryParse(input, out double result);
            if (double.IsNaN(result) || double.IsInfinity(result) || result < 0)
            {
                result = 149.1;
            }
            return (decimal)result;
        }

        public MaterialType ExtractMaterialType(string input)
        {
            string substring = "";

            Regex regexPattern = new(@"(?:Material|Materiale):\s*(.*)");
            var match = regexPattern.Match(input);
            if (match.Success)
            {
                substring = match.Groups[1].Value;
                int endIndex = substring.IndexOf("<");
                substring = substring[..endIndex];
            }

            string lowercaseSubstring = substring.ToLowerInvariant();
            if (lowercaseSubstring.Contains("porcelæn") || lowercaseSubstring.Contains("porcelain")) { return MaterialType.porcelain; }
            if (lowercaseSubstring.Contains("stål") || lowercaseSubstring.Contains("steel")) { return MaterialType.steel; }
            if (lowercaseSubstring.Contains("glas") || lowercaseSubstring.Contains("glass")) { return MaterialType.glass; }
            if (lowercaseSubstring.Contains("guld") || lowercaseSubstring.Contains("gold")) { return MaterialType.gold; }
            if (lowercaseSubstring.Contains("sølv") || lowercaseSubstring.Contains("silver")) { return MaterialType.silver; }
            if (lowercaseSubstring.Contains("keramik") || lowercaseSubstring.Contains("ceramics")) { return MaterialType.ceramics; }
            if (lowercaseSubstring.Contains("stentøj") || lowercaseSubstring.Contains("stoneware")) { return MaterialType.stoneware; }
            if (lowercaseSubstring.Contains("fajance")) { return MaterialType.fajance; }

            return MaterialType.undefined;
        }

        public List<string[]> GetCsvEntries()
        {
            return ReadCsv("./products.csv");
        }
        private List<string[]> ReadCsv(string filename)
        {
            List<string[]> data = new();
            using (var reader = new StreamReader(filename))
            {
                string? line = reader.ReadLine();
                while (line != null)
                {
                    var values = new List<string>();
                    var currentField = "";

                    foreach (var c in line)
                    {
                        if (c == ',' && currentField.StartsWith("\"\"") && !currentField.EndsWith("\"\""))
                        {
                            // This comma is inside a field, so ignore it
                            currentField += c;
                        }
                        else if (c == ',' && currentField.EndsWith("\"\""))
                        {
                            // This comma is at the end of a quoted field, so add the field to the list and start a new field
                            values.Add(currentField);
                            currentField = "";
                        }
                        else if (c == ',' && !currentField.EndsWith("\"\""))
                        {
                            // This comma is at the end of a non-quoted field, so add the field to the list and start a new field
                            values.Add(currentField);
                            currentField = "";
                        }
                        else
                        {
                            // Add the character to the current field
                            currentField += c;
                        }
                    }

                    // Add the last field to the list
                    values.Add(currentField);

                    data.Add(values.ToArray());
                    line = reader.ReadLine();
                }
            }
            return data;
        }


    }
}

//public bool ProductIsValid(Product product, List<ProductItem> productItems)
//{
//    bool result = true;
//    if (
//        product.ModelNumber == "" ||
//        product.Material == ((int)MaterialType.undefined) ||
//        product.Design == null ||
//        product.Manufacturer == "" ||
//        product.Subcategories.Count == 0
//    )
//    {
//        result = false;
//    }
//    foreach (ProductItem po in productItems)
//    {
//        if (po.Images == null || po.Images.Count == 0)
//        {
//            result = false;
//        }
//    }

//    return result;
//}