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
                .Matches("^[A-Za-z0-9]+$")
                .WithMessage("Class name must contain only alphanumeric characters.")
                .MaximumLength(100);

            RuleFor(x => x.UpdateClassRequest.Description)
                .NotEmpty()
                .MaximumLength(1000);
        });
    }
}
