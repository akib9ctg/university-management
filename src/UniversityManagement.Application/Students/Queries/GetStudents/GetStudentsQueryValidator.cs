using FluentValidation;

namespace UniversityManagement.Application.Students.Queries.GetStudents;

public sealed class GetStudentsQueryValidator : AbstractValidator<GetStudentsQuery>
{
    private const int MaxPageSize = 100;

    public GetStudentsQueryValidator()
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

            RuleFor(x => x.Request.Email)
                .EmailAddress()
                .When(x => !string.IsNullOrWhiteSpace(x.Request.Email));
        });
    }
}

