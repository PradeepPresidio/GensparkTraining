using System.Threading.Tasks;
using BankApp.Interfaces;
using BankApp.Models;
using FirstAPI.Models.DTOs.DoctorSpecialities;
using Microsoft.VisualBasic;

namespace BankApp.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<int, User> _userRepository;
        private readonly IRepository<int, TransactionLogDTO> _transactionLogDTORepository;
        public UserService(IRepository<int, User> userRepository,
                           IRepository<int, TransactionLogDTO> transactionLogDTORepository)
        {
            _userRepository = userRepository;
            _transactionLogDTORepository = transactionLogDTORepository;
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
            var Users = _userRepository.GetAll();
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
        public async Task<ICollection<TransactionLogDTO>> GetUserTransactionsById(int id)
        {
            var logs = _transactionLogDTORepository.GetAll();
            logs = logs.Where(t => t.SenderId == id || t.ReceiverId == id);
            List<TransactionLogDTO> transactionLogs = new();
            foreach (var log in logs)
            {
                transactionLogs.Add(convertTransactionLogtoDTO(log));
            }
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