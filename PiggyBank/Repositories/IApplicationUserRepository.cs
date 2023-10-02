using PiggyBank.Data;

namespace PiggyBank.Repositories
{
    public interface IApplicationUserRepository
    {
        Task AddDefaultCategories(ApplicationUser user);
    }
}
