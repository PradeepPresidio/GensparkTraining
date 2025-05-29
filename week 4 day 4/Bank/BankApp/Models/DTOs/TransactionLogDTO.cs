namespace BankApp.Models.DTOs
{
    public class TransactionLogDTO
    {
        public int SenderId {  get; set; }
        public int ReceiverId { get; set; }
        public string TransactionDatetime { get; set; }
        public string TransactionAmount { get; set; }
    }
}