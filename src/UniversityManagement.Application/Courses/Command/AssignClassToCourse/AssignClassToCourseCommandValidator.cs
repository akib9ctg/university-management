using FluentValidation;

namespace UniversityManagement.Application.Courses.Command.AssignClassToCourse;

public sealed class AssignClassToCourseCommandValidator : AbstractValidator<AssignClassToCourseCommand>
{
    public AssignClassToCourseCommandValidator()
    {
        RuleFor(x => x.CourseId)
            .NotEmpty();

        RuleFor(x => x.ClassId)
            .NotEmpty();
    }
}
