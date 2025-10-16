using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading;
using System.Threading.Tasks;
using UniversityManagement.Application.Students;
using UniversityManagement.Application.Students.Commands.CreateStudent;
using UniversityManagement.Application.Students.Commands.DeleteStudent;
using UniversityManagement.Application.Students.Commands.EnrollStudentInClass;
using UniversityManagement.Application.Students.Commands.EnrollStudentInCourse;
using UniversityManagement.Application.Students.Commands.UpdateStudent;
using UniversityManagement.Application.Students.Queries.GetStudentById;
using UniversityManagement.Application.Students.Queries.GetStudentClassEnrollments;
using UniversityManagement.Application.Students.Queries.GetStudentClassmates;
using UniversityManagement.Application.Students.Queries.GetStudentCoursesAndClasses;
using UniversityManagement.Application.Students.Queries.GetStudents;
using UniversityManagement.API;

namespace UniversityManagement.API.Controllers
{
    [Authorize]
    [Route("api/students")]
    public class StudentsController : BaseApiController
    {
        private readonly ISender _sender;

        public StudentsController(ISender sender)
        {
            _sender = sender;
        }

        [Authorize(Policy = PolicyNames.StaffOnly)]
        [HttpPost]
        public async Task<IActionResult> CreateStudent(CreateStudentRequest request, CancellationToken cancellationToken = default)
        {
            var result = await _sender.Send(new CreateStudentCommand(request), cancellationToken);
            return Success(result);
        }

        [Authorize(Policy = PolicyNames.StaffOnly)]
        [HttpGet("{studentId:guid}")]
        public async Task<IActionResult> GetById(Guid studentId, CancellationToken cancellationToken = default)
        {
            var result = await _sender.Send(new GetStudentByIdQuery(studentId), cancellationToken);
            return Success(result);
        }

        [Authorize(Policy = PolicyNames.StaffOnly)]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetStudentsRequest request, CancellationToken cancellationToken = default)
        {
            var result = await _sender.Send(new GetStudentsQuery(request ?? new GetStudentsRequest()), cancellationToken);
            return Success(result);
        }

        [Authorize(Policy = PolicyNames.StaffOnly)]
        [HttpGet("{studentId:guid}/class-enrollments")]
        public async Task<IActionResult> GetClassEnrollments(Guid studentId, CancellationToken cancellationToken = default)
        {
            var result = await _sender.Send(new GetStudentClassEnrollmentsQuery(studentId), cancellationToken);
            return Success(result);
        }

        [Authorize(Policy = PolicyNames.StaffOnly)]
        [HttpPut("{studentId:guid}")]
        public async Task<IActionResult> Update(Guid studentId, UpdateStudentRequest request, CancellationToken cancellationToken = default)
        {
            if (studentId != request.Id)
            {
                return Failure("Route student identifier does not match request payload.", StatusCodes.Status400BadRequest);
            }

            var result = await _sender.Send(new UpdateStudentCommand(request), cancellationToken);
            return Success(result);
        }

        [Authorize(Policy = PolicyNames.StaffOnly)]
        [HttpDelete("{studentId:guid}")]
        public async Task<IActionResult> DeleteById(Guid studentId, CancellationToken cancellationToken = default)
        {
            var result = await _sender.Send(new DeleteStudentCommand(studentId), cancellationToken);
            return Success(result, "Student deleted successfully.");
        }

        [Authorize(Policy = PolicyNames.StaffOnly)]
        [HttpPost("{studentId:guid}/classes/{classId:guid}")]
        public async Task<IActionResult> EnrollInClass(Guid studentId, Guid classId, CancellationToken cancellationToken = default)
        {
            var assignedByUserId = GetCurrentUserId();
            var request = new EnrollStudentInClassRequest(studentId, classId, assignedByUserId);
            var result = await _sender.Send(new EnrollStudentInClassCommand(request), cancellationToken);
            return Success(result, "Student enrolled in class successfully.");
        }

        [Authorize(Policy = PolicyNames.StaffOnly)]
        [HttpPost("{studentId:guid}/courses/{courseId:guid}")]
        public async Task<IActionResult> EnrollInCourse(Guid studentId, Guid courseId, CancellationToken cancellationToken = default)
        {
            var assignedByUserId = GetCurrentUserId();
            var request = new EnrollStudentInCourseRequest(studentId, courseId, assignedByUserId);
            var result = await _sender.Send(new EnrollStudentInCourseCommand(request), cancellationToken);
            return Success(result, "Student enrolled in course successfully.");
        }

        [Authorize(Policy = PolicyNames.StudentOnly)]
        [HttpGet("me/courses")]
        public async Task<IActionResult> GetMyCoursesAndClasses(CancellationToken cancellationToken = default)
        {
            var studentId = GetCurrentUserId();

            if (studentId is null)
            {
                return Failure("Unable to determine the authenticated student.", StatusCodes.Status401Unauthorized);
            }

            var result = await _sender.Send(new GetStudentCoursesAndClassesQuery(studentId.Value), cancellationToken);
            return Success(result);
        }

        [Authorize(Policy = PolicyNames.StudentOnly)]
        [HttpGet("me/classmates")]
        public async Task<IActionResult> GetMyClassmates(CancellationToken cancellationToken = default)
        {
            var studentId = GetCurrentUserId();

            if (studentId is null)
            {
                return Failure("Unable to determine the authenticated student.", StatusCodes.Status401Unauthorized);
            }

            var result = await _sender.Send(new GetStudentClassmatesQuery(studentId.Value), cancellationToken);
            return Success(result);
        }
    }
}
