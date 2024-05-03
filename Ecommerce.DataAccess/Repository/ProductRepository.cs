using Ecommerce.DataAccess.Data;
using Ecommerce.DataAccess.Repository.IRepository;
using Ecommerce.Models;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.DataAccess.Repository
{
    public class ProductRepository: Repository<Product>, IProductRepository
    {

        private ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public IEnumerable<Product> FilterByCategory(string category)
        {
            return _db.Products.Include(p => p.Category)
            .Where(p => p.Category.Name == category)
            .ToList();

        }

        public IEnumerable<Product> FilterByCategoryAndPrice(string category,int minPrice,int maxPrice)
        {
            return _db.Products.Include(p => p.Category)
            .Where(p => p.Category.Name == category && p.Price>= minPrice && p.Price <=maxPrice )
            .ToList();

        }

        IEnumerable<Product> IProductRepository.FilterProductByPrice(int minPrice, int maxPrice)
        {
            return _db.Products.Include(p => p.Category)
           .Where(p => p.Price >= minPrice && p.Price <= maxPrice)
           .ToList();

        }

        public void Update(Product obj)
        {
            var objFromDb = _db.Products.FirstOrDefault(x => x.Id == obj.Id);
            if (objFromDb != null)
            {
                objFromDb.Name = obj.Name;
                objFromDb.Description = obj.Description;
                objFromDb.Price = obj.Price;
                objFromDb.StockQuantity = obj.StockQuantity;            
                objFromDb.CategoryId = obj.CategoryId;
                objFromDb.CreatedAt=obj.CreatedAt;
                objFromDb.UpdatedAt=DateTime.Now;   
                
                
                if (obj.ImageUrl != null)
                {
                    objFromDb.ImageUrl = obj.ImageUrl;
                }
            }



        }

     
    }
}
