using NgTrade.Models.Data;

namespace NgTrade.Models.Info
{
    public class OrderModel
    {
        public string Symbol { get; set; }
        public int Shares { get; set; }
        public string Action { get; set; }
        public double Price { get; set; }
        public UserProfile UserProfile { get; set; }
    }
}