using Entities.CoreServicesModels.SeasonModels;
using Entities.CoreServicesModels.SubscriptionModels;
using Entities.DBModels.AccountModels;
using Entities.DBModels.SeasonModels;
using Entities.DBModels.SubscriptionModels;
using Entities.RequestFeatures;

namespace Entities.CoreServicesModels.AccountModels;

public class AccountSubscriptionParameters : RequestParameters
{
    public int NotEqualSubscriptionId { get; set; }
    public int Fk_Account { get; set; }
    public int Fk_Subscription { get; set; }
    public int Fk_Season { get; set; }

    public bool? IsAction { get; set; }

    public bool? IsActive { get; set; }

    public string Order_id { get; set; }
    public string DashboardSearch { get; set; }

    public DateTime? CreatedAtFrom { get; set; }
    public DateTime? CreatedAtTo { get; set; }
}
public class AccountSubscriptionModel : BaseEntity
{
    [DisplayName(nameof(Account))]
    [ForeignKey(nameof(Account))]
    public int Fk_Account { get; set; }

    public AccountModel Account { get; set; }

    [DisplayName(nameof(Subscription))]
    [ForeignKey(nameof(Subscription))]
    public int Fk_Subscription { get; set; }

    public SubscriptionModel Subscription { get; set; }

    [DisplayName(nameof(Season))]
    [ForeignKey(nameof(Season))]
    public int Fk_Season { get; set; }

    [DisplayName(nameof(Season))]
    public SeasonModel Season { get; set; }

    [DisplayName(nameof(IsAction))]
    public bool IsAction { get; set; }

    [DisplayName(nameof(IsActive))]
    public bool IsActive { get; set; }

    [DisplayName(nameof(Order_id))]
    public string Order_id { get; set; }

    [DisplayName(nameof(Cost))]
    public int Cost { get; set; }
}

public class AccountSubscriptionCreateOrEditModel
{
    [DisplayName(nameof(Account))]
    [ForeignKey(nameof(Account))]
    public int Fk_Account { get; set; }

    [DisplayName(nameof(Subscription))]
    [ForeignKey(nameof(Subscription))]
    public int Fk_Subscription { get; set; }

    [DisplayName(nameof(Season))]
    [ForeignKey(nameof(Season))]
    public int Fk_Season { get; set; }

    [DisplayName(nameof(IsAction))]
    public bool IsAction { get; set; }

    [DisplayName(nameof(IsActive))]
    public bool IsActive { get; set; }

    [DisplayName(nameof(Cost))]
    public int Cost { get; set; }
}