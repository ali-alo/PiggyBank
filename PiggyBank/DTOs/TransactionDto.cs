using PiggyBank.Models;
using System.ComponentModel.DataAnnotations;

namespace PiggyBank.DTOs
{
    public class TransactionCreationDto
    {
        public bool IsIncome { get; set; }
        [Required]
        public Category Category { get; set; }
        public DateTime Time { get; set; } = DateTime.UtcNow;
        [Required]
        [Range(1000,Double.MaxValue, ErrorMessage = "Значение не может быть меньше одной тысячи")]
        public decimal Amount { get; set; }
        public string? Comment { get; set; }

    }
}
