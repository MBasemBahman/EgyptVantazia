using Entities.CoreServicesModels.SeasonModels;
using Entities.CoreServicesModels.TeamModels;
using Entities.DBModels.SeasonModels;

namespace CoreServices.Logic
{
    public class SeasonServices
    {
        private readonly RepositoryManager _repository;

        public SeasonServices(RepositoryManager repository)
        {
            _repository = repository;
        }

        #region Season Services
        public IQueryable<SeasonModel> GetSeasons(SeasonParameters parameters,
                bool otherLang)
        {
            return _repository.Season
                       .FindAll(parameters, trackChanges: false)
                       .Select(a => new SeasonModel
                       {
                           Id = a.Id,
                           CreatedAt = a.CreatedAt,
                           CreatedBy = a.CreatedBy,
                           LastModifiedAt = a.LastModifiedAt,
                           LastModifiedBy = a.LastModifiedBy,
                           _365_SeasonId = a._365_SeasonId,
                           Name = otherLang ? a.SeasonLang.Name : a.Name,
                           ImageUrl = a.StorageUrl + a.ImageUrl,
                           IsCurrent = a.IsCurrent
                       })
                       .Search(parameters.SearchColumns, parameters.SearchTerm)
                       .Sort(parameters.OrderBy);
        }

        public Dictionary<string, string> GetSeasonLookUp(SeasonParameters parameters, bool otherLang)
        {
            return GetSeasons(parameters, otherLang).OrderByDescending(a => a._365_SeasonId)
                                                   .ToDictionary(a => a.Id.ToString(), a => a.Name);
        }

        public async Task<PagedList<SeasonModel>> GetSeasonPaged(
                  SeasonParameters parameters,
                  bool otherLang)
        {
            return await PagedList<SeasonModel>.ToPagedList(GetSeasons(parameters, otherLang), parameters.PageNumber, parameters.PageSize);
        }

        public async Task<Season> FindSeasonbyId(int id, bool trackChanges)
        {
            return await _repository.Season.FindById(id, trackChanges);
        }

        public async Task<Season> FindSeasonBy365Id(string id, bool trackChanges)
        {
            return await _repository.Season.FindBy365Id(id, trackChanges);
        }

        public void CreateSeason(Season Season)
        {
            _repository.Season.Create(Season);
        }

        public async Task DeleteSeason(int id)
        {
            Season Season = await FindSeasonbyId(id, trackChanges: true);
            _repository.Season.Delete(Season);
        }

        public SeasonModel GetSeasonbyId(int id, bool otherLang)
        {
            return GetSeasons(new SeasonParameters { Id = id }, otherLang).FirstOrDefault();
        }

        public int GetSeasonCount()
        {
            return _repository.Season.Count();
        }

        public SeasonModel GetCurrentSeason(bool otherLang = false)
        {
            return GetSeasons(new SeasonParameters { IsCurrent = true }, otherLang: otherLang)
                   .OrderByDescending(a => a.Id)
                   .FirstOrDefault();
        }

        public Season AddSeasonGameWeaks(Season season, List<GameWeakCreateOrEditModel> gameWeaks)
        {

            if (gameWeaks != null && gameWeaks.Any())
            {
                foreach (GameWeakCreateOrEditModel gameWeak in gameWeaks)
                {
                    CreateGameWeak(new GameWeak
                    {
                        Fk_Season = season.Id,
                        Name = gameWeak.Name,
                        _365_GameWeakId = gameWeak._365_GameWeakId,
                        GameWeakLang = new GameWeakLang
                        {
                            Name = gameWeak.NameEn
                        }
                    });
                }
            }
            return season;
        }

        public async Task<Season> DeleteSeasonGameWeaks(Season season, List<int> gameWeakIds)
        {
            if (gameWeakIds != null && gameWeakIds.Any())
            {
                foreach (int id in gameWeakIds)
                {
                    await DeleteGameWeak(id);
                }
            }

            return season;
        }

        public async Task<Season> UpdateSeasonGameWeaks(Season season, List<GameWeakCreateOrEditModel> newData)
        {
            List<int> oldData = GetGameWeaks(new GameWeakParameters { Fk_Season = season.Id }, otherLang: false).Select(a => a.Id).ToList();

            List<int> AddData = newData.Select(a => a.Id).ToList().Except(oldData).ToList();

            List<int> RmvData = oldData.Except(newData.Select(a => a.Id).ToList()).ToList();

            List<GameWeakCreateOrEditModel> DataToUpdate = newData.Where(a => oldData.Contains(a.Id)).ToList();

            season = AddSeasonGameWeaks(season, newData.Where(a => AddData.Contains(a.Id)).ToList());

            season = await DeleteSeasonGameWeaks(season, RmvData);

            if (DataToUpdate != null && DataToUpdate.Any())
            {
                foreach (GameWeakCreateOrEditModel data in DataToUpdate)
                {
                    GameWeak dataDb = await FindGameWeakbyId(data.Id, trackChanges: true);
                    dataDb.Name = data.Name;
                    dataDb._365_GameWeakId = data._365_GameWeakId;
                    dataDb.GameWeakLang.Name = data.NameEn;
                    dataDb.IsCurrent = data.IsCurrent;
                    dataDb.IsNext = data.IsNext;
                    dataDb.IsPrev = data.IsPrev;
                }
            }
            return season;
        }

