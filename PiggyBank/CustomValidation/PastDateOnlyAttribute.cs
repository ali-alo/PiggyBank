using System.ComponentModel.DataAnnotations;

namespace PiggyBank.CustomValidation
{
    public class PastDateOnlyAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            DateTime? dateTime = (DateTime?)value;
            if (dateTime.HasValue)
            {
                if (DateTime.UtcNow >= dateTime.Value.ToUniversalTime())
                {
                    return true;
                }
            }
            return false;
        }
    }
}
