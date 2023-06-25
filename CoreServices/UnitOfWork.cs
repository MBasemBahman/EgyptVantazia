using BaseDB;
using CoreServices.Logic;

namespace CoreServices
{
    public class UnitOfWork
    {
        private readonly RepositoryManager _repository;
        private readonly BaseContext _dBContext;

        private UserService _userService;
        private LogServices _logServices;
        private AccountServices _accountServices;
        private DashboardAdministrationServices _dashboardAdministrationServices;
        private LocationServices _locationServices;
        private AccountTeamServices _accountTeamServices;
        private AppInfoServices _appInfoServices;
        private NewsServices _newsServices;
        private PlayerScoreServices _playerScoreServices;
        private PlayerMarkServices _playerMarkServices;
        private PlayerStateServices _PlayerStateServices;
        private PlayerTransfersServices _playerTransfersServices;
        private PrivateLeagueServices _privateLeagueServices;
        private SeasonServices _seasonServices;
        private SponsorServices _sponsorServices;
        private StandingsServices _standingsServices;
        private TeamServices _teamServices;
        private SubscriptionServices _subscriptionServices;
        private AuditServices _auditServices;
        private NotificationServices _notificationServices;

        public UnitOfWork(RepositoryManager repository, BaseContext dBContext)
        {
            _repository = repository;
            _dBContext = dBContext;
        }

        public async Task Save()
        {
            await _repository.Save();
        }

        public LogServices Log
        {
            get
            {
                _logServices ??= new LogServices(_repository);
                return _logServices;
            }
        }
        public UserService User
        {
            get
            {
                _userService ??= new UserService(_repository);
                return _userService;
            }
        }
        public AccountServices Account
        {
            get
            {
                _accountServices ??= new AccountServices(_repository);
                return _accountServices;
            }
        }

        public DashboardAdministrationServices DashboardAdministration
        {
            get
            {
                _dashboardAdministrationServices ??= new DashboardAdministrationServices(_repository);
                return _dashboardAdministrationServices;
            }
        }
        public LocationServices Location
        {
            get
            {
                _locationServices ??= new LocationServices(_repository);
                return _locationServices;
            }
        }
        public AccountTeamServices AccountTeam
        {
            get
            {
                _accountTeamServices ??= new AccountTeamServices(_repository, _dBContext);
                return _accountTeamServices;
            }
        }
        public AppInfoServices AppInfo
        {
            get
            {
                _appInfoServices ??= new AppInfoServices(_repository);
                return _appInfoServices;
            }
        }
        public NewsServices News
        {
            get
            {
                _newsServices ??= new NewsServices(_repository);
                return _newsServices;
            }
        }
        public PlayerScoreServices PlayerScore
        {
            get
            {
                _playerScoreServices ??= new PlayerScoreServices(_repository);
                return _playerScoreServices;
            }
        }
        public PlayerMarkServices PlayerMark
        {
            get
            {
                _playerMarkServices ??= new PlayerMarkServices(_repository);
                return _playerMarkServices;
            }
        }
        public PlayerStateServices PlayerState
        {
            get
            {
                _PlayerStateServices ??= new PlayerStateServices(_repository);
                return _PlayerStateServices;
            }
        }
        public PlayerTransfersServices PlayerTransfers
        {
            get
            {
                _playerTransfersServices ??= new PlayerTransfersServices(_repository);
                return _playerTransfersServices;
            }
        }
        public PrivateLeagueServices PrivateLeague
        {
            get
            {
                _privateLeagueServices ??= new PrivateLeagueServices(_repository);
                return _privateLeagueServices;
            }
        }
        public SeasonServices Season
        {
            get
            {
                _seasonServices ??= new SeasonServices(_repository);
                return _seasonServices;
            }
        }
        public SponsorServices Sponsor
        {
            get
            {
                _sponsorServices ??= new SponsorServices(_repository);
                return _sponsorServices;
            }
        }
        public StandingsServices Standings
        {
            get
            {
                _standingsServices ??= new StandingsServices(_repository);
                return _standingsServices;
            }
        }
        public TeamServices Team
        {
            get
            {
                _teamServices ??= new TeamServices(_repository);
                return _teamServices;
            }
        }

        public SubscriptionServices Subscription
        {
            get
            {
                _subscriptionServices ??= new SubscriptionServices(_repository);
                return _subscriptionServices;
            }
        }

        public AuditServices Audit
        {
            get
            {
                _auditServices ??= new AuditServices(_repository);
                return _auditServices;
            }
        }

        public NotificationServices Notification
        {
            get
            {
                _notificationServices ??= new NotificationServices(_repository);
                return _notificationServices;
            }
        }
    }
}