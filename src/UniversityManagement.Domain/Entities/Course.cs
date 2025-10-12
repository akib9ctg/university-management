using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversityManagement.Domain.Common;

namespace UniversityManagement.Domain.Entities
{
    public class Course: BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public ICollection<CourseClass> CourseClasses { get; set; } = new List<CourseClass>();
        public ICollection<UserCourse> UserCourses { get; set; } = new List<UserCourse>();
        public ICollection<UserCourseClass> UserCoursesClasses { get; set; } = new List<UserCourseClass>();
    }
}
