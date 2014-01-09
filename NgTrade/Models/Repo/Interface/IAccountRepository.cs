using System.Collections.Generic;
using NgTrade.Models.Data;

namespace NgTrade.Models.Repo.Interface
{
    public interface IAccountRepository
    {
        UserProfile GetAccountProfile(int id);
        void AddToMailingList(MailingList mailingList);
        MailingList GetMailingList(string email);
        List<UserProfile> GetAllAccountProfiles();
        List<MailingList> GetMailingLists();
    }
}