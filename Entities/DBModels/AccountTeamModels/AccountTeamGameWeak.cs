﻿using Entities.DBModels.SeasonModels;

namespace Entities.DBModels.AccountTeamModels
{
    public class AccountTeamGameWeak : AuditEntity
    {
        [DisplayName(nameof(AccountTeam))]
        [ForeignKey(nameof(AccountTeam))]
        public int Fk_AccountTeam { get; set; }

        [DisplayName(nameof(AccountTeam))]
        public AccountTeam AccountTeam { get; set; }

        [DisplayName(nameof(GameWeak))]
        [ForeignKey(nameof(GameWeak))]
        public int Fk_GameWeak { get; set; }

        [DisplayName(nameof(GameWeak))]
        public GameWeak GameWeak { get; set; }

        #region Cards

        [DisplayName(nameof(BenchBoost))]
        public bool BenchBoost { get; set; }

        [DisplayName(nameof(FreeHit))]
        public bool FreeHit { get; set; }

        [DisplayName(nameof(WildCard))]
        public bool WildCard { get; set; }

        [DisplayName(nameof(DoubleGameWeak))]
        public bool DoubleGameWeak { get; set; }

        [DisplayName(nameof(Top_11))]
        public bool Top_11 { get; set; }

        [DisplayName(nameof(TripleCaptain))]
        public bool TripleCaptain { get; set; }

        #endregion

        #region Calculations

        [DisplayName(nameof(TotalPoints))]
        public int? TotalPoints { get; set; }

        [DisplayName(nameof(SeasonTotalPoints))]
        public double SeasonTotalPoints { get; set; }

        [DisplayName(nameof(PrevPoints))]
        public int PrevPoints { get; set; }

        [DisplayName(nameof(TansfarePoints))]
        public int TansfarePoints { get; set; }

        [DisplayName(nameof(BenchPoints))]
        public int BenchPoints { get; set; }

        #endregion

        #region Ranking

        [DisplayName(nameof(SeasonGlobalRanking))]
        public double SeasonGlobalRanking { get; set; }

        [DisplayName(nameof(GlobalRanking))]
        public double GlobalRanking { get; set; }

        [DisplayName(nameof(CountryRanking))]
        public double CountryRanking { get; set; }

        [DisplayName(nameof(FavouriteTeamRanking))]
        public double FavouriteTeamRanking { get; set; }

        #endregion
    }
}
