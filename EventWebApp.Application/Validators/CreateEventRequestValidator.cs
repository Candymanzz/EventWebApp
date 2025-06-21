using EventWebApp.Application.DTOs;
using FluentValidation;

namespace EventWebApp.Application.Validators
{
    public class CreateEventRequestValidator : AbstractValidator<CreateEventRequest>
    {
        public CreateEventRequestValidator()
        {
            RuleFor(e => e.Title).NotEmpty().MaximumLength(100);
            RuleFor(e => e.Description).NotEmpty();
            RuleFor(e => e.DateTime).NotEmpty().GreaterThan(DateTime.UtcNow);
            RuleFor(e => e.Location).NotEmpty().MaximumLength(200);
            RuleFor(e => e.Category).NotEmpty().MaximumLength(100);
            RuleFor(e => e.MaxParticipants).NotEmpty().GreaterThan(0);
        }
    }
}
