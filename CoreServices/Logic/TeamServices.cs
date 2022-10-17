using Entities.CoreServicesModels.TeamModels;
using Entities.DBModels.TeamModels;

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
                           ImageUrl = a.StorageUrl + a.ImageUrl,
                       })
                       .Search(parameters.SearchColumns, parameters.SearchTerm)
                       .Sort(parameters.OrderBy);
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
            return GetTeams(new TeamParameters { Id = id }, otherLang).SingleOrDefault();
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
                           ImageUrl = a.StorageUrl + a.ImageUrl,
                           _365_PositionId = a._365_PositionId,
                           PlayersCount = a.Players.Count
                       })
                       .Search(parameters.SearchColumns, parameters.SearchTerm)
                       .Sort(parameters.OrderBy);
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
            return GetPlayerPositions(new PlayerPositionParameters { Id = id }, otherLang).SingleOrDefault();
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

        #region Player Services
        public IQueryable<PlayerModel> GetPlayers(PlayerParameters parameters,
                bool otherLang)
        {
            return _repository.Player
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
                           ImageUrl = !string.IsNullOrEmpty(a.ImageUrl) ? a.StorageUrl + a.ImageUrl : a.Team.StorageUrl + a.Team.ImageUrl,
                           Fk_PlayerPosition = a.Fk_PlayerPosition,
                           Fk_Team = a.Fk_Team,
                           PlayerNumber = a.PlayerNumber,
                           ShortName = a.ShortName,
                           PlayerPosition = new PlayerPositionModel
                           {
                               Name = otherLang ? a.PlayerPosition.PlayerPositionLang.Name : a.PlayerPosition.Name,
                               ImageUrl = a.PlayerPosition.StorageUrl + a.PlayerPosition.ImageUrl,
                               _365_PositionId = a.PlayerPosition._365_PositionId
                           },
                           Team = new TeamModel
                           {
                               Name = otherLang ? a.Team.TeamLang.Name : a.Team.Name,
                               ImageUrl = a.Team.StorageUrl + a.Team.ImageUrl,
                               _365_TeamId = a.Team._365_TeamId
                           },
                           BuyPrice = a.PlayerPrices.OrderByDescending(b => b.Id).Select(a => a.BuyPrice).FirstOrDefault(),
                           SellPrice = a.PlayerPrices.OrderByDescending(b => b.Id).Select(a => a.SellPrice).FirstOrDefault(),
                           ScorePoints = parameters.IncludeScore ?
                                        a.PlayerGameWeaks
                                         .SelectMany(b => b.PlayerGameWeakScores)
                                         .Where(b => (parameters.Fk_ScoreType == 0 || b.Fk_ScoreType == parameters.Fk_ScoreType) &&
                                                     (parameters.Fk_Season == 0 || b.PlayerGameWeak.TeamGameWeak.GameWeak.Fk_Season == parameters.Fk_ScoreType) &&
                                                     (parameters.Fk_GameWeak == 0 || b.PlayerGameWeak.TeamGameWeak.Fk_GameWeak == parameters.Fk_GameWeak))
                                         .Select(b => b.Points).Sum() : 0,
                           ScoreValues = parameters.IncludeScore ?
                                        a.PlayerGameWeaks
                                         .SelectMany(b => b.PlayerGameWeakScores)
                                         .Where(b => (parameters.Fk_ScoreType == 0 || b.Fk_ScoreType == parameters.Fk_ScoreType) &&
                                                     (parameters.Fk_Season == 0 || b.PlayerGameWeak.TeamGameWeak.GameWeak.Fk_Season == parameters.Fk_ScoreType) &&
                                                     (parameters.Fk_GameWeak == 0 || b.PlayerGameWeak.TeamGameWeak.Fk_GameWeak == parameters.Fk_GameWeak))
                                         .Select(b => b.FinalValue).Sum() : 0,
                       })
                       .Search(parameters.SearchColumns, parameters.SearchTerm)
                       .Sort(parameters.OrderBy);
        }


        public async Task<PagedList<PlayerModel>> GetPlayerPaged(
                  PlayerParameters parameters,
                  bool otherLang)
        {
            return await PagedList<PlayerModel>.ToPagedList(GetPlayers(parameters, otherLang), parameters.PageNumber, parameters.PageSize);
        }

        public async Task<Player> FindPlayerbyId(int id, bool trackChanges)
        {
            return await _repository.Player.FindById(id, trackChanges);
        }

        public async Task<Player> FindPlayerby365Id(string id, bool trackChanges)
        {
            return await _repository.Player.FindBy365Id(id, trackChanges);
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
            return GetPlayers(new PlayerParameters { Id = id }, otherLang).SingleOrDefault();
        }

        public int GetPlayerCount()
        {
            return _repository.Player.Count();
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
                               ImageUrl = a.Team.StorageUrl + a.Team.ImageUrl,
                               _365_TeamId = a.Team._365_TeamId
                           },
                           Player = new PlayerModel
                           {
                               Name = otherLang ? a.Player.PlayerLang.Name : a.Player.Name,
                               ImageUrl = !string.IsNullOrEmpty(a.Player.ImageUrl) ? a.Player.StorageUrl + a.Player.ImageUrl : a.Player.Team.StorageUrl + a.Player.Team.ImageUrl,
                               _365_PlayerId = a.Player._365_PlayerId
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
            return GetPlayerPrices(new PlayerPriceParameters { Id = id }, otherLang).SingleOrDefault();
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
