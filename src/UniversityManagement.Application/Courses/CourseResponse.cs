using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversityManagement.Application.Courses
{
    public sealed record CourseResponse
    (
        Guid Id,
        string Name,
        string? Description,
        DateTime CreatedAt,
        DateTime? ModifiedAt
    );
}
