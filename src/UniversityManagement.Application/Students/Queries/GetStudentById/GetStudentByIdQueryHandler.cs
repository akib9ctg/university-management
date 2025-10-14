using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UniversityManagement.Application.Students;
using UniversityManagement.Application.Users.Interfaces;
using UniversityManagement.Domain.Enums;

namespace UniversityManagement.Application.Students.Queries.GetStudentById
{
    public sealed class GetStudentByIdQueryHandler : IRequestHandler<GetStudentByIdQuery, StudentResponse>
    {
        private readonly IUserRepository _userRepository;

        public GetStudentByIdQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<StudentResponse> Handle(GetStudentByIdQuery request, CancellationToken cancellationToken)
        {
            var student = await _userRepository.GetByIdAsync(request.Id, cancellationToken);

            if (student is null || student.Role != UserRole.Student)
            {
                throw new KeyNotFoundException($"Student with Id {request.Id} not found.");
            }

            return StudentResponse.FromEntity(student);
        }
    }
}
