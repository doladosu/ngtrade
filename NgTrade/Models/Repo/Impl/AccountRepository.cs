using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using NgTrade.Models.Data;
using NgTrade.Models.Repo.Interface;

namespace NgTrade.Models.Repo.Impl
{
    public class AccountRepository : IAccountRepository
    {
        private const string ALL_ACCOUNTPROFILES_CACHE_KEY = "AllAccountProfilesCache";
        private static readonly object CacheLockObjectCurrentSales = new object();

        public UserProfile GetAccountProfile(int id)
        {
            var allAccountProfiles = GetAllAccountProfiles();
            return allAccountProfiles.FirstOrDefault(q => q.UserId == id);
        }

        public List<UserProfile> GetAllAccountProfiles()
        {
            try
            {
                var cache = MemoryCache.Default;
                var result = cache[ALL_ACCOUNTPROFILES_CACHE_KEY] as List<UserProfile>;

                if (result == null)
                {
                    lock (CacheLockObjectCurrentSales)
                    {
                        result = cache[ALL_ACCOUNTPROFILES_CACHE_KEY] as List<UserProfile>;

                        var policy = new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(15) };

                        if (result == null)
                        {
                            using (var db = new UsersContext())
                            {
                                result = db.UserProfiles.ToList();
                            }
                            cache.Add(ALL_ACCOUNTPROFILES_CACHE_KEY, result, policy);
                        }
                    }
                }
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public void AddToMailingList(MailingList mailingList)
        {
            try
            {
                using (var db = new UsersContext())
                {
                    db.MailingLists.Add(mailingList);
                    db.SaveChanges();
                }
            }
            catch (Exception)
            {
            }
        }

        public MailingList GetMailingList(string email)
        {
            try
            {
                using (var db = new UsersContext())
                {
                    var mailingList = db.MailingLists.FirstOrDefault(m => m.Email.ToLower() == email.ToLower());
                    return mailingList;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<MailingList> GetMailingLists()
        {
            try
            {
                using (var db = new UsersContext())
                {
                    var mailingList = db.MailingLists.ToList();
                    return mailingList;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}