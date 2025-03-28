﻿using Entities.CoreServicesModels.TeamModels;
using Entities.DBModels.TeamModels;
using static Contracts.EnumData.DBModelsEnum;

namespace Repository.DBModels.TeamModels
{
    public class PlayerRepository : RepositoryBase<Player>
    {
        public PlayerRepository(BaseContext context) : base(context)
        {
        }

        public IQueryable<Player> FindAll(PlayerParameters parameters, bool trackChanges)
        {
            return FindByCondition(a => true, trackChanges)
                   .Filter(parameters.Id,
                           parameters.Fk_Team,
                           parameters.Fk_Teams,
                           parameters.Fk_GameWeak,
                           parameters.Fk_Season,
                           parameters.Fk_GameWeaks,
                           parameters.Fk_PlayerPosition,
                           parameters.Fk_FormationPosition,
                           parameters._365_PlayerId,
                           parameters.CreatedAtFrom,
                           parameters.CreatedAtTo,
                           parameters._365_PlayerIds,
                           parameters.Fk_TeamGameWeak_Ignored,
                           parameters.Fk_TeamGameWeak,
                           parameters.Fk_Players,
                           parameters.BuyPriceFrom,
                           parameters.BuyPriceTo,
                           parameters.SellPriceFrom,
                           parameters.SellPriceTo,
                           parameters.IsActive,
                           parameters._365_MatchId,
                           parameters.InExternalTeam);
        }

        public async Task<Player> FindById(int id, bool trackChanges)
        {
            return await FindByCondition(a => a.Id == id, trackChanges)
                        .Include(a => a.PlayerLang)
                        .FirstOrDefaultAsync();
        }

        public async Task<Player> FindBy365Id(string id, bool trackChanges)
        {
            return await FindByCondition(a => a._365_PlayerId == id, trackChanges)
                        .Include(a => a.PlayerLang)
                        .FirstOrDefaultAsync();
        }

        public new void Create(Player entity)
        {
            if (entity._365_PlayerId.IsExisting() && FindByCondition(a => a._365_PlayerId == entity._365_PlayerId, trackChanges: false).Any())
            {
                Player oldEntity = FindByCondition(a => a._365_PlayerId == entity._365_PlayerId, trackChanges: true)
                                .Include(a => a.PlayerLang)
                                .First();

                //oldEntity.Name = entity.Name;
                //oldEntity.IsActive = false;
                oldEntity._365_PlayerId = entity._365_PlayerId;
                //oldEntity.ShortName = entity.ShortName;
                oldEntity.Age = entity.Age;
                oldEntity.Height = entity.Height;
                oldEntity.Birthdate = entity.Birthdate;
                //oldEntity.PlayerNumber = entity.PlayerNumber;
                //oldEntity.Fk_PlayerPosition = entity.Fk_PlayerPosition;
                oldEntity.Fk_FormationPosition = entity.Fk_FormationPosition;
                oldEntity.Fk_Team = entity.Fk_Team;
                //oldEntity.PlayerLang.Name = entity.PlayerLang.Name;
                //oldEntity.PlayerLang.ShortName = entity.PlayerLang.ShortName;
            }
            else
            {
                entity.PlayerLang ??= new PlayerLang
                {
                    Name = entity.Name,
                };

                //entity.IsActive = false;

                entity.PlayerPrices ??= new List<PlayerPrice>
                {
                    new PlayerPrice
                    {
                        BuyPrice = 5,
                        SellPrice = 5,
                        Fk_Team= entity.Fk_Team,
                    }
                };

                base.Create(entity);
            }
        }

        public List<int> GetRandomTeam(int fk_Season, bool isTop_11)
        {
            IQueryable<PlayerModelForRandomTeam> playersQuery = FindByCondition(a => a.PlayerPrices.Any(b => b.BuyPrice > 0) &&
                                              (a.Team.AwayGameWeaks.Any(b => b.GameWeak.Fk_Season == fk_Season) ||
                                               a.Team.HomeGameWeaks.Any(b => b.GameWeak.Fk_Season == fk_Season)), trackChanges: false)
                                                .Select(a => new PlayerModelForRandomTeam
                                                {
                                                    Fk_Player = a.Id,
                                                    Fk_PlayerPosition = a.Fk_PlayerPosition,
                                                    Fk_FormationPosition = a.Fk_FormationPosition,
                                                    Fk_Team = a.Fk_Team,
                                                    BuyPrice = a.PlayerPrices
                                                                .Where(b => b.BuyPrice > 0)
                                                                .OrderBy(b => b.Id)
                                                                .Select(b => b.BuyPrice)
                                                                .First(),
                                                    TotalPoints = a.PlayerSeasonScoreStates
                                                                   .Where(b => b.Fk_Season == fk_Season &&
                                                                               b.Fk_ScoreState == (int)ScoreStateEnum.Total)
                                                                   .Select(b => b.Points)
                                                                   .FirstOrDefault()
                                                });

            playersQuery = isTop_11 ? playersQuery.OrderByDescending(a => a.TotalPoints) : (IQueryable<PlayerModelForRandomTeam>)playersQuery.OrderBy(a => Guid.NewGuid());

            List<PlayerModelForRandomTeam> players = GetRandomPlayers(playersQuery, isTop_11);


            return players.Select(a => a.Fk_Player).ToList();
        }

