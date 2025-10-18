using UniversityManagement.Domain.Common;

namespace UniversityManagement.Domain.Entities
{
    public class Class : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public ICollection<CourseClass> CourseClasses { get; set; } = new List<CourseClass>();
        public ICollection<UserCourseClass> UserCourseClasses { get; set; } = new List<UserCourseClass>();

    }
}
