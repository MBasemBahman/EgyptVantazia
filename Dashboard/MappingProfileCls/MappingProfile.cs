using Dashboard.Areas.AccountEntity.Models;
using Dashboard.Areas.AccountSubscriptionEntity.Models;
using Dashboard.Areas.AccountTeamEntity.Models;
using Dashboard.Areas.AppInfoEntity.Models;
using Dashboard.Areas.DashboardAdministration.Models;
using Dashboard.Areas.Location.Models;
using Dashboard.Areas.LogEntity.Models;
using Dashboard.Areas.NewsEntity.Models;
using Dashboard.Areas.NotificationEntity.Models;
using Dashboard.Areas.PaymentEntity.Models;
using Dashboard.Areas.PlayerMarkEntity.Models;
using Dashboard.Areas.PlayerScoreEntity.Models;
using Dashboard.Areas.PlayerStateEntity.Models;
using Dashboard.Areas.PlayerTransferEntity.Models;
using Dashboard.Areas.PrivateLeagueEntity.Models;
using Dashboard.Areas.SeasonEntity.Models;
using Dashboard.Areas.SponsorEntity.Models;
using Dashboard.Areas.StandingsEntity.Models;
using Dashboard.Areas.SubscriptionEntity.Models;
using Dashboard.Areas.TeamEntity.Models;
using Dashboard.Areas.UserEntity.Models;
using Dashboard.Areas.PromoCodeEntity.Models;
using Dashboard.Areas.MatchStatisticEntity.Models;
using Entities.CoreServicesModels.AccountModels;
using Entities.CoreServicesModels.AccountTeamModels;
using Entities.CoreServicesModels.AppInfoModels;
using Entities.CoreServicesModels.LocationModels;
using Entities.CoreServicesModels.LogModels;
using Entities.CoreServicesModels.NewsModels;
using Entities.CoreServicesModels.NotificationModels;
using Entities.CoreServicesModels.PlayerMarkModels;
using Entities.CoreServicesModels.PlayerScoreModels;
using Entities.CoreServicesModels.PlayerStateModels;
using Entities.CoreServicesModels.PlayerTransfersModels;
using Entities.CoreServicesModels.PrivateLeagueModels;
using Entities.CoreServicesModels.SeasonModels;
using Entities.CoreServicesModels.SponsorModels;
using Entities.CoreServicesModels.StandingsModels;
using Entities.CoreServicesModels.SubscriptionModels;
using Entities.CoreServicesModels.TeamModels;
using Entities.CoreServicesModels.UserModels;
using Entities.CoreServicesModels.PromoCodeModels;
using Entities.CoreServicesModels.MatchStatisticModels;
using Entities.DBModels.AccountModels;
using Entities.DBModels.AccountTeamModels;
using Entities.DBModels.AppInfoModels;
using Entities.DBModels.DashboardAdministrationModels;
using Entities.DBModels.LocationModels;
using Entities.DBModels.NewsModels;
using Entities.DBModels.NotificationModels;
using Entities.DBModels.PlayerMarkModels;
using Entities.DBModels.PlayerScoreModels;
using Entities.DBModels.PlayerStateModels;
using Entities.DBModels.PrivateLeagueModels;
using Entities.DBModels.SeasonModels;
using Entities.DBModels.SponsorModels;
using Entities.DBModels.StandingsModels;
using Entities.DBModels.SubscriptionModels;
using Entities.DBModels.TeamModels;
using Entities.DBModels.PromoCodeModels;
using Entities.DBModels.MatchStatisticModels;
using Entities.RequestFeatures;
using AccountTeamGameWeakDto = Dashboard.Areas.AccountTeamEntity.Models.AccountTeamGameWeakDto;

