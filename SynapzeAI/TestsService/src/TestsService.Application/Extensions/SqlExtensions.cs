using Dapper;

namespace TestsService.Application.Extensions
{
    public static class SqlExtensions
    {
        public static string ApplyPagination(
            int page,
            int pageSize,
            DynamicParameters parameters)
        {
            parameters.Add("@PageSize", pageSize);
            parameters.Add("@Offset", (page - 1) * pageSize);

            return " LIMIT @PageSize OFFSET @Offset";
        }

        public static string ApplySorting(
            DynamicParameters parameters,
            string? orderBy = null)
        {
            parameters.Add("@OrderBy", orderBy ?? "id");
            
            return " ORDER BY @OrderBy";
        }

        public static string ApplySearch(
            DynamicParameters parameters,
            string fieldName,
            string value)
        {
            parameters.Add("@FieldName", fieldName);
            parameters.Add("@Value", value);
            
            return " WHERE @FieldName ILIKE '%' || @Value || '%'";
        }
    }
}
