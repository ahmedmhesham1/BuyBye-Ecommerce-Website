using Ecommerce.Data;
using Ecommerce.Models;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.RepoServices
{
	public class VariationOptionRepoService : IVariationOptionRepo
	{
		public ApplicationDbContext Context { get; }
		public VariationOptionRepoService(ApplicationDbContext context)
		{
			Context = context;
		}

		public void Delete(int id)
		{
			var deletedVariationOption = Context.VariationOptions.Find(id);
			if (deletedVariationOption != null)
			{
				Context.VariationOptions.Remove(deletedVariationOption);
				Context.SaveChanges();
			}
		}

		public List<VariationOption> GetAll()
		{
			return Context.VariationOptions.Include(vo => vo.Variation).ToList();
		}

		public VariationOption GetDetails(int id)
		{
			return Context.VariationOptions.Include(vo => vo.Variation).FirstOrDefault(vo => vo.Id == id);

		}

		public void Insert(VariationOption VariationOption)
		{
			if (VariationOption != null)
			{
				Context.VariationOptions.Add(VariationOption);
				Context.SaveChanges();
			}
		}

		public void Update(int id, VariationOption VariationOption)
		{
			var oldVarOpt = Context.VariationOptions.Find(id);
			if (oldVarOpt != null)
			{
				oldVarOpt.VariationId = VariationOption.VariationId;
				oldVarOpt.Value = VariationOption.Value;
				Context.SaveChanges();
			}
		}
	}
}