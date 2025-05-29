using BankApp.Interfaces;
using BankApp.Models;
using BankApp.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using BankApp.Services;
namespace BankApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // POST: api/User
        [HttpPost]
        public async Task<ActionResult<User>> AddUser([FromBody] UserDTO userDto)
        {
            if (userDto == null)
                return BadRequest("User data is required.");

            var result = await _userService.AddUser(userDto);
            return CreatedAtAction(nameof(GetUserByAccountNumber), new { accno = result.AccountId }, result);
        }

        // GET: api/User/by-account/ACC123456
        [HttpGet("by-account/{accno}")]
        public async Task<ActionResult<UserDTO>> GetUserByAccountNumber(string accno)
        {
            var user = await _userService.GetUserByAccNo(accno);
            if (user == null)
                return NotFound($"User with account number {accno} not found.");
            return Ok(user);
        }

        // GET: api/User/transactions/5
        [HttpGet("transactions/{id}")]
        public async Task<ActionResult<ICollection<TransactionLogDTO>>> GetUserTransactionsById(int id)
        {
            var transactions = await _userService.GetUserTransactionsById(id);
            return Ok(transactions);
        }
    }
}
