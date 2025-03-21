﻿using Repository.DBModels.AccountModels;
using Repository.DBModels.AccountTeamModels;
using Repository.DBModels.AppInfoModels;
using Repository.DBModels.AuditModels;
using Repository.DBModels.DashboardAdministrationModels;
using Repository.DBModels.LocationModels;
using Repository.DBModels.LogModels;
using Repository.DBModels.NewsModels;
using Repository.DBModels.NotificationModels;
using Repository.DBModels.PlayerMarkModels;
using Repository.DBModels.PlayerScoreModels;
using Repository.DBModels.PlayerStateModels;
using Repository.DBModels.PlayersTransfersModels;
using Repository.DBModels.PrivateLeagueModels;
using Repository.DBModels.SeasonModels;
using Repository.DBModels.SponsorModels;
using Repository.DBModels.StandingsModels;
using Repository.DBModels.SubscripitonModels;
using Repository.DBModels.TeamModels;
using Repository.DBModels.PromoCodeModels;
using Repository.DBModels.MatchStatisticModels;
using Entities.DBModels.TeamModels;

namespace Repository
{
    public class RepositoryManager
    {
        private readonly BaseContext _dBContext;

        #region LogModels
        private LogRepository _logRepository;
        #endregion

        #region UserModels
        private UserRepository _userRepository;
        private DeviceRepository _deviceRepository;
        private VerificationRepository _verificationRepository;
        private RefreshTokenRepository _refreshTokenRepository;
        #endregion

        #region AccountModels
        private AccountRepository _accountRepository;
        private AccountSubscriptionRepository _accountSubscriptionRepository;
        private PaymentRepository _paymentRepository;
        #endregion

        #region AppInfoModels
        private AppAboutRepository _appAboutRepository;
        #endregion

        #region DashboardAdministrationModels

        private AdministrationRolePremissionRepository _administrationRolePremissionRepository;
        private DashboardAccessLevelRepository _dashboardAccessLevelRepository;
        private DashboardAdministrationRoleRepository _dashboardAdministrationRoleRepository;
        private DashboardAdministratorRepository _dashboardAdministratorRepository;
        private DashboardViewRepository _dashboardViewRepository;

        #endregion

        #region LocationModels
        private CountryRepository _countryRepository;
        #endregion

        #region NewsModels
        private NewsAttachmentRepository _newsAttachmentRepository;
        private NewsRepository _newsRepository;
        #endregion

        #region PlayerMarkModels
        private MarkRepository _markRepository;
        private PlayerMarkRepository _playerMarkRepository;
        private PlayerMarkGameWeakRepository _playerMarkGameWeakRepository;
        private PlayerMarkTeamGameWeakRepository _playerMarkTeamGameWeakRepository;
        private PlayerMarkReasonMatchRepository _playerMarkReasonMatchRepository;
        private PlayerMarkGameWeakScoreRepository _playerMarkGameWeakScoreRepository;
        #endregion
        
        #region PlayerScoreModels
        private PlayerGameWeakRepository _playerGameWeakRepository;
        private PlayerGameWeakScoreRepository _playerGameWeakScoreRepository;
        private ScoreTypeRepository _scoreTypeRepository;
        #endregion

        #region PlayersTransfersModels
        private PlayerTransferRepository _playerTransferRepository;
        #endregion

        #region PrivateLeagueModels
        private PrivateLeagueMemberRepository _privateLeagueMemberRepository;
        private PrivateLeagueRepository _privateLeagueRepository;
        #endregion

        #region SeasonModels
        private GameWeakRepository _gameWeakRepository;
        private SeasonRepository _seasonRepository;
        private TeamGameWeakRepository _teamGameWeakRepository;
        #endregion

        #region SponsorModels
        private SponsorRepository _sponsorRepository;
        private SponsorViewRepository _sponsorViewRepository;
        #endregion

        #region StandingsModels
        private StandingsRepository _standingsRepository;
        #endregion

        #region TeamModels
        private PlayerPositionRepository _playerPositionRepository;
        private FormationPositionRepository _formationPositionRepository;

        private PlayerPriceRepository _playerPriceRepository;
        private PlayerRepository _playerRepository;
        private TeamRepository _teamRepository;
        #endregion

        #region AccountTeamModels
        private AccountTeamGameWeakRepository _accountTeamGameWeakRepository;
        private AccountTeamPlayerGameWeakRepository _accountTeamPlayerGameWeakRepository;
        private AccountTeamPlayerRepository _accountTeamPlayerRepository;
        private AccountTeamRepository _accountTeamRepository;
        private TeamPlayerTypeRepository _teamPlayerTypeRepository;
        private CommunicationStatusRepository _communicationStatusRepository;
        #endregion

