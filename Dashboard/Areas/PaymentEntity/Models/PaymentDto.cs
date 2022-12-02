using Dashboard.Areas.AccountEntity.Models;
using Dashboard.Areas.PlayerTransferEntity.Models;
using Dashboard.Areas.SeasonEntity.Models;
using Entities.DBModels.SeasonModels;
using System.ComponentModel;
using Dashboard.Areas.SubscriptionEntity.Models;
using Entities.CoreServicesModels.AccountModels;

namespace Dashboard.Areas.PaymentEntity.Models
{
    public class PaymentFilter : DtParameters
    {
        public int Id { get; set; }
        public int Fk_Account { get; set; }
    }
    public class PaymentDto : PaymentModel
    {
        [DisplayName(nameof(CreatedAt))]
        public new string CreatedAt { get; set; }

        [DisplayName(nameof(Account))]
        public new AccountDto Account { get; set; }
    }
}
