using Entities.CoreServicesModels.PlayerMarkModels;
using Entities.CoreServicesModels.TeamModels;
using Entities.DBModels.PlayerMarkModels;

namespace CoreServices.Logic
{
    public class PlayerMarkServices
    {
        private readonly RepositoryManager _repository;

        public PlayerMarkServices(RepositoryManager repository)
        {
            _repository = repository;
        }

        #region Mark Services
        public IQueryable<MarkModel> GetMarks(MarkParameters parameters,
                bool otherLang)
        {
            return _repository.Mark
                       .FindAll(parameters, trackChanges: false)
                       .Select(x => new MarkModel
                       {
                           Id = x.Id,
                           CreatedAt = x.CreatedAt,
                           CreatedBy = x.CreatedBy,
                           LastModifiedAt = x.LastModifiedAt,
                           LastModifiedBy = x.LastModifiedBy,
                           Name = otherLang ? x.MarkLang.Name : x.Name,
                           PlayerMarkCount = x.PlayerMarks.Count,
                           ImageUrl = x.StorageUrl + x.ImageUrl
                       })
                       .Search(parameters.SearchColumns, parameters.SearchTerm)
                       .Sort(parameters.OrderBy);
        }

        public IQueryable<Mark> GetMarks(MarkParameters parameters)
        {
            return _repository.Mark
                       .FindAll(parameters, trackChanges: false);
        }


        public async Task<PagedList<MarkModel>> GetMarkPaged(
                  MarkParameters parameters,
                  bool otherLang)
        {
            return await PagedList<MarkModel>.ToPagedList(GetMarks(parameters, otherLang), parameters.PageNumber, parameters.PageSize);
        }

        public async Task<Mark> FindMarkbyId(int id, bool trackChanges)
        {
            return await _repository.Mark.FindById(id, trackChanges);
        }

        public Dictionary<string, string> GetMarksLookUp(MarkParameters parameters, bool otherLang)
        {
            return GetMarks(parameters, otherLang).ToDictionary(a => a.Id.ToString(), a => a.Name);
        }
        
        public async Task<string> UploudMark(string rootPath, IFormFile file)
        {
            FileUploader uploader = new(rootPath);
            return await uploader.UploudFile(file, "Uploud/Mark");
        }
        
        public void CreateMark(Mark Mark)
        {
            _repository.Mark.Create(Mark);
        }

        public async Task DeleteMark(int id)
        {
            Mark Mark = await FindMarkbyId(id, trackChanges: true);
            _repository.Mark.Delete(Mark);
        }

        public MarkModel GetMarkbyId(int id, bool otherLang)
        {
            return GetMarks(new MarkParameters { Id = id }, otherLang).FirstOrDefault();
        }

        public int GetMarkCount()
        {
            return _repository.Mark.Count();
        }
        #endregion

        #region PlayerMark Services
        public IQueryable<PlayerMarkModel> GetPlayerMarks(PlayerMarkParameters parameters,
                bool otherLang)
        {
            return _repository.PlayerMark
                       .FindAll(parameters, trackChanges: false)
                       .Select(a => new PlayerMarkModel
                       {
                           Id = a.Id,
                           CreatedAt = a.CreatedAt,
                           CreatedBy = a.CreatedBy,
                           LastModifiedAt = a.LastModifiedAt,
                           LastModifiedBy = a.LastModifiedBy,
                           Fk_Player = a.Fk_Player,
                           DateTo = a.DateTo,
                           Percent = a.Percent,
                           Notes = a.Notes,
                           Player = new PlayerModel
                           {
                               Id = a.Fk_Player,
                               InExternalTeam = a.Player.InExternalTeam,
                               Name = otherLang ? a.Player.PlayerLang.Name : a.Player.Name,
                               ShortName = otherLang ? a.Player.PlayerLang.ShortName : a.Player.ShortName,
                               ImageUrl = !string.IsNullOrEmpty(a.Player.ImageUrl) ? a.Player.StorageUrl + a.Player.ImageUrl : a.Player.Team.ShirtStorageUrl + a.Player.Team.ShirtImageUrl,
                               _365_PlayerId = a.Player._365_PlayerId,
                               Fk_PlayerPosition = a.Player.Fk_PlayerPosition,
                               Fk_FormationPosition = a.Player.Fk_FormationPosition,
                               Fk_Team = a.Player.Fk_Team
                           },
                           Fk_Mark = a.Fk_Mark,
                           Mark = new MarkModel
                           {
                               ImageUrl = a.Mark.StorageUrl + a.Mark.ImageUrl,
                               Id = a.Mark.Id,
                               Name = otherLang ? a.Mark.MarkLang.Name : a.Mark.Name
                           },
                           Count = a.Count,
                           Used = a.Used,
                       })
                       .Search(parameters.SearchColumns, parameters.SearchTerm)
                       .Sort(parameters.OrderBy);
        }


