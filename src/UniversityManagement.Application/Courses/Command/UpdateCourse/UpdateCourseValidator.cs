using FluentValidation;

namespace UniversityManagement.Application.Courses.Command.UpdateCourse;

public sealed class UpdateCourseCommandValidator : AbstractValidator<UpdateCourseCommand>
{
    public UpdateCourseCommandValidator()
    {
        RuleFor(x => x.updateCourseRequest)
            .NotNull()
            .WithMessage("Request payload is required.");

        When(x => x.updateCourseRequest is not null, () =>
        {
            RuleFor(x => x.updateCourseRequest.Id)
                .NotEmpty();

            RuleFor(x => x.updateCourseRequest.Name)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.updateCourseRequest.Description)
                .NotEmpty()
                .MaximumLength(1000);
        });
    }
}
