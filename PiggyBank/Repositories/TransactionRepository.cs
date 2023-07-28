using Microsoft.EntityFrameworkCore;
using PiggyBank.Data;
using PiggyBank.DTOs;
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

        public async Task<Transaction?> FindTransactionAsync(string userId, int transactionId)
        {
            var transaction = await _context.Transactions.FirstOrDefaultAsync(t => t.Id == transactionId && t.UserId == userId);
            return transaction;
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

        public async Task<IList<Transaction>> GetAllUserTransactionsByCategoryAsync(string userId, int categoryId)
        {
            var transactions = await _context.Transactions.Include(t => t.Category).Where(t => t.User.Id == userId && t.Category.Id == categoryId).OrderByDescending(t => t.Time).ToListAsync();
            if (!transactions.Any())
            {
                return new List<Transaction>();
            }
            return transactions;
        }

        public async Task<bool> UpdateTransactionAsync(string userId, Transaction transactionUpdated)
        {
            var transaction = await FindTransactionAsync(userId, transactionUpdated.Id);
            if (transaction != null)
            {
                transaction = transactionUpdated;
            }
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteTransactionAsync(string userId, int transactionId)
        {
            var transaction = await _context.Transactions.FirstOrDefaultAsync(t => t.UserId == userId && t.Id == transactionId);
            if (transaction == null)
            {
                return false;
            }
            _context.Transactions.Remove(transaction);
            _context.SaveChanges();
            return true;

        }
    }
}
