namespace UniversityManagement.Application.Classes
{
    public sealed record ClassResponse
    (
        Guid Id,
        string Name,
        string? Description,
        DateTime CreatedAt,
        DateTime? ModifiedAt
    );
}
