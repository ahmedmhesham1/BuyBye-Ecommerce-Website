using Ecommerce.Data;
using Ecommerce.Models;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.RepoServices
{
	public class ReviewRepoService : IReviewRepo
	{
		public ApplicationDbContext Context { get; }
		public ReviewRepoService(ApplicationDbContext context)
		{
			Context = context;
		}

		public void Delete(int id)
		{
			var r = Context.Reviews.Find(id);
			if (r != null)
			{
				Context.Reviews.Remove(r);
				Context.SaveChanges();
			}
		}

		public List<Review> GetAll()
		{
			return Context.Reviews.Include(r => r.Customer).Include(r => r.Product).ToList();
		}
		public List<Review> GetAllForPrd(int id)
		{
			return Context.Reviews.Where(p=>p.ProductId==id).Include(r => r.Customer).Include(r => r.Product).ToList();
		}

		public Review GetDetails(int id)
		{
			return Context.Reviews.Include(r => r.Customer).Include(r => r.Product).Where(r => r.Id == id).FirstOrDefault();
		}

		public void Insert(Review Review)
		{
			if (Review != null)
			{
				Context.Reviews.Add(Review);
				Context.SaveChanges();
			}
		}

		public void Update(int id, Review Review)
		{
			var updatedReview = Context.Reviews.Where(r => r.Id == id).FirstOrDefault();
			if (updatedReview != null)
			{
				Context.Update(updatedReview);
				Context.SaveChanges();
			}
		}
	}
}