        #region SubscriptionModels
        private SubscriptionRepository _subscriptionRepository;
        #endregion

        #region PlayerStateModels
        private PlayerGameWeakScoreStateRepository _playerGameWeakScoreStateRepository;
        private PlayerSeasonScoreStateRepository _playerSeasonScoreStateRepository;
        private ScoreStateRepository _scoreStateRepository;
        #endregion

        #region AuditModels
        private JobAuditRepository _jobAuditRepository;
        #endregion

        #region NotificationModels
        private NotificationRepository _notificationRepository;
        #endregion

        #region MatchStatisticModels
        private MatchStatisticScoreRepository _matchStatisticScoreRepository;
        private StatisticCategoryRepository _statisticCategoryRepository;
        private StatisticScoreRepository _statisticScoreRepository;
        #endregion

        #region PromoCodeModels
        private PromoCodeRepository _promoCodeRepository;
        private PromoCodeSubscriptionRepository _promoCodeSubscriptionRepository;
        #endregion



        public RepositoryManager(BaseContext dBContext)
        {
            _dBContext = dBContext;
        }

        public async Task Save()
        {
            _ = await _dBContext.SaveChangesAsync();
        }

        #region LogModels
        public LogRepository Log
        {
            get
            {
                _logRepository ??= new LogRepository(_dBContext);
                return _logRepository;
            }
        }
        #endregion

        #region UserModels

        public UserRepository User
        {
            get
            {
                _userRepository ??= new UserRepository(_dBContext);
                return _userRepository;
            }
        }

        public DeviceRepository Device
        {
            get
            {
                _deviceRepository ??= new DeviceRepository(_dBContext);
                return _deviceRepository;
            }
        }


        public VerificationRepository Verification
        {
            get
            {
                _verificationRepository ??= new VerificationRepository(_dBContext);
                return _verificationRepository;
            }
        }


        public RefreshTokenRepository RefreshToken
        {
            get
            {
                _refreshTokenRepository ??= new RefreshTokenRepository(_dBContext);
                return _refreshTokenRepository;
            }
        }

        #endregion

        #region AccountModels
        public AccountRepository Account
        {
            get
            {
                _accountRepository ??= new AccountRepository(_dBContext);
                return _accountRepository;
            }
        }

        public AccountSubscriptionRepository AccountSubscription
        {
            get
            {
                _accountSubscriptionRepository ??= new AccountSubscriptionRepository(_dBContext);
                return _accountSubscriptionRepository;
            }
        }

        public PaymentRepository Payment
        {
            get
            {
                _paymentRepository ??= new PaymentRepository(_dBContext);
                return _paymentRepository;
            }
        }

        #endregion

        #region AppInfoModels
        public AppAboutRepository AppAbout
        {
            get
            {
                _appAboutRepository ??= new AppAboutRepository(_dBContext);
                return _appAboutRepository;
            }
        }
        #endregion

        #region DashboardAdministrationModels

        public AdministrationRolePremissionRepository AdministrationRolePremission
        {
            get
            {
                _administrationRolePremissionRepository ??= new AdministrationRolePremissionRepository(_dBContext);
                return _administrationRolePremissionRepository;
            }
        }

        public DashboardAccessLevelRepository DashboardAccessLevel
        {
            get
            {
                _dashboardAccessLevelRepository ??= new DashboardAccessLevelRepository(_dBContext);
                return _dashboardAccessLevelRepository;
            }
        }

        public DashboardAdministrationRoleRepository DashboardAdministrationRole
        {
            get
            {
                _dashboardAdministrationRoleRepository ??= new DashboardAdministrationRoleRepository(_dBContext);
                return _dashboardAdministrationRoleRepository;
            }
        }

        public DashboardAdministratorRepository DashboardAdministrator
        {
            get
            {
                _dashboardAdministratorRepository ??= new DashboardAdministratorRepository(_dBContext);
                return _dashboardAdministratorRepository;
            }
        }

        public DashboardViewRepository DashboardView
        {
            get
            {
                _dashboardViewRepository ??= new DashboardViewRepository(_dBContext);
                return _dashboardViewRepository;
            }
        }

        #endregion

        #region LocationModels
        public CountryRepository Country
        {
            get
            {
                _countryRepository ??= new CountryRepository(_dBContext);
                return _countryRepository;
            }
        }
        #endregion

        #region NewsModels
        public NewsAttachmentRepository NewsAttachment
        {
            get
            {
                _newsAttachmentRepository ??= new NewsAttachmentRepository(_dBContext);
                return _newsAttachmentRepository;
            }
        }

        public NewsRepository News
        {
            get
            {
                _newsRepository ??= new NewsRepository(_dBContext);
                return _newsRepository;
            }
        }
        #endregion

