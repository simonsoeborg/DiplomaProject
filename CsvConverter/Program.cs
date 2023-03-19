
using System.Text.RegularExpressions;

class Program
{

    static void Main(string[] args)
    {
        List<string[]> data = new();

        // Open the CSV file and read the data
        using (StreamReader reader = new("./products.csv"))
        {
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                string[] values = line.Split(',');
                data.Add(values);
            }
        }
        List<Product> Products = new();
        List<string> modelNumbers = new();

        for (int i = 1; i < data.Count; i++)
        {
            var dataItem = data[i];
            Product product = new Product();


            product.Name = TryParseName(dataItem[2]);
            if (product.Name == null)
            {
                break;
            }
            modelNumbers.Add(product.Name);
            product.ModelNumber = TryParseModelNumber(product.Name);



            Products.Add(product);
        }
        RecognizeModelnumberPattern(modelNumbers.ToArray());

        Console.WriteLine("Amount of items before cleansing: {0}", data.Count);
        Console.WriteLine("Amount of items after cleansing: {0}", Products.Count);
    }

    class Product
    {
        public string? Name { get; set; }
        public string ModelNumber { get; set; }

    }

    public static void RecognizeModelnumberPattern(string[] modelNumbers)
    {
        /* Expected types of modelnumber formatting before splitting:
             * Model.name (no modelnumber in name)
             * Nr: 12
             * Nr: 123 - Model.name
             * Nr: 1234 - Model.name
             * Nr: 12345 - Model.name
             * Nr: 1/1234 - Model.name
             * 12 Model.name
             * 123 Model.name
             * 1234 Model.name
        */

        /* RegEx for each type of format */
        Regex type8 = new("\\d+ [\\w.]+"); // "12 Model.name":
        Regex type7 = new("\\d+ [\\w.]+"); // "123 Model.name"
        Regex type6 = new("\\d+ [\\w.]+"); // "1234 Model.name"
        Regex type5 = new("Nr: \\d{2}"); // "Nr: 12"
        Regex type4 = new("Nr: \\d{3} - [\\w.]+"); // "Nr: 123 - Model.name"
        Regex type3 = new("Nr: \\d{4} - [\\w.]+"); // "Nr: 1234 - Model.name"
        Regex type2 = new("Nr: \\d{5} - [\\w.]+"); // "Nr: 12345 - Model.name"
        Regex type1 = new("Nr: \\d+\\/\\d{4} - [\\w.]+"); // "Nr: 1/1234 - Model.name"

        Dictionary<int, Regex> regexMap = new()
        {
            { 1, type1 },{ 2, type2 },{ 3, type3 },{ 4, type4 },
            { 5, type5 },{ 6, type6 },{ 7, type7 },{ 8, type8 }
        };

        Dictionary<int, int> typeCount = new();
        for (int i = 1; i <= 8; i++)
        {
            typeCount.Add(i, 0);
        }

        foreach (string modelNumber in modelNumbers)
        {
            foreach (var regex in regexMap)
            {
                if (regex.Value.IsMatch(modelNumber))
                {
                    Console.WriteLine("modelNumber: {0}", modelNumber);
                    Console.WriteLine("Match on type {0}\n", regex.Key);
                    typeCount[regex.Key]++;
                    break;
                }
            }
        }

        int totalCount = typeCount.GetValueOrDefault(1) +
            typeCount.GetValueOrDefault(2) +
            typeCount.GetValueOrDefault(3) +
            typeCount.GetValueOrDefault(4) +
            typeCount.GetValueOrDefault(5) +
            typeCount.GetValueOrDefault(6) +
            typeCount.GetValueOrDefault(7) +
            typeCount.GetValueOrDefault(8);

        Console.WriteLine(
            "Type1 amount: {0}\n" +
            "Type2 amount: {1}\n" +
            "Type3 amount: {2}\n" +
            "Type4 amount: {3}\n" +
            "Type5 amount: {4}\n" +
            "Type6 amount: {5}\n" +
            "Type7 amount: {6}\n" +
            "Type8 amount: {7}\n" +
            "Total matches: {8}\n",
            typeCount.GetValueOrDefault(1),
            typeCount.GetValueOrDefault(2),
            typeCount.GetValueOrDefault(3),
            typeCount.GetValueOrDefault(4),
            typeCount.GetValueOrDefault(5),
            typeCount.GetValueOrDefault(6),
            typeCount.GetValueOrDefault(7),
            typeCount.GetValueOrDefault(8),
            totalCount
            );
    }

    public static string? TryParseName(string name)
    {
        if (name != null && name.Length! > 0 && name.Length! < 155)
        {
            return name;
        }
        else
        {
            return null;
        }
    }

    public static string TryParseModelNumber(string name)
    {
        string modelNumber = "";
        try
        {



            modelNumber = name.Split('-')[0];
            if (modelNumber.Contains('/'))
            {
                // modelnumber is probably of this format "Nr: 1234 / 1234"
            }
            if (modelNumber.Contains(':'))
            {

            }
            return modelNumber;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Invalid model number: {modelNumber}, Exception thrown: {e}");
        }
        return modelNumber;
    }
}

