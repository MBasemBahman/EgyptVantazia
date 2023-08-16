using Entities.CoreServicesModels.PlayerMarkModels;
using Entities.CoreServicesModels.PlayerStateModels;
using Entities.CoreServicesModels.SeasonModels;
using Entities.CoreServicesModels.TeamModels;
using Entities.DBModels.PlayerStateModels;
using Entities.DBModels.TeamModels;
using static Contracts.EnumData.DBModelsEnum;
using static Entities.EnumData.LogicEnumData;

namespace CoreServices.Logic
{
    public class TeamServices
    {
        private readonly RepositoryManager _repository;

        public TeamServices(RepositoryManager repository)
        {
            _repository = repository;
        }

        #region Team Services

        public IQueryable<Team> GetTeams(TeamParameters parameters)
        {
            return _repository.Team.FindAll(parameters, trackChanges: false);
        }

        public IQueryable<TeamModel> GetTeams(TeamParameters parameters,
                bool otherLang)
        {
            return _repository.Team
                       .FindAll(parameters, trackChanges: false)
                       .Select(a => new TeamModel
                       {
                           Id = a.Id,
                           CreatedAt = a.CreatedAt,
                           CreatedBy = a.CreatedBy,
                           LastModifiedAt = a.LastModifiedAt,
                           LastModifiedBy = a.LastModifiedBy,
                           _365_TeamId = a._365_TeamId,
                           Name = otherLang ? a.TeamLang.Name : a.Name,
                           IsActive = a.IsActive,
                           ImageUrl = a.StorageUrl + a.ImageUrl,
                           ShirtImageUrl = a.ShirtStorageUrl + a.ShirtImageUrl,
                           ShortName = otherLang ? a.TeamLang.ShortName : a.ShortName,
                           Fk_Season = a.Fk_Season,
                           Season = new SeasonModel
                           {
                               Name = otherLang ? a.Season.SeasonLang.Name : a.Season.Name,
                           }
                       })
                       .Search(parameters.SearchColumns, parameters.SearchTerm)
                       //.Sort(parameters.OrderBy)
                       .OrderBy(a => a.Name);
        }


        public async Task<PagedList<TeamModel>> GetTeamPaged(
                  TeamParameters parameters,
                  bool otherLang)
        {
            return await PagedList<TeamModel>.ToPagedList(GetTeams(parameters, otherLang), parameters.PageNumber, parameters.PageSize);
        }

        public async Task<Team> FindTeambyId(int id, bool trackChanges)
        {
            return await _repository.Team.FindById(id, trackChanges);
        }

        public async Task<Team> FindTeamby365Id(string id, bool trackChanges)
        {
            return await _repository.Team.FindBy365Id(id, trackChanges);
        }

        public void UpdateTeamActivation(int _365CompetitionsEnum, bool isActive)
        {
            _repository.Team.UpdateActivation(_365CompetitionsEnum, isActive);
        }

        public void CreateTeam(Team Team)
        {
            _repository.Team.Create(Team);
        }

        public async Task DeleteTeam(int id)
        {
            Team Team = await FindTeambyId(id, trackChanges: true);
            _repository.Team.Delete(Team);
        }

        public TeamModel GetTeambyId(int id, bool otherLang)
        {
            return GetTeams(new TeamParameters { Id = id }, otherLang).FirstOrDefault();
        }

        public int GetTeamCount()
        {
            return _repository.Team.Count();
        }

        public async Task<string> UploudTeamImage(string rootPath, IFormFile file)
        {
            FileUploader uploader = new(rootPath);
            return await uploader.UploudFile(file, "Uploud/Team");
        }

        public void RemoveTeamImage(string rootPath, string filePath)
        {
            FileUploader uploader = new(rootPath);
            uploader.DeleteFile(filePath);
        }

        public Dictionary<string, string> GetTeamLookUp(TeamParameters parameters, bool otherLang)
        {
            return GetTeams(parameters, otherLang).ToDictionary(a => a.Id.ToString(), a => a.Name);
        }
        #endregion

