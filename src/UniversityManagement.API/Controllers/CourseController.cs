using MediatR;
using Microsoft.AspNetCore.Mvc;
using UniversityManagement.Application.Courses.Command.CreateCourse;
using UniversityManagement.Application.Courses.Command.DeleteCourse;
using UniversityManagement.Application.Courses.Command.UpdateCourse;
using UniversityManagement.Application.Courses.Queries.GetCourseById;
using UniversityManagement.Application.Courses.Queries.GetCourses;

namespace UniversityManagement.API.Controllers
{
    public class CourseController : BaseApiController
    {
        private readonly ISender _sender;

        public CourseController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateCourse(CreateCourseRequest createCourseRequest, CancellationToken cancellationToken = default)
        {
            var result = await _sender.Send(new CreateCourseCommand(createCourseRequest), cancellationToken);
            return Success(result);
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken = default)
        {
            var result = await _sender.Send(new GetCourseByIdQuery(id), cancellationToken);
            return Success(result);
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll([FromQuery] GetCoursesRequest request, CancellationToken cancellationToken = default)
        {
            var result = await _sender.Send(new GetCoursesQuery(request ?? new GetCoursesRequest()), cancellationToken);
            return Success(result);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(UpdateCourseRequest updateCourseRequest, CancellationToken cancellationToken = default)
        {
            var result = await _sender.Send(new UpdateCourseCommand(updateCourseRequest), cancellationToken);
            return Success(result);
        }

        [HttpDelete("DeleteById")]
        public async Task<IActionResult> DeleteById(Guid id, CancellationToken cancellationToken = default)
        {
            var result = await _sender.Send(new DeleteCourseCommand(id), cancellationToken);
            return Success(result, "Course deleted successfully.");
        }
    }
}
