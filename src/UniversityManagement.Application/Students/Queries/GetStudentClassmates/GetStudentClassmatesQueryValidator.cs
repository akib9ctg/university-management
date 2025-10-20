using FluentValidation;

namespace UniversityManagement.Application.Students.Queries.GetStudentClassmates;

public sealed class GetStudentClassmatesQueryValidator : AbstractValidator<GetStudentClassmatesQuery>
{
    public GetStudentClassmatesQueryValidator()
    {
        RuleFor(x => x.StudentId)
            .NotEmpty();
    }
}

