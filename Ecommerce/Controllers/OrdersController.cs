using Microsoft.AspNetCore.Mvc;
using Ecommerce.RepoServices;
using Microsoft.AspNetCore.Authorization;

namespace Ecommerce.Controllers
{
    [Authorize(Roles ="User")]
    public class OrdersController : Controller
    {
		public IOrderRepo OrderRepo { get; }

		public OrdersController(IOrderRepo orderRepo)
        {
			OrderRepo = orderRepo;
		}

        // GET: Orders
        public IActionResult Index()
        { 
            return View(OrderRepo.GetAllUserOrders());
        }

    }
}
