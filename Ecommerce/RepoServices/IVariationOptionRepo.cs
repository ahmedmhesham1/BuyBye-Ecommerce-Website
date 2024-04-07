using Ecommerce.Models;

namespace Ecommerce.RepoServices
{
	public interface IVariationOptionRepo
	{
		public List<VariationOption> GetAll();
		public VariationOption GetDetails(int id);
		public void Insert(VariationOption VariationOption);
		public void Update(int id, VariationOption VariationOption);
		public void Delete(int id);
	}
}
