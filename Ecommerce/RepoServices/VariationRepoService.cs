using Ecommerce.Data;
using Ecommerce.Models;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.RepoServices
{
	public class VariationRepoService : IVariationRepo
	{
		public ApplicationDbContext Context { get; }
		public VariationRepoService(ApplicationDbContext context)
		{
			Context = context;
		}
		public void Delete(int id)
		{
			var deletedVariation = Context.Variations.Find(id);
			if (deletedVariation != null)
			{
				Context.Variations.Remove(deletedVariation);
				Context.SaveChanges();
			}
		}

		public List<Variation> GetAll()
		{
			return Context.Variations.Include(v => v.Category).Include(v => v.Options).ToList();
		}

		public Variation GetDetails(int id)
		{
			return Context.Variations.Include(v => v.Category).Include(v => v.Options).FirstOrDefault(v => v.Id == id);
		}

		public void Insert(int categoryId, string name)
		{
			if (categoryId != null)
			{
				Variation variation = new() { Name = name , CategoryId = categoryId };
				Context.Variations.Add(variation);
				Context.SaveChanges();
			}
		}

		public void Update(int id, Variation Variation)
		{
			var oldVar = Context.Variations.Find(id);
			if (oldVar != null)
			{
				oldVar.CategoryId = Variation.CategoryId;
				oldVar.Name = Variation.Name;
				Context.SaveChanges();
			}
		}
	}
}