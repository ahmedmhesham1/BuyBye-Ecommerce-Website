using System.ComponentModel.DataAnnotations;

namespace Ecommerce.DTO
{
    public class ResetPassDTO 
    {
        //public string Id {  get; set; }
        [DataType(DataType.Password)]

        public string RecentPassword { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }



    }
}
