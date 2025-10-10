using MediatR;
using UniversityManagement.Domain.Entities;

namespace UniversityManagement.Application.Auth.Commands.SignUp
{
    public sealed record SignUpCommand
    (
        string Email, 
        string Password, 
        string FirstName,
        string LastName
    ) : IRequest<SignUpResponse>;
}
