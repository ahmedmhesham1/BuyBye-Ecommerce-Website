using Ecommerce.Models;

namespace Ecommerce.RepoServices
{
	public interface IProductRepo
	{
		public List<Product> GetAll();
		public List<Product> GetAllSellerProducts();
		public List<Product> GetLastSellerProducts();
		public Product GetDetails(int id);
		public void Insert(Product product, IFormFile Image);
		public void Update(int id, Product updatedProduct, IFormFile Image);
		public void Delete(int id);
		public List<Product> GetWaitingList();
		public List<Product> GetApproved();
		public void ApproveAll();
		public void RejectAll();
		public CartItem AddToCart(int productItemNum, int quantity);
		public List<Product> GetTop3();


	}
}
