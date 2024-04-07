using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.Models
{
    public class CartItem
    {
        [Key]
        public int Id { get; set; }
        public ProductItem? ProductItem { get; set; }
        
        [ForeignKey(nameof(ProductItem))]
        public int ProductItemId { get; set; }
        public int Quantity { get; set; }
        public Cart? Cart { get; set; }
        [ForeignKey(nameof(Cart))]
        public int CartId { get; set;}
    }
}