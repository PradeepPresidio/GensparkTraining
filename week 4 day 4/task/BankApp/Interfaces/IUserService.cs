using System;
using BankApp.Models;
using BankApp.Models.DTOs;
namespace BankApp.Interfaces
{
	public interface IUserService
	{
		public Task<UserDTO> GetUserByAccNo(string accno);
		public Task<ICollection<TransactionLogDTO>> GetUserTransactionsById(int id);
		public Task<User> AddUser(UserDTO user);
	}
}