using ClassLibrary;
using CsvConverter;

class Program
{
    public RegexHelper RegexHelper = new RegexHelper();

    static void Main(string[] args)
    {
        List<string[]> data = new();

        using (var reader = new StreamReader("./products.csv"))
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

        foreach (var dat in data)
        {
            if (dat.Length > 53 || dat.Length < 53)
            {
                Console.WriteLine("Outlier");
                foreach (string item in dat)
                {
                    //Console.WriteLine(item);
                }
            }
        }

        List<Product> Products = new();
        //List<ProductItem> ProductItems = new();
        List<string> names = new();
        List<int> lengths = new();

        for (int i = 1; i < data.Count; i++)
        {
            var dataItem = data[i];
            names.Add(dataItem[2]);
            Products.Add(ExtractProduct(dataItem));
            // TODO: Create ExtractProductItem(dataItem) method

            // TODO: Insert all products and productItems into the database
        }
        //RegexHelper.TestRegexFilter(names.ToArray(), RegexHelper.RegexMap());

        int counter = 0;
        foreach (var product in Products)
        {
            if (product.ModelNumber != "")
            {
                counter++;
                //Console.WriteLine("Name: {0}\nModelNumber: {1}\n", product.Name, product.ModelNumber);
            }
        }

        Console.WriteLine("Added data to {0}/{1} products generated from {2} dataItems", counter, Products.Count, data.Count);
    }

    public static Product ExtractProduct(string[] dataItem)
    {
        Product product = new();
        string[] nameAndModelNumber = RegexHelper.RecognizeModelnumberPattern(dataItem[2]);
        product.ModelNumber = nameAndModelNumber[0];
        product.Name = nameAndModelNumber[1];
        //Console.WriteLine("dataItem[11]: {0}", dataItem[11]);
        // TODO: Find a way to extract the materialType, Design, Dimension and Subcategory from the rest of the data

        // TODO: MaterialType

        // TODO: Design

        // TODO: Dimension

        // TODO: Subcategory
        return product;
    }
}

