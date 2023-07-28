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

        public async Task CreateAsync(Transaction transaction)
        {
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();
        }

        public Task<Transaction?> FindTransactionAsync(string userId, int transactionId)
        {
            throw new NotImplementedException();
        }

        public async Task<IList<Transaction>> GetAllUserTransactionsAsync(string userId)
        {
            var transactions = await _context.Transactions.Include(t => t.Category).Where(t => t.User.Id == userId).OrderByDescending(t => t.Time).ToListAsync();
            if (!transactions.Any())
            {
                return new List<Transaction>();
            }
            return transactions;
        }
    }
}
