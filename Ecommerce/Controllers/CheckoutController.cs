using Ecommerce.RepoServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ecommerce.Models;
using Stripe;
using Stripe.Checkout;
using Newtonsoft.Json;
using System.Net;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Text;

namespace Ecommerce.Controllers
{
	[Authorize(Roles = "User")]
	public class CheckoutController : Controller
    {
        public IUserRepo UserRepo;
        public IAddressRepo AddressRepo;
        public ICartRepo CartRepo;
        public ICartItemRepo ItemRepo;
        IOrderRepo OrderRepo;


        public CheckoutController(IUserRepo userRepo, ICartRepo cartRepo, ICartItemRepo itemRepo, IOrderRepo orderRepo, IAddressRepo addressRepo)
        {
            AddressRepo =addressRepo;
            UserRepo = userRepo;
            CartRepo = cartRepo;
            ItemRepo = itemRepo;
            OrderRepo = orderRepo;
        }
        public IActionResult BillingDetails()
        {
            //var u = await UserManager.GetUserAsync(User);

            var user = UserRepo.GetDetails(UserRepo.GetCurrentLoggedInUserId());
            ViewBag.user= user;
            //return View("BillingDetails");
            return View(user);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult BillingDetails(Models.Address address, string phoneNumber)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Assuming UserRepo.GetCurrentLoggedInUserId() returns the current user's ID.
            var userId = UserRepo.GetCurrentLoggedInUserId();
            var user = UserRepo.GetDetails(userId);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            var _address = AddressRepo.GetDetails(address);

            Models.Address Result;
            if (_address != null)
            {
                Result = _address;
            }
            else
            {
                AddressRepo.Insert(address);
                Result = address;
            }

            var addressJson = JsonConvert.SerializeObject(Result);
            HttpContext.Session.SetString("Address", addressJson.ToString());
            HttpContext.Session.SetString("phoneNumber", phoneNumber);

            return RedirectToAction("OrderDetails", new { cartId = user.CartId });
        }


        public ActionResult OrderDetails(int cartId)
        {
            var cartItems = ItemRepo.GetCartItems(cartId);
            ViewBag.cartItem = cartItems;
            return View();
        }

   

