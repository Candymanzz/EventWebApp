using EventWebApp.Application.DTOs;
using FluentValidation;

namespace EventWebApp.Application.Validators
{
    public class RegisterUserToEventRequestValidator : AbstractValidator<RegisterUserToEventRequest>
    {
        public RegisterUserToEventRequestValidator()
        {
            RuleFor(r => r.EventId).NotEmpty();
        }
    }
}
