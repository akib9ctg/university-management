using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UniversityManagement.API;
using UniversityManagement.Application.Classes.Command.CreateClass;
using UniversityManagement.Application.Classes.Command.DeleteClass;
using UniversityManagement.Application.Classes.Command.UpdateClass;
using UniversityManagement.Application.Classes.Queries.GetClassById;
using UniversityManagement.Application.Classes.Queries.GetClassCourses;
using UniversityManagement.Application.Classes.Queries.GetClassStudents;
using UniversityManagement.Application.Classes.Queries.GetClasses;

namespace UniversityManagement.API.Controllers
{
    [Authorize(Policy = PolicyNames.StaffOnly)]
    [Route("api/classes")]
    public class ClassesController : BaseApiController
    {
        private readonly ISender _sender;

        public ClassesController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost]
        public async Task<IActionResult> CreateClass(CreateClassRequest createClassRequest, CancellationToken cancellationToken = default)
        {
            var result = await _sender.Send(new CreateClassCommand(createClassRequest), cancellationToken);
            return Success(result);
        }

        [HttpGet("{classId:guid}")]
        public async Task<IActionResult> GetById(Guid classId, CancellationToken cancellationToken = default)
        {
            var result = await _sender.Send(new GetClassByIdQuery(classId), cancellationToken);
            return Success(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetClassesRequest request, CancellationToken cancellationToken = default)
        {
            var result = await _sender.Send(new GetClassesQuery(request ?? new GetClassesRequest()), cancellationToken);
            return Success(result);
        }

        [HttpGet("{classId:guid}/students")]
        public async Task<IActionResult> GetStudents(Guid classId, CancellationToken cancellationToken = default)
        {
            var result = await _sender.Send(new GetClassStudentsQuery(classId), cancellationToken);
            return Success(result);
        }

        [HttpGet("{classId:guid}/courses")]
        public async Task<IActionResult> GetCourses(Guid classId, CancellationToken cancellationToken = default)
        {
            var result = await _sender.Send(new GetClassCoursesQuery(classId), cancellationToken);
            return Success(result);
        }

        [HttpPut("{classId:guid}")]
        public async Task<IActionResult> Update(Guid classId, UpdateClassRequest updateClassRequest, CancellationToken cancellationToken = default)
        {
            if (classId != updateClassRequest.Id)
            {
                return Failure("Route class identifier does not match request payload.", StatusCodes.Status400BadRequest);
            }

            var result = await _sender.Send(new UpdateClassCommand(updateClassRequest), cancellationToken);
            return Success(result);
        }

        [HttpDelete("{classId:guid}")]
        public async Task<IActionResult> Delete(Guid classId, CancellationToken cancellationToken = default)
        {
            var result = await _sender.Send(new DeleteClassCommand(classId), cancellationToken);
            return Success(result, "Class deleted successfully.");
        }
    }
}
