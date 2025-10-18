namespace UniversityManagement.Application.Auth.Commands.Login
{
    public sealed record LoginRequest
    (
        string Email,
        string Password
    );
}
