using Ecommerce.Data;
using Ecommerce.DTO;
using Ecommerce.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.RepoServices
{
	public class UserRepoService : IUserRepo
	{
		public ApplicationDbContext Context { get; }
		private readonly UserManager<IdentityUser> userManager;
		private readonly IHttpContextAccessor HttpContextAccessor;

		public UserRepoService(ApplicationDbContext context, UserManager<IdentityUser> userManager, IHttpContextAccessor HttpContextAccessor)
		{
			Context = context;
			this.userManager = userManager;
			this.HttpContextAccessor = HttpContextAccessor;
		}

		public void Delete(string id)
		{
			var u = Context.Users.Find(id);
			if (u != null)
			{
				Context.Users.Remove(u);
				Context.SaveChanges();
			}
		}

		public List<User> GetAll()
		{
			return Context.Users.Include(u => u.Cart).Include(u => u.Address).ToList();
		}

		public User GetDetails(string id)
		{
			return Context.Users.Include(u => u.Cart).Include(u => u.Address).Include(u => u.Orders).ThenInclude(o => o.OrderItems).Where(u => u.Id == id).FirstOrDefault();
		}

		public void Update(string id, User User)
		{
			var updatedUser = Context.Users.Where(u => u.Id == id).FirstOrDefault();
			if (updatedUser != null)
			{
				updatedUser.Fname = User.Fname;
				updatedUser.Lname = User.Lname;
				updatedUser.Email = User.Email;
				if (!User.Address.Equals(updatedUser.Address))
				{
					Context.Addresses.Add(User.Address);
					Context.SaveChanges();
					updatedUser.AddressId = User.Address.Id;
				}
				Context.Update(updatedUser);
				Context.SaveChanges();
			}
		}

		public string GetCurrentLoggedInUserId()
		{
			var currentUser = userManager.GetUserAsync(HttpContextAccessor.HttpContext.User).Result;
			return currentUser?.Id ?? "";
		}

		public async Task ResetPassword(ResetPassDTO newPassVM)
		{
			var updatedUser = userManager.GetUserAsync(HttpContextAccessor.HttpContext.User).Result;

			if (updatedUser != null)
			{
				var result = await userManager.ChangePasswordAsync(updatedUser, newPassVM.RecentPassword, newPassVM.Password);
				if (!result.Succeeded)
				{
					throw new Exception();
				}
			}
			
		}
	}

}