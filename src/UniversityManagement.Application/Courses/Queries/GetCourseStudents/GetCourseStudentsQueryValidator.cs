using FluentValidation;

namespace UniversityManagement.Application.Courses.Queries.GetCourseStudents;

public sealed class GetCourseStudentsQueryValidator : AbstractValidator<GetCourseStudentsQuery>
{
    public GetCourseStudentsQueryValidator()
    {
        RuleFor(x => x.CourseId)
            .NotEmpty();
    }
}
