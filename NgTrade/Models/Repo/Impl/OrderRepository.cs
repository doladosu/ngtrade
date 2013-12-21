using System;
using System.Data;
using NgTrade.Models.Data;
using NgTrade.Models.Repo.Interface;

namespace NgTrade.Models.Repo.Impl
{
    public class OrderRepository : IOrderRepository
    {
        public Order CreateOrder(Order order)
        {
            try
            {
                using (var db = new UsersContext())
                {
                    db.Orders.Add(order);
                    db.SaveChanges();
                }
                return order;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public void UpdateOrder(Order order)
        {
            using (var db = new UsersContext())
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
            }
        }
    }
}