using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversityManagement.Application.Courses.Command.DeleteCourse
{
    public sealed record DeleteCourseCommand(Guid id): IRequest;
}
