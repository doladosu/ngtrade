using System.Linq;
using System.Web.Mvc;
using NgTrade.Helpers.Paging;
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

        public ActionResult Index(int? page)
        {
            int pageNumber = (page ?? 1);
            var dailyList = _quoteRepository.GetDayList().ToList();
            var pagingInfo = new PagingInfo
                                        {
                                            CurrentPage = pageNumber,
                                            ItemsPerPage = PAGE_SIZE,
                                            TotalItems = dailyList.Count
                                        };

            var dailyViewModel = new DailyViewModel { PagingInfo = pagingInfo, Quotes = dailyList.Skip(PAGE_SIZE * pageNumber-1).Take(PAGE_SIZE).ToList() };
            return View(dailyViewModel);
        }

        public ActionResult Gainers()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Losers()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
