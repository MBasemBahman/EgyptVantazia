using Entities.DBModels.AccountModels;

namespace Entities.DBModels.LocationModels
{
    [Index(nameof(Name), IsUnique = true)]
    public class Country : AuditImageEntity, ILookUpEntity
    {
        [DisplayName($"{nameof(Name)}{PropertyAttributeConstants.ArLang}")]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Name { get; set; }

        [DisplayName(nameof(Order))]
        public int Order { get; set; }

        [DisplayName(nameof(Accounts))]
        public IList<Account> Accounts { get; set; }

        [DisplayName(nameof(AccountNationalities))]
        public IList<Account> AccountNationalities { get; set; }

        public CountryLang CountryLang { get; set; }
    }

    public class CountryLang : LangEntity<Country>
    {
        [DisplayName($"{nameof(Name)}{PropertyAttributeConstants.EnLang}")]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Name { get; set; }
    }
}
