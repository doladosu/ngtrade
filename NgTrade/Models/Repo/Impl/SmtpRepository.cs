using System.Net.Mail;
using NgTrade.Models.Repo.Interface;
using NgTrade.Models.ViewModel;

namespace NgTrade.Models.Repo.Impl
{
    public class SmtpRepository : ISmtpRepository
    {
        public void SendContactEmail(ContactViewModel contact)
        {
            var mail = new MailMessage(
                            contact.Email,
                            "support@ngtradeonline.com",
                            contact.Name,
                            contact.Message);

            new SmtpClient().Send(mail);
        }
    }
}