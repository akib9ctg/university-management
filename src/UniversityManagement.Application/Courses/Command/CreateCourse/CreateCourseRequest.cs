using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversityManagement.Application.Courses.Command.CreateCourse
{
    public sealed record CreateCourseRequest
    (
        string name,
        string description
    );
}
