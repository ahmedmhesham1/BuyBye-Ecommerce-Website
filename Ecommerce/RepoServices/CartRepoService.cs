using Ecommerce.Data;
using Ecommerce.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace Ecommerce.RepoServices
{
	public class CartRepoService : ICartRepo
	{
		public ApplicationDbContext Context { get; }
		public CartRepoService(ApplicationDbContext context)
		{
			Context = context;
		}

		public void Delete(int id)
		{
			Cart cart = Context.Carts.FirstOrDefault(c => c.Id == id);
			if (cart != null)
			{
                var cartItems = Context.CartItems.Where(item => item.CartId == id).ToList();

                // Remove cart items
                Context.CartItems.RemoveRange(cartItems);
                
				Context.SaveChanges();
			}
		}

		public List<Cart> GetAll()
		{
			return Context.Carts.ToList();
		}

		public Cart GetDetails(int id)
		{
			return Context.Carts.Include(c => c.CartItems).ThenInclude(ci => ci.ProductItem)
				.ThenInclude(pi => pi.Product).Where(c => c.Id == id).FirstOrDefault();
		}

		public void Insert(Cart cart)
		{
			if (cart != null)
			{
				Context.Carts.Add(cart);
				Context.SaveChanges();
			}
		}

		public void Update(int id, Cart cart)
		{
			Cart cartUpdated = Context.Carts.Find(id);
			if (cartUpdated != null)
			{
				cartUpdated.CartItems = cart.CartItems;
				Context.SaveChanges();
			}
		}
	}
}