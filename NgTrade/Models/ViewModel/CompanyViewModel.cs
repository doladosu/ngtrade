using System.Collections.Generic;
using NgTrade.Helpers.Paging;
using NgTrade.Models.Data;

namespace NgTrade.Models.ViewModel
{
    public class CompanyViewModel
    {
        public List<Companyprofile> Companyprofiles { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}