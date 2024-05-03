using Ecommerce.Models;


namespace Ecommerce.DataAccess.Repository.IRepository
{
    public interface IProductRepository :IRepository<Product>
    {
        void Update(Product obj);
        IEnumerable<Product> FilterByCategory(string category);
        IEnumerable<Product> FilterByCategoryAndPrice(string category,int minPrice,int maxPrice);
        IEnumerable<Product> FilterProductByPrice(int minPrice, int maxPrice);
    }
}
