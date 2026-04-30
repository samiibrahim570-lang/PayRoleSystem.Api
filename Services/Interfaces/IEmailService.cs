namespace PayRoleSystem.Services
{
    public interface IEmailService
    {

        //Task SendEmailAsync(string to, string subject = null, string body = null);
        Task<bool> SendEmailAsync(string to, string subject = null, string body = null);

    }
}
