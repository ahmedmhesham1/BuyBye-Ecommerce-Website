using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.Models
{
    public class ProductItem
    {
        [Key]
        public int Id { get; set; }
        public Product? Product { get; set; }
        [ForeignKey("Product")]
        public int? ProductId { get; set; }
		[Range(0, int.MaxValue, ErrorMessage = "The field {0} must be greater than {1}.")]
        public int? QuantityInStock { get; set; }
        public List<OrderItem>? OrderItems { get; set; }
        public List<VariationOption>? Configurations { get; set; } = new List<VariationOption>();
        public List<CartItem>? CartItems { get; set; } = new List<CartItem> { };

        public override string ToString()
        {
            string result = $"Item ID: {Id}\n";
            foreach (VariationOption option in Configurations)
            {
                result += $"{option.Variation.Name}: {option.Value}\n";
            }
            
            return result;
        }
    }
}