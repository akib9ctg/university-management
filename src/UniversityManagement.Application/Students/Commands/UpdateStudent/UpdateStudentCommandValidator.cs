using FluentValidation;

namespace UniversityManagement.Application.Students.Commands.UpdateStudent;

public sealed class UpdateStudentCommandValidator : AbstractValidator<UpdateStudentCommand>
{
    public UpdateStudentCommandValidator()
    {
        RuleFor(x => x.Request)
            .NotNull()
            .WithMessage("Request payload is required.");

        When(x => x.Request is not null, () =>
        {
            RuleFor(x => x.Request.Id)
                .NotEmpty();

            RuleFor(x => x.Request.FirstName)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Request.LastName)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Request.Email)
                .NotEmpty()
                .EmailAddress();
        });
    }
}
