using Ecommerce.Data;
using Ecommerce.Models;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.RepoServices
{
	public class ProductRepoService : IProductRepo
	{
		public ApplicationDbContext Context { get; }
		private readonly IHttpContextAccessor HttpContextAccessor;
		private readonly UserManager<IdentityUser> UserManager;
		private readonly IProductItemRepo productItemRepo;
		private readonly ICartItemRepo cartItemRepo;
		private readonly ICartRepo cartRepo;
		private readonly IUserRepo userRepo;
		private readonly IReviewRepo reviewRepo;




		public IWebHostEnvironment WebHost { get; }
		public ProductRepoService(ApplicationDbContext context,
			IWebHostEnvironment webHostEnvironment,
			UserManager<IdentityUser> userManager, IHttpContextAccessor httpContextAccessor,
			IUserRepo _userRepo, ICartRepo _cartRepo, IProductItemRepo _prdItemRepo, ICartItemRepo cartItemRepo, IReviewRepo reviewRepo)
		{
			Context = context;
			WebHost = webHostEnvironment;
			HttpContextAccessor = httpContextAccessor;
			UserManager = userManager;
			productItemRepo = _prdItemRepo;
			cartRepo = _cartRepo;
			userRepo = _userRepo;
			this.cartItemRepo = cartItemRepo;
			this.reviewRepo = reviewRepo;
		}


		public void Delete(int id)
		{
			var p = Context.Products.Find(id);
			if (p != null)
			{
				Context.Products.Remove(p);
				Context.SaveChanges();
			}
		}

		public List<Product> GetAll()
		{
			return Context.Products.Include(p => p.Seller).Include(p => p.Category).Include(r => r.Reviews).ToList();
		}
		public List<Product> GetAllSellerProducts()
		{

			var currentUser = UserManager.GetUserAsync(HttpContextAccessor.HttpContext.User).Result;
			return Context.Products
				.Where(p => p.SellerId == currentUser.Id).Include(c => c.Category) // Assuming SellerId is the foreign key for the seller's ID
				.ToList();
		}

		public Product GetDetails(int id)
		{
			return Context.Products
					.Include(p => p.Reviews)
					.Include(p => p.Seller)
					.Include(p => p.Category)
					.ThenInclude(c => c.Variations) // Include Variations for Category
					.ThenInclude(v => v.Options)
					.ThenInclude(v => v.ProductItems)// Include Options for each Variation
					.Where(p => p.Id == id)
					.FirstOrDefault();
		}

		public void Insert(Product product, IFormFile Image)
		{
			if (product != null)
			{
				try
				{
					string uploadsFolder = Path.Combine(WebHost.WebRootPath, "Uploads");
					string uniqueFileName = $"{Guid.NewGuid()}_{Image.FileName}";
					string filePath = Path.Combine(uploadsFolder, uniqueFileName);
					using (var fileStream = new FileStream(filePath, FileMode.Create))
					{
						Image.CopyTo(fileStream);
						fileStream.Close();
					}
					product.Image = uniqueFileName;
					Context.Products.Add(product);
					Context.SaveChanges();
				}
				catch
				{
				}

			}
		}

		public void Update(int id, Product updatedProduct, IFormFile Image)
		{
			var existingProduct = Context.Products.FirstOrDefault(p => p.Id == id);

			if (existingProduct != null)
			{
				if (Image == null)
				{
					updatedProduct.Image = existingProduct.Image;
				}
				else
				{
					string uploadsFolder = Path.Combine(WebHost.WebRootPath, "Uploads");
					string uniqueFileName = $"{Guid.NewGuid()}_{Image.FileName}";
					string filePath = Path.Combine(uploadsFolder, uniqueFileName);
					using (var fileStream = new FileStream(filePath, FileMode.Create))
					{
						Image.CopyTo(fileStream);
						fileStream.Close();
					}
					existingProduct.Image = uniqueFileName;
				}
				// Update the properties of the existing product with the new values
				existingProduct.Name = updatedProduct.Name;
				existingProduct.Description = updatedProduct.Description;
				existingProduct.Price = updatedProduct.Price;
				existingProduct.CategoryId = updatedProduct.CategoryId;

				Context.Update(existingProduct);
				Context.SaveChanges();
			}
		}

		public void ApproveAll()
		{
			var allProducts = GetWaitingList();
			if (allProducts.Count() != 0)
			{
				foreach (var product in allProducts)
				{
					product.IsApproved = true;
					Context.Update(product);
					Context.SaveChanges();
				}
			}
		}
		public void RejectAll()
		{
			var allProducts = GetWaitingList();
			if (allProducts.Count() != 0)
			{
				foreach (var product in allProducts)
				{
					Context.Products.Remove(product);
					Context.SaveChanges();
				}
			}
		}
		public List<Product> GetWaitingList()
		{
			return Context.Products.Where(p => p.IsApproved == false).Include(p => p.Seller).Include(p => p.Category).ToList();
		}
		public List<Product> GetApproved()
		{
			return Context.Products.Where(p => p.IsApproved == true).Include(p => p.Seller).Include(p => p.Category).ToList();
		}

		public List<Product> GetLastSellerProducts()
		{
			return Context.Products
				.Where(p => p.SellerId == userRepo.GetCurrentLoggedInUserId()).Include(c => c.Category) // Assuming SellerId is the foreign key for the seller's ID
				.Take(3).OrderByDescending(p => p.Id).ToList();
		}
		public CartItem AddToCart(int productItemNum, int quantity)
		{
			//var u = await userManger.GetUserAsync(User);
			ProductItem selectedItem = productItemRepo.GetDetails(productItemNum);
			User user = userRepo.GetDetails(userRepo.GetCurrentLoggedInUserId());
			Cart cart = cartRepo.GetDetails((int)user.CartId);
			bool exists = false;
			//check if this item is in cart already
			foreach (CartItem item in cart.CartItems)
			{
				if (item.ProductItemId == productItemNum)
				{
					exists = true;
					break;
				}
			}
			if (!exists)
			{
				CartItem cartItem = new CartItem
				{
					Quantity = quantity,
					ProductItemId = productItemNum,
					CartId = (int)user.CartId
				};
				cartItemRepo.Insert(cartItem);
				return cartItem;
			}
			return null;
		}

		public List<Product> GetTop3()
		{
			var products = Context.Products.ToList();
			var averageRatings = new Dictionary<int, double?>();

			foreach (var product in products)
			{
				var reviews = reviewRepo.GetAllForPrd(product.Id);
				double? averageRating = 0;

				if (reviews.Any())
				{
					averageRating = reviews.Average(r => r.Rating);
				}

				averageRatings[product.Id] = averageRating;
			}
			var topRatedProductid = averageRatings.OrderByDescending(pair => pair.Value)
									 .Take(3)
									 .Select(pair => pair.Key)
									 .ToList();
			List<Product> topRatedProducts = new();
			foreach (var productID in topRatedProductid)
			{
				topRatedProducts.Add(GetDetails(productID));
			}

			return topRatedProducts;
		}


	}
}