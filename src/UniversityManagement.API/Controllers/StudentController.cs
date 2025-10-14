using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;
using UniversityManagement.Application.Students;
using UniversityManagement.Application.Students.Commands.CreateStudent;
using UniversityManagement.Application.Students.Commands.DeleteStudent;
using UniversityManagement.Application.Students.Commands.EnrollStudentInClass;
using UniversityManagement.Application.Students.Commands.EnrollStudentInCourse;
using UniversityManagement.Application.Students.Commands.UpdateStudent;
using UniversityManagement.Application.Students.Queries.GetStudentById;
using UniversityManagement.Application.Students.Queries.GetStudents;

namespace UniversityManagement.API.Controllers
{
    public class StudentController : BaseApiController
    {
        private readonly ISender _sender;

        public StudentController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateStudent(CreateStudentRequest request, CancellationToken cancellationToken = default)
        {
            var result = await _sender.Send(new CreateStudentCommand(request), cancellationToken);
            return Success(result);
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken = default)
        {
            var result = await _sender.Send(new GetStudentByIdQuery(id), cancellationToken);
            return Success(result);
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll([FromQuery] GetStudentsRequest request, CancellationToken cancellationToken = default)
        {
            var result = await _sender.Send(new GetStudentsQuery(request ?? new GetStudentsRequest()), cancellationToken);
            return Success(result);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(UpdateStudentRequest request, CancellationToken cancellationToken = default)
        {
            var result = await _sender.Send(new UpdateStudentCommand(request), cancellationToken);
            return Success(result);
        }

        [HttpDelete("DeleteById")]
        public async Task<IActionResult> DeleteById(Guid id, CancellationToken cancellationToken = default)
        {
            var result = await _sender.Send(new DeleteStudentCommand(id), cancellationToken);
            return Success(result, "Student deleted successfully.");
        }

        [HttpPost("enroll-class")]
        public async Task<IActionResult> EnrollInClass(EnrollStudentInClassRequest request, CancellationToken cancellationToken = default)
        {
            var result = await _sender.Send(new EnrollStudentInClassCommand(request), cancellationToken);
            return Success(result, "Student enrolled in class successfully.");
        }

        [HttpPost("enroll-course")]
        public async Task<IActionResult> EnrollInCourse(EnrollStudentInCourseRequest request, CancellationToken cancellationToken = default)
        {
            var result = await _sender.Send(new EnrollStudentInCourseCommand(request), cancellationToken);
            return Success(result, "Student enrolled in course successfully.");
        }
    }
}
        
