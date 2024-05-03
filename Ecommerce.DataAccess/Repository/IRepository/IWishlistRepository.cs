using Ecommerce.Models;

namespace Ecommerce.DataAccess.Repository.IRepository
{
    public interface IWishlistRepository : IRepository<Wishlist>
    {
        void Update(Wishlist obj);
        
    }
}
