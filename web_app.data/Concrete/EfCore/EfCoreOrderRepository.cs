using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using web_app.data.Abstract;
using web_app.entity;

namespace web_app.data.Concrete.EfCore
{
    public class EfCoreOrderRepository : EfCoreGenericRepository<Order, ShopContext>, IOrderRepository
    {
        public List<Order> GetOrders(string userId)
        {
            using (var context = new ShopContext())
            {

                var orders = context.Order.Include(i => i.OrderItems).ThenInclude(i => i.Product).AsQueryable();
                if (!string.IsNullOrEmpty(userId))
                {
                    orders = orders.Where(i => i.UserId == userId);
                }

                return orders.ToList();

            }
        }
    }
}