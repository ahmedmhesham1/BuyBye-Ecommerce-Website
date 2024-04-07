using Ecommerce.Models;
using Ecommerce.RepoServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace Ecommerce.Controllers

{
	

	public class VariationController : Controller

    {

        public IVariationRepo varRepo;

        public ICategoryRepo categoryRepo;

        public VariationController(IVariationRepo _varRepo, ICategoryRepo categoryRepo)

        {

            varRepo = _varRepo;

            this.categoryRepo = categoryRepo;

        }

        // GET: Categories

        public IActionResult Index(int id)   //Category id

        {

            return View(varRepo.GetAll().Where(v => v.CategoryId == id));

        }

        // GET: Categories/Details/5

        public IActionResult Details(int id)

        {

            if (id == null)

            {

                return NotFound();

            }

            var variation = varRepo.GetDetails(id);

            if (variation == null)

            {

                return NotFound();

            }

            return View(variation);

        }

        // GET: Categories/Create

        public IActionResult Create(int id)

        {

            ViewBag.Category = categoryRepo.GetDetails(id);

            return View();

        }

        // POST: Categories/Create

        // To protect from overposting attacks, enable the specific properties you want to bind to.

        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]

        //[ValidateAntiForgeryToken]

        public ActionResult Create(int CategoryId, string Name)

        {

            if (ModelState.IsValid)

            {

                try

                {

                    varRepo.Insert(CategoryId, Name);

                    return RedirectToAction("Create", new { id = CategoryId });

                }

                catch (Exception ex)

                {

                    ModelState.AddModelError("", ex.Message);

                    return Content("variation");

                }

            }

            return Content("variation");

        }

        // GET: Categories/Edit/5

        public IActionResult Edit(int id)

        {
            ViewBag.Category = categoryRepo.GetDetails(id);

            if (id == null)

            {

                return NotFound();

            }

            var variation = varRepo.GetDetails(id);

            if (variation == null)

            {

                return NotFound();

            }

            return View(variation);

        }

        // POST: Categories/Edit/5

        // To protect from overposting attacks, enable the specific properties you want to bind to.

        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]

        //[ValidateAntiForgeryToken]

        public IActionResult Edit(int id, Variation variation)

        {

            Variation EdittedVar = varRepo.GetDetails(id);

            if (id != variation.Id)

            {

                return NotFound();

            }

            if (ModelState.IsValid)

            {

                try

                {

                    varRepo.Update(id, variation);

                }

                catch (DbUpdateConcurrencyException)

                {

                    if (varRepo.GetDetails(id) == null)

                    {

                        return NotFound();

                    }

                    else

                    {

                        throw;

                    }

                }

                return RedirectToAction("Create", "VariationOption",routeValues: new { id = variation.Id });

            }

            return View(variation);

        }

        // GET: Categories/Delete/5

        public IActionResult Delete(int id)

        {

            if (id == null)

            {

                return NotFound();

            }

            var variation = varRepo.GetDetails(id);

            if (variation == null)

            {

                return NotFound();

            }

            return View(variation);

        }

        // POST: Categories/Delete/5

        [HttpPost, ActionName("Delete")]

        [ValidateAntiForgeryToken]

        public IActionResult DeleteConfirmed(int id)

        {

            Variation deletedVar = varRepo.GetDetails(id);

            if (deletedVar != null)

            {

                varRepo.Delete(id);

            }

            return RedirectToAction("Create", routeValues: new { id = deletedVar.CategoryId });

        }

    }

}
