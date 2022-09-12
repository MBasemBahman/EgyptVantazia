using Entities.ServicesModels;

namespace Contracts.Services
{
    public interface IEmailSender
    {
        Task SendEmail(EmailMessage message);
        Task SendHtmlEmail(EmailMessage message);
    }
}
