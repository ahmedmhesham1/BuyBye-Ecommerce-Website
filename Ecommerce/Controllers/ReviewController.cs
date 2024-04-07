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
using Microsoft.AspNetCore.Authorization;

namespace Ecommerce.Controllers
{
    public class ReviewController : Controller
    {
        private readonly IReviewRepo reviewRepo;

        public ReviewController(IReviewRepo context)
        {
            reviewRepo = context;
        }

        // GET: Review
        public  IActionResult Index()
        {
            
            return View(reviewRepo.GetAll());
        }

        // GET: Review/Details/5

        public IActionResult Details(int id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var category = reviewRepo.GetDetails(id);

			if (category == null)
			{
				return NotFound();
			}

			return View(category);
		}

		// GET: Review/Create
		[Authorize(Roles = "User")]

		public IActionResult Create()
        {

            return View();
        }

        // POST: Review/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = "User")]

		public ActionResult CreateReview(int productId, string comment,string CustomerId, int Rating)
		{
			if (ModelState.IsValid)
			{
				var review = new Review
				{
					ProductId = productId,
					Comment = comment,
                    CustomerId = CustomerId,
					Rating =Rating-1
				};
				reviewRepo.Insert(review);

				// ... logic to save the review (using your data access layer)

				return RedirectToAction("Details", "Product", new { id = productId });  // Redirect back to product details
			}

			// Handle potential errors (e.g., model validation issues)

			return View();  // Consider returning a view with pre-populated data and error messages
		}

		// GET: Review/Edit/5
		[Authorize(Roles = "User")]

		public IActionResult Edit(int id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var category = reviewRepo.GetDetails(id);
			if (category == null)
			{
				return NotFound();
			}
			return View(category);
		}

		// POST: Review/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
        [ValidateAntiForgeryToken]
		[Authorize(Roles = "User")]

		public IActionResult Edit(int id, [Bind("Id,ProductId,CustomerId,Comment")] Review review)
        {
            if (id != review.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
					reviewRepo.Insert(review);
                }
                catch (DbUpdateConcurrencyException)
                {
					if (reviewRepo.GetDetails(id) == null)
					{
						return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            return View(review);
        }

		// GET: Review/Delete/5
		[Authorize(Roles = "User")]

		public IActionResult Delete(int id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var category = reviewRepo.GetDetails(id);
			if (category == null)
			{
				return NotFound();
			}

			return View(category);
		}

		// POST: Review/Delete/5
		[HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
		[Authorize(Roles = "User")]

		public IActionResult DeleteConfirmed(int id)
		{
			if (id != null)
			{
				reviewRepo.Delete(id);
			}

			return RedirectToAction(nameof(Index));
		}
	}
}
