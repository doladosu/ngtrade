using System.ComponentModel.DataAnnotations;

namespace NgTrade.Models.ViewModel
{
    public class ReferViewModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        public string Name { get; set; }

        public string ReferralName { get; set; }
    }
}