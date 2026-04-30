namespace PayRoleSystem.Http
{
    public class PageResult<T> where T : class
    {
        public int PageCount { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int RowCount { get; set; }
        public IEnumerable<T> Result { get; set; }

    }
}
