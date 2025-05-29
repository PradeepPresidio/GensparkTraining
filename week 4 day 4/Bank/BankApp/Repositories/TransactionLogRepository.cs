using BankApp.Models;
using BankApp.Interfaces;
using BankApp.Contexts;
using Microsoft.EntityFrameworkCore;
namespace BankApp.Interfaces
{
    public class TransactionLogRepository: Repository<int, TransactionLog>
    {

        public TransactionLogRepository(BankAppContext bankAppContext) : base(bankAppContext)
        {
        }
        public override async Task<TransactionLog> Get(int key)
        {
            var TransactionLog = await _bankAppContext.TransactionLogs.SingleOrDefaultAsync(t => t.Id == key);
            return TransactionLog ?? throw new Exception("TransactionLog not found");
        }
        public override async Task<IEnumerable<TransactionLog>> GetAll()
        {
            var TransactionLogs = _bankAppContext.TransactionLogs;
            if (TransactionLogs.Count == 0)
                throw new Exception("No TransactionLogs in DB");
            return (await TransactionLogs.ToListAsync());
        }
    }
}