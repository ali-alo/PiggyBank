using PiggyBank.Data;

namespace PiggyBank.Repositories
{
    public interface IApplicationUserRepository
    {
        void AddDefaultCategories(ApplicationUser user);
    }
}
