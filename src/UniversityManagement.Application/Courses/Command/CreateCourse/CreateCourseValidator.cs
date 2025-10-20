using FluentValidation;

namespace UniversityManagement.Application.Courses.Command.CreateCourse;

public sealed class CreateCourseCommandValidator : AbstractValidator<CreateCourseCommand>
{
    public CreateCourseCommandValidator()
    {
        RuleFor(x => x.createCourseRequest)
            .NotNull()
            .WithMessage("Request payload is required.");

        When(x => x.createCourseRequest is not null, () =>
        {
            RuleFor(x => x.createCourseRequest.name)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.createCourseRequest.description)
                .NotEmpty()
                .MaximumLength(1000);
        });
    }
}
