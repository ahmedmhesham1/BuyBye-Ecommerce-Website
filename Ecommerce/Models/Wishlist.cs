using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.Models
{
	public class Wishlist
	{
		public int Id { get; set; }
		public User? User { get; set; }

		[ForeignKey(nameof(User))]
		public string? UserId { get; set; }
		public List<Product> Products { get; set; } = new();

	}
}