        public IQueryable<PlayerMark> GetPlayerMarks(PlayerMarkParameters parameters)
        {
            return _repository.PlayerMark
                       .FindAll(parameters, trackChanges: false);
        }

        public async Task<PagedList<PlayerMarkModel>> GetPlayerMarkPaged(
                  PlayerMarkParameters parameters,
                  bool otherLang)
        {
            return await PagedList<PlayerMarkModel>.ToPagedList(GetPlayerMarks(parameters, otherLang), parameters.PageNumber, parameters.PageSize);
        }

        public async Task<PlayerMark> FindPlayerMarkbyId(int id, bool trackChanges)
        {
            return await _repository.PlayerMark.FindById(id, trackChanges);
        }

        public void CreatePlayerMark(PlayerMark PlayerMark)
        {
            _repository.PlayerMark.Create(PlayerMark);
        }

        public async Task DeletePlayerMark(int id)
        {
            PlayerMark PlayerMark = await FindPlayerMarkbyId(id, trackChanges: true);
            _repository.PlayerMark.Delete(PlayerMark);
        }

        public PlayerMarkModel GetPlayerMarkbyId(int id, bool otherLang)
        {
            return GetPlayerMarks(new PlayerMarkParameters { Id = id }, otherLang).FirstOrDefault();
        }

        public int GetPlayerMarkCount()
        {
            return _repository.PlayerMark.Count();
        }

        public async Task UpdatePlayerMarkReasonMatches(int fk_PlayerMark, List<int> fk_GameWeaks, string createdBy)
        {
            List<int> oldData = GetPlayerMarkReasonMatches(new PlayerMarkReasonMatchParameters
            {
                Fk_PlayerMark = fk_PlayerMark
            }, otherLang: false).Select(a => a.Fk_TeamGameWeak).ToList();
        
            List<int> newData = fk_GameWeaks.ToList().Except(oldData).ToList();
        
            List<int> rmvData = oldData.Except(fk_GameWeaks).ToList();
            
            // Add New
            if (newData.Count > 0)
            {
                foreach (int fk_TeamGameWeak in newData)   
                {
                    CreatePlayerMarkReasonMatch(new PlayerMarkReasonMatch
                    {
                        Fk_PlayerMark = fk_PlayerMark,
                        Fk_TeamGameWeak = fk_TeamGameWeak,
                        CreatedBy = createdBy,
                        LastModifiedBy = createdBy,
                    });
                }    
            }
            
            // Remove Old
            if (rmvData.Count > 0)
            {
                List<int> fk_PlayerMarkTeamGameWeaks = _repository.PlayerMarkReasonMatch.FindAll(new PlayerMarkReasonMatchParameters
                {
                    Fk_PlayerMark = fk_PlayerMark,
                    Fk_TeamGameWeaks = rmvData
                }, trackChanges: false).Select(a => a.Id).ToList();
                
                foreach (int fk_PlayerMarkTeamGameWeak in fk_PlayerMarkTeamGameWeaks)
                {
                    await DeletePlayerMarkReasonMatch(fk_PlayerMarkTeamGameWeak);
                }
            }
        }
        
        public async Task UpdatePlayerMarkTeamGameWeaks(int fk_PlayerMark, List<int> fk_GameWeaks, string createdBy)
        {
            List<int> oldData = GetPlayerMarkTeamGameWeaks(new PlayerMarkTeamGameWeakParameters
            {
                Fk_PlayerMark = fk_PlayerMark
            }, otherLang: false).Select(a => a.Fk_TeamGameWeak).ToList();
        
            List<int> newData = fk_GameWeaks.ToList().Except(oldData).ToList();
        
            List<int> rmvData = oldData.Except(fk_GameWeaks).ToList();
            
            // Add New
            if (newData.Count > 0)
            {
                foreach (int fk_TeamGameWeak in newData)   
                {
                    CreatePlayerMarkTeamGameWeak(new PlayerMarkTeamGameWeak
                    {
                        Fk_PlayerMark = fk_PlayerMark,
                        Fk_TeamGameWeak = fk_TeamGameWeak,
                        CreatedBy = createdBy,
                        LastModifiedBy = createdBy,
                    });
                }    
            }
            
            // Remove Old
            if (rmvData.Count > 0)
            {
                List<int> fk_PlayerMarkTeamGameWeaks = _repository.PlayerMarkTeamGameWeak.FindAll(new PlayerMarkTeamGameWeakParameters
                {
                    Fk_PlayerMark = fk_PlayerMark,
                    Fk_TeamGameWeaks = rmvData
                }, trackChanges: false).Select(a => a.Id).ToList();
                
                foreach (int fk_PlayerMarkTeamGameWeak in fk_PlayerMarkTeamGameWeaks)
                {
                    await DeletePlayerMarkTeamGameWeak(fk_PlayerMarkTeamGameWeak);
                }
            }
        }
        
