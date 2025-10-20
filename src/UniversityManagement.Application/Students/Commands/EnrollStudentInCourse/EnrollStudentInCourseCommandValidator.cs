using FluentValidation;

namespace UniversityManagement.Application.Students.Commands.EnrollStudentInCourse;

public sealed class EnrollStudentInCourseCommandValidator : AbstractValidator<EnrollStudentInCourseCommand>
{
    public EnrollStudentInCourseCommandValidator()
    {
        RuleFor(x => x.Request)
            .NotNull()
            .WithMessage("Request payload is required.");

        When(x => x.Request is not null, () =>
        {
            RuleFor(x => x.Request.StudentId)
                .NotEmpty();

            RuleFor(x => x.Request.CourseId)
                .NotEmpty();
        });
    }
}
