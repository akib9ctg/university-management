using FluentValidation;

namespace UniversityManagement.Application.Auth.Commands.SignUp;

public sealed class SignUpCommandValidator : AbstractValidator<SignUpCommand>
{
    public SignUpCommandValidator()
    {
        RuleFor(x => x.SignUpRequest.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.SignUpRequest.Password)
            .NotEmpty()
            .MinimumLength(8);

        RuleFor(x => x.SignUpRequest.Email)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.SignUpRequest.Email)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.SignUpRequest.ConfirmPassword)
            .Equal(x => x.SignUpRequest.Password)
            .WithMessage("Passwords must match.");
    }
}