        #region Player Position Services
        public IQueryable<PlayerPositionModel> GetPlayerPositions(PlayerPositionParameters parameters,
                bool otherLang)
        {
            return _repository.PlayerPosition
                       .FindAll(parameters, trackChanges: false)
                       .Select(a => new PlayerPositionModel
                       {
                           Id = a.Id,
                           CreatedAt = a.CreatedAt,
                           CreatedBy = a.CreatedBy,
                           LastModifiedAt = a.LastModifiedAt,
                           LastModifiedBy = a.LastModifiedBy,
                           Name = otherLang ? a.PlayerPositionLang.Name : a.Name,
                           ShortName = otherLang ? a.PlayerPositionLang.ShortName : a.ShortName,
                           ImageUrl = a.StorageUrl + a.ImageUrl,
                           _365_PositionId = a._365_PositionId,
                           PlayersCount = a.Players.Count,
                       })
                       .Search(parameters.SearchColumns, parameters.SearchTerm)
                       .Sort(parameters.OrderBy);
        }

        public IQueryable<PlayerPosition> GetPlayerPositions(PlayerPositionParameters parameters)
        {
            return _repository.PlayerPosition
                       .FindAll(parameters, trackChanges: false);
        }


        public async Task<PagedList<PlayerPositionModel>> GetPlayerPositionPaged(
                  PlayerPositionParameters parameters,
                  bool otherLang)
        {
            return await PagedList<PlayerPositionModel>.ToPagedList(GetPlayerPositions(parameters, otherLang), parameters.PageNumber, parameters.PageSize);
        }

        public async Task<PlayerPosition> FindPlayerPositionbyId(int id, bool trackChanges)
        {
            return await _repository.PlayerPosition.FindById(id, trackChanges);
        }

        public async Task<PlayerPosition> FindPlayerPositionby365Id(string id, bool trackChanges)
        {
            return await _repository.PlayerPosition.FindBy365Id(id, trackChanges);
        }


        public void CreatePlayerPosition(PlayerPosition PlayerPosition)
        {
            _repository.PlayerPosition.Create(PlayerPosition);
        }

        public async Task DeletePlayerPosition(int id)
        {
            PlayerPosition PlayerPosition = await FindPlayerPositionbyId(id, trackChanges: true);
            _repository.PlayerPosition.Delete(PlayerPosition);
        }

        public PlayerPositionModel GetPlayerPositionbyId(int id, bool otherLang)
        {
            return GetPlayerPositions(new PlayerPositionParameters { Id = id }, otherLang).FirstOrDefault();
        }

        public Dictionary<string, string> GetPlayerPositionLookUp(PlayerPositionParameters parameters, bool otherLang)
        {
            return GetPlayerPositions(parameters, otherLang).ToDictionary(a => a.Id.ToString(), a => a.Name);
        }
        public int GetPlayerPositionCount()
        {
            return _repository.PlayerPosition.Count();
        }

        public async Task<string> UploudPlayerPositionImage(string rootPath, IFormFile file)
        {
            FileUploader uploader = new(rootPath);
            return await uploader.UploudFile(file, "Uploud/PlayerPosition");
        }
        #endregion

        #region Formation Position Services
        public IQueryable<FormationPositionModel> GetFormationPositions(FormationPositionParameters parameters,
                bool otherLang)
        {
            return _repository.FormationPosition
                       .FindAll(parameters, trackChanges: false)
                       .Select(a => new FormationPositionModel
                       {
                           Id = a.Id,
                           CreatedAt = a.CreatedAt,
                           CreatedBy = a.CreatedBy,
                           LastModifiedAt = a.LastModifiedAt,
                           LastModifiedBy = a.LastModifiedBy,
                           Name = otherLang ? a.FormationPositionLang.Name : a.Name,
                           ShortName = otherLang ? a.FormationPositionLang.ShortName : a.ShortName,
                           ImageUrl = a.StorageUrl + a.ImageUrl,
                           _365_PositionId = a._365_PositionId,
                           PlayersCount = a.Players.Count,
                       })
                       .Search(parameters.SearchColumns, parameters.SearchTerm)
                       .Sort(parameters.OrderBy);
        }

        public IQueryable<FormationPosition> GetFormationPositions(FormationPositionParameters parameters)
        {
            return _repository.FormationPosition
                       .FindAll(parameters, trackChanges: false);
        }


        public async Task<PagedList<FormationPositionModel>> GetFormationPositionPaged(
                  FormationPositionParameters parameters,
                  bool otherLang)
        {
            return await PagedList<FormationPositionModel>.ToPagedList(GetFormationPositions(parameters, otherLang), parameters.PageNumber, parameters.PageSize);
        }

        public async Task<FormationPosition> FindFormationPositionbyId(int id, bool trackChanges)
        {
            return await _repository.FormationPosition.FindById(id, trackChanges);
        }

