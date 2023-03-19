using System.Text.RegularExpressions;
using ClassLibrary;

class Program
{

    static void Main(string[] args)
    {
        List<string[]> data = new();

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
        List<string> names = new();

        for (int i = 1; i < data.Count; i++)
        {
            var dataItem = data[i];
            Product product = new()
            {
                Name = TryParseName(dataItem[2])
            };
            if (product.Name == null)
            {
                break;
            }
            names.Add(product.Name);
            product.ModelNumber = TryParseModelNumber(product.Name);



            Products.Add(product);
        }
        TestRegexFilter(names.ToArray(), RegexMap());
    }
    class Something
    {
        public string Name { get; set; }
        public string ModelNumber { get; set; }
    }
    public static void RecognizeModelnumberPattern(string[] names)
    {

        List<Something> Somethings = new();

        Dictionary<int, Regex> regexMap = RegexMap();
        TestRegexFilter(names, regexMap);

    }

    public static void TestRegexFilter(string[] inputs, Dictionary<int, Regex> regexMap)
    {
        int matchedInputcount = 0;
        List<string> failedMatches = new();
        Dictionary<int, List<string>> matches = new();

        Dictionary<int, int> typeCount = new();
        for (int i = 1; i <= regexMap.Count; i++)
        {
            typeCount.Add(i, 0);
            matches.Add(i, new List<string>());

        }

        foreach (string input in inputs)
        {
            bool match = false;
            for (int i = 1; i <= regexMap.Count; i++)
            {

                if (regexMap[i].IsMatch(input))
                {
                    matches[i].Add(input);
                    typeCount[i]++;
                    matchedInputcount++;
                    match = true;
                    break;
                }
            }
            if (!match) failedMatches.Add(input);
        }

        foreach (var match in matches)
        {
            Console.WriteLine("Type {0}:", match.Key);
            foreach (var input in match.Value)
            {
                Console.WriteLine(input);
            }
            Console.WriteLine("\n\n\n");
        }

        Console.WriteLine("Failed matches: ");
        foreach (string input in failedMatches)
        {
            Console.WriteLine(input);
        }

        Console.WriteLine("\nMatched {0}/{1} strings", matchedInputcount, inputs.Length);
        Console.WriteLine("Failed to match {0}\n", failedMatches.Count);

        for (int i = 1; i <= regexMap.Count; i++)
        {
            int countForTypeI = typeCount.GetValueOrDefault(i); ;
            Console.WriteLine("Type{0} amount: {1}", i, countForTypeI);
        }

    }

    public static Dictionary<int, Regex> RegexMap()
    {
        /* RegEx for each type of format */
        Regex type1 = new("Nr:\\s*\\d{1,10}\\s*/\\s*\\d{1,10}\\s*-?"); // Nr: 1234567890/1234567890 - Modelname
        Regex type2 = new("Nr:\\s*\\d{1,10}\\s*-"); // "Nr: 12345567890 - Modelname
        Regex type3 = new("Nr:\\s*\\d{1,10}"); // Nr: 1234567890 Modelnames
        Regex type4 = new("Nr:\\s*\\d{1,10}\\w+"); // Nr: 1234567890a Modelname
        Regex type5 = new("\\A\\d{1,10}[^-][^stk]\\s*[\\w]+"); // 1234567890 Modelname UDEN 'stk'
        Regex type6 = new("Årgang"); // Årgang
        Regex type7 = new("År\\s*[:]?\\s*\\d{1,10}"); // År 1958 eller År: 1958


        return new() {
            { 1, type1 }, { 2, type2 }, { 3, type3 }, { 4, type4 },
            { 5, type5 }, {6, type6}, { 7, type7 }
        };
    }


    public static string StringFormatter(string input, int type)
    {
        return type switch
        {
            1 => "", // "Nr: 1/1234 - Modelname"
            2 => "", // "Nr: 12345 - Modelname"
            3 => "", // "Nr: 1234 - Modelname"
            4 => "", // "Nr: 123 - Modelname"
            5 => "", // "Nr: 12"
            6 => "", // "1234 Modelname"
            7 => "", // "123 Modelname"
            8 => "", // "12 Modelname":
            9 => "", // "1 Modelname":
            _ => "",
        };
    }

    public static string TryParseName(string name)
    {
        if (name != null && name.Length! > 0 && name.Length! < 155)
        {
            return name;
        }
        else
        {
            return "";
        }
    }

    public static string TryParseModelNumber(string name)
    {
        string modelNumber = "";
        try
        {
            modelNumber = name.Split('-')[0];

            return modelNumber;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Invalid model number: {modelNumber}, Exception thrown: {e}");
        }
        return modelNumber;
    }
}

