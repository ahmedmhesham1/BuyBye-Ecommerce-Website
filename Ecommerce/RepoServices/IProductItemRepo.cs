using Ecommerce.Models;

namespace Ecommerce.RepoServices
{
	public interface IProductItemRepo
	{
		public List<ProductItem> GetAll();
		public ProductItem GetDetails(int id);
		public void Insert(ProductItem ProductItem);
		public void Update(int id, ProductItem ProductItem);
		public List<ProductItem> GetProductItemsList(int id);
		public void Delete(int id);
	}
}
