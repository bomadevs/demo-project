namespace DemoProject.Domain.Entities
{
    public class QueryInfo
    {
        public string Query { get; set; }
        public bool? IsActive { get; set; }

        public QueryInfo(string query, bool? isActive = null)
        {
            Query = query;
            IsActive = isActive;
        }
    }
}
