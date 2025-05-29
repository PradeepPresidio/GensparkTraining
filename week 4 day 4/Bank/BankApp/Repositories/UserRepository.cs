using BankApp.Models;
using BankApp.Interfaces;
using BankApp.Contexts;
using Microsoft.EntityFrameworkCore;
namespace BankApp.Interfaces {
	public class UserRepository : Repository<int, User>
	{

		public UserRepository(BankAppContext bankAppContext) : base(bankAppContext)
		{
		}
		public override async Task<User> Get(int key)
		{
			var User = await _bankAppContext.Users.SingleOrDefaultAsync(u => u.Id == key);
			return User ?? throw new Exception("User not found");
		}
		public override async Task<IEnumerable<User>> GetAll()
		{
			var users = _bankAppContext.Users;
			if (users.Count == 0)
				throw new Exception("No Users in DB");
			return (await users.ToListAsync());
		}
	}
}