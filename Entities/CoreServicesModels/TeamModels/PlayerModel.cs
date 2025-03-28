﻿using Entities.CoreServicesModels.PlayerMarkModels;
using Entities.CoreServicesModels.PlayerStateModels;
using Entities.DBModels.AccountTeamModels;
using Entities.DBModels.TeamModels;
using Entities.Extensions;
using Entities.RequestFeatures;
using static Entities.EnumData.LogicEnumData;

namespace Entities.CoreServicesModels.TeamModels
{
    public class PlayerParameters : RequestParameters
    {
        public int Fk_AccountTeam { get; set; }

        public bool CheckLastTransfer { get; set; }

        [DisplayName(nameof(Team))]
        public int Fk_Team { get; set; }

        [DisplayName(nameof(Team))]
        public List<int> Fk_Teams { get; set; }

        [DisplayName(nameof(PlayerPosition))]
        public int Fk_PlayerPosition { get; set; }

        [DisplayName(nameof(FormationPosition))]
        public int Fk_FormationPosition { get; set; }

        [DisplayName(nameof(_365_PlayerId))]
        public string _365_PlayerId { get; set; }

        [DisplayName("CreatedAt")]
        public DateTime? CreatedAtFrom { get; set; }

        public DateTime? CreatedAtTo { get; set; }

        public List<string> _365_PlayerIds { get; set; }

        public int Fk_TeamGameWeak_Ignored { get; set; }

        public int Fk_TeamGameWeak { get; set; }

        public int Fk_ScoreType { get; set; }

        public bool IncludeScore { get; set; }

        public List<int> Fk_ScoreStatesForSeason { get; set; }

        public List<int> Fk_ScoreStatesForGameWeak { get; set; }

        public int Fk_ScoreStateForCustomOrder { get; set; }

        public bool CustomOrderInSeasonList { get; set; }

        public bool CustomOrderDesc { get; set; }

        public int Fk_Season { get; set; }

        public int Fk_SeasonForScores { get; set; }

        public int Fk_GameWeak { get; set; }

        public List<int> Fk_GameWeaks { get; set; }

        public int Fk_GameWeakForScores { get; set; }

        public List<int> Fk_Players { get; set; }

        public double? BuyPriceFrom { get; set; }

        public double? BuyPriceTo { get; set; }

        public double? SellPriceFrom { get; set; }

        public double? SellPriceTo { get; set; }

        public bool? IsActive { get; set; }

        public bool? InExternalTeam { get; set; }

        public string _365_MatchId { get; set; }
    }

    public class PlayerModel : AuditImageEntity
    {
        [DisplayName(nameof(Name))]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Name { get; set; }

        [DisplayName(nameof(ShortName))]
        public string ShortName { get; set; }

        [DisplayName(nameof(_365_PlayerId))]
        public string _365_PlayerId { get; set; }

        [DisplayName(nameof(Team))]
        public int Fk_Team { get; set; }

        [DisplayName(nameof(Team))]
        public TeamModel Team { get; set; }

        [DisplayName(nameof(PlayerPosition))]
        public int Fk_PlayerPosition { get; set; }

        [DisplayName(nameof(PlayerPosition))]
        public PlayerPositionModel PlayerPosition { get; set; }

        [DisplayName(nameof(FormationPosition))]
        public int? Fk_FormationPosition { get; set; }

        [DisplayName(nameof(FormationPosition))]
        public FormationPositionModel FormationPosition { get; set; }

        [DisplayName(nameof(PlayerNumber))]
        public string PlayerNumber { get; set; }

        [DisplayName(nameof(Age))]
        public int Age { get; set; }

        [DisplayName(nameof(Height))]
        public int Height { get; set; }

        [DisplayName(nameof(Birthdate))]
        public DateTime? Birthdate { get; set; }

        [DisplayName(nameof(Birthdate))]
        public string BirthdateString => Birthdate != null ? Birthdate.Value.ToShortDateTimeString() : null;

        [DisplayName(nameof(BuyPrice))]
        public double BuyPrice { get; set; }

        [DisplayName(nameof(SellPrice))]
        public double SellPrice { get; set; }

