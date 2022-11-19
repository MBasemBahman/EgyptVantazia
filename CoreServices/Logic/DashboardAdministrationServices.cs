using Entities.CoreServicesModels.DashboardAdministrationModels;
using Entities.CoreServicesModels.UserModels;
using Entities.DBModels.DashboardAdministrationModels;

namespace CoreServices.Logic
{
    public class DashboardAdministrationServices
    {
        private readonly RepositoryManager _repository;

        public DashboardAdministrationServices(RepositoryManager repository)
        {
            _repository = repository;
        }

        #region  Dashboard View Services
        public IQueryable<DashboardViewModel> GetViews(DashboardViewParameters parameters,
                bool otherLang)
        {
            return _repository.DashboardView
                       .FindAll(parameters, trackChanges: false)
                       .Select(a => new DashboardViewModel
                       {
                           Name = otherLang ? a.DashboardViewLang.Name : a.Name,
                           Id = a.Id,
                           CreatedAt = a.CreatedAt,
                           ViewPath = a.ViewPath,
                           PremissionsCount = a.Premissions.Count
                       })
                       .Search(parameters.SearchColumns, parameters.SearchTerm)
                       .Sort(parameters.OrderBy);
        }


        public async Task<PagedList<DashboardViewModel>> GetViewsPaged(
                  DashboardViewParameters parameters,
                  bool otherLang)
        {
            return await PagedList<DashboardViewModel>.ToPagedList(GetViews(parameters, otherLang), parameters.PageNumber, parameters.PageSize);
        }

        public async Task<DashboardView> FindViewById(int id, bool trackChanges)
        {
            return await _repository.DashboardView.FindById(id, trackChanges);
        }

        public DashboardViewModel GetViewbyId(int id, bool otherLang)
        {
            return GetViews(new DashboardViewParameters { Id = id }, otherLang).FirstOrDefault();
        }

        public List<int> GetViewsByRoleId(int fk_role)
        {
            return _repository.DashboardView
                       .FindAll(new DashboardViewParameters { Fk_Role = fk_role }, trackChanges: false)
                       .Select(a => a.Id)
                       .ToList();
        }

        public void CreateView(DashboardView dashboardView)
        {
            _repository.DashboardView.Create(dashboardView);
        }

        public async Task DeleteView(int id)
        {
            DashboardView dashboardView = await FindViewById(id, trackChanges: false);
            _repository.DashboardView.Delete(dashboardView);
        }

        public Dictionary<string, string> GetViewsLookUp(DashboardViewParameters parameters, bool otherLang)
        {
            return GetViews(parameters, otherLang).ToDictionary(a => a.Id.ToString(), a => a.Name);
        }

        public int GetViewsCount()
        {
            return _repository.DashboardView.Count();
        }
        #endregion

        #region Dashboard Access Level Services
        public IQueryable<DashboardAccessLevelModel> GetAccessLevels(DashboardAccessLevelParameters parameters,
               bool otherLang)
        {
            return _repository.DashboardAccessLevel
                       .FindAll(parameters, trackChanges: false)
                       .Select(a => new DashboardAccessLevelModel
                       {
                           Name = otherLang ? a.DashboardAccessLevelLang.Name : a.Name,
                           Id = a.Id,
                           CreatedAt = a.CreatedAt,
                           CreateAccess = a.CreateAccess,
                           EditAccess = a.EditAccess,
                           DeleteAccess = a.DeleteAccess,
                           ViewAccess = a.ViewAccess,
                           PremissionsCount = a.Premissions.Count
                       })
                       .Search(parameters.SearchColumns, parameters.SearchTerm)
                       .Sort(parameters.OrderBy);
        }

        public async Task<PagedList<DashboardAccessLevelModel>> GetAccessLevelsPaged(
                  DashboardAccessLevelParameters parameters,
                  bool otherLang)
        {
            return await PagedList<DashboardAccessLevelModel>.ToPagedList(GetAccessLevels(parameters, otherLang), parameters.PageNumber, parameters.PageSize);
        }

        public async Task<DashboardAccessLevel> FindAccessLevelById(int id, bool trackChanges)
        {
            return await _repository.DashboardAccessLevel.FindById(id, trackChanges);
        }

