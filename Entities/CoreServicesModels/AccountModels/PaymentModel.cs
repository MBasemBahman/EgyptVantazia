using Entities.RequestFeatures;

namespace Entities.CoreServicesModels.AccountModels
{
    public class PaymentParameters : RequestParameters
    {
        public int Fk_Account { get; set; }

        public string TransactionId { get; set; }

        public string DashboardSearch { get; set; }
    }

    public class PaymentModel : BaseEntity
    {
        [DisplayName(nameof(Account))]
        [ForeignKey(nameof(Account))]
        public int Fk_Account { get; set; }

        [DisplayName(nameof(Account))]
        public AccountModel Account { get; set; }

        [DisplayName("Transaction Id")]
        public string TransactionId { get; set; }

        [DisplayName("Amount")]
        public double Amount { get; set; }

        [DisplayName("PaymentProvider")]
        public string PaymentProvider { get; set; }
    }
}
