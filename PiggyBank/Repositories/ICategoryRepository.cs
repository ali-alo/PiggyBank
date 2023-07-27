using PiggyBank.Data;
using PiggyBank.Models;

namespace PiggyBank.Repositories
{
    public interface ICategoryRepository
    {
        ICollection<Category> GetUserCategories(string userId);
        Task<bool> TryAddCategoryAsync(Category category, ApplicationUser user);
    }
}