        public DashboardAccessLevelModel GetAccessLevelbyId(int id, bool otherLang)
        {
            return GetAccessLevels(new DashboardAccessLevelParameters { Id = id }, otherLang).FirstOrDefault();
        }

        public DashboardAccessLevelModel GetAccessLevelByPremission(DashboardAccessLevelParameters parameters)
        {
            return GetAccessLevels(parameters, otherLang: false).FirstOrDefault();
        }

        public void CreateAccessLevel(DashboardAccessLevel dashboardAccessLevel)
        {
            _repository.DashboardAccessLevel.Create(dashboardAccessLevel);
        }

        public async Task DeleteAccessLevel(int id)
        {
            DashboardAccessLevel dashboardAccessLevel = await FindAccessLevelById(id, trackChanges: false);
            _repository.DashboardAccessLevel.Delete(dashboardAccessLevel);
        }

        public Dictionary<string, string> GetAccessLevelsLookUp(DashboardAccessLevelParameters parameters, bool otherLang)
        {
            return GetAccessLevels(parameters, otherLang).ToDictionary(a => a.Id.ToString(), a => a.Name);
        }

        public int GetAccessLevelsCount()
        {
            return _repository.DashboardAccessLevel.Count();
        }
        #endregion

        #region Dashboard Administration Role Services
        public IQueryable<DashboardAdministrationRoleModel> GetRoles(DashboardAdministrationRoleRequestParameters parameters,
               bool otherLang)
        {
            return _repository.DashboardAdministrationRole
                       .FindAll(parameters, trackChanges: false)
                       .Select(a => new DashboardAdministrationRoleModel
                       {
                           Name = otherLang ? a.DashboardAdministrationRoleLang.Name : a.Name,
                           Id = a.Id,
                           CreatedAt = a.CreatedAt,
                           PremissionsCount = a.Premissions.Count,
                           AdministratorsCount = a.Administrators.Count
                       })
                       .Search(parameters.SearchColumns, parameters.SearchTerm)
                       .Sort(parameters.OrderBy);
        }


        public async Task<PagedList<DashboardAdministrationRoleModel>> GetRolesPaged(
                  DashboardAdministrationRoleRequestParameters parameters,
                  bool otherLang)
        {
            return await PagedList<DashboardAdministrationRoleModel>.ToPagedList(GetRoles(parameters, otherLang), parameters.PageNumber, parameters.PageSize);
        }

        public async Task<DashboardAdministrationRole> FindRoleById(int id, bool trackChanges)
        {
            return await _repository.DashboardAdministrationRole.FindById(id, trackChanges);
        }

        public DashboardAdministrationRoleModel GetRolebyId(int id, bool otherLang)
        {
            return GetRoles(new DashboardAdministrationRoleRequestParameters { Id = id }, otherLang).FirstOrDefault();
        }

        public void CreateRole(DashboardAdministrationRole dashboardAdministrationRole)
        {
            _repository.DashboardAdministrationRole.Create(dashboardAdministrationRole);
        }

        public void CreateRole(DashboardAdministrationRole dashboardAdministrationRole,
            List<RolePermissionCreateOrEditViewModel> permissions)
        {

            dashboardAdministrationRole = AddRolePermissions(dashboardAdministrationRole, permissions);
            _repository.DashboardAdministrationRole.Create(dashboardAdministrationRole);
        }

        public DashboardAdministrationRole AddRolePermissions(DashboardAdministrationRole dashboardAdministrationRole,
            List<RolePermissionCreateOrEditViewModel> permissions)
        {
            if (permissions != null && permissions.Any())
            {
                dashboardAdministrationRole.Premissions = new List<AdministrationRolePremission>();
                foreach (RolePermissionCreateOrEditViewModel permission in permissions.Where(a => a.Fk_Views != null && a.Fk_Views.Any()))
                {
                    foreach (int fk_view in permission.Fk_Views.Distinct())
                    {
                        dashboardAdministrationRole.Premissions.Add(new AdministrationRolePremission
                        {
                            Fk_DashboardAccessLevel = permission.Fk_AccessLevel,
                            Fk_DashboardView = fk_view
                        });
                    }
                }
            }
            return dashboardAdministrationRole;
        }

