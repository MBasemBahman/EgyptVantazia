using System.ComponentModel.DataAnnotations;

namespace API.Areas.PromoCodeArea.Models
{
    public class CheckPromoCodeDto
    {
        [Required]
        public string Code { get; set; }

        public int Fk_Subscription { get; set; }
    }
}
