using Ecommerce.DataAccess.Repository.IRepository;
using Ecommerce.Models;
using Ecommerce.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EcommerceMVC.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class WishlistController : Controller
    {
        
        private readonly IUnitOfWork _unitOfWork;

        public WishlistController(IUnitOfWork unitOfWork)
        {
            
            _unitOfWork = unitOfWork;

        }
        [Authorize]
        public IActionResult Index()
        {

            var claimIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            IEnumerable<Wishlist> wishlist = _unitOfWork.Wishlist.GetAll(u => u.ApplicationUserId == userId, includeProperty: "Product");

            



            return View(wishlist);
        }

        
        public IActionResult Remove(int productId)
        {
            var productToRemove = _unitOfWork.Wishlist.Get(u => u.Product.Id == productId);
            _unitOfWork.Wishlist.Remove(productToRemove);
            _unitOfWork.Save();

            return RedirectToAction("Index");
        }
    }
}
