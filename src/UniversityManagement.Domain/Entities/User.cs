using UniversityManagement.Domain.Common;
using UniversityManagement.Domain.Enums;

namespace UniversityManagement.Domain.Entities
{
    public class User : BaseEntity
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public UserRole Role { get; set; } = UserRole.Student;

        public ICollection<UserCourse> UserCourses { get; set; } = new List<UserCourse>();
        public ICollection<UserCourseClass> UserCoursesClass { get; set; } = new List<UserCourseClass>();
    }
}
