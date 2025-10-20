using FluentValidation;

namespace UniversityManagement.Application.Courses.Queries.GetCourses;

public sealed class GetCoursesQueryValidator : AbstractValidator<GetCoursesQuery>
{
    private const int MaxPageSize = 100;

    public GetCoursesQueryValidator()
    {
        When(x => x.Request is not null, () =>
        {
            RuleFor(x => x.Request.PageNumber)
                .GreaterThanOrEqualTo(1);

            RuleFor(x => x.Request.PageSize)
                .InclusiveBetween(1, MaxPageSize);

            RuleFor(x => x.Request.OrderBy)
                .NotEmpty();

            RuleFor(x => x.Request.SortDirection)
                .IsInEnum();
        });
    }
}
