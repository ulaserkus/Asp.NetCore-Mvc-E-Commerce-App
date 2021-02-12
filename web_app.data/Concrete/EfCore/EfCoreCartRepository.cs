using System.Linq;
using Microsoft.EntityFrameworkCore;
using web_app.data.Concrete.EfCore;
using web_app.entity;

namespace web_app.data.Concrete
{
    public class EfCoreCartRepository : EfCoreGenericRepository<Cart, ShopContext>, ICartRepository
    {
        public void ClearCart(int cardId)
        {
             using (var context = new ShopContext())
            {
                var cmd = @"delete from CartItems where CartId =@p0";
                context.Database.ExecuteSqlRaw(cmd,cardId);
            }
            
        }

        public void DeleteFromCart(int cartId, int productId)
        {
            using (var context = new ShopContext())
            {
                var cmd = @"delete from CartItems where CartId =@p0 and ProductId =@p1 ";
                context.Database.ExecuteSqlRaw(cmd,cartId,productId);
            }
        }

        public Cart GetByUserId(string userId)
        {
           using(var context = new ShopContext()){
               return context.Carts.Include(i=>i.CartItems).ThenInclude(i=>i.Product).FirstOrDefault(i=>i.UserId==userId);
           }
        }

          public override void Update(Cart entity)
        {
            using (var context = new ShopContext())
            {
               context.Carts.Update(entity);
               context.SaveChanges();
            }
            
        }
    }
}