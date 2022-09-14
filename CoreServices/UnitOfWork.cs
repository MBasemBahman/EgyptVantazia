using CoreServices.Logic;

namespace CoreServices
{
    public class UnitOfWork
    {
        private readonly RepositoryManager _repository;
        private UserService _userService;
        private LogServices _logServices;
        private AccountServices _accountServices;
        private DashboardAdministrationServices _dashboardAdministrationServices;
        private LocationServices _locationServices;

        public UnitOfWork(RepositoryManager repository)
        {
            _repository = repository;
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
    }
}