using System;
using System.Collections.Generic;
using NgTrade.Models.Data;

namespace NgTrade.Models.Repo.Interface
{
    public interface IQuoteRepository
    {
        List<Quote> GetTopFiveMarketLosersToday();
        List<Quote> GetTopFiveMarketGainersToday();
        Quote GetQuote(string symbol);
        List<Quote> GetQuoteList(string symbol);
        DateTime GetCurrentStockDay();
        List<Quote> GetDayList();
    }
}