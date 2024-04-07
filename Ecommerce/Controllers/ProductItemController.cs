using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ecommerce.Data;
using Ecommerce.Models;
using Microsoft.AspNetCore.Identity;
using Ecommerce.RepoServices;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;
using NuGet.Packaging.Signing;
using Microsoft.AspNetCore.Authorization;
using Ecommerce.DTO;

namespace Ecommerce.Controllers
{
    public class ProductItemController : Controller
    {
        public IProductItemRepo prdItemRepo;
        public IProductRepo prdRepo;
        public IVariationRepo variationRepo;
        public IVariationOptionRepo variationOptRepo;

		public ProductItemController(IProductItemRepo _prdRepo, IProductRepo prdRepo, IVariationRepo variationRepo, IVariationOptionRepo _variationOptRepo)
        {
            prdItemRepo = _prdRepo;
            this.prdRepo = prdRepo;
            this.variationRepo = variationRepo;
            variationOptRepo = _variationOptRepo;
		}

        // GET: Categories
        public IActionResult Index(int id)   //prd id
        {
            return View(prdItemRepo.GetAll().Where(p => p.ProductId == id));
        }

        // GET: Categories/Details/5
        public IActionResult Details(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var variation = prdItemRepo.GetDetails(id);

            if (variation == null)
            {
                return NotFound();
            }

            return View(variation);
        }

        // GET: Categories/Create
        [Authorize(Roles ="User")]
        public IActionResult Create(Product prd)
        {
            var variations = variationRepo.GetAll().Where(v => v.CategoryId == prd.CategoryId);
            ViewBag.variations = variations;
            ViewBag.CatId=prd.CategoryId;
            ViewBag.prdId = prd.Id;
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
		[Authorize(Roles = "User")]
		public ActionResult Create([Bind(["ProductId","QuantityInStock"])] ProductItem productItem, IFormCollection form)
        {
            if (ModelState.IsValid)
            {
                try
                {
					foreach (var varOpt in form["Value"])
					{
						var obj = variationOptRepo.GetDetails(int.Parse(varOpt));

						productItem.Configurations.Add(obj);
					}
					prdItemRepo.Insert(productItem);
					return RedirectToAction(nameof(Create),prdRepo.GetDetails((int)productItem.ProductId));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(productItem);
                }
            }
            return View(productItem);
        }

		// GET: Categories/Edit/5
		[Authorize(Roles = "User")]
		public IActionResult Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prdItem = prdItemRepo.GetDetails(id);
            if (prdItem == null)
            {
                return NotFound();
            }
            return View(prdItem);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
		[Authorize(Roles = "User")]

		public IActionResult Edit(int id, ProductItem productItem)
        {
            ProductItem EdittedprdItem = prdItemRepo.GetDetails(id);

            if (id != EdittedprdItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    prdItemRepo.Update(id, productItem);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (prdItemRepo.GetDetails(id) == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), routeValues: new { id = EdittedprdItem.ProductId });
            }
            return View(productItem);
        }

		// GET: Categories/Delete/5
		[Authorize(Roles = "User")]
		public IActionResult Delete(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productItem = prdItemRepo.GetDetails(id);
            if (productItem == null)
            {
                return NotFound();
            }

            return View(productItem);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
		[Authorize(Roles = "User")]
		public IActionResult DeleteConfirmed(int id)
        {
            ProductItem deletedprdItem = prdItemRepo.GetDetails(id);
            if (deletedprdItem != null)
            {
                prdItemRepo.Delete(id);
            }

            return RedirectToAction(nameof(Index), routeValues: new { id = deletedprdItem.ProductId });
        }

        public IActionResult GetProductItem(int id, List<int> values)
        {
            var product = prdRepo.GetDetails(id);
            List<VariationOption> options = new();

            foreach (int value in values)
            {
                options.Add(variationOptRepo.GetDetails(value));
            }

            var prdItems = prdItemRepo.GetAll().Where(prd => prd.ProductId == id).ToList();

            foreach (var option in options)
            {
                prdItems = prdItems.Where(p => p.Configurations.Contains(option)).ToList();
            }

            var selectedPrdItem = prdItems.FirstOrDefault();
            ProductItemDTO productItemDTO = new() { Id = selectedPrdItem?.Id ?? 0, Quantity = selectedPrdItem?.QuantityInStock ?? 0 };
            return Json(productItemDTO);
        }

    }
}