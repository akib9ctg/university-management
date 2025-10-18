using MediatR;
using UniversityManagement.Application.Classes.Interfaces;
using UniversityManagement.Application.Students;

namespace UniversityManagement.Application.Classes.Queries.GetClassStudents
{
    public sealed class GetClassStudentsQueryHandler : IRequestHandler<GetClassStudentsQuery, List<StudentResponse>>
    {
        private readonly IClassRepository _classRepository;

        public GetClassStudentsQueryHandler(IClassRepository classRepository)
        {
            _classRepository = classRepository;
        }

        public async Task<List<StudentResponse>> Handle(GetClassStudentsQuery request, CancellationToken cancellationToken)
        {
            var students = await _classRepository.GetStudentsByClassIdAsync(request.ClassId, cancellationToken);
            return students.Select(StudentResponse.FromEntity).ToList();
        }
    }
}
