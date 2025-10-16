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
    public class StudentController : BaseApiController
    {
        private readonly ISender _sender;

        public StudentController(ISender sender)
        {
            _sender = sender;
        }

        [Authorize(Policy = PolicyNames.StaffOnly)]
        [HttpPost("create")]
        public async Task<IActionResult> CreateStudent(CreateStudentRequest request, CancellationToken cancellationToken = default)
        {
            var result = await _sender.Send(new CreateStudentCommand(request), cancellationToken);
            return Success(result);
        }

        [Authorize(Policy = PolicyNames.StaffOnly)]
        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken = default)
        {
            var result = await _sender.Send(new GetStudentByIdQuery(id), cancellationToken);
            return Success(result);
        }

        [Authorize(Policy = PolicyNames.StaffOnly)]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll([FromQuery] GetStudentsRequest request, CancellationToken cancellationToken = default)
        {
            var result = await _sender.Send(new GetStudentsQuery(request ?? new GetStudentsRequest()), cancellationToken);
            return Success(result);
        }

        [Authorize(Policy = PolicyNames.StudentOnly)]
        [HttpGet("courses-and-classes")]
        public async Task<IActionResult> GetCoursesAndClasses(CancellationToken cancellationToken = default)
        {
            var studentId = GetCurrentStudentId();

            if (studentId is null)
            {
                return Failure("Unable to determine the authenticated student.", StatusCodes.Status401Unauthorized);
            }

            var result = await _sender.Send(new GetStudentCoursesAndClassesQuery(studentId.Value), cancellationToken);
            return Success(result);
        }

        [Authorize(Policy = PolicyNames.StudentOnly)]
        [HttpGet("classmates")]
        public async Task<IActionResult> GetClassmates(CancellationToken cancellationToken = default)
        {
            var studentId = GetCurrentStudentId();

            if (studentId is null)
            {
                return Failure("Unable to determine the authenticated student.", StatusCodes.Status401Unauthorized);
            }

            var result = await _sender.Send(new GetStudentClassmatesQuery(studentId.Value), cancellationToken);
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
        [HttpPut("Update")]
        public async Task<IActionResult> Update(UpdateStudentRequest request, CancellationToken cancellationToken = default)
        {
            var result = await _sender.Send(new UpdateStudentCommand(request), cancellationToken);
            return Success(result);
        }

        [Authorize(Policy = PolicyNames.StaffOnly)]
        [HttpDelete("DeleteById")]
        public async Task<IActionResult> DeleteById(Guid id, CancellationToken cancellationToken = default)
        {
            var result = await _sender.Send(new DeleteStudentCommand(id), cancellationToken);
            return Success(result, "Student deleted successfully.");
        }

        [Authorize(Policy = PolicyNames.StaffOnly)]
        [HttpPost("enroll-class")]
        public async Task<IActionResult> EnrollInClass(EnrollStudentInClassRequest request, CancellationToken cancellationToken = default)
        {
            var result = await _sender.Send(new EnrollStudentInClassCommand(request), cancellationToken);
            return Success(result, "Student enrolled in class successfully.");
        }

        [Authorize(Policy = PolicyNames.StaffOnly)]
        [HttpPost("enroll-course")]
        public async Task<IActionResult> EnrollInCourse(EnrollStudentInCourseRequest request, CancellationToken cancellationToken = default)
        {
            var result = await _sender.Send(new EnrollStudentInCourseCommand(request), cancellationToken);
            return Success(result, "Student enrolled in course successfully.");
        }

        private Guid? GetCurrentStudentId()
        {
            var subjectClaim = User?.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

            return Guid.TryParse(subjectClaim, out var studentId)
                ? studentId
                : null;
        }
    }
}
        
