using PiggyBank.Data;
using PiggyBank.Models;

namespace PiggyBank.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;
        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // double check if you really need this
        public ICollection<Category> GetUserCategories(string userId)
        {
            return _context.Categories.Where(c => c.Users.Any(u => u.Id == userId)).ToList();
        }
    }
}
