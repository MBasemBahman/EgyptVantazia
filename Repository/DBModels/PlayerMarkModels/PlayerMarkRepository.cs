﻿using Entities.CoreServicesModels.PlayerMarkModels;
using Entities.DBModels.LocationModels;
using Entities.DBModels.PlayerMarkModels;

namespace Repository.DBModels.PlayerMarkModels
{
    public class PlayerMarkRepository : RepositoryBase<PlayerMark>
    {
        public PlayerMarkRepository(BaseContext context) : base(context)
        {
        }

        public IQueryable<PlayerMark> FindAll(PlayerMarkParameters parameters, bool trackChanges)
        {
            return FindByCondition(a => true, trackChanges)
                   .Filter(parameters.Id,
                           parameters.Fk_Player,
                           parameters.Fk_Season,
                           parameters.Fk_Mark,
                           parameters.IsValid,
                           parameters.Fk_Teams,
                           parameters.Fk_Players,
                           parameters.SearchBy);
        }

        public async Task<PlayerMark> FindById(int id, bool trackChanges)
        {
            return await FindByCondition(a => a.Id == id, trackChanges)
                        .Include(a => a.PlayerMarkLang)
                        .FirstOrDefaultAsync();
        }

        public new void Delete(PlayerMark entity)
        {
            DBContext.PlayerMarkTeamGameWeaks.RemoveRange(DBContext.PlayerMarkTeamGameWeaks.Where(a => a.Fk_PlayerMark == entity.Id).ToList());
            DBContext.PlayerMarkReasonMatch.RemoveRange(DBContext.PlayerMarkReasonMatch.Where(a => a.Fk_PlayerMark == entity.Id).ToList());
            DBContext.PlayerMarkGameWeakScores.RemoveRange(DBContext.PlayerMarkGameWeakScores.Where(a => a.Fk_PlayerMark == entity.Id).ToList());

            base.Delete(entity);
        }


        public new void Create(PlayerMark entity)
        {
            entity.PlayerMarkLang ??= new PlayerMarkLang
            {
                Notes = entity.Notes,
            };
            base.Create(entity);
        }
    }

    public static class PlayerMarkRepositoryExtension
    {
        public static IQueryable<PlayerMark> Filter(
            this IQueryable<PlayerMark> PlayerMarks,
            int id,
            int fk_Player,
            int fk_Season,
            int fk_Mark,
            bool? isValid,
            List<int> fk_Teams,
            List<int> fk_Players,
            string searchBy)
        {
            return PlayerMarks.Where(a => (id == 0 || a.Id == id) &&
                                    (fk_Player == 0 || a.Fk_Player == fk_Player) &&
                                    (isValid == null || (isValid == true ? a.DateTo >= DateTime.UtcNow : a.DateTo < DateTime.UtcNow)) &&
                                    (fk_Season == 0 || a.Player.Team.Fk_Season == fk_Season) &&
                                    (fk_Mark == 0 || a.Fk_Mark == fk_Mark) &&
                                    (fk_Teams == null || !fk_Teams.Any() || fk_Teams.Contains(a.Player.Fk_Team)) &&
                                    (fk_Players == null || !fk_Players.Any() || fk_Players.Contains(a.Fk_Player)) &&
                                    (string.IsNullOrEmpty(searchBy) ||
                                    a.Player.Name.ToLower().Contains(searchBy.ToLower()) ||
                                    a.Mark.Name.ToLower().Contains(searchBy.ToLower())));
        }

    }
}
