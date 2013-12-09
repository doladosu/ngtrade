using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Caching;
using System.Web.Mvc;
using NgTrade.Helpers.Paging;
using NgTrade.Models.Data;
using NgTrade.Models.Info;
using NgTrade.Models.Repo.Interface;
using NgTrade.Models.ViewModel;

namespace NgTrade.Controllers
{
    public class DailyController : BaseController
    {
        private const int PageSize = 10;

        public DailyController(IAccountRepository accountRepository, IQuoteRepository quoteRepository) : base(accountRepository, quoteRepository, null, null, null)
        {
        }

        [OutputCache(CacheProfile = "StaticPageCache")]
        public ActionResult Index(int? page, string dateFilter, string sector)
        {
            int pageNumber = (page ?? 1);
            var dailyList = new List<Quote>();
            var companiesSector = GetCompaniesSectors();
            if (string.IsNullOrWhiteSpace(dateFilter) && string.IsNullOrWhiteSpace(sector))
            {
                var dailyListCacheModel = HttpContext.Cache.Get("dailyListIndexDCache") as IEnumerable<Quote>;
                if (dailyListCacheModel != null)
                {
                    dailyList = dailyListCacheModel.ToList();
                }
                else
                {
                    dailyList = QuoteRepository.GetDayList().ToList();
                    var expireMins = Int32.Parse(ConfigurationManager.AppSettings["CacheExpireMins"]);
                    HttpContext.Cache.Add("dailyListIndexDCache", dailyList, null,
                                          DateTime.Now.AddMinutes(expireMins), Cache.NoSlidingExpiration,
                                          CacheItemPriority.Normal, null);
                }
            }
            else 
            {
                if (!string.IsNullOrWhiteSpace(sector))
                {
                    var dailyListWithSector = GetDaysListWithSector().ToList();
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
                    var dailyListCacheModel = HttpContext.Cache.Get("dailyListIndexDDCache") as IEnumerable<Quote>;
                    if (dailyListCacheModel != null)
                    {
                        dailyList = dailyListCacheModel.ToList();
                    }
                    else
                    {
                        dailyList = QuoteRepository.GetDayList().Where(q => q.Date == DateTime.Parse(dateFilter)).ToList();
                        var expireMins = Int32.Parse(ConfigurationManager.AppSettings["CacheExpireMins"]);
                        HttpContext.Cache.Add("dailyListIndexDDCache", dailyList, null,
                                              DateTime.Now.AddMinutes(expireMins), Cache.NoSlidingExpiration,
                                              CacheItemPriority.Normal, null);
                    }
                }
            }
            var pagingInfo = new PagingInfo
                                        {
                                            CurrentPage = pageNumber,
                                            ItemsPerPage = PageSize,
                                            TotalItems = dailyList.Count
                                        };

            var dailyViewModel = new DailyViewModel { SectorFilter = sector, PagingInfo = pagingInfo, Quotes = dailyList.Skip(PageSize * (pageNumber-1)).Take(PageSize).ToList(), Sectors = companiesSector};
            return View(dailyViewModel);
        }

        [OutputCache(CacheProfile = "StaticPageCache")]
        public ActionResult Gainers(int? page)
        {
            int pageNumber = (page ?? 1);
            List<Quote> dailyList;
            var dailyListCacheModel = HttpContext.Cache.Get("dailyListGainersDCache") as IEnumerable<Quote>;
            if (dailyListCacheModel != null)
            {
                dailyList = dailyListCacheModel.ToList();
            }
            else
            {
                dailyList =  QuoteRepository.GetDayList().Where(q => q.Close > q.Open).OrderByDescending(q => q.Change1).ToList();
                var expireMins = Int32.Parse(ConfigurationManager.AppSettings["CacheExpireMins"]);
                HttpContext.Cache.Add("dailyListGainersDCache", dailyList, null,
                                      DateTime.Now.AddMinutes(expireMins), Cache.NoSlidingExpiration,
                                      CacheItemPriority.Normal, null);
            }
            var pagingInfo = new PagingInfo
            {
                CurrentPage = pageNumber,
                ItemsPerPage = PageSize,
                TotalItems = dailyList.Count
            };

            var dailyViewModel = new DailyViewModel { PagingInfo = pagingInfo, Quotes = dailyList.Skip(PageSize * (pageNumber - 1)).Take(PageSize).ToList() };
            return View(dailyViewModel);
        }