        #endregion

        #region PlayerMarkGameWeakScore Services
        public IQueryable<PlayerMarkGameWeakScoreModel> GetPlayerMarkGameWeakScores(PlayerMarkGameWeakScoreParameters parameters,
                bool otherLang)
        {
            return _repository.PlayerMarkGameWeakScore
                       .FindAll(parameters, trackChanges: false)
                       .Select(a => new PlayerMarkGameWeakScoreModel
                       {
                           Id = a.Id,
                           CreatedAt = a.CreatedAt,
                           CreatedBy = a.CreatedBy,
                           LastModifiedAt = a.LastModifiedAt,
                           LastModifiedBy = a.LastModifiedBy,
                           Fk_PlayerMark = a.Fk_PlayerMark,
                           Fk_PlayerGameWeakScore = a.Fk_PlayerGameWeakScore
                       })
                       .Search(parameters.SearchColumns, parameters.SearchTerm)
                       .Sort(parameters.OrderBy);
        }

        public IQueryable<PlayerMarkGameWeakScore> GetPlayerMarkGameWeakScores(PlayerMarkGameWeakScoreParameters parameters)
        {
            return _repository.PlayerMarkGameWeakScore
                       .FindAll(parameters, trackChanges: false);
        }


        public async Task<PagedList<PlayerMarkGameWeakScoreModel>> GetPlayerMarkGameWeakScorePaged(
                  PlayerMarkGameWeakScoreParameters parameters,
                  bool otherLang)
        {
            return await PagedList<PlayerMarkGameWeakScoreModel>.ToPagedList(GetPlayerMarkGameWeakScores(parameters, otherLang), parameters.PageNumber, parameters.PageSize);
        }

        public async Task<PlayerMarkGameWeakScore> FindPlayerMarkGameWeakScorebyId(int id, bool trackChanges)
        {
            return await _repository.PlayerMarkGameWeakScore.FindById(id, trackChanges);
        }

        public void CreatePlayerMarkGameWeakScore(PlayerMarkGameWeakScore PlayerMarkGameWeakScore)
        {
            _repository.PlayerMarkGameWeakScore.Create(PlayerMarkGameWeakScore);
        }

        public async Task DeletePlayerMarkGameWeakScore(int id)
        {
            PlayerMarkGameWeakScore PlayerMarkGameWeakScore = await FindPlayerMarkGameWeakScorebyId(id, trackChanges: true);
            _repository.PlayerMarkGameWeakScore.Delete(PlayerMarkGameWeakScore);
        }

        public PlayerMarkGameWeakScoreModel GetPlayerMarkGameWeakScorebyId(int id, bool otherLang)
        {
            return GetPlayerMarkGameWeakScores(new PlayerMarkGameWeakScoreParameters { Id = id }, otherLang).FirstOrDefault();
        }

        public int GetPlayerMarkGameWeakScoreCount()
        {
            return _repository.PlayerMarkGameWeakScore.Count();
        }
        
        #endregion
        
        #region PlayerMarkTeamGameWeak Services
        public IQueryable<PlayerMarkTeamGameWeakModel> GetPlayerMarkTeamGameWeaks(PlayerMarkTeamGameWeakParameters parameters,
                bool otherLang)
        {   
            return _repository.PlayerMarkTeamGameWeak
                       .FindAll(parameters, trackChanges: false)
                       .Select(a => new PlayerMarkTeamGameWeakModel
                       {
                           Id = a.Id,
                           CreatedAt = a.CreatedAt,
                           CreatedBy = a.CreatedBy,
                           LastModifiedAt = a.LastModifiedAt,
                           LastModifiedBy = a.LastModifiedBy,
                           Fk_PlayerMark = a.Fk_PlayerMark,
                           Fk_TeamGameWeak = a.Fk_TeamGameWeak
                       })
                       .Search(parameters.SearchColumns, parameters.SearchTerm)
                       .Sort(parameters.OrderBy);
        }

        public IQueryable<PlayerMarkTeamGameWeak> GetPlayerMarkTeamGameWeaks(PlayerMarkTeamGameWeakParameters parameters)
        {
            return _repository.PlayerMarkTeamGameWeak
                       .FindAll(parameters, trackChanges: false);
        }


