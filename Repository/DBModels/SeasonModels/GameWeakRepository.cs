﻿using Entities.CoreServicesModels.SeasonModels;
using Entities.DBModels.SeasonModels;

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
                           parameters._365_GameWeakId);
        }

        public async Task<GameWeak> FindById(int id, bool trackChanges)
        {
            return await FindByCondition(a => a.Id == id, trackChanges)
                        .Include(a => a.GameWeakLang)
                        .SingleOrDefaultAsync();
        }

        public async Task<GameWeak> FindBy365Id(string id, bool trackChanges)
        {
            return await FindByCondition(a => a._365_GameWeakId == id, trackChanges)
                        .Include(a => a.GameWeakLang)
                        .SingleOrDefaultAsync();
        }

        public new void Create(GameWeak entity)
        {
            entity.GameWeakLang ??= new GameWeakLang
            {
                Name = entity.Name,
            };
            base.Create(entity);
        }
    }

    public static class GameWeakRepositoryExtension
    {
        public static IQueryable<GameWeak> Filter(
            this IQueryable<GameWeak> GameWeaks,
            int id,
            int Fk_Season,
            string _365_GameWeakId)
        {
            return GameWeaks.Where(a => (id == 0 || a.Id == id) &&
                                        (Fk_Season == 0 || a.Fk_Season == Fk_Season) &&
                                        (string.IsNullOrWhiteSpace(_365_GameWeakId) || a._365_GameWeakId == _365_GameWeakId));

        }

    }
}
