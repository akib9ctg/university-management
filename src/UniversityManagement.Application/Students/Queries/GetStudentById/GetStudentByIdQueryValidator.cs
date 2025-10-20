using FluentValidation;

namespace UniversityManagement.Application.Students.Queries.GetStudentById;

public sealed class GetStudentByIdQueryValidator : AbstractValidator<GetStudentByIdQuery>
{
    public GetStudentByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}

