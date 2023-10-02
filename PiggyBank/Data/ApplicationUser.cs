using Microsoft.AspNetCore.Identity;
using PiggyBank.Models;

namespace PiggyBank.Data
{
    public class ApplicationUser : IdentityUser
    {
        public List<Category> Categories { get; set; } = new List<Category>();
        public List<Transaction> Transactions { get; set; } = new List<Transaction>();
        public decimal Balance { get; set; }
    }
}
