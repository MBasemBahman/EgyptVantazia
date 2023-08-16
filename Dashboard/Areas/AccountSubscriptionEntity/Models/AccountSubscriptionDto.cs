using Dashboard.Areas.AccountEntity.Models;
using Dashboard.Areas.SeasonEntity.Models;
using Dashboard.Areas.SubscriptionEntity.Models;
using Entities.CoreServicesModels.AccountModels;
using Entities.DBModels.SeasonModels;
using System.ComponentModel;

namespace Dashboard.Areas.AccountSubscriptionEntity.Models
{
    public class AccountSubscriptionFilter : DtParameters
    {
        public int Id { get; set; }

        public int Fk_Account { get; set; }
        public int Fk_Subscription { get; set; }

        [DisplayName(nameof(Season))]
        public int Fk_Season { get; set; }

        [DisplayName("CreatedAt")]
        public DateTime? CreatedAtFrom { get; set; }

        [DisplayName(nameof(CreatedAtTo))]
        public DateTime? CreatedAtTo { get; set; }

        [DisplayName(nameof(AccountFullName))]
        public string AccountFullName { get; set; }

        [DisplayName(nameof(AccountUserName))]
        public string AccountUserName { get; set; }

        public string DashboardSearch { get; set; }
    }
    public class AccountSubscriptionDto : AccountSubscriptionModel
    {
        [DisplayName(nameof(CreatedAt))]
        public new string CreatedAt { get; set; }

        [DisplayName(nameof(LastModifiedAt))]
        public string LastModifiedAt { get; set; }

        [DisplayName(nameof(Account))]
        public new AccountDto Account { get; set; }

        [DisplayName(nameof(Season))]
        public new SeasonDto Season { get; set; }

        [DisplayName(nameof(Subscription))]
        public new SubscriptionDto Subscription { get; set; }
    }

    public enum AccountSubscriptionReturnPageEnum
    {
        Index = 1,
        AccountProfile = 2
    }
}