        public ActionResult PaymentInfo(int id)
        {
            return View("PaymentInfo");
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult PaymentInfo(string email)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = UserRepo.GetCurrentLoggedInUserId();
            var user = UserRepo.GetDetails(userId);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            var cartItems = ItemRepo.GetCartItems((int)user.CartId);

            var options = new Stripe.Checkout.SessionCreateOptions
            {
                SuccessUrl = "http://localhost:5143/Checkout/OrderConfirmation",
                CancelUrl = "http://localhost:5143/Checkout/PaymentInfo",
                CustomerEmail = email,
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
            };

            foreach (var item in cartItems)
            {
                var lineItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)((int)item.ProductItem.Product.Price * 100),
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.ProductItem.Product.Name,
                            Description = item.ProductItem.Product.Description,
                            Metadata = new Dictionary<string, string> {
                        { "ProductItem", item.ProductItem.ToString() }
                    }
                        },
                        Currency = "EGP",
                    },
                    Quantity = item.Quantity,
                };
                options.LineItems.Add(lineItem);
            }

            var service = new Stripe.Checkout.SessionService();
            Session session = service.Create(options);

            Response.Headers.Append("Location", session.Url);
            TempData["session"] = session.Id;
            return new StatusCodeResult(303);
        }



		//public IActionResult OrderConfirmation()
		//{
		//	var service = new SessionService();
		//	Session session = service.Get(TempData["session"].ToString());

		//	// Check if the payment status is "paid"
		//	if (session.PaymentStatus == "paid")
		//	{
		//		// Check if the address and phone number are available in the session
		//		if (HttpContext.Session.TryGetValue("Address", out byte[] addressBytes) &&
		//			HttpContext.Session.TryGetValue("phoneNumber", out byte[] phoneNumberBytes))
		//		{
		//			// Deserialize address and phone number from session
		//			var addressJson = Encoding.UTF8.GetString(addressBytes);
		//			var phoneNumber = Encoding.UTF8.GetString(phoneNumberBytes);
		//			var address = JsonConvert.DeserializeObject<Models.Address>(addressJson);

		//			// Validate the address (e.g., check if required fields are filled)
		//			if (address != null)
		//			{
		//				var user = UserRepo.GetDetails(UserRepo.GetCurrentLoggedInUserId());
		//				var cartItems = ItemRepo.GetCartItems((int)user.CartId);

		//				// Check if cart items exist
		//				if (cartItems != null && cartItems.Any())
		//				{
		//					Order order = new Order()
		//					{
		//						OrderDate = DateTime.Now,
		//						UserId = user.Id,
		//						AddressId = address.Id,
		//						PhoneNumber = phoneNumber
		//					};

		//					List<OrderItem> _orderItems = new List<OrderItem>();
		//					foreach (var cItem in cartItems)
		//					{
		//						order.TotalPrice += cItem.ProductItem.Product.Price * cItem.Quantity;
		//						removeQuantity(cItem);
		//						_orderItems.Add(new()
		//						{
		//							ProductId = cItem.ProductItemId,
		//							ProductItem = cItem.ProductItem,
		//							Quantity = cItem.Quantity,
		//						});
		//					}

		//					order.OrderItems = _orderItems;
		//					OrderRepo.Insert(order);
		//					user.Orders.Add(order);

		//					CartRepo.Delete((int)user.CartId);
		//					return View();
		//				}
		//				else
		//				{
		//					ModelState.AddModelError("", "Cart is empty.");
		//				}
		//			}
		//			else
		//			{
		//				ModelState.AddModelError("", "Invalid address.");
		//			}
		//		}
		//		else
		//		{
		//			ModelState.AddModelError("", "Address or phone number is missing.");
		//		}
		//	}
		//	else
		//	{
		//		return View("PaymentInfo");
		//	}

		//	return View();
		//}


		public IActionResult OrderConfirmation()
        {
            var service = new SessionService();
            Session session = service.Get(TempData["session"].ToString());
            if(session.PaymentStatus == "paid")
            {
                //var u = await UserManager.GetUserAsync(User);

                var addressJson = HttpContext.Session.GetString("Address") as string;
                var address = JsonConvert.DeserializeObject<Models.Address>(addressJson);

                var user = UserRepo.GetDetails(UserRepo.GetCurrentLoggedInUserId());
                var cartItems = ItemRepo.GetCartItems((int)user.CartId);

                Order order = new Order()
                {
                    OrderDate = DateTime.Now,
                    UserId = user.Id,
                    AddressId = address.Id,
                    PhoneNumber = HttpContext.Session.GetString("phoneNumber") as string
            };
                List<OrderItem> _orderItems = new List<OrderItem>();
                foreach(var cItem in cartItems)
                {
					order.TotalPrice += cItem.ProductItem.Product.Price * cItem.Quantity;
					removeQuantity(cItem);
                    _orderItems.Add(new()
                    {
                        ProductId = cItem.ProductItemId,
                        ProductItem = cItem.ProductItem,
                        Quantity = cItem.Quantity,
                    });
                }
                order.OrderItems = _orderItems;
                OrderRepo.Insert(order);
                user.Orders.Add(order);
                
                CartRepo.Delete((int)user.CartId);
                return View();
            }
            else
            {
                return View("PaymentInfo");
            }
        }
        public void removeQuantity(CartItem cItem)
        {
            cItem.ProductItem.QuantityInStock-= cItem.Quantity;
        }
        [HttpPost]
        public IActionResult CashOnDelivery()
        {
            try
            {
                //var u = await UserManager.GetUserAsync(User);

                var addressJson = HttpContext.Session.GetString("Address") as string;
                var address = JsonConvert.DeserializeObject<Models.Address>(addressJson);

                var user = UserRepo.GetDetails(UserRepo.GetCurrentLoggedInUserId());
                var cartItems = ItemRepo.GetCartItems((int)user.CartId);

                Order order = new Order()
                {
                    OrderDate = DateTime.Now,
                    UserId = user.Id,
                    AddressId = address.Id,
                    PhoneNumber = HttpContext.Session.GetString("phoneNumber") as string
                };
                List<OrderItem> _orderItems = new List<OrderItem>();
                foreach (var cItem in cartItems)
                {
                    order.TotalPrice += cItem.ProductItem.Product.Price * cItem.Quantity;
                    removeQuantity(cItem);
                    _orderItems.Add(new()
                    {
                        ProductId = cItem.ProductItemId,
                        ProductItem = cItem.ProductItem,
                        Quantity = cItem.Quantity,
                    });
                }
                order.OrderItems = _orderItems;
                OrderRepo.Insert(order);
                user.Orders.Add(order);

                CartRepo.Delete((int)user.CartId);
                return View("OrderConfirmation");
            }
            catch
            {
                return View("PaymentInfo");
            }
        }
        public IActionResult TestView()
        {
            return View("/Auth/login.cshtml");
        }        
    }
}
