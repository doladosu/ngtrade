using System;
using NgTrade.Models.Data;

namespace NgTrade.Models.Repo.Interface
{
    public interface IAccountRepository
    {
        UserProfile GetAccountProfile(int id);
    }
}