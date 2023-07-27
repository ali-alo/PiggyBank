using PiggyBank.Models;

namespace PiggyBank.Repositories
{
    public interface ICategoryRepository
    {
        ICollection<Category> GetUserCategories(string userId);
    }
}
