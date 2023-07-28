using Microsoft.AspNetCore.Identity;
using PiggyBank.Data;

namespace PiggyBank.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public bool IsIncome { get; set; }
        public Category Category { get; set; }
        public int CategoryId { get; set; }
        public DateTime Time { get; set; }
        public decimal Amount { get; set; }
        public string? Comment { get; set; }
        public ApplicationUser User { get; set; }
        public string UserId { get; set; }
    }
}
