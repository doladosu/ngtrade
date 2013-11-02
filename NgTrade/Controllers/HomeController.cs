using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web.Caching;
using System.Web.Mvc;
using DoddleReport;
using DoddleReport.Web;
using NgTrade.Helpers.Paging;
using NgTrade.Models.Data;
using NgTrade.Models.Repo.Interface;
using NgTrade.Models.ViewModel;

namespace NgTrade.Controllers
{
    public class HomeController : Controller
    {
        private readonly IQuoteRepository _quoteRepository;
        private const int PageSize = 10;

        public HomeController(IQuoteRepository quoteRepository)
        {
            _quoteRepository = quoteRepository;
        }

        [OutputCache(CacheProfile = "StaticPageCache")]
        public ActionResult Index()
        {
            List<Quote> dayLosersList;
            var dayLosersListCacheModel = HttpContext.Cache.Get("dayLosersListCache") as IEnumerable<Quote>;
            if (dayLosersListCacheModel != null)
            {
                dayLosersList = dayLosersListCacheModel.ToList();
            }
            else
            {
                dayLosersList = _quoteRepository.GetTopFiveMarketLosersToday();
                var expireMins = Int32.Parse(ConfigurationManager.AppSettings["CacheExpireMins"]);
                HttpContext.Cache.Add("dayLosersListCache", dayLosersList, null,
                                      DateTime.Now.AddMinutes(expireMins), Cache.NoSlidingExpiration,
                                      CacheItemPriority.Normal, null);
            }

            List<Quote> dayGainersList;
            var dayGainersListCacheModel = HttpContext.Cache.Get("dayGainersListCache") as IEnumerable<Quote>;
            if (dayGainersListCacheModel != null)
            {
                dayGainersList = dayGainersListCacheModel.ToList();
            }
            else
            {
                dayGainersList = _quoteRepository.GetTopFiveMarketGainersToday();
                var expireMins = Int32.Parse(ConfigurationManager.AppSettings["CacheExpireMins"]);
                HttpContext.Cache.Add("dayGainersListCache", dayGainersList, null,
                                      DateTime.Now.AddMinutes(expireMins), Cache.NoSlidingExpiration,
                                      CacheItemPriority.Normal, null);
            }

            DateTime stockDay;
            var stockDayCacheModel = HttpContext.Cache.Get("stockDayCache") as DateTime?;
            if (stockDayCacheModel != null)
            {
                stockDay = stockDayCacheModel.GetValueOrDefault();
            }
            else
            {
                stockDay = _quoteRepository.GetCurrentStockDay();
                var expireMins = Int32.Parse(ConfigurationManager.AppSettings["CacheExpireMins"]);
                HttpContext.Cache.Add("stockDayCache", stockDay, null,
                                      DateTime.Now.AddMinutes(expireMins), Cache.NoSlidingExpiration,
                                      CacheItemPriority.Normal, null);
            }

            var dayLosers = dayLosersList;
            var dayGainers = dayGainersList;
            var quoteDay = stockDay;
            var sQuoteDay = String.Format("{0:ddd, MMM d, yyyy}", quoteDay);
            var homeViewModel = new HomeViewModel
            {
                DayGainers = dayGainers,
                DayLosers = dayLosers,
                SQuoteDay = sQuoteDay
            };
            return View(homeViewModel);
        }

        [OutputCache(CacheProfile = "StaticPageCache")]
        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        [OutputCache(CacheProfile = "StaticPageCache")]
        public ActionResult Faq()
        {
            return View();
        }

        [OutputCache(CacheProfile = "StaticPageCache")]
        public ActionResult Terms()
        {
            return View();
        }

        [OutputCache(CacheProfile = "StaticPageCache")]
        public ActionResult Privacy()
        {
            return View();
        }

