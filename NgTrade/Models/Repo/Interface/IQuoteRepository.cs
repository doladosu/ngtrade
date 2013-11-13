using System;
using System.Collections.Generic;
using NgTrade.Models.Data;
using NgTrade.Models.Info;

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
        Companyprofile GetCompany(string symbol);
        List<Companyprofile> GetCompanies();
        List<QuoteSector> GetDaysListWithSector();
        List<string> GetSymbols(string symbol);
    }
}