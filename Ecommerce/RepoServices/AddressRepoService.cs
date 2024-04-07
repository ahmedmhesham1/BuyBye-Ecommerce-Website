using Ecommerce.Data;
using Ecommerce.Models;

namespace Ecommerce.RepoServices
{
	public class AddressRepoService : IAddressRepo
	{
		public ApplicationDbContext Context { get;}

        public AddressRepoService(ApplicationDbContext context)
        {
            Context= context;
        }

        public void Delete(int id)
		{
			Address AddDeleted = Context.Addresses.Find(id);
			if (AddDeleted != null)
			{
				Context.Addresses.Remove(AddDeleted);
				Context.SaveChanges();
			}
		}

		public List<Address> GetAll()
		{
			return Context.Addresses.ToList();
		}

		public Address GetDetails(int id)
		{
			return Context.Addresses.Find(id);
		}

		public void Insert(Address address)
		{
			if (address != null)
			{
				Context.Addresses.Add(address);
				Context.SaveChanges();
			}
		}

		public void Update(int id, Address address)
		{
			Address AddUpdated = Context.Addresses.Find(id);
			if (AddUpdated != null)
			{
				AddUpdated.UnitNumber = address.UnitNumber;
				AddUpdated.StreetName = address.StreetName;
				AddUpdated.PostalCode = address.PostalCode;
				AddUpdated.City = address.City;
				AddUpdated.Country = address.Country;

				Context.SaveChanges();
			}
		}
        public bool IsExist(Address address)
		{
			return Context.Addresses
				.Any(
				a=>a.City== address.City
				&&
				a.UnitNumber==address.UnitNumber
				&&
				a.StreetName==address.StreetName
				&&
				a.PostalCode==address.PostalCode
				&&
				a.Country==address.Country);
		}

        public Address GetDetails(Address address)
		{
            return Context.Addresses
                .FirstOrDefault(
                a => a.City == address.City
                &&
                a.UnitNumber == address.UnitNumber
                &&
                a.StreetName == address.StreetName
                &&
                a.PostalCode == address.PostalCode
                &&
                a.Country == address.Country);
        }
    }
}
