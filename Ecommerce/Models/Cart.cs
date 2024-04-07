using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.Models
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }
        public User? User { get; set; }
        [ForeignKey("User")]
        public string? UserId { get; set; }
        public List<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}