        public void UpdateRolePermissions(int fk_role, List<RolePermissionCreateOrEditViewModel> permissions)
        {
            foreach (RolePermissionCreateOrEditViewModel permission in permissions)
            {
                permission.Fk_Views ??= new List<int>();

                List<int> oldViews = GetPremissions(new AdministrationRolePremissionParameters
                { Fk_DashboardAccessLevel = permission.Fk_AccessLevel, Fk_DashboardAdministrationRole = fk_role }
                       , otherLang: false).Select(a => a.Fk_DashboardView).ToList();

                List<int> addedViews = permission.Fk_Views.Except(oldViews).ToList();
                List<int> removedViews = oldViews.Except(permission.Fk_Views).ToList();

                AddRolePermissions(fk_role, permission.Fk_AccessLevel, addedViews);
                RemoveRolePermissions(fk_role, permission.Fk_AccessLevel, removedViews);

            }
        }

        private void AddRolePermissions(int fk_role, int fk_accesslevel, List<int> fk_views)
        {
            foreach (int fk_view in fk_views)
            {
                AdministrationRolePremission premission = new()
                {
                    Fk_DashboardAccessLevel = fk_accesslevel,
                    Fk_DashboardAdministrationRole = fk_role,
                    Fk_DashboardView = fk_view
                };

                _repository.AdministrationRolePremission.Create(premission);
            }
        }

        private void RemoveRolePermissions(int fk_role, int fk_accesslevel, List<int> fk_views)
        {
            foreach (int fk_view in fk_views)
            {
                AdministrationRolePremission premission =
                    _repository.AdministrationRolePremission.FindAll(new AdministrationRolePremissionParameters
                    {
                        Fk_DashboardAccessLevel = fk_accesslevel,
                        Fk_DashboardAdministrationRole = fk_role,
                        Fk_DashboardView = fk_view
                    }, trackChanges: false).FirstOrDefault();

                _repository.AdministrationRolePremission.Delete(premission);
            }
        }


        public async Task DeleteRole(int id)
        {
            DashboardAdministrationRole dashboardAdministrationRole = await FindRoleById(id, trackChanges: false);
            _repository.DashboardAdministrationRole.Delete(dashboardAdministrationRole);
        }

        public Dictionary<string, string> GetRolesLookUp(DashboardAdministrationRoleRequestParameters parameters, bool otherLang)
        {
            return GetRoles(parameters, otherLang).ToDictionary(a => a.Id.ToString(), a => a.Name);
        }

        public int GetRolesCount()
        {
            return _repository.DashboardAdministrationRole.Count();
        }

        public DashboardAdministrationRoleCreateOrEditViewModel GetRoleCreateOrEditViewModel(
            DashboardAdministrationRoleCreateOrEditModel role,
            List<RolePermissionCreateOrEditViewModel> permissions,
            bool otherLang,
            int id = 0)
        {
            DashboardAdministrationRoleCreateOrEditViewModel model = new()
            {
                Role = role,
                Permissions = new List<RolePermissionCreateOrEditViewModel>()
            };

            if (permissions == null)
            {
                foreach (DashboardAccessLevelModel accesslevel in GetAccessLevels(new DashboardAccessLevelParameters(), otherLang).ToList())
                {
                    model.Permissions.Add(new RolePermissionCreateOrEditViewModel
                    {
                        Fk_AccessLevel = accesslevel.Id,
                        AccessLevelName = accesslevel.Name,
                        Fk_Views = id > 0 ? GetPremissions(
                            new AdministrationRolePremissionParameters
                            {
                                Fk_DashboardAccessLevel = accesslevel.Id,
                                Fk_DashboardAdministrationRole = id
                            }, otherLang: false)
                        .Select(b => b.Fk_DashboardView).ToList()
                        : new List<int>()
                    });
                }
            }
            else
            {
                model.Permissions = permissions ?? model.Permissions;
            }

            return model;
        }
        #endregion

