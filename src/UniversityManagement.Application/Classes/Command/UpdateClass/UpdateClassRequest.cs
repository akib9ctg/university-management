namespace UniversityManagement.Application.Classes.Command.UpdateClass
{
    public sealed record UpdateClassRequest
    (
        Guid Id,
        string Name,
        string Description
    );
}