        public List<PlayerModelForRandomTeam> GetRandomPlayers(IQueryable<PlayerModelForRandomTeam> playersQuery, bool isTop_11)
        {
            List<PlayerModelForRandomTeam> playersList = new();

            do
            {
                playersList = new();

                playersList.AddRange(GetRandomPlayers(playersQuery, PlayerPositionEnum.Goalkeeper, 2));
                playersList.AddRange(GetRandomPlayers(playersQuery, PlayerPositionEnum.Defender, 5));
                playersList.AddRange(GetRandomPlayers(playersQuery, PlayerPositionEnum.Midfielder, 5));
                playersList.AddRange(GetRandomPlayers(playersQuery, PlayerPositionEnum.Attacker, 3));

            } while (playersList.GroupBy(a => a.Fk_Team).Any(a => a.Count() > 3) ||
                     (isTop_11 == false && playersList.Select(a => a.BuyPrice).Sum() > 100));

            return playersList;
        }

        public List<PlayerModelForRandomTeam> GetRandomPlayers(IQueryable<PlayerModelForRandomTeam> playersQuery, PlayerPositionEnum playerPosition, int count)
        {
            List<PlayerModelForRandomTeam> playersList = new();

            do
            {
                playersList = new();

                playersList.AddRange(playersQuery.Where(a => a.Fk_PlayerPosition == (int)playerPosition)
                                                 .Skip(0)
                                                 .Take(count)
                                                 .ToList());
            } while (playersList.GroupBy(a => a.Fk_Team).Any(a => a.Count() > 1));

            return playersList;
        }
    }

    public static class PlayerRepositoryExtension
    {
        public static IQueryable<Player> Filter(
            this IQueryable<Player> Players,
            int id,
            int Fk_Team,
            List<int> Fk_Teams,
            int Fk_GameWeak,
            int Fk_Season,
            List<int> Fk_GameWeaks,
            int Fk_PlayerPosition,
            int Fk_FormationPosition,
            string _365_PlayerId,
            DateTime? createdAtFrom,
            DateTime? createdAtTo,
            List<string> _365_PlayerIds,
            int fk_TeamGameWeak_Ignored,
            int fk_TeamGameWeak,
            List<int> fk_Players,
            double? buyPriceFrom,
            double? buyPriceTo,
            double? sellPriceFrom,
            double? sellPriceTo,
            bool? isActive,
            string _365_MatchId,
            bool? inExternalTeam)

        {
            return Players.Where(a => (id == 0 || a.Id == id) &&
                                      (Fk_Team == 0 || a.Fk_Team == Fk_Team) &&
                                      (Fk_Teams == null || !Fk_Teams.Any() || Fk_Teams.Contains(a.Fk_Team)) &&
                                      (string.IsNullOrWhiteSpace(_365_MatchId) || a.PlayerGameWeaks.Any(b => b.TeamGameWeak._365_MatchId == _365_MatchId)) &&
                                      (isActive == null || a.IsActive == isActive) &&
                                      (inExternalTeam == null || a.InExternalTeam == inExternalTeam) &&
                                      (buyPriceFrom == null || a.PlayerPrices.OrderByDescending(b => b.Id).Select(a => a.BuyPrice).FirstOrDefault() >= buyPriceFrom) &&
                                      (buyPriceTo == null || a.PlayerPrices.OrderByDescending(b => b.Id).Select(a => a.BuyPrice).FirstOrDefault() <= buyPriceTo) &&
                                      (sellPriceFrom == null || a.PlayerPrices.OrderByDescending(b => b.Id).Select(a => a.SellPrice).FirstOrDefault() >= sellPriceFrom) &&
                                      (sellPriceTo == null || a.PlayerPrices.OrderByDescending(b => b.Id).Select(a => a.SellPrice).FirstOrDefault() <= sellPriceTo) &&
                                      (Fk_GameWeak == 0 || a.PlayerGameWeaks.Any(a => a.TeamGameWeak.Fk_GameWeak == Fk_GameWeak)) &&
                                      (Fk_Season == 0 || a.Team.Fk_Season == Fk_Season) &&
                                      (Fk_GameWeaks == null || !Fk_GameWeaks.Any() || a.PlayerGameWeaks.Any(a => Fk_GameWeaks.Contains(a.TeamGameWeak.Fk_GameWeak))) &&
                                      (createdAtFrom == null || a.CreatedAt >= createdAtFrom) &&
                                      (createdAtTo == null || a.CreatedAt <= createdAtTo) &&
                                      (fk_TeamGameWeak_Ignored == 0 || !a.PlayerGameWeaks.Any(b => b.Fk_TeamGameWeak == fk_TeamGameWeak_Ignored)) &&
                                      (fk_TeamGameWeak == 0 || a.PlayerGameWeaks.Any(b => b.Fk_TeamGameWeak == fk_TeamGameWeak)) &&
                                      (Fk_PlayerPosition == 0 || a.Fk_PlayerPosition == Fk_PlayerPosition) &&
                                      (Fk_FormationPosition == 0 || a.Fk_FormationPosition == Fk_FormationPosition) &&
                                      (_365_PlayerIds == null || !_365_PlayerIds.Any() || _365_PlayerIds.Contains(a._365_PlayerId)) &&
                                      (string.IsNullOrWhiteSpace(_365_PlayerId) || a._365_PlayerId == _365_PlayerId) &&
                                      (fk_Players == null || !fk_Players.Any() || fk_Players.Contains(a.Id)));

        }
    }
}
