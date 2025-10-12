using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversityManagement.Application.Courses.Command.UpdateCourse
{
    public sealed record UpdateCourseRequest
    (
        Guid Id,
        string Name,
        string Description
    );
}
