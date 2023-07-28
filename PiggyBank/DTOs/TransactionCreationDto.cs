using Microsoft.AspNetCore.Mvc.ModelBinding;
using PiggyBank.CustomValidation;
using PiggyBank.Models;
using System.ComponentModel.DataAnnotations;

namespace PiggyBank.DTOs
{
    public class TransactionCreationDto
    {
        public bool IsIncome { get; set; }
        [Required(ErrorMessage = "Укажите категорию")]
        [Range(0, int.MaxValue, ErrorMessage = "Укажите категорию")]
        public int CategoryId { get; set; }
        [Required]
        [PastDateOnly(ErrorMessage = "Выбранная дата и время не могут быть в будущем")]
        public DateTime Time { get; set; } = DateTime.Now.Date + new TimeSpan(DateTime.Now.TimeOfDay.Hours,DateTime.Now.TimeOfDay.Minutes, 0);
        [Required]
        [Range(1000, Double.MaxValue, ErrorMessage = "Значение не может быть меньше одной тысячи сум")]
        public decimal Amount { get; set; }
        public string? Comment { get; set; }
        [BindNever]
        public ICollection<Category> UserCategories { get; set; } = new List<Category>();

    }
}
