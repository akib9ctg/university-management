using System;
using UniversityManagement.Domain.Entities;

namespace UniversityManagement.Application.Classes
{
    public sealed record ClassResponse(
        Guid Id,
        string Name,
        string? Description,
        DateTime CreatedAt,
        DateTime? ModifiedAt)
    {
        public static ClassResponse FromEntity(Class @class) =>
            new(
                @class.Id,
                @class.Name,
                @class.Description,
                @class.CreatedAt,
                @class.ModifiedAt);
    }
}
