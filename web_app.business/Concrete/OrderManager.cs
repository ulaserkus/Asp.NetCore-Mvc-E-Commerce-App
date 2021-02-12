using System.Collections.Generic;
using web_app.business.Abstract;
using web_app.data.Abstract;
using web_app.entity;

namespace web_app.business.Concrete
{
    public class OrderManager : IOrderService
    {   
        private IOrderRepository _orderRepository;
        public OrderManager(IOrderRepository orderRepository)
        {
            _orderRepository=orderRepository;
            
        }
        public void Create(Order entity)
        {
          _orderRepository.Create(entity);
        }

        public List<Order> GetOrders(string userId)
        {
            return _orderRepository.GetOrders(userId);
        }
    }
}