using System.Threading.Tasks;
using BankApp.Interfaces;
using BankApp.Models;
using BankApp.Models.DTOs;
using Microsoft.VisualBasic;

namespace BankApp.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<int, User> _userRepository;
        private readonly IRepository<int, TransactionLog> _transactionLogRepository;
        public UserService(IRepository<int, User> userRepository,
                           IRepository<int, TransactionLog> transactionLogRepository)
        {
            _userRepository = userRepository;
            _transactionLogRepository = transactionLogRepository;
        }
        public async Task<User> AddUser(UserDTO user)
        {
            User newUser = convertDTOtoUser(user);
            newUser = await _userRepository.Add(newUser);
            return newUser;
        }
        public User convertDTOtoUser(UserDTO userDto)
        {
            return new User()
            {
                Name = userDto.Name,
                AccountId = userDto.AccountId,
                Amount = userDto.Amount
            };
        }
        public async Task<UserDTO> GetUserByAccNo(string accno)
        {
            var Users = await _userRepository.GetAll();
            User MatchedUser = Users.FirstOrDefault(u => u.AccountId == accno);
            UserDTO userDto = convertUsertoDTO(MatchedUser);
            return userDto;
        }
        public UserDTO convertUsertoDTO(User userObj)
        {
            return new UserDTO()
            {
                Name = userObj.Name,
                AccountId = userObj.AccountId,
                Amount = userObj.Amount
            };
        }
        //public async Task<ICollection<TransactionLogDTO>> GetUserTransactionsById(int id)
        //{
        //    var logs = await _transactionLogRepository.GetAll();
        //    logs = logs.Where(t => t.SenderId == id || t.ReceiverId == id);
        //    List<TransactionLogDTO> transactionLogs = new();
        //    foreach (var log in logs)
        //    {
        //        transactionLogs.Add(convertTransactionLogtoDTO(log));
        //    }
        //    return transactionLogs;
        //}

        public async Task<ICollection<TransactionLogDTO>> GetUserTransactionsById(int id)
        {
            var transactionLogs = await _context.TransactionLogs
                .FromSqlInterpolated($"EXEC proc_GetUserTransactionsById({id})")
                .Select(t => new TransactionLogDTO
                {
                    SenderId = t.SenderId,
                    ReceiverId = t.ReceiverId,
                    TransactionAmount = t.TransactionAmount,
                    TransactionDatetime = t.TransactionDatetime
                })
                .ToListAsync();

            return transactionLogs;
        }

        public TransactionLogDTO convertTransactionLogtoDTO(TransactionLog trans)
        {
            return new TransactionLogDTO()
            {
                SenderId = trans.SenderId,
                ReceiverId = trans.ReceiverId,
                TransactionAmount = trans.TransactionAmount,
                TransactionDatetime = trans.TransactionDatetime
            };
        }
    }
}