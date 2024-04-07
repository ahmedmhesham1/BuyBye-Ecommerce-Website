using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.Models
{
    public class Review
    {
        [Key]
        public int Id { get; set; }
        public Product Product { get; set; }
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public User Customer { get; set; }

        [ForeignKey("Customer")]
        public string CustomerId { get; set; }
        public string? Comment { get; set; }
        [Range(0, 5, ErrorMessage = "Rating is from 0 to 5 only!") ]
        public int? Rating { get; set; }
    }
}