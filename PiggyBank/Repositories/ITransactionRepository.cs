using PiggyBank.Models;

namespace PiggyBank.Repositories
{
    public interface ITransactionRepository
    {
        Task CreateAsync(Transaction transaction);
        Task<Transaction?> FindTransactionAsync(string userId, int transactionId);
        Task<IList<Transaction>> GetAllUserTransactionsAsync(string userId);
    }
}
