using CoreServices.Logic;

namespace CoreServices
{
    public class UnitOfWork
    {
        private readonly RepositoryManager _repository;
        private UserService _userService;
        private DashboardAdministrationServices _dashboardAdministrationServices;

        public UnitOfWork(RepositoryManager repository)
        {
            _repository = repository;
        }

        public async Task Save()
        {
            await _repository.Save();
        }

        public UserService User
        {
            get
            {
                _userService ??= new UserService(_repository);
                return _userService;
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

    }
}