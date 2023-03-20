using ClassLibrary;
using CsvConverter;
using System.Data.SqlTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data.Entity;
using System.Reflection.Metadata;

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
        //List<ProductItem> ProductItems = new();
        List<string> names = new();
        List<Subcategory> subcategories = context.Subcategories.ToList();

        for (int i = 1; i < data.Count; i++)
        {
            var dataItem = data[i];
            names.Add(dataItem[2]);
            Products.Add(ExtractProduct(dataItem, subcategories));
            // TODO: Create ExtractProductItem(dataItem) method

            // TODO: Insert all products and productItems into the database
        }
        //RegexHelper.TestRegexFilter(names.ToArray(), RegexHelper.RegexMap());
        int productCounter = 0;
        int materialCounter = 0;
        int modelNumbercounter = 0;
        int designCounter = 0;
        int manufacturerCount = 0;

        foreach (var product in Products)
        {
            if (product.Material != MaterialType.undefined)
            {
                materialCounter++;
            }
            if (product.ModelNumber != "")
            {
                modelNumbercounter++;
            }
            if (product.Design != "")
            {
                designCounter++;
            }
            if (product.Manufacturer != "")
            {
                manufacturerCount++;
            }
            if (product.ModelNumber != "" && product.Material != MaterialType.undefined && product.Design != null && product.Manufacturer != "")
            {
                productCounter++;
                //Console.WriteLine(product.ToString());
            }
        }

        Console.WriteLine("Materials mapped: {0}/{1}\n" +
            "Modelnumbers mapped: {2}/{1}\n" +
            "Designs mapped: {3}/{1}\n" +
            "Manufacturers mapped: {4}/{1}",
            materialCounter, data.Count, modelNumbercounter, designCounter, manufacturerCount);


        Console.WriteLine("Added data to {0}/{1} products generated from {2} dataItems", productCounter, Products.Count, data.Count);
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

        // TODO: Dimension

        product.SubcategoryId = FindSubcategoryId(subcategories, dataItem[3], product.Name);
        // TODO: Subcategory
        return product;
    }
    public static int FindSubcategoryId(List<Subcategory> subcategories, string input, string name)
    {
        int result = 0;
        List<string> subcategoryMatches = new();
        Console.WriteLine("Finding subcategories for {0}\n", name);

        foreach (Subcategory subcategory in subcategories)
        {
            if (input.Contains(subcategory.Name))
            {
                subcategoryMatches.Add(subcategory.Name);
                //Console.WriteLine("Subcategory {0} matched  with input {1}\n", subcategory.Name, input);
            }
        }
        foreach (string subcat in subcategoryMatches)
        {
            Console.WriteLine(subcat);
        }
        Console.WriteLine("\n\n\n");

        return result;
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

