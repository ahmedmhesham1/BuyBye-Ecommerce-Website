using Ecommerce.Models;

namespace Ecommerce.RepoServices
{
	public interface IAddressRepo
	{
		public List<Address> GetAll();
		public Address GetDetails(int id);
		public void Insert(Address address);
		public void Update(int id, Address address);
		public void Delete(int id);
        public bool IsExist(Address address);
        public Address GetDetails(Address address);
    }
}
