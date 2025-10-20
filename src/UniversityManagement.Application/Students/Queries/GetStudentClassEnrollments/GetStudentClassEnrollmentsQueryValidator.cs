using FluentValidation;

namespace UniversityManagement.Application.Students.Queries.GetStudentClassEnrollments;

public sealed class GetStudentClassEnrollmentsQueryValidator : AbstractValidator<GetStudentClassEnrollmentsQuery>
{
    public GetStudentClassEnrollmentsQueryValidator()
    {
        RuleFor(x => x.StudentId)
            .NotEmpty();
    }
}

