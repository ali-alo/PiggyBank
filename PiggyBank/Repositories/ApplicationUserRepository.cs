using Microsoft.EntityFrameworkCore;
using PiggyBank.Data;
using PiggyBank.Models;

namespace PiggyBank.Repositories
{
    public class ApplicationUserRepository : IApplicationUserRepository
    {
        private readonly ApplicationDbContext _context;

        public ApplicationUserRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddDefaultCategories(ApplicationUser user)
        {
            List<Category> categories = new List<Category>();
            for (int i = 1; i <= 10; i++)
            {
                var category = _context.Categories.FirstOrDefault(c => c.Id == i);
                if (category != null)
                    categories.Add(category);
            }
            user.Categories.AddRange(categories);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateBalance(ApplicationUser user, decimal amount)
        {
            user.Balance += amount;
            await _context.SaveChangesAsync();
        }
    }
}
