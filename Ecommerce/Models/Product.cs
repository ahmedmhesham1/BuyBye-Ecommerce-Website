using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        [MinLength(10)]
        public string? Description { get; set; }

        [ForeignKey("Seller")]
        public string? SellerId { get; set; }
        public User? Seller { get; set; }
        public decimal Price { get; set; }
        public bool IsApproved { get; set; } = false;
        public string? Image {  get; set; }
        public Category? Category { get; set; }
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public List<ProductItem>? Items { get; set; }
        public List<Review>? Reviews { get; set; }

		public List<Wishlist>? Wishlists { get; set; }
	}
}
