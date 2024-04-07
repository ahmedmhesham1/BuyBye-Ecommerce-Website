using Ecommerce.DTO;
using Ecommerce.Models;
using Microsoft.AspNetCore.Identity;

namespace Ecommerce.RepoServices
{
	public interface IUserRepo
	{
		public List<User> GetAll();
		public User GetDetails(string id);
		public void Update(string id, User User);
		public void Delete(string id);

		public string GetCurrentLoggedInUserId();
		public Task ResetPassword(ResetPassDTO newPassVM);

	}
}