        [OutputCache(CacheProfile = "StaticPageCache")]
        public ActionResult Losers(int? page)
        {
            int pageNumber = (page ?? 1);
            List<Quote> dailyList;
            var dailyListCacheModel = HttpContext.Cache.Get("dailyListLosersDCache") as IEnumerable<Quote>;
            if (dailyListCacheModel != null)
            {
                dailyList = dailyListCacheModel.ToList();
            }
            else
            {
                dailyList = QuoteRepository.GetDayList().Where(q => q.Close < q.Open).OrderBy(q => q.Change1).ToList();
                var expireMins = Int32.Parse(ConfigurationManager.AppSettings["CacheExpireMins"]);
                HttpContext.Cache.Add("dailyListLosersDCache", dailyList, null,
                                      DateTime.Now.AddMinutes(expireMins), Cache.NoSlidingExpiration,
                                      CacheItemPriority.Normal, null);
            }
            var pagingInfo = new PagingInfo
            {
                CurrentPage = pageNumber,
                ItemsPerPage = PageSize,
                TotalItems = dailyList.Count
            };

            var dailyViewModel = new DailyViewModel { PagingInfo = pagingInfo, Quotes = dailyList.Skip(PageSize * (pageNumber - 1)).Take(PageSize).ToList() };
            return View(dailyViewModel);
        }

        private List<string> GetCompaniesSectors()
        {
            List<string> companySectors;
            var companyprofilesCacheModel = HttpContext.Cache.Get("companyprofilesDCache") as IEnumerable<string>;
            if (companyprofilesCacheModel != null)
            {
                companySectors = companyprofilesCacheModel.ToList();
            }
            else
            {
                companySectors = QuoteRepository.GetCompanies().OrderBy(q => q.Category).Select(q => q.Category).Distinct().ToList();
                var expireMins = Int32.Parse(ConfigurationManager.AppSettings["CacheExpireMins"]);
                HttpContext.Cache.Add("companyprofilesDCache", companySectors, null,
                                      DateTime.Now.AddMinutes(expireMins), Cache.NoSlidingExpiration,
                                      CacheItemPriority.Normal, null);
            }

            return companySectors;
        }

        private IEnumerable<QuoteSector> GetDaysListWithSector()
        {
            List<QuoteSector> daysListWithSector;
            var daysListWithSectorCacheModel = HttpContext.Cache.Get("daysListWithSectorDCache") as IEnumerable<QuoteSector>;
            if (daysListWithSectorCacheModel != null)
            {
                daysListWithSector = daysListWithSectorCacheModel.ToList();
            }
            else
            {
                daysListWithSector = QuoteRepository.GetDaysListWithSector();
                var expireMins = Int32.Parse(ConfigurationManager.AppSettings["CacheExpireMins"]);
                HttpContext.Cache.Add("daysListWithSectorDCache", daysListWithSector, null,
                                      DateTime.Now.AddMinutes(expireMins), Cache.NoSlidingExpiration,
                                      CacheItemPriority.Normal, null);
            }

            return daysListWithSector;
        }

        public ActionResult ClearCache()
        {
            Response.RemoveOutputCacheItem("/");

            HttpContext.Cache.Remove("dailyListIndexDCache");
            HttpContext.Cache.Remove("dailyListIndexDDCache");
            HttpContext.Cache.Remove("dailyListGainersACache");
            HttpContext.Cache.Remove("dailyListLosersDCache");

            return RedirectToAction("Index", "Home");
        }

    }
}
