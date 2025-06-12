using FluentValidation;
using EventWebApp.Application.DTOs;

namespace EventWebApp.Application.Validators
{
    public class UpdateEventRequestValidator : AbstractValidator<UpdateEventRequest>
    {
        public UpdateEventRequestValidator()
        {
            RuleFor(e => e.Id).NotEmpty();
            RuleFor(e => e.Title).NotEmpty().MaximumLength(100);
            RuleFor(e => e.Description).NotEmpty();
            RuleFor(e => e.DateTime).GreaterThan(DateTime.UtcNow).WithMessage("Date must be in the future");
            RuleFor(e => e.Location).NotEmpty().MaximumLength(200);
            RuleFor(e => e.Category).NotEmpty().MaximumLength(100);
            RuleFor(e => e.MaxParticipants).GreaterThan(0);
        }
    }
}
