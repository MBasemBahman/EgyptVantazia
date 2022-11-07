namespace Entities.DBModels.AccountModels
{
    public class AccountRefCode : BaseEntity
    {
        [DisplayName(nameof(NewAccount))]
        [ForeignKey(nameof(NewAccount))]
        public int Fk_NewAccount { get; set; }

        [DisplayName(nameof(NewAccount))]
        public Account NewAccount { get; set; }

        [DisplayName(nameof(RefAccount))]
        [ForeignKey(nameof(RefAccount))]
        public int Fk_RefAccount { get; set; }

        [DisplayName(nameof(RefAccount))]
        public Account RefAccount { get; set; }
    }
}
