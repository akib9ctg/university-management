using FluentValidation;

namespace UniversityManagement.Application.Courses.Queries.GetCourseById;

public sealed class GetCourseByIdQueryValidator : AbstractValidator<GetCourseByIdQuery>
{
    public GetCourseByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}
