namespace UniversityManagement.Application.Common.Models
{
    public static class PaginationDefaults
    {
        public const int DefaultPageNumber = 1;
        public const int DefaultPageSize = 10;

        public static int NormalizePageNumber(int pageNumber) =>
            pageNumber < 1 ? DefaultPageNumber : pageNumber;

        public static int NormalizePageSize(int pageSize) =>
            pageSize < 1 ? DefaultPageSize : pageSize;
    }
}
