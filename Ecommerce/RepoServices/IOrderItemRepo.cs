using Ecommerce.Models;

namespace Ecommerce.RepoServices
{
	public interface IOrderItemRepo
	{
		public List<OrderItem> GetAll();
		public OrderItem GetDetails(int id);
		public void Insert(OrderItem OrderItem);
		public void Update(int id, OrderItem OrderItem);
		public void Delete(int id);
	}
}
