using Ecommerce.Data;
using Ecommerce.Models;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.RepoServices
{
	public class CartItemRepoService : ICartItemRepo
	{
		public ApplicationDbContext Context { get; }
		public CartItemRepoService(ApplicationDbContext context)
		{
			Context = context;
		}

		public void Delete(int id)
		{
			CartItem cartItem = Context.CartItems.FirstOrDefault(c => c.Id == id);
			if (cartItem != null)
			{
				Context.CartItems.Remove(cartItem);
				Context.SaveChanges();
			}
		}

		public List<CartItem> GetAll()
		{
			return Context.CartItems.Include(c => c.ProductItem).Include(c => c.Cart).ToList();

		}
        public List<CartItem> GetCartItems(int cartId)
        {
            return Context.CartItems.Include(c => c.ProductItem).ThenInclude(p=>p.Configurations).ThenInclude(c=>c.Variation).Include(p=>p.ProductItem).ThenInclude(i=>i.Product).Include(c => c.Cart).Where(c=>c.CartId==cartId).ToList();

        }
        public CartItem GetDetails(int id)
		{
			return Context.CartItems.Include(c => c.ProductItem).Where(c => c.Id == id).FirstOrDefault();
			//return Context.CartItems.Find(id);
		}

		public void Insert(CartItem cartItem)
		{
			if (cartItem != null)
			{
				Context.CartItems.Add(cartItem);
				Context.SaveChanges();
			}
		}

		public void Update(int id, CartItem cartItem)
		{
			CartItem cartItemUpdated = Context.CartItems.Find(id);
			if (cartItemUpdated != null)
			{
				cartItemUpdated.Quantity = cartItem.Quantity;

				Context.SaveChanges();
			}
		}
	}
}