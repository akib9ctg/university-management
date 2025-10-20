using FluentValidation;

namespace UniversityManagement.Application.Classes.Queries.GetClassStudents;

public sealed class GetClassStudentsQueryValidator : AbstractValidator<GetClassStudentsQuery>
{
    public GetClassStudentsQueryValidator()
    {
        RuleFor(x => x.ClassId)
            .NotEmpty();
    }
}
