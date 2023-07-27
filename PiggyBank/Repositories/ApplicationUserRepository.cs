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
        public void AddDefaultCategories(ApplicationUser user)
        {
            List<Category> categories = new List<Category>();
            for (int i = 1; i <= 10; i++)
            {
                var category = _context.Categories.FirstOrDefault(c => c.Id == i);
                if (category != null)
                    categories.Add(category);
            }
            user.Categories.AddRange(categories);
            _context.SaveChanges();
        }
    }
}
