using FluentValidation;

namespace UniversityManagement.Application.Classes.Command.UpdateClass;

public sealed class UpdateClassCommandValidator : AbstractValidator<UpdateClassCommand>
{
    public UpdateClassCommandValidator()
    {
        RuleFor(x => x.UpdateClassRequest)
            .NotNull()
            .WithMessage("Request payload is required.");

        When(x => x.UpdateClassRequest is not null, () =>
        {
            RuleFor(x => x.UpdateClassRequest.Id)
                .NotEmpty();

            RuleFor(x => x.UpdateClassRequest.Name)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.UpdateClassRequest.Description)
                .NotEmpty()
                .MaximumLength(1000);
        });
    }
}
