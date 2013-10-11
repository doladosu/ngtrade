using System;
using System.Collections.Generic;
using System.Linq;
using NgTrade.Models.Data;
using NgTrade.Models.Repo.Interface;

namespace NgTrade.Models.Repo.Impl
{
    public class QuoteRepository : IQuoteRepository
    {
        public List<Quote> GetTopFiveMarketLosersToday()
        {
            try
            {
                using (var db = new UsersContext())
                {
                    var dateTimeQuote = db.Quotes.OrderByDescending(q => q.Date).FirstOrDefault();
                    var quotes = db.Quotes.Where(q => q.Close < q.Open && q.Date == dateTimeQuote.Date).OrderBy(q => q.Change1);
                    return quotes.Take(5).ToList();
                }
            }
            catch (Exception)
            {
                return null;
            }

        }

        public List<Quote> GetTopFiveMarketGainersToday()
        {
            try
            {
                using (var db = new UsersContext())
                {
                    var dateTimeQuote = db.Quotes.OrderByDescending(q => q.Date).FirstOrDefault();
                    var quotes = db.Quotes.Where(q => q.Close > q.Open && q.Date == dateTimeQuote.Date).OrderByDescending(q => q.Change1);
                    return quotes.Take(5).ToList();
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Quote GetQuote(string symbol)
        {
            try
            {
                using (var db = new UsersContext())
                {
                    var quotes = db.Quotes.Where(q => q.Symbol == symbol);
                    var dateTime = (quotes.Select(q => q.Date)).Max();
                    return db.Quotes.FirstOrDefault(q => q.Symbol == symbol && q.Date == dateTime);
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<Quote> GetQuoteList(string symbol)
        {
            try
            {
                using (var db = new UsersContext())
                {
                    var quotes = db.Quotes.Where(q => q.Symbol == symbol);
                    return quotes.ToList();
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public DateTime GetCurrentStockDay()
        {
            try
            {
                using (var db = new UsersContext())
                {
                    var dateTimeQuote = db.Quotes.OrderByDescending(q => q.Date).FirstOrDefault();
                    if (dateTimeQuote != null)
                        return dateTimeQuote.Date;
                }
            }
            catch (Exception)
            {
            }
            return DateTime.Now;
        }

        public List<Quote> GetDayList()
        {
            try
            {
                using (var db = new UsersContext())
                {
                    var dateTimeQuote = db.Quotes.OrderByDescending(q => q.Date).FirstOrDefault();
                    var quotes = db.Quotes.Where(q => q.Date == dateTimeQuote.Date).OrderBy(q => q.Symbol);
                    return quotes.ToList();
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}