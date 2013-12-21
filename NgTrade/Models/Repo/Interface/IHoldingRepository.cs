using System;
using System.Collections.Generic;
using NgTrade.Models.Data;
using NgTrade.Models.Info;

namespace NgTrade.Models.Repo.Interface
{
    public interface IHoldingRepository
    {
        List<Holding> GetHoldings(int accountId);
        Holding CreateHolding(OrderModel orderModel);
    }
}