namespace UniversityManagement.Application.Students.Commands.CreateStudent
{
    public sealed record CreateStudentRequest
    (
        string FirstName,
        string LastName,
        string Email,
        string Password,
        string ConfirmPassword
    );
}
