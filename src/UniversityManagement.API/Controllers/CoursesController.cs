using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversityManagement.Application.Courses.Command.AssignClassToCourse;
using UniversityManagement.Application.Courses.Command.CreateCourse;
using UniversityManagement.Application.Courses.Command.DeleteCourse;
using UniversityManagement.Application.Courses.Command.UpdateCourse;
using UniversityManagement.Application.Courses.Queries.GetCourseById;
using UniversityManagement.Application.Courses.Queries.GetCourseClasses;
using UniversityManagement.Application.Courses.Queries.GetCourses;
using UniversityManagement.Application.Courses.Queries.GetCourseStudents;

namespace UniversityManagement.API.Controllers
{
    [Authorize(Policy = PolicyNames.StaffOnly)]
    [Route("api/courses")]
    public class CoursesController : BaseApiController
    {
        private readonly ISender _sender;

        public CoursesController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCourse(CreateCourseRequest createCourseRequest, CancellationToken cancellationToken = default)
        {
            var result = await _sender.Send(new CreateCourseCommand(createCourseRequest), cancellationToken);
            return Success(result);
        }

        [HttpGet("{courseId:guid}")]
        public async Task<IActionResult> GetById(Guid courseId, CancellationToken cancellationToken = default)
        {
            var result = await _sender.Send(new GetCourseByIdQuery(courseId), cancellationToken);
            return Success(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetCoursesRequest request, CancellationToken cancellationToken = default)
        {
            var result = await _sender.Send(new GetCoursesQuery(request ?? new GetCoursesRequest()), cancellationToken);
            return Success(result);
        }

        [HttpGet("{courseId:guid}/students")]
        public async Task<IActionResult> GetStudents(Guid courseId, CancellationToken cancellationToken = default)
        {
            var result = await _sender.Send(new GetCourseStudentsQuery(courseId), cancellationToken);
            return Success(result);
        }

        [HttpGet("{courseId:guid}/classes")]
        public async Task<IActionResult> GetClasses(Guid courseId, CancellationToken cancellationToken = default)
        {
            var result = await _sender.Send(new GetCourseClassesQuery(courseId), cancellationToken);
            return Success(result);
        }

        [HttpPost("{courseId:guid}/classes/{classId:guid}")]
        public async Task<IActionResult> AddClass(Guid courseId, Guid classId, CancellationToken cancellationToken = default)
        {
            var created = await _sender.Send(new AssignClassToCourseCommand(courseId, classId), cancellationToken);
            var message = created
                ? "Class linked to course successfully."
                : "Class is already linked to this course.";

            return Success(created, message);
        }

        [HttpPut("{courseId:guid}")]
        public async Task<IActionResult> Update(Guid courseId, UpdateCourseRequest updateCourseRequest, CancellationToken cancellationToken = default)
        {
            if (courseId != updateCourseRequest.Id)
            {
                return Failure("Route course identifier does not match request payload.", StatusCodes.Status400BadRequest);
            }

            var result = await _sender.Send(new UpdateCourseCommand(updateCourseRequest), cancellationToken);
            return Success(result);
        }

        [HttpDelete("{courseId:guid}")]
        public async Task<IActionResult> Delete(Guid courseId, CancellationToken cancellationToken = default)
        {
            var result = await _sender.Send(new DeleteCourseCommand(courseId), cancellationToken);
            return Success(result, "Course deleted successfully.");
        }
    }
}
