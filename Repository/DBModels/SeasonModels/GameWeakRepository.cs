﻿using Entities.CoreServicesModels.SeasonModels;
using Entities.DBModels.SeasonModels;
using Entities.DBModels.TeamModels;

namespace Repository.DBModels.SeasonModels
{
    public class GameWeakRepository : RepositoryBase<GameWeak>
    {
        public GameWeakRepository(DbContext context) : base(context)
        {
        }

        public IQueryable<GameWeak> FindAll(GameWeakParameters parameters, bool trackChanges)
        {
            return FindByCondition(a => true, trackChanges)
                   .Filter(parameters.Id,
                           parameters.Fk_Season,
                           parameters._365_GameWeakId,
                           parameters.IsCurrent,
                           parameters.IsCurrentSeason,
                           parameters.BiggerThanWeak,
                           parameters.LowerThanWeak,
                           parameters.Deadline,
                           parameters.DeadlineFrom,
                           parameters.DeadlineTo,
                           parameters.IsNext,
                           parameters.IsPrev);
        }

        public async Task<GameWeak> FindById(int id, bool trackChanges)
        {
            return await FindByCondition(a => a.Id == id, trackChanges)
                        .Include(a => a.GameWeakLang)
                        .SingleOrDefaultAsync();
        }

        public async Task<GameWeak> FindBy365Id(string id, int fk_Season, bool trackChanges)
        {
            return await FindByCondition(a => a.Fk_Season == fk_Season && a._365_GameWeakId == id, trackChanges)
                        .Include(a => a.GameWeakLang)
                        .SingleOrDefaultAsync();
        }

        public void ResetCurrent()
        {
            List<GameWeak> gameWeaks = FindByCondition(a => true, trackChanges: true).ToList();
            gameWeaks.ForEach(a =>
            {
                a.IsCurrent = false;
                a.IsPrev = false;
                a.IsNext = false;
            });
        }

        public new void Create(GameWeak entity)
        {
            if (entity._365_GameWeakId.IsExisting() && FindByCondition(a => a.Fk_Season == entity.Fk_Season && a._365_GameWeakId == entity._365_GameWeakId, trackChanges: false).Any())
            {
                GameWeak oldEntity = FindByCondition(a => a.Fk_Season == entity.Fk_Season && a._365_GameWeakId == entity._365_GameWeakId, trackChanges: true)
                                .Include(a => a.GameWeakLang)
                                .First();

                oldEntity.Name = entity.Name;
                oldEntity._365_GameWeakId = entity._365_GameWeakId;
                oldEntity.GameWeakLang.Name = entity.GameWeakLang.Name;
            }
            else
            {
                entity.GameWeakLang ??= new GameWeakLang
                {
                    Name = entity.Name,
                };
                base.Create(entity);
            }
        }
    }

    public static class GameWeakRepositoryExtension
    {
        public static IQueryable<GameWeak> Filter(
            this IQueryable<GameWeak> GameWeaks,
            int id,
            int Fk_Season,
            string _365_GameWeakId,
            bool? isCurrent,
            bool? isCurrentSeason,
            int? biggerThanWeak,
            int? lowerThanWeak,
            DateTime? deadline,
            DateTime? deadlineFrom,
            DateTime? deadlineTo,
            bool? isNext,
            bool? isPrev)
        {
            return GameWeaks.Where(a => (id == 0 || a.Id == id) &&
                                        (deadline == null || a.Deadline == deadline) &&
                                        (deadlineFrom == null || a.Deadline >= deadlineFrom) &&
                                        (deadlineTo == null || a.Deadline <= deadlineTo) &&
                                        (Fk_Season == 0 || a.Fk_Season == Fk_Season) &&
                                        (biggerThanWeak == null || (!string.IsNullOrEmpty(a._365_GameWeakId) && Convert.ToInt32(a._365_GameWeakId) > biggerThanWeak.Value)) &&
                                        (lowerThanWeak == null || (!string.IsNullOrEmpty(a._365_GameWeakId) && Convert.ToInt32(a._365_GameWeakId) < lowerThanWeak.Value)) &&
                                        (isCurrent == null || a.IsCurrent == isCurrent) &&
                                        (isNext == null || a.IsNext == isNext) &&
                                        (isPrev == null || a.IsPrev == isPrev) &&
                                        (isCurrentSeason == null || a.Season.IsCurrent == isCurrentSeason) &&
                                        (string.IsNullOrWhiteSpace(_365_GameWeakId) || a._365_GameWeakId == _365_GameWeakId));

        }

    }
}
