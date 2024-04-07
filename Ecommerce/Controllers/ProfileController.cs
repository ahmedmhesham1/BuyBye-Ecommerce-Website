using Ecommerce.RepoServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers
{
	[Authorize(Roles ="User")]
	public class ProfileController : Controller
	{
		public IProductRepo prdRepo;
		public UserManager<IdentityUser> userManger;
		public ICategoryRepo categoryRepo;
		public IProductItemRepo prdItemRepo;
		public IVariationRepo variationRepo;
		public IVariationOptionRepo variationOptRepo;
        private readonly IOrderRepo ordRepo;

        public ProfileController(IProductRepo _prdRepo,
						UserManager<IdentityUser> _usrManger,
						ICategoryRepo _categoryRepo,
						IProductItemRepo _prdItemRepo,
						IVariationRepo _variationRepo,
						IVariationOptionRepo _variationOptRepo, IOrderRepo ordRepo)
		{
			prdRepo = _prdRepo;
			userManger = _usrManger;
			categoryRepo = _categoryRepo;
			prdItemRepo = _prdItemRepo;
			variationRepo = _variationRepo;
			variationOptRepo = _variationOptRepo;
            this.ordRepo = ordRepo;
        }
		public IActionResult Index()
		{
			ViewBag.SellerPrds = prdRepo.GetLastSellerProducts();
            ViewBag.LatestOrders = ordRepo.GetLatestOrders();
			var orders = ordRepo.GetSellerOrders();
			return View(orders);
		}
        public IActionResult Rest()
		{
			return View();
		}
    }
}
