using Entities.CoreServicesModels.PlayerScoreModels;
using Entities.DBModels.PlayerScoreModels;
using static Contracts.EnumData.DBModelsEnum;

namespace Repository.DBModels.PlayerScoreModels
{
    public class PlayerGameWeakScoreRepository : RepositoryBase<PlayerGameWeakScore>
    {
        public PlayerGameWeakScoreRepository(BaseContext context) : base(context)
        {
        }

        public IQueryable<PlayerGameWeakScore> FindAll(PlayerGameWeakScoreParameters parameters, bool trackChanges)
        {
            return FindByCondition(a => true, trackChanges)
                   .Filter(parameters.Id,
                           parameters.Fk_PlayerGameWeak,
                           parameters.Fk_TeamGameWeak,
                           parameters.Fk_TeamIgnored,
                           parameters.Fk_ScoreType,
                           parameters.Fk_ScoreTypes,
                           parameters.Fk_Player,
                           parameters.Fk_GameWeak,
                           parameters.FinalValueFrom,
                           parameters.FinalValueTo,
                           parameters.PointsFrom,
                           parameters.PointsTo,
                           parameters.CheckCleanSheet,
                           parameters.CheckReceiveGoals,
                           parameters.Fk_Players,
                           parameters.Fk_Teams,
                           parameters.Fk_Season,
                           parameters.IsEnded,
                           parameters.RateFrom,
                           parameters.RateTo,
                           parameters.DashboardSearch);

        }

        public async Task<PlayerGameWeakScore> FindById(int id, bool trackChanges)
        {
            return await FindByCondition(a => a.Id == id, trackChanges)
                        .FirstOrDefaultAsync();
        }

        public new void Create(PlayerGameWeakScore entity)
        {
            if (FindByCondition(a => a.Fk_PlayerGameWeak == entity.Fk_PlayerGameWeak && a.Fk_ScoreType == entity.Fk_ScoreType, trackChanges: false).Any())
            {
                PlayerGameWeakScore oldEntity = FindByCondition(a => a.Fk_PlayerGameWeak == entity.Fk_PlayerGameWeak && a.Fk_ScoreType == entity.Fk_ScoreType, trackChanges: true).First();

                oldEntity.Value = entity.Value;
                oldEntity.FinalValue = entity.FinalValue;
                oldEntity.GameTime = entity.GameTime;
                oldEntity.Points = entity.Points;
            }
            else
            {
                base.Create(entity);
            }
        }

        public PlayerTotalScoreModel GetPlayerTotalScores(int fk_Player, int fk_Season, int fk_GameWeak, List<int> fk_ScoreTypes)
        {
            return FindAll(new PlayerGameWeakScoreParameters
            {
                Fk_Player = fk_Player,
                Fk_Season = fk_Season,
                Fk_GameWeak = fk_GameWeak,
                Fk_ScoreTypes = fk_ScoreTypes
            }, trackChanges: false)
                .GroupBy(a => 1)
                .Select(a => new PlayerTotalScoreModel
                {
                    Points = a.Sum(b => b.Points),
                    FinalValue = a.Sum(b => b.FinalValue)
                })
                .SingleOrDefault();
        }
    }
}

