using System.Collections.Generic;
using NgTrade.Models.Data;

namespace NgTrade.Models.ViewModel
{
    public class ResearchViewModel
    {
        public Companyprofile CompanyProfile { get; set; }
        public IEnumerable<QuoteModel> StockHistory { get; set; }
        public Quote StockQuote { get; set; }
    }
}