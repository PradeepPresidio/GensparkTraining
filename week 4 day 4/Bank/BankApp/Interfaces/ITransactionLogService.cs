using system;
using BankApp.Models;
using BankApp.Models.DTOs;
namespace BankApp.Interfaces
{
    public interface ITransactionLogService
    {
        public Task<TransactionLog> AddTransaction(TransactionLogDTO trans);
        public Task<ICollection<TransactionLogDTO>> GetTransactionsBetweenbyAccId(string senderAccId, string receiverAccId);
    }
}