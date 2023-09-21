namespace Entities.DBModels.AccountModels
{
    public class Payment : BaseEntity
    {
        [DisplayName(nameof(Account))]
        [ForeignKey(nameof(Account))]
        public int Fk_Account { get; set; }

        [DisplayName(nameof(Account))]
        public Account Account { get; set; }

        [DisplayName("Transaction Id")]
        public string TransactionId { get; set; }

        [DisplayName("Amount")]
        public double Amount { get; set; }

        [DisplayName("PaymentProvider")]
        public string PaymentProvider { get; set; }
    }
}
