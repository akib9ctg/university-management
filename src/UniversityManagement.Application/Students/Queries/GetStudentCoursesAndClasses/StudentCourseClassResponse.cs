using System;
using UniversityManagement.Domain.Entities;

namespace UniversityManagement.Application.Students.Queries.GetStudentCoursesAndClasses
{
    public sealed record StudentCourseClassResponse(
        Guid CourseId,
        string CourseName,
        Guid ClassId,
        string ClassName)
    {
        public static StudentCourseClassResponse FromEntity(UserCourseClass enrollment) =>
            new(
                enrollment.CourseId,
                enrollment.Course.Name,
                enrollment.ClassId,
                enrollment.Class.Name);
    }
}
