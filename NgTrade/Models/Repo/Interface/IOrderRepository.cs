using NgTrade.Models.Data;

namespace NgTrade.Models.Repo.Interface
{
    public interface IOrderRepository
    {
        Order CreateOrder(Order order);
        void UpdateOrder(Order order);
    }
}