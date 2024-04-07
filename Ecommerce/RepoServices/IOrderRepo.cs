using Ecommerce.Models;

namespace Ecommerce.RepoServices
{
	public interface IOrderRepo
	{
		public List<Order> GetAll();
		public List<Order> GetLatestOrders();
		public List<Order> GetAllUserOrders();
		public Order GetDetails(int id);
		public void Insert(Order Order);
		public void Update(int id, Order Order);
		public void Delete(int id);
		public List<Order> GetSellerOrders();
		public void ApproveOrder(int orderId);

    }
}
