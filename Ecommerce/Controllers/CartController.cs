using Ecommerce.Models;
using Ecommerce.RepoServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers
{
    [Authorize(Roles = "User")]

    public class CartController : Controller
    {
        private readonly ICartRepo cartRepo;
        private readonly IUserRepo userRepo;

        public CartController(ICartRepo _cartRepo, IUserRepo userRepo)
        {
            cartRepo = _cartRepo;
            this.userRepo = userRepo;
        }

        // GET: CartController/Details
        public ActionResult Details()
        {
            try
            {
                User user = userRepo.GetDetails(userRepo.GetCurrentLoggedInUserId());

                Cart cart = cartRepo.GetDetails((int)user.CartId);
                return View(cart);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return View("CatchView");
            }

        }

    }
}