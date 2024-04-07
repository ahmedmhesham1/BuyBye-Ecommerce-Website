using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.Models
{
    public class VariationOption
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Variation")]
        public int VariationId { get; set; }
        public Variation? Variation { get; set; }
        public List<ProductItem>? ProductItems { get; set; }
        public string? Value { get; set; }
    }
}