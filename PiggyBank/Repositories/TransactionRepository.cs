using Microsoft.EntityFrameworkCore;
using PiggyBank.Data;
using PiggyBank.Models;

namespace PiggyBank.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly ApplicationDbContext _context;

        public TransactionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<Transaction?> CreateAsync(Transaction transaction)
        {
            throw new NotImplementedException();
        }

        public Task<Transaction?> FindTransactionAsync(string userId, int transactionId)
        {
            throw new NotImplementedException();
        }

        public async Task<ICollection<Transaction>> GetAllUserTransactionsAsync(string userId)
        {
            var transactions = await _context.Transactions.Where(t => t.User.Id == userId).ToListAsync();
            if (!transactions.Any())
            {
                return new List<Transaction>();
            }
            return transactions;
        }
    }
}
