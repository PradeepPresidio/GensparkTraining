using BankApp.Models;
using BankApp.Interfaces;
using BankApp.Repositories;
using BankApp.Models.DTOs;
using Microsoft.VisualBasic;
using System.Threading.Tasks;

namespace BankApp.Services
{
public class TransactionLogService : ITransactionLogService {
     private readonly IRepository<int, User> _userRepository;
    private readonly IRepository<int, TransactionLog> _transactionLogRepository;

    public TransactionLogService(IRepository<int,User> userRepository,
                                IRepository<int,TransactionLog> transactionLogRepository)
    {
        _userRepository = userRepository;
        _transactionLogRepository = transactionLogRepository;
    }
        public async Task<TransactionLog> MakeTransaction(string senderId, string receiverId, double amount)
        {
            try
            {
                var allUsers = _userRepository.GetAll();
                User sender = allUsers.Where(u => u.Id == senderId);
                User receiver = allUsers.Where(u => u.Id == receiverId);
                if (sender.Amount < amount)
                {
                    throw new Exception("Sender insufficient balance");
                }
                sender.Amount -= amount;
                receiver.Amount += amount;
                await _transactionLogRepository.Update(sender.id, sender);
                await _transactionLogRepository.Update(receiver.id, receiver);
                TransactionLog tlog = new TransactionLog()
                {
                    SenderId = sender.Id,
                    receiverId = receiver.Id,
                    TransactionAmount = amount,
                    TransactionDatetime = datetime.Now.toString()
                }
            }
        }
        public TransactionLog convertDTOtoTransactionLog(TransactionLogDTO transDto)
        {
            return new TransactionLog()
            {
                SenderId = transDto.SenderId,
                ReceiverId = transDto.ReceiverId,
                TransactionDatetime = transDto.TransactionDatetime,
                TransactionAmount = transDto.TransactionAmount
            }
        }
        public async Task<ICollection<TransactionLogDTO>> GetTransactionsBetweenbyUsersAccId(string senderAccId, string receiverAccId)
        {

        }
    }