        #region PlayerScoreModels
        public PlayerGameWeakRepository PlayerGameWeak
        {
            get
            {
                _playerGameWeakRepository ??= new PlayerGameWeakRepository(_dBContext);
                return _playerGameWeakRepository;
            }
        }
        public PlayerGameWeakScoreRepository PlayerGameWeakScore
        {
            get
            {
                _playerGameWeakScoreRepository ??= new PlayerGameWeakScoreRepository(_dBContext);
                return _playerGameWeakScoreRepository;
            }
        }
        public ScoreTypeRepository ScoreType
        {
            get
            {
                _scoreTypeRepository ??= new ScoreTypeRepository(_dBContext);
                return _scoreTypeRepository;
            }
        }
        #endregion

        #region PlayerMarkModels

        public MarkRepository Mark
        {
            get
            {
                _markRepository ??= new MarkRepository(_dBContext);
                return _markRepository;
            }
        }
        
        public PlayerMarkRepository PlayerMark
        {
            get
            {
                _playerMarkRepository ??= new PlayerMarkRepository(_dBContext);
                return _playerMarkRepository;
            }
        }
        
        public PlayerMarkGameWeakRepository PlayerMarkGameWeak
        {
            get
            {
                _playerMarkGameWeakRepository ??= new PlayerMarkGameWeakRepository(_dBContext);
                return _playerMarkGameWeakRepository;
            }
        }
        
        public PlayerMarkTeamGameWeakRepository PlayerMarkTeamGameWeak
        {
            get
            {
                _playerMarkTeamGameWeakRepository ??= new PlayerMarkTeamGameWeakRepository(_dBContext);
                return _playerMarkTeamGameWeakRepository;
            }
        }
        
        public PlayerMarkReasonMatchRepository PlayerMarkReasonMatch
        {
            get
            {
                _playerMarkReasonMatchRepository ??= new PlayerMarkReasonMatchRepository(_dBContext);
                return _playerMarkReasonMatchRepository;
            }
        }
        
        public PlayerMarkGameWeakScoreRepository PlayerMarkGameWeakScore
        {
            get
            {
                _playerMarkGameWeakScoreRepository ??= new PlayerMarkGameWeakScoreRepository(_dBContext);
                return _playerMarkGameWeakScoreRepository;
            }
        }

        #endregion

        #region PlayersTransfersModels
        public PlayerTransferRepository PlayerTransfer
        {
            get
            {
                _playerTransferRepository ??= new PlayerTransferRepository(_dBContext);
                return _playerTransferRepository;
            }
        }
        #endregion

        #region PrivateLeagueModels
        public PrivateLeagueMemberRepository PrivateLeagueMember
        {
            get
            {
                _privateLeagueMemberRepository ??= new PrivateLeagueMemberRepository(_dBContext);
                return _privateLeagueMemberRepository;
            }
        }
        public PrivateLeagueRepository PrivateLeague
        {
            get
            {
                _privateLeagueRepository ??= new PrivateLeagueRepository(_dBContext);
                return _privateLeagueRepository;
            }
        }
        #endregion

        #region SeasonModels
        public GameWeakRepository GameWeak
        {
            get
            {
                _gameWeakRepository ??= new GameWeakRepository(_dBContext);
                return _gameWeakRepository;
            }
        }
        public SeasonRepository Season
        {
            get
            {
                _seasonRepository ??= new SeasonRepository(_dBContext);
                return _seasonRepository;
            }
        }
        public TeamGameWeakRepository TeamGameWeak
        {
            get
            {
                _teamGameWeakRepository ??= new TeamGameWeakRepository(_dBContext);
                return _teamGameWeakRepository;
            }
        }
        #endregion

        #region SponsorModels
        public SponsorRepository Sponsor
        {
            get
            {
                _sponsorRepository ??= new SponsorRepository(_dBContext);
                return _sponsorRepository;
            }
        }
        public SponsorViewRepository SponsorView
        {
            get
            {
                _sponsorViewRepository ??= new SponsorViewRepository(_dBContext);
                return _sponsorViewRepository;
            }
        }
        #endregion

        #region StandingsModels
        public StandingsRepository Standings
        {
            get
            {
                _standingsRepository ??= new StandingsRepository(_dBContext);
                return _standingsRepository;
            }
        }
        #endregion