namespace Dashboard.MappingProfileCls
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            MapperConfiguration configuration = new(cfg =>
            {
                cfg.AllowNullCollections = false;
            });

            CreateMap<DateTime, string>().ConvertUsing(new DateTimeTypeConverter());
            CreateMap<DateTime?, string>().ConvertUsing(new DateTimeNullableTypeConverter());
            CreateMap<TimeSpan, string>().ConvertUsing(new TimeSpanTypeConverter());
            CreateMap<TimeSpan?, string>().ConvertUsing(new TimeSpanNullableTypeConverter());
            CreateMap<string, List<string>>().ConvertUsing(new ListOfStringTypeConverter());

            _ = CreateMap<DtParameters, RequestParameters>()
                .ForMember(dest => dest.SearchTerm, opt => opt.MapFrom(src => src.Search == null ? "" : src.Search.Value))
                .ForMember(dest => dest.OrderBy, opt =>
                                   opt.MapFrom(src => src.Order == null ?
                                                      "" :
                                                      (src.Order[0].Dir.ToString().ToLower() == "asc" ?
                                                       src.Columns[src.Order[0].Column].Data :
                                                       (src.Columns[src.Order[0].Column].Data.Contains(',') ?
                                                        src.Columns[src.Order[0].Column].Data.Replace(",", " desc,") :
                                                        src.Columns[src.Order[0].Column].Data + " desc"))))
                .ForMember(dest => dest.PageSize, opt => opt.MapFrom(src => src.Length))
                .ForMember(dest => dest.PageNumber, opt => opt.MapFrom(src => src.Length > 0 ? (src.Start / src.Length) + 1 : 0))
                .IncludeAllDerived();

            _ = CreateMap<UserAuthenticatedDto, UserDto>();


            #region Account Models

            #region Account
            _ = CreateMap<Account, AccountCreateModel>();

            _ = CreateMap<AccountCreateModel, Account>();

            _ = CreateMap<AccountModel, AccountDto>();

            _ = CreateMap<AccountFilter, AccountParameters>();

            #endregion

            #region AccountSubscription
            _ = CreateMap<AccountSubscription, AccountSubscriptionCreateOrEditModel>();

            _ = CreateMap<AccountSubscriptionCreateOrEditModel, AccountSubscription>();

            _ = CreateMap<AccountSubscriptionModel, AccountSubscriptionDto>();

            _ = CreateMap<AccountSubscriptionFilter, AccountSubscriptionParameters>();

            #endregion

            #region Payment

            _ = CreateMap<PaymentModel, PaymentDto>();

            _ = CreateMap<PaymentFilter, PaymentParameters>();

            #endregion

            #endregion

            #region Log Models
            _ = CreateMap<LogModel, LogDto>();

            _ = CreateMap<LogFilter, LogParameters>();

            #endregion

            #region User Models

            _ = CreateMap<UserFilter, UserParameters>();

            _ = CreateMap<UserModel, UserDto>();

            _ = CreateMap<User, UserCreateModel>();

            _ = CreateMap<UserCreateModel, User>();

            _ = CreateMap<UserDeviceFilter, UserDeviceParameters>();

            _ = CreateMap<UserDeviceModel, UserDeviceDto>();

            _ = CreateMap<RefreshTokenFilter, RefreshTokenParameters>();

            _ = CreateMap<RefreshTokenModel, RefreshTokenDto>();

            _ = CreateMap<VerificationFilter, VerificationParameters>();

            _ = CreateMap<VerificationModel, Areas.UserEntity.Models.VerificationDto>();

            _ = CreateMap<UserFilter, UserParameters>();

            #endregion

            #region Dashboard Administration Models

            #region Dashboard View
            _ = CreateMap<DashboardViewModel, DashboardViewDto>();

            _ = CreateMap<DashboardView, DashboardViewCreateOrEditModel>();

            _ = CreateMap<DashboardViewLang, DashboardViewLangModel>();

            _ = CreateMap<DashboardViewCreateOrEditModel, DashboardView>();

            _ = CreateMap<DashboardViewLangModel, DashboardViewLang>();
            #endregion

            #region Dashboard Administration Role
            _ = CreateMap<DashboardAdministrationRoleModel, DashboardAdministrationRoleDto>();

            _ = CreateMap<DashboardAdministrationRole, DashboardAdministrationRoleCreateOrEditModel>();

            _ = CreateMap<DashboardAdministrationRoleLang, DashboardAdministrationRoleLangModel>();

            _ = CreateMap<DashboardAdministrationRoleCreateOrEditModel, DashboardAdministrationRole>();

            _ = CreateMap<DashboardAdministrationRoleLangModel, DashboardAdministrationRoleLang>();

            _ = CreateMap<DashboardAdministrationRoleFilter, DashboardAdministrationRoleRequestParameters>();
            #endregion

            #region Dashboard Access Level
            _ = CreateMap<DashboardAccessLevelModel, DashboardAccessLevelDto>();

            _ = CreateMap<DashboardAccessLevel, DashboardAccessLevelCreateOrEditModel>();

            _ = CreateMap<DashboardAccessLevelLang, DashboardAccessLevelLangModel>();

            _ = CreateMap<DashboardAccessLevelCreateOrEditModel, DashboardAccessLevel>();

            _ = CreateMap<DashboardAccessLevelLangModel, DashboardAccessLevelLang>();
            #endregion

            #region Dashboard Administrator
            _ = CreateMap<DashboardAdministrator, DashboardAdministratorCreateOrEditModel>();

            _ = CreateMap<DashboardAdministratorCreateOrEditModel, DashboardAdministrator>();

            _ = CreateMap<DashboardAdministratorModel, DashboardAdministratorDto>();

            _ = CreateMap<DashboardAdministratorFilter, DashboardAdministratorParameters>();
            #endregion
            #endregion

            #region Location

            #region Country

            _ = CreateMap<CountryModel, CountryDto>();

            _ = CreateMap<Country, CountryCreateOrEditModel>();



            _ = CreateMap<CountryCreateOrEditModel, Country>()
                      .ForMember(dest => dest.ImageUrl, opt => opt.Ignore())
                .ForMember(dest => dest.StorageUrl, opt => opt.Ignore());

            _ = CreateMap<CountryLangModel, CountryLang>();

            _ = CreateMap<CountryLang, CountryLangModel>();
            #endregion

            #endregion

            #region App Info Models

            #region AppAbout

            _ = CreateMap<AppAboutModel, AppAboutDto>();

            _ = CreateMap<AppAbout, AppAboutCreateOrEditModel>();

            _ = CreateMap<AppAboutLang, AppAboutLangModel>();

            _ = CreateMap<AppAboutCreateOrEditModel, AppAbout>();

            _ = CreateMap<AppAboutLangModel, AppAboutLang>();
            #endregion

            #endregion

            #region Team Models

            #region Team
            _ = CreateMap<Team, TeamCreateOrEditModel>();

            _ = CreateMap<TeamCreateOrEditModel, Team>()
                .ForMember(dest => dest.ShirtImageUrl, opt => opt.Ignore())
                .ForMember(dest => dest.ShirtStorageUrl, opt => opt.Ignore())
                .ForMember(dest => dest.ImageUrl, opt => opt.Ignore())
                .ForMember(dest => dest.StorageUrl, opt => opt.Ignore());

            _ = CreateMap<TeamModel, TeamDto>();

            _ = CreateMap<TeamFilter, TeamParameters>();

            _ = CreateMap<TeamLangModel, TeamLang>();

            _ = CreateMap<TeamLang, TeamLangModel>();

            #endregion

            #region Player Position
            _ = CreateMap<PlayerPosition, PlayerPositionCreateOrEditModel>();

            _ = CreateMap<PlayerPositionCreateOrEditModel, PlayerPosition>()
                .ForMember(dest => dest.ImageUrl, opt => opt.Ignore())
                .ForMember(dest => dest.StorageUrl, opt => opt.Ignore());

            _ = CreateMap<PlayerPositionModel, PlayerPositionDto>();
            _ = CreateMap<FormationPositionModel, FormationPositionDto>();

            _ = CreateMap<PlayerPositionFilter, PlayerPositionParameters>();

            _ = CreateMap<PlayerPositionLangModel, PlayerPositionLang>();

            _ = CreateMap<PlayerPositionLang, PlayerPositionLangModel>();

            #endregion

            #region Player
            _ = CreateMap<Player, PlayerCreateOrEditModel>()
                .ForMember(dest => dest.PlayerPrices, opt => opt.Ignore());

            _ = CreateMap<PlayerCreateOrEditModel, Player>()
                .ForMember(dest => dest.PlayerPrices, opt => opt.Ignore())
                .ForMember(dest => dest.ImageUrl, opt => opt.Ignore())
                .ForMember(dest => dest.StorageUrl, opt => opt.Ignore());

            _ = CreateMap<PlayerModel, PlayerDto>();

            _ = CreateMap<PlayerFilter, PlayerParameters>();

            _ = CreateMap<PlayerLangModel, PlayerLang>();

            _ = CreateMap<PlayerLang, PlayerLangModel>();

            #endregion

            #region Player Price
            _ = CreateMap<PlayerPriceModel, PlayerPriceDto>();
            _ = CreateMap<PlayerPriceModel, PlayerPriceCreateOrEditModel>();

            _ = CreateMap<PlayerPrice, PlayerPriceEditModel>();
            _ = CreateMap<PlayerPriceEditModel, PlayerPrice>()
                .ForMember(dest => dest.Player, opt => opt.Ignore())
                .ForMember(dest => dest.Team, opt => opt.Ignore());
            #endregion


            #endregion

            #region News Models

            #region News
            _ = CreateMap<News, NewsCreateOrEditModel>();

            _ = CreateMap<NewsCreateOrEditModel, News>()
                .ForMember(dest => dest.ImageUrl, opt => opt.Ignore())
                .ForMember(dest => dest.StorageUrl, opt => opt.Ignore());

            _ = CreateMap<NewsModel, NewsDto>();

            _ = CreateMap<NewsFilter, NewsParameters>();

            _ = CreateMap<NewsLangModel, NewsLang>();

            _ = CreateMap<NewsLang, NewsLangModel>();

            #endregion

            #region News Attachment
            _ = CreateMap<NewsAttachmentModel, NewsAttachmentDto>();

            _ = CreateMap<NewsAttachmentFilter, NewsAttachmentParameters>();
            #endregion

            #endregion

            #region Sponsor Models

            #region Sponsor
            _ = CreateMap<Sponsor, SponsorCreateOrEditModel>()
                .ForMember(a => a.SponsorViews, opt => opt.Ignore());

            _ = CreateMap<SponsorCreateOrEditModel, Sponsor>()
                .ForMember(dest => dest.SponsorViews, opt => opt.Ignore())
                .ForMember(dest => dest.ImageUrl, opt => opt.Ignore())
                .ForMember(dest => dest.StorageUrl, opt => opt.Ignore());

            _ = CreateMap<SponsorModel, SponsorDto>();

            _ = CreateMap<SponsorFilter, SponsorParameters>();

            _ = CreateMap<SponsorLangModel, SponsorLang>();

            _ = CreateMap<SponsorLang, SponsorLangModel>();

            #endregion

            #endregion

            #region Account Team Models

            #region TeamPlayerType
            _ = CreateMap<TeamPlayerType, TeamPlayerTypeCreateOrEditModel>();

            _ = CreateMap<TeamPlayerTypeCreateOrEditModel, TeamPlayerType>();

            _ = CreateMap<TeamPlayerTypeModel, TeamPlayerTypeDto>();

            _ = CreateMap<TeamPlayerTypeFilter, RequestParameters>();

            _ = CreateMap<TeamPlayerTypeLangModel, TeamPlayerTypeLang>();

            _ = CreateMap<TeamPlayerTypeLang, TeamPlayerTypeLangModel>();

            #endregion
            
            #region CommunicationStatus
            
            _ = CreateMap<CommunicationStatus, CommunicationStatusCreateOrEditModel>();

            _ = CreateMap<CommunicationStatusCreateOrEditModel, CommunicationStatus>();

            _ = CreateMap<CommunicationStatusModel, CommunicationStatusDto>();

            _ = CreateMap<CommunicationStatusFilter, RequestParameters>();

            #endregion

            #region Account Team

            _ = CreateMap<AccountTeamModel, AccountTeamDto>();

            _ = CreateMap<AccountTeamFilter, AccountTeamParameters>();

            _ = CreateMap<AccountTeam, AccountTeamCreateOrEditModel>();

            _ = CreateMap<AccountTeamCreateOrEditModel, AccountTeam>();

            #endregion

            #region Player Transfer

            _ = CreateMap<PlayerTransferModel, PlayerTransferDto>();

            _ = CreateMap<PlayerTransferDto, PlayerTransferModel>();

            _ = CreateMap<PlayerTransferFilter, PlayerTransferParameters>();

            #endregion
            
            #region Account Team GameWeak

            _ = CreateMap<AccountTeamGameWeakModel,
                Dashboard.Areas.AccountTeamGameWeakEntity.Models.AccountTeamGameWeakDto>();

            _ = CreateMap<Dashboard.Areas.AccountTeamGameWeakEntity.Models.AccountTeamGameWeakDto,
                AccountTeamGameWeakModel>();

            _ = CreateMap<Dashboard.Areas.AccountTeamGameWeakEntity.Models.AccountTeamGameWeakFilter,
                AccountTeamGameWeakParameters>();

            #endregion

            #region Account Team Game Weak
            _ = CreateMap<AccountTeamGameWeakModel, AccountTeamGameWeakDto>();
            #endregion

            #region Account Team Player
            _ = CreateMap<AccountTeamPlayerModel, AccountTeamPlayerDto>();
            #endregion

            #region Account Team Player Game Weak
            _ = CreateMap<AccountTeamPlayerGameWeakModel, AccountTeamPlayerGameWeakDto>();
            #endregion

            #endregion

            #region Private League Models

            #region PrivateLeague

            _ = CreateMap<PrivateLeague, PrivateLeagueCreateOrEditModel>();

            _ = CreateMap<PrivateLeagueCreateOrEditModel, PrivateLeague>();

            _ = CreateMap<PrivateLeagueModel, PrivateLeagueDto>();

            _ = CreateMap<PrivateLeagueFilter, PrivateLeagueParameters>();

            #endregion

            #region PrivateLeagueMember
            _ = CreateMap<PrivateLeagueMemberModel, PrivateLeagueMemberDto>();

            _ = CreateMap<PrivateLeagueMemberFilter, PrivateLeagueMemberParameters>();

            #endregion

            #endregion

            #region Player Score Models

            #region ScoreType
            _ = CreateMap<ScoreType, ScoreTypeCreateOrEditModel>();

            _ = CreateMap<ScoreTypeCreateOrEditModel, ScoreType>();

            _ = CreateMap<ScoreTypeModel, ScoreTypeDto>();

            _ = CreateMap<ScoreTypeFilter, ScoreTypeParameters>();

            _ = CreateMap<ScoreTypeLangModel, ScoreTypeLang>();

            _ = CreateMap<ScoreTypeLang, ScoreTypeLangModel>();
            #endregion

            #region PlayerGameWeak
            _ = CreateMap<PlayerGameWeak, PlayerGameWeakCreateOrEditModel>()
                .ForMember(dest => dest.PlayerGameWeakScores, opt => opt.Ignore());

            _ = CreateMap<PlayerGameWeakCreateOrEditModel, PlayerGameWeak>()
                .ForMember(dest => dest.PlayerGameWeakScores, opt => opt.Ignore());

            _ = CreateMap<PlayerGameWeakModel, PlayerGameWeakDto>();

            _ = CreateMap<PlayerGameWeakFilter, PlayerGameWeakParameters>();

            #endregion

            #region  PlayerGameWeakScore
            _ = CreateMap<PlayerGameWeakScoreModel, PlayerGameWeakScoreDto>();
            _ = CreateMap<PlayerGameWeakScoreModel, PlayerGameWeakScoreCreateOrEditModel>();

            _ = CreateMap<PlayerGameWeakScoreFilter, PlayerGameWeakScoreParameters>();
            #endregion
            #endregion
            
            #region Player Marks Models

            #region Mark
            _ = CreateMap<Mark, MarkCreateOrEditModel>()
                .ForMember(dest => dest.ImageUrl, opt => opt.Ignore());

            _ = CreateMap<MarkCreateOrEditModel, Mark>()
                .ForMember(dest => dest.ImageUrl, opt => opt.Ignore());

            _ = CreateMap<MarkModel, MarkDto>();

            _ = CreateMap<MarkFilter, MarkParameters>();

            _ = CreateMap<MarkLangModel, MarkLang>();

            _ = CreateMap<MarkLang, MarkLangModel>();
            #endregion
            
            #region PlayerMark
            
            _ = CreateMap<PlayerMark, PlayerMarkCreateOrEditModel>();

            _ = CreateMap<PlayerMarkCreateOrEditModel, PlayerMark>();

            CreateMap<PlayerMarkLangModel, PlayerMarkLang>();

            CreateMap<PlayerMarkLang, PlayerMarkLangModel>();

            _ = CreateMap<PlayerMarkModel, PlayerMarkDto>();

            _ = CreateMap<PlayerMarkFilter, PlayerMarkParameters>();
            
            #endregion

            #endregion

            #region Season Models

            #region Season
            _ = CreateMap<Season, SeasonCreateOrEditModel>()
                .ForMember(dest => dest.GameWeaks, opt => opt.Ignore());

            _ = CreateMap<SeasonCreateOrEditModel, Season>()
                .ForMember(dest => dest.GameWeaks, opt => opt.Ignore())
                .ForMember(dest => dest.ImageUrl, opt => opt.Ignore())
                .ForMember(dest => dest.StorageUrl, opt => opt.Ignore());

            _ = CreateMap<SeasonModel, SeasonDto>();

            _ = CreateMap<SeasonFilter, SeasonParameters>();

            _ = CreateMap<SeasonLangModel, SeasonLang>();

            _ = CreateMap<SeasonLang, SeasonLangModel>();

            #endregion

            #region  Game Weak
            _ = CreateMap<GameWeakModel, GameWeakDto>();
            _ = CreateMap<GameWeak, GameWeakCreateOrEditModel>()
                .ForMember(dest => dest.NameEn, opt => opt.MapFrom(src => src.GameWeakLang.Name));
            #endregion


            #region Team Game Weak
            _ = CreateMap<TeamGameWeak, TeamGameWeakCreateOrEditModel>();

            _ = CreateMap<TeamGameWeakCreateOrEditModel, TeamGameWeak>();

            _ = CreateMap<TeamGameWeakModel, TeamGameWeakDto>();

            _ = CreateMap<TeamGameWeakFilter, TeamGameWeakParameters>();
            #endregion

            #endregion

            #region Standings Models

            #region Standings
            _ = CreateMap<Standings, StandingsCreateOrEditModel>();

            _ = CreateMap<StandingsCreateOrEditModel, Standings>();

            _ = CreateMap<StandingsModel, StandingsDto>();

            _ = CreateMap<StandingsFilter, StandingsParameters>();

            #endregion

            #endregion

            #region Player Transfer Models
            _ = CreateMap<PlayerTransferModel, PlayerTransferDto>();
            #endregion

            #region Subscription Models

            #region Subscription

            _ = CreateMap<Subscription, SubscriptionCreateOrEditModel>();

            _ = CreateMap<SubscriptionCreateOrEditModel, Subscription>()
                .ForMember(dest => dest.ImageUrl, opt => opt.Ignore())
                .ForMember(dest => dest.StorageUrl, opt => opt.Ignore());

            _ = CreateMap<SubscriptionModel, SubscriptionDto>();

            _ = CreateMap<SubscriptionFilter, SubscriptionParameters>();

            _ = CreateMap<SubscriptionLangModel, SubscriptionLang>();

            _ = CreateMap<SubscriptionLang, SubscriptionLangModel>();

            #endregion

            #endregion

            #region ScoreState Model

            #region ScoreState
            _ = CreateMap<ScoreState, ScoreStateCreateOrEditModel>();

            _ = CreateMap<ScoreStateCreateOrEditModel, ScoreState>();

            _ = CreateMap<ScoreStateModel, ScoreStateDto>();

            _ = CreateMap<ScoreStateFilter, ScoreStateParameters>();

            _ = CreateMap<ScoreStateLangModel, ScoreStateLang>();

            _ = CreateMap<ScoreStateLang, ScoreStateLangModel>();

            #endregion

            #region PlayerGameWeakScoreState
            _ = CreateMap<PlayerGameWeakScoreState, PlayerGameWeakScoreStateCreateOrEditModel>();

            _ = CreateMap<PlayerGameWeakScoreStateCreateOrEditModel, PlayerGameWeakScoreState>();

            _ = CreateMap<PlayerGameWeakScoreStateModel, PlayerGameWeakScoreStateDto>();

            _ = CreateMap<PlayerGameWeakScoreStateFilter, PlayerGameWeakScoreStateParameters>();

            #endregion

            #region PlayerSeasonScoreState
            _ = CreateMap<PlayerSeasonScoreState, PlayerSeasonScoreStateCreateOrEditModel>();

            _ = CreateMap<PlayerSeasonScoreStateCreateOrEditModel, PlayerSeasonScoreState>();

            _ = CreateMap<PlayerSeasonScoreStateModel, PlayerSeasonScoreStateDto>();

            _ = CreateMap<PlayerSeasonScoreStateFilter, PlayerSeasonScoreStateParameters>();

            #endregion

            #endregion

            #region Notification
            _ = CreateMap<Notification, NotificationCreateOrEditModel>();

            _ = CreateMap<NotificationCreateOrEditModel, Notification>()
                .ForMember(dest => dest.ImageUrl, opt => opt.Ignore())
                .ForMember(dest => dest.StorageUrl, opt => opt.Ignore());

            _ = CreateMap<NotificationModel, NotificationDto>();

            _ = CreateMap<NotificationFilter, NotificationParameters>();

            _ = CreateMap<NotificationLangModel, NotificationLang>();

            _ = CreateMap<NotificationLang, NotificationLangModel>();

            #endregion

            #region ScoreStype

            _ = CreateMap<PlayerGameWeakScoreDto, PlayerGameWeakScoreCreateDto>();

            #endregion

            #region Match Statistics

            #region StatisticCategory
            _ = CreateMap<StatisticCategory, StatisticCategoryCreateOrEditModel>();

            _ = CreateMap<StatisticCategoryCreateOrEditModel, StatisticCategory>();

            _ = CreateMap<StatisticCategoryModel, StatisticCategoryDto>();

            _ = CreateMap<StatisticCategoryFilter, StatisticCategoryParameters>();

            _ = CreateMap<StatisticCategoryLangModel, StatisticCategoryLang>();

            _ = CreateMap<StatisticCategoryLang, StatisticCategoryLangModel>();

            #endregion

            #region StatisticScore
            _ = CreateMap<StatisticScore, StatisticScoreCreateOrEditModel>();

            _ = CreateMap<StatisticScoreCreateOrEditModel, StatisticScore>();

            _ = CreateMap<StatisticScoreModel, StatisticScoreDto>();

            _ = CreateMap<StatisticScoreFilter, StatisticScoreParameters>();

            _ = CreateMap<StatisticScoreLangModel, StatisticScoreLang>();

            _ = CreateMap<StatisticScoreLang, StatisticScoreLangModel>();

            #endregion

            #region PlayerSeasonScoreState
            _ = CreateMap<MatchStatisticScore, MatchStatisticScoreCreateOrEditModel>();

            _ = CreateMap<MatchStatisticScoreCreateOrEditModel, MatchStatisticScore>();

            _ = CreateMap<MatchStatisticScoreModel, MatchStatisticScoreDto>();

            _ = CreateMap<MatchStatisticScoreFilter, MatchStatisticScoreParameters>();

            #endregion

            #endregion

            #region PromoCode Models

            #region Promo Code
            _ = CreateMap<PromoCode, PromoCodeCreateOrEditModel>();


            _ = CreateMap<PromoCodeCreateOrEditModel, PromoCode>();

            _ = CreateMap<PromoCodeModel, PromoCodeDto>();

            _ = CreateMap<PromoCodeFilter, PromoCodeParameters>();

            _ = CreateMap<PromoCodeLangModel, PromoCodeLang>();

            _ = CreateMap<PromoCodeLang, PromoCodeLangModel>();

            #endregion

        

            #endregion

        }
    }

    public class DateTimeNullableTypeConverter : ITypeConverter<DateTime?, string>
    {
        public string Convert(DateTime? source, string destination, ResolutionContext context)
        {
            return source == null ? "" : source.Value.ToString(ApiConstants.DateTimeStringFormat);
        }
    }

    public class DateTimeTypeConverter : ITypeConverter<DateTime, string>
    {
        public string Convert(DateTime source, string destination, ResolutionContext context)
        {
            return source.ToString(ApiConstants.DateTimeStringFormat);
        }
    }


    public class TimeSpanNullableTypeConverter : ITypeConverter<TimeSpan?, string>
    {
        public string Convert(TimeSpan? source, string destination, ResolutionContext context)
        {
            return source == null ? "" : new DateTime(source.Value.Ticks).ToString(ApiConstants.TimeFormat);
        }
    }

    public class TimeSpanTypeConverter : ITypeConverter<TimeSpan, string>
    {
        public string Convert(TimeSpan source, string destination, ResolutionContext context)
        {
            return new DateTime(source.Ticks).ToString(ApiConstants.TimeFormat);
        }
    }

    public class ListOfStringTypeConverter : ITypeConverter<string, List<string>>
    {
        public List<string> Convert(string source, List<string> destination, ResolutionContext context)
        {
            return !string.IsNullOrEmpty(source) ? source.Split(',').ToList() : null;
        }
    }
}
