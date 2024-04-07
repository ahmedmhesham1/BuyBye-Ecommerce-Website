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
using Microsoft.AspNetCore.Authorization;

namespace Ecommerce.Controllers
{
	[Authorize(Roles = "User")]
	public class SellerProductController : Controller
	{
		private readonly IProductRepo prdRepo;
		private readonly ICategoryRepo categoryRepo;
		private readonly IProductItemRepo prdItemRepo;
		private readonly IUserRepo userRepo;
		private readonly IReviewRepo reviewRepo;
		private readonly IOrderRepo OrderRepo;

        public SellerProductController(IProductRepo _prdRepo,ICategoryRepo _categoryRepo,IProductItemRepo _prdItemRepo, IUserRepo _userRepo, IReviewRepo _reviewRepo, IOrderRepo OrderRepo)
		{
			prdRepo = _prdRepo;
			categoryRepo = _categoryRepo;
			prdItemRepo = _prdItemRepo;
			userRepo = _userRepo;
			reviewRepo = _reviewRepo;
			this.OrderRepo = OrderRepo;

        }

		// GET: SellerProduct
		public IActionResult Index(string? prdName, int pg = 1)
		{
			var products = prdRepo.GetAllSellerProducts();
			//const int pageSize = 5;
			//if (pg < 1)  // if user entered a page number less than 1 like -1 or 0 which is invalid
			//	pg = 1;
			//int recsCount = products.Count();
			//var pager = new Pager(recsCount, pg, pageSize);
			//int recSkip = (pg - 1) * pageSize;
			//var data = products.Skip(recSkip).Take(pager.PageSize).ToList();
			//ViewBag.pageSize = pageSize;

			//this.ViewBag.Pager = pager;
			if (!string.IsNullOrEmpty(prdName))
			{
				//	string insensitivePrdName = prdName.ToLower();
				//	return View(prdRepo.GetAll().Where(p => p.Name.ToLower().Contains(prdName)));
				return View(products);
			}

			//return View(data);
            return View(products);

		}

		public IActionResult Details(int id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var product = prdRepo.GetDetails(id);
			ViewBag.ProductItemsList = prdItemRepo.GetProductItemsList(id);

			if (product == null)
			{
				return NotFound();
			}
			var u = userRepo.GetDetails(userRepo.GetCurrentLoggedInUserId());
			if (u.Id != product.SellerId)
			{
				return RedirectToAction("Index");
			}
			ViewBag.CustomerId = u.Id;
			ViewBag.Customer = userRepo.GetDetails(ViewBag.CustomerId);
			ViewBag.ProductId = product.Id;

			//// for rating
			///
			var ratings = reviewRepo.GetAllForPrd(id);
			if (ratings.Count() > 0)
			{
				var ratingSum = ratings.Sum(r => r.Rating);
				ViewBag.ratingSum = ratingSum;
				var ratingCount = ratings.Count();
				ViewBag.ratingCount = ratingCount;
			}
			else
			{
				ViewBag.ratingCount = 1;
				ViewBag.ratingSum = 1;
			}
			ViewBag.reviewRating = reviewRepo.GetAllForPrd(id).ToList();

			return View(product);
		}
		public IActionResult Create()
		{
			//var u = await userManger.GetUserAsync(User);
			ViewBag.SellerId = userRepo.GetCurrentLoggedInUserId();
			SelectList CatList = new SelectList(categoryRepo.GetAll(), "Id", "Name");
			ViewBag.CatSL = CatList;

			return View();
		}

		// POST: Categories/Create

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Create(Product product, IFormFile? Image)

		{
			if (ModelState.IsValid)
			{
				try
				{
					prdRepo.Insert(product, Image);
					return RedirectToAction("Create", "ProductItem", product); /// da el hayt3'ayyarrrrr lel product item action
				}
				catch (Exception ex)
				{
					ModelState.AddModelError("", ex.Message);
					return View("CatchView");
				}
			}
			return View(product);
		}
		public IActionResult Edit(int id)
		{
			//var u = await userManger.GetUserAsync(User);

			ViewBag.SellerId = userRepo.GetCurrentLoggedInUserId();
			SelectList CatList = new SelectList(categoryRepo.GetAll(), "Id", "Name");
			ViewBag.CatSL = CatList;
			ViewBag.img = prdRepo.GetDetails(id).Image;

			if (id == null)
			{
				return NotFound();
			}

			var product = prdRepo.GetDetails(id);
			product.Image = ViewBag.img;
			if (product == null)
			{
				return NotFound();
			}
			return View(product);
		}

		// POST: Categories/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Edit(int id, Product product, IFormFile? Image, string ExistingImage)
		{

			if (id != product.Id)
			{
				return NotFound();
			}


			if (ModelState.IsValid)
			{
				try
				{
					// If no new image is uploaded, keep the existing image value

					prdRepo.Update(id, product, Image);
				}
				catch (DbUpdateConcurrencyException)
				{
					if (prdRepo.GetDetails(id) == null)
					{
						return NotFound();
					}
					else
					{
						return View("CatchView");
					}
				}
				return RedirectToAction(nameof(Index));
			}
			return View(product);
		}

		// GET: Categories/Delete/5
		public IActionResult Delete(int id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var product = prdRepo.GetDetails(id);
			if (product == null)
			{
				return NotFound();
			}

			return View(product);
		}

		// POST: Categories/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public IActionResult DeleteConfirmed(int id)
		{
			if (id != null)
			{
				prdRepo.Delete(id);
			}

			return RedirectToAction(nameof(Index));
		}
        public IActionResult approveOrders()
        {
            var orders = OrderRepo.GetSellerOrders();
            return View(orders);
        }
        [HttpPost]
        public IActionResult approveOrders(int orderId)
        {
            var orders = OrderRepo.GetSellerOrders();
            OrderRepo.ApproveOrder(orderId);
            return View(orders);
        }

    }
}
