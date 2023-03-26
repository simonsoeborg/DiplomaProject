
namespace ClassLibrary.Models.DTO
{

    public class ProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string ModelNumber { get; set; } = null!;
        public string Manufacturer { get; set; } = null!;
        public MaterialType Material { get; set; }
        public string? Design { get; set; }
        public string? Dimension { get; set; }
        public List<int> SubcategoryIds { get; set; } = new List<int>();

        public override string ToString()
        {
            string subcatString;
            if (SubcategoryIds.Count > 0)
            {
                subcatString = "";
                foreach (int subcatId in SubcategoryIds)
                {
                    subcatString += subcatId + ", ";
                }
            }
            else
            {
                subcatString = "No subcategories";
            }

            return
                "Id: " + Id +
                "\nName: " + Name +
                "\nModelNumber: " + ModelNumber +
                "\nMaterial: " + Material +
                "\nDesign: " + Design +
                "\nDimension: " + Dimension +
                "\nSubcategoryIds: " + subcatString;
        }
    }
}

