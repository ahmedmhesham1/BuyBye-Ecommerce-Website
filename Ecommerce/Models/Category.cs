using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
		[MinLength(3)]
		public string? Description { get; set; }
        public List<Product>? Products { get; set; }
        public List<Variation>? Variations { get; set; }

    }
}