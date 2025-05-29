namespace BankApp.Models.DTOs
{
    public class UserDTO
    {
        public string Name { get; set; } = string.Empty;
        public string AccountId { get; set; } = string.Empty;
        public double Amount { get; set; }
    }
}