        public async Task<FormationPosition> FindFormationPositionby365Id(string id, bool trackChanges)
        {
            return await _repository.FormationPosition.FindBy365Id(id, trackChanges);
        }


        public void CreateFormationPosition(FormationPosition FormationPosition)
        {
            _repository.FormationPosition.Create(FormationPosition);
        }

        public async Task DeleteFormationPosition(int id)
        {
            FormationPosition FormationPosition = await FindFormationPositionbyId(id, trackChanges: true);
            _repository.FormationPosition.Delete(FormationPosition);
        }

        public FormationPositionModel GetFormationPositionbyId(int id, bool otherLang)
        {
            return GetFormationPositions(new FormationPositionParameters { Id = id }, otherLang).FirstOrDefault();
        }

        public Dictionary<string, string> GetFormationPositionLookUp(FormationPositionParameters parameters, bool otherLang)
        {
            return GetFormationPositions(parameters, otherLang).ToDictionary(a => a.Id.ToString(), a => a.Name);
        }
        public int GetFormationPositionCount()
        {
            return _repository.FormationPosition.Count();
        }

        public async Task<string> UploudFormationPositionImage(string rootPath, IFormFile file)
        {
            FileUploader uploader = new(rootPath);
            return await uploader.UploudFile(file, "Uploud/FormationPosition");
        }
        #endregion

