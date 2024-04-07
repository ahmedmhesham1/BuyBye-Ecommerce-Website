using Ecommerce.Models;
using Ecommerce.RepoServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers
{
    [Authorize(Roles = "User")]
    public class CartItemController : Controller
    {

        private readonly ICartItemRepo cartItemRepo;

        private readonly IProductItemRepo productItemRepo;
        private readonly IProductRepo prdRepo;
        private readonly IVariationOptionRepo variationOptRepo;
        public CartItemController(IProductRepo prdRepo, IVariationOptionRepo _variationOptRepo, ICartItemRepo _cartItemRepo, IUserRepo _userRepo, IProductItemRepo _productItemRepo, ICartRepo _cartRepo)
        {
            cartItemRepo = _cartItemRepo;
            productItemRepo = _productItemRepo;
            variationOptRepo = _variationOptRepo;
            this.prdRepo = prdRepo;

        }
        [HttpPost]
        public IActionResult AddtoCart(int id, int quantity, List<int> values)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                List<VariationOption> options = new List<VariationOption>();
                var prdItems = productItemRepo.GetAll().Where(item => item.ProductId == id).ToList(); //items of this prd

                foreach (var varOpt in values)
                {
                    var obj = variationOptRepo.GetDetails(varOpt);
                    prdItems = prdItems.Where(item => item.Configurations.Contains(obj)).ToList();
                }

                if (prdItems.Any())
                {
                    int selectedItem = prdItems.FirstOrDefault().Id;
                    int selectedQuantity = quantity;
                    CartItem cartItem = prdRepo.AddToCart(selectedItem, selectedQuantity);
                    TempData["AlertMessage"] = "Product Added Successfully";
                    
                    Product product = prdRepo.GetDetails((int)prdItems.FirstOrDefault().ProductId);
                    return Json(new { id = cartItem.Id, quantity= selectedQuantity, price=product.Price, name=product.Name, image = product.Image, cartId = cartItem.CartId});
                }
                else
                {
                    return NotFound("Product not found");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, "An error occurred while adding the product to the cart.");
            }
        }

        public ActionResult Delete(int id)
        {
            try
            {
                int cartId = cartItemRepo.GetDetails(id).CartId;
                cartItemRepo.Delete(id);
                return RedirectToAction("Details", "Cart", new { id = cartId });
            }
            catch
            {
                return NotFound();
            }
        }

        [HttpPost]
        public ActionResult UpdateQuantity(int id, int quantity)
        {
            try
            {
                CartItem updatedItem = cartItemRepo.GetDetails(id);
                updatedItem.Quantity = quantity;
                cartItemRepo.Update(id, updatedItem);

                Console.WriteLine(cartItemRepo.GetDetails(id).Quantity);
                return Json(new { success = true });
            }
            catch
            {
                return NotFound();
            }
        }
    }
}