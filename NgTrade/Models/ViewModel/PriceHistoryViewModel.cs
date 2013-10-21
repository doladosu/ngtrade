using System.Collections.Generic;
using NgTrade.Helpers.Paging;
using NgTrade.Models.Data;

namespace NgTrade.Models.ViewModel
{
    public class PriceHistoryViewModel
    {
        public List<Quote> Quotes { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public string StockName { get; set; }
    }
}