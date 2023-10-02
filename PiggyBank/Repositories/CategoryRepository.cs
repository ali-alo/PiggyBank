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

        public ICollection<Category> GetUserCategories(string userId)
        {
            return _context.Categories.Where(c => c.Users.Any(u => u.Id == userId)).OrderByDescending(c => c.IsIncome).ThenBy(c => c.Name).ToList();
        }

        public ICollection<Category> GetUserIncomeCategories(string userId)
        {
            return _context.Categories.Where(c => c.Users.Any(u => u.Id == userId) && c.IsIncome).OrderBy(c => c.Name).ToList();
        }

        public ICollection<Category> GetUserExpenseCategories(string userId)
        {
            return _context.Categories.Where(c => c.Users.Any(u => u.Id == userId) && !c.IsIncome).OrderBy(c => c.Name).ToList();
        }

        public async Task<Category?> GetCategoryAsync(string userId, int categoryId)
        {
            return await _context.Categories.FirstOrDefaultAsync(c => c.Users.Any(u => u.Id == userId) && c.Id == categoryId);
        }

        public async Task<bool> RemoveCategory(int categoryId, ApplicationUser user)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == categoryId);
            if (category == null)
                return false;
            await RemoveCategoryFromUser(category, user);
            await _context.SaveChangesAsync();
            return true;
        }

        private async Task RemoveCategoryFromUser(Category category, ApplicationUser user)
        {
            var userCategoryTransactionIds = await GetCategoryTransactionIds(user.Id, category.Id);
            user.Transactions.RemoveAll(t => userCategoryTransactionIds.Contains(t.Id));
            user.Categories.Remove(category);
        }

        public async Task<int> GetCategoryTransactionsCount(string userId, int categoryId)
        {
            var count =  await _context.Transactions
                .Include(t => t.Category)
                .Where(t => t.User.Id == userId && t.Category.Id == categoryId)
                .CountAsync();
            return count;
        }

        private async Task<List<int>> GetCategoryTransactionIds(string userId, int categoryId) =>
           await _context.Transactions
                .Where(t => t.User.Id == userId && t.Category.Id == categoryId).Select(t => t.Id).ToListAsync();
    }
}
