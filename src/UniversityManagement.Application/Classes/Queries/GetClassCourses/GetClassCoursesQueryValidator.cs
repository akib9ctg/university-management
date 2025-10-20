using FluentValidation;

namespace UniversityManagement.Application.Classes.Queries.GetClassCourses;

public sealed class GetClassCoursesQueryValidator : AbstractValidator<GetClassCoursesQuery>
{
    public GetClassCoursesQueryValidator()
    {
        RuleFor(x => x.ClassId)
            .NotEmpty();
    }
}
