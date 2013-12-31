using System;
using System.Configuration;
using System.Net.Mail;
using NgTrade.Models.Repo.Interface;
using NgTrade.Models.ViewModel;

namespace NgTrade.Models.Repo.Impl
{
    public class SmtpRepository : ISmtpRepository
    {
        private readonly string _host = ConfigurationManager.AppSettings["smtpServer"];
        private readonly int _port = Int32.Parse(ConfigurationManager.AppSettings["smtpPort"]);
        private readonly string _userName = ConfigurationManager.AppSettings["smtpUsername"];
        private readonly string _password = ConfigurationManager.AppSettings["smtpPassword"];
        private readonly string _fromEmail = ConfigurationManager.AppSettings["smtpFromEmail"];

        public void SendContactEmail(ContactViewModel contact)
        {
            using (var client = new SmtpClient(_host, _port))
            {
                client.Credentials = new System.Net.NetworkCredential(_userName, _password);
                client.EnableSsl = true;
                client.Send(_fromEmail, _fromEmail, contact.Name + " with email " + contact.Email + " contacted Ngtradeonline", contact.Message);
            }
        }

        public void AddEmailToNewsletter(string email)
        {
            using (var client = new SmtpClient(_host, _port))
            {
                client.Credentials = new System.Net.NetworkCredential(_userName, _password);
                client.EnableSsl = true;
                client.Send(_fromEmail, _fromEmail, "Add " + email + " to mailing list", "Add " + email + " to mailing list");
            }
        }
    }
}