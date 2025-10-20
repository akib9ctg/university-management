using FluentValidation;

namespace UniversityManagement.Application.Classes.Command.DeleteClass;

public sealed class DeleteClassCommandValidator : AbstractValidator<DeleteClassCommand>
{
    public DeleteClassCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}