        #region Player Services
        public IQueryable<PlayerModel> GetPlayers(PlayerParameters parameters,
                bool otherLang)
        {
            IQueryable<PlayerModel> quary = _repository.Player
                                   .FindAll(parameters, trackChanges: false)
                                   .Select(a => new PlayerModel
                                   {
                                       Id = a.Id,
                                       CreatedAt = a.CreatedAt,
                                       CreatedBy = a.CreatedBy,
                                       LastModifiedAt = a.LastModifiedAt,
                                       LastModifiedBy = a.LastModifiedBy,
                                       _365_PlayerId = a._365_PlayerId,
                                       Name = otherLang ? a.PlayerLang.Name : a.Name,
                                       IsActive = a.IsActive,
                                       ShortName = otherLang ? a.PlayerLang.ShortName : a.ShortName,
                                       ImageUrl = !string.IsNullOrEmpty(a.ImageUrl) ? a.StorageUrl + a.ImageUrl : a.Team.ShirtStorageUrl + a.Team.ShirtImageUrl,
                                       Fk_PlayerPosition = a.Fk_PlayerPosition,
                                       Fk_FormationPosition = a.Fk_FormationPosition,
                                       Fk_Team = a.Fk_Team,
                                       PlayerNumber = a.PlayerNumber,
                                       Age = a.Age,
                                       Height = a.Height,
                                       Birthdate = a.Birthdate,
                                       InExternalTeam = a.InExternalTeam,
                                       Top15 = a.PlayerSeasonScoreStates
                                                .Where(b => b.Season.IsCurrent && b.Top15 != null)
                                                .Select(b => b.Top15)
                                                .FirstOrDefault(),
                                       PlayerPosition = new PlayerPositionModel
                                       {
                                           Name = otherLang ? a.PlayerPosition.PlayerPositionLang.Name : a.PlayerPosition.Name,
                                           ShortName = otherLang ? a.PlayerPosition.PlayerPositionLang.ShortName : a.PlayerPosition.ShortName,
                                           ImageUrl = a.PlayerPosition.StorageUrl + a.PlayerPosition.ImageUrl,
                                           _365_PositionId = a.PlayerPosition._365_PositionId
                                       },
                                       FormationPosition = a.Fk_FormationPosition > 0 ? new FormationPositionModel
                                       {
                                           Name = otherLang ? a.FormationPosition.FormationPositionLang.Name : a.FormationPosition.Name,
                                           ShortName = otherLang ? a.FormationPosition.FormationPositionLang.ShortName : a.FormationPosition.ShortName,
                                           ImageUrl = a.FormationPosition.StorageUrl + a.FormationPosition.ImageUrl,
                                           _365_PositionId = a.FormationPosition._365_PositionId
                                       } : null,
                                       Team = new TeamModel
                                       {
                                           Name = otherLang ? a.Team.TeamLang.Name : a.Team.Name,
                                           ShortName = otherLang ? a.Team.TeamLang.ShortName : a.Team.ShortName,
                                           ImageUrl = a.Team.StorageUrl + a.Team.ImageUrl,
                                           ShirtImageUrl = a.Team.ShirtStorageUrl + a.Team.ShirtImageUrl,
                                           _365_TeamId = a.Team._365_TeamId
                                       },
                                       BuyPrice = a.PlayerPrices.OrderByDescending(b => b.Id).Select(a => a.BuyPrice).FirstOrDefault(),
                                       SellPrice = a.PlayerPrices.OrderByDescending(b => b.Id).Select(a => a.SellPrice).FirstOrDefault(),
                                       SeasonScoreStates = parameters.IncludeScore && parameters.Fk_SeasonForScores > 0 ?
                                                           a.PlayerSeasonScoreStates
                                                            .Where(b => b.Fk_Season == parameters.Fk_SeasonForScores &&
                                                                        (parameters.Fk_ScoreStatesForSeason == null ||
                                                                         !parameters.Fk_ScoreStatesForSeason.Any() ||
                                                                         parameters.Fk_ScoreStatesForSeason.Contains(b.Fk_ScoreState)))
                                                            .Select(b => new PlayerSeasonScoreStateModel
                                                            {
                                                                Id = b.Id,
                                                                Fk_Player = b.Fk_Player,
                                                                Points = b.Points,
                                                                Percent = b.Percent,
                                                                Value = b.Value,
                                                                Fk_ScoreState = b.Fk_ScoreState,
                                                                Fk_Season = b.Fk_Season,
                                                                LastModifiedAt = b.LastModifiedAt,
                                                                ScoreState = new ScoreStateModel
                                                                {
                                                                    Id = b.Fk_ScoreState,
                                                                    Name = otherLang ? b.ScoreState.ScoreStateLang.Name : b.ScoreState.Name,
                                                                }
                                                            })
                                                            .ToList() : null,
                                       SeasonScoreState = parameters.IncludeScore &&
                                                          parameters.Fk_SeasonForScores > 0 &&
                                                          parameters.CustomOrderInSeasonList &&
                                                          parameters.Fk_ScoreStateForCustomOrder > 0 ?
                                                           a.PlayerSeasonScoreStates
                                                            .Where(b => b.Fk_Season == parameters.Fk_SeasonForScores &&
                                                                        b.Fk_ScoreState == parameters.Fk_ScoreStateForCustomOrder &&
                                                                        (parameters.Fk_ScoreStatesForSeason == null ||
                                                                         !parameters.Fk_ScoreStatesForSeason.Any() ||
                                                                         parameters.Fk_ScoreStatesForSeason.Contains(b.Fk_ScoreState)))
                                                            .Select(b => new PlayerSeasonScoreStateModel
                                                            {
                                                                Points = b.Points,
                                                                Percent = b.Percent,
                                                                Value = b.Value,
                                                                Fk_ScoreState = b.Fk_ScoreState,
                                                            })
                                                            .FirstOrDefault() : null,
                                       HaveSeasonScoreState = parameters.IncludeScore &&
                                                          parameters.Fk_SeasonForScores > 0 &&
                                                          parameters.CustomOrderInSeasonList &&
                                                          parameters.Fk_ScoreStateForCustomOrder > 0 ?
                                                           a.PlayerSeasonScoreStates
                                                            .Any(b => b.Fk_Season == parameters.Fk_SeasonForScores &&
                                                                        b.Fk_ScoreState == parameters.Fk_ScoreStateForCustomOrder &&
                                                                        (parameters.Fk_ScoreStatesForSeason == null ||
                                                                         !parameters.Fk_ScoreStatesForSeason.Any() ||
                                                                         parameters.Fk_ScoreStatesForSeason.Contains(b.Fk_ScoreState))) : false,
                                       GameWeakScoreStates = parameters.IncludeScore && parameters.Fk_GameWeakForScores > 0 ?
                                                           a.PlayerGameWeakScoreStates
                                                            .Where(b => b.Fk_GameWeak == parameters.Fk_GameWeakForScores &&
                                                                        (parameters.Fk_ScoreStatesForGameWeak == null ||
                                                                         !parameters.Fk_ScoreStatesForGameWeak.Any() ||
                                                                         parameters.Fk_ScoreStatesForGameWeak.Contains(b.Fk_ScoreState)))
                                                            .Select(b => new PlayerGameWeakScoreStateModel
                                                            {
                                                                Id = b.Id,
                                                                Fk_Player = b.Fk_Player,
                                                                Points = b.Points,
                                                                Percent = b.Percent,
                                                                Value = b.Value,
                                                                Fk_ScoreState = b.Fk_ScoreState,
                                                                Fk_GameWeak = b.Fk_GameWeak,
                                                                LastModifiedAt = b.LastModifiedAt,
                                                                GameWeak = new GameWeakModel
                                                                {
                                                                    Id = b.Fk_GameWeak,
                                                                    Name = otherLang ? b.GameWeak.GameWeakLang.Name : b.GameWeak.Name,
                                                                },
                                                                ScoreState = new ScoreStateModel
                                                                {
                                                                    Id = b.Fk_ScoreState,
                                                                    Name = otherLang ? b.ScoreState.ScoreStateLang.Name : b.ScoreState.Name,
                                                                }
                                                            })
                                                            .ToList() : null,
                                       GameWeakScoreState = parameters.IncludeScore &&
                                                            parameters.Fk_GameWeakForScores > 0 &&
                                                            parameters.CustomOrderInSeasonList == false &&
                                                            parameters.Fk_ScoreStateForCustomOrder > 0 ?
                                                            a.PlayerGameWeakScoreStates
                                                            .Where(b => b.Fk_GameWeak == parameters.Fk_GameWeakForScores &&
                                                                        b.Fk_ScoreState == parameters.Fk_ScoreStateForCustomOrder &&
                                                                        (parameters.Fk_ScoreStatesForGameWeak == null ||
                                                                         !parameters.Fk_ScoreStatesForGameWeak.Any() ||
                                                                         parameters.Fk_ScoreStatesForGameWeak.Contains(b.Fk_ScoreState)))
                                                            .Select(b => new PlayerGameWeakScoreStateModel
                                                            {
                                                                Points = b.Points,
                                                                Percent = b.Percent,
                                                                Value = b.Value,
                                                            })
                                                            .FirstOrDefault() : null,
                                       HaveGameWeakScoreState = parameters.IncludeScore &&
                                                            parameters.Fk_GameWeakForScores > 0 &&
                                                            parameters.CustomOrderInSeasonList == false &&
                                                            parameters.Fk_ScoreStateForCustomOrder > 0 ?
                                                            a.PlayerGameWeakScoreStates
                                                            .Any(b => b.Fk_GameWeak == parameters.Fk_GameWeakForScores &&
                                                                        b.Fk_ScoreState == parameters.Fk_ScoreStateForCustomOrder &&
                                                                        (parameters.Fk_ScoreStatesForGameWeak == null ||
                                                                         !parameters.Fk_ScoreStatesForGameWeak.Any() ||
                                                                         parameters.Fk_ScoreStatesForGameWeak.Contains(b.Fk_ScoreState))) : false,
                                       LastTransferTypeEnum = (parameters.CheckLastTransfer && parameters.Fk_AccountTeam > 1) ?
                                                               a.PlayerTransfers
                                                                .Where(b => b.Fk_AccountTeam == parameters.Fk_AccountTeam &&
                                                                            b.AccountTeam
                                                                             .AccountTeamGameWeaks
                                                                             .Any(c => c.Fk_GameWeak == b.Fk_GameWeak &&
                                                                                       c.FreeHit == false))
                                                                .OrderByDescending(b => b.Id)
                                                                .Select(b => b.TransferTypeEnum)
                                                                .FirstOrDefault() : null,
                                       PlayerMarks = a.PlayerMarks
                                              .Where(b => b.DateTo == null || b.DateTo >= DateTime.UtcNow)
                                              .Select(b => new PlayerMarkModel
                                              {
                                                  Count = b.Count,
                                                  Used = b.Used,
                                                  Mark = new MarkModel
                                                  {
                                                      ImageUrl = b.Mark.StorageUrl + b.Mark.ImageUrl,
                                                      Id = b.Mark.Id,
                                                      Name = otherLang ? b.Mark.MarkLang.Name : b.Mark.Name
                                                  }
                                              })
                                              .ToList(),
                                       BuyingCount = a.PlayerTransfers.Count(a => a.TransferTypeEnum == TransferTypeEnum.Buying),
                                       SellingCount = a.PlayerTransfers.Count(a => a.TransferTypeEnum == TransferTypeEnum.Selling),
                                   })
                                   .Search(parameters.SearchColumns, parameters.SearchTerm);

            if (parameters.Fk_ScoreStateForCustomOrder > 0)
            {
                if (parameters.CustomOrderInSeasonList)
                {
                    quary = quary.Where(a => a.HaveSeasonScoreState == true);

                    if (parameters.Fk_ScoreStateForCustomOrder == (int)ScoreStateEnum.Total)
                    {
                        if (parameters.CustomOrderDesc)
                        {
                            quary = quary.OrderByDescending(a => a.SeasonScoreState.Points);
                        }
                        else
                        {
                            quary = quary.OrderBy(a => a.SeasonScoreState.Points);
                        }
                    }
                    else if (parameters.Fk_ScoreStateForCustomOrder == (int)ScoreStateEnum.PlayerSelection)
                    {
                        if (parameters.CustomOrderDesc)
                        {
                            quary = quary.OrderByDescending(a => a.SeasonScoreState.Percent);
                        }
                        else
                        {
                            quary = quary.OrderBy(a => a.SeasonScoreState.Percent);
                        }
                    }
                    else if (parameters.Fk_ScoreStateForCustomOrder == (int)ScoreStateEnum.BuyingCount ||
                             parameters.Fk_ScoreStateForCustomOrder == (int)ScoreStateEnum.SellingCount)
                    {
                        if (parameters.CustomOrderDesc)
                        {
                            quary = quary.OrderByDescending(a => a.SeasonScoreState.Value);
                        }
                        else
                        {
                            quary = quary.OrderBy(a => a.SeasonScoreState.Value);
                        }
                    }
                }
                else
                {
                    quary = quary.Where(a => a.HaveGameWeakScoreState == true);

                    if (parameters.Fk_ScoreStateForCustomOrder == (int)ScoreStateEnum.Total)
                    {
                        if (parameters.CustomOrderDesc)
                        {
                            quary = quary.OrderByDescending(a => a.GameWeakScoreState.Points);
                        }
                        else
                        {
                            quary = quary.OrderBy(a => a.GameWeakScoreState.Points);
                        }
                    }
                    else if (parameters.Fk_ScoreStateForCustomOrder == (int)ScoreStateEnum.PlayerSelection)
                    {
                        if (parameters.CustomOrderDesc)
                        {
                            quary = quary.OrderByDescending(a => a.GameWeakScoreState.Percent);
                        }
                        else
                        {
                            quary = quary.OrderBy(a => a.GameWeakScoreState.Percent);
                        }
                    }
                    else if (parameters.Fk_ScoreStateForCustomOrder == (int)ScoreStateEnum.BuyingCount ||
                             parameters.Fk_ScoreStateForCustomOrder == (int)ScoreStateEnum.SellingCount)
                    {
                        if (parameters.CustomOrderDesc)
                        {
                            quary = quary.OrderByDescending(a => a.GameWeakScoreState.Value);
                        }
                        else
                        {
                            quary = quary.OrderBy(a => a.GameWeakScoreState.Value);
                        }
                    }
                }
            }
            else
            {
                quary = quary.Sort(parameters.OrderBy);
            }
            return quary;
        }

