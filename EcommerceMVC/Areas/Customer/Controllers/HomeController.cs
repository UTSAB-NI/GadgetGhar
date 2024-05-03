using Ecommerce.DataAccess.Repository;
using Ecommerce.DataAccess.Repository.IRepository;
using Ecommerce.Models;
using Ecommerce.Models.ViewModels;
using EcommerceMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace EcommerceMVC.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        [BindProperty]
        public ShopVM shopVM { get; set; }

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;

        }
        
        public IActionResult Index()
        {
            var laptopCategory = _unitOfWork.Product.FilterByCategory("Laptop");
            var mobileCategory = _unitOfWork.Product.FilterByCategory("Mobile");
            var earbudCategory = _unitOfWork.Product.FilterByCategory("Earbud");
            var smartwatchCategory = _unitOfWork.Product.FilterByCategory("Smartwatch");
            var televisionCategory = _unitOfWork.Product.FilterByCategory("Television");

            ViewBag.laptopCategory = laptopCategory;
            ViewBag.mobileCategory = mobileCategory;
            ViewBag.earbudCategory = earbudCategory;
            ViewBag.smartwatchCategory = smartwatchCategory;
            ViewBag.televisionCategory = televisionCategory;

            IEnumerable<Product> productList = _unitOfWork.Product.GetAll(includeProperty: "Category");
            return View(productList);
           
        }
        //[HttpPost]
        //[ActionName("Index")]
        //public IActionResult Index(Wishlist wishlist)
        //{


        //}


        public IActionResult Details(int? productId)
        {
            ShoppingCart cart = new ShoppingCart()
            {
                Product = _unitOfWork.Product.Get(u => u.Id == productId, includeProperty: "Category"),
                Count = 1,
                ProductId = (int)productId
            };
            
            return View(cart);
        }



        [HttpPost]
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingCart)
        {
            var claimIdentity=(ClaimsIdentity)User.Identity;
            var userId = claimIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            shoppingCart.ApplicationUserId= userId;
            ShoppingCart cartFromDb = _unitOfWork.ShoppingCart.Get(u => u.ProductId == shoppingCart.ProductId && u.ApplicationUserId==userId);

            if(cartFromDb != null)
            {
                cartFromDb.Count += shoppingCart.Count;
                _unitOfWork.ShoppingCart.Update(cartFromDb);
            }
            else
            {
                _unitOfWork.ShoppingCart.Add(shoppingCart);
            }

           
            _unitOfWork.Save();

            return RedirectToAction("Index");
        }

        public IActionResult InsertToCart(int productId)
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            var product = _unitOfWork.Product.Get(u => u.Id == productId);
            ShoppingCart shoppingCart = new ShoppingCart()
            {
                ProductId = product.Id,               
                Count = 1,
                ApplicationUserId = userId,
        };
            
            ShoppingCart cartFromDb = _unitOfWork.ShoppingCart.Get(u => u.ProductId == shoppingCart.ProductId && u.ApplicationUserId == userId);

            if (cartFromDb != null)
            {
                cartFromDb.Count += shoppingCart.Count;
                _unitOfWork.ShoppingCart.Update(cartFromDb);
            }
            else
            {
                _unitOfWork.ShoppingCart.Add(shoppingCart);
            }


            _unitOfWork.Save();

            return RedirectToAction("Index");
        }

        public IActionResult Shop(string? category)
        {
            ShopVM shopVM;
            if (!string.IsNullOrEmpty(category))
            {
                shopVM = new ShopVM()
                {
                    Products = _unitOfWork.Product.GetAll(u => u.Category.Name == category, includeProperty: "Category"),
                    FilterPrice = new FilterPrice()
                };
            }
            else
            {
                shopVM = new ShopVM()
                {
                    Products = _unitOfWork.Product.GetAll(includeProperty: "Category"),
                    FilterPrice = new FilterPrice()
                };
            }

            return View(shopVM);
            //if (shopVM == null || shopVM.Products == null)
            //{
            //    shopVM = new ShopVM
            //    {
            //        Products = _unitOfWork.Product.GetAll(includeProperty: "Category"),
            //        FilterPrice = new FilterPrice()
            //    };
            //}

            //return View(shopVM);
            //ShopVM shopViewModel;
            //if (ViewBag.shopVM != null)
            //{
            //    shopViewModel = new ShopVM
            //    {
            //        Products = ViewBag.shopVM,
            //        FilterPrice = new FilterPrice() // Initialize FilterPrice object
            //    };
            //}
            //else
            //{
            //    shopViewModel = new ShopVM
            //    {
            //        Products = _unitOfWork.Product.GetAll(includeProperty: "Category"),
            //        FilterPrice = new FilterPrice() // Initialize FilterPrice object
            //    };
            //}



            ////IEnumerable<Product> productList =
            ////shopevm.Products = productList;
            ////shopevm.FilterPrice = new();
            //return View(shopViewModel);

        }

        [HttpPost]
        [ActionName("Shop")]
        public IActionResult ShopPOST(ShopVM shopVM)
        {
            ShopVM shopViewModel = new ShopVM();
            //checking if Category is "All" and either of the input is 0 .For this All product should be retrieved
            if (shopVM.FilterPrice.CategoryName == "All" && (shopVM.FilterPrice.FromPrice == 0 && shopVM.FilterPrice.ToPrice == 0))
            {

                shopViewModel = new()
                {
                    Products = _unitOfWork.Product.GetAll(includeProperty: "Category"),
                    FilterPrice = new FilterPrice() // Initialize FilterPrice object
                };
            }
            //checking if Category is "All" and neither of the input is 0 .For this All product should be filtered based on Price
            else if (shopVM.FilterPrice.CategoryName == "All" &&  shopVM.FilterPrice.ToPrice != 0)
            {
                shopViewModel = new()
                {
                    Products = _unitOfWork.Product.FilterProductByPrice(shopVM.FilterPrice.FromPrice, shopVM.FilterPrice.ToPrice),
                    FilterPrice = new FilterPrice() // Initialize FilterPrice object
                };
            }
            else
            {
                //checking if Category is other than "All and either of the input is 0 .For this  product should be filtered based on Category only

                if (shopVM.FilterPrice.ToPrice == 0 && shopVM.FilterPrice.FromPrice == 0)
                {
                    shopViewModel = new()
                    {
                        Products = _unitOfWork.Product.FilterByCategory(shopVM.FilterPrice.CategoryName),
                        FilterPrice = new FilterPrice() // Initialize FilterPrice object
                    };


                }
                else
                //checking if Category is other than "All" and neither of the input is 0 .For this  product should be filtered based on Category as well as Price.
                {
                    shopViewModel = new()
                    {

                        Products = _unitOfWork.Product.FilterByCategoryAndPrice(shopVM.FilterPrice.CategoryName, shopVM.FilterPrice.FromPrice, shopVM.FilterPrice.ToPrice),
                        FilterPrice = new FilterPrice() // Initialize FilterPrice object
                    };
                }
            }
            return View(shopViewModel);

        }
        
        public ActionResult Wishlist(int productId)
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

             Wishlist wishlist = new ()
            {
                
                ProductId = productId,
                ApplicationUserId = userId,
                
            };

            var wishlistFromDb = _unitOfWork.Wishlist.Get(
                w => w.ProductId == productId && w.ApplicationUserId == userId,
                    includeProperty: "Product");


            if (wishlistFromDb == null)
            {
                _unitOfWork.Wishlist.Add(wishlist);
                _unitOfWork.Save();
            }

           
            return RedirectToAction("Index");

            
            
        }

        public IActionResult Search(string? category,string? searchText)
        {
            ShopVM shopVM = new ShopVM();
            if(searchText == null) { 
                
                return RedirectToAction("Index"); 
            
            }
            if(category=="All Categories")
            {
                shopVM = new ShopVM()
                {
                    Products = _unitOfWork.Product.GetAll(u => u.Name.Contains(searchText), includeProperty: "Category"),
                    FilterPrice = new FilterPrice()
                };
            }
            
            
            else
            {

                shopVM = new ShopVM()
                {
                    Products = _unitOfWork.Product.GetAll(u => u.Name.Contains(searchText) && u.Category.Name == category, includeProperty: "Category"),
                    FilterPrice = new FilterPrice()
                };
            }
        
        
            
            return View(shopVM);
        }

        public IActionResult CategoryResult(string? category)
        {
            //ShopVM shopVM;
            //shopVM = new ShopVM()
            //{
            //    Products = _unitOfWork.Product.GetAll(u => u.Category.Name==category, includeProperty: "Category"),
            //    FilterPrice = new FilterPrice()
            //};
            //ViewBag.shopVM=shopVM;
            //return RedirectToAction("Shop",shopVM);

            return RedirectToAction("Shop", new { category });
        }

        public IActionResult QuickView(int productId)
        {
            var product = _unitOfWork.Product.Get(u => u.Id == productId);
            return View("QuickView",product);
           
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Compare()
        {
            return View();
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
