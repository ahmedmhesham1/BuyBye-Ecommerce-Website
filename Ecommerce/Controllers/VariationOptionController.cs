using Ecommerce.Models;
using Ecommerce.RepoServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Controllers
{
    public class VariationOptionController : Controller
    {
		private readonly IVariationRepo varRepo;
        private readonly IVariationOptionRepo variationOptionRepo;
        public VariationOptionController(IVariationRepo _varRepo, IVariationOptionRepo _variationOptionRepo)
        {
            varRepo = _varRepo;
            variationOptionRepo = _variationOptionRepo;
        }

        // GET: Categories
        public IActionResult Index(int id)   //Category id
        {
            return View(variationOptionRepo.GetAll().Where(v => v.VariationId == id));
        }

        // GET: Categories/Details/5
        public IActionResult Details(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var variationopt = variationOptionRepo.GetDetails(id);

            if (variationopt == null)
            {
                return NotFound();
            }

            return View(variationopt);
        }

        // GET: Categories/Create
        public IActionResult Create(int id)
        {
            ViewBag.Variation = varRepo.GetDetails(id);
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create([Bind("VariationId,Value")] VariationOption varOpt)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    variationOptionRepo.Insert(varOpt);

                    return RedirectToAction("Create", new { id = varOpt.VariationId });
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
            if (id == null)
            {
                return NotFound();
            }

            var variationopt = variationOptionRepo.GetDetails(id);
            if (variationopt == null)
            {
                return NotFound();
            }
            return View(variationopt);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult Edit(int id, VariationOption variationopt)
        {
            VariationOption EdittedVar = variationOptionRepo.GetDetails(id);

            if (id != variationopt.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    variationOptionRepo.Update(id, variationopt);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (variationOptionRepo.GetDetails(id) == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), routeValues: new { id = EdittedVar.VariationId });
            }
            return View(variationopt);
        }

        // GET: Categories/Delete/5
        public IActionResult Delete(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var variationop = variationOptionRepo.GetDetails(id);
            if (variationop == null)
            {
                return NotFound();
            }

            return View(variationop);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            VariationOption deletedVar = variationOptionRepo.GetDetails(id);
            if (deletedVar != null)
            {
                variationOptionRepo.Delete(id);
            }

            return RedirectToAction("Create", routeValues: new { id = deletedVar.VariationId });
        }

    }
}
