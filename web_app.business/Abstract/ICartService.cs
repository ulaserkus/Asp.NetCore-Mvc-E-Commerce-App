using web_app.entity;

namespace web_app.business.Abstract
{
    public interface ICartService
    {
        void InitializeCart(string userId);

        Cart GetCartByUserId(string userId);

        void AddToCart(string userId, int productId, int quantity);
        void DeleteFromCart(string userId, int productId);

        void ClearCart(int cardId);
    }
}