using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using NgTrade.Models.Data;
using NgTrade.Models.Repo.Interface;

namespace NgTrade.Models.Repo.Impl
{
    public class HoldingRepository : IHoldingRepository
    {
        private const string ALL_HOLDINGS_CACHE_KEY = "AllHoldingsCache";
        private static readonly object CacheLockObjectCurrentSales = new object();


        public List<Holding> GetHoldings(int accountId)
        {
            var allHoldings = GetAllHoldings();
            return allHoldings.Where(q => q.AccountId == accountId).OrderByDescending(q => q.DatePurchased).ToList();
        }

        public List<Holding> GetAllHoldings()
        {
            try
            {
                var cache = MemoryCache.Default;
                var result = cache[ALL_HOLDINGS_CACHE_KEY] as List<Holding>;

                if (result == null)
                {
                    lock (CacheLockObjectCurrentSales)
                    {
                        result = cache[ALL_HOLDINGS_CACHE_KEY] as List<Holding>;

                        var policy = new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(15) };

                        if (result == null)
                        {
                            using (var db = new UsersContext())
                            {
                                result = db.Holdings.ToList();
                            }
                            cache.Add(ALL_HOLDINGS_CACHE_KEY, result, policy);
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

    }
}