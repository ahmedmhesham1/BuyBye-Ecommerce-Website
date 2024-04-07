using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.Models
{
    public class Variation
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Category")]
        public int? CategoryId { get; set; }
        public string Name { get; set; }
        public List<VariationOption>? Options { get; set; }
        public Category? Category { get; set; }

    }
}