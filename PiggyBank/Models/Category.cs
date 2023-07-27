using PiggyBank.Data;

namespace PiggyBank.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsIncome { get; set; }
        public List<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();

    }
}
