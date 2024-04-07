using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.Models
{
    [Serializable]
    public class Address
    {
        [Key]
        public int Id { get; set; }
        [MinLength(3)]
        public string? City { get; set; }
		[MinLength(3)]
		public string? Country { get; set; } = "Egypt";
        public int? UnitNumber { get; set; }
		[MinLength(3)]
		public string? StreetName { get; set; }
        public int? PostalCode { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            Address other = (Address)obj; return string.Equals(UnitNumber, other.UnitNumber) && string.Equals(StreetName, other.StreetName) && string.Equals(City, other.City) && string.Equals(Country, other.Country) && string.Equals(PostalCode, other.PostalCode);
        }
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17; hash = hash * 23 + (UnitNumber?.GetHashCode() ?? 0); hash = hash * 23 + (StreetName?.GetHashCode() ?? 0); hash = hash * 23 + (City?.GetHashCode() ?? 0); hash = hash * 23 + (Country?.GetHashCode() ?? 0); hash = hash * 23 + (PostalCode?.GetHashCode() ?? 0); return hash;
            }
        }
    }
}
