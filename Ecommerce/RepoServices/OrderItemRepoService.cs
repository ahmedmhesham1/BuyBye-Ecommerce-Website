using Ecommerce.Data;
using Ecommerce.Models;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.RepoServices
{
	public class OrderItemRepoService : IOrderItemRepo
	{
		public ApplicationDbContext Context { get; }
		public OrderItemRepoService(ApplicationDbContext context)
		{
			Context = context;
		}

		public void Delete(int id)
		{
			var orderItemRepo = Context.OrdersItems.Find(id);
			if (orderItemRepo != null)
			{
				Context.OrdersItems.Remove(orderItemRepo);
				Context.SaveChanges();
			}
		}

		public List<OrderItem> GetAll()
		{
			var orderItems = Context.OrdersItems.Include(o => o.ProductItem).Include(o => o.Order).ToList();
			return orderItems;
		}

		public OrderItem GetDetails(int id)
		{
			var orderItem = Context.OrdersItems.Include(o => o.ProductItem).Include(o => o.Order).Where(o => o.Id == id).FirstOrDefault();
			return orderItem;
		}

		public void Insert(OrderItem OrderItem)
		{
			Context.OrdersItems.Add(OrderItem);
			Context.SaveChanges();
		}

		public void Update(int id, OrderItem OrderItem)
		{
			OrderItem item = Context.OrdersItems.Find(id);
			item.Quantity = OrderItem.Quantity;
			//Context.Update(OrderItem);
			Context.SaveChanges();
		}
	}
}