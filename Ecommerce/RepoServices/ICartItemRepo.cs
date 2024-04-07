using Ecommerce.Models;

namespace Ecommerce.RepoServices
{
	public interface ICartItemRepo
	{
		public List<CartItem> GetAll();
		public List<CartItem> GetCartItems(int cartId);
        public CartItem GetDetails(int id);
		public void Insert(CartItem cartItem);
		public void Update(int id, CartItem cartItem);
		public void Delete(int id);
	}
}
