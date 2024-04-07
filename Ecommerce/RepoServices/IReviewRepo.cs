using Ecommerce.Models;

namespace Ecommerce.RepoServices
{
	public interface IReviewRepo
	{
		public List<Review> GetAll();
		public Review GetDetails(int id);
		public void Insert(Review Review);
		public void Update(int id, Review Review);
		public void Delete(int id);
		public List<Review> GetAllForPrd(int id);
	}
}
