using PiggyBank.Data;
using PiggyBank.Models;

namespace PiggyBank.Repositories
{
    public interface ICategoryRepository
    {
        ICollection<Category> GetUserCategories(string userId);
        ICollection<Category> GetUserIncomeCategories(string userId);
        ICollection<Category> GetUserExpenseCategories(string userId);
        Task<bool> TryAddCategoryAsync(Category category, ApplicationUser user);
        Task<bool> RemoveCategory(int categoryId, ApplicationUser user);
        Task<int> GetCategoryTransactionsCount(string userId, int categoryId);
        Task<Category?> GetCategoryAsync(string userId, int categoryId);
    }
}
