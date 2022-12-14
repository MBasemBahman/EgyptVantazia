using Dashboard.Areas.AccountEntity.Models;
using Entities.CoreServicesModels.AccountModels;
using System.ComponentModel;

namespace Dashboard.Areas.PaymentEntity.Models
{
    public class PaymentFilter : DtParameters
    {
        public int Id { get; set; }
        public int Fk_Account { get; set; }

        public string DashboardSearch { get; set; }
    }
    public class PaymentDto : PaymentModel
    {
        [DisplayName(nameof(CreatedAt))]
        public new string CreatedAt { get; set; }

        [DisplayName(nameof(Account))]
        public new AccountDto Account { get; set; }
    }
}