        public IQueryable<Player> GetPlayers(PlayerParameters parameters)
        {
            IQueryable<Player> quary = _repository.Player
                                   .FindAll(parameters, trackChanges: false);

            return quary;
        }

        public async Task<PagedList<PlayerModel>> GetPlayerPaged(
                  PlayerParameters parameters,
                  bool otherLang)
        {
            return await PagedList<PlayerModel>.ToPagedList(GetPlayers(parameters, otherLang), parameters.PageNumber, parameters.PageSize);
        }

        public Dictionary<string, string> GetPlayerLookUp(PlayerParameters parameters, bool otherLang)
        {
            return GetPlayers(parameters, otherLang).ToDictionary(a => a.Id.ToString(), a => a.Name);
        }

        public async Task<Player> FindPlayerbyId(int id, bool trackChanges)
        {
            return await _repository.Player.FindById(id, trackChanges);
        }

        public async Task<Player> FindPlayerby365Id(string id, bool trackChanges)
        {
            return await _repository.Player.FindBy365Id(id, trackChanges);
        }

        public void UpdatePlayerActivation(int fk_Team, bool isActive)
        {
            _repository.Player.UpdateActivation(fk_Team, isActive);
        }

        public void CreatePlayer(Player Player)
        {
            _repository.Player.Create(Player);
        }

