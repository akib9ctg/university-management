using FluentValidation;

namespace UniversityManagement.Application.Students.Queries.GetStudentCoursesAndClasses;

public sealed class GetStudentCoursesAndClassesQueryValidator : AbstractValidator<GetStudentCoursesAndClassesQuery>
{
    public GetStudentCoursesAndClassesQueryValidator()
    {
        RuleFor(x => x.StudentId)
            .NotEmpty();
    }
}

