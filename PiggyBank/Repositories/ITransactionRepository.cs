namespace PiggyBank.Repositories
{
    public interface ITransactionRepository
    {
        Task<Models.Transaction?> CreateAsync(Models.Transaction transaction);
        Task<Models.Transaction?> FindTransactionAsync(int userId, int transactionId);
        Task<ICollection<Models.Transaction?>> GetAllTransactionsAsync(int userId);
    }
}
