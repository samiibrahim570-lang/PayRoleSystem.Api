namespace PayRoleSystem.Http
{
    public interface IPageRequest
    {
        int Page { get; set; }
        int PageSize { get; set; }
    }
}
