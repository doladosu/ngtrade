using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NgTrade.Helpers.Paging;
using NgTrade.Models.Data;
using NgTrade.Models.Info;
using NgTrade.Models.Repo.Interface;
using NgTrade.Models.ViewModel;

namespace NgTrade.Controllers
{
    public class DailyController : Controller
    {
        private readonly IQuoteRepository _quoteRepository;
        private const int PAGE_SIZE = 10;

        public DailyController(IQuoteRepository quoteRepository)
        {
            _quoteRepository = quoteRepository;
        }

        public ActionResult Index(int? page, string dateFilter, string sector)
        {
            int pageNumber = (page ?? 1);
            List<Quote> dailyList = new List<Quote>();
            var companiesSector = GetCompaniesSectors();
            if (string.IsNullOrWhiteSpace(dateFilter) && string.IsNullOrWhiteSpace(sector))
            {
                dailyList = _quoteRepository.GetDayList().ToList();
            }
            else 
            {
                if (!string.IsNullOrWhiteSpace(sector))
                {
                    List<QuoteSector> dailyListWithSector = GetDaysListWithSector().ToList();
                    var updatedDailyList = (from quoteSector in dailyListWithSector
                                            where quoteSector.Category != null && quoteSector.Category.ToLower() == sector.ToLower()
                                                    select new Quote
                                                               {
                                                                   Change1 = quoteSector.Change1, Close = quoteSector.Close, Date = quoteSector.Date, High = quoteSector.High, Low = quoteSector.Low, Open = quoteSector.Open, QuoteId = quoteSector.QuoteId, Symbol = quoteSector.Symbol, Trades = quoteSector.Trades, Volume = quoteSector.Volume
                                                               }).ToList();
                    dailyList = updatedDailyList;
                }
                if (!string.IsNullOrWhiteSpace(dateFilter))
                {
                    dailyList = _quoteRepository.GetDayList().Where(q => q.Date == DateTime.Parse(dateFilter)).ToList();
                }
            }
            var pagingInfo = new PagingInfo
                                        {
                                            CurrentPage = pageNumber,
                                            ItemsPerPage = PAGE_SIZE,
                                            TotalItems = dailyList.Count
                                        };

            var dailyViewModel = new DailyViewModel { SectorFilter = sector, PagingInfo = pagingInfo, Quotes = dailyList.Skip(PAGE_SIZE * (pageNumber-1)).Take(PAGE_SIZE).ToList(), Sectors = companiesSector};
            return View(dailyViewModel);
        }

        private List<string> GetCompaniesSectors()
        {
            var categories = _quoteRepository.GetCompanies().OrderBy(q => q.Category).Select(q => q.Category).Distinct();
            return categories.ToList();
        }

        private List<QuoteSector> GetDaysListWithSector()
        {
           var dailyListWithSector = _quoteRepository.GetDaysListWithSector();
            return dailyListWithSector;
        }
        public ActionResult Gainers(int? page)
        {
            int pageNumber = (page ?? 1);
            var dailyList = _quoteRepository.GetDayList().Where(q => q.Close >= q.Open).OrderByDescending(q => q.Change1).ToList();
            var pagingInfo = new PagingInfo
            {
                CurrentPage = pageNumber,
                ItemsPerPage = PAGE_SIZE,
                TotalItems = dailyList.Count
            };

            var dailyViewModel = new DailyViewModel { PagingInfo = pagingInfo, Quotes = dailyList.Skip(PAGE_SIZE * pageNumber - 1).Take(PAGE_SIZE).ToList() };
            return View(dailyViewModel);
        }

        public ActionResult Losers(int? page)
        {
            int pageNumber = (page ?? 1);
            var dailyList = _quoteRepository.GetDayList().Where(q => q.Close < q.Open).OrderBy(q => q.Change1).ToList();
            var pagingInfo = new PagingInfo
            {
                CurrentPage = pageNumber,
                ItemsPerPage = PAGE_SIZE,
                TotalItems = dailyList.Count
            };

            var dailyViewModel = new DailyViewModel { PagingInfo = pagingInfo, Quotes = dailyList.Skip(PAGE_SIZE * pageNumber - 1).Take(PAGE_SIZE).ToList() };
            return View(dailyViewModel);
        }
    }
}
