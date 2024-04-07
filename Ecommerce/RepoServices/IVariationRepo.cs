using Ecommerce.Models;

namespace Ecommerce.RepoServices
{
	public interface IVariationRepo
	{
		public List<Variation> GetAll();
		public Variation GetDetails(int id);
		public void Insert(int categoryId, string name);
        public void Update(int id, Variation Variation);
		public void Delete(int id);
	}
}
