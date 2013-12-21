using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace NgTrade.Models.ViewModel
{
    public class TradeViewModel
    {
        [Required]
        [Display(Name = "Symbol")]
        public string Symbol { get; set; }

        [Required]
        [Display(Name = "# of Shares")]
        public int Shares { get; set; }

        [Display(Name = "Buy or Sell")]
        public string Action { get; set; }
        public IEnumerable<SelectListItem> ActionsList { get; set; }

        [Required]
        [Display(Name = "Price")]
        public double Price { get; set; }
    }
}