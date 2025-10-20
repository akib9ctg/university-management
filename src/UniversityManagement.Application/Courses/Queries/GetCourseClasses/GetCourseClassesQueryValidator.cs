using FluentValidation;

namespace UniversityManagement.Application.Courses.Queries.GetCourseClasses;

public sealed class GetCourseClassesQueryValidator : AbstractValidator<GetCourseClassesQuery>
{
    public GetCourseClassesQueryValidator()
    {
        RuleFor(x => x.CourseId)
            .NotEmpty();
    }
}
