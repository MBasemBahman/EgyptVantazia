using Entities.DBModels.AccountModels;

namespace Entities.DBModels.PrivateLeagueModels
{
    public class PrivateLeagueMember : AuditEntity
    {
        [DisplayName(nameof(Account))]
        [ForeignKey(nameof(Account))]
        public int Fk_Account { get; set; }

        [DisplayName(nameof(Account))]
        public Account Account { get; set; }

        [DisplayName(nameof(PrivateLeague))]
        [ForeignKey(nameof(PrivateLeague))]
        public int Fk_PrivateLeague { get; set; }

        [DisplayName(nameof(PrivateLeague))]
        public PrivateLeague PrivateLeague { get; set; }

        [DisplayName(nameof(IsAdmin))]
        public bool IsAdmin { get; set; }
    }
}
