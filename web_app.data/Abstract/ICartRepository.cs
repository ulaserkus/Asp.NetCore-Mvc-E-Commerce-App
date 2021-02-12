using web_app.data.Abstract;
using web_app.entity;

namespace web_app.data
{
    public interface ICartRepository:IRepository<Cart>
    {
         Cart GetByUserId(string userId);
        void DeleteFromCart(int ıd, int productId);
        void ClearCart(int cardId);
    }
}