        [OutputCache(CacheProfile = "StaticPageCache")]
        public ActionResult Research(string stockTicker)
        {
            if (!string.IsNullOrEmpty(stockTicker))
            {
                try
                {
                    var companyProfile = _quoteRepository.GetCompany(stockTicker);
                    var stockData = _quoteRepository.GetQuote(stockTicker);
                    var stockHist = _quoteRepository.GetQuoteList(stockTicker);

                    if (companyProfile == null || stockData == null || stockHist == null)
                    {
                        return RedirectToAction("PriceHistory", new {stockName = stockTicker});
                    }
                    var stockHistory = stockHist.Select(quote => new QuoteModel
                    {
                        Date = quote.Date.ToString("yyyy-MM-dd"),
                        Low = String.Format("{0:0.00}", quote.Low),
                        Open = String.Format("{0:0.00}", quote.Open),
                        Volume = quote.Volume,
                        Close = String.Format("{0:0.00}", quote.Close),
                        High = String.Format("{0:0.00}", quote.High)
                    }).ToList();
                    var filename = stockData.Symbol.ToLower();
                    var stockDataPath = Server.MapPath(string.Format("~/data{0}.csv", filename));
                    var csv = ExportCsv(stockHistory);
                    var outputFile = new StreamWriter(stockDataPath);
                    outputFile.Write(csv);
                    outputFile.Close();

                    var researchViewModel = new ResearchViewModel
                    {
                        CompanyProfile = companyProfile,
                        StockHistory = stockHistory,
                        StockQuote = stockData
                    };
                    return View(researchViewModel);
                }
                catch (Exception ex)
                {
                    throw ex;
                    Console.Write(ex);
                }
            }
            return RedirectToAction("Index", "Daily");
        }

        [OutputCache(CacheProfile = "StaticPageCache")]
        public ReportResult DailyPriceList()
        {
            List<Quote> dayList;
            var dayListCacheModel = HttpContext.Cache.Get("dayListCache") as IEnumerable<Quote>;
            if (dayListCacheModel != null)
            {
                dayList = dayListCacheModel.ToList();
            }
            else
            {
                dayList = _quoteRepository.GetDayList();
                var expireMins = Int32.Parse(ConfigurationManager.AppSettings["CacheExpireMins"]);
                HttpContext.Cache.Add("dayListCache", dayList, null,
                                      DateTime.Now.AddMinutes(expireMins), Cache.NoSlidingExpiration,
                                      CacheItemPriority.Normal, null);
            }

            DateTime stockDay;
            var stockDayCacheModel = HttpContext.Cache.Get("stockDayCache") as DateTime?;
            if (stockDayCacheModel != null)
            {
                stockDay = stockDayCacheModel.GetValueOrDefault();
            }
            else
            {
                stockDay = _quoteRepository.GetCurrentStockDay();
                var expireMins = Int32.Parse(ConfigurationManager.AppSettings["CacheExpireMins"]);
                HttpContext.Cache.Add("stockDayCache", stockDay, null,
                                      DateTime.Now.AddMinutes(expireMins), Cache.NoSlidingExpiration,
                                      CacheItemPriority.Normal, null);
            }
            // Get the data for the report (any IEnumerable will work)
            var query = dayList;
            var dayQuote = stockDay;

            // Create the report and turn our query into a ReportSource
            var report = new Report(query.ToReportSource());


            // Customize the Text Fields
            report.TextFields.Title =
                "Nigerian Stock Exchange Trading Online NSE Daily Price List - NgTradeOnline NSE Daily Price List";
            report.TextFields.SubTitle = "This is the daily price list for " +
                                         string.Format("{0:ddd, MMM d, yyyy}", dayQuote);
            report.TextFields.Footer = "Copyright &copy; " + DateTime.Now.Year + " NgTradeOnline";

            // Render hints allow you to pass additional hints to the reports as they are being rendered
            report.RenderHints.BooleanCheckboxes = true;

            // Customize the data fields
            //report.DataFields["ChangeTracker"].Hidden = true;
            report.DataFields["QuoteId"].Hidden = true;
            report.DataFields["Date"].Hidden = true;
            report.DataFields["Close"].DataFormatString = "{0:N}";
            report.DataFields["Open"].DataFormatString = "{0:N}";
            report.DataFields["Open"].HeaderText = "OPEN";
            report.DataFields["Low"].DataFormatString = "{0:N}";
            report.DataFields["High"].DataFormatString = "{0:N}";
            report.DataFields["Change1"].DataFormatString = "{0:N}";
            report.DataFields["Change1"].HeaderText = "CHANGE";

            // Return the ReportResult
            // the type of report that is rendered will be determined by the extension in the URL (.pdf, .xls, .html, etc)
            return new ReportResult(report);
        }