        #region Dashboard Administrator Services
        public IQueryable<DashboardAdministratorModel> GetAdministrators(DashboardAdministratorParameters parameters,
               bool otherLang)
        {
            return _repository.DashboardAdministrator
                       .FindAll(parameters, trackChanges: false)
                       .Select(a => new DashboardAdministratorModel
                       {
                           Fk_User = a.Fk_User,
                           Fk_DashboardAdministrationRole = a.Fk_DashboardAdministrationRole,
                           Id = a.Id,
                           CreatedAt = a.CreatedAt,
                           CreatedBy = a.CreatedBy,
                           LastModifiedAt = a.LastModifiedAt,
                           LastModifiedBy = a.LastModifiedBy,
                           User = new UserModel
                           {
                               Name = a.User.Name,
                               UserName = a.User.UserName,
                               PhoneNumber = a.User.PhoneNumber,
                               EmailAddress = a.User.EmailAddress,
                           },
                           DashboardAdministrationRole = new DashboardAdministrationRoleModel
                           {
                               Name = otherLang ? a.DashboardAdministrationRole.DashboardAdministrationRoleLang.Name : a.DashboardAdministrationRole.Name
                           },
                           JobTitle = a.JobTitle,
                       })
                       .Search(parameters.SearchColumns, parameters.SearchTerm)
                       .Sort(parameters.OrderBy);
        }


        public async Task<PagedList<DashboardAdministratorModel>> GetAdministratorsPaged(
                  DashboardAdministratorParameters parameters,
                  bool otherLang)
        {
            return await PagedList<DashboardAdministratorModel>.ToPagedList(GetAdministrators(parameters, otherLang), parameters.PageNumber, parameters.PageSize);
        }

        public async Task<DashboardAdministrator> FindAdministratorById(int id, bool trackChanges)
        {
            return await _repository.DashboardAdministrator.FindById(id, trackChanges);
        }

        public async Task<DashboardAdministrator> FindByUserId(int id, bool trackChanges)
        {
            return await _repository.DashboardAdministrator.FindByUserId(id, trackChanges);
        }

        public DashboardAdministratorModel GetAdministratorbyId(int id, bool otherLang)
        {
            return GetAdministrators(new DashboardAdministratorParameters { Id = id, GetDevelopers = true }, otherLang).FirstOrDefault();
        }

        public void CreateAdministrator(DashboardAdministrator dashboardAdministrator)
        {
            if (dashboardAdministrator.User != null)
            {
                dashboardAdministrator.User.Password = BC.HashPassword(dashboardAdministrator.User.Password);
            }
            _repository.DashboardAdministrator.Create(dashboardAdministrator);
        }

        public async Task DeleteAdministrator(int id)
        {
            DashboardAdministrator dashboardAdministrator = await FindAdministratorById(id, trackChanges: false);
            _repository.DashboardAdministrator.Delete(dashboardAdministrator);
        }

        public int GetAdministratorsCount()
        {
            return _repository.DashboardAdministrator.Count();
        }
        #endregion

        #region Administration Role Premission Services
        public IQueryable<AdministrationRolePremissionModel> GetPremissions(AdministrationRolePremissionParameters parameters,
               bool otherLang)
        {
            return _repository.AdministrationRolePremission
                       .FindAll(parameters, trackChanges: false)
                       .Select(a => new AdministrationRolePremissionModel
                       {
                           Fk_DashboardAccessLevel = a.Fk_DashboardAccessLevel,
                           Fk_DashboardAdministrationRole = a.Fk_DashboardAdministrationRole,
                           Fk_DashboardView = a.Fk_DashboardView,
                           Id = a.Id,
                           CreatedAt = a.CreatedAt,
                           DashboardAdministrationRole = new DashboardAdministrationRoleModel
                           {
                               Name = otherLang ? a.DashboardAdministrationRole.DashboardAdministrationRoleLang.Name : a.DashboardAdministrationRole.Name
                           },
                           DashboardAccessLevel = new DashboardAccessLevelModel
                           {
                               Name = otherLang ? a.DashboardAccessLevel.DashboardAccessLevelLang.Name : a.DashboardAccessLevel.Name
                           },
                           DashboardView = new DashboardViewModel
                           {
                               Name = otherLang ? a.DashboardView.DashboardViewLang.Name : a.DashboardView.Name
                           }
                       })
                       .Search(parameters.SearchColumns, parameters.SearchTerm)
                       .Sort(parameters.OrderBy);
        }



        #endregion

    }
}
