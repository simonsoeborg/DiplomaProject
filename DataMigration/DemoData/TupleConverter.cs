using Newtonsoft.Json;

namespace DataMigration
{
    class TupleConverter
    {
        static void Main()
        {
            var subcategoryTupleList = TupleList.subcategoryTupleList;
            var jsonArray = new List<object>();

            foreach (var tuple in subcategoryTupleList)
            {
                var jsonObject = new
                {
                    Id = tuple.Item1,
                    CategoryId = tuple.Item2,
                    Name = tuple.Item3,
                    Order = tuple.Item4,
                    ImageUrl = tuple.Item5,
                    Description = "Beskrivelse af " + tuple.Item3
                };

                jsonArray.Add(jsonObject);
            }

            var jsonString = JsonConvert.SerializeObject(jsonArray, Formatting.Indented);

            var fileName = "subcategories.json";

            File.WriteAllText(fileName, jsonString);

            Console.WriteLine("JSON array saved to " + fileName);
        }
    }
}