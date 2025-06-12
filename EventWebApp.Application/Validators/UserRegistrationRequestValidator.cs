using EventWebApp.Application.DTOs;
using FluentValidation;

namespace EventWebApp.Application.Validators
{
    public class UserRegistrationRequestValidator : AbstractValidator<UserRegistrationRequest>
    {
        public UserRegistrationRequestValidator() 
        {
            RuleFor(u => u.FirstName).NotEmpty().MaximumLength(100);
            RuleFor(u => u.LastName).NotEmpty().MaximumLength(100);
            RuleFor(u => u.DateOfBirth).NotEmpty().LessThan(DateTime.Today);
            RuleFor(u => u.Email).NotEmpty().EmailAddress();
        }
    }
}
