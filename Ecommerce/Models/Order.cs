using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public DateTime? DeliverDate { get; set; } = DateTime.Now.AddDays(14);
        public bool IsDelivered { get; set; } = false;
        public List<OrderItem>? OrderItems { get; set; }
        public User User { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        public decimal TotalPrice { get; set; }
        public Address? Address { get; set; }

        [ForeignKey("Address")]
		public int AddressId { get; set; }
		public Status OrderStatus { get; set; } = Status.Preparing;
        [Phone]
        public string PhoneNumber {  get; set; }

    }


    public enum Status
    {
        Preparing, Shipped, Delivered, Cancelled
    }
}
