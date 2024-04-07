using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ecommerce.Data;
using Ecommerce.Models;
using Ecommerce.RepoServices;
using Microsoft.AspNetCore.Identity;
using Ecommerce.DTO;

namespace Ecommerce.Controllers
{
	public class ProductController : Controller
	{
		public IProductRepo prdRepo;
		private readonly IVariationRepo variationRepo;
		private readonly IProductItemRepo prdItemRepo;
		private readonly IVariationOptionRepo variationOptRepo;
		private readonly IReviewRepo reviewRepo;
		private readonly IUserRepo userRepo;

		public ProductController(IProductRepo _prdRepo,
			IVariationRepo _variationRepo,
			IProductItemRepo _prdItemRepo,
			IVariationOptionRepo _variationOptionRepo,
			IReviewRepo _reviewRepo, IUserRepo _userRepo)
		{
			prdRepo = _prdRepo;
			variationRepo = _variationRepo;
			prdItemRepo = _prdItemRepo;
			variationOptRepo = _variationOptionRepo;
			reviewRepo = _reviewRepo;
			userRepo = _userRepo;

		}

		// GET: Product
		public IActionResult Index()
		{
			return View(prdRepo.GetAll());

		}

        // GET: Product/Details/5
        public IActionResult Details(int id)
        {
            // 1. Parameter Validation
            if (id <= 0)
            {
                return BadRequest("Invalid product ID.");
            }

            // 2. Error Handling
            var product = prdRepo.GetDetails(id);
            if (product == null)
            {
                return NotFound("Product not found.");
            }

            // 3. Move logic to separate methods or services
            List<List<VariationOption>> variationOptions = GetVariationOptionsForProduct(id, product.CategoryId);
            SetReviewDataInViewBag(id);

            return View(product);
        }

        private List<List<VariationOption>> GetVariationOptionsForProduct(int productId, int categoryId)
        {
            var variationOptions = new List<List<VariationOption>>();
            var variations = variationRepo.GetAll().Where(v => v.CategoryId == categoryId).ToList();

            foreach (var variation in variations)
            {
                var options = prdItemRepo.GetAll()
                    .Where(item => item.ProductId == productId && item.Configurations.Any(config => config.VariationId == variation.Id))
                    .SelectMany(item => item.Configurations.Where(config => config.VariationId == variation.Id))
                    .Distinct()
                    .ToList();

                variationOptions.Add(options);
            }

            ViewBag.variationOptions = variationOptions;
            ViewBag.variations = variations;

            return variationOptions;
        }

        private void SetReviewDataInViewBag(int productId)
        {
            string userId = userRepo.GetCurrentLoggedInUserId();
            ViewBag.ProductId = productId;

            if (!string.IsNullOrEmpty(userId))
            {
                ViewBag.CustomerId = userId;
                ViewBag.Customer = userRepo.GetDetails(ViewBag.CustomerId);
            }

            var ratings = reviewRepo.GetAllForPrd(productId);
            if (ratings.Count() > 0)
            {
                var ratingSum = ratings.Sum(r => r.Rating);
                ViewBag.ratingSum = ratingSum;
                ViewBag.ratingCount = ratings.Count();
            }
            else
            {
                ViewBag.ratingCount = 1;
                ViewBag.ratingSum = 1;
            }

            ViewBag.reviewRating = ratings.ToList();
        }


        

    }
}
