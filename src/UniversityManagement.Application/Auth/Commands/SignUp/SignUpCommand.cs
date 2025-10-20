using MediatR;

namespace UniversityManagement.Application.Auth.Commands.SignUp
{
    public sealed record SignUpCommand
    (
        SignUpRequest SignUpRequest
    ) : IRequest<SignUpResponse>;
}
