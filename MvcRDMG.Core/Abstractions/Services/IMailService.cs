namespace MvcRDMG.Core.Abstractions.Services
{
    public interface IMailService
    {
        bool SendMail(string to, string from, string subject, string body);
    }
}