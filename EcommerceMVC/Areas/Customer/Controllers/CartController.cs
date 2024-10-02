﻿using Ecommerce.DataAccess.Repository.IRepository;
using Ecommerce.Models;
using Ecommerce.Models.ViewModels;
using Ecommerce.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EcommerceMVC.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        [BindProperty]
        public ShoppingCartVM ShoppingCartVM { get; set; }
        public CartController(IUnitOfWork unitOfWork)
        {
          
            _unitOfWork = unitOfWork;

        }
        [Authorize]
        public IActionResult Index()
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
             ShoppingCartVM = new()
            {
                 ShoppingCartList=_unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId, includeProperty: "Product"),
                 OrderHeader=new()
                 
             };
            foreach(var cart in ShoppingCartVM.ShoppingCartList)
            {
                cart.Price= (double)cart.Product.Price;
                ShoppingCartVM.OrderHeader.OrderTotal += (cart.Price*cart.Count);
            }
            

            return View(ShoppingCartVM);
        }


        //public IActionResult Remove(int cartId)
        //{
        //    var cartToRemove = _unitOfWork.ShoppingCart.Get(u => u.Id == cartId);
        //    _unitOfWork.ShoppingCart.Remove(cartToRemove);
        //    _unitOfWork.Save();

        //    return RedirectToAction("Index");
        //}

        public IActionResult Remove(int cartId)
        {
            try
            {
                var cartToRemove = _unitOfWork.ShoppingCart.Get(u => u.Id == cartId);
                if (cartToRemove == null)
                {
                    return NotFound();
                }

                _unitOfWork.ShoppingCart.Remove(cartToRemove);
                _unitOfWork.Save();

                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                // Handle any exceptions
                return StatusCode(500, new { error = ex.Message });
            }
        }


        public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartVM = new()
            {
                ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId,
                includeProperty: "Product"),
                OrderHeader = new()
            };

            ShoppingCartVM.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUser.Get(u => u.Id == userId);

            ShoppingCartVM.OrderHeader.Name = ShoppingCartVM.OrderHeader.ApplicationUser.Name;
            ShoppingCartVM.OrderHeader.PhoneNumber = ShoppingCartVM.OrderHeader.ApplicationUser.PhoneNumber;
            ShoppingCartVM.OrderHeader.StreetAddress = ShoppingCartVM.OrderHeader.ApplicationUser.StreetAddress;
            ShoppingCartVM.OrderHeader.City = ShoppingCartVM.OrderHeader.ApplicationUser.City;
            ShoppingCartVM.OrderHeader.State = ShoppingCartVM.OrderHeader.ApplicationUser.State;
            ShoppingCartVM.OrderHeader.PostalCode = ShoppingCartVM.OrderHeader.ApplicationUser.PostalCode;



            foreach (var cart in ShoppingCartVM.ShoppingCartList)
            {
                cart.Price = (double)cart.Product.Price;
                ShoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Count);
            }
            
            return View(ShoppingCartVM);
        }


        [HttpPost]
        [ActionName("Summary")]
       
        public IActionResult SummaryPOST()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartVM.ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId,
                includeProperty: "Product");

            ShoppingCartVM.OrderHeader.OrderDate = System.DateTime.Now;
            ShoppingCartVM.OrderHeader.ApplicationUserId = userId;

            ApplicationUser applicationUser = _unitOfWork.ApplicationUser.Get(u => u.Id == userId);
           

                foreach (var cart in ShoppingCartVM.ShoppingCartList)
                {
                    cart.Price = (double)cart.Product.Price;
                    ShoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Count);
                }



                //it is a regular user
                ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusDelayedPayment;
                ShoppingCartVM.OrderHeader.OrderStatus = SD.StatusApproved;

                _unitOfWork.OrderHeader.Add(ShoppingCartVM.OrderHeader);
                _unitOfWork.Save();
                foreach (var cart in ShoppingCartVM.ShoppingCartList)
                {
                    OrderDetail orderDetail = new()
                    {
                        ProductId = cart.ProductId,
                        OrderHeaderId = ShoppingCartVM.OrderHeader.Id,
                        Price = cart.Price,
                        count = cart.Count
                    };
                    _unitOfWork.OrderDetail.Add(orderDetail);
                    _unitOfWork.Save();
                }
            
           


            return RedirectToAction(nameof(OrderConfirmation), new { id = ShoppingCartVM.OrderHeader.Id });
        }


        public IActionResult OrderConfirmation(int id)
        {

            //OrderHeader orderHeader = _unitOfWork.OrderHeader.Get(u => u.Id == id, includeProperties: "ApplicationUser");
            //if (orderHeader.PaymentStatus != SD.PaymentStatusDelayedPayment)
            //{
            //    //this is an order by customer

            //    var service = new SessionService();
            //    Session session = service.Get(orderHeader.SessionId);

            //    if (session.PaymentStatus.ToLower() == "paid")
            //    {
            //        _unitOfWork.OrderHeader.UpdateStripePaymentID(id, session.Id, session.PaymentIntentId);
            //        _unitOfWork.OrderHeader.UpdateStatus(id, SD.StatusApproved, SD.PaymentStatusApproved);
            //        _unitOfWork.Save();
            //    }
            //    HttpContext.Session.Clear();

            //}

            //_emailSender.SendEmailAsync(orderHeader.ApplicationUser.Email, "New Order - Bulky Book",
            //    $"<p>New Order Created - {orderHeader.Id}</p>");

            //List<ShoppingCart> shoppingCarts = _unitOfWork.ShoppingCart
            //    .GetAll(u => u.ApplicationUserId == orderHeader.ApplicationUserId).ToList();

            //_unitOfWork.ShoppingCart.RemoveRange(shoppingCarts);
            //_unitOfWork.Save();

            return View(id);
        }


        

       
    }
}