public static class PlayerGameWeakScoreRepositoryExtension
{
    public static IQueryable<PlayerGameWeakScore> Filter
        (this IQueryable<PlayerGameWeakScore> PlayerGameWeakScores,
         int id,
         int fk_PlayerGameWeak,
         int fk_TeamGameWeak,
         int fk_TeamIgnored,
         int fk_ScoreType,
         List<int> fk_ScoreTypes,
         int fk_Player,
         int fk_GameWeak,
         int? finalValueFrom,
         int? finalValueTo,
         int? pointsFrom,
         int? pointsTo,
         bool checkCleanSheet,
         bool checkReceiveGoals,
         List<int> fk_Players,
         List<int> fk_Teams,
         int fk_Season,
         bool? isEnded,
         double rateFrom,
         double rateTo,
         string dashboardSearch)
    {
        return PlayerGameWeakScores.Where(a => (id == 0 || a.Id == id) &&
                                               
                                               (string.IsNullOrEmpty(dashboardSearch) || 
                                                a.Id.ToString().Contains(dashboardSearch) ||
                                                a.ScoreType.Name.Contains(dashboardSearch) ||
                                                a.PlayerGameWeak.Player.Name.Contains(dashboardSearch) ) &&
                                               
                                               (finalValueFrom == null || a.FinalValue >= finalValueFrom) &&
                                               (finalValueTo == null || a.FinalValue <= finalValueTo) &&
                                               (fk_PlayerGameWeak == 0 || a.Fk_PlayerGameWeak == fk_PlayerGameWeak) &&
                                               (fk_TeamIgnored == 0 || a.PlayerGameWeak.Player.Fk_Team != fk_TeamIgnored) &&
                                               (fk_TeamGameWeak == 0 || a.PlayerGameWeak.Fk_TeamGameWeak == fk_TeamGameWeak) &&
                                               (fk_Player == 0 || a.PlayerGameWeak.Fk_Player == fk_Player) &&
                                               (fk_GameWeak == 0 || a.PlayerGameWeak.TeamGameWeak.Fk_GameWeak == fk_GameWeak) &&
                                               (fk_ScoreType == 0 || a.Fk_ScoreType == fk_ScoreType) &&
                                               (fk_ScoreTypes == null || !fk_ScoreTypes.Any() || fk_ScoreTypes.Contains(a.Fk_ScoreType)) &&
                                              (pointsFrom == null || pointsFrom == 0 || a.Points >= pointsFrom) &&
                                              (pointsTo == null || pointsFrom == 0 || a.Points <= pointsTo) &&
                                              (rateFrom == 0 || a.PlayerGameWeak.Ranking >= rateFrom) &&
                                              (rateTo == 0 || a.PlayerGameWeak.Ranking <= rateTo) &&
                                               (checkCleanSheet == false || (a.Fk_ScoreType == (int)ScoreTypeEnum.Minutes &&
                                                 a.Points > 1 &&
                                                 ((a.PlayerGameWeak.Player.Fk_Team == a.PlayerGameWeak.TeamGameWeak.Fk_Away &&
                                                   a.PlayerGameWeak.TeamGameWeak.HomeScore == 0) ||
                                                  (a.PlayerGameWeak.Player.Fk_Team == a.PlayerGameWeak.TeamGameWeak.Fk_Home &&
                                                   a.PlayerGameWeak.TeamGameWeak.AwayScore == 0)))) &&
                                              (checkReceiveGoals == false || (a.Fk_ScoreType == (int)ScoreTypeEnum.Minutes &&
                                                  a.Points > 1 &&
                                                  ((a.PlayerGameWeak.Player.Fk_Team == a.PlayerGameWeak.TeamGameWeak.Fk_Away &&
                                                    a.PlayerGameWeak.TeamGameWeak.HomeScore > 0) ||
                                                   (a.PlayerGameWeak.Player.Fk_Team == a.PlayerGameWeak.TeamGameWeak.Fk_Home &&
                                                    a.PlayerGameWeak.TeamGameWeak.AwayScore > 0)))) &&
                                              (fk_Teams == null || !fk_Teams.Any() ||
                                               fk_Teams.Contains(a.PlayerGameWeak.TeamGameWeak.Fk_Home) || fk_Teams.Contains(a.PlayerGameWeak.TeamGameWeak.Fk_Away)) &&
                                              (fk_Players == null || !fk_Players.Any() ||
                                               fk_Players.Contains(a.PlayerGameWeak.Fk_Player)) &&
                                              (fk_Season == 0 || a.PlayerGameWeak.TeamGameWeak.GameWeak.Fk_Season == fk_Season) &&
                                              (isEnded == null || a.PlayerGameWeak.TeamGameWeak.IsEnded == isEnded)
                                               );
    }
}
