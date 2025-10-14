using MediatR;
using Microsoft.AspNetCore.Mvc;
using UniversityManagement.Application.Classes.Command.CreateClass;
using UniversityManagement.Application.Classes.Command.DeleteClass;
using UniversityManagement.Application.Classes.Command.UpdateClass;
using UniversityManagement.Application.Classes.Queries.GetClassById;
using UniversityManagement.Application.Classes.Queries.GetClassCourses;
using UniversityManagement.Application.Classes.Queries.GetClassStudents;
using UniversityManagement.Application.Classes.Queries.GetClasses;

namespace UniversityManagement.API.Controllers
{
    public class ClassController : BaseApiController
    {
        private readonly ISender _sender;

        public ClassController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateClass(CreateClassRequest createClassRequest, CancellationToken cancellationToken = default)
        {
            var result = await _sender.Send(new CreateClassCommand(createClassRequest), cancellationToken);
            return Success(result);
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken = default)
        {
            var result = await _sender.Send(new GetClassByIdQuery(id), cancellationToken);
            return Success(result);
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll([FromQuery] GetClassesRequest request, CancellationToken cancellationToken = default)
        {
            var result = await _sender.Send(new GetClassesQuery(request ?? new GetClassesRequest()), cancellationToken);
            return Success(result);
        }

        [HttpGet("GetAllStudentsByClassId")]
        public async Task<IActionResult> GetAllStudentsByClassId(Guid classId, CancellationToken cancellationToken = default)
        {
            var result = await _sender.Send(new GetClassStudentsQuery(classId), cancellationToken);
            return Success(result);
        }

        [HttpGet("GetAllCoursesByClassId")]
        public async Task<IActionResult> GetAllCoursesByClassId(Guid classId, CancellationToken cancellationToken = default)
        {
            var result = await _sender.Send(new GetClassCoursesQuery(classId), cancellationToken);
            return Success(result);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(UpdateClassRequest updateClassRequest, CancellationToken cancellationToken = default)
        {
            var result = await _sender.Send(new UpdateClassCommand(updateClassRequest), cancellationToken);
            return Success(result);
        }

        [HttpDelete("DeleteById")]
        public async Task<IActionResult> DeleteById(Guid id, CancellationToken cancellationToken = default)
        {
            var result = await _sender.Send(new DeleteClassCommand(id), cancellationToken);
            return Success(result, "Class deleted successfully.");
        }
    }
}
