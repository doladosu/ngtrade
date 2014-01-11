using System.Collections.Generic;
using NgTrade.Helpers.Paging;
using NgTrade.Models.Info;

namespace NgTrade.Models.ViewModel
{
    public class PortfolioViewModel
    {
        public List<PortfolioModel> PortfolioVm { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public int Ot { get; set; }
    }
}