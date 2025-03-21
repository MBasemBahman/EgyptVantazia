﻿using Entities.CoreServicesModels.PlayerStateModels;
using Entities.DBModels.PlayerScoreModels;
using Entities.DBModels.PlayerStateModels;
using Microsoft.Data.SqlClient;

namespace Repository.DBModels.PlayerStateModels
{
    public class PlayerGameWeakScoreStateRepository : RepositoryBase<PlayerGameWeakScoreState>
    {
        public PlayerGameWeakScoreStateRepository(BaseContext context) : base(context)
        {
        }

        public IQueryable<PlayerGameWeakScoreState> FindAll(PlayerGameWeakScoreStateParameters parameters, bool trackChanges)
        {
            return FindByCondition(a => true, trackChanges)
                   .Filter(parameters.Id,
                           parameters.Fk_GameWeak,
                           parameters.Fk_Season,
                           parameters.Fk_Player,
                           parameters.Fk_ScoreState,
                           parameters.Fk_Players,
                           parameters.Fk_ScoreStates,
                           parameters.Fk_GameWeaks,
                           parameters.Fk_PlayerPosition,
                           parameters.Fk_FormationPosition,
                           parameters.PointsFrom,
                           parameters.PointsTo,
                           parameters.PercentFrom,
                           parameters.PercentTo,
                           parameters.ValueFrom,
                           parameters.ValueTo,
                           parameters.IsTop15,
                           parameters.DashboardSearch);

        }

        public async Task<PlayerGameWeakScoreState> FindById(int id, bool trackChanges)
        {
            return await FindByCondition(a => a.Id == id, trackChanges)
                        .FirstOrDefaultAsync();
        }

        public void UpdateTop15(int fk_GameWeak)
        {
            _ = DBContext.Database.ExecuteSqlRaw("EXEC [dbo].[SP_UpdatePlayerGameWeakScoreStateTop15] @GameWeakId", new SqlParameter("@GameWeakId", fk_GameWeak));
        }

        public new void Create(PlayerGameWeakScoreState entity)
        {
            if (entity.Points == 0 && entity.Value == 0)
            {
                return;
            }
            if (FindByCondition(a => a.Fk_Player == entity.Fk_Player && a.Fk_GameWeak == entity.Fk_GameWeak && a.Fk_ScoreState == entity.Fk_ScoreState, trackChanges: false).Any())
            {
                PlayerGameWeakScoreState oldEntity = FindByCondition(a => a.Fk_Player == entity.Fk_Player && a.Fk_GameWeak == entity.Fk_GameWeak && a.Fk_ScoreState == entity.Fk_ScoreState, trackChanges: true).First();

                oldEntity.Points = entity.Points;
                oldEntity.Value = entity.Value;
                oldEntity.Percent = entity.Percent;
            }
            else
            {
                base.Create(entity);
            }
        }

        public void ResetPlayerGameWeakScoreState(int fk_Player, int fk_GameWeak, int fk_Team)
        {
            if ((fk_GameWeak > 0 && fk_Player > 0) ||
                (fk_GameWeak > 0 && fk_Team > 0))
            {
                List<PlayerGameWeakScoreState> data = FindByCondition(a => (fk_Player == 0 || a.Fk_Player == fk_Player) &&
                                                                           (fk_GameWeak == 0 || a.Fk_GameWeak == fk_GameWeak) &&
                                                                           (fk_Team == 0 || a.Player.Fk_Team == fk_Team),
                                           trackChanges: true).ToList();
                Delete(data);
            }
        }
    }

    public static class PlayerGameWeakScoreStateRepositoryExtension
    {
        public static IQueryable<PlayerGameWeakScoreState> Filter(
            this IQueryable<PlayerGameWeakScoreState> PlayerGameWeakScoreStates,
            int id,
            int? fk_GameWeak,
            int? Fk_Season,
            int fk_Player,
            int fk_ScoreState,
            List<int> fk_Players,
            List<int> fk_ScoreStates,
            List<int> fk_GameWeaks,
            int fk_PlayerPosition,
            int fk_FormationPosition,
            double? pointsFrom,
            double? pointsTo,
            double? percentFrom,
            double? percentTo,
            double? valueFrom,
            double? valueTo,
            bool? IsTop15,
            string dashboardSearch)
        {
            return PlayerGameWeakScoreStates.Where(a => (id == 0 || a.Id == id) &&

                                                    (string.IsNullOrEmpty(dashboardSearch) ||
                                                     a.Id.ToString().Contains(dashboardSearch) ||
                                                     a.Player.Name.Contains(dashboardSearch) ||
                                                     a.GameWeak.Name.Contains(dashboardSearch) ||
                                                     a.ScoreState.Name.Contains(dashboardSearch)) &&

                                                  (IsTop15 == null || (IsTop15 == true ? a.Top15 != null : a.Top15 == null)) &&
                                                  (Fk_Season == null || a.GameWeak.Fk_Season == Fk_Season) &&
                                                  (fk_GameWeak == null || a.Fk_GameWeak == fk_GameWeak) &&
                                                  (fk_Player == 0 || a.Fk_Player == fk_Player) &&
                                                  (fk_PlayerPosition == 0 || a.Player.Fk_PlayerPosition == fk_PlayerPosition) &&
                                                  (fk_FormationPosition == 0 || a.Player.Fk_FormationPosition == fk_FormationPosition) &&
                                                  (fk_ScoreState == 0 || a.Fk_ScoreState == fk_ScoreState) &&
                                                  (fk_Players == null || !fk_Players.Any() ||
                                                   fk_Players.Contains(a.Fk_Player)) &&
                                                  (fk_ScoreStates == null || !fk_ScoreStates.Any() ||
                                                   fk_ScoreStates.Contains(a.Fk_ScoreState)) &&
                                                  (fk_GameWeaks == null || !fk_GameWeaks.Any() ||
                                                   fk_GameWeaks.Contains(a.Fk_GameWeak)) &&
                                                  (pointsFrom == null || a.Points >= pointsFrom) &&
                                                  (pointsTo == null || a.Points <= pointsTo) &&
                                                  (percentFrom == null || a.Percent >= percentFrom) &&
                                                  (percentTo == null || a.Percent <= percentTo) &&
                                                  (valueFrom == null || a.Value >= valueFrom) &&
                                                  (valueTo == null || a.Value <= valueTo));


        }

    }
}




