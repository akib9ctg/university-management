using MediatR;
using UniversityManagement.Application.Users.Interfaces;
using UniversityManagement.Domain.Enums;

namespace UniversityManagement.Application.Students.Commands.DeleteStudent
{
    public sealed class DeleteStudentCommandHandler : IRequestHandler<DeleteStudentCommand, bool>
    {
        private readonly IUserRepository _userRepository;

        public DeleteStudentCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> Handle(DeleteStudentCommand request, CancellationToken cancellationToken)
        {
            var student = await _userRepository.GetByIdAsync(request.Id, cancellationToken);

            if (student is null || student.Role != UserRole.Student)
            {
                throw new KeyNotFoundException($"Student with Id {request.Id} not found.");
            }

            await _userRepository.DeleteAsync(student, cancellationToken);

            return true;
        }
    }
}
