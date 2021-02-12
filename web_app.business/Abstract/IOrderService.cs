using System.Collections.Generic;
using web_app.entity;

namespace web_app.business.Abstract
{
    public interface IOrderService
    {
         void Create(Order entity) ;

         List<Order> GetOrders(string userId);
    }
}