namespace UniversityManagement.Application.Auth.Commands.SignUp
{
    public sealed record SignUpRequest
    (
        string FirstName,
        string LastName,
        string Email,
        string Password,
        string ConfirmPassword
    );
}
