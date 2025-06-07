using System.ComponentModel.DataAnnotations;

namespace EventWebApp.Core.CustomAttribute
{
    public class FutureDateValidationAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value is DateTime date)
            {
                return date > DateTime.Now;
            }
            return false;
        }
    }
}
