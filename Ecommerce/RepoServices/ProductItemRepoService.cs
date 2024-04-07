using Ecommerce.Data;
using Ecommerce.Models;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.RepoServices
{
	public class ProductItemRepoService : IProductItemRepo
	{
		public ApplicationDbContext Context { get; }
		public ProductItemRepoService(ApplicationDbContext context)
		{
			Context = context;
		}
		public void Delete(int id)
		{
			var p = Context.ProductItems.Find(id);
			Context.ProductItems.Remove(p);
			Context.SaveChanges();
		}

		public List<ProductItem> GetAll()
		{
			var products = Context.ProductItems.Include(p => p.Product).Include(p => p.OrderItems).Include(p => p.Configurations).Include(p => p.CartItems).ToList();
			return products;
		}

		public ProductItem GetDetails(int id)
		{
			ProductItem p = Context.ProductItems.Include(p => p.Product).Include(p => p.OrderItems).Include(p => p.Configurations).Include(p => p.CartItems).FirstOrDefault(p => p.Id == id);
			return p;
		}
		public List<ProductItem> GetProductItemsList(int id)
		{
			return Context.ProductItems.Include(p => p.Product)
				.Include(p => p.Configurations)
				.ThenInclude(c => c.Variation)
				.Where(p => p.ProductId == id).ToList();
		}
		public void Insert(ProductItem ProductItem)
		{
			Context.ProductItems.Add(ProductItem);
			Context.SaveChanges();
		}

		public void Update(int id, ProductItem ProductItem)
		{
			Context.ProductItems.Update(ProductItem);
			Context.SaveChanges();
		}
	}
}