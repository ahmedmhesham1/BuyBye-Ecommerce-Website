using Ecommerce.Models;
using Ecommerce.RepoServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using X.PagedList;


namespace Ecommerce.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public IProductRepo ProductRepo { get; }
        public ICategoryRepo CategoryRepo { get; }
        public IReviewRepo ReviewRepo { get; }

        public HomeController(ILogger<HomeController> logger, IProductRepo productRepo, ICategoryRepo categoryRepo, IReviewRepo reviewRepo)
        {
            _logger = logger;
            ProductRepo = productRepo;
            CategoryRepo = categoryRepo;
            ReviewRepo = reviewRepo;
        }

        public IActionResult Index(int? categoryId, string prdName, int? pageNo, string? data, decimal? minPrice, decimal? maxPrice)
        {
            ViewBag.list = new SelectList(CategoryRepo.GetAll(), "Id", "Name", categoryId);
            ViewBag.catlist = CategoryRepo.GetAll();
            ViewBag.CategoryId = categoryId;
            ViewBag.Data = data;
            ViewBag.MinPrice = minPrice;
            ViewBag.MaxPrice = maxPrice;
            ViewBag.search = prdName;
            ViewBag.TopRated = ProductRepo.GetTop3();

            //reviews ratings

            var products = ProductRepo.GetAll();
            var averageRatings = new Dictionary<int, double?>();  //(int => prdId , double => rate)

            foreach (var product in products)
            {
                var reviews = ReviewRepo.GetAllForPrd(product.Id);
                double? averageRating = 0;

                if (reviews.Any())
                {
                    averageRating = reviews.Average(r => r.Rating);
                }

                averageRatings[product.Id] = averageRating;
            }

            ViewBag.AverageRatings = averageRatings;

            //filter by price
            if (data == "option1")
            {
                return View(ProductRepo.GetAll().OrderBy(p => p.Price).Where(p => p.Price <= 500 && p.IsApproved).ToPagedList(pageNo ?? 1, 12));
            }
            else if (data == "option2")
            {
                return View(ProductRepo.GetAll().OrderBy(p => p.Price).Where(p => p.Price > 500 && p.Price <= 1000 && p.IsApproved).ToPagedList(pageNo ?? 1, 12));
            }
            else if (data == "option3")
            {
                return View(ProductRepo.GetAll().OrderBy(p => p.Price).Where(p => p.Price > 1000 && p.Price <= 1500 && p.IsApproved).ToPagedList(pageNo ?? 1, 12));
            }
            else if (data == "option4")
            {
                return View(ProductRepo.GetAll().OrderBy(p => p.Price).Where(p => p.Price > 2000 && p.IsApproved).ToPagedList(pageNo ?? 1, 12));
            }
            else if (data == "custom" && minPrice != null && maxPrice != null)
            {
                return View(ProductRepo.GetAll().OrderBy(p => p.Price).Where(p => p.Price >= minPrice && p.Price <= maxPrice && p.IsApproved).ToPagedList(pageNo ?? 1, 12));
            }

            //search
            if (!string.IsNullOrEmpty(prdName))
            {
                string insensitivePrdName = prdName.Trim().ToLower();
                return View(ProductRepo.GetAll().OrderByDescending(p => p.Id).Where(p => p.Name.ToLower().Contains(insensitivePrdName) && p.IsApproved).ToPagedList(pageNo ?? 1, 12));
            }

            //filter by category
            if (!categoryId.HasValue)
            {
                return View(ProductRepo.GetAll().Where(p => p.IsApproved).OrderByDescending(p => p.Id).ToPagedList(pageNo ?? 1, 12));
            }
            else
            {
                return View(ProductRepo.GetAll().Where(p => p.IsApproved).OrderByDescending(p => p.Id).Where(p => p.CategoryId == categoryId).ToPagedList(pageNo ?? 1, 12));
            }


        }

        public IActionResult Privacy()
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
