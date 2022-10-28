using Entities.DBModels.AccountModels;
using Entities.DBModels.SubscriptionModels;
using Entities.RequestFeatures;

namespace Entities.CoreServicesModels.AccountModels;

public class AccountSubscriptionParameters : RequestParameters
{
    public int Fk_Account { get; set; }
    public int Fk_Subscription { get; set; }
}
public class AccountSubscriptionModel : BaseEntity
{
    [DisplayName(nameof(Account))]
    [ForeignKey(nameof(Account))]
    public int Fk_Account { get; set; }

    [DisplayName(nameof(Subscription))]
    [ForeignKey(nameof(Subscription))]
    public int Fk_Subscription { get; set; }

    [DisplayName(nameof(StartDate))]
    public DateTime StartDate { get; set; }

    [DisplayName(nameof(EndDate))]
    public DateTime EndDate { get; set; }

    [DisplayName(nameof(IsAction))]
    public bool IsAction { get; set; }
}