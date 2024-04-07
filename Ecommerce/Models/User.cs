using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.Models
{
    [Table("User")]
    public class User: IdentityUser
    {
        [MaxLength(20)]
        public string? Fname { get; set; }
        [MaxLength(20)]
        public string? Lname { get; set; }
        public Address? Address { get; set; }
        [ForeignKey(nameof(Address))]
        public int? AddressId { get; set; }
        public List<Order>? Orders { get; set; }=new List<Order>();
        public List<Review>? Reviews { get; set; }=new List<Review>();
        public Cart? Cart { get; set; } = new();
        [ForeignKey("Cart")]
        public int? CartId { get; set; }
        public List<Product>? PostedProducts { get; set; } = new List<Product>();

		public Wishlist? Wishlist { get; set; } = new();

		[ForeignKey("Wishlist")]
		public int WishlistId { get; set; }
	}
}
