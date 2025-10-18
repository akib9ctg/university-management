using UniversityManagement.Domain.Common;

namespace UniversityManagement.Domain.Entities
{
    public class UserCourse : BaseEntity
    {
        public Guid UserId { get; set; }
        public User User { get; set; } = default!;

        public Guid CourseId { get; set; }
        public Course Course { get; set; } = default!;

        public Guid? AssignedByUserId { get; set; }
        public User? AssignedByUser { get; set; }

    }
}
