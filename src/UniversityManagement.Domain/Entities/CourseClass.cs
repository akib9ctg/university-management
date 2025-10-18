using UniversityManagement.Domain.Common;

namespace UniversityManagement.Domain.Entities
{
    public class CourseClass : BaseEntity
    {
        public Guid CourseId { get; set; }
        public Course Course { get; set; } = default!;
        public Guid ClassId { get; set; }
        public Class Class { get; set; } = default!;
    }
}
