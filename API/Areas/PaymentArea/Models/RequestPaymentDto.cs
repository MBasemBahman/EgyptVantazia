using static Entities.EnumData.LogicEnumData;

namespace API.Areas.PaymentArea.Models
{
    public class RequestPaymentDto
    {
        public PyamentTypeEnum PyamentType { get; set; }

        public string WalletIdentifier { get; set; }

        public int Fk_Subscription { get; set; }
    }
}
