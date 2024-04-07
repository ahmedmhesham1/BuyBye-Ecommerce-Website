using Ecommerce.Data;
using Ecommerce.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.RepoServices
{
	public class OrderRepoService : IOrderRepo
	{
		public ApplicationDbContext Context;
		private IUserRepo userRepo;
		public OrderRepoService(ApplicationDbContext context, IUserRepo userRepo)
		{
			Context = context;
			this.userRepo = userRepo;

		}
		public void Delete(int id)
		{
			var order = Context.Orders.Find(id);
			Context.Orders.Remove(order);
			Context.SaveChanges();
		}

		public List<Order> GetAll()
		{
			var orders = Context.Orders.Include(o => o.User).Include(o => o.OrderItems).Include(o => o.Address).ToList();
			return orders;
		}

		public List<Order> GetAllUserOrders()
		{
			var orders = Context.Orders.Where(o => o.UserId == userRepo.GetCurrentLoggedInUserId()).OrderByDescending(o => o.OrderDate).Include(o => o.User).Include(o => o.OrderItems).ThenInclude(oi => oi.ProductItem).ThenInclude(pi => pi.Product).Include(o => o.Address).ToList();
			return orders;
		}

		public Order GetDetails(int id)
		{

			var order = Context.Orders.Include(o => o.User).Include(o => o.OrderItems).Include(o => o.Address).FirstOrDefault(o => o.Id == id);
			return order;
		}

		public List<Order> GetLatestOrders()
		{
			var orders = Context.Orders.Where(o => o.UserId == userRepo.GetCurrentLoggedInUserId()).Take(3).OrderByDescending(o => o.OrderDate).Include(o => o.User).Include(o => o.OrderItems).ThenInclude(oi => oi.ProductItem).ThenInclude(pi => pi.Product).Include(o => o.Address).ToList();
			return orders;
		}

		public void Insert(Order Order)
		{
			Context.Orders.Add(Order);
			Context.SaveChanges();
		}

		public void Update(int id, Order Order)
		{
			Context.Orders.Update(Order);
			Context.SaveChanges();
		}
        public void ApproveOrder(int orderId)
        {
            var order = Context.Orders.Find(orderId);
            order.OrderStatus = Status.Delivered;
            Context.SaveChanges();
        }

        public List<Order> GetSellerOrders()
        {
            var sellerId = userRepo.GetCurrentLoggedInUserId();

            var orders = Context.Orders.Include(o => o.User).Include(o => o.OrderItems).ThenInclude(o => o.ProductItem)
                .ThenInclude(o => o.Product).ThenInclude(o => o.Seller).ToList();
            var result = new List<Order>();
            foreach (var order in orders)
            {
                foreach (var oItem in order.OrderItems)
                {
                    if (oItem.ProductItem.Product.SellerId == sellerId)
                    {
                        result.Add(order);
                    }

                }
            }

            return result;
        }
    }
}