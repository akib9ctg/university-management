namespace UniversityManagement.Application.Common.Interfaces
{
    public interface ICurrentUserService
    {
        Guid? GetUserId();
    }
}