        public async Task DeletePlayer(int id)
        {
            Player Player = await FindPlayerbyId(id, trackChanges: true);
            _repository.Player.Delete(Player);
        }

        public async Task<string> UploudPlayerImage(string rootPath, IFormFile file)
        {
            FileUploader uploader = new(rootPath);
            return await uploader.UploudFile(file, "Uploud/Player");
        }

        public PlayerModel GetPlayerbyId(int id, bool otherLang)
        {
            return GetPlayers(new PlayerParameters { Id = id }, otherLang).FirstOrDefault();
        }

        public int GetPlayerCount()
        {
            return _repository.Player.Count();
        }

        public PlayerCustomStateResult GetPlayerCustomStateResult(int fk_Player, int fk_Season, int fk_GameWaek)
        {
            return _repository.Player.FindAll(new PlayerParameters
            {
                Id = fk_Player
            }, trackChanges: false)
                .Select(a => new PlayerCustomStateResult
                {
                    BuyingCount = a.PlayerTransfers.Count(b => (fk_GameWaek == 0 || b.Fk_GameWeak == fk_GameWaek) &&
                                                               (fk_Season == 0 || b.GameWeak.Fk_Season == fk_Season) &&
                                                               b.TransferTypeEnum == TransferTypeEnum.Buying),
                    SellingCount = a.PlayerTransfers.Count(b => (fk_GameWaek == 0 || b.Fk_GameWeak == fk_GameWaek) &&
                                                               (fk_Season == 0 || b.GameWeak.Fk_Season == fk_Season) &&
                                                               b.TransferTypeEnum == TransferTypeEnum.Selling),
                    BuyingPrice = a.PlayerTransfers.Where(b => (fk_GameWaek == 0 || b.Fk_GameWeak == fk_GameWaek) &&
                                                               (fk_Season == 0 || b.GameWeak.Fk_Season == fk_Season) &&
                                                               b.TransferTypeEnum == TransferTypeEnum.Buying)
                                                   .OrderByDescending(a => a.Id)
                                                   .Select(a => a.Cost)
                                                   .FirstOrDefault(),
                    SellingPrice = a.PlayerTransfers.Where(b => (fk_GameWaek == 0 || b.Fk_GameWeak == fk_GameWaek) &&
                                                                (fk_Season == 0 || b.GameWeak.Fk_Season == fk_Season) &&
                                                               b.TransferTypeEnum == TransferTypeEnum.Selling)
                                                   .OrderByDescending(a => a.Id)
                                                   .Select(a => a.Cost)
                                                   .FirstOrDefault(),
                    PlayerSelection = a.AccountTeamPlayers
                                       .SelectMany(b => b.AccountTeamPlayerGameWeaks)
                                       .Count(b => (fk_GameWaek == 0 || b.Fk_GameWeak == fk_GameWaek) &&
                                                   (fk_Season == 0 || b.GameWeak.Fk_Season == fk_Season)),
                    PlayerCaptain = a.AccountTeamPlayers
                                       .SelectMany(b => b.AccountTeamPlayerGameWeaks)
                                       .Count(b => (fk_GameWaek == 0 || b.Fk_GameWeak == fk_GameWaek) &&
                                                   (fk_Season == 0 || b.GameWeak.Fk_Season == fk_Season) &&
                                                   b.Fk_TeamPlayerType == (int)TeamPlayerTypeEnum.Captian)
                })
                .FirstOrDefault();
        }

