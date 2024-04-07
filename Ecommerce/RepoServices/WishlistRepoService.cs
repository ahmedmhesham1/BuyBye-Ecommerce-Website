using Ecommerce.Data;
using Ecommerce.Models;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.RepoServices
{
	public class WishlistRepoService : IWishlistRepo
	{
		public ApplicationDbContext Context { get; }
		private readonly IProductRepo prdRrepo;
		public WishlistRepoService(ApplicationDbContext context, IProductRepo prdRrepo)
		{
			Context = context;
			this.prdRrepo = prdRrepo;
		}
		public void Delete(int id)
		{
			var wishlist = Context.Wishlists.Find(id);
			if (wishlist != null)
			{
				Context.Wishlists.Remove(wishlist);
				Context.SaveChanges();
			}
		}

		public void AddProduct(Wishlist wishlist, Product product)
		{
			if (wishlist != null && product != null)
			{
				wishlist.Products.Add(product);
				Context.SaveChanges();
			}

		}

		public List<Wishlist> GetAll()
		{
			return Context.Wishlists.ToList();
		}

		public Wishlist GetDetails(int id)
		{
			return Context.Wishlists.Include(w => w.Products).ThenInclude(p => p.Items).
				   Include(w => w.Products).ThenInclude(p => p.Reviews).Where(w => w.Id == id).FirstOrDefault();
		}

		public void Update(int id, Wishlist wishlist)
		{
			var updatedWishlist = Context.Wishlists.Where(w => w.Id == id).FirstOrDefault();
			if (updatedWishlist != null)
			{
				Context.Update(updatedWishlist);
				Context.SaveChanges();
			}
		}

		public void ClearAllprds(int id)
		{
			Wishlist wishlist = GetDetails(id);

			if (wishlist != null)
			{
				wishlist.Products.Clear();
				Context.SaveChanges();
			}
		}

		public void RemovePrd(int id, int prd)
		{
			Wishlist wishlist = GetDetails(id);
			Product product = prdRrepo.GetDetails(prd);

			if (wishlist != null)
			{
				wishlist.Products.Remove(product);
				Context.SaveChanges();
			}
		}
	}
}