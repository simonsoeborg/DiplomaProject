using ClassLibrary;
using CsvConverter;
using Microsoft.EntityFrameworkCore;

class Program
{
    public RegexHelper RegexHelper = new();
    public static GroenlundDbContext context = new(
        options: new DbContextOptionsBuilder<GroenlundDbContext>()
        .UseMySql("server=130.225.170.249;Database=GroenlundDb;User=GroenlundDB;Password=gl12345;port=3306;",
        ServerVersion.AutoDetect("server=130.225.170.249;Database=GroenlundDb;User=GroenlundDB;Password=gl12345;port=3306;")).Options

        );

    static void Main(string[] args)
    {

        List<string[]> data = ReadCsv("./products.csv");

        List<Product> Products = new();
        List<ProductItem> ProductItems = new();
        List<string> names = new();
        List<Subcategory> subcategories = context.Subcategories.ToList();

        for (int i = 1; i < data.Count; i++)
        {
            var dataItem = data[i];
            names.Add(dataItem[2]);
            Product product = ExtractProduct(dataItem, subcategories);
            if (ProductIsValid(product))
            {
                Products.Add(product);
                List<ProductItem> generatedProductItems = GenerateProductItems(product);
                ProductItems.AddRange(generatedProductItems);
            }


        }

        foreach (Product prod in Products)
        {
            context.Products.Add(prod);
            try
            {
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed creating product" + ex.Message);
            }
        }

        foreach (ProductItem productItem in ProductItems)
        {
            context.ProductItems.Add(productItem);
            try
            {
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed creating product" + ex.Message);
            }
        }

        //RegexHelper.TestRegexFilter(names.ToArray(), RegexHelper.RegexMap());
        int productCount = 0;
        int materialCount = 0;
        int modelNumberCount = 0;
        int designCount = 0;
        int manufacturerCount = 0;
        int subcategoryCount = 0;

        foreach (var product in Products)
        {
            if (product.Material != MaterialType.undefined)
            {
                materialCount++;
            }
            if (product.ModelNumber != "")
            {
                modelNumberCount++;
            }
            if (product.Design != "")
            {
                designCount++;
            }
            if (product.Manufacturer != "")
            {
                manufacturerCount++;
            }
            if (product.Subcategories.Count > 0)
            {
                subcategoryCount++;
                //Console.WriteLine("Subcategories for " + product.Name);
                //foreach (var subcategory in product.Subcategories)
                //{
                //    Console.WriteLine(subcategory.Name);
                //}
                //Console.WriteLine("\n\n\n");
            }
            if (product.ModelNumber != "" &&
                product.Material != MaterialType.undefined &&
                product.Design != null &&
                product.Manufacturer != "" &&
                product.Subcategories.Count > 0
                )
            {
                productCount++;
                //Console.WriteLine(product.ToString());
            }
        }

        Console.WriteLine("Materials mapped: {0}/{1}\n" +
            "Modelnumbers mapped: {2}/{1}\n" +
            "Designs mapped: {3}/{1}\n" +
            "Manufacturers mapped: {4}/{1}\n" +
            "Subcategories mapped: {5}/{1}\n",
            materialCount, data.Count, modelNumberCount, designCount, manufacturerCount, subcategoryCount);


        Console.WriteLine("Added data to {0}/{1} products generated from {2} dataItems", productCount, Products.Count, data.Count);
    }

    public static bool ProductIsValid(Product product)
    {
        bool result = true;
        if (
            product.ModelNumber == "" ||
            product.Material == MaterialType.undefined ||
            product.Design == null ||
            product.Manufacturer == "" ||
            product.Subcategories.Count == 0
        )
        {
            result = false;
        }

        return result;
    }
    public static List<ProductItem> GenerateProductItems(Product product)
    {
        List<ProductItem> productItems = new();
        int amount = new Random().Next(5);
        for (int i = 0; i <= amount; i++)
        {

            int conditionType = new Random().Next(3);
            int qualityType = new Random().Next(3);
            int purchasePrice = new Random().Next(10500);
            int currentPrice = purchasePrice * 2;
            ProductItem productItem = new()
            {
                Product = product,
                ProductId = product.Id,
                Condition = (ConditionType)conditionType,
                Quality = (QualityType)qualityType,
                Sold = 0,
                Weight = 0,
                PurchasePrice = purchasePrice,
                CurrentPrice = currentPrice,
                CreatedDate = RandomDay(),
                CustomText = "",

            };
            productItems.Add(productItem);
        }

        return productItems;

    }

    public static DateTime RandomDay()
    {
        DateTime start = new DateTime(2019, 1, 1);
        int range = (DateTime.Today - start).Days;
        return start.AddDays(new Random().Next(range));
    }

    public static Product ExtractProduct(string[] dataItem, List<Subcategory> subcategories)
    {
        Product product = new();
        string[] nameAndModelNumber = RegexHelper.RecognizeModelnumberPattern(dataItem[2]);
        product.ModelNumber = nameAndModelNumber[0];
        product.Name = nameAndModelNumber[1];
        product.Material = ExtractMaterialType(dataItem[3]);
        product.Manufacturer = ExtractManufacturer(dataItem[3]);
        product.Design = ExtractDesign(dataItem[3]);
        product.Subcategories = ExtractSubcategories(subcategories, dataItem[3]);
        product.Dimension = ExtractDimension(dataItem[3]);

        return product;
    }

