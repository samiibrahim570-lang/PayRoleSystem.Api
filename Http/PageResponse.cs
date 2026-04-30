namespace PayRoleSystem.Http
{
    public class PageResponse<T> where T : class
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int PageCount { get; set; }
        public int RowCount { get; set; }
        public IEnumerable<T> Rows { get; set; }
    }
}
