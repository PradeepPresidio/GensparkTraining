namespace BankApp.Models
{
	public class User
	{
		public int Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public string AccountId { get; set; } = string.Empty;
		public double Amount { get; set; }

		public ICollection<TransactionLog> SentTransactions { get; set; }
		public ICollection<TransactionLog> ReceivedTransactions { get; set; }
	}
}