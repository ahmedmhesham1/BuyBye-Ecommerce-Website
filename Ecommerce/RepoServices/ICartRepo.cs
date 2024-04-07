using Ecommerce.Models;

namespace Ecommerce.RepoServices
{
	public interface ICartRepo
	{
		public List<Cart> GetAll();
		public Cart GetDetails(int id);
		public void Insert(Cart cart);
		public void Update(int id, Cart cart);
		public void Delete(int id);
	}
}
