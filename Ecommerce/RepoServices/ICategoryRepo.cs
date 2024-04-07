using Ecommerce.Models;

namespace Ecommerce.RepoServices
{
	public interface ICategoryRepo
	{
		public List<Category> GetAll();
		public Category GetDetails(int id);
		public void Insert(Category Category);
		public void Update(int id, Category Category);
		public void Delete(int id);
	}
}
