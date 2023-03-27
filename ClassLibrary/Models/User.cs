namespace ClassLibrary.Models
{
    public class User
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public byte[] PasswordHash { get; set; } = null!;
        public byte[] PasswordSalt { get; set; } = null!;
        public string CreatedAt { get; set; } = null!;
        public int? Age { get; set; }
    }
}