        public List<PlayerModel> GetRandomTeam(int fk_Season, bool isTop_11, bool otherLang)
        {
            List<int> ids = _repository.Player.GetRandomTeam(fk_Season, isTop_11);
            return GetPlayers(new PlayerParameters
            {
                Fk_Players = ids
            }, otherLang).ToList();
        }
        #endregion

        #region PlayerPrice Services
        public IQueryable<PlayerPriceModel> GetPlayerPrices(PlayerPriceParameters parameters,
                bool otherLang)
        {
            return _repository.PlayerPrice
                       .FindAll(parameters, trackChanges: false)
                       .Select(a => new PlayerPriceModel
                       {
                           Id = a.Id,
                           CreatedAt = a.CreatedAt,
                           CreatedBy = a.CreatedBy,
                           LastModifiedAt = a.LastModifiedAt,
                           LastModifiedBy = a.LastModifiedBy,
                           BuyPrice = a.BuyPrice,
                           Fk_Player = a.Fk_Player,
                           Fk_Team = a.Fk_Team,
                           SellPrice = a.SellPrice,
                           Team = new TeamModel
                           {
                               Name = otherLang ? a.Team.TeamLang.Name : a.Team.Name,
                               ShortName = otherLang ? a.Team.TeamLang.ShortName : a.Team.ShortName,
                               ImageUrl = a.Team.StorageUrl + a.Team.ImageUrl,
                               ShirtImageUrl = a.Team.ShirtStorageUrl + a.Team.ShirtImageUrl,
                               _365_TeamId = a.Team._365_TeamId
                           },
                           Player = new PlayerModel
                           {
                               Name = otherLang ? a.Player.PlayerLang.Name : a.Player.Name,
                               ShortName = otherLang ? a.Player.PlayerLang.ShortName : a.Player.ShortName,
                               ImageUrl = !string.IsNullOrEmpty(a.Player.ImageUrl) ? a.Player.StorageUrl + a.Player.ImageUrl : a.Player.Team.ShirtStorageUrl + a.Player.Team.ShirtImageUrl,
                               _365_PlayerId = a.Player._365_PlayerId,
                               InExternalTeam = a.Player.InExternalTeam,
                           },
                       })
                       .Search(parameters.SearchColumns, parameters.SearchTerm)
                       .Sort(parameters.OrderBy);
        }


