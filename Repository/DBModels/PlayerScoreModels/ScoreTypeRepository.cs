﻿using Entities.DBModels.PlayerScoreModels;
using Entities.RequestFeatures;


namespace Repository.DBModels.PlayerScoreModels
{
    public class ScoreTypeRepository : RepositoryBase<ScoreType>
    {
        public ScoreTypeRepository(DbContext context) : base(context)
        {
        }

        public IQueryable<ScoreType> FindAll(RequestParameters parameters, bool trackChanges)
        {
            return FindByCondition(a => true, trackChanges)
                   .Filter(parameters.Id);
        }

        public async Task<ScoreType> FindById(int id, bool trackChanges)
        {
            return await FindByCondition(a => a.Id == id, trackChanges)
                        .Include(a => a.ScoreTypeLang)
                        .SingleOrDefaultAsync();
        }

        public new void Create(ScoreType entity)
        {
            entity.ScoreTypeLang ??= new ScoreTypeLang
            {
                Name = entity.Name,
            };
            base.Create(entity);
        }
    }

    public static class ScoreTypeRepositoryExtension
    {
        public static IQueryable<ScoreType> Filter(this IQueryable<ScoreType> ScoreTypes, int id)
        {
            return ScoreTypes.Where(a => id == 0 || a.Id == id);
        }

    }
}