        #region TeamModels
        public FormationPositionRepository FormationPosition
        {
            get
            {
                _formationPositionRepository ??= new FormationPositionRepository(_dBContext);
                return _formationPositionRepository;
            }
        }
        public PlayerPositionRepository PlayerPosition
        {
            get
            {
                _playerPositionRepository ??= new PlayerPositionRepository(_dBContext);
                return _playerPositionRepository;
            }
        }
        public PlayerPriceRepository PlayerPrice
        {
            get
            {
                _playerPriceRepository ??= new PlayerPriceRepository(_dBContext);
                return _playerPriceRepository;
            }
        }
        public PlayerRepository Player
        {
            get
            {
                _playerRepository ??= new PlayerRepository(_dBContext);
                return _playerRepository;
            }
        }
        public TeamRepository Team
        {
            get
            {
                _teamRepository ??= new TeamRepository(_dBContext);
                return _teamRepository;
            }
        }
        #endregion

        #region AccountTeamModels
        public AccountTeamGameWeakRepository AccountTeamGameWeak
        {
            get
            {
                _accountTeamGameWeakRepository ??= new AccountTeamGameWeakRepository(_dBContext);
                return _accountTeamGameWeakRepository;
            }
        }
        public AccountTeamPlayerGameWeakRepository AccountTeamPlayerGameWeak
        {
            get
            {
                _accountTeamPlayerGameWeakRepository ??= new AccountTeamPlayerGameWeakRepository(_dBContext);
                return _accountTeamPlayerGameWeakRepository;
            }
        }
        public AccountTeamPlayerRepository AccountTeamPlayer
        {
            get
            {
                _accountTeamPlayerRepository ??= new AccountTeamPlayerRepository(_dBContext);
                return _accountTeamPlayerRepository;
            }
        }
        public AccountTeamRepository AccountTeam
        {
            get
            {
                _accountTeamRepository ??= new AccountTeamRepository(_dBContext);
                return _accountTeamRepository;
            }
        }

        public TeamPlayerTypeRepository TeamPlayerType
        {
            get
            {
                _teamPlayerTypeRepository ??= new TeamPlayerTypeRepository(_dBContext);
                return _teamPlayerTypeRepository;
            }
        }
        
        public CommunicationStatusRepository CommunicationStatus
        {
            get
            {
                _communicationStatusRepository ??= new CommunicationStatusRepository(_dBContext);
                return _communicationStatusRepository;
            }
        }
        #endregion

        #region SubscriptionModels
        public SubscriptionRepository Subscription
        {
            get
            {
                _subscriptionRepository ??= new SubscriptionRepository(_dBContext);
                return _subscriptionRepository;
            }
        }
        #endregion

        #region PlayerStateModels
        public PlayerGameWeakScoreStateRepository PlayerGameWeakScoreState
        {
            get
            {
                _playerGameWeakScoreStateRepository ??= new PlayerGameWeakScoreStateRepository(_dBContext);
                return _playerGameWeakScoreStateRepository;
            }
        }
        public PlayerSeasonScoreStateRepository PlayerSeasonScoreState
        {
            get
            {
                _playerSeasonScoreStateRepository ??= new PlayerSeasonScoreStateRepository(_dBContext);
                return _playerSeasonScoreStateRepository;
            }
        }
        public ScoreStateRepository ScoreState
        {
            get
            {
                _scoreStateRepository ??= new ScoreStateRepository(_dBContext);
                return _scoreStateRepository;
            }
        }
        #endregion

        #region AuditModels

        public JobAuditRepository JobAudit
        {
            get
            {
                _jobAuditRepository ??= new JobAuditRepository(_dBContext);
                return _jobAuditRepository;
            }
        }
        #endregion

        #region NotificationModels

        public NotificationRepository Notification
        {
            get
            {
                _notificationRepository ??= new NotificationRepository(_dBContext);
                return _notificationRepository;
            }
        }
        #endregion

        #region MatchStatisticModels

        public MatchStatisticScoreRepository MatchStatisticScore
        {
            get
            {
                _matchStatisticScoreRepository ??= new MatchStatisticScoreRepository(_dBContext);
                return _matchStatisticScoreRepository;
            }
        }

        public StatisticCategoryRepository StatisticCategory
        {
            get
            {
                _statisticCategoryRepository ??= new StatisticCategoryRepository(_dBContext);
                return _statisticCategoryRepository;
            }
        }

        public StatisticScoreRepository StatisticScore
        {
            get
            {
                _statisticScoreRepository ??= new StatisticScoreRepository(_dBContext);
                return _statisticScoreRepository;
            }
        }
        #endregion

        #region PromoCodeModels

        public PromoCodeRepository PromoCode
        {
            get
            {
                _promoCodeRepository ??= new PromoCodeRepository(_dBContext);
                return _promoCodeRepository;
            }
        }

        public PromoCodeSubscriptionRepository PromoCodeSubscription
        {
            get
            {
                _promoCodeSubscriptionRepository ??= new PromoCodeSubscriptionRepository(_dBContext);
                return _promoCodeSubscriptionRepository;
            }
        }
        #endregion

    }
}