        [DisplayName(nameof(IsActive))]
        public bool IsActive { get; set; }

        [DisplayName(nameof(InExternalTeam))]
        public bool InExternalTeam { get; set; }

        public int? Top15 { get; set; }

        public PlayerSeasonScoreStateModel SeasonScoreState { get; set; }

        public bool HaveSeasonScoreState { get; set; }

        public List<PlayerSeasonScoreStateModel> SeasonScoreStates { get; set; }

        public List<PlayerGameWeakScoreStateModel> GameWeakScoreStates { get; set; }

        public PlayerGameWeakScoreStateModel GameWeakScoreState { get; set; }

        public bool HaveGameWeakScoreState { get; set; }

        public IList<TeamModel> NextMatches { get; set; }

        public TeamPlayerType TeamPlayerType { get; set; }

        public TransferTypeEnum? LastTransferTypeEnum { get; set; }

        public List<PlayerMarkModel> PlayerMarks { get; set; }

        [DisplayName(nameof(BuyingCount))]
        public int BuyingCount { get; set; }

        [DisplayName(nameof(SellingCount))]
        public int SellingCount { get; set; }
    }


    public class PlayerCreateOrEditModel
    {
        public PlayerCreateOrEditModel()
        {
            PlayerPrices = new List<PlayerPriceCreateOrEditModel>();
        }

        [DisplayName($"{nameof(Name)}{PropertyAttributeConstants.ArLang}")]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Name { get; set; }

        [DisplayName($"{nameof(ShortName)}{PropertyAttributeConstants.ArLang}")]
        public string ShortName { get; set; }

        [DisplayName(nameof(_365_PlayerId))]
        public string _365_PlayerId { get; set; }

        [DisplayName(nameof(Age))]
        public int Age { get; set; }

        [DisplayName(nameof(Height))]
        public int Height { get; set; }

        [DisplayName(nameof(Birthdate))]
        public DateTime? Birthdate { get; set; }

        [DisplayName(nameof(Team))]
        [ForeignKey(nameof(Team))]
        public int Fk_Team { get; set; }

        [DisplayName(nameof(PlayerPosition))]
        public int Fk_PlayerPosition { get; set; }

        [DisplayName(nameof(FormationPosition))]
        public int? Fk_FormationPosition { get; set; }

        [DisplayName(nameof(PlayerNumber))]
        public string PlayerNumber { get; set; }


        [DisplayName(nameof(ImageUrl))]
        public string ImageUrl { get; set; }

        [DisplayName(nameof(StorageUrl))]
        [DataType(DataType.Url, ErrorMessage = PropertyAttributeConstants.TypeValidationMsg)]
        [Url]
        public string StorageUrl { get; set; }

        [DisplayName(nameof(IsActive))]
        public bool IsActive { get; set; }

        [DisplayName(nameof(InExternalTeam))]
        public bool InExternalTeam { get; set; }

        [DisplayName(nameof(PlayerPrices))]
        public List<PlayerPriceCreateOrEditModel> PlayerPrices { get; set; }

        public PlayerLangModel PlayerLang { get; set; }
    }

    public class PlayerLangModel
    {
        [DisplayName($"{nameof(Name)}{PropertyAttributeConstants.EnLang}")]
        [Required(ErrorMessage = PropertyAttributeConstants.RequiredMsg)]
        public string Name { get; set; }

        [DisplayName($"{nameof(ShortName)}{PropertyAttributeConstants.EnLang}")]
        public string ShortName { get; set; }
    }

    public class PlayerCustomStateResult
    {
        public double BuyingPrice { get; set; }
        public double SellingPrice { get; set; }
        public double BuyingCount { get; set; }
        public double SellingCount { get; set; }
        public double PlayerSelection { get; set; }
        public double PlayerCaptain { get; set; }
    }

    public class PlayerModelForRandomTeam
    {
        public int Fk_Player { get; set; }
        public int Fk_PlayerPosition { get; set; }
        public int? Fk_FormationPosition { get; set; }
        public int Fk_Team { get; set; }
        public double BuyPrice { get; set; }
        public double TotalPoints { get; set; }
    }
}
