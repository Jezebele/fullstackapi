namespace FullStackAPI.Models
{
    public class Contact
    {
        public long Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public long Phone { get; set; }
        public int UserId { get; set; }
    }
}
