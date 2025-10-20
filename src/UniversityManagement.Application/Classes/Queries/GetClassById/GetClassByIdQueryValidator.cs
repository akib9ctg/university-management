using FluentValidation;

namespace UniversityManagement.Application.Classes.Queries.GetClassById;

public sealed class GetClassByIdQueryValidator : AbstractValidator<GetClassByIdQuery>
{
    public GetClassByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}
