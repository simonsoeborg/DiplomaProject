
namespace ClassLibrary.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string ModelNumber { get; set; } = null!;
        public string Manufacturer { get; set; } = null!;
        public MaterialType Material { get; set; }
        public string? Design { get; set; }
        public string? Dimension { get; set; }
        public virtual ICollection<Subcategory> Subcategories { get; set; } = new List<Subcategory>();

        public override string ToString()
        {
            string subcatString;
            if (Subcategories.Count > 0)
            {
                subcatString = "";
                foreach (Subcategory subcat in Subcategories)
                {
                    subcatString += subcat.Name + ", ";
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
                "\nSubcategories: " + subcatString;
        }
    }
}
