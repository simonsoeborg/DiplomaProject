﻿using ClassLibrary.Models;
using DataMigration;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

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
        List<string[]> data = ReadCsv("./products.csv");

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
            Product product = new()
            {
                Id = productIdCounter,
                Material = ExtractMaterialType(dataItem[3]),
                Manufacturer = ExtractManufacturer(dataItem[3]),
                Design = ExtractDesign(dataItem[3]),
                Subcategories = ExtractSubcategories(categories, subcategories, dataItem[3]),
                Dimension = ExtractDimension(dataItem[3]),

            };
            string[] nameAndModelNumber = RegexHelper.RecognizeModelnumberPattern(dataItem[2]);
            product.ModelNumber = nameAndModelNumber[0];
            product.Name = nameAndModelNumber[1];

            var imageUrls = ExtractImages(dataItem[4]);

            List<ProductItem> productItems = new();

            // Create productItems for the product
            int amount = new Random().Next(2);
            int poImageIdCounter = imageIdCounter;
            for (int x = 0; x <= amount; x++)
            {
                int conditionType = new Random().Next(3);
                int qualityType = new Random().Next(3);
                int purchasePrice = new Random().Next(10500);
                int currentPrice = purchasePrice * 2;
                ProductItem productItem = new()
                {
                    Id = productItemIdCounter + x,
                    Product = product,
                    ProductId = product.Id,
                    Condition = (ConditionType)conditionType,
                    Quality = (QualityType)qualityType,
                    Sold = 0,
                    Weight = InferWeight(product.Material),
                    PurchasePrice = purchasePrice,
                    CurrentPrice = currentPrice,
                    CreatedDate = RandomDay(),
                    CustomText = "",
                    Images = new List<Image>(),
                };

                if (imageUrls != null && imageUrls.Count > 0)
                {
                    foreach (var img in imageUrls)
                    {
                        if (string.IsNullOrEmpty(img)) continue;
                        var imgId = img.Split('.')[0];
                        string imgReducedSize = "https://static.wixstatic.com/media/" + img + "/v1/fill/w_630,h_840,al_c,q_85,usm_0.66_1.00_0.01/" + imgId + ".webp";
                        //Console.WriteLine(imgId);
                        //Console.WriteLine(imgReducedSize);

                        Image image = new()
                        {
                            ProductItemId = productItem.Id,
                            Id = poImageIdCounter,
                            Url = imgReducedSize,
                        };
                        productItem.Images.Add(image);
                        poImageIdCounter++;
                    }
                }

                productItems.Add(productItem);
            }

            if (ProductIsValid(product, productItems))
            {
                Products.Add(product);
                ProductItems.AddRange(productItems);
                productIdCounter++;
                productItemIdCounter += productItems.Count;
                foreach (var productItem in productItems)
                {
                    imageIdCounter += productItem.Images.Count;
                    Images.AddRange(productItem.Images);
                    productItem.Images = new List<Image>();
                }
            }
        }

        Console.WriteLine("Added data to {0} products generated from {1} dataItems", Products.Count, data.Count);
        return (Products, ProductItems, Images);
    }
    public static void TestRegExFilter()
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

    private static decimal InferWeight(MaterialType material)
    {
        decimal result = 0;
        if (material == MaterialType.gold || material == MaterialType.silver)
        {
            result = (decimal)new Random().NextDouble() * 50;
        }
        return result;
    }

    private static bool ProductIsValid(Product product, List<ProductItem> productItems)
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
        foreach (ProductItem po in productItems)
        {
            if (po.Images == null || po.Images.Count == 0)
            {
                result = false;
            }
        }

        return result;
    }

    private static DateTime RandomDay()
    {
        DateTime start = new(2019, 1, 1);
        int range = (DateTime.Today - start).Days;
        return start.AddDays(new Random().Next(range));
    }

    // TODO - This should be finished if time allows for it.
    private static string ExtractDimension(string input)
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

    private static List<string> ExtractImages(string input)
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
    private static List<Subcategory> ExtractSubcategories(List<Category> categories, List<Subcategory> subcategories, string input)
    {
        List<Subcategory> subcategoriesResult = new();
        Category? cat = InferCategory(categories, subcategories, input);
        if (cat == null)
        {
            return subcategoriesResult;
        }

        List<Subcategory> possibleSubcategories = subcategories.FindAll(x => x.Category == cat);
        foreach (Subcategory subcategory in possibleSubcategories)
        {
            if (input.ToLowerInvariant().Contains(subcategory.Name.ToLowerInvariant()))
            {
                subcategoriesResult.Add(subcategory);
            }
        }

        return subcategoriesResult;
    }

    private static Category? InferCategory(List<Category> categories, List<Subcategory> subcategories, string input)
    {
        List<Category> possibleCategories = new();
        input = input.ToLowerInvariant();
        List<CategoryStrings> categoryStrings = CategoryStringsList(categories);
        categoryStrings.Reverse();

        foreach (var catStrings in categoryStrings)
        {
            foreach (string catString in catStrings.categoryStrings)
            {
                if (input.Contains(catString))
                {
                    return catStrings.category;
                    //possibleCategories.Add(catStrings.category);
                }
            }
        }

        if (possibleCategories.Count > 0)
        {
            List<string> categoryNames = possibleCategories.Select(c => c.Name).ToList();
            string foundCategories = "Found " + categoryNames.Count;
            foreach (var category in categoryNames)
            {
                foundCategories += " " + category;
            }
            foundCategories += " categories for input\n" + input;
            Console.WriteLine(foundCategories);
        }

        // Remove any double entries
        possibleCategories = possibleCategories.Distinct().ToList();
        if (possibleCategories.Count == 1)
        {
            return possibleCategories[0];
        }
        // If more than one possible category is found, return null so the program skips adding this product
        return null;

    }



    private static string ExtractManufacturer(string input)
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
    private static string ExtractDesign(string input)
    {
        string result = "";
        int startIndex = input.IndexOf("Design");

        if (startIndex != -1)
        {
            startIndex += "Design".Length;
            string subString = input.Substring(startIndex, 40)
                .Trim('&', ' ', ';', ':', '"')
                .Split('<')[0]
                .Trim();
            result = subString;
        }

        return result;
    }

    private static MaterialType ExtractMaterialType(string input)
    {
        string substring = "";
        if (input.Contains("Materiale") || input.Contains("Material"))
        {
            int startIndex = input.IndexOf("Materiale");
            if (startIndex < 0)
            {
                startIndex = input.IndexOf("Material");
            }
            substring = input[startIndex..];
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
    private static List<CategoryStrings> CategoryStringsList(List<Category> categories)
    {
        List<CategoryStrings> catStrings = new();
        CategoryStrings one = new()
        {
            category = categories.FirstOrDefault(c => c.Id == 1)!,
            categoryStrings = new List<string>() { "mega", "musselmalet", "halvblonde", "helblonde", "riflet", "flora", "konkylie", "purpur" },
        };
        catStrings.Add(one);

        CategoryStrings two = new()
        {
            category = categories.FirstOrDefault(c => c.Id == 2)!,
            categoryStrings = new List<string>() { "tallerken", "kop", "krus", "skål", "kande", "fad", "kage", "krukker", "frokost", "middag", "dyb" },
        };
        catStrings.Add(two);

        CategoryStrings three = new()
        {
            category = categories.FirstOrDefault(c => c.Id == 3)!,
            categoryStrings = new List<string>() { "bjørn", "fugl", "hund", "kat", "hest", "menneske", "mand", "kvinde", "pige", "dreng" },
        };
        catStrings.Add(three);

        CategoryStrings four = new()
        {
            category = categories.FirstOrDefault(c => c.Id == 4)!,
            categoryStrings = new List<string>() { "aluminia", "saxbo", "arne bang", "palshus", "kähler", "keramik", "stentøj" },
        };
        catStrings.Add(four);

        CategoryStrings five = new()
        {
            category = categories.FirstOrDefault(c => c.Id == 5)!,
            categoryStrings = new List<string>() { "broche", "halskæde", "smykke", "saltkar", "guld", "sølv" },
        };
        catStrings.Add(five);

        CategoryStrings six = new()
        {
            category = categories.FirstOrDefault(c => c.Id == 6)!,
            categoryStrings = new List<string>() { "gaffel", "gafler", "kniv", "ske", "bestik", "kage" },
        };
        catStrings.Add(six);

        CategoryStrings seven = new()
        {
            category = categories.FirstOrDefault(c => c.Id == 7)!,
            categoryStrings = new List<string>() { "platte" },
        };
        catStrings.Add(seven);

        CategoryStrings eight = new()
        {
            category = categories.FirstOrDefault(c => c.Id == 8)!,
            categoryStrings = new List<string>() { "vinglas", "ølglas", "dessertglas", "vase" },
        };
        catStrings.Add(eight);

        return catStrings;
    }
    private static List<string[]> ReadCsv(string filename)
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

    class CategoryStrings
    {
        public Category category;
        public List<string> categoryStrings = new();
    }
}

