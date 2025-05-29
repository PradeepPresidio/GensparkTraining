namespace BankApp.Models
{
    public class TransactionLog
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public string TransactionDatetime { get; set; }
        public double TransactionAmount { get; set; }

        public User Sender { get; set; }
        public User Receiver { get; set; }
    }
}