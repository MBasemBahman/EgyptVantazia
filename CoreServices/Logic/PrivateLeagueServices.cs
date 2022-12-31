using Entities.CoreServicesModels.AccountModels;
using Entities.CoreServicesModels.AccountTeamModels;
using Entities.CoreServicesModels.PrivateLeagueModels;
using Entities.DBModels.PrivateLeagueModels;

namespace CoreServices.Logic
{
    public class PrivateLeagueServices
    {
        private readonly RepositoryManager _repository;

        public PrivateLeagueServices(RepositoryManager repository)
        {
            _repository = repository;
        }

        #region PrivateLeague Services
        public IQueryable<PrivateLeagueModel> GetPrivateLeagues(PrivateLeagueParameters parameters)
        {
            return _repository.PrivateLeague
                       .FindAll(parameters, trackChanges: false)
                       .Select(a => new PrivateLeagueModel
                       {
                           Id = a.Id,
                           CreatedAt = a.CreatedAt,
                           CreatedBy = a.CreatedBy,
                           LastModifiedAt = a.LastModifiedAt,
                           LastModifiedBy = a.LastModifiedBy,
                           Name = a.Name,
                           UniqueCode = a.UniqueCode,
                           MemberCount = a.PrivateLeagueMembers.Count,
                           IsAdmin = a.PrivateLeagueMembers.Any(b => b.Fk_Account == parameters.Fk_Account && b.IsAdmin),
                           MyPosition = a.PrivateLeagueMembers
                                         .Where(b => b.Fk_Account == parameters.Fk_Account)
                                         .Select(b => b.Ranking)
                                         .FirstOrDefault()
                       })
                       .Search(parameters.SearchColumns, parameters.SearchTerm)
                       .Sort(parameters.OrderBy);
        }

        public async Task<PagedList<PrivateLeagueModel>> GetPrivateLeaguePaged(
                  PrivateLeagueParameters parameters)
        {
            return await PagedList<PrivateLeagueModel>.ToPagedList(GetPrivateLeagues(parameters), parameters.PageNumber, parameters.PageSize);
        }

        public Dictionary<string, string> GetPrivateLeagueLookUp(PrivateLeagueParameters parameters)
        {
            return GetPrivateLeagues(parameters).ToDictionary(a => a.Id.ToString(), a => a.Name);
        }

        public async Task<PrivateLeague> FindPrivateLeaguebyId(int id, bool trackChanges)
        {
            return await _repository.PrivateLeague.FindById(id, trackChanges);
        }

        public void CreatePrivateLeague(PrivateLeague PrivateLeague)
        {
            _repository.PrivateLeague.Create(PrivateLeague);
        }

        public async Task DeletePrivateLeague(int id)
        {
            PrivateLeague PrivateLeague = await FindPrivateLeaguebyId(id, trackChanges: true);
            _repository.PrivateLeague.Delete(PrivateLeague);
        }

        public PrivateLeagueModel GetPrivateLeaguebyId(int id)
        {
            return GetPrivateLeagues(new PrivateLeagueParameters { Id = id }).FirstOrDefault();
        }

        public int GetPrivateLeagueCount()
        {
            return _repository.PrivateLeague.Count();
        }
        #endregion

        #region PrivateLeagueMember Services
        public IQueryable<PrivateLeagueMemberModel> GetPrivateLeagueMembers(PrivateLeagueMemberParameters parameters)
        {
            return _repository.PrivateLeagueMember
                       .FindAll(parameters, trackChanges: false)
                       .Select(a => new PrivateLeagueMemberModel
                       {
                           Id = a.Id,
                           CreatedAt = a.CreatedAt,
                           CreatedBy = a.CreatedBy,
                           LastModifiedAt = a.LastModifiedAt,
                           LastModifiedBy = a.LastModifiedBy,
                           Fk_Account = a.Fk_Account,
                           Fk_PrivateLeague = a.Fk_PrivateLeague,
                           IsAdmin = a.IsAdmin,
                           Ranking = a.Ranking,
                           Account = new AccountModel
                           {
                               ImageUrl = a.Account.StorageUrl + a.Account.ImageUrl,
                               FullName = a.Account.FullName,
                               AccountTeam = a.Account.AccountTeams
                                                      .Where(b => b.Fk_Season == parameters.Fk_Season)
                                                      .Select(b => new AccountTeamModel
                                                      {
                                                          Id = b.Id,
                                                          Fk_Account = b.Fk_Account,
                                                          Fk_Season = b.Fk_Season,
                                                          Name = b.Name,
                                                          IsVip = b.IsVip,
                                                          TotalMoney = b.TotalMoney,
                                                          TotalPoints = b.TotalPoints,
                                                          ImageUrl = b.StorageUrl + b.ImageUrl,
                                                          CountryRanking = b.CountryRanking,
                                                          GlobalRanking = b.GlobalRanking,
                                                          FavouriteTeamRanking = b.FavouriteTeamRanking,
                                                          CurrentGameWeakPoints = b.AccountTeamGameWeaks
                                                                                   .Where(a => a.GameWeak.IsCurrent == true)
                                                                                   .Select(a => a.TotalPoints ?? 0)
                                                                                   .FirstOrDefault(),
                                                          PrevGameWeakPoints = b.AccountTeamGameWeaks
                                                                                   .Where(a => a.GameWeak.IsPrev == true)
                                                                                   .Select(a => a.TotalPoints ?? 0)
                                                                                   .FirstOrDefault()
                                                      })
                                                      .FirstOrDefault()
                           },
                           PrivateLeague = new PrivateLeagueModel
                           {
                               Name = a.PrivateLeague.Name
                           },
                       })
                       .Search(parameters.SearchColumns, parameters.SearchTerm)
                       .Sort(parameters.OrderBy);
        }

        public async Task<PagedList<PrivateLeagueMemberModel>> GetPrivateLeagueMemberPaged(
                  PrivateLeagueMemberParameters parameters)
        {
            return await PagedList<PrivateLeagueMemberModel>.ToPagedList(GetPrivateLeagueMembers(parameters), parameters.PageNumber, parameters.PageSize);
        }

        public async Task<PrivateLeagueMember> FindPrivateLeagueMemberbyId(int id, bool trackChanges)
        {
            return await _repository.PrivateLeagueMember.FindById(id, trackChanges);
        }

        public void CreatePrivateLeagueMember(PrivateLeagueMember PrivateLeagueMember)
        {
            _repository.PrivateLeagueMember.Create(PrivateLeagueMember);
        }

        public async Task DeletePrivateLeagueMember(int id)
        {
            PrivateLeagueMember PrivateLeagueMember = await FindPrivateLeagueMemberbyId(id, trackChanges: true);
            _repository.PrivateLeagueMember.Delete(PrivateLeagueMember);
        }

        public PrivateLeagueMemberModel GetPrivateLeagueMemberbyId(int id)
        {
            return GetPrivateLeagueMembers(new PrivateLeagueMemberParameters { Id = id }).FirstOrDefault();
        }

        public int GetPrivateLeagueMemberCount()
        {
            return _repository.PrivateLeagueMember.Count();
        }
        #endregion
    }
}
