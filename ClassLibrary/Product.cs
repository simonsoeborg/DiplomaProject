
namespace ClassLibrary
{
    public class Product
    {
        public Product()
        {
            Subcategories = new HashSet<Subcategory>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string ModelNumber { get; set; }
        public MaterialType Material { get; set; }
        public string Manufacturer { get; set; }
        public string? Design { get; set; }
        public string? Dimension { get; set; }

        public virtual ICollection<Subcategory> Subcategories { get; set; }

        public override string ToString()
        {

            return "Id: " + Id +
                " Name: " + Name +
                " ModelNumber: " + ModelNumber +
                " Material: " + Material +
                " Design: " + Design +
                " Dimension: " + Dimension +
                " SubcategoryIds: [" + string.Join(", ", Subcategories.Select(sub => sub.Id).ToArray()) + "]";
        }
    }

    public enum MaterialType
    {
        undefined = 0,
        porcelain = 1,
        steel = 2,
        glass = 3,
        gold = 4,
        silver = 5,
        ceramics = 6,
        stoneware = 7,
        fajance = 8,
        // etc..
    }
}
