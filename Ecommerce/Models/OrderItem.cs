using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.Models
{
    public class OrderItem
    {
        [Key]
        public int Id { get; set; }
        public ProductItem? ProductItem { get; set; }

        [ForeignKey("ProductItem")]
        public int? ProductId { get; set; }
        public int? Quantity { get; set; }
        public Order Order { get; set; }
    }
}