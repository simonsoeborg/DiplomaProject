
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
            return "Id: " + Id +
                " Name: " + Name +
                " ModelNumber: " + ModelNumber +
                " Material: " + Material +
                " Design: " + Design +
                " Dimension: " + Dimension;
            //" SubcategoryId: " + SubcategoryId;
        }
    }
}
