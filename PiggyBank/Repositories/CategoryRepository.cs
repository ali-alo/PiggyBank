using Microsoft.EntityFrameworkCore;
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

        public async Task<bool> TryAddCategoryAsync(Category category, ApplicationUser user)
        {
            var cat = await _context.Categories.Include(c => c.Users)
                .FirstOrDefaultAsync(c => c.Name.Trim().ToLower() == category.Name.Trim().ToLower() && c.IsIncome == category.IsIncome);
            if (cat == null)
            {
                category.Users.Add(user);
                _context.Categories.Add(category);
            }
            else
            {
                if (cat.Users.Contains(user))
                {
                    return false;
                }
                cat.Users.Add(user);
            }
            await _context.SaveChangesAsync();
            return true;
        }

        // double check if you really need this
        public ICollection<Category> GetUserCategories(string userId)
        {
            return _context.Categories.Where(c => c.Users.Any(u => u.Id == userId)).OrderByDescending(c => c.IsIncome).ThenBy(c => c.Name).ToList();
        }
    }
}