        public async Task<string> UploudSeasonImage(string rootPath, IFormFile file)
        {
            FileUploader uploader = new(rootPath);
            return await uploader.UploudFile(file, "Uploud/Season");
        }
        #endregion

        #region GameWeak Services
        public IQueryable<GameWeakModel> GetGameWeaks(GameWeakParameters parameters,
                bool otherLang)
        {
            return _repository.GameWeak
                       .FindAll(parameters, trackChanges: false)
                       .Select(a => new GameWeakModel
                       {
                           Id = a.Id,
                           CreatedAt = a.CreatedAt,
                           CreatedBy = a.CreatedBy,
                           LastModifiedAt = a.LastModifiedAt,
                           LastModifiedBy = a.LastModifiedBy,
                           _365_GameWeakId = a._365_GameWeakId,
                           Name = otherLang ? a.GameWeakLang.Name : a.Name,
                           Deadline = a.Deadline,
                           JobId = a.JobId,
                           Fk_Season = a.Fk_Season,
                           Season = new SeasonModel
                           {
                               Name = otherLang ? a.Season.SeasonLang.Name : a.Season.Name,
                               ImageUrl = a.Season.StorageUrl + a.Season.ImageUrl,
                               _365_SeasonId = a.Season._365_SeasonId,
                           },
                           IsCurrent = a.IsCurrent,
                           IsNext = a.IsNext,
                           IsPrev = a.IsPrev,
                           Order = string.IsNullOrWhiteSpace(a._365_GameWeakId) ? 0 : Convert.ToInt32(a._365_GameWeakId)
                       })
                       .Search(parameters.SearchColumns, parameters.SearchTerm)
                       .Sort(parameters.OrderBy);
        }

        public GameWeakModel GetCurrentGameWeak(bool otherLang = false)
        {
            return GetGameWeaks(new GameWeakParameters { IsCurrent = true, IsCurrentSeason = true }, otherLang: otherLang)
                   .OrderByDescending(a => a.Order)
                   .FirstOrDefault();
        }

        public GameWeakModel GetNextGameWeak(bool otherLang = false)
        {
            return GetGameWeaks(new GameWeakParameters
            {
                IsNext = true
            }, otherLang: otherLang).FirstOrDefault();
        }

        public GameWeakModel GetPrevGameWeak(bool otherLang = false)
        {
            return GetGameWeaks(new GameWeakParameters
            {
                IsPrev = true
            }, otherLang: otherLang).FirstOrDefault();
        }

        public async Task<PagedList<GameWeakModel>> GetGameWeakPaged(
                  GameWeakParameters parameters,
                  bool otherLang)
        {
            return await PagedList<GameWeakModel>.ToPagedList(GetGameWeaks(parameters, otherLang), parameters.PageNumber, parameters.PageSize);
        }

        public async Task<GameWeak> FindGameWeakbyId(int id, bool trackChanges)
        {
            return await _repository.GameWeak.FindById(id, trackChanges);
        }

        public void ResetCurrentGameWeaks()
        {
            _repository.GameWeak.ResetCurrent();
        }

        public async Task<GameWeak> FindGameWeakby365Id(string id, int fk_Season, bool trackChanges)
        {
            return await _repository.GameWeak.FindBy365Id(id, fk_Season, trackChanges);
        }

        public List<GameWeak> FindGameWeaks(GameWeakParameters parameters, bool trackChanges)
        {
            return _repository.GameWeak
                              .FindAll(parameters, trackChanges)
                              .Include(a => a.GameWeakLang)
                              .ToList()
                              .OrderBy(a => a._365_GameWeakId.ParseToInt())
                              .ToList();
        }

        public Dictionary<string, string> GetGameWeakLookUp(GameWeakParameters parameters, bool otherLang)
        {
            return GetGameWeaks(parameters, otherLang)
                .OrderByDescending(a => a._365_GameWeakId).ToDictionary(a => a.Id.ToString(), a => a.Name);
        }
        public void CreateGameWeak(GameWeak GameWeak)
        {
            _repository.GameWeak.Create(GameWeak);
        }

