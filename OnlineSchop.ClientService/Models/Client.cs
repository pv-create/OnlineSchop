namespace OnlineSchop.ClientService.Models
{
    public class Client
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public decimal CreditLimit { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}