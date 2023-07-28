using PiggyBank.Models;

namespace PiggyBank.Repositories
{
    public interface ITransactionRepository
    {
        Task CreateAsync(Transaction transaction);
        Task<Transaction?> FindTransactionAsync(string userId, int transactionId);
        Task<IList<Transaction>> GetAllUserTransactionsAsync(string userId);
        Task<IList<Transaction>> GetAllUserTransactionsByCategoryAsync(string userId, int categoryId);
        Task<bool> UpdateTransactionAsync(string userId, Transaction transactionUpdated);
        Task<bool> DeleteTransactionAsync(string userId, int transactionId);
    }
}
