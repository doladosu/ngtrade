using System.Collections.Generic;
using NgTrade.Models.ViewModel;

namespace NgTrade.Models.Repo.Interface
{
    public interface ISmtpRepository
    {
        void SendContactEmail(ContactViewModel contact);
        void SendReferralEmail(ReferViewModel referViewModel);
        void SendForgotPasswordEmail(string email, string body);
        void SendDailyEmail(List<string> emails);
    }
}