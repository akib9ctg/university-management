using UniversityManagement.Domain.Entities;

namespace UniversityManagement.Application.Students.Queries.GetStudentClassEnrollments
{
    public sealed record StudentClassEnrollmentResponse(
        Guid EnrollmentId,
        Guid ClassId,
        string ClassName,
        Guid CourseId,
        string CourseName,
        Guid? AssignedByUserId,
        string? AssignedByUserFullName,
        string? AssignedByUserEmail,
        DateTime AssignedAt)
    {
        public static StudentClassEnrollmentResponse FromEntity(UserCourseClass enrollment)
        {
            var assignedBy = enrollment.AssignedByUser;
            var assignedByFullName = assignedBy is null
                ? null
                : $"{assignedBy.FirstName} {assignedBy.LastName}".Trim();

            return new StudentClassEnrollmentResponse(
                enrollment.Id,
                enrollment.ClassId,
                enrollment.Class.Name,
                enrollment.CourseId,
                enrollment.Course.Name,
                enrollment.AssignedByUserId,
                string.IsNullOrWhiteSpace(assignedByFullName) ? null : assignedByFullName,
                assignedBy?.Email,
                enrollment.AssignedAt);
        }
    }
}
