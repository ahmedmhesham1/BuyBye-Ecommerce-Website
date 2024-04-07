using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ecommerce.RepoServices;
using Ecommerce.Models;
using Microsoft.AspNetCore.Authorization;

namespace Ecommerce.Controllers
{
	[Authorize(Roles = "User")]
	public class WishlistController : Controller
	{
		private readonly IUserRepo userRepo;
		private readonly IWishlistRepo wishListRepo;
		private readonly IProductRepo productRepo;

		public WishlistController(IUserRepo userRepo, IWishlistRepo wishListRepo, IProductRepo productRepo)
		{
			this.userRepo = userRepo;
			this.wishListRepo = wishListRepo;
			this.productRepo = productRepo;
		}
		public IActionResult Details()
		{
			var user = userRepo.GetDetails(userRepo.GetCurrentLoggedInUserId());

			Wishlist wishlist = wishListRepo.GetDetails(user.WishlistId);

			return View(wishlist);
		}

		public void AddToWishlist(int id)
		{
			var user = userRepo.GetDetails(userRepo.GetCurrentLoggedInUserId());

			Wishlist wishlist = wishListRepo.GetDetails(user.WishlistId);

			Product product = productRepo.GetDetails(id);

			wishListRepo.AddProduct(wishlist, product);
		}

		[HttpPost]
		public void ClearAllprds()
		{
			var user = userRepo.GetDetails(userRepo.GetCurrentLoggedInUserId());

			wishListRepo.ClearAllprds(user.WishlistId);
		}

		[HttpPost]
		public void RemovePrd(int prdId)
		{
			var user = userRepo.GetDetails(userRepo.GetCurrentLoggedInUserId());

			wishListRepo.RemovePrd(user.WishlistId, prdId);
		}
	}
}