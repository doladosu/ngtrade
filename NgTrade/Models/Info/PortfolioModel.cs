using System;

namespace NgTrade.Models.Info
{
    public class PortfolioModel
    {
        public decimal? Purchaseprice { get; set; }
        public int Holdingid { get; set; }
        public int Quantity { get; set; }
        public DateTime? Purchasedate { get; set; }
        public int? AccountAccountid { get; set; }
        public string QuoteSymbol { get; set; }
        public decimal? CurrentPrice { get; set; }
        public decimal? PurchaseBasis { get; set; }
        public decimal? GainLoss { get; set; }
        public decimal? MarketValue { get; set; }
    }
}