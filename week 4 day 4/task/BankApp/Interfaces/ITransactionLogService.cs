using System;
using BankApp.Models;
using BankApp.Models.DTOs;
namespace BankApp.Interfaces
{
    public interface ITransactionLogService
    {
        public Task<TransactionLog> MakeTransaction(string senderId,string receiverId,double amount);
        public Task<ICollection<TransactionLogDTO>> GetTransactionsBetweenbyUsersAccId(string senderAccId, string receiverAccId);
    }
}