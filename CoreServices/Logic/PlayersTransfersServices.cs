using Entities.CoreServicesModels.PlayerTransfersModels;
using Entities.CoreServicesModels.SeasonModels;
using Entities.CoreServicesModels.TeamModels;
using Entities.DBModels.PlayersTransfersModels;

namespace CoreServices.Logic
{
    public class PlayerTransfersServices
    {
        private readonly RepositoryManager _repository;

        public PlayerTransfersServices(RepositoryManager repository)
        {
            _repository = repository;
        }

        #region PlayerTransfer Services
        public IQueryable<PlayerTransferModel> GetPlayerTransfers(PlayerTransferParameters parameters,
                bool otherLang)
        {
            return _repository.PlayerTransfer
                       .FindAll(parameters, trackChanges: false)
                       .Select(a => new PlayerTransferModel
                       {
                           Id = a.Id,
                           CreatedAt = a.CreatedAt,
                           CreatedBy = a.CreatedBy,
                           LastModifiedAt = a.LastModifiedAt,
                           LastModifiedBy = a.LastModifiedBy,
                           Cost = a.Cost,
                           Fk_AccountTeam = a.Fk_AccountTeam,
                           Fk_GameWeak = a.Fk_GameWeak,
                           Fk_Player = a.Fk_Player,
                           IsFree = a.IsFree,
                           TransferTypeEnum = a.TransferTypeEnum,
                           GameWeak = new GameWeakModel
                           {
                               Name = otherLang ? a.GameWeak.GameWeakLang.Name : a.GameWeak.Name,
                               _365_GameWeakId = a.GameWeak._365_GameWeakId,
                               Fk_Season = a.GameWeak.Fk_Season,
                               Season = new SeasonModel
                               {
                                   Name = otherLang ? a.GameWeak.Season.SeasonLang.Name : a.GameWeak.Season.Name
                               }
                           },
                           Player = new PlayerModel
                           {
                               Name = otherLang ? a.Player.PlayerLang.Name : a.Player.Name,
                               ImageUrl = !string.IsNullOrEmpty(a.Player.ImageUrl) ? a.Player.StorageUrl + a.Player.ImageUrl : a.Player.Team.ShirtStorageUrl + a.Player.Team.ShirtImageUrl,
                               _365_PlayerId = a.Player._365_PlayerId,
                               Fk_Team = a.Player.Fk_Team,
                               Team = new TeamModel
                               {
                                   Name = otherLang ? a.Player.Team.TeamLang.Name : a.Player.Team.Name
                               }
                           },
                       })
                       .Search(parameters.SearchColumns, parameters.SearchTerm)
                       .Sort(parameters.OrderBy);
        }


        public async Task<PagedList<PlayerTransferModel>> GetPlayerTransferPaged(
                  PlayerTransferParameters parameters,
                  bool otherLang)
        {
            return await PagedList<PlayerTransferModel>.ToPagedList(GetPlayerTransfers(parameters, otherLang), parameters.PageNumber, parameters.PageSize);
        }

        public async Task<PlayerTransfer> FindPlayerTransferbyId(int id, bool trackChanges)
        {
            return await _repository.PlayerTransfer.FindById(id, trackChanges);
        }

        public void CreatePlayerTransfer(PlayerTransfer PlayerTransfer)
        {
            _repository.PlayerTransfer.Create(PlayerTransfer);
        }

        public async Task DeletePlayerTransfer(int id)
        {
            PlayerTransfer PlayerTransfer = await FindPlayerTransferbyId(id, trackChanges: true);
            _repository.PlayerTransfer.Delete(PlayerTransfer);
        }

        public PlayerTransferModel GetPlayerTransferbyId(int id, bool otherLang)
        {
            return GetPlayerTransfers(new PlayerTransferParameters { Id = id }, otherLang).SingleOrDefault();
        }

        public int GetPlayerTransferCount()
        {
            return _repository.PlayerTransfer.Count();
        }
        #endregion
    }
}
