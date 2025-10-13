namespace UniversityManagement.Application.Classes.Command.CreateClass
{
    public sealed record CreateClassRequest
    (
        string name,
        string description
    );
}