        public async Task<PagedList<PlayerMarkTeamGameWeakModel>> GetPlayerMarkTeamGameWeakPaged(
                  PlayerMarkTeamGameWeakParameters parameters,
                  bool otherLang)
        {
            return await PagedList<PlayerMarkTeamGameWeakModel>.ToPagedList(GetPlayerMarkTeamGameWeaks(parameters, otherLang), parameters.PageNumber, parameters.PageSize);
        }

        public async Task<PlayerMarkTeamGameWeak> FindPlayerMarkTeamGameWeakbyId(int id, bool trackChanges)
        {
            return await _repository.PlayerMarkTeamGameWeak.FindById(id, trackChanges);
        }

        public void CreatePlayerMarkTeamGameWeak(PlayerMarkTeamGameWeak PlayerMarkTeamGameWeak)
        {
            _repository.PlayerMarkTeamGameWeak.Create(PlayerMarkTeamGameWeak);
        }

        public async Task DeletePlayerMarkTeamGameWeak(int id)
        {
            PlayerMarkTeamGameWeak PlayerMarkTeamGameWeak = await FindPlayerMarkTeamGameWeakbyId(id, trackChanges: true);
            _repository.PlayerMarkTeamGameWeak.Delete(PlayerMarkTeamGameWeak);
        }

        public PlayerMarkTeamGameWeakModel GetPlayerMarkTeamGameWeakbyId(int id, bool otherLang)
        {
            return GetPlayerMarkTeamGameWeaks(new PlayerMarkTeamGameWeakParameters { Id = id }, otherLang).FirstOrDefault();
        }

        public int GetPlayerMarkTeamGameWeakCount()
        {
            return _repository.PlayerMarkTeamGameWeak.Count();
        }
        
        #endregion
        
        #region PlayerMarkReasonMatch Services
        public IQueryable<PlayerMarkReasonMatchModel> GetPlayerMarkReasonMatches(PlayerMarkReasonMatchParameters parameters,
                bool otherLang)
        {   
            return _repository.PlayerMarkReasonMatch
                       .FindAll(parameters, trackChanges: false)
                       .Select(a => new PlayerMarkReasonMatchModel
                       {
                           Id = a.Id,
                           CreatedAt = a.CreatedAt,
                           CreatedBy = a.CreatedBy,
                           LastModifiedAt = a.LastModifiedAt,
                           LastModifiedBy = a.LastModifiedBy,
                           Fk_PlayerMark = a.Fk_PlayerMark,
                           Fk_TeamGameWeak = a.Fk_TeamGameWeak
                       })
                       .Search(parameters.SearchColumns, parameters.SearchTerm)
                       .Sort(parameters.OrderBy);
        }

        public IQueryable<PlayerMarkReasonMatch> GetPlayerMarkReasonMatches(PlayerMarkReasonMatchParameters parameters)
        {
            return _repository.PlayerMarkReasonMatch
                       .FindAll(parameters, trackChanges: false);
        }


        public async Task<PagedList<PlayerMarkReasonMatchModel>> GetPlayerMarkReasonMatchPaged(
                  PlayerMarkReasonMatchParameters parameters,
                  bool otherLang)
        {
            return await PagedList<PlayerMarkReasonMatchModel>.ToPagedList(GetPlayerMarkReasonMatches(parameters, otherLang), parameters.PageNumber, parameters.PageSize);
        }

        public async Task<PlayerMarkReasonMatch> FindPlayerMarkReasonMatchbyId(int id, bool trackChanges)
        {
            return await _repository.PlayerMarkReasonMatch.FindById(id, trackChanges);
        }

        public void CreatePlayerMarkReasonMatch(PlayerMarkReasonMatch PlayerMarkReasonMatch)
        {
            _repository.PlayerMarkReasonMatch.Create(PlayerMarkReasonMatch);
        }

        public async Task DeletePlayerMarkReasonMatch(int id)
        {
            PlayerMarkReasonMatch PlayerMarkReasonMatch = await FindPlayerMarkReasonMatchbyId(id, trackChanges: true);
            _repository.PlayerMarkReasonMatch.Delete(PlayerMarkReasonMatch);
        }

        public PlayerMarkReasonMatchModel GetPlayerMarkReasonMatchbyId(int id, bool otherLang)
        {
            return GetPlayerMarkReasonMatches(new PlayerMarkReasonMatchParameters { Id = id }, otherLang).FirstOrDefault();
        }

        public int GetPlayerMarkReasonMatchCount()
        {
            return _repository.PlayerMarkReasonMatch.Count();
        }
        
        #endregion
    }
}
