using FluentValidation;

namespace UniversityManagement.Application.Students.Commands.CreateStudent;

public sealed class CreateStudentCommandValidator : AbstractValidator<CreateStudentCommand>
{
    public CreateStudentCommandValidator()
    {
        RuleFor(x => x.Request)
            .NotNull()
            .WithMessage("Request payload is required.");

        When(x => x.Request is not null, () =>
        {
            RuleFor(x => x.Request.FirstName)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Request.LastName)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Request.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Request.Password)
                .NotEmpty()
                .MinimumLength(8);

            RuleFor(x => x.Request.ConfirmPassword)
                .Equal(x => x.Request.Password)
                .WithMessage("Password and Confirm Password must match.");
        });
    }
}
