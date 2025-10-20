using FluentValidation;

namespace UniversityManagement.Application.Students.Commands.EnrollStudentInClass;

public sealed class EnrollStudentInClassCommandValidator : AbstractValidator<EnrollStudentInClassCommand>
{
    public EnrollStudentInClassCommandValidator()
    {
        RuleFor(x => x.Request)
            .NotNull()
            .WithMessage("Request payload is required.");

        When(x => x.Request is not null, () =>
        {
            RuleFor(x => x.Request.StudentId)
                .NotEmpty();

            RuleFor(x => x.Request.ClassId)
                .NotEmpty();
        });
    }
}
