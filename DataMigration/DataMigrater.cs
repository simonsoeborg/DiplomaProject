using ClassLibrary.Models;
using DataMigration;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

class DataMigrater
{
    public RegexHelper RegexHelper;
    private readonly GroenlundDbContext _context;

    public DataMigrater(GroenlundDbContext context)
    {
        RegexHelper = new();
        _context = context;
    }
    public void CreateDataInDatabase()
    {
        MsSqlSetIdentityInsert("ON");
        CreateCategoriesInDatabase();
        CreateSubcategoriesInDatabase();
        CreateProductsInDatabase();
        MsSqlSetIdentityInsert("OFF");

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
        var getData = ExtractProducts();
        List<Product> Products = getData.Products;
        List<ProductItem> ProductItems = getData.ProductItems;
        var jsonString = JsonConvert.SerializeObject(Products, Formatting.Indented);
        var fileName = "products.json";
        File.WriteAllText(fileName, jsonString);
        Console.WriteLine("JSON array saved to " + fileName);

        jsonString = JsonConvert.SerializeObject(ProductItems, Formatting.Indented);
        fileName = "productitems.json";
        File.WriteAllText(fileName, jsonString);
        Console.WriteLine("JSON array saved to " + fileName);

    }



    private void MsSqlSetIdentityInsert(string onOrOff)
    {
        List<string> queries = new()
        {
            $"SET IDENTITY_INSERT GroenlundDB.dbo.Categories {onOrOff};",
            $"SET IDENTITY_INSERT GroenlundDB.dbo.Subcategories {onOrOff};",
            $"SET IDENTITY_INSERT GroenlundDB.dbo.Products {onOrOff};",
            $"SET IDENTITY_INSERT GroenlundDB.dbo.ProductItems {onOrOff};"
        };

        foreach (string query in queries)
        {
            try
            {
                _context.Database.ExecuteSqlRaw(query);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

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

        var categoriesJson = DemoDataRepository.GetCategories();
        foreach (var category in categoriesJson)
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

        var subcategoriesJson = DemoDataRepository.GetSubcategories();
        foreach (var subcategory in subcategoriesJson)
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

    private void CreateProductsInDatabase()
    {
        var existingProducts = _context.Products;
        var existingProductItems = _context.ProductItems;
        if (existingProducts.Any())
        {
            try
            {
                _context.Products.ExecuteDelete();
                Console.WriteLine("Deleted all products in database\n");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        if (existingProductItems.Any())
        {
            try
            {
                _context.ProductItems.ExecuteDelete();
                Console.WriteLine("Deleted all productItems in database\n");

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        var getData = ExtractProducts();
        List<Product> Products = getData.Products;
        List<ProductItem> ProductItems = getData.ProductItems;

        foreach (Product prod in Products)
        {
            _context.Products.Add(prod);
            try
            {
                _context.SaveChanges();
                Console.WriteLine(prod.Name + " added");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed creating product" + ex.Message);
            }
        }


        _context.SaveChanges();

        foreach (ProductItem productItem in ProductItems)
        {
            _context.ProductItems.Add(productItem);
            try
            {
                _context.SaveChanges();
                Console.WriteLine("ProductItem for " + productItem.Product.Name + " added");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed creating productItem" + ex.Message + "\n");
                Console.WriteLine("StackTrace:" + ex.StackTrace + "\n");
                Console.WriteLine("InnerException:" + ex.InnerException + "\n");
            }
        }
    }

    private (List<Product> Products, List<ProductItem> ProductItems) ExtractProducts()
    {
        List<string[]> data = ReadCsv("./products.csv");

        List<Product> Products = new();
        List<ProductItem> ProductItems = new();
        List<Subcategory> subcategories = _context.Subcategories.ToList();

        for (int i = 1; i < data.Count; i++)
        {
            var dataItem = data[i];
            Product product = ExtractProduct(dataItem, subcategories);
            if (ProductIsValid(product))
            {
                Products.Add(product);
                List<ProductItem> generatedProductItems = GenerateProductItems(product);
                ProductItems.AddRange(generatedProductItems);
            }
        }

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
            }
            if (product.ModelNumber != "" &&
                product.Material != ((int)MaterialType.undefined) &&
                product.Design != null &&
                product.Manufacturer != "" &&
                product.Subcategories.Count > 0
                )
            {
                productCount++;
            }
        }

        Console.WriteLine("Materials mapped: {0}/{1}\n" +
            "Modelnumbers mapped: {2}/{1}\n" +
            "Designs mapped: {3}/{1}\n" +
            "Manufacturers mapped: {4}/{1}\n" +
            "Subcategories mapped: {5}/{1}\n",
            materialCount, data.Count, modelNumberCount, designCount, manufacturerCount, subcategoryCount
        );

        Console.WriteLine("Added data to {0}/{1} products generated from {2} dataItems", productCount, Products.Count, data.Count);
        return (Products, ProductItems);
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

    private bool ProductIsValid(Product product)
    {
        bool result = true;
        if (
            product.ModelNumber == "" ||
            product.Material == ((int)MaterialType.undefined) ||
            product.Design == null ||
            product.Manufacturer == "" ||
            product.Subcategories.Count == 0
        )
        {
            result = false;
        }

        return result;
    }
    private List<ProductItem> GenerateProductItems(Product product)
    {
        List<ProductItem> productItems = new();
        int amount = new Random().Next(2);
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

    private DateTime RandomDay()
    {
        DateTime start = new DateTime(2019, 1, 1);
        int range = (DateTime.Today - start).Days;
        return start.AddDays(new Random().Next(range));
    }

    private Product ExtractProduct(string[] dataItem, List<Subcategory> subcategories)
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
    private string ExtractDimension(string input)
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
    private List<Subcategory> ExtractSubcategories(List<Subcategory> subcategories, string input)
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

    private string ExtractManufacturer(string input)
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
    private string ExtractDesign(string input)
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
    private string TrimString(string input, string removeValue)
    {
        string result = input;
        if (input.Contains(removeValue))
        {
            int removeValueIndex = input.IndexOf(removeValue);
            result = input.Substring(removeValueIndex + removeValue.Length);

        }
        return result;

    }
    private MaterialType ExtractMaterialType(string input)
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