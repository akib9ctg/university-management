using FluentValidation;

namespace UniversityManagement.Application.Classes.Queries.GetClasses;

public sealed class GetClassesQueryValidator : AbstractValidator<GetClassesQuery>
{
    private const int MaxPageSize = 100;

    public GetClassesQueryValidator()
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
