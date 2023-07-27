namespace PiggyBank.Repositories
{
    public interface ITransactionRepository
    {
        Task<Models.Transaction?> CreateAsync(Models.Transaction transaction);
        Task<Models.Transaction?> FindTransactionAsync(string userId, int transactionId);
        Task<ICollection<Models.Transaction>> GetAllUserTransactionsAsync(string userId);
    }
}
