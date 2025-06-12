using FluentValidation;
using EventWebApp.Application.DTOs;

namespace EventWebApp.Application.Validators
{
    public class RegisterUserToEventRequestValidator : AbstractValidator<RegisterUserToEventRequest>
    {
        public RegisterUserToEventRequestValidator()
        {
            RuleFor(r => r.UserId).NotEmpty();
            RuleFor(r => r.EventId).NotEmpty();
        }
    }
}