    // TODO - This should be finished if time allows for it.
    public static string ExtractDimension(string input)
    {
        string result = "";
        int startIndex = 0;
        // Dimension can be Height / Højde - 
        if (input.ToLowerInvariant().Contains("height"))
        {
            startIndex = input.ToLowerInvariant().IndexOf("height") + "height".Length;

        }
        else if (input.ToLowerInvariant().Contains("højde"))
        {
            startIndex = input.ToLowerInvariant().IndexOf("height") + "height".Length;
        }
        if (startIndex != 0)
        {
            string substring = input.ToLowerInvariant().Substring(startIndex);
            result = substring;
        }

        return result;
    }
    public static List<Subcategory> ExtractSubcategories(List<Subcategory> subcategories, string input)
    {
        List<Subcategory> subcategoriesResult = new();

        foreach (Subcategory subcategory in subcategories)
        {
            if (input.Contains(subcategory.Name))
            {
                subcategoriesResult.Add(subcategory);
            }
        }

        return subcategoriesResult;
    }

    public static string ExtractManufacturer(string input)
    {
        string result = "";
        string[] manufacturers = { "Royal Copenhagen", "Bing og Grøndahl", "Axel Brüel", "Dahl Jensen", "Kähler", "Holmegaard",
                           "Georg Jensen", "Orrefors", "Hjorth", "Knabstrup", "Lyngby Porcelæn", "Aluminia", "Nymølle Keramik",
                           "Saxbo Keramik", "Dansk Glasværk", "Palshus Keramik", "Arne Bang Keramik", "Søholm Keramik",
                           "Ipsen & Co. Keramik", "A. Michelsen", "Cohr", "Anton Michelsen", "Evald Nielsen", "Hans Hansen",
                           "Kay Bojesen", "Carl Hansen & Søn" };
        foreach (string manufacturer in manufacturers)
        {
            if (input.Contains(manufacturer))
            {
                result = manufacturer;
            }
        }

        return result;
    }
    public static string ExtractDesign(string input)
    {
        string result = "";
        if (input.Contains("Design"))
        {
            int startIndex = input.IndexOf("Design");
            string subString = input.Substring(startIndex + "Design".Length, 40);

            // Trim various characters;
            subString = TrimString(subString, "&nsbp");
            subString = TrimString(subString, "\"");
            subString = TrimString(subString, ";");
            subString = TrimString(subString, ":");
            subString = TrimString(subString, "\"");
            subString = subString.Trim();

            if (subString.Contains("<"))
            {
                int charIndex = subString.IndexOf("<");
                subString = subString.Substring(0, charIndex);
            }

            result = subString;
        }

        return result;
    }
    public static string TrimString(string input, string removeValue)
    {
        string result = input;
        if (input.Contains(removeValue))
        {
            int removeValueIndex = input.IndexOf(removeValue);
            result = input.Substring(removeValueIndex + removeValue.Length);

        }
        return result;

    }
    public static MaterialType ExtractMaterialType(string input)
    {
        string substring = "";
        if (input.Contains("Materiale") || input.Contains("Material"))
        {
            int startIndex = input.IndexOf("Materiale");
            if (startIndex < 0)
            {
                startIndex = input.IndexOf("Material");
            }
            substring = input.Substring(startIndex);
        }

        if (substring.ToLowerInvariant().Contains("porcelæn") || substring.ToLowerInvariant().Contains("porcelain"))
        {
            return MaterialType.porcelain;
        }

        if (substring.ToLowerInvariant().Contains("stål") || substring.ToLowerInvariant().Contains("steel"))
        {
            return MaterialType.steel;
        }
        if (substring.ToLowerInvariant().Contains("glas") || substring.ToLowerInvariant().Contains("glass"))
        {
            return MaterialType.glass;
        }
        if (substring.ToLowerInvariant().Contains("guld") || substring.ToLowerInvariant().Contains("gold"))
        {
            return MaterialType.gold;
        }
        if (substring.ToLowerInvariant().Contains("sølv") || substring.ToLowerInvariant().Contains("silver"))
        {
            return MaterialType.silver;
        }
        if (substring.ToLowerInvariant().Contains("keramik") || substring.ToLowerInvariant().Contains("ceramics"))
        {
            return MaterialType.ceramics;
        }
        if (substring.ToLowerInvariant().Contains("stentøj") || substring.ToLowerInvariant().Contains("stoneware"))
        {
            return MaterialType.stoneware;
        }
        if (substring.ToLowerInvariant().Contains("fajance"))
        {
            return MaterialType.fajance;
        }


        return MaterialType.undefined;
    }
    public static List<string[]> ReadCsv(string filename)
    {
        List<string[]> data = new();
        using (var reader = new StreamReader(filename))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
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
            }
        }
        return data;
    }
}

