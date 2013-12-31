using NgTrade.Models.ViewModel;

namespace NgTrade.Models.Repo.Interface
{
    public interface ISmtpRepository
    {
        void SendContactEmail(ContactViewModel contact);
        void AddEmailToNewsletter(string email);
    }
}