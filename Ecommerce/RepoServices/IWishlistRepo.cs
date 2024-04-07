using Ecommerce.Models;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.RepoServices
{
	public interface IWishlistRepo
	{
		public void AddProduct(Wishlist wishlist, Product product);
		public void ClearAllprds(int id);
		public Wishlist GetDetails(int id);
		public List<Wishlist> GetAll();
		public void Update(int id, Wishlist wishlist);
		public void Delete(int id);

		public void RemovePrd(int id, int prd);
	}
}