        public async Task DeleteGameWeak(int id)
        {
            GameWeak GameWeak = await FindGameWeakbyId(id, trackChanges: true);
            _repository.GameWeak.Delete(GameWeak);
        }

        public GameWeakModel GetGameWeakbyId(int id, bool otherLang)
        {
            return GetGameWeaks(new GameWeakParameters { Id = id }, otherLang).FirstOrDefault();
        }

        public int GetGameWeakCount()
        {
            return _repository.GameWeak.Count();
        }
        #endregion

        #region TeamGameWeak Services
        public IQueryable<TeamGameWeakModel> GetTeamGameWeaks(TeamGameWeakParameters parameters,
                bool otherLang)
        {
            return _repository.TeamGameWeak
                       .FindAll(parameters, trackChanges: false)
                       .Select(a => new TeamGameWeakModel
                       {
                           Id = a.Id,
                           IsEnded = a.IsEnded,
                           CreatedAt = a.CreatedAt,
                           CreatedBy = a.CreatedBy,
                           LastModifiedAt = a.LastModifiedAt,
                           LastModifiedBy = a.LastModifiedBy,
                           AwayScore = a.AwayScore,
                           Fk_Away = a.Fk_Away,
                           Fk_GameWeak = a.Fk_GameWeak,
                           Fk_Home = a.Fk_Home,
                           HomeScore = a.HomeScore,
                           StartTime = a.StartTime,
                           _365_MatchId = a._365_MatchId,
                           IsDelayed = a.IsDelayed,
                           JobId = a.JobId,
                           Away = new TeamModel
                           {
                               Name = otherLang ? a.Away.TeamLang.Name : a.Away.Name,
                               ShortName = otherLang ? a.Away.TeamLang.ShortName : a.Away.ShortName,
                               ImageUrl = a.Away.StorageUrl + a.Away.ImageUrl,
                               ShirtImageUrl = a.Away.ShirtStorageUrl + a.Away.ShirtImageUrl,
                               _365_TeamId = a.Away._365_TeamId,

                           },
                           Home = new TeamModel
                           {
                               Name = otherLang ? a.Home.TeamLang.Name : a.Home.Name,
                               ShortName = otherLang ? a.Home.TeamLang.ShortName : a.Home.ShortName,
                               ImageUrl = a.Home.StorageUrl + a.Home.ImageUrl,
                               ShirtImageUrl = a.Home.ShirtStorageUrl + a.Home.ShirtImageUrl,
                               _365_TeamId = a.Home._365_TeamId
                           },
                           GameWeak = new GameWeakModel
                           {
                               Name = otherLang ? a.GameWeak.GameWeakLang.Name : a.GameWeak.Name,
                               _365_GameWeakId = a.GameWeak._365_GameWeakId,
                               Fk_Season = a.GameWeak.Fk_Season,
                               Season = new SeasonModel
                               {
                                   Name = otherLang ? a.GameWeak.Season.SeasonLang.Name : a.GameWeak.Season.Name
                               }
                           }
                       })
                       .Search(parameters.SearchColumns, parameters.SearchTerm)
                       .Sort(parameters.OrderBy);
        }


        public async Task<PagedList<TeamGameWeakModel>> GetTeamGameWeakPaged(
                  TeamGameWeakParameters parameters,
                  bool otherLang)
        {
            return await PagedList<TeamGameWeakModel>.ToPagedList(GetTeamGameWeaks(parameters, otherLang), parameters.PageNumber, parameters.PageSize);
        }

        public async Task<TeamGameWeak> FindTeamGameWeakbyId(int id, bool trackChanges)
        {
            return await _repository.TeamGameWeak.FindById(id, trackChanges);
        }

        public async Task<TeamGameWeak> FindTeamGameWeakby365Id(string id, bool trackChanges)
        {
            return await _repository.TeamGameWeak.FindBy365Id(id, trackChanges);
        }

        public DateTime? GetFirstTeamGameWeakMatchDate()
        {
            return GetNextGameWeak().Deadline.Value.AddHours(-2);
        }

        public void CreateTeamGameWeak(TeamGameWeak TeamGameWeak)
        {
            _repository.TeamGameWeak.Create(TeamGameWeak);
        }

        public async Task DeleteTeamGameWeak(int id)
        {
            TeamGameWeak TeamGameWeak = await FindTeamGameWeakbyId(id, trackChanges: true);
            _repository.TeamGameWeak.Delete(TeamGameWeak);
        }

        public TeamGameWeakModel GetTeamGameWeakbyId(int id, bool otherLang)
        {
            return GetTeamGameWeaks(new TeamGameWeakParameters { Id = id }, otherLang).FirstOrDefault();
        }

        public int GetTeamGameWeakCount()
        {
            return _repository.TeamGameWeak.Count();
        }
        #endregion
    }
}
