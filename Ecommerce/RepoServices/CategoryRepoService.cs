using Ecommerce.Data;
using Ecommerce.Models;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.RepoServices
{
	public class CategoryRepoService : ICategoryRepo
	{
		public ApplicationDbContext Context { get; }
		public CategoryRepoService(ApplicationDbContext context)
		{
			Context = context;
		}

		public void Delete(int id)
		{
			Category category = Context.Categories.FirstOrDefault(c => c.Id == id);
			if (category != null)
			{
				Context.Categories.Remove(category);
				Context.SaveChanges();
			}
		}

		public List<Category> GetAll()
		{
			return Context.Categories.Include(c=>c.Variations).ToList();
		}

		public Category GetDetails(int id)
		{
			return Context.Categories.Include(c => c.Variations).Include(c => c.Products).FirstOrDefault(c => c.Id == id);
		}

		public void Insert(Category Category)
		{
			if (Category != null)
			{
				Context.Categories.Add(Category);
				Context.SaveChanges();
			}
		}

		public void Update(int id, Category Category)
		{
			Category categoryUpdated = Context.Categories.Find(id);

			if (categoryUpdated != null)
			{
				categoryUpdated.Products = Category.Products;
				categoryUpdated.Variations = Category.Variations;
				categoryUpdated.Description = Category.Description;
				categoryUpdated.Name = Category.Name;

				Context.SaveChanges();
			}
		}
	}
}