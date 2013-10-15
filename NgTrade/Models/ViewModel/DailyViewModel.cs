using System.Collections.Generic;
using NgTrade.Helpers.Paging;
using NgTrade.Models.Data;

namespace NgTrade.Models.ViewModel
{
    public class DailyViewModel
    {
        public List<Quote> Quotes { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public List<string> Sectors { get; set; }
        public string SectorFilter { get; set; }
    }
}