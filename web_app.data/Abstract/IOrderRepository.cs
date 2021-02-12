using System.Collections.Generic;
using web_app.entity;

namespace web_app.data.Abstract
{
    public interface IOrderRepository : IRepository<Order>
    {
        List<Order> GetOrders(string userId);
    }
}