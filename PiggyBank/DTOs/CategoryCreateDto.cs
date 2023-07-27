using System.ComponentModel.DataAnnotations;

namespace PiggyBank.DTOs
{
    public class CategoryCreateDto
    {
        [Display(Name = "Название")]
        [Required(ErrorMessage = "Поле имя не может быть пустым")]
        [MinLength(3, ErrorMessage = "Поле должно иметь минимум 3 символа")]
        public string Name { get; set; }
        [Display(Name = "Тип")]
        public bool IsIncome { get; set; }
    }
}
