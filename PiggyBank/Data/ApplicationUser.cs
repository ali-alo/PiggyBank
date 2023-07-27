using Microsoft.AspNetCore.Identity;
using PiggyBank.Models;

namespace PiggyBank.Data
{
    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        public List<Category> Categories { get; set; } = new List<Category>();
        [PersonalData]
        public List<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