        [OutputCache(CacheProfile = "StaticPageCache")]
        public ActionResult PriceHistory(string stockName, int? page)
        {
            int pageNumber = (page ?? 1);
            var query = _quoteRepository.GetQuoteList(stockName).OrderByDescending(q => q.Date).ToList();
          
            var pagingInfo = new PagingInfo
            {
                CurrentPage = pageNumber,
                ItemsPerPage = PageSize,
                TotalItems = query.Count
            };

            var priceHistoryViewModel = new PriceHistoryViewModel { PagingInfo = pagingInfo, Quotes = query.Skip(PageSize * (pageNumber - 1)).Take(PageSize).ToList(), StockName = stockName};
            return View(priceHistoryViewModel);
        }

        public static string ExportCsv<T>(List<T> list)
        {
            return Helpers.FileExtension.GetCsv(list);
        }

        public ActionResult ClearCache()
        {
            Response.RemoveOutputCacheItem("/");

            HttpContext.Cache.Remove("dayListCache");
            HttpContext.Cache.Remove("dayGainersCache");
            HttpContext.Cache.Remove("dayLosersCache");
            HttpContext.Cache.Remove("dayLosersListCache");
            HttpContext.Cache.Remove("dayGainersListCache");
            HttpContext.Cache.Remove("stockDayCache");

            return RedirectToAction("Index", "Home");
        }

        [OutputCache(CacheProfile = "StaticPageCache")]
        public ActionResult Leadership()
        {
            return View();
        }

        [OutputCache(CacheProfile = "StaticPageCache")]
        public ActionResult News()
        {
            return View();
        }

        [OutputCache(CacheProfile = "StaticPageCache")]
        public ActionResult Events()
        {
            return View();
        }

        [OutputCache(CacheProfile = "StaticPageCache")]
        public ActionResult Blog()
        {
            return View();
        }

        [OutputCache(CacheProfile = "StaticPageCache")]
        public ActionResult GetSymbols(string term)
        {
            var symbols = _quoteRepository.GetSymbols(term);
            return Json( symbols, JsonRequestBehavior.AllowGet);
        }

        [OutputCache(CacheProfile = "StaticPageCache")]
        public ActionResult Companies(int? page)
        {
            var pageNumber = (page ?? 1);
            List<Companyprofile> companyprofiles;
            var companyCacheModel = HttpContext.Cache.Get("companiesCache") as IEnumerable<Companyprofile>;
            if (companyCacheModel != null)
            {
                companyprofiles = companyCacheModel.ToList();
            }
            else
            {
                companyprofiles = _quoteRepository.GetCompanies().ToList();
                var expireMins = Int32.Parse(ConfigurationManager.AppSettings["CacheExpireMins"]);
                HttpContext.Cache.Add("companiesCache", companyprofiles, null,
                                      DateTime.Now.AddMinutes(expireMins), Cache.NoSlidingExpiration,
                                      CacheItemPriority.Normal, null);
            }
            var pagingInfo = new PagingInfo
            {
                CurrentPage = pageNumber,
                ItemsPerPage = PageSize,
                TotalItems = companyprofiles.Count
            };

            var companyViewModel = new CompanyViewModel { PagingInfo = pagingInfo, Companyprofiles = companyprofiles.Skip(PageSize * (pageNumber - 1)).Take(PageSize).ToList() };
            return View(companyViewModel);
        }

    }
}
