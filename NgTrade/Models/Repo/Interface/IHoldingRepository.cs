using System;
using System.Collections.Generic;
using NgTrade.Models.Data;

namespace NgTrade.Models.Repo.Interface
{
    public interface IHoldingRepository
    {
        List<Holding> GetHoldings(int accountId);
    }
}