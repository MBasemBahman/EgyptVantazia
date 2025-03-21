﻿using Entities.DBModels.AccountModels;
using Entities.DBModels.MatchStatisticModels;
using Entities.DBModels.SeasonModels;
using Entities.DBModels.StandingsModels;

namespace Entities.DBModels.TeamModels
{
    public class Team : AuditImageEntity, ILookUpEntity
    {
        [DisplayName($"{nameof(Name)}{PropertyAttributeConstants.ArLang}")]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Name { get; set; }

        [DisplayName($"{nameof(ShortName)}{PropertyAttributeConstants.ArLang}")]
        public string ShortName { get; set; }

        [DisplayName(nameof(_365_TeamId))]
        public string _365_TeamId { get; set; }

        [DisplayName(nameof(ShirtImageUrl))]
        public string ShirtImageUrl { get; set; }

        [DisplayName(nameof(ShirtStorageUrl))]
        [DataType(DataType.Url, ErrorMessage = PropertyAttributeConstants.TypeValidationMsg)]
        [Url]
        public string ShirtStorageUrl { get; set; }

        [DisplayName(nameof(IsActive))]
        public bool IsActive { get; set; }

        [DisplayName(nameof(Season))]
        [ForeignKey(nameof(Season))]
        public int Fk_Season { get; set; }

        [DisplayName(nameof(Season))]
        public Season Season { get; set; }

        [DisplayName(nameof(Players))]
        public IList<Player> Players { get; set; }

        [DisplayName(nameof(PlayerPrices))]
        public IList<PlayerPrice> PlayerPrices { get; set; }

        [DisplayName(nameof(HomeGameWeaks))]
        public IList<TeamGameWeak> HomeGameWeaks { get; set; }

        [DisplayName(nameof(AwayGameWeaks))]
        public IList<TeamGameWeak> AwayGameWeaks { get; set; }

        [DisplayName(nameof(AccountFavourites))]
        public IList<Account> AccountFavourites { get; set; }

        [DisplayName(nameof(Standings))]
        public IList<Standings> Standings { get; set; }

        [DisplayName(nameof(MatchStatisticScores))]
        public List<MatchStatisticScore> MatchStatisticScores { get; set; }

        public TeamLang TeamLang { get; set; }
    }

    public class TeamLang : LangEntity<Team>
    {
        [DisplayName($"{nameof(Name)}{PropertyAttributeConstants.EnLang}")]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Name { get; set; }

        [DisplayName($"{nameof(ShortName)}{PropertyAttributeConstants.EnLang}")]
        public string ShortName { get; set; }
    }
}
