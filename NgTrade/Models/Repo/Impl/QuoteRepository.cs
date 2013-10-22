using System;
using System.Collections.Generic;
using System.Linq;
using NgTrade.Models.Data;
using NgTrade.Models.Info;
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
                    var quotes = db.Quotes.Where(q => q.Symbol.ToLower().Trim() == symbol.ToLower().Trim());
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
                    var quotes = db.Quotes.ToList();
                     var   qu = quotes.Where(q => q.Symbol.ToLower().Trim() == symbol.ToLower().Trim());
                    return quotes.ToList();
                }
            }
            catch (Exception exception)
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

        public Companyprofile GetCompany(string symbol)
        {
            try
            {
                using (var db = new UsersContext())
                {
                    var quotes = db.Companyprofiles.Where(q => q.Symbol.ToLower().Trim() == symbol.ToLower().Trim());
                    return quotes.FirstOrDefault();
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<Companyprofile> GetCompanies()
        {
            try
            {
                using (var db = new UsersContext())
                {
                    var quotes = db.Companyprofiles;
                    return quotes.ToList();
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<QuoteSector> GetDaysListWithSector()
        {
            try
            {
                using (var db = new UsersContext())
                {
                    var dateTimeQuote = db.Quotes.OrderByDescending(q => q.Date).FirstOrDefault();

                    var items = (from e in db.Quotes.Where(q => q.Date == dateTimeQuote.Date).ToList()
                                 join a in db.Companyprofiles
                                     on e.Symbol equals a.Symbol into result
                                 from a in result.DefaultIfEmpty()
                                 select new {e, a}).Select(quote => new QuoteSector()
                                                                        {
                                                                            Category =
                                                                                (quote.a != null)
                                                                                    ? quote.a.Category
                                                                                    : "",
                                                                            Change1 = quote.e.Change1,
                                                                            Close = quote.e.Close,
                                                                            Date = quote.e.Date,
                                                                            High = quote.e.High,
                                                                            Low = quote.e.Low,
                                                                            Open = quote.e.Open,
                                                                            QuoteId = quote.e.QuoteId,
                                                                            Symbol = quote.e.Symbol,
                                                                            Trades = quote.e.Trades,
                                                                            Volume = quote.e.Volume
                                                                        }).ToList();
                    return items;
                }
            }
            catch (Exception ex)
            {
            }
            return null;
        }
    }
}