        public async Task<PagedList<PlayerPriceModel>> GetPlayerPricePaged(
                  PlayerPriceParameters parameters,
                  bool otherLang)
        {
            return await PagedList<PlayerPriceModel>.ToPagedList(GetPlayerPrices(parameters, otherLang), parameters.PageNumber, parameters.PageSize);
        }

        public async Task<PlayerPrice> FindPlayerPricebyId(int id, bool trackChanges)
        {
            return await _repository.PlayerPrice.FindById(id, trackChanges);
        }

        public void CreatePlayerPrice(PlayerPrice PlayerPrice)
        {
            _repository.PlayerPrice.Create(PlayerPrice);
        }

        public async Task DeletePlayerPrice(int id)
        {
            PlayerPrice PlayerPrice = await FindPlayerPricebyId(id, trackChanges: true);
            _repository.PlayerPrice.Delete(PlayerPrice);
        }

        public PlayerPriceModel GetPlayerPricebyId(int id, bool otherLang)
        {
            return GetPlayerPrices(new PlayerPriceParameters { Id = id }, otherLang).FirstOrDefault();
        }

        public int GetPlayerPriceCount()
        {
            return _repository.PlayerPrice.Count();
        }


        public Player AddPlayerPrices(Player player, List<PlayerPriceCreateOrEditModel> prices)
        {

            if (prices != null && prices.Any())
            {
                foreach (PlayerPriceCreateOrEditModel price in prices)
                {
                    CreatePlayerPrice(new PlayerPrice
                    {
                        Fk_Player = player.Id,
                        Fk_Team = player.Fk_Team,
                        BuyPrice = price.BuyPrice,
                        SellPrice = price.SellPrice
                    });
                }
            }
            return player;
        }

        public void AddPlayersPrices(List<PlayerPrice> prices, string userName)
        {
            if (prices != null && prices.Any())
            {
                foreach (PlayerPrice price in prices)
                {
                    CreatePlayerPrice(new PlayerPrice
                    {
                        Fk_Player = price.Fk_Player,
                        Fk_Team = price.Fk_Team,
                        BuyPrice = price.BuyPrice,
                        SellPrice = price.SellPrice,
                        CreatedBy = userName
                    });
                }
            }
        }

        public async Task<Player> DeletePlayerPrices(Player player, List<int> pricesIds)
        {
            if (pricesIds != null && pricesIds.Any())
            {
                foreach (int id in pricesIds)
                {
                    await DeletePlayerPrice(id);
                }
            }

            return player;
        }

        public async Task<Player> UpdatePlayerPrices(Player player, List<PlayerPriceCreateOrEditModel> newData)
        {
            List<int> oldData = GetPlayerPrices(new PlayerPriceParameters { Fk_Player = player.Id }, otherLang: false).Select(a => a.Id).ToList();

            List<int> AddData = newData.Select(a => a.Id).ToList().Except(oldData).ToList();

            List<int> RmvData = oldData.Except(newData.Select(a => a.Id).ToList()).ToList();

            List<PlayerPriceCreateOrEditModel> DataToUpdate = newData.Where(a => oldData.Contains(a.Id)).ToList();

            player = AddPlayerPrices(player, newData.Where(a => AddData.Contains(a.Id)).ToList());

            player = await DeletePlayerPrices(player, RmvData);

            if (DataToUpdate != null && DataToUpdate.Any())
            {
                foreach (PlayerPriceCreateOrEditModel data in DataToUpdate)
                {
                    PlayerPrice dataDb = await FindPlayerPricebyId(data.Id, trackChanges: true);
                    dataDb.SellPrice = data.SellPrice;
                    dataDb.BuyPrice = data.BuyPrice;
                }
            }
            return player;
        }
        #endregion
    }
}
