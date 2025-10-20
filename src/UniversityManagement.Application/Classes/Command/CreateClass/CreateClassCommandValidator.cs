using FluentValidation;

namespace UniversityManagement.Application.Classes.Command.CreateClass;

public sealed class CreateClassCommandValidator : AbstractValidator<CreateClassCommand>
{
    public CreateClassCommandValidator()
    {
        RuleFor(x => x.CreateClassRequest)
            .NotNull()
            .WithMessage("Request payload is required.");

        When(x => x.CreateClassRequest is not null, () =>
        {
            RuleFor(x => x.CreateClassRequest.name)
                .NotEmpty()
                .Matches("^[A-Za-z0-9]+$")
                .WithMessage("Class name must contain only alphanumeric characters.")
                .MaximumLength(100);

            RuleFor(x => x.CreateClassRequest.description)
                .NotEmpty()
                .MaximumLength(1000);
        });
    }
}
