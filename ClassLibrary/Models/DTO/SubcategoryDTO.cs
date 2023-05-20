
namespace ClassLibrary.Models.DTO
{
    public class SubcategoryDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int Order { get; set; }
        public string? ImageUrl { get; set; }
        public string? Description { get; set; }
        public int CategoryId { get; set; }
    }
}
