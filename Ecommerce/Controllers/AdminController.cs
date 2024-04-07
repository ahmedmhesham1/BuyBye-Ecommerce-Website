using Ecommerce.Data;
using Ecommerce.RepoServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AdminController : Controller
    {
        public IProductRepo ProductRepo { get; }
        public ICategoryRepo CategoryRepo { get; }
        public IOrderRepo OrderRepo { get; }
        public IUserRepo UserRepo { get; }

        public AdminController(IProductRepo productRepo, ICategoryRepo categoryRepo, IOrderRepo OrderRepo, IUserRepo UserRepo)
        {
            ProductRepo = productRepo;
            CategoryRepo = categoryRepo;
            this.OrderRepo = OrderRepo;
            this.UserRepo = UserRepo;
        }


        public IActionResult Index()
        {
            var allprds = ProductRepo.GetWaitingList();
            return View(allprds);
        }
        public IActionResult ApproveProduct(int id)
        {
            var prd = ProductRepo.GetDetails(id);
            if (prd != null)
            {
                prd.IsApproved = true;
                ProductRepo.Update(id, prd,null);
            }
            return RedirectToAction("Index");
        }
        public IActionResult ApproveAllProducts()
        {
            ProductRepo.ApproveAll();
            return RedirectToAction("Index");
        }
        public IActionResult RejectAllProducts()
        {
            ProductRepo.RejectAll();
            return RedirectToAction("Index");
        }
        public IActionResult RejectProduct(int id)
        {
            var prd = ProductRepo.GetDetails(id);
            if (prd != null)
            {
                ProductRepo.Delete(id);
            }
            return RedirectToAction("Index");
        }
        public IActionResult GetSalesData()
        {
            var salesData = OrderRepo.GetAll()
                .Where(o => o.OrderDate.Year == DateTime.Now.Year)
                .GroupBy(o => o.OrderDate.Month)
                .Select(g => new { Month = g.Key, TotalSales = g.Sum(o => o.TotalPrice) })
                .OrderBy(g => g.Month)
                .ToList();


            return Json(salesData);
        }

        public IActionResult Chart()
        {
            var orders = OrderRepo.GetAll().Count;
            ViewBag.OrdersNum = orders;

            var prds = ProductRepo.GetAll().Count();
            ViewBag.ProductsNum = prds;

            var cats = CategoryRepo.GetAll().Count();
            ViewBag.CategoriesNum = cats;

            var users = UserRepo.GetAll().Count;
            ViewBag.UsersNum = users;
            return View();
        }



    }
}