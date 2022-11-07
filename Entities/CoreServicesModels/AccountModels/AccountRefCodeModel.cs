using Entities.RequestFeatures;

namespace Entities.CoreServicesModels.AccountModels
{
    public class AccountRefCodeParameters : RequestParameters
    {
        public int Fk_RefAccount { get; set; }

        public string RefCode { get; set; }
    }

    public class AccountRefCodeModel : BaseEntity
    {
        [DisplayName(nameof(NewAccount))]
        [ForeignKey(nameof(NewAccount))]
        public int Fk_NewAccount { get; set; }

        [DisplayName(nameof(NewAccount))]
        public AccountModel NewAccount { get; set; }

        [DisplayName(nameof(RefAccount))]
        [ForeignKey(nameof(RefAccount))]
        public int Fk_RefAccount { get; set; }

        [DisplayName(nameof(RefAccount))]
        public AccountModel RefAccount { get; set; }
    }
}
